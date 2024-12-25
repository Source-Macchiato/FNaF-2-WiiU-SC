﻿using UnityEngine;
using UnityEngine.SceneManagement;
using RTLTMPro;

public class MenuData : MonoBehaviour
{
    public int nightNumber;
    public GameObject gameTitle;
    public RTLTextMeshPro nightNumberText;

    // Scripts
    SaveGameState saveGameState;
    SaveManager saveManager;

    // Advertisement
    public GameObject advertisementImage;
    public GameObject[] AnimEnabled;
    private bool advertisementIsActive;
    private float startTime;
    private float waitTime = 10f;

    void Start()
    {
        // Get scripts
        saveGameState = FindObjectOfType<SaveGameState>();
        saveManager = FindObjectOfType<SaveManager>();

        // Load
        nightNumber = SaveManager.LoadNightNumber();

        // Disable advertisement by default
        advertisementIsActive = false;
        advertisementImage.SetActive(false);

        SaveIntroDreamPlayed();

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

    public void SaveAndUpdateLanguage()
    {
        // Get SwitcherData scripts
        SwitcherData[] switchers = FindObjectsOfType<SwitcherData>();

        foreach (SwitcherData switcher in switchers)
        {
            if (switcher.switcherId == "switcher.translation")
            {
                saveManager.SaveLanguage(switcher.optionsName[switcher.currentOptionId]);
                bool saveResult = saveGameState.DoSave();

                // Reload the language
                I18n.LoadLanguage();
            }
        }
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
        // Get SwitcherData scripts
        SwitcherData[] switchers = FindObjectsOfType<SwitcherData>();

        // Get language
        string language = I18n.GetLanguage();

        foreach (SwitcherData switcher in switchers)
        {
            if (switcher.switcherId == "switcher.translation")
            {
                // Find language index
                int languageIndex = System.Array.IndexOf(switcher.optionsName, language);

                if (languageIndex >= 0 && languageIndex < switcher.optionsName.Length)
                {
                    switcher.currentOptionId = languageIndex;
                }
            }
        }
    }
}