using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using WiiU = UnityEngine.WiiU;
using TMPro;

public class PopupData
{
    public GameObject popupObject;
    public int actionType;
    public string popupId;
    public int optionId;

    public PopupData(GameObject popupObject, int actionType, string popupId, int optionId)
    {
        this.popupObject = popupObject;
        this.actionType = actionType;
        this.popupId = popupId;
        this.optionId = optionId;
    }
}

public class MenuManager : MonoBehaviour
{
    // Prefab for creating buttons dynamically
    [Header("Prefabs")]
    public GameObject[] popupPrefab;
    public GameObject selectionPrefab;
    public GameObject selectionPopupPrefab;

    // Audio
    [Header("Audio")]
    public AudioSource buttonAudio;

    [Header("Enabled controllers")]
    public bool gamepadController;
    public bool proController;
    public bool classicController;
    public bool wiimoteAndNunchuk;

    // Parent transform where menu buttons will be placed
    public GameObject cursorContainer;
    public GameObject popupContainer;
    public GameObject[] menus;
    public Button[] defaultButtons;

    // List to keep track of generated callbacks
    private Dictionary<int, UnityEngine.Events.UnityAction> backCallbacks = new Dictionary<int, UnityEngine.Events.UnityAction>();

    // Stack to keep track of active popups
    private Queue<PopupData> popupQueue = new Queue<PopupData>();

    // Store menu history
    private Stack<int> menuHistory = new Stack<int>();

    // Flag to check if the user is navigating back
    private bool isNavigatingBack = false;

    [HideInInspector]
    public bool canNavigate = true;

    [HideInInspector]
    public int currentMenuId = 0;

    // Instantiate selection cursor
    [HideInInspector]
    public GameObject currentSelection;
    [HideInInspector]
    public GameObject currentPopupSelection;

    // Elements to keep in memory
    [HideInInspector]
    public ScrollRect currentScrollRect;
    public PopupData currentPopup;

    private TouchScreenKeyboard keyboard;

    // Stick navigation
    private float stickNavigationCooldown = 0.2f;
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

        // Generate cursors
        currentSelection = Instantiate(selectionPrefab, cursorContainer.transform);
        currentPopupSelection = Instantiate(selectionPopupPrefab, cursorContainer.transform);

