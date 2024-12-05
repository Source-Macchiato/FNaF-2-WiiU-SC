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
        // Adding buttons to the main menu with corresponding actions
        //menuManager.AddButton(0, 0, NewGame, "mainmenu.newgame");
        //menuManager.AddButton(0, 0, Continue, "mainmenu.continue");
        //menuManager.AddButton(0, 0, Options, "mainmenu.options");
        //menuManager.AddButton(0, 0, Credits, "mainmenu.credits");

        //menuManager.AddButton("Language", Language, 1, "mainmenu.language");
        //menuManager.AddButton("Layout", Layout, 1, "mainmenu.layout");
        //menuManager.AddButton("Online", Online, 1, "mainmenu.online");

        //menuManager.AddButton("Analytic Data", Analytics, 4, "mainmenu.analyticdata");

        // Set back callbacks for specific menus
        menuManager.SetBackCallback(2, OnBackFromCredits);
        //menuManager.SetBackCallback(2, OnBackFromLanguage);

        // Display main menu after loaded all buttons
    }

    // Buttons functions
    public void NewGame()
    {
        //          !! Have to do : CanNavigate on MenuManager !!!
        // I know bbg, the system was almost finished so now it's done
        menuManager.canNavigate = false;

        // Reset night number and save it
        menuData.nightNumber = 0;
        menuData.SaveNightNumber();

        menuData.LoadAdvertisement();
    }

    public void Continue()
    {
        menuManager.canNavigate = false;

        if (menuData.nightNumber == 0) // Night is 1
        {
            menuData.LoadAdvertisement();
        }
        else if (menuData.nightNumber >= 1 && menuData.nightNumber <= 4) // Night is between 2 and 5
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

    public void Options()
    {
        menuManager.ChangeMenu(1);
    }

    void Language()
    {
        menuManager.ChangeMenu(3);
    }

    public void Credits()
    {
        menuManager.ChangeMenu(2);

        if (menuManager.GetCurrentMenu() != null)
        {
            Transform creditsChild = menuManager.GetCurrentMenu().transform.GetChild(0);
            menuManager.currentScrollRect = creditsChild.GetComponent<ScrollRect>();
        }
    }

    void Layout()
    {

    }

    void Online()
    {
        menuManager.ChangeMenu(4);
    }

    void Analytics()
    {

    }

    // Callback functions
    void OnBackFromLanguage()
    {
        
    }

    void OnBackFromCredits()
    {
        menuManager.currentScrollRect = null;
        menuData.ToggleGameTitle(true);
    }
}