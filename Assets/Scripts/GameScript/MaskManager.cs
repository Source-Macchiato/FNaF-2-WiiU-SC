using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class MaskManager : MonoBehaviour
{
    public Animator maskAnimator;
    public AudioSource putMaskOnAudio;
    public AudioSource putMaskOffAudio;
    public AudioSource deepBreathsAudio;

    public bool maskPreviousState = false;

    public bool isMaskActive = false;
    public bool isMaskLocked = false;

    // References to WiiU controllers
    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    // Scripts
    NightPlayer nightPlayer;
    MonitorManager monitorManager;

    void Start()
	{
        // Access the WiiU GamePad and Remote
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

        // Get scripts
        nightPlayer = FindObjectOfType<NightPlayer>();
        monitorManager = FindObjectOfType<MonitorManager>();
    }

    void Update()
	{
        isMaskLocked = nightPlayer.BlackoutPrepared && isMaskActive;

        // Get the current state of the GamePad and Remote
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        if (gamePadState.gamePadErr == WiiU.GamePadError.None)
        {
            if (gamePadState.IsTriggered(WiiU.GamePadButton.R))
            {
                ToggleMask();
            }
        }

        switch (remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.R))
                {
                    ToggleMask();
                }
                break;
            case WiiU.RemoteDevType.Classic:
                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.R))
                {
                    ToggleMask();
                }
                break;
            default:
                if (remoteState.IsTriggered(WiiU.RemoteButton.Two))
                {
                    ToggleMask();
                }
                break;
        }

        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ToggleMask();
            }
        }

        if (nightPlayer.isJumpscared && isMaskActive)
        {
            DisableMask();
        }
    }

    private void ToggleMask()
    {
        if (!nightPlayer.isJumpscared)
        {
            if (isMaskActive)
            {
                if (!isMaskLocked)
                {
                    DisableMask();
                }
            }
            else
            {
                if (!monitorManager.isMonitorActive && !monitorManager.isToggling)
                {
                    EnableMask();
                }
            }
        }
    }

    private void EnableMask()
    {
        isMaskActive = true;
        maskPreviousState = false;
        maskAnimator.Play("On");
        putMaskOnAudio.Play();

        // If mask of audio is playing stop it
        if (putMaskOffAudio.isPlaying)
        {
            putMaskOffAudio.Stop();
        }

        // Start deep breaths when mask is put on
        if (!deepBreathsAudio.isPlaying)
        {
            deepBreathsAudio.Play();
        }
    }

    private void DisableMask()
    {
        isMaskActive = false;
        maskPreviousState = true;
        maskAnimator.Play("Off");
        putMaskOffAudio.Play();

        // If mask on audio is playing stop it
        if (putMaskOnAudio.isPlaying)
        {
            putMaskOnAudio.Stop();
        }

        // Stop deep breaths when mask is removed
        if (deepBreathsAudio.isPlaying)
        {
            deepBreathsAudio.Stop();
        }
    }
}