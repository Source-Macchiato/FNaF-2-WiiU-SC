﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Audio;
using RTLTMPro;

public class MenuData : MonoBehaviour
{
    public int nightNumber;
    public int layoutId;
    public GameObject gameTitle;
    public RTLTextMeshPro nightNumberText;
    public GameObject nightNumberContainer;
    public GameObject continueButtonGameObject;
    public GameObject starsContainer;
    public SwitcherData difficultySwitcher;
    public CardSwitcherData[] characterCardSwitchers;
    public GameObject[] customNightStars;

    // Advertisement
    public GameObject advertisementImage;
    public GameObject[] AnimEnabled;
    private bool advertisementIsActive;
    private float startTime;
    private float waitTime = 10f;

    [Header("Switchers")]
    public SwitcherData languageSwitcher;
    public SwitcherData analyticsSwitcher;
    public SwitcherData motionSwitcher;
    public SwitcherData pointerSwitcher;
    public SwitcherData generalVolumeSwitcher;
    public SwitcherData musicVolumeSwitcher;
    public SwitcherData voiceVolumeSwitcher;
    public SwitcherData sfxVolumeSwitcher;

    public AudioMixer audioMixer;

    // Scripts
    SaveGameState saveGameState;
    SaveManager saveManager;
    MenuManager menuManager;
    AnalyticsData analyticsData;

    void Start()
    {
        // Get scripts
        saveGameState = FindObjectOfType<SaveGameState>();
        saveManager = FindObjectOfType<SaveManager>();
        menuManager = FindObjectOfType<MenuManager>();
        analyticsData = FindObjectOfType<AnalyticsData>();

        // Load
        nightNumber = SaveManager.LoadNightNumber();
        layoutId = SaveManager.LoadLayoutId();
        LoadVolume();

        // Disable advertisement by default
        advertisementIsActive = false;
        advertisementImage.SetActive(false);

        SaveIntroDreamPlayed();
        ToggleDisplayStars(true);

        // System for display night number and prevent being out of range
        if (nightNumber >= 0 && nightNumber <= 4) // If is between night 1 and night 5
        {
            nightNumberText.text = (nightNumber + 1).ToString();
        }
        else if (nightNumber >= 5) // If is night 6 or more
        {
            nightNumberText.text = "5";
        }
        else // Or if is another value, in the case less than night 1
        {
            nightNumberText.text = "1";
        }
    }
	
	// Update is called once per frame
	void Update()
    {
        // Load scene after advertisementload is called
        if (Time.time - startTime >= waitTime && advertisementIsActive == true)
        {
            SceneManager.LoadScene("NextNight");
        }

        // Display night text only when continue button is selected
        nightNumberContainer.SetActive(EventSystem.current.currentSelectedGameObject == continueButtonGameObject);
    }

    public void ToggleDisplayStars(bool visibility)
    {
        starsContainer.SetActive(visibility);
    }

    public void LoadAdvertisement()
    {
        foreach (GameObject Animator in AnimEnabled)
        {
            Animator.GetComponent<Animator>().enabled = false;
        }
        advertisementIsActive = true;
        startTime = Time.time;
        advertisementImage.SetActive(true);
    }

    public void DisplaySelectedLayoutButton()
    {
        // Dictionary to map cardId to layoutId
        Dictionary<string, int> cardLayoutMapping = new Dictionary<string, int>();
        cardLayoutMapping.Add("card.tvonly", 0);
        cardLayoutMapping.Add("card.tvgamepadclassic", 1);
        cardLayoutMapping.Add("card.tvgamepadalternative", 2);
        cardLayoutMapping.Add("card.gamepadonly", 3);

        // Get all CardData scripts
        CardData[] cards = FindObjectsOfType<CardData>();

        foreach (CardData card in cards)
        {
            // Check if cardId matches layoutId
            if (cardLayoutMapping.ContainsKey(card.cardId) && cardLayoutMapping[card.cardId] == layoutId)
            {
                Button button = card.GetComponent<Button>();
                if (button != null)
                {
                    button.Select();
                }
            }
        }
    }

    public void SaveAndUpdateLanguage()
    {
        saveManager.SaveLanguage(languageSwitcher.optionsName[languageSwitcher.currentOptionId]);
        bool saveResult = saveGameState.DoSave();

        // Reload the language
        I18n.LoadLanguage();
    }

