using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinGame : MonoBehaviour {

	public AudioSource Children;
	public int currentNight;
	public string WhatChallengeIsThis;
	public bool isNight7;

	// Use this for initialization
	void Start () {
		StartCoroutine(Main());
	}
	
	IEnumerator Main()
	{
		yield return new WaitForSeconds(4f);
		Children.Play();
		yield return new WaitForSeconds(8f);
		if (currentNight == 5)
		{
			DataManager.SaveValue("currentNight", currentNight + 1, "data:/");
			SceneManager.LoadScene("Night5Ending");
		}
		else if (currentNight == 2)
		{
			DataManager.SaveValue("currentNight", currentNight + 1, "data:/");
			SceneManager.LoadScene("Night2PostCutscene");
		}
		else if (currentNight == 3)
		{
			DataManager.SaveValue("currentNight", currentNight + 1, "data:/");
			SceneManager.LoadScene("Night3PostCutscene");
		}
		else if (currentNight == 4)
		{
			DataManager.SaveValue("currentNight", currentNight + 1, "data:/");
			SceneManager.LoadScene("Night4PostCutscene");
		}
		else if (currentNight == 6)
		{
			DataManager.SaveValue("beatNight6", true, "data:/");
			SceneManager.LoadScene("Night6Ending");
		}
		else if (currentNight == 7)
		{
			if (WhatChallengeIsThis != null)
			{
				DataManager.SaveValue(WhatChallengeIsThis, true, "data:/");
				if (WhatChallengeIsThis == "Golden Freddy Mode Completed")
				{
					DataManager.SaveValue("beat10/20", true, "data:/");
				}
			}
			SceneManager.LoadScene("Night7Ending");
		}
		else
		{
			DataManager.SaveValue("currentNight", currentNight + 1, "data:/");
			SceneManager.LoadScene("MainMenuLoader");
		}
	}
}
