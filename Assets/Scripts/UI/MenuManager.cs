using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public GameObject[] buttonPrefab;
    public GameObject cardPrefab;
    public GameObject switcherPrefab;
    public GameObject cardSwitcherPrefab;
    public GameObject descriptionPrefab;
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
    public Transform[] menus;
    public Transform[] extraMenus;

    // Dictionary for storing extra containers by menuId
    private Dictionary<int, GameObject> extraContainers = new Dictionary<int, GameObject>();
    public Dictionary<int, List<GameObject>> extraMenuButtons = new Dictionary<int, List<GameObject>>();
    public Dictionary<int, List<GameObject>> menuButtons = new Dictionary<int, List<GameObject>>();

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

    private int currentMenuId = 0;

    // Instantiate selection cursor
    [HideInInspector]
    public GameObject currentSelection;
    public GameObject currentPopupSelection;

    // Elements to keep in memory
    [HideInInspector]
    public ScrollRect currentScrollRect;
    public PopupData currentPopup;
    public Button currentButton;

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

        // Generate cursor
        GameObject canvaUI = GameObject.Find("CanvaUI");
        GameObject cursorContainer = canvaUI.transform.Find("CursorContainer").gameObject;

        currentSelection = Instantiate(selectionPrefab, cursorContainer.transform);
        currentPopupSelection = Instantiate(selectionPopupPrefab, cursorContainer.transform);

        // Associates each extra menu with its menuId
        for (int i = 0; i < extraMenus.Length; i++)
        {
            if (extraMenus[i] != null)
            {
                extraContainers[i] = extraMenus[i].gameObject;
                extraContainers[i].SetActive(false); // Hides extra menus on startup
            }
        }
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
                    VerticalLayoutGroup verticalContainer = currentButton.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>();

                    if (currentScrollRect == null && currentPopup == null && verticalContainer != null && canNavigate)
                    {
                        if (lastNavigationTime > stickNavigationCooldown)
                        {
                            if (leftStickGamepad.y > stickDeadzone)
                            {
                                MenuNavigation(currentButton.navigation.selectOnUp);
                            }
                            else if (leftStickGamepad.y < -stickDeadzone)
                            {
                                MenuNavigation(currentButton.navigation.selectOnDown);
                            }

                            lastNavigationTime = 0f;
                        }
                    }
                    else if (currentScrollRect == null && currentPopup == null && verticalContainer == null && canNavigate)
                    {
                        if (lastNavigationTime > stickNavigationCooldown)
                        {
                            if (leftStickGamepad.y > stickDeadzone || leftStickGamepad.y < -stickDeadzone)
                            {
                                ToggleContainer();
                            }

                            lastNavigationTime = 0f;
                        }
                    }
                    else if (currentScrollRect != null && currentPopup == null && canNavigate)
                    {
                        ScrollNavigation(new Vector2(0, leftStickGamepad.y));
                    }
                }

                if (Mathf.Abs(leftStickGamepad.x) > stickDeadzone)
                {
                    HorizontalLayoutGroup horizontalContainer = currentButton.transform.parent.gameObject.GetComponent<HorizontalLayoutGroup>();

                    if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer != null && canNavigate)
                    {
                        if (lastNavigationTime > stickNavigationCooldown)
                        {
                            if (leftStickGamepad.x > stickDeadzone)
                            {
                                MenuNavigation(currentButton.navigation.selectOnRight);
                            }
                            else if (leftStickGamepad.x < -stickDeadzone)
                            {
                                MenuNavigation(currentButton.navigation.selectOnLeft);
                            }

                            lastNavigationTime = 0f;
                        }
                    }
                    else if (currentScrollRect == null && currentPopup == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer == null && extraContainers.ContainsKey(currentMenuId) && canNavigate)
                    {
                        if (lastNavigationTime > stickNavigationCooldown)
                        {
                            if (leftStickGamepad.x > stickDeadzone || leftStickGamepad.x < -stickDeadzone)
                            {
                                ToggleContainer();
                            }

                            lastNavigationTime = 0f;
                        }
                    }
                    else if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() != null && canNavigate)
                    {
                        if (lastNavigationTime > stickNavigationCooldown)
                        {
                            if (leftStickGamepad.x > stickDeadzone)
                            {
                                SwitcherNavigation(Vector2.right);
                            }
                            else if (leftStickGamepad.x < -stickDeadzone)
                            {
                                SwitcherNavigation(Vector2.left);
                            }

                            lastNavigationTime = 0f;
                        }
                    }
                }

                // Is Triggered
                if (gamePadState.IsTriggered(WiiU.GamePadButton.Up))
                {
                    VerticalLayoutGroup verticalContainer = currentButton.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>();

                    if (currentScrollRect == null && verticalContainer != null && canNavigate)
                    {
                        MenuNavigation(currentButton.navigation.selectOnUp);
                    }
                    else if (currentScrollRect == null && currentPopup == null && verticalContainer == null && canNavigate)
                    {
                        ToggleContainer();
                    }
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.Down))
                {
                    VerticalLayoutGroup verticalContainer = currentButton.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>();

                    if (currentScrollRect == null && verticalContainer != null && canNavigate)
                    {
                        MenuNavigation(currentButton.navigation.selectOnDown);
                    }
                    else if (currentScrollRect == null && currentPopup == null && verticalContainer == null && canNavigate)
                    {
                        ToggleContainer();
                    }
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.Left))
                {
                    HorizontalLayoutGroup horizontalContainer = currentButton.transform.parent.gameObject.GetComponent<HorizontalLayoutGroup>();

                    if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer != null && canNavigate)
                    {
                        MenuNavigation(currentButton.navigation.selectOnLeft);
                    }
                    else if (currentScrollRect == null && currentPopup == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer == null && extraContainers.ContainsKey(currentMenuId) && canNavigate)
                    {
                        ToggleContainer();
                    }
                    else if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() != null && canNavigate)
                    {
                        SwitcherNavigation(Vector2.left);
                    }
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.Right))
                {
                    HorizontalLayoutGroup horizontalContainer = currentButton.transform.parent.gameObject.GetComponent<HorizontalLayoutGroup>();

                    if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer != null && canNavigate)
                    {
                        MenuNavigation(currentButton.navigation.selectOnRight);
                    }
                    else if (currentScrollRect == null && currentPopup == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer == null && extraContainers.ContainsKey(currentMenuId) && canNavigate)
                    {
                        ToggleContainer();
                    }
                    else if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() != null && canNavigate)
                    {
                        SwitcherNavigation(Vector2.right);
                    }
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.ZL))
                {
                    if (currentButton.gameObject.GetComponent<CardSwitcher>() != null && canNavigate)
                    {
                        currentButton.gameObject.GetComponent<CardSwitcher>().DecreaseDifficulty();
                    }
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.ZR))
                {
                    if (currentButton.gameObject.GetComponent<CardSwitcher>() != null && canNavigate)
                    {
                        currentButton.gameObject.GetComponent<CardSwitcher>().IncreaseDifficulty();
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

                                if (button == currentButton)
                                {
                                    currentPopup.optionId = index; // Should never be -1
                                    break;
                                }

                                index++;
                            }
                        }
                    }
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.B))
                {
                    if (currentPopup == null && canNavigate)
                    {
                        GoBack();
                    }
                }

                // Is Pressed
                if (gamePadState.IsPressed(WiiU.GamePadButton.Up))
                {
                    if (currentScrollRect != null && currentPopup == null && canNavigate)
                    {
                        ScrollNavigation(new Vector2(0, 1));
                    }
                }
                else if (gamePadState.IsPressed(WiiU.GamePadButton.Down))
                {
                    if (currentScrollRect != null && currentPopup == null && canNavigate)
                    {
                        ScrollNavigation(new Vector2(0, -1));
                    }
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
                        VerticalLayoutGroup verticalContainer = currentButton.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>();

                        if (currentScrollRect == null && currentPopup == null && verticalContainer != null && canNavigate)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (leftStickProController.y > stickDeadzone)
                                {
                                    MenuNavigation(currentButton.navigation.selectOnUp);
                                }
                                else if (leftStickProController.y < -stickDeadzone)
                                {
                                    MenuNavigation(currentButton.navigation.selectOnDown);
                                }

                                lastNavigationTime = 0f;
                            }
                        }
                        else if (currentScrollRect == null && currentPopup == null && verticalContainer == null && canNavigate)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (leftStickProController.y > stickDeadzone || leftStickProController.y < -stickDeadzone)
                                {
                                    ToggleContainer();
                                }

                                lastNavigationTime = 0f;
                            }
                        }
                        else if (currentScrollRect != null && currentPopup == null && canNavigate)
                        {
                            ScrollNavigation(new Vector2(0, leftStickProController.y));
                        }
                    }

                    if (Mathf.Abs(leftStickProController.x) > stickDeadzone)
                    {
                        HorizontalLayoutGroup horizontalContainer = currentButton.transform.parent.gameObject.GetComponent<HorizontalLayoutGroup>();

                        if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer != null && canNavigate)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (leftStickProController.x > stickDeadzone)
                                {
                                    MenuNavigation(currentButton.navigation.selectOnRight);
                                }
                                else if (leftStickProController.x < -stickDeadzone)
                                {
                                    MenuNavigation(currentButton.navigation.selectOnLeft);
                                }

                                lastNavigationTime = 0f;
                            }
                        }
                        else if (currentScrollRect == null && currentPopup == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer == null && extraContainers.ContainsKey(currentMenuId) && canNavigate)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (leftStickProController.x > stickDeadzone || leftStickProController.x < -stickDeadzone)
                                {
                                    ToggleContainer();
                                }

                                lastNavigationTime = 0f;
                            }
                        }
                        else if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() != null && canNavigate)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (leftStickProController.x > stickDeadzone)
                                {
                                    SwitcherNavigation(Vector2.right);
                                }
                                else if (leftStickProController.x < -stickDeadzone)
                                {
                                    SwitcherNavigation(Vector2.left);
                                }

                                lastNavigationTime = 0f;
                            }
                        }
                    }

                    // Is Triggered
                    if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Up))
                    {
                        VerticalLayoutGroup verticalContainer = currentButton.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>();

                        if (currentScrollRect == null && verticalContainer != null && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnUp);
                        }
                        else if (currentScrollRect == null && currentPopup == null && verticalContainer == null && canNavigate)
                        {
                            ToggleContainer();
                        }
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Down))
                    {
                        VerticalLayoutGroup verticalContainer = currentButton.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>();

                        if (currentScrollRect == null && verticalContainer != null && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnDown);
                        }
                        else if (currentScrollRect == null && currentPopup == null && verticalContainer == null && canNavigate)
                        {
                            ToggleContainer();
                        }
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Left))
                    {
                        HorizontalLayoutGroup horizontalContainer = currentButton.transform.parent.gameObject.GetComponent<HorizontalLayoutGroup>();

                        if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer != null && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnLeft);
                        }
                        else if (currentScrollRect == null && currentPopup == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer == null && extraContainers.ContainsKey(currentMenuId) && canNavigate)
                        {
                            ToggleContainer();
                        }
                        else if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() != null && canNavigate)
                        {
                            SwitcherNavigation(Vector2.left);
                        }
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Right))
                    {
                        HorizontalLayoutGroup horizontalContainer = currentButton.transform.parent.gameObject.GetComponent<HorizontalLayoutGroup>();

                        if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer != null && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnRight);
                        }
                        else if (currentScrollRect == null && currentPopup == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer == null && extraContainers.ContainsKey(currentMenuId) && canNavigate)
                        {
                            ToggleContainer();
                        }
                        else if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() != null && canNavigate)
                        {
                            SwitcherNavigation(Vector2.right);
                        }
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.ZL))
                    {
                        if (currentButton.gameObject.GetComponent<CardSwitcher>() != null && canNavigate)
                        {
                            currentButton.gameObject.GetComponent<CardSwitcher>().DecreaseDifficulty();
                        }
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.ZR))
                    {
                        if (currentButton.gameObject.GetComponent<CardSwitcher>() != null && canNavigate)
                        {
                            currentButton.gameObject.GetComponent<CardSwitcher>().IncreaseDifficulty();
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

                                    if (button == currentButton)
                                    {
                                        currentPopup.optionId = index; // Should never be -1
                                        break;
                                    }

                                    index++;
                                }
                            }
                        }
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.B))
                    {
                        if (currentPopup == null && canNavigate)
                        {
                            GoBack();
                        }
                    }

                    // Is Pressed
                    if (remoteState.pro.IsPressed(WiiU.ProControllerButton.Up))
                    {
                        if (currentScrollRect != null && currentPopup == null && canNavigate)
                        {
                            ScrollNavigation(new Vector2(0, 1));
                        }
                    }
                    else if (remoteState.pro.IsPressed(WiiU.ProControllerButton.Down))
                    {
                        if (currentScrollRect != null && currentPopup == null && canNavigate)
                        {
                            ScrollNavigation(new Vector2(0, -1));
                        }
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
                        VerticalLayoutGroup verticalContainer = currentButton.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>();

                        if (currentScrollRect == null && currentPopup == null && verticalContainer != null && canNavigate)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (leftStickClassicController.y > stickDeadzone)
                                {
                                    MenuNavigation(currentButton.navigation.selectOnUp);
                                }
                                else if (leftStickClassicController.y < -stickDeadzone)
                                {
                                    MenuNavigation(currentButton.navigation.selectOnDown);
                                }

                                lastNavigationTime = 0f;
                            }

                        }
                        else if (currentScrollRect == null && currentPopup == null && verticalContainer == null && canNavigate)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (leftStickClassicController.y > stickDeadzone || leftStickClassicController.y < -stickDeadzone)
                                {
                                    ToggleContainer();
                                }

                                lastNavigationTime = 0f;
                            }
                        }
                        else if (currentScrollRect != null && currentPopup == null && canNavigate)
                        {
                            ScrollNavigation(new Vector2(0, leftStickClassicController.y));
                        }
                    }

                    if (Mathf.Abs(leftStickClassicController.x) > stickDeadzone)
                    {
                        HorizontalLayoutGroup horizontalContainer = currentButton.transform.parent.gameObject.GetComponent<HorizontalLayoutGroup>();

                        if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer != null && canNavigate)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (leftStickClassicController.x > stickDeadzone)
                                {
                                    MenuNavigation(currentButton.navigation.selectOnRight);
                                }
                                else if (leftStickClassicController.x < -stickDeadzone)
                                {
                                    MenuNavigation(currentButton.navigation.selectOnLeft);
                                }

                                lastNavigationTime = 0f;
                            }
                        }                        
                        else if (currentScrollRect == null && currentPopup == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer == null && extraContainers.ContainsKey(currentMenuId) && canNavigate)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (leftStickClassicController.x > stickDeadzone || leftStickClassicController.x < -stickDeadzone)
                                {
                                    ToggleContainer();
                                }

                                lastNavigationTime = 0f;
                            }
                        }
                        else if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() != null && canNavigate)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (leftStickClassicController.x > stickDeadzone)
                                {
                                    SwitcherNavigation(Vector2.right);
                                }
                                else if (leftStickClassicController.x < -stickDeadzone)
                                {
                                    SwitcherNavigation(Vector2.left);
                                }

                                lastNavigationTime = 0f;
                            }
                        }
                    }

                    // Is Triggered
                    if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Up))
                    {
                        VerticalLayoutGroup verticalContainer = currentButton.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>();

                        if (currentScrollRect == null && verticalContainer != null && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnUp);
                        }
                        else if (currentScrollRect == null && currentPopup == null && verticalContainer == null && canNavigate)
                        {
                            ToggleContainer();
                        }
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Down))
                    {
                        VerticalLayoutGroup verticalContainer = currentButton.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>();

                        if (currentScrollRect == null && verticalContainer != null && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnDown);
                        }
                        else if (currentScrollRect == null && currentPopup == null && verticalContainer == null && canNavigate)
                        {
                            ToggleContainer();
                        }
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Left))
                    {
                        HorizontalLayoutGroup horizontalContainer = currentButton.transform.parent.gameObject.GetComponent<HorizontalLayoutGroup>();

                        if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer != null && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnLeft);
                        }
                        else if (currentScrollRect == null && currentPopup == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer == null && extraContainers.ContainsKey(currentMenuId) && canNavigate)
                        {
                            ToggleContainer();
                        }
                        else if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() != null && canNavigate)
                        {
                            SwitcherNavigation(Vector2.left);
                        }
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Right))
                    {
                        HorizontalLayoutGroup horizontalContainer = currentButton.transform.parent.gameObject.GetComponent<HorizontalLayoutGroup>();

                        if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer != null && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnRight);
                        }                        
                        else if (currentScrollRect == null && currentPopup == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer == null && extraContainers.ContainsKey(currentMenuId) && canNavigate)
                        {
                            ToggleContainer();
                        }
                        else if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() != null && canNavigate)
                        {
                            SwitcherNavigation(Vector2.right);
                        }
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.ZL) || remoteState.classic.IsTriggered(WiiU.ClassicButton.L))
                    {
                        if (currentButton.gameObject.GetComponent<CardSwitcher>() != null && canNavigate)
                        {
                            currentButton.gameObject.GetComponent<CardSwitcher>().DecreaseDifficulty();
                        }
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.ZR) || remoteState.classic.IsTriggered(WiiU.ClassicButton.R))
                    {
                        if (currentButton.gameObject.GetComponent<CardSwitcher>() != null && canNavigate)
                        {
                            currentButton.gameObject.GetComponent<CardSwitcher>().IncreaseDifficulty();
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

                                    if (button == currentButton)
                                    {
                                        currentPopup.optionId = index; // Should never be -1
                                        break;
                                    }

                                    index++;
                                }
                            }
                        }
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.B))
                    {
                        if (currentPopup == null && canNavigate)
                        {
                            GoBack();
                        }
                    }

                    // Is Pressed
                    if (remoteState.classic.IsPressed(WiiU.ClassicButton.Up))
                    {
                        if (currentScrollRect != null && currentPopup == null && canNavigate)
                        {
                            ScrollNavigation(new Vector2(0, 1));
                        }
                    }
                    else if (remoteState.classic.IsPressed(WiiU.ClassicButton.Down))
                    {
                        if (currentScrollRect != null && currentPopup == null && canNavigate)
                        {
                            ScrollNavigation(new Vector2(0, -1));
                        }
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
                        VerticalLayoutGroup verticalContainer = currentButton.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>();

                        if (currentScrollRect == null && currentPopup == null && verticalContainer != null && canNavigate)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (stickNunchuk.y > stickDeadzone)
                                {
                                    MenuNavigation(currentButton.navigation.selectOnUp);
                                }
                                else if (stickNunchuk.y < -stickDeadzone)
                                {
                                    MenuNavigation(currentButton.navigation.selectOnDown);
                                }

                                lastNavigationTime = 0f;
                            }
                        }
                        else if (currentScrollRect == null && currentPopup == null && verticalContainer == null && canNavigate)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (stickNunchuk.y > stickDeadzone || stickNunchuk.y < -stickDeadzone)
                                {
                                    ToggleContainer();
                                }

                                lastNavigationTime = 0f;
                            }
                        }
                        else if (currentScrollRect != null && currentPopup == null && canNavigate)
                        {
                            ScrollNavigation(new Vector2(0, stickNunchuk.y));
                        }
                    }

                    if (Mathf.Abs(stickNunchuk.x) > stickDeadzone)
                    {
                        HorizontalLayoutGroup horizontalContainer = currentButton.transform.parent.gameObject.GetComponent<HorizontalLayoutGroup>();

                        if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer != null && canNavigate)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (stickNunchuk.x > stickDeadzone)
                                {
                                    MenuNavigation(currentButton.navigation.selectOnRight);
                                }
                                else if (stickNunchuk.x < -stickDeadzone)
                                {
                                    MenuNavigation(currentButton.navigation.selectOnLeft);
                                }

                                lastNavigationTime = 0f;
                            }
                        }
                        else if (currentScrollRect == null && currentPopup == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer == null && extraContainers.ContainsKey(currentMenuId) && canNavigate)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (stickNunchuk.x > stickDeadzone || stickNunchuk.x < -stickDeadzone)
                                {
                                    ToggleContainer();
                                }

                                lastNavigationTime = 0f;
                            }
                        }
                        else if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() != null && canNavigate)
                        {
                            if (lastNavigationTime > stickNavigationCooldown)
                            {
                                if (stickNunchuk.x > stickDeadzone)
                                {
                                    SwitcherNavigation(Vector2.right);
                                }
                                else if (stickNunchuk.x < -stickDeadzone)
                                {
                                    SwitcherNavigation(Vector2.left);
                                }

                                lastNavigationTime = 0f;
                            }
                        }
                    }

                    // Is Triggered
                    if (remoteState.IsTriggered(WiiU.RemoteButton.Up))
                    {
                        VerticalLayoutGroup verticalContainer = currentButton.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>();

                        if (currentScrollRect == null && verticalContainer != null && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnUp);
                        }
                        else if (currentScrollRect == null && currentPopup == null && verticalContainer == null && canNavigate)
                        {
                            ToggleContainer();
                        }
                    }
                    else if (remoteState.IsTriggered(WiiU.RemoteButton.Down))
                    {
                        VerticalLayoutGroup verticalContainer = currentButton.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>();

                        if (currentScrollRect == null && verticalContainer != null && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnDown);
                        }
                        else if (currentScrollRect == null && currentPopup == null && verticalContainer == null && canNavigate)
                        {
                            ToggleContainer();
                        }
                    }
                    else if (remoteState.IsTriggered(WiiU.RemoteButton.Left))
                    {
                        HorizontalLayoutGroup horizontalContainer = currentButton.transform.parent.gameObject.GetComponent<HorizontalLayoutGroup>();

                        if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer != null && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnLeft);
                        }                        
                        else if (currentScrollRect == null && currentPopup == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer == null && extraContainers.ContainsKey(currentMenuId) && canNavigate)
                        {
                            ToggleContainer();
                        }
                        else if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() != null && canNavigate)
                        {
                            SwitcherNavigation(Vector2.left);
                        }
                    }
                    else if (remoteState.IsTriggered(WiiU.RemoteButton.Right))
                    {
                        HorizontalLayoutGroup horizontalContainer = currentButton.transform.parent.gameObject.GetComponent<HorizontalLayoutGroup>();

                        if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer != null && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnRight);
                        }                        
                        else if (currentScrollRect == null && currentPopup == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer == null && extraContainers.ContainsKey(currentMenuId) && canNavigate)
                        {
                            ToggleContainer();
                        }
                        else if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() != null && canNavigate)
                        {
                            SwitcherNavigation(Vector2.right);
                        }
                    }
                    else if (remoteState.IsTriggered(WiiU.RemoteButton.Minus) || remoteState.IsTriggered(WiiU.RemoteButton.NunchukZ))
                    {
                        if (currentButton.gameObject.GetComponent<CardSwitcher>() != null && canNavigate)
                        {
                            currentButton.gameObject.GetComponent<CardSwitcher>().DecreaseDifficulty();
                        }
                    }
                    else if (remoteState.IsTriggered(WiiU.RemoteButton.Plus) || remoteState.IsTriggered(WiiU.RemoteButton.NunchukC))
                    {
                        if (currentButton.gameObject.GetComponent<CardSwitcher>() != null && canNavigate)
                        {
                            currentButton.gameObject.GetComponent<CardSwitcher>().IncreaseDifficulty();
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

                                    if (button == currentButton)
                                    {
                                        currentPopup.optionId = index; // Should never be -1
                                        break;
                                    }

                                    index++;
                                }
                            }
                        }
                    }
                    else if (remoteState.IsTriggered(WiiU.RemoteButton.B))
                    {
                        if (currentPopup == null && canNavigate)
                        {
                            GoBack();
                        }
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
                VerticalLayoutGroup verticalContainer = currentButton.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>();

                if (currentScrollRect == null && verticalContainer != null && canNavigate)
                {
                    MenuNavigation(currentButton.navigation.selectOnUp);
                }
                else if (currentScrollRect == null && currentPopup == null && verticalContainer == null && canNavigate)
                {
                    ToggleContainer();
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                VerticalLayoutGroup verticalContainer = currentButton.transform.parent.gameObject.GetComponent<VerticalLayoutGroup>();

                if (currentScrollRect == null && verticalContainer != null && canNavigate)
                {
                    MenuNavigation(currentButton.navigation.selectOnDown);
                }
                else if (currentScrollRect == null && currentPopup == null && verticalContainer == null && canNavigate)
                {
                    ToggleContainer();
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                HorizontalLayoutGroup horizontalContainer = currentButton.transform.parent.gameObject.GetComponent<HorizontalLayoutGroup>();

                if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer != null && canNavigate)
                {
                    MenuNavigation(currentButton.navigation.selectOnLeft);
                }                
                else if (currentScrollRect == null && currentPopup == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer == null && extraContainers.ContainsKey(currentMenuId) && canNavigate)
                {
                    ToggleContainer();
                }
                else if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() != null && canNavigate)
                {
                    SwitcherNavigation(Vector2.left);
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                HorizontalLayoutGroup horizontalContainer = currentButton.transform.parent.gameObject.GetComponent<HorizontalLayoutGroup>();

                if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer != null && canNavigate)
                {
                    MenuNavigation(currentButton.navigation.selectOnRight);
                }
                
                else if (currentScrollRect == null && currentPopup == null && currentButton.gameObject.GetComponent<SwitcherData>() == null && horizontalContainer == null && extraContainers.ContainsKey(currentMenuId) && canNavigate)
                {
                    ToggleContainer();
                }
                else if (currentScrollRect == null && currentButton.gameObject.GetComponent<SwitcherData>() != null && canNavigate)
                {
                    SwitcherNavigation(Vector2.right);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                if (currentButton.gameObject.GetComponent<CardSwitcher>() != null && canNavigate)
                {
                    currentButton.gameObject.GetComponent<CardSwitcher>().DecreaseDifficulty();
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                if (currentButton.gameObject.GetComponent<CardSwitcher>() != null && canNavigate)
                {
                    currentButton.gameObject.GetComponent<CardSwitcher>().IncreaseDifficulty();
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

                            if (button == currentButton)
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
                if (currentPopup == null && canNavigate)
                {
                    GoBack();
                }
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
            if (currentScrollRect == null && currentPopup == null && menuButtons.ContainsKey(currentMenuId) && menuButtons[currentMenuId].Count > 0 && currentButton.GetComponent<SwitcherData>() == null && currentButton.GetComponent<CardSwitcher>() == null)
            {
                if (!currentSelection.activeSelf)
                {
                    currentSelection.SetActive(true);
                }

                UpdateSelectionPosition(currentButton.gameObject);
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

                UpdateSelectionPosition(currentButton.gameObject);
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

    // Adds a button to the menu with the given text and click action
    public void AddButton(int menuId, int buttonType, UnityEngine.Events.UnityAction onClickAction, string translationId, bool isExtraContainer = false)
    {
        // Check the target container to add the button
        Transform parentTransform = isExtraContainer
            ? extraContainers.ContainsKey(menuId) ? extraContainers[menuId].transform : null
            : menus[menuId];

        if (parentTransform == null) return;  // If the extra container does not exist for this menu, stop adding

        // Instantiates the button in the specified container
        GameObject newButton = Instantiate(buttonPrefab[buttonType], parentTransform);

        // Configure the button text
        GameObject buttonTextComponent = newButton.transform.Find("Text").gameObject;
        Text textComponent = buttonTextComponent.GetComponent<Text>();
        TMP_Text tmpTextComponent = buttonTextComponent.GetComponent<TextMeshProUGUI>();

        // Configure text translation
        I18nTextTranslator translator = buttonTextComponent.GetComponent<I18nTextTranslator>();
        translator.textId = translationId;

        // Add action to button
        Button buttonComponent = newButton.GetComponent<Button>();
        buttonComponent.onClick.AddListener(onClickAction);

        // Adds the button to the appropriate container list
        if (isExtraContainer)
        {
            if (!extraMenuButtons.ContainsKey(menuId))
                extraMenuButtons[menuId] = new List<GameObject>();
            extraMenuButtons[menuId].Add(newButton);
        }
        else
        {
            if (!menuButtons.ContainsKey(menuId))
                menuButtons[menuId] = new List<GameObject>();
            menuButtons[menuId].Add(newButton);
        }

        // Configures navigation between buttons
        int buttonIndex = isExtraContainer ? extraMenuButtons[menuId].Count - 1 : menuButtons[menuId].Count - 1;
        if (buttonIndex > 0)
        {
            GameObject previousButton = isExtraContainer ? extraMenuButtons[menuId][buttonIndex - 1] : menuButtons[menuId][buttonIndex - 1];
            Button previousButtonComponent = previousButton.GetComponent<Button>();

            // Set navigation for the new button
            Navigation newNav = buttonComponent.navigation;
            newNav.mode = Navigation.Mode.Explicit;
            newNav.selectOnUp = previousButtonComponent;
            buttonComponent.navigation = newNav;

            // Set navigation for the previous button
            Navigation prevNav = previousButtonComponent.navigation;
            prevNav.mode = Navigation.Mode.Explicit;
            prevNav.selectOnDown = buttonComponent;
            previousButtonComponent.navigation = prevNav;
        }
    }

    public void AddPopup(string translationId, int actionType, string popupId) // Action type : 0 = Press input to continue, 1 = Options
    {
        // Instantiate the popup prefab
        GameObject newPopup = Instantiate(popupPrefab[actionType]);

        // Set the translation for the popup's "PopupText" child
        GameObject popupTextComponent = newPopup.transform.Find("PopupText").gameObject;
        I18nTextTranslator translator = popupTextComponent.GetComponent<I18nTextTranslator>();
        translator.textId = translationId;

        // Add the popup to the queue
        PopupData popupData = new PopupData(newPopup, actionType, popupId, -1);
        popupQueue.Enqueue(popupData);

        // Check if no popup is currently shown
        if (currentPopup == null)
        {
            ShowNextPopup();
        }
    }

    public void AddCard(int menuId, string cardText, Sprite cardImage = null)
    {
        // Instantiate the card prefab
        GameObject newCard = Instantiate(cardPrefab, menus[menuId]);

        // Get the card button
        Button cardComponent = newCard.GetComponent<Button>();

        // Set the card text
        GameObject cardTextComponent = newCard.transform.Find("Text").gameObject;
        Text textComponent = cardTextComponent.GetComponent<Text>();
        TMP_Text tmpTextComponent = cardTextComponent.GetComponent<TextMeshProUGUI>();
        
        if (textComponent != null)
        {
            textComponent.text = cardText;
        }

        if (tmpTextComponent != null)
        {
            tmpTextComponent.text = cardText;
        }

        // Set the card image
        if (cardImage != null)
        {
            GameObject cardImageComponent = newCard.transform.Find("Cover").gameObject;
            Image image = cardImageComponent.GetComponent<Image>();
            image.sprite = cardImage;
        }

        // Add the card to the correct menu list in the dictionary
        if (!menuButtons.ContainsKey(menuId))
        {
            menuButtons[menuId] = new List<GameObject>();
        }

        // Handle navigation setup
        int cardIndex = menuButtons[menuId].Count;

        if (cardIndex > 0)
        {
            // Get the previous button
            GameObject previousCard = menuButtons[menuId][cardIndex - 1];
            Button previousCardComponent = previousCard.GetComponent<Button>();

            // Set navigation for the new button
            Navigation newNav = cardComponent.navigation;
            newNav.mode = Navigation.Mode.Explicit;
            newNav.selectOnLeft = previousCardComponent;
            cardComponent.navigation = newNav;

            // Set navigation for the previous button
            Navigation prevNav = previousCardComponent.navigation;
            prevNav.mode = Navigation.Mode.Explicit;
            prevNav.selectOnRight = cardComponent;
            previousCardComponent.navigation = prevNav;
        }

        // Add the new button to the list
        menuButtons[menuId].Add(newCard);
    }

    public void AddSwitcher(int menuId, string[] optionsName, string switcherId)
    {
        // Instantiate the switcher prefab
        GameObject newSwitcher = Instantiate(switcherPrefab, menus[menuId]);

        // Get the switcher button
        Button switcherComponent = newSwitcher.GetComponent<Button>();

        // Get SwitcherData script
        SwitcherData switcherData = newSwitcher.GetComponent<SwitcherData>();
        switcherData.optionsName = optionsName;

        // Set switcher ID
        switcherData.switcherId = switcherId;

        // Add the switcher to the correct menu list in the dictionary
        if (!menuButtons.ContainsKey(menuId))
        {
            menuButtons[menuId] = new List<GameObject>();
        }

        // Handle navigation setup
        int switcherIndex = menuButtons[menuId].Count;

        if (switcherIndex > 0)
        {
            // Get the previous button
            GameObject previousSwitcher = menuButtons[menuId][switcherIndex - 1];
            Button previousSwitcherComponent = previousSwitcher.GetComponent<Button>();

            // Set navigation for the new button
            Navigation newNav = switcherComponent.navigation;
            newNav.mode = Navigation.Mode.Explicit;
            newNav.selectOnUp = previousSwitcherComponent;
            switcherComponent.navigation = newNav;

            // Set navigation for the previous button
            Navigation prevNav = previousSwitcherComponent.navigation;
            prevNav.mode = Navigation.Mode.Explicit;
            prevNav.selectOnDown = switcherComponent;
            previousSwitcherComponent.navigation = prevNav;
        }

        // Add the new button to the list
        menuButtons[menuId].Add(newSwitcher);
    }

    public void AddCardSwitcher(int menuId, string titleName, Sprite cover, string descriptionTranslationId, int minValue, int maxValue, int defaultValue = 0)
    {
        // Instantiate the card switcher prefab
        GameObject newCardSwitcher = Instantiate(cardSwitcherPrefab, menus[menuId]);

        // Get the card switcher button
        Button cardSwitcherComponent = newCardSwitcher.GetComponent<Button>();

        // Get CardSwitcher script
        CardSwitcher cardSwitcher = newCardSwitcher.GetComponent<CardSwitcher>();
        cardSwitcher.titleName = titleName;
        cardSwitcher.coverSprite = cover;
        cardSwitcher.descriptionTranslationId = descriptionTranslationId;
        cardSwitcher.minValue = minValue;
        cardSwitcher.maxValue = maxValue;
        cardSwitcher.difficultyId = defaultValue;
        cardSwitcher.UpdateCardSwitcher();

        // Add the card switcher to the correct menu list in the dictionary
        if (!menuButtons.ContainsKey(menuId))
        {
            menuButtons[menuId] = new List<GameObject>();
        }

        // Handle navigation setup
        int cardIndex = menuButtons[menuId].Count;

        if (cardIndex > 0)
        {
            // Get the previous button
            GameObject previousCard = menuButtons[menuId][cardIndex - 1];
            Button previousCardComponent = previousCard.GetComponent<Button>();

            // Set navigation for the new button
            Navigation newNav = cardSwitcherComponent.navigation;
            newNav.mode = Navigation.Mode.Explicit;
            newNav.selectOnLeft = previousCardComponent;
            cardSwitcherComponent.navigation = newNav;

            // Set navigation for the previous button
            Navigation prevNav = previousCardComponent.navigation;
            prevNav.mode = Navigation.Mode.Explicit;
            prevNav.selectOnRight = cardSwitcherComponent;
            previousCardComponent.navigation = prevNav;
        }

        // Add the new button to the list
        menuButtons[menuId].Add(newCardSwitcher);
    }

    public void AddDescription(int menuId, string translationId)
    {
        // Instantiate the description prefab
        GameObject newDescription = Instantiate(descriptionPrefab, menus[menuId]);

        // Set the translation for the description
        I18nTextTranslator translator = newDescription.GetComponent<I18nTextTranslator>();
        translator.textId = translationId;
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
            GameObject popupContainer = GameObject.Find("PopupContainer");
            currentPopup.popupObject.transform.SetParent(popupContainer.transform, false);

            if (currentPopup.actionType == 1)
            {
                // Select button
                GameObject optionsContainer = currentPopup.popupObject.transform.Find("Options").gameObject;
                GameObject buttonGameObject = optionsContainer.transform.GetChild(0).gameObject;
                Button selectButton = buttonGameObject.GetComponent<Button>();
                selectButton.Select();
                currentButton = selectButton;
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
                // Enable first button visual
                Button newButton = menuButtons[currentMenuId][0].GetComponent<Button>();
                newButton.Select();

                currentButton = newButton;
            }
        }
    }

    // Navigates through the menu buttons based on the direction
    public void MenuNavigation(Selectable nextSelectable)
    {
        if (nextSelectable != null && menuButtons.ContainsKey(currentMenuId) && menuButtons[currentMenuId].Count > 0)
        {
            // Get next button and select it
            Button newButton = nextSelectable.GetComponent<Button>();
            newButton.Select();

            // Set current button
            currentButton = newButton;

            // Play effect
            if (buttonAudio != null)
            {
                buttonAudio.Play();
            }
        }
    }

    public void ScrollNavigation(Vector2 direction)
    {
        if (currentScrollRect != null && currentPopup == null)
        {
            float scrollAmount = direction.y * 0.5f * Time.deltaTime;
            Vector2 newPosition = currentScrollRect.normalizedPosition + new Vector2(0f, scrollAmount);
            newPosition.y = Mathf.Clamp01(newPosition.y);
            currentScrollRect.normalizedPosition = newPosition;
        }
    }

    public void SwitcherNavigation(Vector2 direction)
    {
        SwitcherData switcherData = currentButton.GetComponent<SwitcherData>();

        if (direction == Vector2.left)
        {
            if (switcherData.currentOptionId > 0)
            {
                switcherData.currentOptionId--;

                // Play effect
                if (buttonAudio != null)
                {
                    buttonAudio.Play();
                }
            }
        }
        else if (direction == Vector2.right)
        {
            if (switcherData.currentOptionId < (switcherData.optionsName.Length - 1))
            {
                switcherData.currentOptionId++;

                // Play effect
                if (buttonAudio != null)
                {
                    buttonAudio.Play();
                }
            }
        }
    }

    // Clicks the currently selected button
    private void ClickSelectedButton()
    {
        if (currentButton != null)
        {
            currentButton.onClick.Invoke();
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

        foreach (Transform menu in menus)
        {
            menu.gameObject.SetActive(menu == menus[menuId]);
        }

        currentMenuId = menuId;

        // Hide other extra containers and show current extra container (if exists)
        foreach (var container in extraContainers.Values)
        {
            container.SetActive(false);
        }

        if (extraContainers.ContainsKey(menuId))
        {
            extraContainers[menuId].SetActive(true);
        }

        if (menuButtons.ContainsKey(menuId) && menuButtons[menuId].Count > 0)
        {
            Button newButton = menuButtons[menuId][0].GetComponent<Button>();
            newButton.Select();
            currentButton = newButton;
            buttonAudio.Play();
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

    public GameObject GetCurrentMenu()
    {
        if (currentMenuId >= 0 && currentMenuId < menus.Length)
        {
            return menus[currentMenuId].gameObject;
        }
        return null;
    }

    // Method to switch between main container and extra container, if available
    public void ToggleContainer()
    {
        if (!extraContainers.ContainsKey(currentMenuId)) return;

        List<GameObject> currentButtons = currentButton.transform.IsChildOf(extraContainers[currentMenuId].transform)
            ? menuButtons[currentMenuId]
            : extraMenuButtons[currentMenuId];

        if (currentButtons != null && currentButtons.Count > 0)
        {
            Button newButton = currentButtons[0].GetComponent<Button>();
            newButton.Select();
            currentButton = newButton;
        }

        // Play effect
        if (buttonAudio != null)
        {
            buttonAudio.Play();
        }
    }
}