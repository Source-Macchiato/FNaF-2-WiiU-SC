using System.Collections;
using UnityEngine;

public class SixAM : MonoBehaviour
{
	public Animator fadeAnimator;
	public AudioSource happyChildrenAudio;

	private int nightNumber;

	// Scripts
	SaveGameState saveGameState;
	SaveManager saveManager;
    LevelLoader levelLoader;

    void Start()
	{
		// Get scripts
		saveGameState = FindObjectOfType<SaveGameState>();
		saveManager = FindObjectOfType<SaveManager>();
		levelLoader = FindObjectOfType<LevelLoader>();

		// Disable loading screen when the game starts
		levelLoader.loadingScreen.SetActive(false);

		// Get current night number
		nightNumber = SaveManager.LoadNightNumber();

		// Increase night number and save it
        nightNumber++;

        saveManager.SaveNightNumber(nightNumber);
        bool saveResult = saveGameState.DoSave();

        StartCoroutine(LoadNextScene());
	}

	private IEnumerator LoadNextScene()
	{
		yield return new WaitForSeconds(4f);

		happyChildrenAudio.Play();

		yield return new WaitForSeconds(5f);

		fadeAnimator.Play("Fade Out");

		yield return new WaitForSeconds(1f);

		// Enable loading screen
		levelLoader.loadingScreen.SetActive(true);

		if (nightNumber == 1) // If it's the first night (but 6AM scene is never called for the first night, it's just in case)
		{
			levelLoader.LoadLevel("NextNight");
		}
		else if (nightNumber >= 2 && nightNumber <= 4) // If it's between second night and fourth night
		{
			levelLoader.LoadLevel("Dream");
		}
		else if (nightNumber >= 5 && nightNumber <= 7) // If it's between fifth night and seventh night
		{
			levelLoader.LoadLevel("TheEnd");
		}
	}
}
