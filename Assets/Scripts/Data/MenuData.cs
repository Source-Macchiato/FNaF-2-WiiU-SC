using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuData : MonoBehaviour
{
    public float nightNumber;
    public Text currentLanguageText;
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
        saveManager.SaveLanguage(currentLanguageText.text);
        bool saveResult = saveGameState.DoSave();

        // Reload the language
        I18n.LoadLanguage();
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
}