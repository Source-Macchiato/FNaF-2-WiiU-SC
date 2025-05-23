﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuSetup : MonoBehaviour
{
    // Reference to the main and sub menus
    public MenuManager menuManager;
    public MenuData menuData;

    void Start()
    {
        // Set back callbacks for specific menus
        menuManager.SetBackCallback(1, OnBackFromOptions);
        menuManager.SetBackCallback(2, OnBackFromCredits);
        menuManager.SetBackCallback(3, OnBackFromLanguage);
        menuManager.SetBackCallback(4, OnBackFromLayout);
        menuManager.SetBackCallback(6, OnBackFromAnalytics);
        menuManager.SetBackCallback(7, OnBackFromControls);
        menuManager.SetBackCallback(9, OnBackFromVolume);
    }

    // Buttons functions
    public void NewGame()
    {
        menuManager.canNavigate = false;

        // Reset night number and save it
        menuData.nightNumber = 0;
        menuData.SaveNightNumber();

        menuData.LoadAdvertisement();
    }

    public void Continue()
    {
        menuManager.canNavigate = false;

        if (menuData.nightNumber >= 0 && menuData.nightNumber <= 4) // Night is between 1 and 5
        {
            SceneManager.LoadScene("NextNight");
        }
        else if (menuData.nightNumber >= 5)
        {
            // Reset night number to 4 and save it
            menuData.nightNumber = 4;
            menuData.SaveNightNumber();

            SceneManager.LoadScene("NextNight");
        }
    }

    public void SixthNight()
    {
        menuManager.canNavigate = false;

        menuData.nightNumber = 5;
        menuData.SaveNightNumber();

        SceneManager.LoadScene("NextNight");
    }

    public void Options()
    {
        menuManager.ChangeMenu(1);

        menuData.ToggleDisplayStars(false);
    }

    public void Language()
    {
        menuManager.ChangeMenu(3);

        menuData.LoadLanguageAndUpdateSwitcher();
    }

    public void Credits()
    {
        menuManager.ChangeMenu(2);

        menuData.ToggleDisplayStars(false);
    }
    
    public void Layout()
    {
        menuManager.ChangeMenu(4);

        menuData.DisplaySelectedLayoutButton();
    }

    public void StartCustomNight()
    {
        menuData.SaveDifficulties();

        menuManager.canNavigate = false;

        menuData.nightNumber = 6;
        menuData.SaveNightNumber();

        SceneManager.LoadScene("NextNight");
    }

    public void Analytics()
    {
        menuManager.ChangeMenu(6);

        menuData.LoadShareAnalyticsAndUpdateSwitcher();
    }

    public void Controls()
    {
        menuManager.ChangeMenu(7);

        menuData.LoadMotionControls();
        menuData.LoadPointerVisibility();
    }

    public void Volume()
    {
        menuManager.ChangeMenu(9);

        menuData.UpdateVolumeSwitchers();
    }

    // Callback functions
    void OnBackFromOptions()
    {
        menuData.ToggleDisplayStars(true);
    }

    void OnBackFromLanguage()
    {
        menuData.SaveAndUpdateLanguage();

        menuData.UpdateAnalyticsLanguage();
    }

    void OnBackFromCredits()
    {
        menuData.ToggleGameTitle(true);
        menuData.ToggleDisplayStars(true);
    }

    void OnBackFromLayout()
    {
        menuData.UpdateAnalyticsLayout();
    }

    void OnBackFromAnalytics()
    {
        menuData.SaveAndUpdateShareAnalytics();
    }

    void OnBackFromControls()
    {
        menuData.SaveMotionControls();
        menuData.SavePointerVisibility();
    }

    void OnBackFromVolume()
    {
        menuData.SaveAndUpdateVolume();
    }
}