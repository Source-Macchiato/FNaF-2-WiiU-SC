using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using WiiU = UnityEngine.WiiU;

public class GamepadClickAdapter : MonoBehaviour
{
    public Vector2 canvasResolution = new Vector2(1280f, 720f);
    public Vector2 gamepadResolution = new Vector2(854f, 480f);

    private WiiU.GamePad gamePad;

    private bool isClicking = false; // Track if currently clicking

    void Start()
    {
        gamePad = WiiU.GamePad.access;

        // Disable the default input module
        EventSystem.current.GetComponent<StandaloneInputModule>().enabled = false;
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
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 adaptedMousePos = AdaptScreenPosition(Input.mousePosition);
            PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = adaptedMousePos };
            SimulatePointerDown(pointerData);
            isClicking = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 adaptedMousePos = AdaptScreenPosition(Input.mousePosition);
            PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = adaptedMousePos };
            SimulatePointerUpAndClick(pointerData);
            isClicking = false;
        }
    }

    private void HandleGamepadTouchInput()
    {
        var gamePadState = gamePad.state;

        if (gamePadState.touch.touch == 1 && !isClicking)
        {
            Vector2 touchPos = new Vector2(gamePadState.touch.x, gamePadState.touch.y);
            Vector2 adaptedTouchPos = AdaptGamepadPosition(touchPos);

            PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = adaptedTouchPos };
            SimulatePointerDown(pointerData);
            isClicking = true;
        }
        else if (gamePadState.touch.touch == 0 && isClicking)
        {
            Vector2 touchPos = new Vector2(gamePadState.touch.x, gamePadState.touch.y);
            Vector2 adaptedTouchPos = AdaptGamepadPosition(touchPos);

            PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = adaptedTouchPos };
            SimulatePointerUpAndClick(pointerData);
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

    private void SimulatePointerDown(PointerEventData pointerData)
    {
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (RaycastResult result in raycastResults)
        {
            ExecuteEvents.Execute(result.gameObject, pointerData, ExecuteEvents.pointerDownHandler);
        }
    }

    private void SimulatePointerUpAndClick(PointerEventData pointerData)
    {
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (RaycastResult result in raycastResults)
        {
            ExecuteEvents.Execute(result.gameObject, pointerData, ExecuteEvents.pointerUpHandler);
            ExecuteEvents.Execute(result.gameObject, pointerData, ExecuteEvents.pointerClickHandler);
        }
    }
}