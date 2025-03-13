using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class BearMovement : MonoBehaviour
{
    public Vector2 playerDirection;
    public bool isMoving;

    // References to WiiU controllers
    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    void Start()
	{
        // Access the WiiU GamePad and Remote
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);
    }
	
	void Update()
	{
        // Get the current state of the GamePad and Remote
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        playerDirection = Vector2.zero;
        isMoving = false;

        if (gamePadState.gamePadErr == WiiU.GamePadError.None)
        {
            Vector2 leftStickGamepad = gamePadState.lStick;

            if (leftStickGamepad.magnitude > 0.1f)
            {
                playerDirection = leftStickGamepad;

                isMoving = true;
            }

            if (gamePadState.IsPressed(WiiU.GamePadButton.Up))
            {
                playerDirection.y += 1;

                isMoving = true;
            }
            if (gamePadState.IsPressed(WiiU.GamePadButton.Left))
            {
                playerDirection.x -= 1;

                isMoving = true;
            }
            if (gamePadState.IsPressed(WiiU.GamePadButton.Down))
            {
                playerDirection.y -= 1;

                isMoving = true;
            }
            if (gamePadState.IsPressed(WiiU.GamePadButton.Right))
            {
                playerDirection.x += 1;

                isMoving = true;
            }
        }

        switch (remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                Vector2 leftStickProController = remoteState.pro.leftStick;

                if (leftStickProController.magnitude > 0.1f)
                {
                    playerDirection = leftStickProController;

                    isMoving = true;
                }

                if (remoteState.pro.IsPressed(WiiU.ProControllerButton.Up))
                {
                    playerDirection.y += 1;

                    isMoving = true;
                }
                if (remoteState.pro.IsPressed(WiiU.ProControllerButton.Left))
                {
                    playerDirection.x -= 1;

                    isMoving = true;
                }
                if (remoteState.pro.IsPressed(WiiU.ProControllerButton.Down))
                {
                    playerDirection.y -= 1;

                    isMoving = true;
                }
                if (remoteState.pro.IsPressed(WiiU.ProControllerButton.Right))
                {
                    playerDirection.x += 1;

                    isMoving = true;
                }
                break;
            case WiiU.RemoteDevType.Classic:
                Vector2 leftStickClassicController = remoteState.classic.leftStick;

                if (leftStickClassicController.magnitude > 0.1f)
                {
                    playerDirection = leftStickClassicController;

                    isMoving = true;
                }

                if (remoteState.classic.IsPressed(WiiU.ClassicButton.Up))
                {
                    playerDirection.y += 1;

                    isMoving = true;
                }
                if (remoteState.classic.IsPressed(WiiU.ClassicButton.Left))
                {
                    playerDirection.x -= 1;

                    isMoving = true;
                }
                if (remoteState.classic.IsPressed(WiiU.ClassicButton.Down))
                {
                    playerDirection.y -= 1;

                    isMoving = true;
                }
                if (remoteState.classic.IsPressed(WiiU.ClassicButton.Right))
                {
                    playerDirection.x += 1;

                    isMoving = true;
                }
                break;
            default:
                Vector2 stickNunchuk = remoteState.nunchuk.stick;

                if (stickNunchuk.magnitude > 0.1f)
                {
                    playerDirection = stickNunchuk;

                    isMoving = true;
                }

                if (remoteState.IsPressed(WiiU.RemoteButton.Up))
                {
                    playerDirection.y += 1;

                    isMoving = true;
                }
                if (remoteState.IsPressed(WiiU.RemoteButton.Left))
                {
                    playerDirection.x -= 1;

                    isMoving = true;
                }
                if (remoteState.IsPressed(WiiU.RemoteButton.Down))
                {
                    playerDirection.y -= 1;

                    isMoving = true;
                }
                if (remoteState.IsPressed(WiiU.RemoteButton.Right))
                {
                    playerDirection.x += 1;

                    isMoving = true;
                }
                break;
        }

        if (Application.isEditor)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                playerDirection.y += 1;

                isMoving = true;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                playerDirection.x -= 1;

                isMoving = true;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                playerDirection.y -= 1;

                isMoving = true;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                playerDirection.x += 1;

                isMoving = true;
            }
        }
    }
}