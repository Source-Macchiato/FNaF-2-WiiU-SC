using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class MaskManager : MonoBehaviour
{
    public Animator maskAnimator;
    public AudioSource putMaskOnAudio;
    public AudioSource putMaskOffAudio;
    public AudioSource deepBreathsAudio;

    public bool isMaskActive = false;

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

        nightPlayer = FindObjectOfType<NightPlayer>();
    }
	
	void Update()
	{
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
                DisableMask();
            }
            else
            {
                if (!nightPlayer.isMonitorActive)
                {
                    EnableMask();
                }
            }
        }
    }

    private void EnableMask()
    {
        isMaskActive = true;

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
