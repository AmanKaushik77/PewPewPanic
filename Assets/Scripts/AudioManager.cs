using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource shootAudioSource;
    [SerializeField] private AudioClip bulletSound;
    [SerializeField] private AudioClip laserSound;
    [SerializeField] private AudioSource powerUpAudioSource;
    [SerializeField] private AudioClip powerUpSound;

    private GameManager gameManager;
    private float masterVolume = 1f;
    private bool isMuted = false;
    private static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        if (shootAudioSource == null)
        {
            shootAudioSource = GetComponent<AudioSource>();
            if (shootAudioSource == null)
            {
                Debug.LogError("No shoot AudioSource found on " + gameObject.name + "!");
            }
        }

        if (powerUpAudioSource == null)
        {
            Debug.LogWarning("PowerUp AudioSource not assigned in Inspector. Attempting to find one.");
            powerUpAudioSource = GetComponent<AudioSource>();
            if (powerUpAudioSource == null)
            {
                Debug.LogError("No power-up AudioSource found on " + gameObject.name + "!");
            }
        }

        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in AudioManager!");
        }

        // Load saved settings
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        isMuted = PlayerPrefs.GetInt("IsMuted", 0) == 1;
        UpdateVolume();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.timeScale > 0f)
        {
            PlayShootSound();
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded: " + scene.name + ". Updating volume.");
        UpdateVolume(); // Ensure volume is applied to all AudioSources in the new scene
    }

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
        }
    }

    public void PlayPowerUpSound()
    {
        if (powerUpAudioSource != null && powerUpSound != null)
        {
            Debug.Log("Playing power-up sound from AudioManager at " + Time.time);
            powerUpAudioSource.PlayOneShot(powerUpSound);
        }
    }

    public void SetMasterVolume(float volume)
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateVolume();
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.Save();
        Debug.Log("Master volume set to: " + masterVolume);
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        UpdateVolume();
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log("Audio " + (isMuted ? "muted" : "unmuted"));
    }

    public bool IsMuted()
    {
        return isMuted;
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }

    private void UpdateVolume()
    {
        float effectiveVolume = isMuted ? 0f : masterVolume;
        if (shootAudioSource != null)
        {
            shootAudioSource.volume = effectiveVolume;
            Debug.Log("Shoot AudioSource volume set to: " + effectiveVolume);
        }
        if (powerUpAudioSource != null)
        {
            powerUpAudioSource.volume = effectiveVolume;
            Debug.Log("PowerUp AudioSource volume set to: " + effectiveVolume);
        }
        // Update background AudioSources
        AudioSource[] allSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource source in allSources)
        {
            if (source != shootAudioSource && source != powerUpAudioSource)
            {
                if (source.gameObject.CompareTag("BackgroundAudio") || source.gameObject.name == "UI")
                {
                    source.volume = effectiveVolume;
                    Debug.Log("Background AudioSource on " + source.gameObject.name + " volume set to: " + effectiveVolume);
                }
            }
        }
    }
}