using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Warning : MonoBehaviour
{
    public GameObject loadingScreen;

    LevelLoader levelLoader;

	void Start()
	{
        // Get LevelLoader script
        levelLoader = FindObjectOfType<LevelLoader>();

        // Disable loading screen when the level starts
        loadingScreen.SetActive(false);

        StartCoroutine(InitCoroutine());
    }

    IEnumerator InitCoroutine()
    {
        yield return new WaitForSeconds(5);

        loadingScreen.SetActive(true);

        if (SaveManager.LoadIntroDreamPlayed() == 0)
        {
            levelLoader.LoadLevel("Dream");
        }
        else
        {
            levelLoader.LoadLevel("MainMenu");
        }
    }
}
