using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class AnalyticsData : MonoBehaviour
{
    private string projectToken = "9637629c27bb7871e9fa3bbe294cf09153b8be5831caa03ab935fb098928ee9b";
    private string analyticsToken;
    private int canShareAnalytics;

    MenuManager menuManager;
    SaveManager saveManager;
    SaveGameState saveGameState;

    // Add analytics
    [Serializable]
    private class AddAnalyticsResponse
    {
        public string[] message;
        public AddDataResponse data;
    }

    [Serializable]
    private class AddDataResponse
    {
        public string token;
    }

    // Update analytics
    [Serializable]
    private class UpdateAnalyticsResponse
    {
        public string[] message;
    }

    void Start()
    {
        menuManager = FindObjectOfType<MenuManager>();
        saveManager = FindObjectOfType<SaveManager>();
        saveGameState = FindObjectOfType<SaveGameState>();

        canShareAnalytics = SaveManager.LoadShareAnalytics();

        if (canShareAnalytics == -1)
        {
            menuManager.AddPopup(1);
        }
        else if (canShareAnalytics == 1)
        {
            StartCoroutine(SendAnalytics());
        }
    }

    private IEnumerator SendAnalytics()
	{
        string url = "https://api.brew-connect.com/v1/online/send_analytics";
        string json = "{" +
            "\"project_token\": \"" + projectToken + "\"," +
            "\"category_name\": \"game\"," +
            "\"analytics_entries\": [" +
                "{" +
                    "\"name\": \"username\"," +
                    "\"value\": \"" + GetAccountName() + "\"" +
                "}," +
                "{" +
                    "\"name\": \"version\"," +
                    "\"value\": \"" + GetVersion() + "\"" +
                "}," +
                "{" +
                    "\"name\": \"language\"," +
                    "\"value\": \"" + GetLanguage() + "\"" +
                "}" +
            "]" +
        "}";
        byte[] post = System.Text.Encoding.UTF8.GetBytes(json);

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("content-Type", "application/json");

        using (WWW www = new WWW(url, post, headers))
        {
            yield return www;

            string jsonResponse = www.text;
            AddAnalyticsResponse response = JsonUtility.FromJson<AddAnalyticsResponse>(jsonResponse);

            Debug.Log(response.message[0]);

            if (StatusCode(www) == 201)
            {
                analyticsToken = response.data.token;
            }
        }
    }

    public IEnumerator UpdateAnalytics(string key, string value)
    {
        if (analyticsToken != null)
        {
            string url = "https://api.brew-connect.com/v1/online/update_analytics";
            string json = "{" +
                "\"analytics_token\": \"" + analyticsToken + "\"," +
                "\"project_token\": \"" + projectToken + "\"," +
                "\"category_name\": \"game\"," +
                "\"analytics_entries\": [" +
                    "{" +
                        "\"name\": \"" + key + "\"," +
                        "\"value\": \"" + value + "\"" +
                    "}" +
                "]" +
            "}";
            byte[] post = System.Text.Encoding.UTF8.GetBytes(json);

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("content-Type", "application/json");

            using (WWW www = new WWW(url, post, headers))
            {
                yield return www;

                string jsonResponse = www.text;
                UpdateAnalyticsResponse response = JsonUtility.FromJson<UpdateAnalyticsResponse>(jsonResponse);

                Debug.Log(response.message[0]);
            }
        }
    }

    private int StatusCode(WWW www)
    {
        string statusLine;
        if (www.responseHeaders.TryGetValue("STATUS", out statusLine))
        {
            string[] parts = statusLine.Split(' ');
            int statusCode;
            if (parts.Length > 1 && int.TryParse(parts[1], out statusCode))
            {
                return statusCode;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }

    private string GetAccountName()
    {
        string username = WiiU.Core.accountName;

        if (username == "" || username == "<WiiU_AccountName>")
        {
            return "Unknown";
        }
        else
        {
            return username;
        }
    }

    private string GetVersion()
    {
        TextAsset versionAsset = Resources.Load<TextAsset>("Meta/version");
        
        return versionAsset.text;
    }

    public string GetLanguage()
    {
        string language = SaveManager.LoadLanguage();

        if (language == null)
        {
            language = "en";
        }

        return language.ToUpper();
    }

    public void ShareAnalytics(bool share)
    {
        menuManager.CloseCurrentPopup();

        saveManager.SaveShareAnalytics(share ? 1 : 0);
        bool saveResult = saveGameState.DoSave();

        if (share)
        {
            StartCoroutine(SendAnalytics());
        }
    }
}
