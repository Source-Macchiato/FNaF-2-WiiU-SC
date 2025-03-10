﻿using UnityEngine;
using WiiU = UnityEngine.WiiU;
using UnityEngine.SceneManagement;

public class UnlockAllStars : MonoBehaviour
{
    // References to WiiU controllers
    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    // Scripts
    SaveGameState saveGameState;
    SaveManager saveManager;

    private WiiU.GamePadButton[] unlockCodeGamepad =
    {
        WiiU.GamePadButton.L, WiiU.GamePadButton.L, WiiU.GamePadButton.R, WiiU.GamePadButton.R,
        WiiU.GamePadButton.Left, WiiU.GamePadButton.Right, WiiU.GamePadButton.A, WiiU.GamePadButton.B,
        WiiU.GamePadButton.Y, WiiU.GamePadButton.X
    };

    private WiiU.ProControllerButton[] unlockCodeProController =
    {
        WiiU.ProControllerButton.L, WiiU.ProControllerButton.L, WiiU.ProControllerButton.R, WiiU.ProControllerButton.R,
        WiiU.ProControllerButton.Left, WiiU.ProControllerButton.Right, WiiU.ProControllerButton.A,
        WiiU.ProControllerButton.B, WiiU.ProControllerButton.Y, WiiU.ProControllerButton.X
    };

    private WiiU.ClassicButton[] unlockCodeClassicController =
    {
        WiiU.ClassicButton.L, WiiU.ClassicButton.L, WiiU.ClassicButton.R, WiiU.ClassicButton.R,
        WiiU.ClassicButton.Left, WiiU.ClassicButton.Right, WiiU.ClassicButton.A, WiiU.ClassicButton.B,
        WiiU.ClassicButton.Y, WiiU.ClassicButton.X
    };

    private WiiU.RemoteButton[] unlockCodeRemote =
    {
        WiiU.RemoteButton.Minus, WiiU.RemoteButton.Minus, WiiU.RemoteButton.Plus, WiiU.RemoteButton.Plus,
        WiiU.RemoteButton.Left, WiiU.RemoteButton.Right, WiiU.RemoteButton.A, WiiU.RemoteButton.B,
        WiiU.RemoteButton.NunchukC, WiiU.RemoteButton.NunchukZ
    };

    private KeyCode[] unlockCodePC =
    {
        KeyCode.L, KeyCode.L, KeyCode.R, KeyCode.R, KeyCode.LeftArrow,
        KeyCode.RightArrow, KeyCode.A, KeyCode.B, KeyCode.Y, KeyCode.X
    };

    private int unlockIndexGamepad = 0;
    private int unlockIndexProController = 0;
    private int unlockIndexClassicController = 0;
    private int unlockIndexRemote = 0;
    private int unlockIndexPC = 0;

    void Start()
    {
        saveGameState = FindObjectOfType<SaveGameState>();
        saveManager = FindObjectOfType<SaveManager>();

        // Access the WiiU GamePad and Remote
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);
    }

    void Update()
    {
        // Get the current state of the GamePad and Remote
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        // Gamepad unlock code combo
        if (gamePadState.IsTriggered(unlockCodeGamepad[unlockIndexGamepad]))
        {
            unlockIndexGamepad++;

            if (unlockIndexGamepad == unlockCodeGamepad.Length)
            {
                Debug.Log("Unlock code activated with Gamepad !");

                UnlockStars();

                unlockIndexGamepad = 0;
            }
        }
        else if (Input.anyKeyDown)
        {
            unlockIndexGamepad = 0;
        }

        // Remote unlock code combo
        switch(remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                if (remoteState.pro.IsTriggered(unlockCodeProController[unlockIndexProController]))
                {
                    unlockIndexProController++;

                    if (unlockIndexProController == unlockCodeProController.Length)
                    {
                        Debug.Log("Unlock code activated with Pro Controller !");

                        UnlockStars();

                        unlockIndexProController = 0;
                    }
                }
                else if (Input.anyKeyDown)
                {
                    unlockIndexProController = 0;
                }
                break;
            case WiiU.RemoteDevType.Classic:
                if (remoteState.classic.IsTriggered(unlockCodeClassicController[unlockIndexClassicController]))
                {
                    unlockIndexClassicController++;

                    if (unlockIndexClassicController == unlockCodeClassicController.Length)
                    {
                        Debug.Log("Unlock code activated with Classic Controller");

                        UnlockStars();

                        unlockIndexClassicController = 0;
                    }
                }
                else if (Input.anyKeyDown)
                {
                    unlockIndexClassicController = 0;
                }
                break;
            default:
                if (remoteState.IsTriggered(unlockCodeRemote[unlockIndexRemote]))
                {
                    unlockIndexRemote++;

                    if (unlockIndexRemote == unlockCodeRemote.Length)
                    {
                        Debug.Log("Unlock code activated with Wiimote");

                        UnlockStars();

                        unlockIndexRemote = 0;
                    }
                }
                else if (Input.anyKeyDown)
                {
                    unlockIndexRemote = 0;
                }
                break;
        }

        // Keyboard unlock code combo
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(unlockCodePC[unlockIndexPC]))
            {
                unlockIndexPC++;

                if (unlockIndexPC == unlockCodePC.Length)
                {
                    Debug.Log("Unlock code activated with keyboard !");

                    UnlockStars();

                    unlockIndexPC = 0;
                }
            }
            else if (Input.anyKeyDown)
            {
                unlockIndexPC = 0;
            }
        }
    }

    void UnlockStars()
    {
        for (int i = 0; i < 3; i++)
        {
            saveManager.SaveUnlockedStars(i, true);
        }

        bool saveResult = saveGameState.DoSave();

        SceneManager.LoadScene("MainMenu");
    }
}
