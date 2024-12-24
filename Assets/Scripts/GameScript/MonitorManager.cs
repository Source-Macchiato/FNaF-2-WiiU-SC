using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class MonitorManager : MonoBehaviour
{
    public bool isMonitorActive = false;

    // References to WiiU controllers
    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    // Scripts
    NightPlayer nightPlayer;
    MaskManager maskManager;

    void Start()
	{
        // Access the WiiU GamePad and Remote
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

        // Get scripts
        nightPlayer = FindObjectOfType<NightPlayer>();
        maskManager = FindObjectOfType<MaskManager>();
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
                ToggleMonitor();
            }
        }

        switch (remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.L))
                {
                    ToggleMonitor();
                }
                break;
            case WiiU.RemoteDevType.Classic:
                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.L))
                {
                    ToggleMonitor();
                }
                break;
            default:
                if (remoteState.IsTriggered(WiiU.RemoteButton.One))
                {
                    ToggleMonitor();
                }
                break;
        }

        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                ToggleMonitor();
            }
        }

        if (nightPlayer.isJumpscared && isMonitorActive)
        {
            DisableMonitor();
        }
    }

    private void ToggleMonitor()
    {
        if (!nightPlayer.isJumpscared)
        {
            if (isMonitorActive)
            {
                DisableMonitor();
            }
            else
            {
                if (!maskManager.isMaskActive)
                {
                    EnableMonitor();
                }
            }
        }
    }

    private void EnableMonitor()
    {

    }

    private void DisableMonitor()
    {

    }
}
