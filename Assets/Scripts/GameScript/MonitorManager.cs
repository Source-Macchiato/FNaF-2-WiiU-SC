using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class MonitorManager : MonoBehaviour
{
    // References to WiiU controllers
    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    void Start()
	{
        // Access the WiiU GamePad and Remote
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);
    }
	
	void Update()
	{
        // Get the current state of the GamePad and Remote
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        if (gamePadState.gamePadErr == WiiU.GamePadError.None)
        {
            if (gamePadState.IsTriggered(WiiU.GamePadButton.L))
            {

            }
        }

        switch (remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.L))
                {

                }
                break;
            case WiiU.RemoteDevType.Classic:
                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.L))
                {

                }
                break;
            default:
                if (remoteState.IsTriggered(WiiU.RemoteButton.One))
                {

                }
                break;
        }

        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {

            }
        }
    }
}
