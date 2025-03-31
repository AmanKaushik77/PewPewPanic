using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScoreCounterUI scoreCounterUI;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI waveText;

    [Header("Power-Up Settings")]
    public float powerUpDuration = 10f;
    public float speedBuffMultiplier = 1.5f;
    public GameObject laserPrefab;

    [Header("Game Over Settings")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    private int currentScore = 0;
    private int lives = 5;
    private int currentWave = 1;

    // Power-up states
    public bool HasLaser { get; private set; } = false;
    public float SpeedMultiplier { get; private set; } = 1f;

    public int CurrentWave { get { return currentWave; } }

    private void Start()
    {
        UpdateLivesUI();
        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave;
            Debug.Log("Initial WaveText: " + waveText.text);
        }
        else
        {
            Debug.LogWarning("WaveText UI element is not assigned in the inspector.");
        }
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
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
        if (scoreCounterUI != null)
            scoreCounterUI.UpdateScore(currentScore);
    }

    public void EnemyPassedBoundary()
    {
        lives--;
        UpdateLivesUI();
        if (lives <= 0)
        {
            Debug.Log("Game Over");
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

    private void GameOver()
    {
        Time.timeScale = 0f;
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        StartCoroutine(RestartGameAfterDelay());
    }

    private IEnumerator RestartGameAfterDelay()
    {
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
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
        yield return new WaitForSeconds(powerUpDuration);
        HasLaser = false;
        Debug.Log("Laser Power-Up Expired!");
    }

    private IEnumerator SpeedBuffRoutine()
    {
        SpeedMultiplier = speedBuffMultiplier;
        Debug.Log("Speed Buff Activated! Multiplier: " + SpeedMultiplier);
        yield return new WaitForSeconds(powerUpDuration);
        SpeedMultiplier = 1f;
        Debug.Log("Speed Buff Expired!");
    }
}
