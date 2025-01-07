using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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

    // Scripts
    SaveGameState saveGameState;
    SaveManager saveManager;
    MenuManager menuManager;
    CardSwitcherData[] cardSwitchers;

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
        menuManager = FindObjectOfType<MenuManager>();
        cardSwitchers = FindObjectsOfType<CardSwitcherData>();

        // Load
        nightNumber = SaveManager.LoadNightNumber();
        layoutId = SaveManager.LoadLayoutId();

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

        // Display night text only when continue button is selected
        nightNumberContainer.SetActive(EventSystem.current.currentSelectedGameObject == continueButtonGameObject);

        // Display stars only if current menu is 0
        starsContainer.SetActive(menuManager.currentMenuId == 0);
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
        foreach (CardSwitcherData cardSwitcher in cardSwitchers)
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
}