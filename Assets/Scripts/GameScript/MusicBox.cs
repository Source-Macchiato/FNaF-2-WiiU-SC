using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using WiiU = UnityEngine.WiiU;

public class MusicBox : MonoBehaviour
{
	public Image progress;
    public AudioSource musicBoxTheme;
    public AudioSource windUpSound;
    public GameObject musicBoxContainer;

	private float unwindTime;
    private float currentUnwindTime;
    public bool windUpMusicBox = false;
    private bool isWindUpSoundPlaying = false;

    private int nightNumber;

    // References to WiiU controllers
    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    // Scripts
    private NightPlayer nightPlayer;

    void Start()
	{
        // Access the WiiU GamePad and Remote
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

        // Get scripts
        nightPlayer = FindObjectOfType<NightPlayer>();

        // Get night number
        nightNumber = SaveManager.LoadNightNumber();

        // Assign unwind time
        unwindTime = UnwindSpeed();
        currentUnwindTime = unwindTime;
    }
	
	void Update()
	{
        // Get the current state of the GamePad and Remote
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        if (gamePadState.gamePadErr == WiiU.GamePadError.None)
        {
            if (gamePadState.IsPressed(WiiU.GamePadButton.B))
            {
                windUpMusicBox = true;
            }
            else
            {
                windUpMusicBox = false;
            }
        }

        switch (remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                if (remoteState.pro.IsPressed(WiiU.ProControllerButton.B))
                {
                    windUpMusicBox = true;
                }
                else
                {
                    windUpMusicBox = false;
                }
                break;
            case WiiU.RemoteDevType.Classic:
                if (remoteState.classic.IsPressed(WiiU.ClassicButton.B))
                {
                    windUpMusicBox = true;
                }
                else
                {
                    windUpMusicBox = false;
                }
                break;
            default:
                if (remoteState.IsPressed(WiiU.RemoteButton.B))
                {
                    windUpMusicBox = true;
                }
                else
                {
                    windUpMusicBox = false;
                }
                break;
        }

        if (Application.isEditor)
        {
            if (Input.GetKey(KeyCode.B))
            {
                windUpMusicBox = true;
            }
            else
            {
                windUpMusicBox = false;
            }
        }

        WindUpMusicBox();
        UnwindMusicBox();
        UpdateProgressFill();
        HandleMuteWithMonitor();
        HandleMusicBoxVisibility();
        HandleWindUpSound();
    }

    private void WindUpMusicBox()
    {
        if (windUpMusicBox)
        {
            // Increase timer value
            currentUnwindTime += Time.deltaTime * 3f;
            currentUnwindTime = Mathf.Clamp(currentUnwindTime, 0f, unwindTime);
        }
    }

    private void UnwindMusicBox()
    {
        if (!windUpMusicBox)
        {
            // At night 1 if the time is not 2am or more the music box will not unwind
            if (nightNumber != 0 && nightPlayer.currentTime < 2)
            {
                currentUnwindTime -= Time.deltaTime;
                currentUnwindTime = Mathf.Clamp(currentUnwindTime, 0f, unwindTime);
            }
        }
    }

    private void UpdateProgressFill()
    {
        // Calculate unloading
        progress.fillAmount = currentUnwindTime / unwindTime;
    }

    private float UnwindSpeed()
    {
        if (nightNumber == 0 || nightNumber == 1) // Night 1 or 2
        {
            return 50f;
        }
        else if (nightNumber == 2) // Night 3
        {
            return 33.33f;
        }
        else if (nightNumber == 3) // Night 4
        {
            return 25f;
        }
        else if (nightNumber == 4) // Night 5
        {
            return 20f;
        }
        else if (nightNumber == 5 || nightNumber == 6) // Night 6 or 7
        {
            return 16.67f;
        }
        else
        {
            return 0f;
        }
    }

    private void HandleMuteWithMonitor()
    {
        if (nightPlayer.isMonitorActive)
        {
            if (musicBoxTheme.mute)
            {
                musicBoxTheme.mute = false;
            }
        }
        else
        {
            if (!musicBoxTheme.mute)
            {
                musicBoxTheme.mute = true;
            }
        }
    }

    private void HandleMusicBoxVisibility()
    {
        if (nightPlayer.isMonitorActive)
        {
            if (nightPlayer.currentCam == 11)
            {
                if (!musicBoxContainer.activeSelf)
                {
                    musicBoxContainer.SetActive(true);
                }
            }
            else
            {
                if (musicBoxContainer.activeSelf)
                {
                    musicBoxContainer.SetActive(false);
                }
            }
        }
        else
        {
            if (musicBoxContainer.activeSelf)
            {
                musicBoxContainer.SetActive(false);
            }
        }
    }

    private void HandleWindUpSound()
    {
        if (nightPlayer.isMonitorActive)
        {
            // Unmute sound when monitor is up
            if (windUpSound.mute)
            {
                windUpSound.mute = false;
            }

            if (nightPlayer.currentCam == 11)
            {
                if (windUpMusicBox && !isWindUpSoundPlaying)
                {
                    StartCoroutine(PlayWindUpSoundWithDelay());
                }
            }
        }
        else
        {
            // Mute sound when monitor is down
            if (!windUpSound.mute)
            {
                windUpSound.mute = true;
            }
        }
    }

    private IEnumerator PlayWindUpSoundWithDelay()
    {
        isWindUpSoundPlaying = true;

        // Jouer le son entièrement
        windUpSound.Play();

        // Attendre que le son soit terminé
        yield return new WaitForSeconds(windUpSound.clip.length);

        // Ajouter un délai avant de permettre un nouveau déclenchement
        yield return new WaitForSeconds(0.15f);

        isWindUpSoundPlaying = false;
    }

    public void ChangeMusicBoxVolume(float volume)
    {
        musicBoxTheme.volume = volume;
    }
}