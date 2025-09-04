using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedBeta : MonoBehaviour
{
    [Header("BrewConnect")]
    [SerializeField] private string projectToken;

    private MenuManager menuManager;

    void Awake()
    {
        menuManager = FindObjectOfType<MenuManager>();

        if (!Application.isEditor)
        {
            menuManager.AddPopup(3);
        }
    }

    void Start()
    {
        if (!Application.isEditor)
        {
            StartCoroutine(IsTester(SaveManager.token, projectToken));
        }
    }

    IEnumerator IsTester(string userToken, string projectToken)
    {
        string url = "https://api.brew-connect.com/v1/online/is_tester";
        string json = "{\"user_token\":\"" + userToken + "\",\"project_token\":\"" + projectToken + "\"}";
        byte[] post = Encoding.UTF8.GetBytes(json);

        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("content-Type", "application/json");

        using (WWW www = new WWW(url, post, headers))
        {
            yield return www;

            if (StatusCode(www) == 200)
            {
                menuManager.CloseCurrentPopup();
            }
            else
            {
                Application.Quit();
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