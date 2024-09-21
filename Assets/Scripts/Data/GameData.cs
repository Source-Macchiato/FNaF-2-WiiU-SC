using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameData : MonoBehaviour
{
    public float NightNumber;
    public Text currentLanguageText;

    // Scripts
    SaveGameState saveGameState;
    SaveManager saveManager;

    // Advertisement
    public GameObject advertisementImage;
    private bool advertisementIsActive;
    private float startTime;
    private float waitTime = 10f;

    public bool search;

    void Start()
    {
        // Get scripts
        saveGameState = FindObjectOfType<SaveGameState>();
        saveManager = FindObjectOfType<SaveManager>();

        // Load night number from save and display it
        NightNumber = SaveManager.LoadNightNumber();

        // Disable advertisement by default
        advertisementIsActive = false;
        advertisementImage.SetActive(false);
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
        advertisementIsActive = true;
        startTime = Time.time;
        advertisementImage.SetActive(true);
    }

    public void SaveAndUpdateLanguage()
    {
        saveManager.SaveLanguage(currentLanguageText.text);
        bool saveResult = saveGameState.DoSave();

        // Reload the language
        I18n.LoadLanguage();
    }
}