using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TheEnd : MonoBehaviour
{
	public Image endingScreen;
	public Sprite[] endingSprites;

	private int nightNumber;

	// Scripts
	LevelLoader levelLoader;

	void Start()
	{
		// Get scripts
		levelLoader = FindObjectOfType<LevelLoader>();

		// Disable loading screen when the game starts
		levelLoader.loadingScreen.SetActive(false);

		// Get night number
		nightNumber = SaveManager.LoadNightNumber();

		// Display right image and save stars
		if (nightNumber == 4) // If night 5
		{
			endingScreen.sprite = endingSprites[0];
		}
		else if (nightNumber == 5) // If night 6
		{
			endingScreen.sprite = endingSprites[1];
		}
		else if (nightNumber == 6) // If night 7 / Custom Night
		{
			endingScreen.sprite = endingSprites[2];
		}

		// Load next scene
		StartCoroutine(LoadNextScene());
	}
	
	private IEnumerator LoadNextScene()
	{
		yield return new WaitForSeconds(20f);

		// Enable loading screen
		levelLoader.loadingScreen.SetActive(true);

		// Load next scene
		levelLoader.LoadLevel("MainMenu");
	}
}