    public void SaveAndUpdateShareAnalytics()
    {
        // Get SwitcherData scripts
        saveManager.SaveShareAnalytics(analyticsSwitcher.currentOptionId == 1 ? 0 : 1);
        bool saveResult = saveGameState.DoSave();

        analyticsData.CanShareAnalytics();
    }

    public void SaveMotionControls()
    {
        saveManager.SaveMotionControls(motionSwitcher.currentOptionId == 0);
        bool saveResult = saveGameState.DoSave();
    }

    public void SavePointerVisibility()
    {
        saveManager.SavePointerVisibility(pointerSwitcher.currentOptionId == 0);
        bool saveResult = saveGameState.DoSave();
    }

    public void SaveNightNumber()
    {
        saveManager.SaveNightNumber(nightNumber);
        bool saveResult = saveGameState.DoSave();
    }

    public void ToggleGameTitle(bool visibility)
    {
        gameTitle.SetActive(visibility);
    }

    private void SaveIntroDreamPlayed()
    {
        if (SaveManager.LoadIntroDreamPlayed() == 0)
        {
            saveManager.SaveIntroDreamPlayed(1);
            bool saveResult = saveGameState.DoSave();
        }
    }

    public void LoadLanguageAndUpdateSwitcher()
    {
        // Get language
        string language = I18n.GetLanguage();

        // Find language index
        int languageIndex = System.Array.IndexOf(languageSwitcher.optionsName, language);

        if (languageIndex >= 0 && languageIndex < languageSwitcher.optionsName.Length)
        {
            languageSwitcher.currentOptionId = languageIndex;
            languageSwitcher.UpdateText();
        }
    }

    public void LoadShareAnalyticsAndUpdateSwitcher()
    {
        // Get share analytics
        int shareAnalytics = SaveManager.LoadShareAnalytics();

        int switcherIndex = shareAnalytics == 1 ? 0 : 1;

        if (switcherIndex >= 0 && switcherIndex < analyticsSwitcher.optionsName.Length)
        {
            analyticsSwitcher.currentOptionId = switcherIndex;
            analyticsSwitcher.UpdateText();
        }
    }

    public void LoadMotionControls()
    {
        // Get motion controls status
        bool motionControls = SaveManager.LoadMotionControls();

        int switcherIndex = motionControls ? 0 : 1;

        if (switcherIndex >= 0 && switcherIndex < motionSwitcher.optionsName.Length)
        {
            motionSwitcher.currentOptionId = switcherIndex;
            motionSwitcher.UpdateText();
        }
    }

    public void LoadPointerVisibility()
    {
        bool pointerVisibility = SaveManager.LoadPointerVisibility();

        int switcherIndex = pointerVisibility ? 0 : 1;

        if (switcherIndex >= 0 && switcherIndex < pointerSwitcher.optionsName.Length)
        {
            pointerSwitcher.currentOptionId = switcherIndex;
            pointerSwitcher.UpdateText();
        }
    }

    public void SelectLayoutButton()
    {
        // Dictionary to map cardId to layoutId
        Dictionary<string, int> cardLayoutMapping = new Dictionary<string, int>();
        cardLayoutMapping.Add("card.tvonly", 0);
        cardLayoutMapping.Add("card.tvgamepadclassic", 1);
        cardLayoutMapping.Add("card.tvgamepadalternative", 2);
        cardLayoutMapping.Add("card.gamepadonly", 3);

        string cardId = EventSystem.current.currentSelectedGameObject.GetComponent<CardData>().cardId;

        layoutId = cardLayoutMapping[cardId];

        // Save layout id
        saveManager.SaveLayoutId(layoutId);
        bool saveResult = saveGameState.DoSave();
    }

