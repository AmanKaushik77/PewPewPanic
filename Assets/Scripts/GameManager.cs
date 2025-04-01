using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("Power-Up Settings")]
    public float powerUpDuration = 10f; // Medium difficulty base value
    public float speedBuffMultiplier = 1.5f;
    public GameObject laserPrefab;

    [Header("Game Over Settings")]
    [SerializeField] private GameObject gameOverPanel;

    [Header("Complete Screen Settings")]
    [SerializeField] private GameObject completePanel;

    private int currentScore = 0;
    private int lives; // Set based on difficulty
    private int currentWave = 1;
    private float adjustedPowerUpDuration;

    public bool HasLaser { get; private set; } = false;
    public float SpeedMultiplier { get; private set; } = 1f;
    public int CurrentWave { get { return currentWave; } }

    private void Start()
    {
        AdjustDifficultySettings();
        Time.timeScale = 1f;
        UpdateLivesUI();
        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave;
            Debug.Log("Initial WaveText: " + waveText.text);
        }
        if (scoreText != null)
        {
            scoreText.text = "Score: 0";
            Debug.Log("Initial ScoreText: Score: 0");
        }
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (completePanel != null)
            completePanel.SetActive(false);
    }

    void AdjustDifficultySettings()
    {
        int difficulty = DifficultyManager.CurrentDifficulty;
        switch (difficulty)
        {
            case 1: // Easy
                lives = 4; 
                adjustedPowerUpDuration = powerUpDuration * 1.5f;
                break;
            case 2: // Medium
                lives = 3; 
                adjustedPowerUpDuration = powerUpDuration;
                break;
            case 3: // Hard
                lives = 2; 
                adjustedPowerUpDuration = powerUpDuration * 0.75f;
                break;
        }
        Debug.Log($"Difficulty set to {difficulty}: Lives: {lives}, PowerUp Duration: {adjustedPowerUpDuration}");
    }

    public void SetWave(int newWave)
    {
        StopAllCoroutines();
        StartCoroutine(WaveTransition(newWave));
    }

    private IEnumerator WaveTransition(int newWave)
    {
        if (waveText != null)
        {
            waveText.text = "Wave Cleared";
            Debug.Log("WaveText set to: Wave Cleared");
            Canvas.ForceUpdateCanvases();
        }
        yield return new WaitForSeconds(2f);

        currentWave = newWave;
        if (waveText != null)
        {
            if (newWave == 4)
            {
                waveText.text = "Boss Level";
                Debug.Log("WaveText updated to: Boss Level");
            }
            else
            {
                waveText.text = "Wave: " + currentWave;
                Debug.Log("WaveText updated to: " + waveText.text);
            }
        }
    }

    public void EnemyDestroyed(int wave)
    {
        int scoreToAdd = 0;
        switch (wave)
        {
            case 1: scoreToAdd = 100; break;
            case 2: scoreToAdd = 200; break;
            case 3: scoreToAdd = 300; break;
            case 4: scoreToAdd = 500; break;
            default:
                Debug.LogWarning("Invalid wave value: " + wave);
                break;
        }
        currentScore += scoreToAdd;
        Debug.Log("Score increased by " + scoreToAdd + ". New score: " + currentScore);
        if (scoreText != null)
            scoreText.text = "Score: " + currentScore;
    }

    public void EnemyPassedBoundary()
    {
        lives--;
        Debug.Log($"Enemy passed boundary! Lives remaining: {lives}");
        UpdateLivesUI();
        if (lives <= 0)
        {
            Debug.Log("Game Over triggered: No lives remaining");
            GameOver();
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Lives: " + lives;
        else
            Debug.LogWarning("LivesText UI element is not assigned in the inspector.");
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        else
            Debug.LogWarning("GameOverPanel is not assigned in the inspector.");
        StartCoroutine(RestartGameAfterDelay());
    }

    private IEnumerator RestartGameAfterDelay()
    {
        yield return new WaitForSecondsRealtime(2f);
        DestroyAllObjects();
        Time.timeScale = 1f;
        Debug.Log("Switching to MainMenu scene (Game Over).");
        SceneManager.LoadScene("MainMenu");
    }

    public void BossDestroyed()
    {
        Debug.Log("BossDestroyed() called");
        if (completePanel != null)
        {
            completePanel.SetActive(true);
            Debug.Log("CompletePanel activated");
        }
        else
        {
            Debug.LogWarning("CompletePanel is not assigned in the inspector.");
        }
        Time.timeScale = 0f;
        StartCoroutine(BossRestartAfterDelay());
    }

    private IEnumerator BossRestartAfterDelay()
    {
        yield return new WaitForSecondsRealtime(3f);
        DestroyAllObjects();
        Time.timeScale = 1f;
        Debug.Log("Switching to MainMenu scene (Boss Destroyed).");
        SceneManager.LoadScene("MainMenu");
    }

    private void DestroyAllObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj != gameObject)
                Destroy(obj);
        }
    }

    public void ActivateLaserPowerUp()
    {
        StartCoroutine(LaserPowerUpRoutine());
    }

    public void ActivateSpeedBuff()
    {
        StartCoroutine(SpeedBuffRoutine());
    }

    private IEnumerator LaserPowerUpRoutine()
    {
        HasLaser = true;
        Debug.Log("Laser Power-Up Activated!");
        yield return new WaitForSeconds(adjustedPowerUpDuration);
        HasLaser = false;
        Debug.Log("Laser Power-Up Expired!");
    }

    private IEnumerator SpeedBuffRoutine()
    {
        SpeedMultiplier = speedBuffMultiplier;
        Debug.Log("Speed Buff Activated! Multiplier: " + SpeedMultiplier);
        yield return new WaitForSeconds(adjustedPowerUpDuration);
        SpeedMultiplier = 1f;
        Debug.Log("Speed Buff Expired!");
    }
}