using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WiiU = UnityEngine.WiiU;

public class MinimapNavigation : MonoBehaviour
{
    public Button defaultSelectedButton;
    public GameObject minimapPanel;
    public GameObject[] buttonObjects;
    public GameObject lastSelectedObject;

    // Stick navigation
    private float stickNavigationCooldown = 0.3f;
    private float lastNavigationTime;
    private float stickDeadzone = 0.19f;

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

        // Can navigate only when the minimap is active
        if (minimapPanel.activeSelf)
        {
            // Set default selected gameobject if no gameobject is selected
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                CheckCurrentButton();

                NavigateTo(defaultSelectedButton);
            }

            // Check inputs and navigate
            if (gamePadState.gamePadErr == WiiU.GamePadError.None)
            {
                Vector2 leftStickGamepad = gamePadState.lStick;

                if (Mathf.Abs(leftStickGamepad.y) > stickDeadzone)
                {
                    if (lastNavigationTime > stickNavigationCooldown)
                    {
                        if (leftStickGamepad.y > stickDeadzone)
                        {
                            CheckCurrentButton();

                            NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnUp);
                        }
                        else if (leftStickGamepad.y < -stickDeadzone)
                        {
                            CheckCurrentButton();

                            NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnDown);
                        }

                        lastNavigationTime = 0f;
                    }
                }

                if (Mathf.Abs(leftStickGamepad.x) > stickDeadzone)
                {
                    if (lastNavigationTime > stickNavigationCooldown)
                    {
                        if (leftStickGamepad.x > stickDeadzone)
                        {
                            CheckCurrentButton();

                            NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnRight);
                        }
                        else if (leftStickGamepad.x < -stickDeadzone)
                        {
                            CheckCurrentButton();

                            NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnLeft);
                        }

                        lastNavigationTime = 0f;
                    }
                }

                if (gamePadState.IsTriggered(WiiU.GamePadButton.Up))
                {
                    CheckCurrentButton();

                    NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnUp);
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.Down))
                {
                    CheckCurrentButton();

                    NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnDown);
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.Left))
                {
                    CheckCurrentButton();

                    NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnLeft);
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.Right))
                {
                    CheckCurrentButton();

                    NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnRight);
                }
            }

            switch (remoteState.devType)
            {
                case WiiU.RemoteDevType.ProController:
                    Vector2 leftStickProController = remoteState.pro.leftStick;

                    if (Mathf.Abs(leftStickProController.y) > stickDeadzone)
                    {
                        if (lastNavigationTime > stickNavigationCooldown)
                        {
                            if (leftStickProController.y > stickDeadzone)
                            {
                                CheckCurrentButton();

                                NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnUp);
                            }
                            else if (leftStickProController.y < -stickDeadzone)
                            {
                                CheckCurrentButton();

                                NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnDown);
                            }

                            lastNavigationTime = 0f;
                        }
                    }

                    if (Mathf.Abs(leftStickProController.x) > stickDeadzone)
                    {
                        if (lastNavigationTime > stickNavigationCooldown)
                        {
                            if (leftStickProController.x > stickDeadzone)
                            {
                                CheckCurrentButton();

                                NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnRight);
                            }
                            else if (leftStickProController.x < -stickDeadzone)
                            {
                                CheckCurrentButton();

                                NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnLeft);
                            }

                            lastNavigationTime = 0f;
                        }
                    }

                    if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Up))
                    {
                        CheckCurrentButton();

                        NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnUp);
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Down))
                    {
                        CheckCurrentButton();

                        NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnDown);
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Left))
                    {
                        CheckCurrentButton();

                        NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnLeft);
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Right))
                    {
                        CheckCurrentButton();

                        NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnRight);
                    }
                    break;
                case WiiU.RemoteDevType.Classic:
                    Vector2 leftStickClassicController = remoteState.classic.leftStick;

                    if (Mathf.Abs(leftStickClassicController.y) > stickDeadzone)
                    {
                        if (lastNavigationTime > stickNavigationCooldown)
                        {
                            if (leftStickClassicController.y > stickDeadzone)
                            {
                                CheckCurrentButton();

                                NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnUp);
                            }
                            else if (leftStickClassicController.y < -stickDeadzone)
                            {
                                CheckCurrentButton();

                                NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnDown);
                            }

                            lastNavigationTime = 0f;
                        }
                    }

                    if (Mathf.Abs(leftStickClassicController.x) > stickDeadzone)
                    {
                        if (lastNavigationTime > stickNavigationCooldown)
                        {
                            if (leftStickClassicController.x > stickDeadzone)
                            {
                                CheckCurrentButton();

                                NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnRight);
                            }
                            else if (leftStickClassicController.x < -stickDeadzone)
                            {
                                CheckCurrentButton();

                                NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnLeft);
                            }

                            lastNavigationTime = 0f;
                        }
                    }

                    if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Up))
                    {
                        CheckCurrentButton();

                        NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnUp);
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Down))
                    {
                        CheckCurrentButton();

                        NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnDown);
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Left))
                    {
                        CheckCurrentButton();

                        NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnLeft);
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Right))
                    {
                        CheckCurrentButton();

                        NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnRight);
                    }
                    break;
                default:
                    Vector2 stickNunchuk = remoteState.nunchuk.stick;

                    if (Mathf.Abs(stickNunchuk.y) > stickDeadzone)
                    {
                        if (lastNavigationTime > stickNavigationCooldown)
                        {
                            if (stickNunchuk.y > stickDeadzone)
                            {
                                CheckCurrentButton();

                                NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnUp);
                            }
                            else if (stickNunchuk.y < -stickDeadzone)
                            {
                                CheckCurrentButton();

                                NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnDown);
                            }

                            lastNavigationTime = 0f;
                        }
                    }

                    if (Mathf.Abs(stickNunchuk.x) > stickDeadzone)
                    {
                        if (lastNavigationTime > stickNavigationCooldown)
                        {
                            if (stickNunchuk.x > stickDeadzone)
                            {
                                CheckCurrentButton();

                                NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnRight);
                            }
                            else if (stickNunchuk.x < -stickDeadzone)
                            {
                                CheckCurrentButton();

                                NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnLeft);
                            }

                            lastNavigationTime = 0f;
                        }
                    }

                    if (remoteState.IsTriggered(WiiU.RemoteButton.Up))
                    {
                        CheckCurrentButton();

                        NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnUp);
                    }
                    else if (remoteState.IsTriggered(WiiU.RemoteButton.Down))
                    {
                        CheckCurrentButton();

                        NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnDown);
                    }
                    else if (remoteState.IsTriggered(WiiU.RemoteButton.Left))
                    {
                        CheckCurrentButton();

                        NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnLeft);
                    }
                    else if (remoteState.IsTriggered(WiiU.RemoteButton.Right))
                    {
                        CheckCurrentButton();

                        NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnRight);
                    }
                    break;
            }

            if (Application.isEditor)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    CheckCurrentButton();

                    NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnUp);
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    CheckCurrentButton();

                    NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnDown);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    CheckCurrentButton();

                    NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnLeft);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    CheckCurrentButton();

                    NavigateTo(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnRight);
                }
            }
        }

        // Calculate stick last navigation time
        lastNavigationTime += Time.deltaTime;
    }

    void NavigateTo(Selectable nextSelectable)
    {
        EventSystem.current.SetSelectedGameObject(nextSelectable.gameObject);

        lastSelectedObject = nextSelectable.gameObject;

        EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
    }

    void CheckCurrentButton()
    {
        bool isMinimapButton = false;

        foreach (GameObject buttonObject in buttonObjects)
        {
            if (buttonObject == EventSystem.current.currentSelectedGameObject)
            {
                isMinimapButton = true;
                break;
            }
        }

        if (!isMinimapButton)
        {
            EventSystem.current.SetSelectedGameObject(lastSelectedObject);
        }
    }

    public void ButtonUpdateSelection(Button button)
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);

        lastSelectedObject = button.gameObject;
    }
}