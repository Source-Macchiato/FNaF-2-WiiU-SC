using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class LightsManager : MonoBehaviour
{
    public AudioSource buzzLightAudio;

    public RectTransform officeRect;

    public bool isLightActive = false;
    public bool leftLightEnabled = false;
    public bool centerLightEnabled = false;
    public bool rightLightEnabled = false;

    // References to WiiU controllers
    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    // Scripts
    NightPlayer nightPlayer;
    MaskManager maskManager;
    MoveInOffice moveInOffice;

    void Start()
	{
        // Access the WiiU GamePad and Remote
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

        // Get scripts
        nightPlayer = FindObjectOfType<NightPlayer>();
        maskManager = FindObjectOfType<MaskManager>();
        moveInOffice = FindObjectOfType<MoveInOffice>();
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
        CurrentLightPosition();
    }

    private void ToggleLight()
    {
        if (!nightPlayer.isJumpscared)
        {
            if (isLightActive && !nightPlayer.isMonitorActive && !maskManager.isMaskActive)
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
        // Light buzz audio
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

    private void CurrentLightPosition()
    {
        float positionX = officeRect.localPosition.x;

        if (positionX <= (moveInOffice.leftEdge - 160) && positionX >= (moveInOffice.rightEdge + 160)) // Center
        {
            if (leftLightEnabled)
            {
                leftLightEnabled = false;
            }

            if (!centerLightEnabled)
            {
                centerLightEnabled = true;   
            }

            if (rightLightEnabled)
            {
                rightLightEnabled = false;
            }
        }
        else if (positionX > (moveInOffice.leftEdge - 160)) // Left
        {
            if (!leftLightEnabled)
            {
                leftLightEnabled = true;
            }
            
            if (centerLightEnabled)
            {
                centerLightEnabled = false;
            }

            if (rightLightEnabled)
            {
                rightLightEnabled = false;
            }
        }
        else if (positionX < (moveInOffice.rightEdge + 160)) // Right
        {
            if (leftLightEnabled)
            {
                leftLightEnabled = false;
            }

            if (centerLightEnabled)
            {
                centerLightEnabled = false;
            }

            if (!rightLightEnabled)
            {
                rightLightEnabled = true;
            }
        }
    }
}