using System.Collections;
using UnityEngine;

public class NextNight : MonoBehaviour
{
    private int nightNumber;

    public GameObject[] nightDisplayers;
    public GameObject loadingScreen;

    LevelLoader levelLoader;

    void Start()
    {
        // Get LevelLoader script
        levelLoader = FindObjectOfType<LevelLoader>();

        // Disable loading screen when the level starts
        loadingScreen.SetActive(false);

        // Get night number
        nightNumber = SaveManager.LoadNightNumber();

        // Display current night displayer and hide others
        for (int i = 0; i < nightDisplayers.Length; i++)
        {
            nightDisplayers[i].SetActive(i == nightNumber);
        }

        // The timer before load the next scene
        StartCoroutine(InitCoroutine());
    }

    IEnumerator InitCoroutine()
    {
        yield return new WaitForSeconds(5);

        loadingScreen.SetActive(true);

        levelLoader.LoadLevel("Office");
    }
}