    public void SaveDifficulties()
    {
        foreach (CardSwitcherData cardSwitcher in characterCardSwitchers)
        {
            if (cardSwitcher.id == "customnight.freddy")
            {
                PlayerPrefs.SetInt("FreddyAI", cardSwitcher.difficultyValue);
            }
            else if (cardSwitcher.id == "customnight.bonnie")
            {
                PlayerPrefs.SetInt("BonnieAI", cardSwitcher.difficultyValue);
            }
            else if (cardSwitcher.id == "customnight.chica")
            {
                PlayerPrefs.SetInt("ChicaAI", cardSwitcher.difficultyValue);
            }
            else if (cardSwitcher.id == "customnight.foxy")
            {
                PlayerPrefs.SetInt("FoxyAI", cardSwitcher.difficultyValue);
            }
            else if (cardSwitcher.id == "customnight.balloonboy")
            {
                PlayerPrefs.SetInt("BalloonBoyAI", cardSwitcher.difficultyValue);
            }
            else if (cardSwitcher.id == "customnight.toyfreddy")
            {
                PlayerPrefs.SetInt("ToyFreddyAI", cardSwitcher.difficultyValue);
            }
            else if (cardSwitcher.id == "customnight.toybonnie")
            {
                PlayerPrefs.SetInt("ToyBonnieAI", cardSwitcher.difficultyValue);
            }
            else if (cardSwitcher.id == "customnight.toychica")
            {
                PlayerPrefs.SetInt("ToyChicaAI", cardSwitcher.difficultyValue);
            }
            else if (cardSwitcher.id == "customnight.mangle")
            {
                PlayerPrefs.SetInt("MangleAI", cardSwitcher.difficultyValue);
            }
            else if (cardSwitcher.id == "customnight.goldenfreddy")
            {
                PlayerPrefs.SetInt("GoldenFreddyAI", cardSwitcher.difficultyValue);
            }

            PlayerPrefs.Save();
        }
    }

    public void DifficultyConfigs()
    {
        if (difficultySwitcher.currentOptionId == 0)
        {
            ChangeDifficulty(20, 20, 20, 20, 0, 0, 0, 0, 0, 0);
        }
        else if (difficultySwitcher.currentOptionId == 1)
        {
            ChangeDifficulty(0, 0, 0, 0, 10, 10, 10, 10, 10, 0);
        }
        else if (difficultySwitcher.currentOptionId == 2)
        {
            ChangeDifficulty(0, 20, 0, 5, 0, 0, 20, 0, 0, 0);
        }
        else if (difficultySwitcher.currentOptionId == 3)
        {
            ChangeDifficulty(0, 0, 0, 0, 20, 0, 0, 0, 20, 10);
        }
        else if (difficultySwitcher.currentOptionId == 4)
        {
            ChangeDifficulty(0, 0, 0, 20, 0, 0, 0, 0, 20, 0);
        }
        else if (difficultySwitcher.currentOptionId == 5)
        {
            ChangeDifficulty(0, 0, 20, 0, 0, 0, 0, 20, 20, 0);
        }
        else if (difficultySwitcher.currentOptionId == 6)
        {
            ChangeDifficulty(20, 0, 0, 10, 10, 20, 0, 0, 0, 10);
        }
        else if (difficultySwitcher.currentOptionId == 7)
        {
            ChangeDifficulty(5, 5, 5, 5, 5, 5, 5, 5, 5, 5);
        }
        else if (difficultySwitcher.currentOptionId == 8)
        {
            ChangeDifficulty(10, 10, 10, 10, 10, 10, 10, 10, 10, 10);
        }
        else if (difficultySwitcher.currentOptionId == 9)
        {
            ChangeDifficulty(20, 20, 20, 20, 20, 20, 20, 20, 20, 20);
        }

        DisplayCustomNightStars(difficultySwitcher.currentOptionId);
    }

