using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class AIManager : MonoBehaviour {
    public NightPlayer nightPlayer;
    public int currentTime;
    public int currentNight;

    public float TimeMultiplier;
    public bool isNight7;
    public int PuppetAI;

    // AI Dictionary for storing AI levels dynamically
    private Dictionary<string, int> aiLevels;

    // AI Configuration per night and time
    private Dictionary<int, Dictionary<int, Dictionary<string, int>>> nightConfigurations;


    private void Start() {
        // Initialize AI levels
        aiLevels = new Dictionary<string, int>() {
            { "ToyFreddyAI", 20 },
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
                    { "MangleAI", 0 }, //3
                    { "BalloonBoyAI", 3 },
                    { "WitheredFoxyAI", 3 }
                }}
            }},
            // Night 3
            { 2, new Dictionary<int, Dictionary<string, int>>() {
                { 12, new Dictionary<string, int>() {
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
                { 12, new Dictionary<string, int>() {
                    { "MangleAI", 0 }, //5
                    { "BalloonBoyAI", 3 },
                    { "WitheredBonnieAI", 1 },
                    { "WitheredFoxyAI", 7 }
                }},
                { 2, new Dictionary<string, int>() {
                    { "ToyBonnieAI", 1 },
                    { "MangleAI", 0 }, //5
                    { "BalloonBoyAI", 3 },
                    { "WitheredFreddyAI", 3 },
                    { "WitheredBonnieAI", 4 },
                    { "WitheredChicaAI", 4 },
                    { "WitheredFoxyAI", 7 }
                }}
            }},
            // Night 5
            { 4, new Dictionary<int, Dictionary<string, int>>() {
                { 12, new Dictionary<string, int>() {
                    { "ToyFreddyAI", 5 },
                    { "ToyBonnieAI", 2 },
                    { "ToyChicaAI", 2 },
                    { "MangleAI", 0 }, //1
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
                    { "MangleAI", 0 }, //10
                    { "BalloonBoyAI", 5 },
                    { "WitheredFreddyAI", 5 },
                    { "WitheredBonnieAI", 5 },
                    { "WitheredChicaAI", 5 },
                    { "WitheredFoxyAI", 7 }
                }}
            }},
            // Night 6
            { 5, new Dictionary<int, Dictionary<string, int>>() {
                { 12, new Dictionary<string, int>() {
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
                    { "MangleAI", 0 }, //10
                    { "BalloonBoyAI", 9 },
                    { "WitheredFreddyAI", 10 },
                    { "WitheredBonnieAI", 10 },
                    { "WitheredChicaAI", 10 },
                    { "WitheredFoxyAI", 15 }
                }}
            }}
        };
    }

    private void Update() {
        currentTime = nightPlayer.currentTime;
        currentNight = nightPlayer.currentNight;
        TimedEvents();

        //i'm very sorry for spagetthi code, we have to finish the game fast. Hello to the guy who will decompile this shitty ass source code -- shiro
        // Good luck everyone -- Alyx
        if (nightPlayer.currentCam == nightPlayer.MangleCamera)
        {
            nightPlayer.CamWatchAI = 9;
        }
        if (nightPlayer.currentCam == nightPlayer.BBCamera)
        {
            nightPlayer.CamWatchAI = 8;
        }
        if (nightPlayer.currentCam == nightPlayer.ToyBonnieCamera)
        {
            nightPlayer.CamWatchAI = 1;
        }
        if (nightPlayer.currentCam == nightPlayer.ToyChicaCamera)
        {
            nightPlayer.CamWatchAI = 3;
        }
        if (nightPlayer.currentCam == nightPlayer.ToyFreddyCamera)
        {
            nightPlayer.CamWatchAI = 2;
        }
        if (nightPlayer.currentCam == nightPlayer.WitheredBonnieCamera)
        {
            nightPlayer.CamWatchAI = 4;
        }
        if (nightPlayer.currentCam == nightPlayer.WitheredChicaCamera)
        {
            nightPlayer.CamWatchAI = 6;
        }
        if (nightPlayer.currentCam == nightPlayer.WitheredFreddyCamera)
        {
            nightPlayer.CamWatchAI = 5;
        }
        if (nightPlayer.currentCam == nightPlayer.WitheredFoxyCamera)
        {
            nightPlayer.CamWatchAI = 7;
        }

        if (Application.isEditor)
        {
            Debug.Log("currentCam : " + nightPlayer.currentCam);
            Debug.Log("camwatch : "+ nightPlayer.CamWatchAI);
            Debug.Log("TFreddyCamera : " + nightPlayer.ToyFreddyCamera);
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
