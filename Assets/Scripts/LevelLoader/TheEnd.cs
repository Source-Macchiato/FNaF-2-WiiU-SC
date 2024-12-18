using System.Collections;
using UnityEngine;

public class TheEnd : MonoBehaviour
{
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
