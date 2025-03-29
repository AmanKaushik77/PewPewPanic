using System.Collections;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private ScoreCounterUI scoreCounterUI;  // Reference to the animated score UI
    [SerializeField] private TextMeshProUGUI livesText;        // UI element for displaying lives
    [SerializeField] private TextMeshProUGUI waveText;         // UI element for displaying the current wave

    private int currentScore = 0;
    private int lives = 5;
    private int currentWave = 1;

    // Expose the current wave for other scripts.
    public int CurrentWave { get { return currentWave; } }

    private void Start()
    {
        UpdateLivesUI();
        // When the game starts, simply display "Wave: 1"
        if (waveText != null)
        {
            waveText.text = "Wave: " + currentWave;
            Debug.Log("Initial WaveText: " + waveText.text);
        }
        else
        {
            Debug.LogWarning("WaveText UI element is not assigned in the inspector.");
        }
    }

    /// <summary>
    /// Call this method when a wave is cleared to update to the next wave.
    /// It immediately displays "Wave Cleared" for 2 seconds, then updates:
    /// if newWave equals 4, it shows "Boss Level"; otherwise, it shows "Wave: X".
    /// </summary>
    public void SetWave(int newWave)
    {
        StopAllCoroutines(); // Ensure no overlapping transitions.
        StartCoroutine(WaveTransition(newWave));
    }

    private IEnumerator WaveTransition(int newWave)
    {
        if (waveText != null)
        {
            waveText.text = "Wave Cleared";
            Debug.Log("WaveText set to: Wave Cleared");
            Canvas.ForceUpdateCanvases(); // Force UI update immediately.
        }
        else
        {
            Debug.LogWarning("WaveText UI element is not assigned in the inspector.");
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

    /// <summary>
    /// Called when an enemy is destroyed.
    /// For non-boss enemies, points are based on the current wave:
    ///   Wave 1 = 100 points, Wave 2 = 200 points, Wave 3 = 300 points.
    /// For bosses (isBoss == true), the score is 500 points.
    /// </summary>
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
        {
            scoreCounterUI.UpdateScore(currentScore);
        }
    }

    /// <summary>
    /// Called when an enemy passes z = -3.
    /// </summary>
    public void EnemyPassedBoundary()
    {
        lives--;
        UpdateLivesUI();
        if (lives <= 0)
        {
            Debug.Log("Game Over");
            // Insert game-over logic here.
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = "Lives: " + lives;
        else
            Debug.LogWarning("LivesText UI element is not assigned in the inspector.");
    }
}
