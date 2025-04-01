using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyManager : MonoBehaviour
{
    public Slider difficultySlider;
    public TMP_Text difficultyText;

    public static int CurrentDifficulty { get; private set; } = 1;

    void Start()
    {
        CurrentDifficulty = PlayerPrefs.GetInt("Difficulty", 1);
        difficultySlider.value = CurrentDifficulty;

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