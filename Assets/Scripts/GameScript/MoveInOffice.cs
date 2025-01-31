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

    void Start()
	{
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

        canUseMotionControls = SaveManager.LoadMotionControls();
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

                    if (pointerPosition.x < 300f)
                    {
                        MoveLeft();
                    }

                    if (pointerPosition.x > WiiU.Core.GetScreenWidth(WiiU.DisplayIndex.TV) - 300f)
                    {
                        MoveRight();
                    }

                    if (pointerCursor != null)
                    {
                        pointerCursor.anchoredPosition = pointerPosition;
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
}
