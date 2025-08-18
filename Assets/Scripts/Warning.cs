using System.Collections;
using UnityEngine;

public class Warning : MonoBehaviour
{
    public Animator warningAnimator;

    private bool skipRequested = false;

    LevelLoader levelLoader;

	void Start()
	{
        // Get LevelLoader script
        levelLoader = FindObjectOfType<LevelLoader>();

        // Disable loading screen when the level starts
        levelLoader.loadingScreen.SetActive(false);

        StartCoroutine(InitCoroutine());
    }

    void Update()
    {
        if (Input.anyKeyDown && !skipRequested)
        {
            skipRequested = true;
        }
    }

    IEnumerator InitCoroutine()
    {
        // Wait 4 seconds or until skip is requested
        float elapsedTime = 0f;
        while (elapsedTime < 4f && !skipRequested)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Play fade out animation
        warningAnimator.Play("Fade Out");

        // Wait one second
        yield return new WaitForSeconds(1f);

        // Display loading screen
        levelLoader.loadingScreen.SetActive(true);

        // Request level to load
        if (!SaveManager.saveData.game.dreamIntroPlayed)
        {
            levelLoader.LoadLevel("Dream");
        }
        else
        {
            levelLoader.LoadLevel("MainMenu");
        }
    }
}