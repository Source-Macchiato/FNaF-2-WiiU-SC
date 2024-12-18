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

	private IEnumerator LoadNextScene()
	{
		yield return new WaitForSeconds(6f);

		stareAudio.Stop();
	}
}
