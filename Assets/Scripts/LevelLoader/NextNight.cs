using System.Collections;
using UnityEngine;

public class NextNight : MonoBehaviour
{
    private int nightNumber;

    [SerializeField] private I18nTextTranslator nightTextTranslator;
    [SerializeField] private Animator nightAnimator;

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
        nightNumber = SaveManager.saveData.game.nightNumber;

        // Display current night text
        switch (nightNumber)
        {
            case 0:
                nightTextTranslator.textId = "nextnight.firstnight";
                break;
            case 1:
                nightTextTranslator.textId = "nextnight.secondnight";
                break;
            case 2:
                nightTextTranslator.textId = "nextnight.thirdnight";
                break;
            case 3:
                nightTextTranslator.textId = "nextnight.fourthnight";
                break;
            case 4:
                nightTextTranslator.textId = "nextnight.fifthnight";
                break;
            case 5:
                nightTextTranslator.textId = "nextnight.sixthnight";
                break;
            case 6:
                nightTextTranslator.textId = "nextnight.seventhnight";
                break;
            default:
                nightTextTranslator.textId = "nextnight.firstnight";
                break;
        }

        nightTextTranslator.UpdateText();

        // Analytics and the timer before load the next scene
        if (analyticsData != null)
        {
            StartCoroutine(analyticsData.UpdateAnalytics("current_night", analyticsData.GetCurrentNight()));
        }

        StartCoroutine(LoadOffice());
    }

    IEnumerator LoadOffice()
    {
        yield return new WaitForSeconds(3);

        nightAnimator.Play("FadeOut");

        yield return new WaitForSeconds(1);

        levelLoader.loadingScreen.SetActive(true);

        levelLoader.LoadLevel("Office");
    }
}
