using System.Collections;
using UnityEngine;

public class SixAM : MonoBehaviour
{
	public Animator fadeAnimator;
	public AudioSource happyChildrenAudio;

	private int nightNumber;

	// AI levels
	private int freddyAI;
    private int bonnieAI;
    private int chicaAI;
	private int foxyAI;
	private int balloonBoyAI;
	private int toyFreddyAI;
	private int toyBonnieAI;
	private int toyChicaAI;
	private int mangleAI;
	private int goldenFreddyAI;

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

		if (nightNumber == 7)
		{
            // Get AI levels
            freddyAI = PlayerPrefs.GetInt("FreddyAI", 0);
            bonnieAI = PlayerPrefs.GetInt("BonnieAI", 0);
            chicaAI = PlayerPrefs.GetInt("ChicaAI", 0);
            foxyAI = PlayerPrefs.GetInt("FoxyAI", 0);
            balloonBoyAI = PlayerPrefs.GetInt("BalloonBoyAI", 0);
            toyFreddyAI = PlayerPrefs.GetInt("ToyFreddyAI", 0);
            toyBonnieAI = PlayerPrefs.GetInt("ToyBonnieAI", 0);
            toyChicaAI = PlayerPrefs.GetInt("ToyChicaAI", 0);
            mangleAI = PlayerPrefs.GetInt("MangleAI", 0);
            goldenFreddyAI = PlayerPrefs.GetInt("GoldenFreddyAI", 0);

			SetModeFinished();
        }

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

		if (nightNumber == 1) // If it's the first night
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

	private void SetModeFinished()
	{
		if (CheckCustomNightMode(20, 20, 20, 20, 0, 0, 0, 0, 0, 0))
		{
			saveManager.SaveDoneMode(0, true);
		}
		else if (CheckCustomNightMode(0, 0, 0, 0, 10, 10, 10, 10, 10, 0))
		{
			saveManager.SaveDoneMode(1, true);
		}
		else if (CheckCustomNightMode(0, 20, 0, 5, 0, 0, 20, 0, 0, 0))
		{
			saveManager.SaveDoneMode(2, true);
		}
		else if (CheckCustomNightMode(0, 0, 0, 0, 20, 0, 0, 0, 20, 10))
		{
			saveManager.SaveDoneMode(3, true);
		}
		else if (CheckCustomNightMode(0, 0, 0, 20, 0, 0, 0, 0, 20, 0))
		{
			saveManager.SaveDoneMode(4, true);
		}
		else if (CheckCustomNightMode(0, 0, 20, 0, 0, 0, 0, 20, 20, 0))
		{
			saveManager.SaveDoneMode(5, true);
		}
		else if (CheckCustomNightMode(20, 0, 0, 10, 10, 20, 0, 0, 0, 10))
		{
			saveManager.SaveDoneMode(6, true);
		}
		else if (CheckCustomNightMode(5, 5, 5, 5, 5, 5, 5, 5, 5, 5))
		{
			saveManager.SaveDoneMode(7, true);
		}
		else if (CheckCustomNightMode(10, 10, 10, 10, 10, 10, 10, 10, 10, 10))
		{
			saveManager.SaveDoneMode(8, true);
		}
		else if (CheckCustomNightMode(20, 20, 20, 20, 20, 20, 20, 20, 20, 20))
		{
			saveManager.SaveDoneMode(9, true);
		}
	}

	private bool CheckCustomNightMode(int freddy, int bonnie, int chica, int foxy, int bb, int toyFreddy, int toyBonnie, int toyChica, int mangle, int goldenFreddy)
	{
		return freddyAI == freddy &&
			bonnieAI == bonnie &&
			chicaAI == chica &&
			foxyAI == foxy &&
			balloonBoyAI == bb &&
			toyFreddyAI == toyFreddy &&
			toyBonnieAI == toyBonnie &&
			toyChicaAI == toyChica &&
			mangleAI == mangle &&
			goldenFreddyAI == goldenFreddy;
    }
}
