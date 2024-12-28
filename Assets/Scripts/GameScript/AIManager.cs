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
                    { "MangleAI", 3 },
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
                    { "MangleAI", 5 },
                    { "BalloonBoyAI", 3 },
                    { "WitheredBonnieAI", 1 },
                    { "WitheredFoxyAI", 7 }
                }},
                { 2, new Dictionary<string, int>() {
                    { "ToyBonnieAI", 1 },
                    { "MangleAI", 5 },
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
                    { "MangleAI", 1 },
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
                    { "MangleAI", 10 },
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
                    { "MangleAI", 3 },
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
                    { "MangleAI", 10 },
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
    }

    public void TimedEvents() {
    if (nightConfigurations.ContainsKey(currentNight)) {
        Debug.Log("Found configuration for Night:"+currentNight);
        var nightConfig = nightConfigurations[currentNight];
        if (nightConfig.ContainsKey(currentTime)) {
            Debug.Log("Found configuration for Time:"+currentTime);
            var timeConfig = nightConfig[currentTime];
            foreach (var ai in timeConfig) {
                if (aiLevels.ContainsKey(ai.Key)) {
                    aiLevels[ai.Key] = ai.Value;
                    Debug.Log("{ai.Key} set to "+ai.Value);
                }
            }
        } else {
            Debug.Log("No configuration for Time:"+currentTime);
        }
    } else {
        Debug.Log("No configuration for Night:"+currentNight);
    }

    // Vérifier si nuit 6 est complète
    if (currentNight == 6 && currentTime >= 6) {
        SceneManager.LoadScene("6AM");
    }
}
    //Get AI Level in other scripts (like nightplayer)
    public int GetAILevel(string aiName) {
    if (aiLevels.ContainsKey(aiName)) {
        return aiLevels[aiName];
    }
    return -1;
}
}
