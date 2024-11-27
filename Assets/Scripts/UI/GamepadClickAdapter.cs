using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using WiiU = UnityEngine.WiiU;

public class GamepadClickAdapter : MonoBehaviour
{
    // Canvas resolution
    public Vector2 canvasResolution = new Vector2(1280f, 720f);

    // Gamepad resolution
    public Vector2 gamepadResolution = new Vector2(854f, 480f);

    // References to WiiU controllers
    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    void Start()
    {
        // Access the WiiU GamePad and Remote
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

        // Disable default system
        EventSystem.current.GetComponent<StandaloneInputModule>().enabled = false;
    }

    void Update()
    {
        // Get the current state of the GamePad and Remote
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        if (Application.isEditor)
        {
            // Check if click on screen
            if (Input.GetMouseButtonDown(0))
            {
                // Get mouse position
                Vector2 mousePos = Input.mousePosition;

                // Calculate ratio resolution
                float mouseRatioX = canvasResolution.x / Screen.width;
                float mouseRatioY = canvasResolution.y / Screen.height;

                // Adapt mouse position for resolution
                Vector2 adaptedMousePos = new Vector2(
                    mousePos.x * mouseRatioX,
                    mousePos.y * mouseRatioY
                );

                // Convert click coordinates to Raycast for UI interaction
                PointerEventData mousePointerData = new PointerEventData(EventSystem.current);
                mousePointerData.position = adaptedMousePos;

                // Create a list for Raycast results
                List<RaycastResult> mouseRaycastResults = new List<RaycastResult>();

                // Create Raycast UI
                EventSystem.current.RaycastAll(mousePointerData, mouseRaycastResults);

                // If an element is detected
                if (mouseRaycastResults.Count > 0)
                {
                    if (mouseRaycastResults[0].gameObject != EventSystem.current.currentSelectedGameObject)
                    {
                        // Simulate a click on the UI element
                        ExecuteEvents.Execute(mouseRaycastResults[0].gameObject, mousePointerData, ExecuteEvents.pointerClickHandler);
                    }
                }
            }
        }
        else
        {
            // Check if is touching screen
            if (gamePadState.touch.touch == 1)
            {
                // Get touch position
                Vector2 touchPos = new Vector2(gamePadState.touch.x, gamePadState.touch.y);

                // Calculate ratio resolution
                float ratioX = canvasResolution.x / gamepadResolution.x;
                float ratioY = canvasResolution.y / gamepadResolution.y;

                // Reverse Y axis
                float adjustedY = gamepadResolution.y - touchPos.y;

                // Adapt touch position for resolution
                Vector2 adaptedTouchPos = new Vector2(
                    touchPos.x * ratioX,
                    adjustedY * ratioY
                );

                // Convert touch coordinates to Raycast for UI interaction
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = adaptedTouchPos;

                // Create a list for Raycast results
                List<RaycastResult> raycastResults = new List<RaycastResult>();

                // Create Raycast UI
                EventSystem.current.RaycastAll(pointerData, raycastResults);

                // If an element is detected
                if (raycastResults.Count > 0)
                {
                    Debug.Log("UI Element clicked: " + raycastResults[0].gameObject.name);

                    if (raycastResults[0].gameObject != EventSystem.current.currentSelectedGameObject)
                    {
                        // Simulate a click on the UI element
                        ExecuteEvents.Execute(raycastResults[0].gameObject, pointerData, ExecuteEvents.pointerClickHandler);
                    }  
                }
            }
        }
    }
}
