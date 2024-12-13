using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuData : MonoBehaviour
{
    public int nightNumber;
    public GameObject gameTitle;

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