using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class AnalyticsData : MonoBehaviour
{
    private string projectToken = "9637629c27bb7871e9fa3bbe294cf09153b8be5831caa03ab935fb098928ee9b";
    public string analyticsToken;

    [Serializable]
    private class AnalyticsResponse
    {
        public string[] message;
        public DataResponse data;
    }

    [Serializable]
    private class DataResponse
    {
        public string token;
    }

    void Start()
    {
        StartCoroutine(SendAnalytics());
    }

    public IEnumerator SendAnalytics()
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
            AnalyticsResponse response = JsonUtility.FromJson<AnalyticsResponse>(jsonResponse);

            Debug.Log(response.message[0]);

            if (StatusCode(www) == 201)
            {
                analyticsToken = response.data.token;
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

    private string GetLanguage()
    {
        string language = SaveManager.LoadLanguage();

        if (language == null)
        {
            language = "en";
        }

        return language.ToUpper();
    }
}
