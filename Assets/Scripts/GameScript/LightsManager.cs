using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class LightsManager : MonoBehaviour
{
    public AudioSource buzzLightAudio;

    public bool isLightActive = false;

    // References to WiiU controllers
    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    // Scripts
    NightPlayer nightPlayer;

    void Start()
	{
        // Access the WiiU GamePad and Remote
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

        // Get scripts
        nightPlayer = FindObjectOfType<NightPlayer>();
    }
	
	void Update()
	{
        // Get the current state of the GamePad and Remote
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        if (gamePadState.gamePadErr == WiiU.GamePadError.None)
        {
            if (gamePadState.IsPressed(WiiU.GamePadButton.A))
            {
                isLightActive = true;
            }
            else
            {
                isLightActive = false;
            }
        }

        switch (remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                if (remoteState.pro.IsPressed(WiiU.ProControllerButton.A))
                {
                    isLightActive = true;
                }
                else
                {
                    isLightActive = false;
                }
                break;
            case WiiU.RemoteDevType.Classic:
                if (remoteState.classic.IsPressed(WiiU.ClassicButton.A))
                {
                    isLightActive = true;
                }
                else
                {
                    isLightActive = false;
                }
                break;
            default:
                if (remoteState.IsPressed(WiiU.RemoteButton.A))
                {
                    isLightActive = true;
                }
                else
                {
                    isLightActive = false;
                }
                break;
        }

        if (Application.isEditor)
        {
            if (Input.GetKey(KeyCode.A))
            {
                isLightActive = true;
            }
            else
            {
                isLightActive = false;
            }
        }

        ToggleLight();
    }

    private void ToggleLight()
    {
        if (!nightPlayer.isJumpscared)
        {
            if (isLightActive)
            {
                EnableLight();
            }
            else
            {
                DisableLight();
            }
        }
        else
        {
            DisableLight();
        }
    }

    private void EnableLight()
    {
        if (!buzzLightAudio.isPlaying)
        {
            buzzLightAudio.Play();
        }
    }

    private void DisableLight()
    {
        if (buzzLightAudio.isPlaying)
        {
            buzzLightAudio.Stop();
        }
    }
}
