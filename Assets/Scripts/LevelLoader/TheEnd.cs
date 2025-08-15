using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TheEnd : MonoBehaviour
{
	public AudioSource musicBoxAudio;
	public Image endingScreen;
	public Sprite[] endingSprites;

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

		// Disable loading screen when the scene starts
		levelLoader.loadingScreen.SetActive(false);

		// Get night number
		nightNumber = SaveManager.saveData.game.nightNumber;

		// Display right image and save stars
		if (nightNumber == 5) // If night 5
		{
			endingScreen.sprite = endingSprites[0];

			saveManager.SaveUnlockedStars(0, true);
		}
		else if (nightNumber == 6) // If night 6
		{
			endingScreen.sprite = endingSprites[1];

			saveManager.SaveUnlockedStars(1, true);
		}
		else if (nightNumber == 7) // If night 7 / Custom Night
		{
			endingScreen.sprite = endingSprites[2];

			saveManager.SaveUnlockedStars(2, true);
		}

		// Load next scene
		StartCoroutine(LoadNextScene());
	}
	
	private IEnumerator LoadNextScene()
	{
		yield return new WaitForSeconds(19f);

		// Stop music box
		musicBoxAudio.Stop();

		// Enable loading screen
		levelLoader.loadingScreen.SetActive(true);

		// Load next scene
		levelLoader.LoadLevel("MainMenu");
	}
}
