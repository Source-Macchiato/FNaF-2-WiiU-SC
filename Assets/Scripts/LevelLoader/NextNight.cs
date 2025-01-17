using System.Collections;
using UnityEngine;

public class NextNight : MonoBehaviour
{
    private int nightNumber;

    public GameObject[] nightDisplayers;

    AnalyticsData analyticsData;
    LevelLoader levelLoader;

    void Start()
    {
        // Get scripts
        analyticsData = FindObjectOfType<AnalyticsData>();
        levelLoader = FindObjectOfType<LevelLoader>();

        // Disable loading screen when the level starts
        levelLoader.loadingScreen.SetActive(false);

        // Get night number
        nightNumber = SaveManager.LoadNightNumber();

        // Display current night displayer and hide others
        for (int i = 0; i < nightDisplayers.Length; i++)
        {
            nightDisplayers[i].SetActive(i == nightNumber);
        }

        // Analytics and the timer before load the next scene
        StartCoroutine(analyticsData.UpdateAnalytics("current_night", analyticsData.GetCurrentNight()));
        StartCoroutine(LoadOffice());
    }

    IEnumerator LoadOffice()
    {
        yield return new WaitForSeconds(5);

        levelLoader.loadingScreen.SetActive(true);

        levelLoader.LoadLevel("Office");
    }
}
