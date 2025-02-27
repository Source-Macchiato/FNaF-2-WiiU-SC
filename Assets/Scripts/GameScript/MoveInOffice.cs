using System;
using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class MoveInOffice : MonoBehaviour
{
    public GameObject OfficeContainer;
    public RectTransform pointerCursor;

    [HideInInspector]
    public bool camIsUp = false;
    public bool canMove = true;

    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    public float speed = 8f;
    public float leftEdge = 320f;
    public float rightEdge = -320f;
    private float stickDeadzone = 0.19f;
    public bool allowMouseMove = true;
    private bool canUseMotionControls = true;
    private bool isPointerDisplayed = true;
    private Vector3 lastMousePosition;

    void Start()
	{
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

        canUseMotionControls = SaveManager.LoadMotionControls();
        isPointerDisplayed = SaveManager.LoadPointerVisibility();

        lastMousePosition = Input.mousePosition;
    }
	
	void Update()
	{
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        // Gamepad
        if (gamePadState.gamePadErr == WiiU.GamePadError.None)
        {
            Vector2 leftStickGamepad = gamePadState.lStick;

            if (Mathf.Abs(leftStickGamepad.x) > stickDeadzone)
            {
                if (leftStickGamepad.x < 0)
                {
                    MoveLeft();
                }
                else
                {
                    MoveRight();
                }
            }

            if (gamePadState.IsPressed(WiiU.GamePadButton.Left))
            {
                MoveLeft();
            }
            else if (gamePadState.IsPressed(WiiU.GamePadButton.Right))
            {
                MoveRight();
            }

            if (IsGamepadInputTriggered(gamePadState))
            {
                pointerCursor.gameObject.SetActive(false);
            }
        }

        // Remotes
        switch (remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                Vector2 leftStickProController = remoteState.pro.leftStick;

                if (Mathf.Abs(leftStickProController.x) > stickDeadzone)
                {
                    if (leftStickProController.x < 0)
                    {
                        MoveLeft();
                    }
                    else
                    {
                        MoveRight();
                    }
                }

                if (remoteState.pro.IsPressed(WiiU.ProControllerButton.Left))
                {
                    MoveLeft();
                }
                else if (remoteState.pro.IsPressed(WiiU.ProControllerButton.Right))
                {
                    MoveRight();
                }

                if (IsProInputTriggered(remoteState))
                {
                    pointerCursor.gameObject.SetActive(false);
                }
                break;
            case WiiU.RemoteDevType.Classic:
                Vector2 leftStickClassicController = remoteState.classic.leftStick;

                if (Mathf.Abs(leftStickClassicController.x) > stickDeadzone)
                {
                    if (leftStickClassicController.x < 0)
                    {
                        MoveLeft();
                    }
                    else
                    {
                        MoveRight();
                    }
                }

                if (remoteState.classic.IsPressed(WiiU.ClassicButton.Left))
                {
                    MoveLeft();
                }
                else if (remoteState.classic.IsPressed(WiiU.ClassicButton.Right))
                {
                    MoveRight();
                }

                if (IsClassicInputTriggered(remoteState))
                {
                    pointerCursor.gameObject.SetActive(false);
                }
                break;
            default:
                Vector2 stickNunchuk = remoteState.nunchuk.stick;

                if (Mathf.Abs(stickNunchuk.x) > stickDeadzone)
                {
                    if (stickNunchuk.x < 0)
                    {
                        MoveLeft();
                    }
                    else
                    {
                        MoveRight();
                    }
                }

                // Pointer
                if (canUseMotionControls)
                {
                    Vector2 pointerPosition = remoteState.pos;
                    pointerPosition.x = ((pointerPosition.x + 1.0f) / 2.0f) * WiiU.Core.GetScreenWidth(WiiU.DisplayIndex.TV);
                    pointerPosition.y = WiiU.Core.GetScreenHeight(WiiU.DisplayIndex.TV) - ((pointerPosition.y + 1.0f) / 2.0f) * WiiU.Core.GetScreenHeight(WiiU.DisplayIndex.TV);

                    if (pointerPosition.x < 250f)
                    {
                        MoveLeft();
                    }

                    if (pointerPosition.x > WiiU.Core.GetScreenWidth(WiiU.DisplayIndex.TV) - 250f)
                    {
                        MoveRight();
                    }

                    if (isPointerDisplayed)
                    {
                        if (IsRemoteInputTriggered(remoteState))
                        {
                            pointerCursor.gameObject.SetActive(true);
                        }

                        if (pointerCursor != null && pointerCursor.gameObject.activeSelf)
                        {
                            pointerCursor.anchoredPosition = pointerPosition;
                        }
                    }
                    else
                    {
                        pointerCursor.gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (remoteState.IsPressed(WiiU.RemoteButton.Left))
                    {
                        MoveLeft();
                    }
                    else if (remoteState.IsPressed(WiiU.RemoteButton.Right))
                    {
                        MoveRight();
                    }
                }
                break;
        }

        // Keyboard
        if (Application.isEditor)
        {
            if (Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x < 300f && allowMouseMove)
            {
                MoveLeft();
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x > WiiU.Core.GetScreenWidth(WiiU.DisplayIndex.TV) - 300f && allowMouseMove)
            {
                MoveRight();
            }

            if (Input.anyKeyDown || Input.mousePosition != lastMousePosition)
            {
                pointerCursor.gameObject.SetActive(false);

                lastMousePosition = Input.mousePosition;
            }
        }
    }

    private void MoveLeft()
    {
        if (!camIsUp && canMove)
        {
            if (OfficeContainer.transform.localPosition.x <= leftEdge)
            {
                OfficeContainer.transform.Translate(Vector3.right * speed * Time.deltaTime);
            }
        }
    }

    private void MoveRight()
    {
        if (!camIsUp && canMove)
        {
            if (OfficeContainer.transform.localPosition.x >= rightEdge)
            {
                OfficeContainer.transform.Translate(Vector3.left * speed * Time.deltaTime);
            }
        }
    }

    // Functions for check if any input is triggered
    private bool IsGamepadInputTriggered(WiiU.GamePadState gamePadState)
    {
        foreach (WiiU.GamePadButton button in Enum.GetValues(typeof(WiiU.GamePadButton)))
        {
            if (gamePadState.IsTriggered(button))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsProInputTriggered(WiiU.RemoteState remoteState)
    {
        foreach (WiiU.ProControllerButton button in Enum.GetValues(typeof(WiiU.ProControllerButton)))
        {
            if (remoteState.pro.IsTriggered(button))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsClassicInputTriggered(WiiU.RemoteState remoteState)
    {
        foreach (WiiU.ClassicButton button in Enum.GetValues(typeof(WiiU.ClassicButton)))
        {
            if (remoteState.classic.IsTriggered(button))
            {
                return true;
            }
        }
        return false;
    }

    private bool IsRemoteInputTriggered(WiiU.RemoteState remoteState)
    {
        foreach (WiiU.RemoteButton button in Enum.GetValues(typeof(WiiU.RemoteButton)))
        {
            if (remoteState.IsTriggered(button))
            {
                return true;
            }
        }
        return false;
    }
}