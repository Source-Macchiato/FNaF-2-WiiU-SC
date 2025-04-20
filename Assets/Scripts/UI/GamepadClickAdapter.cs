using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using WiiU = UnityEngine.WiiU;

public class GamepadClickAdapter : MonoBehaviour
{
    public Vector2 canvasResolution = new Vector2(960f, 720f);
    public Vector2 gamepadResolution = new Vector2(854f, 480f);

    private WiiU.GamePad gamePad;

    private bool isClicking = false; // Track if currently clicking
    private GameObject pointerPress; // Track the object pressed
    private PointerEventData pointerData; // Shared pointer data instance

    void Start()
    {
        gamePad = WiiU.GamePad.access;

        // Disable the default input module
        EventSystem.current.GetComponent<StandaloneInputModule>().enabled = false;

        // Initialize PointerEventData
        pointerData = new PointerEventData(EventSystem.current);
    }

    void Update()
    {
        if (Application.isEditor)
        {
            HandleMouseInput();
        }
        else
        {
            HandleGamepadTouchInput();
        }
    }

    private void HandleMouseInput()
    {
        pointerData.position = AdaptScreenPosition(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            SimulatePointerDown();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            SimulatePointerUp();
        }
    }

    private void HandleGamepadTouchInput()
    {
        var gamePadState = gamePad.state;

        if (gamePadState.touch.touch == 1)
        {
            pointerData.position = AdaptGamepadPosition(new Vector2(gamePadState.touch.x, gamePadState.touch.y));

            if (!isClicking)
            {
                SimulatePointerDown();
                isClicking = true;
            }
        }
        else if (isClicking && gamePadState.touch.touch == 0)
        {
            SimulatePointerUp();
            isClicking = false;
        }
    }

    private Vector2 AdaptScreenPosition(Vector2 inputPosition)
    {
        float ratioX = canvasResolution.x / Screen.width;
        float ratioY = canvasResolution.y / Screen.height;
        return new Vector2(inputPosition.x * ratioX, inputPosition.y * ratioY);
    }

    private Vector2 AdaptGamepadPosition(Vector2 inputPosition)
    {
        float ratioX = canvasResolution.x / gamepadResolution.x;
        float ratioY = canvasResolution.y / gamepadResolution.y;
        float adjustedY = gamepadResolution.y - inputPosition.y; // Reverse Y-axis
        return new Vector2(inputPosition.x * ratioX, adjustedY * ratioY);
    }

    private void SimulatePointerDown()
    {
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (var result in raycastResults)
        {
            GameObject currentOverGo = result.gameObject;

            GameObject executedPress = ExecuteEvents.ExecuteHierarchy(currentOverGo, pointerData, ExecuteEvents.pointerDownHandler);

            if (executedPress != null)
            {
                pointerPress = executedPress;
                pointerData.pointerPress = pointerPress;
                pointerData.rawPointerPress = currentOverGo;
                break;
            }
        }
    }

    private void SimulatePointerUp()
    {
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        if (pointerPress != null)
        {
            ExecuteEvents.Execute(pointerPress, pointerData, ExecuteEvents.pointerUpHandler);

            if (raycastResults.Exists(r => r.gameObject == pointerPress))
            {
                ExecuteEvents.Execute(pointerPress, pointerData, ExecuteEvents.pointerClickHandler);
            }

            pointerData.pointerPress = null;
            pointerData.rawPointerPress = null;
            pointerPress = null;
        }
    }
}