﻿using UnityEngine;
using UnityEngine.UI;

public class MenuSetup : MonoBehaviour
{
    // Reference to the main and sub menus
    public MenuManager menuManager;
    public MenuData menuData;

    void Start()
    {
        // Adding buttons to the main menu with corresponding actions
        menuManager.AddButton(0, 0, NewGame, "mainmenu.newgame");
        menuManager.AddButton(0, 0, Continue, "mainmenu.continue");
        menuManager.AddButton(0, 0, Options, "mainmenu.options");
        menuManager.AddButton(0, 0, Credits, "mainmenu.credits");

        //menuManager.AddButton("Language", Language, 1, "mainmenu.language");
        //menuManager.AddButton("Layout", Layout, 1, "mainmenu.layout");
        //menuManager.AddButton("Online", Online, 1, "mainmenu.online");

        //menuManager.AddButton("Analytic Data", Analytics, 4, "mainmenu.analyticdata");

        // Set back callbacks for specific menus
        //menuManager.SetBackCallback(3, OnBackFromCredits);
        //menuManager.SetBackCallback(2, OnBackFromLanguage);

        // Display main menu after loaded all buttons
        menuManager.ChangeMenu(0);
    }

    // Buttons functions
    void NewGame()
    {
        //          !! Have to do : CanNavigate on MenuManager !!!
        // I know bbg, the system was almost finished so now it's done
        menuManager.canNavigate = false;

        menuData.NightNumber = 1;
        PlayerPrefs.SetFloat("NightNumber", menuData.NightNumber);
        PlayerPrefs.Save();
        menuData.LoadAdvertisement();
    }

    void Continue()
    {
        menuManager.canNavigate = false;
    }

    void Options()
    {
        menuManager.ChangeMenu(1);
    }

    void Language()
    {
        menuManager.ChangeMenu(2);
    }

    void Credits()
    {
        menuManager.ChangeMenu(3);

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
    }
}