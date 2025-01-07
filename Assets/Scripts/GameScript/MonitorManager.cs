﻿using System.Collections;
using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class MonitorManager : MonoBehaviour
{
    public GameObject minimap;
    public GameObject rec;
    public GameObject monitorContainer;
    public GameObject roomName;
    public GameObject blackBackground;

    public Animator monitorAnimator;

    public AudioSource monitorOnAudio;
    public AudioSource monitorOffAudio;

    public bool isMonitorActive = false;
    public bool isToggling = false;

    private int layoutId;

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

        layoutId = SaveManager.LoadLayoutId();

        // Elements to disable when the game starts
        minimap.SetActive(false);
        rec.SetActive(false);
        monitorContainer.SetActive(false);
        roomName.SetActive(false);
        blackBackground.SetActive(false);
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

        if (nightPlayer.isJumpscared && isMonitorActive || nightPlayer.BlackoutActive && isMonitorActive)
        {
            StartCoroutine(DisableMonitor());
        }
    }

    private void ToggleMonitor()
    {
        if (!nightPlayer.isJumpscared && !nightPlayer.BlackoutActive && !isToggling)
        {
            if (isMonitorActive)
            {
                StartCoroutine(DisableMonitor());

                nightPlayer.ActionsMonitorOff();
            }
            else
            {
                if (!maskManager.isMaskActive)
                {
                    StartCoroutine(EnableMonitor());

                    nightPlayer.ActionsMonitorOn();
                }
            }
        }
    }

    private IEnumerator EnableMonitor()
    {
        isToggling = true;

        monitorOnAudio.Play();

        monitorAnimator.Play("On");

        yield return new WaitForSeconds(0.233f);

        isMonitorActive = true;
        moveInOffice.canMove = false;

        monitorContainer.SetActive(true);
        minimap.SetActive(true);
        rec.SetActive(true);
        nightPlayer.JJ.SetActive(false);
        roomName.SetActive(true);
        if (layoutId != 2)
        {
            blackBackground.SetActive(true);
        }

        isToggling = false;
    }

    private IEnumerator DisableMonitor()
    {
        isToggling = true;

        monitorOffAudio.Play();

        minimap.SetActive(false);
        rec.SetActive(false);
        monitorContainer.SetActive(false);
        roomName.SetActive(false);
        blackBackground.SetActive(false);

        monitorAnimator.Play("Off");

        isMonitorActive = false;
        moveInOffice.canMove = true;

        yield return new WaitForSeconds(0.233f);

        isToggling = false;
    }
}
