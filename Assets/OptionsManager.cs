using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Button muteButton;
    [SerializeField] private Sprite unmutedSprite;
    [SerializeField] private Sprite mutedSprite;

    private AudioManager audioManager;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found in OptionsManager!");
            return;
        }

        if (volumeSlider != null)
        {
            volumeSlider.onValueChanged.AddListener(SetVolume);
            volumeSlider.value = audioManager.GetMasterVolume() * 10f; // Load current volume (0-10)
        }

        if (muteButton != null)
        {
            muteButton.onClick.AddListener(ToggleMute);
            // Set initial sprite and state
            bool isMuted = audioManager.IsMuted();
            muteButton.image.sprite = isMuted ? mutedSprite : unmutedSprite;
        }
    }

    void SetVolume(float volume)
    {
        audioManager.SetMasterVolume(volume / 10f);
    }

    void ToggleMute()
    {
        audioManager.ToggleMute();
        bool isMuted = audioManager.IsMuted();
        if (muteButton != null && unmutedSprite != null && mutedSprite != null)
        {
            muteButton.image.sprite = isMuted ? mutedSprite : unmutedSprite;
        }
    }
}