using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Save
	public void SaveLanguage(string language)
	{
        PlayerPrefs.SetString("Language", language);
        PlayerPrefs.Save();
    }

    public void SaveNightNumber(int nightNumber)
    {
        PlayerPrefs.SetInt("NightNumber", nightNumber);
        PlayerPrefs.Save();
    }

    public void SaveShareData(float shareData)
    {
        PlayerPrefs.SetFloat("ShareData", shareData);
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

    public static int LoadNightNumber()
    {
        if (PlayerPrefs.HasKey("NightNumber"))
        {
            return PlayerPrefs.GetInt("NightNumber");
        }
        else
        {
            return 0;
        }
    }

    public static int LoadShareData()
    {
        if (PlayerPrefs.HasKey("ShareData"))
        {
            return PlayerPrefs.GetInt("ShareData");
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
}