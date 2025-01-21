using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AIManager : MonoBehaviour {
    public NightPlayer nightPlayer;
    public MaskManager maskManager;
    public int currentTime;
    public int currentNight;

    public float TimeMultiplier;
    public bool isNight7;
    public int PuppetAI;

    public float RollChanceFreddy = 7f;
    public float RollChanceBonnie = 7f;
    public float RollChanceChica = 7f;
    public float RollChanceMangle = 7f;
    public float RollChanceToyFreddy = 7f;
    public float RollChanceToyChica = 7f;


    // AI Dictionary for storing AI levels dynamically
    private Dictionary<string, int> aiLevels;

    // AI Configuration per night and time
    private Dictionary<int, Dictionary<int, Dictionary<string, int>>> nightConfigurations;


    private void Start() {
        // Initialize AI levels

        if(Application.isEditor && !isNight7)
        {
            aiLevels = new Dictionary<string, int>() {
            { "ToyFreddyAI", 0 },
            { "ToyBonnieAI", 0 },
            { "ToyChicaAI", 0 },
            { "WitheredFreddyAI", 0 },
            { "WitheredBonnieAI", 0 },
            { "WitheredChicaAI", 0 },
            { "WitheredFoxyAI", 0 },
            { "GoldenFreddyAI", 0 },
            { "MangleAI", 0 },
            { "BalloonBoyAI", 0 },
            { "PaperpalsAI", 0 }
            };
        }
        else
        {
            aiLevels = new Dictionary<string, int>() {
                { "ToyFreddyAI", 0 },
                { "ToyBonnieAI", 0 },
                { "ToyChicaAI", 0 },
                { "WitheredFreddyAI", 0 },
                { "WitheredBonnieAI", 0 },
                { "WitheredChicaAI", 0 },
                { "WitheredFoxyAI", 0 },
                { "GoldenFreddyAI", 0 },
                { "MangleAI", 0 },
                { "BalloonBoyAI", 0 },
                { "PaperpalsAI", 0 }
            };
        }

        // Configure AI settings for each night and time
        nightConfigurations = new Dictionary<int, Dictionary<int, Dictionary<string, int>>>() {

            // Night 1
            { 0, new Dictionary<int, Dictionary<string, int>>() {
                { 1, new Dictionary<string, int>() {
                    { "ToyBonnieAI", 2 },
                    { "ToyChicaAI", 2 }
                }},
                { 2, new Dictionary<string, int>() {
                    { "ToyFreddyAI", 2 },
                    { "ToyBonnieAI", 3 }
                }}
            }},
            // Night 2
            { 1, new Dictionary<int, Dictionary<string, int>>() {
                { 1, new Dictionary<string, int>() {
                    { "ToyFreddyAI", 2 },
                    { "ToyBonnieAI", 3 },
                    { "ToyChicaAI", 3 },
                    { "MangleAI", 3 }, //3
                    { "BalloonBoyAI", 3 },
                    { "WitheredFoxyAI", 3 }
                }}
            }},
            // Night 3
            { 2, new Dictionary<int, Dictionary<string, int>>() {
                { 0, new Dictionary<string, int>() {
                    { "BalloonBoyAI", 1 },
                    { "WitheredBonnieAI", 1 },
                    { "WitheredChicaAI", 1 },
                    { "WitheredFoxyAI", 2 }
                }},
                { 1, new Dictionary<string, int>() {
                    { "ToyBonnieAI", 1 },
                    { "ToyChicaAI", 1 },
                    { "BalloonBoyAI", 2 },
                    { "WitheredFreddyAI", 2 },
                    { "WitheredBonnieAI", 2 },
                    { "WitheredChicaAI", 2 },
                    { "WitheredFoxyAI", 3 }
                }}
            }},
            // Night 4
            { 3, new Dictionary<int, Dictionary<string, int>>() {
                { 0, new Dictionary<string, int>() {
                    { "MangleAI", 5 }, //5
                    { "BalloonBoyAI", 3 },
                    { "WitheredBonnieAI", 1 },
                    { "WitheredFoxyAI", 7 }
                }},
                { 2, new Dictionary<string, int>() {
                    { "ToyBonnieAI", 1 },
                    { "MangleAI", 5 }, //5
                    { "BalloonBoyAI", 3 },
                    { "WitheredFreddyAI", 3 },
                    { "WitheredBonnieAI", 4 },
                    { "WitheredChicaAI", 4 },
                    { "WitheredFoxyAI", 7 }
                }}
            }},
            // Night 5
            { 4, new Dictionary<int, Dictionary<string, int>>() {
                { 0, new Dictionary<string, int>() {
                    { "ToyFreddyAI", 5 },
                    { "ToyBonnieAI", 2 },
                    { "ToyChicaAI", 2 },
                    { "MangleAI", 1 }, //1
                    { "BalloonBoyAI", 5 },
                    { "WitheredFreddyAI", 2 },
                    { "WitheredBonnieAI", 2 },
                    { "WitheredChicaAI", 2 },
                    { "WitheredFoxyAI", 5 }
                }},
                { 1, new Dictionary<string, int>() {
                    { "ToyFreddyAI", 1 },
                    { "ToyBonnieAI", 2 },
                    { "ToyChicaAI", 2 },
                    { "MangleAI", 10 }, //10
                    { "BalloonBoyAI", 5 },
                    { "WitheredFreddyAI", 5 },
                    { "WitheredBonnieAI", 5 },
                    { "WitheredChicaAI", 5 },
                    { "WitheredFoxyAI", 7 }
                }}
            }},
            // Night 6
            { 5, new Dictionary<int, Dictionary<string, int>>() {
                { 0, new Dictionary<string, int>() {
                    { "MangleAI", 0 },//3
                    { "BalloonBoyAI", 5 },
                    { "WitheredFreddyAI", 5 },
                    { "WitheredBonnieAI", 5 },
                    { "WitheredChicaAI", 5 },
                    { "WitheredFoxyAI", 10 }
                }},
                { 2, new Dictionary<string, int>() {
                    { "ToyFreddyAI", 5 },
                    { "ToyBonnieAI", 5 },
                    { "ToyChicaAI", 5 },
                    { "MangleAI", 10 }, //10
                    { "BalloonBoyAI", 9 },
                    { "WitheredFreddyAI", 10 },
                    { "WitheredBonnieAI", 10 },
                    { "WitheredChicaAI", 10 },
                    { "WitheredFoxyAI", 15 }
                }}
            }},
            // Night 7
            { 6, new Dictionary<int, Dictionary<string, int>>() {
                { 0, new Dictionary<string, int>() {
                    { "MangleAI", PlayerPrefs.GetInt("MangleAI", 0) },//3
                    { "BalloonBoyAI", PlayerPrefs.GetInt("BalloonBoyAI", 0) },
                    { "WitheredFreddyAI", PlayerPrefs.GetInt("FreddyAI", 0) },
                    { "WitheredBonnieAI", PlayerPrefs.GetInt("BonnieAI", 0) },
                    { "WitheredChicaAI", PlayerPrefs.GetInt("ChicaAI", 0) },
                    { "WitheredFoxyAI", PlayerPrefs.GetInt("FoxyAI", 0) },
                    {"toyFreddyAI", PlayerPrefs.GetInt("ToyFreddyAI", 0)},
                    {"toyBonnieAI", PlayerPrefs.GetInt("ToyBonnieAI", 0)},
                    {"toyChicaAI", PlayerPrefs.GetInt("ToyChicaAI", 0)}
                }}
            }}
        };
    }

    private void Update() {
        currentTime = nightPlayer.currentTime;
        currentNight = nightPlayer.currentNight;
        TimedEvents();

        if(nightPlayer.WitheredFreddyCamera == 14)
		{
			if (maskManager.isMaskActive)
			{
				Debug.Log("withered freddy mask active");
			    // Toujours échouer si le masque est actif
			    nightPlayer.WitheredFreddyMovement = 5f;
			    // Une chance sur 5 de revenir à la caméra 7
				int RandomChance = Random.Range(0, 5);
				RollChanceFreddy -= Time.deltaTime;

				if(RollChanceFreddy <= 0f)
				{
					RollChanceFreddy = 7f;
					if (RandomChance == 0)
			    	{
			    	    nightPlayer.WitheredFreddyCamera = 7;

			    	}
				}
			}
		}
    if(nightPlayer.WitheredBonnieCamera == 14)
    {
        if (maskManager.isMaskActive)
        {
        Debug.Log("withered bonnie mask active");
        nightPlayer.WitheredBonnieMovement = 5f;
        int RandomChance = Random.Range(0, 5);
        RollChanceBonnie -= Time.deltaTime;

        if(RollChanceBonnie <= 0f)
        {
            RollChanceBonnie = 7f;
            if (RandomChance == 0)
            {
            nightPlayer.WitheredBonnieCamera = 7;
            }
        }
        }
    }

    if(nightPlayer.WitheredChicaCamera == 14)
    {
        if (maskManager.isMaskActive)
        {
        Debug.Log("withered chica mask active");
        nightPlayer.WitheredChicaMovement = 5f;
        int RandomChance = Random.Range(0, 5);
        RollChanceChica -= Time.deltaTime;

        if(RollChanceChica <= 0f)
        {
            RollChanceChica = 7f;
            if (RandomChance == 0)
            {
            nightPlayer.WitheredChicaCamera = 7;
            }
        }
        }
    }

    if(nightPlayer.MangleCamera == 14)
    {
        if (maskManager.isMaskActive)
        {
        Debug.Log("mangle mask active");
        nightPlayer.MangleMovement = 5f;
        int RandomChance = Random.Range(0, 5);
        RollChanceMangle -= Time.deltaTime;

        if(RollChanceMangle <= 0f)
        {
            RollChanceMangle = 7f;
            if (RandomChance == 0)
            {
            nightPlayer.MangleCamera = 7;
            }
        }
        }
    }

    if(nightPlayer.ToyChicaCamera == 14)
    {
        if (maskManager.isMaskActive)
        {
        Debug.Log("toy chica mask active");
        nightPlayer.ToyChicaMovement = 5f;
        int RandomChance = Random.Range(0, 5);
        RollChanceToyChica -= Time.deltaTime;

        if(RollChanceToyChica <= 0f)
        {
            RollChanceToyChica = 7f;
            if (RandomChance == 0)
            {
            nightPlayer.ToyChicaCamera = 7;
            }
        }
        }
    }

    if(nightPlayer.ToyFreddyCamera == 14)
    {
        if (maskManager.isMaskActive)
        {
        Debug.Log("toy freddy mask active");
        nightPlayer.ToyFreddyMovement = 5f;
        int RandomChance = Random.Range(0, 5);
        RollChanceToyFreddy -= Time.deltaTime;

        if(RollChanceToyFreddy <= 0f)
        {
            RollChanceToyFreddy = 7f;
            if (RandomChance == 0)
            {
            nightPlayer.ToyFreddyCamera = 7;
            }
        }
        }
    }
    }

    public void TimedEvents() {
        if (nightConfigurations.ContainsKey(currentNight))
        {
            var nightConfig = nightConfigurations[currentNight];
            if (nightConfig.ContainsKey(currentTime))
            {
                var timeConfig = nightConfig[currentTime];
                foreach (var ai in timeConfig)
                {
                    if (aiLevels.ContainsKey(ai.Key))
                    {
                        aiLevels[ai.Key] = ai.Value;
                    }
                }
            }
        }

        if (currentTime >= 6)
        {
            SceneManager.LoadScene("6AM");
        }
    }

    //Get AI Level in other scripts (like nightplayer)
    public int GetAILevel(string aiName)
    {
        if (aiLevels.ContainsKey(aiName))
        {
            return aiLevels[aiName];
        }
        return -1;
    }


}
