using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionChecker : MonoBehaviour
{
    private string projectToken = "9637629c27bb7871e9fa3bbe294cf09153b8be5831caa03ab935fb098928ee9b";

    // Scripts
    private MenuManager menuManager;

    [Serializable]
    public class VersionData
    {
        public string version;
    }

    private void Start()
    {
        menuManager = FindObjectOfType<MenuManager>();

        StartCoroutine(CheckVersion());
    }

    IEnumerator CheckVersion()
    {
        string url = "https://api.brew-connect.com/v1/online/get_version";
        string json = "{\"project_token\":\"" + projectToken + "\"}";
        byte[] post = Encoding.UTF8.GetBytes(json);

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/json");

        using (WWW www = new WWW(url, post, headers))
        {
            yield return www;

            if (StatusCode(www) == 200)
            {
                VersionData data = JsonUtility.FromJson<VersionData>(www.text);
                string onlineVersion = data.version;

                TextAsset localVersionAsset = Resources.Load<TextAsset>("Meta/version");
                string localVersion = localVersionAsset.text;

                if (onlineVersion.Trim() == localVersion.Trim())
                {
                    Debug.Log("Same version number");
                }
                else
                {
                    menuManager.AddPopup(0);

                    Debug.Log("Different version number");
                }
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
}