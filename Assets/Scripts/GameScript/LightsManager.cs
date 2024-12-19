using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class LightsManager : MonoBehaviour
{
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
                
            }
        }

        switch (remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                if (remoteState.pro.IsPressed(WiiU.ProControllerButton.A))
                {

                }
                break;
            case WiiU.RemoteDevType.Classic:
                if (remoteState.classic.IsPressed(WiiU.ClassicButton.A))
                {

                }
                break;
            default:
                if (remoteState.IsPressed(WiiU.RemoteButton.A))
                {

                }
                break;
        }
    }

    private void ToggleLights()
    {
        if (!nightPlayer.isJumpscared)
        {

        }
    }
}
