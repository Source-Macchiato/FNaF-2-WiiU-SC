using System;
using System.Text;
using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class SaveManager : MonoBehaviour
{
    public static SaveData saveData = new SaveData();
    public static string token;

    void Start()
    {
        // Load data
        string json = SaveGameState.DoLoad();

        if (json != string.Empty)
        {
            saveData = JsonUtility.FromJson<SaveData>(json);

            Debug.Log("Data has been loaded.");
        }
        else
        {
            Debug.Log("Data has not been loaded.");
        }

        // Load token
        WiiU.SDCard.Init();
        if (WiiU.SDCard.FileExists("wiiu/apps/BrewConnect/token"))
        {
            token = WiiU.SDCard.ReadAllText("wiiu/apps/BrewConnect/token").Trim();
        }
        WiiU.SDCard.DeInit();
    }

    public static void Save()
    {
        string json = JsonUtility.ToJson(saveData);
        byte[] data = Encoding.UTF8.GetBytes(json);
        SaveGameState.DoSave(data);
    }
}

[Serializable]
public class SaveData
{
    public Game game = new Game();
    public Settings settings = new Settings();
    public bool[] achievements = new bool[Enum.GetValues(typeof(Achievements.achievements)).Length];

    public void UnlockAchievement(Achievements.achievements a)
    {
        achievements[(int)a] = true;
    }
}

[Serializable]
public class Game
{
    public int nightNumber = 0;
    public int starsId = 0;
    public bool dreamIntroPlayed = false;
    public bool[] doneModes = new bool[10];
    public bool[] unlockedStars = new bool[3];
    public bool[] playedMinigames = new bool[4];

    public void ChangeDoneModeStatus(int modeId, bool isDone)
    {
        doneModes[modeId] = isDone;
    }

    public void ChangeUnlockedStarStatus(int starId, bool unlocked)
    {
        unlockedStars[starId] = unlocked;
    }

    public void ChangePlayedMinigameStatus(int minigameId, bool played)
    {
        playedMinigames[minigameId] = played;
    }
}

[Serializable]
public class Settings
{
    public string language = string.Empty;
    public string dubbingLanguage = string.Empty;
    public int layoutId = 1;
    public int ratioId = 0;
    public int shareAnalytics = -1;
    public Volume volume = new Volume();
    public bool motionControls = true;
    public bool pointerVisibility = true;
    public bool panoramicEffect = true;
    public bool subtitlesEnabled = true;
}

[Serializable]
public class Volume
{
    public int generalVolume = 10;
    public int musicVolume = 10;
    public int voiceVolume = 10;
    public int sfxVolume = 10;
}