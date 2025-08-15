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

    // --- --- --- ---
    // Save
    public void SaveLanguage(string language)
	{
        PlayerPrefs.SetString("Language", language);
        PlayerPrefs.Save();
    }

    public void SaveShareAnalytics(int shareAnalytics)
    {
        PlayerPrefs.SetInt("ShareAnalytics", shareAnalytics);
        PlayerPrefs.Save();
    }

    public void SaveDubbingLanguage(string language)
    {
        PlayerPrefs.SetString("DubbingLanguage", language);
        PlayerPrefs.Save();
    }

    public void SaveIntroDreamPlayed(int played)
    {
        PlayerPrefs.SetInt("IntroDreamPlayed", played);
        PlayerPrefs.Save();
    }

    public void SaveUnlockedStars(int starId, bool unlock)
    {
        PlayerPrefs.SetInt("Star_" + starId, unlock ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveLayoutId(int layoutId)
    {
        PlayerPrefs.SetInt("Layout", layoutId);
        PlayerPrefs.Save();
    }

    public void SaveUserToken(string token)
    {
        PlayerPrefs.SetString("UserToken", token);
        PlayerPrefs.Save();
    }

    public void SaveMotionControls(bool motionControls)
    {
        PlayerPrefs.SetInt("MotionControls", motionControls ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SavePointerVisibility(bool visibility)
    {
        PlayerPrefs.SetInt("PointerVisibility", visibility ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveDoneMode(int modeId, bool isDone)
    {
        PlayerPrefs.SetInt("DoneMode_" + modeId, isDone ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SaveGeneralVolume(int volume)
    {
        PlayerPrefs.SetInt("GeneralVolume", volume);
        PlayerPrefs.Save();
    }

    public void SaveMusicVolume(int volume)
    {
        PlayerPrefs.SetInt("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SaveVoiceVolume(int volume)
    {
        PlayerPrefs.SetInt("VoiceVolume", volume);
        PlayerPrefs.Save();
    }

    public void SaveSFXVolume(int volume)
    {
        PlayerPrefs.SetInt("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    // Load
    public static string LoadLanguage()
	{
        if (PlayerPrefs.HasKey("Language"))
        {
            return PlayerPrefs.GetString("Language");
        }
        else
        {
            return null;
        }
    }

    public static int LoadShareAnalytics()
    {
        if (PlayerPrefs.HasKey("ShareAnalytics"))
        {
            return PlayerPrefs.GetInt("ShareAnalytics");
        }
        else
        {
            return -1;
        }
    }

    public static string LoadDubbingLanguage()
    {
        if (PlayerPrefs.HasKey("DubbingLanguage"))
        {
            return PlayerPrefs.GetString("DubbingLanguage");
        }
        else
        {
            return null;
        }
    }

    public static int LoadIntroDreamPlayed()
    {
        if (PlayerPrefs.HasKey("IntroDreamPlayed"))
        {
            return PlayerPrefs.GetInt("IntroDreamPlayed");
        }
        else
        {
            return 0;
        }
    }

    public static bool LoadUnlockedStars(int starId)
    {
        if (PlayerPrefs.HasKey("Star_" + starId))
        {
            return PlayerPrefs.GetInt("Star_" + starId, 0) == 1;
        }
        else
        {
            return false;
        }
    }

    public static int LoadLayoutId()
    {
        if (PlayerPrefs.HasKey("Layout"))
        {
            int layoutId = PlayerPrefs.GetInt("Layout");

            return layoutId;
        }
        else
        {
            return 1;
        }
    }

    public static string LoadUserToken()
    {
        if (PlayerPrefs.HasKey("UserToken"))
        {
            return PlayerPrefs.GetString("UserToken");
        }
        else
        {
            return null;
        }
    }

    public static bool LoadMotionControls()
    {
        if (PlayerPrefs.HasKey("MotionControls"))
        {
            return PlayerPrefs.GetInt("MotionControls") == 1;
        }
        else
        {
            return true;
        }
    }

    public static bool LoadPointerVisibility()
    {
        if (PlayerPrefs.HasKey("PointerVisibility"))
        {
            return PlayerPrefs.GetInt("PointerVisibility") == 1;
        }
        else
        {
            return true;
        }
    }

    public static bool LoadDoneMode(int modeId)
    {
        if (PlayerPrefs.HasKey("DoneMode_" + modeId))
        {
            return PlayerPrefs.GetInt("DoneMode_" + modeId) == 1;
        }
        else
        {
            return false;
        }
    }

    public static int LoadGeneralVolume()
    {
        if (PlayerPrefs.HasKey("GeneralVolume"))
        {
            return PlayerPrefs.GetInt("GeneralVolume");
        }
        else
        {
            return 10;
        }
    }

    public static int LoadMusicVolume()
    {
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            return PlayerPrefs.GetInt("MusicVolume");
        }
        else
        {
            return 10;
        }
    }

    public static int LoadVoiceVolume()
    {
        if (PlayerPrefs.HasKey("VoiceVolume"))
        {
            return PlayerPrefs.GetInt("VoiceVolume");
        }
        else
        {
            return 10;
        }
    }

    public static int LoadSFXVolume()
    {
        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            return PlayerPrefs.GetInt("SFXVolume");
        }
        else
        {
            return 10;
        }
    }
}

[Serializable]
public class SaveData
{
    public Game game = new Game();
    public Settings settings = new Settings();
}

[Serializable]
public class Game
{
    public int nightNumber = 0;
    public int starsId = 0;
}

[Serializable]
public class Settings
{

}

[Serializable]
public class Volume
{

}