        ChangeMenu(0);
    }

    void Update()
    {
        // Get the current state of the GamePad and Remote
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        // Handle GamePad input
        if (gamePadState.gamePadErr == WiiU.GamePadError.None)
        {
            // If can navigate with gamepad
            if (gamepadController)
            {
                // Stick
                Vector2 leftStickGamepad = gamePadState.lStick;

                if (Mathf.Abs(leftStickGamepad.y) > stickDeadzone)
                {
                    if (currentScrollRect == null)
                    {
                        if (lastNavigationTime > stickNavigationCooldown)
                        {
                            if (leftStickGamepad.y > stickDeadzone)
                            {
                                MenuNavigation(Vector2.up);
                            }
                            else if (leftStickGamepad.y < -stickDeadzone)
                            {
                                MenuNavigation(Vector2.down);
                            }

                            lastNavigationTime = 0f;
                        }
                    }
                    else
                    {
                        ScrollNavigation(new Vector2(0, leftStickGamepad.y));
                    }
                }

                if (Mathf.Abs(leftStickGamepad.x) > stickDeadzone)
                {
                    if (lastNavigationTime > stickNavigationCooldown)
                    {
                        if (leftStickGamepad.x > stickDeadzone)
                        {
                            MenuNavigation(Vector2.right);
                        }
                        else if (leftStickGamepad.x < -stickDeadzone)
                        {
                            MenuNavigation(Vector2.left);
                        }

                        lastNavigationTime = 0f;
                    }
                }

                // Is Triggered
                if (gamePadState.IsTriggered(WiiU.GamePadButton.Up))
                {
                    MenuNavigation(Vector2.up);
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.Down))
                {
                    MenuNavigation(Vector2.down);
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.Left))
                {
                    MenuNavigation(Vector2.left);
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.Right))
                {
                    MenuNavigation(Vector2.right);
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.ZL))
                {
                    if (EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>() != null && canNavigate)
                    {
                        EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>().DecreaseDifficulty();
                    }
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.ZR))
                {
                    if (EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>() != null && canNavigate)
                    {
                        EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>().IncreaseDifficulty();
                    }
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.L))
                {
                    if (EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>() != null && canNavigate)
                    {
                        EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>().DecreaseOptions();
                    }
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.R))
                {
                    if (EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>() != null && canNavigate)
                    {
                        EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>().IncreaseOptions();
                    }
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.A))
                {
                    if (currentScrollRect == null && currentPopup == null && canNavigate)
                    {
                        ClickSelectedButton();
                    }
                    else if (currentPopup != null && canNavigate)
                    {
                        if (currentPopup.actionType == 0)
                        {
                            CloseCurrentPopup();
                        }
                        else if (currentPopup.actionType == 1)
                        {
                            GameObject optionsContainer = currentPopup.popupObject.transform.Find("Options").gameObject;

                            int index = 0;

                            foreach (Transform child in optionsContainer.transform)
                            {
                                Button button = child.GetComponent<Button>();

                                if (button == EventSystem.current.currentSelectedGameObject.GetComponent<Button>())
                                {
                                    currentPopup.optionId = index; // Should never be -1
                                    break;
                                }

                                index++;
                            }
                        }
                    }

                    DisplayKeyboard();
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.B))
                {
                    GoBack();
                }

                // Is Pressed
                if (gamePadState.IsPressed(WiiU.GamePadButton.Up))
                {
                    ScrollNavigation(new Vector2(0, 1));
                }
                else if (gamePadState.IsPressed(WiiU.GamePadButton.Down))
                {
                    ScrollNavigation(new Vector2(0, -1));
                }
            }
        }

        // Handle Remote input based on the device type
        switch (remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                // If can navigate with Pro Controller
                if (proController)
                {
                    // Stick
                    Vector2 leftStickProController = remoteState.pro.leftStick;

                    if (Mathf.Abs(leftStickProController.y) > stickDeadzone)
                    {
                        if (currentScrollRect == null)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (leftStickProController.y > stickDeadzone)
                                {
                                    MenuNavigation(Vector2.up);
                                }
                                else if (leftStickProController.y < -stickDeadzone)
                                {
                                    MenuNavigation(Vector2.down);
                                }

                                lastNavigationTime = 0f;
                            }
                        }
                        else
                        {
                            ScrollNavigation(new Vector2(0, leftStickProController.y));
                        }
                    }

                    if (Mathf.Abs(leftStickProController.x) > stickDeadzone)
                    {
                        if (lastNavigationTime > stickNavigationCooldown)
                        {
                            if (leftStickProController.x > stickDeadzone)
                            {
                                MenuNavigation(Vector2.right);
                            }
                            else if (leftStickProController.x < -stickDeadzone)
                            {
                                MenuNavigation(Vector2.left);
                            }

                            lastNavigationTime = 0f;
                        }
                    }

                    // Is Triggered
                    if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Up))
                    {
                        MenuNavigation(Vector2.up);
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Down))
                    {
                        MenuNavigation(Vector2.down);
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Left))
                    {
                        MenuNavigation(Vector2.left);
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Right))
                    {
                        MenuNavigation(Vector2.right);
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.ZL))
                    {
                        if (EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>() != null && canNavigate)
                        {
                            EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>().DecreaseDifficulty();
                        }
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.ZR))
                    {
                        if (EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>() != null && canNavigate)
                        {
                            EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>().IncreaseDifficulty();
                        }
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.L))
                    {
                        if (EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>() != null && canNavigate)
                        {
                            EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>().DecreaseOptions();
                        }
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.R))
                    {
                        if (EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>() != null && canNavigate)
                        {
                            EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>().IncreaseOptions();
                        }
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.A))
                    {
                        if (currentScrollRect == null && currentPopup == null && canNavigate)
                        {
                            ClickSelectedButton();
                        }
                        else if (currentPopup != null && canNavigate)
                        {
                            if (currentPopup.actionType == 0)
                            {
                                CloseCurrentPopup();
                            }
                            else if (currentPopup.actionType == 1)
                            {
                                GameObject optionsContainer = currentPopup.popupObject.transform.Find("Options").gameObject;

                                int index = 0;

                                foreach (Transform child in optionsContainer.transform)
                                {
                                    Button button = child.GetComponent<Button>();

                                    if (button == EventSystem.current.currentSelectedGameObject.GetComponent<Button>())
                                    {
                                        currentPopup.optionId = index; // Should never be -1
                                        break;
                                    }

                                    index++;
                                }
                            }
                        }

                        DisplayKeyboard();
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.B))
                    {
                        GoBack();
                    }

                    // Is Pressed
                    if (remoteState.pro.IsPressed(WiiU.ProControllerButton.Up))
                    {
                        ScrollNavigation(new Vector2(0, 1));
                    }
                    else if (remoteState.pro.IsPressed(WiiU.ProControllerButton.Down))
                    {
                        ScrollNavigation(new Vector2(0, -1));
                    }
                }
                break;
            case WiiU.RemoteDevType.Classic:
                // If can navigate with Classic Controller
                if (classicController)
                {
                    // Stick
                    Vector2 leftStickClassicController = remoteState.classic.leftStick;

                    if (Mathf.Abs(leftStickClassicController.y) > stickDeadzone)
                    {
                        if (currentScrollRect == null)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (leftStickClassicController.y > stickDeadzone)
                                {
                                    MenuNavigation(Vector2.up);
                                }
                                else if (leftStickClassicController.y < -stickDeadzone)
                                {
                                    MenuNavigation(Vector2.down);
                                }

                                lastNavigationTime = 0f;
                            }

                        }
                        else
                        {
                            ScrollNavigation(new Vector2(0, leftStickClassicController.y));
                        }
                    }

                    if (Mathf.Abs(leftStickClassicController.x) > stickDeadzone)
                    {
                        if (lastNavigationTime > stickNavigationCooldown)
                        {
                            if (leftStickClassicController.x > stickDeadzone)
                            {
                                MenuNavigation(Vector2.right);
                            }
                            else if (leftStickClassicController.x < -stickDeadzone)
                            {
                                MenuNavigation(Vector2.left);
                            }

                            lastNavigationTime = 0f;
                        }
                    }

                    // Is Triggered
                    if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Up))
                    {
                        MenuNavigation(Vector2.up);
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Down))
                    {
                        MenuNavigation(Vector2.down);
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Left))
                    {
                        MenuNavigation(Vector2.left);
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Right))
                    {
                        MenuNavigation(Vector2.right);
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.ZL) || remoteState.classic.IsTriggered(WiiU.ClassicButton.L))
                    {
                        if (EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>() != null && canNavigate)
                        {
                            EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>().DecreaseDifficulty();
                        }
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.ZR) || remoteState.classic.IsTriggered(WiiU.ClassicButton.R))
                    {
                        if (EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>() != null && canNavigate)
                        {
                            EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>().IncreaseDifficulty();
                        }
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.L))
                    {
                        if (EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>() != null && canNavigate)
                        {
                            EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>().DecreaseOptions();
                        }
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.R))
                    {
                        if (EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>() != null && canNavigate)
                        {
                            EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>().IncreaseOptions();
                        }
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.A))
                    {
                        if (currentScrollRect == null && currentPopup == null && canNavigate)
                        {
                            ClickSelectedButton();
                        }
                        else if (currentPopup != null && canNavigate)
                        {
                            if (currentPopup.actionType == 0)
                            {
                                CloseCurrentPopup();
                            }
                            else if (currentPopup.actionType == 1)
                            {
                                GameObject optionsContainer = currentPopup.popupObject.transform.Find("Options").gameObject;

                                int index = 0;

                                foreach (Transform child in optionsContainer.transform)
                                {
                                    Button button = child.GetComponent<Button>();

                                    if (button == EventSystem.current.currentSelectedGameObject.GetComponent<Button>())
                                    {
                                        currentPopup.optionId = index; // Should never be -1
                                        break;
                                    }

                                    index++;
                                }
                            }
                        }

                        DisplayKeyboard();
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.B))
                    {
                        GoBack();
                    }

                    // Is Pressed
                    if (remoteState.classic.IsPressed(WiiU.ClassicButton.Up))
                    {
                        ScrollNavigation(new Vector2(0, 1));
                    }
                    else if (remoteState.classic.IsPressed(WiiU.ClassicButton.Down))
                    {
                        ScrollNavigation(new Vector2(0, -1));
                    }
                }
                break;
            default:
                // If can navigate with Wiimote and Nunchuk
                if (wiimoteAndNunchuk)
                {
                    // Stick
                    Vector2 stickNunchuk = remoteState.nunchuk.stick;

                    if (Mathf.Abs(stickNunchuk.y) > stickDeadzone)
                    {
                        if (currentScrollRect == null)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (stickNunchuk.y > stickDeadzone)
                                {
                                    MenuNavigation(Vector2.up);
                                }
                                else if (stickNunchuk.y < -stickDeadzone)
                                {
                                    MenuNavigation(Vector2.down);
                                }

                                lastNavigationTime = 0f;
                            }
                        }
                        else
                        {
                            ScrollNavigation(new Vector2(0, stickNunchuk.y));
                        }
                    }

                    if (Mathf.Abs(stickNunchuk.x) > stickDeadzone)
                    {
                        if (lastNavigationTime > stickNavigationCooldown)
                        {
                            if (stickNunchuk.x > stickDeadzone)
                            {
                                MenuNavigation(Vector2.right);
                            }
                            else if (stickNunchuk.x < -stickDeadzone)
                            {
                                MenuNavigation(Vector2.left);
                            }

                            lastNavigationTime = 0f;
                        }
                    }

                    // Is Triggered
                    if (remoteState.IsTriggered(WiiU.RemoteButton.Up))
                    {
                        MenuNavigation(Vector2.up);
                    }
                    else if (remoteState.IsTriggered(WiiU.RemoteButton.Down))
                    {
                        MenuNavigation(Vector2.down);
                    }
                    else if (remoteState.IsTriggered(WiiU.RemoteButton.Left))
                    {
                        MenuNavigation(Vector2.left);
                    }
                    else if (remoteState.IsTriggered(WiiU.RemoteButton.Right))
                    {
                        MenuNavigation(Vector2.right);
                    }
                    else if (remoteState.IsTriggered(WiiU.RemoteButton.Minus) || remoteState.IsTriggered(WiiU.RemoteButton.NunchukZ))
                    {
                        if (EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>() != null && canNavigate)
                        {
                            EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>().DecreaseDifficulty();
                        }
                        else if (EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>() != null && canNavigate)
                        {
                            EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>().DecreaseOptions();
                        }
                    }
                    else if (remoteState.IsTriggered(WiiU.RemoteButton.Plus) || remoteState.IsTriggered(WiiU.RemoteButton.NunchukC))
                    {
                        if (EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>() != null && canNavigate)
                        {
                            EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>().IncreaseDifficulty();
                        }
                        else if (EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>() != null && canNavigate)
                        {
                            EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>().IncreaseOptions();
                        }
                    }
                    else if (remoteState.IsTriggered(WiiU.RemoteButton.A))
                    {
                        if (currentScrollRect == null && currentPopup == null && canNavigate)
                        {
                            ClickSelectedButton();
                        }
                        else if (currentPopup != null && canNavigate)
                        {
                            if (currentPopup.actionType == 0)
                            {
                                CloseCurrentPopup();
                            }
                            else if (currentPopup.actionType == 1)
                            {
                                GameObject optionsContainer = currentPopup.popupObject.transform.Find("Options").gameObject;

                                int index = 0;

                                foreach (Transform child in optionsContainer.transform)
                                {
                                    Button button = child.GetComponent<Button>();

                                    if (button == EventSystem.current.currentSelectedGameObject.GetComponent<Button>())
                                    {
                                        currentPopup.optionId = index; // Should never be -1
                                        break;
                                    }

                                    index++;
                                }
                            }
                        }

                        DisplayKeyboard();
                    }
                    else if (remoteState.IsTriggered(WiiU.RemoteButton.B))
                    {
                        GoBack();
                    }

                    // Is Pressed
                    if (remoteState.IsPressed(WiiU.RemoteButton.Up))
                    {
                        if (currentScrollRect != null && currentPopup == null && canNavigate)
                        {
                            ScrollNavigation(new Vector2(0, 1));
                        }
                    }
                    else if (remoteState.IsPressed(WiiU.RemoteButton.Down))
                    {
                        if (currentScrollRect != null && currentPopup == null && canNavigate)
                        {
                            ScrollNavigation(new Vector2(0, -1));
                        }
                    }
                }
                break;
        }

        // Handle keyboard input, useful for testing in the editor
        if (Application.isEditor)
        {
            // Key Down
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                MenuNavigation(Vector2.up);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                MenuNavigation(Vector2.down);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                MenuNavigation(Vector2.left);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                MenuNavigation(Vector2.right);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                if (EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>() != null && canNavigate)
                {
                    EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>().DecreaseDifficulty();
                }
                else if (EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>() != null && canNavigate)
                {
                    EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>().DecreaseOptions();
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>() != null && canNavigate)
                {
                    EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>().IncreaseDifficulty();
                }
                else if (EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>() != null && canNavigate)
                {
                    EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>().IncreaseOptions();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                if (currentScrollRect == null && currentPopup == null && canNavigate)
                {
                    ClickSelectedButton();
                }
                else if (currentPopup != null && canNavigate)
                {
                    if (currentPopup.actionType == 0)
                    {
                        CloseCurrentPopup();
                    }
                    else if (currentPopup.actionType == 1)
                    {
                        GameObject optionsContainer = currentPopup.popupObject.transform.Find("Options").gameObject;

                        int index = 0;

                        foreach (Transform child in optionsContainer.transform)
                        {
                            Button button = child.GetComponent<Button>();

                            if (button == EventSystem.current.currentSelectedGameObject.GetComponent<Button>())
                            {
                                currentPopup.optionId = index; // Should never be -1
                                break;
                            }

                            index++;
                        }
                    }
                }
            }
            else if (Input.GetKeyDown(KeyCode.Backspace))
            {
                GoBack();
            }

            // Key
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (currentScrollRect != null && currentPopup == null && canNavigate)
                {
                    ScrollNavigation(new Vector2(0, 1));
                }
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                if (currentScrollRect != null && currentPopup == null && canNavigate)
                {
                    ScrollNavigation(new Vector2(0, -1));
                }
            }
        }

        // Toggle visibility for cursors
        if (currentSelection != null)
        {
            if (currentScrollRect == null && currentPopup == null
                && EventSystem.current.currentSelectedGameObject != null
                && EventSystem.current.currentSelectedGameObject.GetComponent<SwitcherData>() == null
                && EventSystem.current.currentSelectedGameObject.GetComponent<CardSwitcherData>() == null
                && EventSystem.current.currentSelectedGameObject.GetComponent<CardData>() == null)
            {
                if (!currentSelection.activeSelf)
                {
                    currentSelection.SetActive(true);
                }

                UpdateSelectionPosition(EventSystem.current.currentSelectedGameObject);
            }
            else
            {
                if (currentSelection.activeSelf)
                {
                    currentSelection.SetActive(false);
                }
            }
        }

        if (currentPopupSelection != null)
        {
            if (currentPopup != null && currentPopup.actionType == 1)
            {
                if (!currentPopupSelection.activeSelf)
                {
                    currentSelection.SetActive(true);
                }

                UpdateSelectionPosition(EventSystem.current.currentSelectedGameObject);
            }
            else
            {
                if (currentPopupSelection.activeSelf)
                {
                    currentPopupSelection.SetActive(false);
                }
            }
        }

        // Calculate stick last navigation time
        lastNavigationTime += Time.deltaTime;
    }

    public void AddPopup(int actionType, string translationId = null, string popupId = null) // Action type : 0 = Press input to continue, 1 = Options
    {
        // Instantiate the popup prefab
        GameObject newPopup = Instantiate(popupPrefab[actionType]);

        // Set the translation for the popup's "PopupText" child
        if (translationId != null)
        {
            GameObject popupTextComponent = newPopup.transform.Find("PopupText").gameObject;
            I18nTextTranslator translator = popupTextComponent.GetComponent<I18nTextTranslator>();
            translator.textId = translationId;
        }

        // Add the popup to the queue
        PopupData popupData = new PopupData(newPopup, actionType, popupId, -1);
        popupQueue.Enqueue(popupData);

        // Check if no popup is currently shown
        if (currentPopup == null)
        {
            ShowNextPopup();
        }
    }

    // Shows the next popup in the queue
    private void ShowNextPopup()
    {
        if (popupQueue.Count > 0)
        {
            // Get the next popup from the queue
            currentPopup = popupQueue.Dequeue();

            // Activate the popup
            currentPopup.popupObject.SetActive(true);

            // Set the position and parent
            currentPopup.popupObject.transform.SetParent(popupContainer.transform, false);

            if (currentPopup.actionType == 1)
            {
                // Select button
                GameObject optionsContainer = currentPopup.popupObject.transform.Find("Options").gameObject;
                Button button = optionsContainer.transform.GetChild(0).GetComponent<Button>();
                button.Select();
            }
            else if (currentPopup.actionType == 2)
            {
                GameObject inputFieldsContainer = currentPopup.popupObject.transform.Find("PopupInputFields").gameObject;
                TMP_InputField inputField = inputFieldsContainer.transform.GetChild(0).GetComponent<TMP_InputField>();
                inputField.Select();
            }
        }
    }

    // Function to close the current popup and show the next one
    public void CloseCurrentPopup()
    {
        if (currentPopup != null)
        {
            // Deactivate and destroy the current popup
            Destroy(currentPopup.popupObject);
            currentPopup = null;

            // Show the next popup if available
            if (popupQueue.Count > 0)
            {
                ShowNextPopup();
            }
            else
            {
                defaultButtons[currentMenuId].Select();
            }
        }
    }

    // Navigates through the menu buttons based on the direction
    public void Select(Selectable nextSelectable)
    {
        if (nextSelectable != null)
        {
            // Get next button and select it
            
            if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
            {
                Button newSelectable = nextSelectable.GetComponent<Button>();
                newSelectable.Select();
            }
            else if (EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null)
            {
                TMP_InputField newSelectable = nextSelectable.GetComponent<TMP_InputField>();
                newSelectable.Select();
            }

            // Play effect
            if (buttonAudio != null)
            {
                buttonAudio.Play();
            }
        }
    }

    private void MenuNavigation(Vector2 direction)
    {
        if (currentScrollRect == null && !keyboard.active && canNavigate)
        {
            if (direction == Vector2.up)
            {
                if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
                {
                    Select(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnUp);
                }
                else if (EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null)
                {
                    Select(EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>().navigation.selectOnUp);
                }
            }
            else if (direction == Vector2.left)
            {
                if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
                {
                    Select(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnLeft);
                }
                else if (EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null)
                {
                    Select(EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>().navigation.selectOnLeft);
                }
            }
            else if (direction == Vector2.down)
            {
                if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
                {
                    Select(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnDown);
                }
                else if (EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null)
                {
                    Select(EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>().navigation.selectOnDown);
                }
            }
            else if (direction == Vector2.right)
            {
                if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
                {
                    Select(EventSystem.current.currentSelectedGameObject.GetComponent<Button>().navigation.selectOnRight);
                }
                else if (EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null)
                {
                    Select(EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>().navigation.selectOnRight);
                }
            }
        }
    }

    public void ScrollNavigation(Vector2 direction)
    {
        if (currentScrollRect != null && currentPopup == null && !keyboard.active && canNavigate)
        {
            RectTransform content = currentScrollRect.content;
            RectTransform viewport = currentScrollRect.viewport;

            if (content != null && viewport != null)
            {
                // Taille totale du contenu et de la vue visible
                float contentHeight = content.rect.height;
                float viewportHeight = viewport.rect.height;

                if (contentHeight > viewportHeight)
                {
                    // Calcul du défilement proportionnel
                    float scrollAmount = (600 / (contentHeight - viewportHeight)) * direction.y * Time.deltaTime;

                    Vector2 newPosition = currentScrollRect.normalizedPosition + new Vector2(0f, scrollAmount);
                    newPosition.y = Mathf.Clamp01(newPosition.y);
                    currentScrollRect.normalizedPosition = newPosition;
                }
            }
        }
    }

    private void DisplayKeyboard()
    {
        if (EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null)
        {
            TMP_InputField inputField = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>();

            keyboard = TouchScreenKeyboard.Open(inputField.text, TouchScreenKeyboardType.Default, false, false, false, false, inputField.placeholder.GetComponent<TextMeshProUGUI>().text);
            keyboard.targetDisplay = WiiU.DisplayIndex.GamePad;
        }
    }

    // Clicks the currently selected button
    private void ClickSelectedButton()
    {
        if (EventSystem.current.currentSelectedGameObject.GetComponent<Button>() != null)
        {
            EventSystem.current.currentSelectedGameObject.GetComponent<Button>().onClick.Invoke();
        }
    }

    private void UpdateSelectionPosition(GameObject selectedButton)
    {
        if (currentPopup == null)
        {
            if (currentSelection != null)
            {
                // Move the selectionPrefab to the left of the selected button
                RectTransform buttonRect = selectedButton.GetComponent<RectTransform>();
                RectTransform selectionRect = currentSelection.GetComponent<RectTransform>();

                // Get the world corners of the button (bottom-left, top-left, top-right, bottom-right)
                Vector3[] buttonCorners = new Vector3[4];
                buttonRect.GetWorldCorners(buttonCorners);

                // Calculate the new position based on the left edge of the button (buttonCorners[0] is the bottom-left corner in world space)
                Vector3 leftEdgePosition = buttonCorners[0]; // Bottom-left corner of the button

                // Convert world position of the button's left edge to local position relative to the canvas
                Vector3 newLocalPos = currentSelection.transform.parent.InverseTransformPoint(leftEdgePosition);

                // Adjust the cursor position slightly to the left (optional, if you want to add padding)
                newLocalPos.x -= selectionRect.rect.width;

                // Set the new position
                selectionRect.localPosition = newLocalPos;
            }
        }
        else
        {
            if (currentPopupSelection != null)
            {
                // Move the selectionPopupPreafab to the left of the selected button
                RectTransform buttonRect = selectedButton.GetComponent<RectTransform>();
                RectTransform selectionRect = currentPopupSelection.GetComponent<RectTransform>();

                // Get the world corners of the button (bottom-left, top-left, top-right, bottom-right)
                Vector3[] buttonCorners = new Vector3[4];
                buttonRect.GetWorldCorners(buttonCorners);

                // Calculate the new position based on the left edge of the button (buttonCorners[0] is the bottom-left corner in world space)
                Vector3 leftEdgePosition = buttonCorners[0]; // Bottom-left corner of the button

                // Convert world position of the button's left edge to local position relative to the canvas
                Vector3 newLocalPos = currentPopupSelection.transform.parent.InverseTransformPoint(leftEdgePosition);

                // Adjust the cursor position slightly to the left (optional, if you want to add padding)
                newLocalPos.x -= selectionRect.rect.width;

                // Set the new position
                selectionRect.localPosition = newLocalPos;
            }
        }
    }

    // Method to change menu and manage display of extra container
    public void ChangeMenu(int menuId)
    {
        if (currentMenuId != menuId && !isNavigatingBack)
        {
            menuHistory.Push(currentMenuId);
        }

        // Menu
        foreach (GameObject menu in menus)
        {
            menu.SetActive(menu == menus[menuId]);
        }

        currentMenuId = menuId;

        // Button
        for (int i = 0; i < defaultButtons.Length; i++)
        {
            if (i == menuId)
            {
                if (defaultButtons[i] != null)
                {
                    defaultButtons[i].Select();
                    buttonAudio.Play();
                }
            }
        }

        isNavigatingBack = false;
    }

    public void SetBackCallback(int menuId, UnityEngine.Events.UnityAction callback)
    {
        if (backCallbacks.ContainsKey(menuId))
        {
            backCallbacks[menuId] = callback;
        }
        else
        {
            backCallbacks.Add(menuId, callback);
        }
    }

    public void GoBack()
    {
        if (currentPopup == null && !keyboard.active && canNavigate)
        {
            if (menuHistory.Count > 0)
            {
                // Set the navigation back flag to true
                isNavigatingBack = true;

                // Execute the callback for the current menu, if it exists
                if (backCallbacks.ContainsKey(currentMenuId) && backCallbacks[currentMenuId] != null)
                {
                    backCallbacks[currentMenuId].Invoke();
                }

                // Retrieve the previous menu ID from the history stack
                int previousMenuId = menuHistory.Pop();

                // Change to the previous menu
                ChangeMenu(previousMenuId);
            }
        }
    }

    public GameObject GetCurrentMenu()
    {
        if (currentMenuId >= 0 && currentMenuId < menus.Length)
        {
            return menus[currentMenuId].gameObject;
        }
        return null;
    }
}