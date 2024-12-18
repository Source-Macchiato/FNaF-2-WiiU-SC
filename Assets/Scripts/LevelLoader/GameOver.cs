using System.Collections;
using UnityEngine;

public class GameOver : MonoBehaviour
{
	public AudioSource stareAudio;

	// Scripts
	LevelLoader levelLoader;

	void Start()
	{
		// Get scripts
		levelLoader = FindObjectOfType<LevelLoader>();

		// Disable loading screen when the scene starts
		levelLoader.loadingScreen.SetActive(false);

		StartCoroutine(LoadNextScene());
    }

	void Update()
	{

	}

	private IEnumerator LoadNextScene()
	{
		yield return new WaitForSeconds(5f);

		stareAudio.Stop();

		yield return new WaitForSeconds(10f);

		levelLoader.loadingScreen.SetActive(true);

		levelLoader.LoadLevel("MainMenu");
	}
}
