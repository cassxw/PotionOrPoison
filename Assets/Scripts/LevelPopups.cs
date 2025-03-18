using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
using System.Collections.Generic;

public class LevelPopups : MonoBehaviour
{
    [SerializeField] private GameObject incidentReportPanel;
    [SerializeField] private GameObject potionBookPanel;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject helpMenuPanel;

    [SerializeField] private Text incidentReportText;
    [SerializeField] private PotionBookEntry[] potionEntries;

    [SerializeField] private Ingredient[] ingredients;

    [SerializeField] private Image day;
    [SerializeField] private Sprite[] days;

    [SerializeField] private AudioSource bulletinSound;

    public Timer timer;

    [SerializeField] private AudioSource bookSound;
    [SerializeField] private AudioClip bookClip; // Opening => first 0.5s
                                                 // Closing => from 0.7s

    // New audio-related fields
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider musicSlider;

    private const string SFX_VOLUME = "SFXVolume";
    private const string MUSIC_VOLUME = "MusicVolume";

    public CursorController cursorController;

    private void Awake()
    {
        InitialisePotionBook();
        InitialisePotions();
        InitialiseDay();
        InitialiseAudioSliders();
    }

    private void InitialisePotionBook()
    {
        if (potionEntries == null || potionEntries.Length == 0)
        {
            Debug.LogWarning("Potion entries not assigned in the inspector. Attempting to find them in the scene.");
            potionEntries = potionBookPanel.GetComponentsInChildren<PotionBookEntry>(true);
        }

        if (potionEntries == null || potionEntries.Length == 0)
        {
            Debug.LogError("No PotionBookEntry components found. Make sure they are properly set up in the scene.");
        }
    }

    private void InitialisePotions()
    {
        int level = GameSystem.Instance.GetLevel();

        if (level > 1)
        {
            ingredients[6].gameObject.SetActive(true);
            ingredients[7].gameObject.SetActive(true);
        
            if (level == 3)
            {
                ingredients[8].gameObject.SetActive(true);
                ingredients[9].gameObject.SetActive(true);
                ingredients[10].gameObject.SetActive(true);
            }
        }
    }

    private void InitialiseDay()
    {
        int level = GameSystem.Instance.GetLevel();
        day.sprite = days[level-1];
    }

    public void ShowIncidentReport()
    {
        cursorController.ChangeCursor("neutral");
        incidentReportPanel.SetActive(true);
        UpdateIncidentReport();
        bulletinSound.time = 0.6f;
        bulletinSound.Play();
    }

    public void HideIncidentReport()
    {
        cursorController.ChangeCursor("neutral");
        bulletinSound.time = 0.6f;
        bulletinSound.Play();
        incidentReportPanel.SetActive(false);
    }

    private void UpdateIncidentReport()
    {
        int currentLevel = GameSystem.Instance.CurrentLevel;
        string reportText = GetIncidentReportText(currentLevel);
        incidentReportText.text = reportText;
    }

    private string GetIncidentReportText(int level)
    {
        switch (level)
        {
            case 1:
                return "Rebels are trying to gather intel about the King's Coronation schedule and guestlist, as well as the palace's layout. Rumours are they are sneaking around and eavesdropping.";
            case 2:
                return "With their knowledge gained, rebels are now preparing the equipment needed for their attack at the King's Coronation, like setting traps.";
            case 3:
                return "Tonight is the King's Coronation - the day of the attack for the rebels! They will be looking for potions that will assist and buff them for combat, to help enact the King's assassination!";

            default:
                return "ERROR";
        }
    }

    public void ShowPotionBook()
    {
        cursorController.ChangeCursor("neutral");
        potionBookPanel.SetActive(true);
        UpdatePotionBook();
        PlayOpeningSound(); // Play opening sound
    }

    public void HidePotionBook()
    {
        cursorController.ChangeCursor("neutral");
        PlayClosingSound();  // Play closing sound
        potionBookPanel.SetActive(false);
    }

    public void UpdatePotionBook()
    {
        if (potionEntries == null || potionEntries.Length == 0)
        {
            Debug.LogError("Potion entries not initialised. Cannot update potion book.");
            return;
        }

        foreach (PotionBookEntry entry in potionEntries)
        {
            if (entry != null)
            {
                entry.UpdateKnownState();
            }
            else
            {
                Debug.LogWarning("Null PotionBookEntry found in potionEntries array.");
            }
        }
    }

    public void ShowPauseMenu()
    {
        cursorController.ChangeCursor("neutral");
        pauseMenuPanel.SetActive(true);
        timer.Pause();
        Time.timeScale = 0f; // Pause the game
    }

    private void InitialiseAudioSliders()
    {
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);

        // Set initial slider values (0 to 1 range)
        float sfxVolume, musicVolume;
        audioMixer.GetFloat(SFX_VOLUME, out sfxVolume);
        audioMixer.GetFloat(MUSIC_VOLUME, out musicVolume);

        sfxSlider.value = Mathf.Pow(10, sfxVolume / 20);
        musicSlider.value = Mathf.Pow(10, musicVolume / 20);
    }

    private void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat(SFX_VOLUME, Mathf.Log10(volume) * 20);
    }

    private void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(MUSIC_VOLUME, Mathf.Log10(volume) * 20);
    }

    public void ShowHelpMenu()
    {
        cursorController.ChangeCursor("neutral");
        helpMenuPanel.SetActive(true);
    }

    public void HideHelpMenu()
    {
        cursorController.ChangeCursor("neutral");
        helpMenuPanel.SetActive(false);
    }

    public void HidePauseMenu()
    {
        cursorController.ChangeCursor("neutral");
        pauseMenuPanel.SetActive(false);
        timer.Resume();
        Time.timeScale = 1f; // Resume the game
    }

    // Play the first 0.5 seconds of the book sound for opening
    private void PlayOpeningSound()
    {
        bookSound.clip = bookClip;
        bookSound.time = 0f; // Start at the beginning
        bookSound.Play();
        StartCoroutine(StopSoundAfterDuration(0.5f)); // Stop after 0.5 seconds
    }

    // Play from 0.7 seconds for the closing sound
    private void PlayClosingSound()
    {
        bookSound.clip = bookClip;
        bookSound.time = 0.7f; // Start at 0.7 seconds
        bookSound.Play();
        // No need to stop it manually as it can play to the end
    }

    // Coroutine to stop sound after a certain duration
    private IEnumerator StopSoundAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        bookSound.Stop();
    }
}