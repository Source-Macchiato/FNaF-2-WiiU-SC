using UnityEngine;
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
        menuManager.SetBackCallback(6, OnBackFromBrewConnect);
        menuManager.SetBackCallback(7, OnBackFromControls);
        menuManager.SetBackCallback(9, OnBackFromVolume);
    }

    // Buttons functions
    public void NewGame()
    {
        if (menuManager.canNavigate)
        {
            menuManager.canNavigate = false;

            // Reset night number and save it
            SaveManager.saveData.game.nightNumber = 0;
            SaveManager.Save();

            menuData.LoadAdvertisement();
        }
    }

    public void Continue()
    {
        if (menuManager.canNavigate)
        {
            menuManager.canNavigate = false;

            if (menuData.nightNumber >= 0 && menuData.nightNumber <= 4) // Night is between 1 and 5
            {
                SceneManager.LoadScene("NextNight");
            }
            else if (menuData.nightNumber >= 5)
            {
                // Reset night number to 4 and save it
                SaveManager.saveData.game.nightNumber = 4;
                SaveManager.Save();

                SceneManager.LoadSceneAsync("NextNight");
            }
        }
    }

    public void SixthNight()
    {
        if (menuManager.canNavigate)
        {
            menuManager.canNavigate = false;

            SaveManager.saveData.game.nightNumber = 5;
            SaveManager.Save();

            SceneManager.LoadSceneAsync("NextNight");
        }
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
        if (menuManager.canNavigate)
        {
            menuManager.canNavigate = false;

            menuData.SaveDifficulties();

            SaveManager.saveData.game.nightNumber = 6;
            SaveManager.Save();

            SceneManager.LoadSceneAsync("NextNight");
        }
    }

    public void BrewConnect()
    {
        menuManager.ChangeMenu(6);

        menuData.LoadAnalyticsAndUpdateSwitcher();
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

    public void Video()
    {
        menuManager.ChangeMenu(10);

        menuData.LoadPanoramaEffect();
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

    void OnBackFromBrewConnect()
    {
        menuData.SaveAndUpdateAnalytics();
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

    void OnBackFromVideo()
    {
        menuData.SavePanoramaEffect();
    }
}