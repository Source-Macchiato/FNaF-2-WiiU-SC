using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedBeta : MonoBehaviour
{
	public string id;
	public string password;
	private string projectToken = "9637629c27bb7871e9fa3bbe294cf09153b8be5831caa03ab935fb098928ee9b";

	private MenuManager menuManager;

    [Serializable]
	private class AuthResponse
	{
        public AuthData data;
    }

    [Serializable]
    private class AuthData
    {
        public string token;
    }

    void Start()
	{
		menuManager = FindObjectOfType<MenuManager>();

		menuManager.AddPopup(2);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.I))
		{
			StartCoroutine(LogIn(id, password));
		}
	}

	IEnumerator LogIn(string id, string password)
	{
		string url = "https://api.brew-connect.com/v1/account/login";
		string json = "{\"id\":\"" + id + "\",\"password\":\"" + password + "\"}";
		byte[] post = System.Text.Encoding.UTF8.GetBytes(json);

		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("Content-Type", "application/json");

		using (WWW www = new WWW(url, post, headers))
		{
			yield return www;

			if (string.IsNullOrEmpty(www.error))
			{
				string jsonResponse = www.text;
				AuthResponse response = JsonUtility.FromJson<AuthResponse>(jsonResponse);

				StartCoroutine(IsTester(response.data.token, projectToken));
			}
			else
			{
				Debug.Log("Login error: " + www.error);
			}	
		}
	}

	IEnumerator IsTester(string userToken, string projectToken)
	{
		string url = "https://api.brew-connect.com/v1/online/is_tester";
		string json = "{\"user_token\":\"" + userToken + "\",\"project_token\":\"" + projectToken + "\"}";
		byte[] post = System.Text.Encoding.UTF8.GetBytes(json);

		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("content-Type", "application/json");

		using (WWW www = new WWW(url, post, headers))
		{
			yield return www;

			Debug.Log(StatusCode(www));
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