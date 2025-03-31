using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource shootAudioSource;    // AudioSource for shooting sound
    [SerializeField] private AudioClip bulletSound;           // Sound for bullets
    [SerializeField] private AudioClip laserSound;           // Sound for lasers
    [SerializeField] private AudioSource powerUpAudioSource; // AudioSource for power-up sound
    [SerializeField] private AudioClip powerUpSound;         // Sound for power-up selection

    private GameManager gameManager;

    void Start()
    {
        // Ensure shoot AudioSource is assigned
        if (shootAudioSource == null)
        {
            shootAudioSource = GetComponent<AudioSource>();
            if (shootAudioSource == null)
            {
                Debug.LogError("No shoot AudioSource found on " + gameObject.name + "!");
            }
        }

        // Ensure power-up AudioSource is assigned
        if (powerUpAudioSource == null)
        {
            Debug.LogWarning("PowerUp AudioSource not assigned in Inspector. Attempting to find one.");
            powerUpAudioSource = GetComponent<AudioSource>(); // Will use shootAudioSource if only one exists
            if (powerUpAudioSource == null)
            {
                Debug.LogError("No power-up AudioSource found on " + gameObject.name + "!");
            }
        }

        // Get GameManager reference
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in AudioManager!");
        }
    }

    void Update()
    {
        // Play shoot sound once per "Fire1" click, but not when paused
        if (Input.GetButtonDown("Fire1") && Time.timeScale > 0f)
        {
            PlayShootSound();
        }
    }

    // Public method to play shoot sound based on bullet or laser
    public void PlayShootSound()
    {
        if (shootAudioSource != null)
        {
            AudioClip soundToPlay = (gameManager != null && gameManager.HasLaser) ? laserSound : bulletSound;
            if (soundToPlay != null)
            {
                Debug.Log("Playing " + (gameManager != null && gameManager.HasLaser ? "laser" : "bullet") + " sound from AudioManager at " + Time.time);
                shootAudioSource.PlayOneShot(soundToPlay);
            }
            else
            {
                Debug.LogWarning("No sound assigned for " + (gameManager != null && gameManager.HasLaser ? "laser" : "bullet") + " in AudioManager!");
            }
        }
    }

    // Public method to play power-up sound
    public void PlayPowerUpSound()
    {
        if (powerUpAudioSource != null && powerUpSound != null)
        {
            Debug.Log("Playing power-up sound from AudioManager at " + Time.time);
            powerUpAudioSource.PlayOneShot(powerUpSound);
        }
        else
        {
            Debug.LogWarning("PowerUp AudioSource or sound clip not assigned in AudioManager!");
        }
    }
}