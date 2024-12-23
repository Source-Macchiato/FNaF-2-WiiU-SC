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
    public bool cameraLightEnabled = false;
    public float flashlightDuration = 115f;
    public float currentFlashlightDuration;
    
    private bool activateLight = false;

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

        currentFlashlightDuration = flashlightDuration;
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
                activateLight = true;
            }
            else
            {
                activateLight = false;
            }
        }

        switch (remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                if (remoteState.pro.IsPressed(WiiU.ProControllerButton.A))
                {
                    activateLight = true;
                }
                else
                {
                    activateLight = false;
                }
                break;
            case WiiU.RemoteDevType.Classic:
                if (remoteState.classic.IsPressed(WiiU.ClassicButton.A))
                {
                    activateLight = true;
                }
                else
                {
                    activateLight = false;
                }
                break;
            default:
                if (remoteState.IsPressed(WiiU.RemoteButton.A))
                {
                    activateLight = true;
                }
                else
                {
                    activateLight = false;
                }
                break;
        }

        if (Application.isEditor)
        {
            if (Input.GetKey(KeyCode.A))
            {
                activateLight = true;
            }
            else
            {
                activateLight = false;
            }
        }

        ToggleLight();
        CurrentLightPosition();
    }

    private void ToggleLight()
    {
        if (!nightPlayer.isJumpscared && activateLight && !maskManager.isMaskActive)
        {
            if (!leftLightEnabled && !centerLightEnabled && !rightLightEnabled && !cameraLightEnabled)
            {
                DisableLight();
            }
            else
            {
                EnableLight();
            }
        }
        else
        {
            DisableLight();
        }
    }

    private void EnableLight()
    {
        isLightActive = true;

        // Light buzz audio
        if (!buzzLightAudio.isPlaying)
        {
            buzzLightAudio.Play();
        }

        if (centerLightEnabled || cameraLightEnabled)
        {
            currentFlashlightDuration -= Time.deltaTime;
        }
    }

    private void DisableLight()
    {
        isLightActive = false;

        if (buzzLightAudio.isPlaying)
        {
            buzzLightAudio.Stop();
        }
    }

    private void CurrentLightPosition()
    {
        if (nightPlayer.isMonitorActive)
        {
            leftLightEnabled = false;
            centerLightEnabled = false;
            rightLightEnabled = false;
            cameraLightEnabled = true;
        }
        else
        {
            float positionX = officeRect.localPosition.x;

            if (positionX <= (moveInOffice.leftEdge - 160) && positionX >= (moveInOffice.rightEdge + 160)) // Center
            {
                if (currentFlashlightDuration >= 0.01)
                {
                    leftLightEnabled = false;
                    centerLightEnabled = true;
                    rightLightEnabled = false;
                    cameraLightEnabled = false;
                }
                else
                {
                    leftLightEnabled = false;
                    centerLightEnabled = false;
                    rightLightEnabled = false;
                    cameraLightEnabled = false;
                }
            }
            else if (positionX > (moveInOffice.leftEdge - 160)) // Left
            {
                leftLightEnabled = true;
                centerLightEnabled = false;
                rightLightEnabled = false;
                cameraLightEnabled = false;
            }
            else if (positionX < (moveInOffice.rightEdge + 160)) // Right
            {
                leftLightEnabled = false;
                centerLightEnabled = false;
                rightLightEnabled = true;
                cameraLightEnabled = false;
            }
        }
    }
}