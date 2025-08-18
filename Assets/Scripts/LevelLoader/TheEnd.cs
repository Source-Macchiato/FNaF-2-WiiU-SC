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
	LevelLoader levelLoader;

	void Start()
	{
		// Get scripts
		levelLoader = FindObjectOfType<LevelLoader>();

		// Disable loading screen when the scene starts
		levelLoader.loadingScreen.SetActive(false);

		// Get night number
		nightNumber = SaveManager.saveData.game.nightNumber;

		// Display right image and save stars
		if (nightNumber == 5) // If night 5
		{
			endingScreen.sprite = endingSprites[0];

			SaveManager.saveData.game.ChangeUnlockedStarStatus(0, true);
			SaveManager.Save();
		}
		else if (nightNumber == 6) // If night 6
		{
			endingScreen.sprite = endingSprites[1];

			SaveManager.saveData.game.ChangeUnlockedStarStatus(1, true);
			SaveManager.Save();
		}
		else if (nightNumber == 7) // If night 7 / Custom Night
		{
			endingScreen.sprite = endingSprites[2];

			SaveManager.saveData.game.ChangeUnlockedStarStatus(2, true);
			SaveManager.Save();
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
