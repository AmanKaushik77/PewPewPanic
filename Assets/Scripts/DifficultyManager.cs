using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyManager : MonoBehaviour
{
    public Slider difficultySlider;
    public TMP_Text difficultyText;

    // Static variable to store difficulty (1 = Easy, 2 = Medium, 3 = Hard)
    public static int CurrentDifficulty { get; private set; } = 1;

    void Start()
    {
        // Load saved difficulty or default to Easy (1)
        CurrentDifficulty = PlayerPrefs.GetInt("Difficulty", 1);
        difficultySlider.value = CurrentDifficulty;

        // Add listener and update initial text
        difficultySlider.onValueChanged.AddListener(UpdateDifficulty);
        UpdateDifficultyText(CurrentDifficulty);
    }

    void UpdateDifficulty(float value)
    {
        CurrentDifficulty = Mathf.RoundToInt(value);
        PlayerPrefs.SetInt("Difficulty", CurrentDifficulty);
        UpdateDifficultyText(CurrentDifficulty);
    }

    void UpdateDifficultyText(float value)
    {
        switch (Mathf.RoundToInt(value))
        {
            case 1:
                difficultyText.text = "Easy";
                break;
            case 2:
                difficultyText.text = "Medium";
                break;
            case 3:
                difficultyText.text = "Hard";
                break;
            default:
                difficultyText.text = "Easy";
                break;
        }
    }
}