using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class ClosedBeta : MonoBehaviour
{
	private string projectToken = "9637629c27bb7871e9fa3bbe294cf09153b8be5831caa03ab935fb098928ee9b";

	private MenuManager menuManager;

    // References to WiiU controllers
    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    [Serializable]
	private class AuthResponse
	{
		public string[] message;
        public AuthData data;
    }

    [Serializable]
    private class AuthData
    {
        public string token;
    }

    void Start()
	{
        // Access the WiiU GamePad and Remote
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

        menuManager = FindObjectOfType<MenuManager>();

		menuManager.AddPopup(2);
	}

	void Update()
	{
        // Get the current state of the GamePad and Remote
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        // Handle GamePad input
		if (gamePadState.gamePadErr == WiiU.GamePadError.None)
		{
			// Is triggered
			if (gamePadState.IsTriggered(WiiU.GamePadButton.Plus))
			{
				SendRequest();
			}
		}

        // Handle Remote input based on the device type
		switch(remoteState.devType)
		{
			case WiiU.RemoteDevType.ProController:
				// Is triggered
				if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Plus))
				{
					SendRequest();
				}
				break;
			case WiiU.RemoteDevType.Classic:
				// Is triggered
				if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Plus))
				{
					SendRequest();
				}
				break;
			default:
				// Is triggered
				if (remoteState.IsTriggered(WiiU.RemoteButton.Plus))
				{
					SendRequest();
				}
				break;
		}

        if (Application.isEditor)
		{
            if (Input.GetKeyDown(KeyCode.Return))
            {
				SendRequest();
            }
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

            string jsonResponse = www.text;
            AuthResponse response = JsonUtility.FromJson<AuthResponse>(jsonResponse);

            Debug.Log(response.message[0]);

            if (StatusCode(www) == 200)
			{
                StartCoroutine(IsTester(response.data.token, projectToken));
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

			if (StatusCode(www) == 200)
			{
				menuManager.CloseCurrentPopup();
			}
			else if (StatusCode(www) == 403)
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

	private void SendRequest()
	{
		if (menuManager.currentPopup != null && menuManager.currentPopup.actionType == 2)
		{
			GameObject inputFieldsContainer = menuManager.currentPopup.popupObject.transform.Find("PopupInputFields").gameObject;
			TMP_InputField idInputField = inputFieldsContainer.transform.GetChild(0).GetComponent<TMP_InputField>();
			TMP_InputField passwordInputField = inputFieldsContainer.transform.GetChild(1).GetComponent<TMP_InputField>();

            StartCoroutine(LogIn(idInputField.text, passwordInputField.text));
        }
    }
}