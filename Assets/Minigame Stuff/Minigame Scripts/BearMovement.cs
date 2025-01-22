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

        isMoving = false;

        if (gamePadState.gamePadErr == WiiU.GamePadError.None)
        {
            if (gamePadState.IsPressed(WiiU.GamePadButton.Up))
            {
                playerDirection = Vector2.up;

                isMoving = true;
            }
            else if (gamePadState.IsPressed(WiiU.GamePadButton.Left))
            {
                playerDirection = Vector2.left;

                isMoving = true;
            }
            else if (gamePadState.IsPressed(WiiU.GamePadButton.Down))
            {
                playerDirection = Vector2.down;

                isMoving = true;
            }
            else if (gamePadState.IsPressed(WiiU.GamePadButton.Right))
            {
                playerDirection = Vector2.right;

                isMoving = true;
            }
        }

        switch (remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                if (remoteState.pro.IsPressed(WiiU.ProControllerButton.Up))
                {
                    playerDirection = Vector2.up;

                    isMoving = true;
                }
                else if (remoteState.pro.IsPressed(WiiU.ProControllerButton.Left))
                {
                    playerDirection = Vector2.left;

                    isMoving = true;
                }
                else if (remoteState.pro.IsPressed(WiiU.ProControllerButton.Down))
                {
                    playerDirection = Vector2.down;

                    isMoving = true;
                }
                else if (remoteState.pro.IsPressed(WiiU.ProControllerButton.Right))
                {
                    playerDirection = Vector2.right;

                    isMoving = true;
                }
                break;
            case WiiU.RemoteDevType.Classic:
                if (remoteState.classic.IsPressed(WiiU.ClassicButton.Up))
                {
                    playerDirection = Vector2.up;

                    isMoving = true;
                }
                else if (remoteState.classic.IsPressed(WiiU.ClassicButton.Left))
                {
                    playerDirection = Vector2.left;

                    isMoving = true;
                }
                else if (remoteState.classic.IsPressed(WiiU.ClassicButton.Down))
                {
                    playerDirection = Vector2.down;

                    isMoving = true;
                }
                else if (remoteState.classic.IsPressed(WiiU.ClassicButton.Right))
                {
                    playerDirection = Vector2.right;

                    isMoving = true;
                }
                break;
            default:
                if (remoteState.IsPressed(WiiU.RemoteButton.Up))
                {
                    playerDirection = Vector2.up;

                    isMoving = true;
                }
                else if (remoteState.IsPressed(WiiU.RemoteButton.Left))
                {
                    playerDirection = Vector2.left;

                    isMoving = true;
                }
                else if (remoteState.IsPressed(WiiU.RemoteButton.Down))
                {
                    playerDirection = Vector2.down;

                    isMoving = true;
                }
                else if (remoteState.IsPressed(WiiU.RemoteButton.Right))
                {
                    playerDirection = Vector2.right;

                    isMoving = true;
                }
                break;
        }

        if (Application.isEditor)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                playerDirection = Vector2.up;

                isMoving = true;
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                playerDirection = Vector2.left;

                isMoving = true;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                playerDirection = Vector2.down;

                isMoving = true;
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                playerDirection = Vector2.right;

                isMoving = true;
            }
        }
    }
}