    private void ChangeDifficulty(int freddy, int bonnie, int chica, int foxy, int bb, int toyFreddy, int toyBonnie, int toyChica, int mangle, int goldenFreddy)
    {
        foreach (CardSwitcherData cardSwitcher in characterCardSwitchers)
        {
            if (cardSwitcher.id == "customnight.freddy")
            {
                cardSwitcher.difficultyValue = freddy;

                cardSwitcher.UpdateCardSwitcher();
            }
            else if (cardSwitcher.id == "customnight.bonnie")
            {
                cardSwitcher.difficultyValue = bonnie;

                cardSwitcher.UpdateCardSwitcher();
            }
            else if (cardSwitcher.id == "customnight.chica")
            {
                cardSwitcher.difficultyValue = chica;

                cardSwitcher.UpdateCardSwitcher();
            }
            else if (cardSwitcher.id == "customnight.foxy")
            {
                cardSwitcher.difficultyValue = foxy;

                cardSwitcher.UpdateCardSwitcher();
            }
            else if (cardSwitcher.id == "customnight.balloonboy")
            {
                cardSwitcher.difficultyValue = bb;

                cardSwitcher.UpdateCardSwitcher();
            }
            else if (cardSwitcher.id == "customnight.toyfreddy")
            {
                cardSwitcher.difficultyValue = toyFreddy;

                cardSwitcher.UpdateCardSwitcher();
            }
            else if (cardSwitcher.id == "customnight.toybonnie")
            {
                cardSwitcher.difficultyValue = toyBonnie;

                cardSwitcher.UpdateCardSwitcher();
            }
            else if (cardSwitcher.id == "customnight.toychica")
            {
                cardSwitcher.difficultyValue = toyChica;

                cardSwitcher.UpdateCardSwitcher();
            }
            else if (cardSwitcher.id == "customnight.mangle")
            {
                cardSwitcher.difficultyValue = mangle;

                cardSwitcher.UpdateCardSwitcher();
            }
            else if (cardSwitcher.id == "customnight.goldenfreddy")
            {
                cardSwitcher.difficultyValue = goldenFreddy;

                cardSwitcher.UpdateCardSwitcher();
            }
        }
    }

    private void DisplayCustomNightStars(int modeId)
    {
        foreach (GameObject star in customNightStars)
        {
            star.SetActive(SaveManager.LoadDoneMode(modeId));
        }
    }

    public void UpdateAnalyticsLanguage()
    {
        StartCoroutine(analyticsData.UpdateAnalytics("language", analyticsData.GetLanguage()));
    }

    public void UpdateAnalyticsLayout()
    {
        StartCoroutine(analyticsData.UpdateAnalytics("layout", analyticsData.GetLayout()));
    }

    private void LoadVolume()
    {
        audioMixer.SetFloat("Master", ConvertToDecibel(SaveManager.LoadGeneralVolume()));
        audioMixer.SetFloat("Music", ConvertToDecibel(SaveManager.LoadMusicVolume()));
        audioMixer.SetFloat("Voice", ConvertToDecibel(SaveManager.LoadVoiceVolume()));
        audioMixer.SetFloat("SFX", ConvertToDecibel(SaveManager.LoadSFXVolume()));

    }

    public void UpdateVolumeSwitchers()
    {
        generalVolumeSwitcher.currentOptionId = SaveManager.LoadGeneralVolume();
        musicVolumeSwitcher.currentOptionId = SaveManager.LoadMusicVolume();
        voiceVolumeSwitcher.currentOptionId = SaveManager.LoadVoiceVolume();
        sfxVolumeSwitcher.currentOptionId = SaveManager.LoadSFXVolume();
    }

    public void SaveAndUpdateVolume()
    {
        // Save and apply general volume
        saveManager.SaveGeneralVolume(generalVolumeSwitcher.currentOptionId);
        audioMixer.SetFloat("Master", ConvertToDecibel(generalVolumeSwitcher.currentOptionId));

        // Save and apply music volume
        saveManager.SaveMusicVolume(musicVolumeSwitcher.currentOptionId);
        audioMixer.SetFloat("Music", ConvertToDecibel(musicVolumeSwitcher.currentOptionId));

        // Save and apply voice volume
        saveManager.SaveVoiceVolume(voiceVolumeSwitcher.currentOptionId);
        audioMixer.SetFloat("Voice", ConvertToDecibel(voiceVolumeSwitcher.currentOptionId));

        // Save and apply SFX volume
        saveManager.SaveSFXVolume(sfxVolumeSwitcher.currentOptionId);
        audioMixer.SetFloat("SFX", ConvertToDecibel(sfxVolumeSwitcher.currentOptionId));

        bool saveResult = saveGameState.DoSave();
    }

    private float ConvertToDecibel(int volume)
    {
        // Convert volume (0-10) to decibels (-80dB to 0dB)
        return Mathf.Log10(Mathf.Max(volume / 10f, 0.0001f)) * 20f;
    }
}