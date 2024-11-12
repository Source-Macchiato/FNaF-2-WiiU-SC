using UnityEngine;

public class SaveManager : MonoBehaviour
{
    // Save
	public void SaveLanguage(string language)
	{
        PlayerPrefs.SetString("Language", language);
        PlayerPrefs.Save();
    }

    public void SaveNightNumber(float nightNumber)
    {
        PlayerPrefs.SetFloat("NightNumber", nightNumber);
        PlayerPrefs.Save();
    }

    public void SaveShareData(float shareData)
    {
        PlayerPrefs.SetFloat("ShareData", shareData);
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
}