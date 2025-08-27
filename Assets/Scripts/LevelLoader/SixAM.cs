using System.Collections;
using UnityEngine;

public class SixAM : MonoBehaviour
{
	public Animator fadeAnimator;
	public AudioSource happyChildrenAudio;

	private int nightNumber;

	// AI levels
	private int witheredFreddyDifficulty;
    private int witheredBonnieDifficulty;
    private int witheredChicaDifficulty;
	private int witheredFoxyDifficulty;
	private int bbDifficulty;
    private int toyFreddyDifficulty;
    private int toyBonnieDifficulty;
	private int toyChicaDifficulty;
	private int mangleDifficulty;
	private int goldenDifficulty;

    // Scripts
	LevelLoader levelLoader;

	void Start()
	{
		// Get scripts
		levelLoader = FindObjectOfType<LevelLoader>();

		// Disable loading screen when the game starts
		levelLoader.loadingScreen.SetActive(false);

		// Get current night number
		nightNumber = SaveManager.saveData.game.nightNumber;

        // Increase night number and apply it
        nightNumber++;
		SaveManager.saveData.game.nightNumber = nightNumber;

        // Get AI levels
        witheredFreddyDifficulty = NightPlayer.witheredFreddyDifficulty;
        witheredBonnieDifficulty = NightPlayer.witheredBonnieDifficulty;
        witheredChicaDifficulty = NightPlayer.witheredChicaDifficulty;
        witheredFoxyDifficulty = NightPlayer.witheredFoxyDifficulty;
        bbDifficulty = NightPlayer.bbDifficulty;
        toyFreddyDifficulty = NightPlayer.toyFreddyDifficulty;
        toyBonnieDifficulty = NightPlayer.toyBonnieDifficulty;
        toyChicaDifficulty = NightPlayer.toyChicaDifficulty;
        mangleDifficulty = NightPlayer.mangleDifficulty;
        goldenDifficulty = NightPlayer.goldenDifficulty;

        if (nightNumber == 7)
		{
			SetModeFinished();
        }

		UnlockNightAchievements();

        SaveManager.Save();

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
			SaveManager.saveData.game.ChangeDoneModeStatus(0, true);
		}
		else if (CheckCustomNightMode(0, 0, 0, 0, 10, 10, 10, 10, 10, 0))
		{
            SaveManager.saveData.game.ChangeDoneModeStatus(1, true);
		}
		else if (CheckCustomNightMode(0, 20, 0, 5, 0, 0, 20, 0, 0, 0))
		{
            SaveManager.saveData.game.ChangeDoneModeStatus(2, true);
		}
		else if (CheckCustomNightMode(0, 0, 0, 0, 20, 0, 0, 0, 20, 10))
		{
            SaveManager.saveData.game.ChangeDoneModeStatus(3, true);
		}
		else if (CheckCustomNightMode(0, 0, 0, 20, 0, 0, 0, 0, 20, 0))
		{
            SaveManager.saveData.game.ChangeDoneModeStatus(4, true);
		}
		else if (CheckCustomNightMode(0, 0, 20, 0, 0, 0, 0, 20, 20, 0))
		{
            SaveManager.saveData.game.ChangeDoneModeStatus(5, true);
		}
		else if (CheckCustomNightMode(20, 0, 0, 10, 10, 20, 0, 0, 0, 10))
		{
            SaveManager.saveData.game.ChangeDoneModeStatus(6, true);
		}
		else if (CheckCustomNightMode(5, 5, 5, 5, 5, 5, 5, 5, 5, 5))
		{
            SaveManager.saveData.game.ChangeDoneModeStatus(7, true);
		}
		else if (CheckCustomNightMode(10, 10, 10, 10, 10, 10, 10, 10, 10, 10))
		{
            SaveManager.saveData.game.ChangeDoneModeStatus(8, true);
		}
		else if (CheckCustomNightMode(20, 20, 20, 20, 20, 20, 20, 20, 20, 20))
		{
			SaveManager.saveData.game.ChangeDoneModeStatus(9, true);
		}
	}

	private void UnlockNightAchievements()
	{
		switch (nightNumber)
		{
			case 1:
				if (MedalsManager.medalsManager != null)
				{
					MedalsManager.medalsManager.UnlockAchievement(Achievements.achievements.ONENIGHTATFREDDYS);
				}
				break;
			case 2:
				if (MedalsManager.medalsManager != null)
				{
					MedalsManager.medalsManager.UnlockAchievement(Achievements.achievements.TWONIGHTSATFREDDYS);
				}
				break;
			case 3:
				if (MedalsManager.medalsManager != null)
				{
					MedalsManager.medalsManager.UnlockAchievement(Achievements.achievements.THREENIGHTSATFREDDYS);
				}
				break;
			case 4:
				if (MedalsManager.medalsManager != null)
				{
					MedalsManager.medalsManager.UnlockAchievement(Achievements.achievements.FOURNIGHTSATFREDDYS);
				}
				break;
			case 5:
				if (MedalsManager.medalsManager != null)
				{
					MedalsManager.medalsManager.UnlockAchievement(Achievements.achievements.FIVENIGHTSATFREDDYS);
				}
				break;
			case 6:
				if (MedalsManager.medalsManager != null)
				{
					MedalsManager.medalsManager.UnlockAchievement(Achievements.achievements.YOUSTAYED);
				}
				break;
			case 7:
				if (CheckCustomNightMode(20, 20, 20, 20, 20, 20, 20, 20, 20, 20))
				{
					if (MedalsManager.medalsManager != null)
					{
						MedalsManager.medalsManager.UnlockAchievement(Achievements.achievements.YOUTAMPERED);
					}
				}
				break;
			default:
				break;
		}
	}

	private bool CheckCustomNightMode(int freddy, int bonnie, int chica, int foxy, int bb, int toyFreddy, int toyBonnie, int toyChica, int mangle, int goldenFreddy)
	{
		return witheredFreddyDifficulty == freddy &&
			witheredBonnieDifficulty == bonnie &&
            witheredChicaDifficulty == chica &&
            witheredFoxyDifficulty == foxy &&
            bbDifficulty == bb &&
            toyFreddyDifficulty == toyFreddy &&
            toyBonnieDifficulty == toyBonnie &&
            toyChicaDifficulty == toyChica &&
            mangleDifficulty == mangle &&
            goldenDifficulty == goldenFreddy;
    }
}
