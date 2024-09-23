using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using WiiU = UnityEngine.WiiU;

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
    public GameObject buttonPrefab;
    public GameObject selectionPrefab;
    public GameObject selectionPopupPrefab;
    public GameObject[] popupPrefab;

    // Audio
    [Header("Audio")]
    public AudioSource buttonAudio;

    [Header("Enabled controllers")]
    public bool gamepad;
    public bool proController;
    public bool classicController;
    public bool wiimoteAndNunchuk;

    // Parent transform where menu buttons will be placed
    public Transform[] menus;

    // List to keep track of all menu buttons
    [HideInInspector]
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
    private GameObject currentSelection;
    private GameObject currentPopupSelection;

    // Elements to keep in memory
    [HideInInspector]
    public ScrollRect currentScrollRect;
    public PopupData currentPopup;
    public Button currentButton;

    // Stick navigation
    private float stickNavigationCooldown = 0.2f;
    private float lastNavigationTime;

    // References to WiiU controllers
    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    private float stickDeadzone = 0.19f;

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
            if (gamepad)
            {
                // Stick
                Vector2 leftStickGamepad = gamePadState.lStick;

                if (Mathf.Abs(leftStickGamepad.y) > stickDeadzone)
                {
                    if (currentScrollRect == null && currentPopup == null && canNavigate)
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
                    else if (currentScrollRect != null && currentPopup == null && canNavigate)
                    {
                        ScrollNavigation(new Vector2(0, leftStickGamepad.y));
                    }
                }

                // Is Triggered
                if (gamePadState.IsTriggered(WiiU.GamePadButton.Up))
                {
                    if (currentScrollRect == null && currentPopup == null && canNavigate)
                    {
                        MenuNavigation(currentButton.navigation.selectOnUp);
                    }
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.Down))
                {
                    if (currentScrollRect == null && currentPopup == null && canNavigate)
                    {
                        MenuNavigation(currentButton.navigation.selectOnDown);
                    }
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.Left))
                {
                    if (currentPopup != null && currentPopup.actionType == 1 && canNavigate)
                    {
                        MenuNavigation(currentButton.navigation.selectOnLeft);
                    }
                }
                else if (gamePadState.IsTriggered(WiiU.GamePadButton.Right))
                {
                    if (currentPopup != null && currentPopup.actionType == 1 && canNavigate)
                    {
                        MenuNavigation(currentButton.navigation.selectOnRight);
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
                        if (currentScrollRect == null && currentPopup == null && canNavigate)
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
                        else if (currentScrollRect != null && currentPopup == null && canNavigate)
                        {
                            ScrollNavigation(new Vector2(0, leftStickProController.y));
                        }
                    }

                    // Is Triggered
                    if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Up))
                    {
                        if (currentScrollRect == null && currentPopup == null && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnUp);
                        }
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Down))
                    {
                        if (currentScrollRect == null && currentPopup == null && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnDown);
                        }
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Left))
                    {
                        if (currentPopup != null && currentPopup.actionType == 1 && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnLeft);
                        }
                    }
                    else if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.Right))
                    {
                        if (currentPopup != null && currentPopup.actionType == 1 && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnRight);
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
                        if (currentScrollRect == null && currentPopup == null && canNavigate)
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
                        else if (currentScrollRect != null && currentPopup == null && canNavigate)
                        {
                            ScrollNavigation(new Vector2(0, leftStickClassicController.y));
                        }
                    }

                    // Is Released
                    if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Up))
                    {
                        if (currentScrollRect == null && currentPopup == null && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnUp);
                        }
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Down))
                    {
                        if (currentScrollRect == null && currentPopup == null && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnDown);
                        }
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Left))
                    {
                        if (currentPopup != null && currentPopup.actionType == 1 && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnLeft);
                        }
                    }
                    else if (remoteState.classic.IsTriggered(WiiU.ClassicButton.Right))
                    {
                        if (currentPopup != null && currentPopup.actionType == 1 && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnRight);
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
                        if (currentScrollRect == null && currentPopup == null && canNavigate)
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
                        else if (currentScrollRect != null && currentPopup == null && canNavigate)
                        {
                            ScrollNavigation(new Vector2(0, stickNunchuk.y));
                        }
                    }

                    // Is Triggered
                    if (remoteState.IsTriggered(WiiU.RemoteButton.Up))
                    {
                        if (currentScrollRect == null && currentPopup == null && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnUp);
                        }
                    }
                    else if (remoteState.IsTriggered(WiiU.RemoteButton.Down))
                    {
                        if (currentScrollRect == null && currentPopup == null && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnDown);
                        }
                    }
                    else if (remoteState.IsTriggered(WiiU.RemoteButton.Left))
                    {
                        if (currentPopup != null && currentPopup.actionType == 1 && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnLeft);
                        }
                    }
                    else if (remoteState.IsTriggered(WiiU.RemoteButton.Right))
                    {
                        if (currentPopup != null && currentPopup.actionType == 1 && canNavigate)
                        {
                            MenuNavigation(currentButton.navigation.selectOnRight);
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
                if (currentScrollRect == null && currentPopup == null && canNavigate)
                {
                    MenuNavigation(currentButton.navigation.selectOnUp);
                }
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                if (currentScrollRect == null && currentPopup == null && canNavigate)
                {
                    MenuNavigation(currentButton.navigation.selectOnDown);
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (currentPopup != null && currentPopup.actionType == 1 && canNavigate)
                {
                    MenuNavigation(currentButton.navigation.selectOnLeft);
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (currentPopup != null && currentPopup.actionType == 1 && canNavigate)
                {
                    MenuNavigation(currentButton.navigation.selectOnRight);
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

            float verticalAxis = Input.GetAxis("LeftStickY");

            if (Mathf.Abs(verticalAxis) > stickDeadzone)
            {
                if (currentScrollRect == null && currentPopup == null && canNavigate)
                {
                    if (lastNavigationTime > stickNavigationCooldown)
                    {
                        Debug.Log(verticalAxis);

                        if (verticalAxis > stickDeadzone)
                        {
                            MenuNavigation(currentButton.navigation.selectOnUp);
                        }
                        else if (verticalAxis < -stickDeadzone)
                        {
                            MenuNavigation(currentButton.navigation.selectOnDown);
                        }

                        lastNavigationTime = 0f;
                    }
                }
                else if (currentScrollRect != null && currentPopup == null && canNavigate)
                {
                    ScrollNavigation(new Vector2(0, verticalAxis));
                }
            }
        }

        // Toggle visibility for cursors
        if (currentSelection != null)
        {
            if (currentScrollRect == null && currentPopup == null)
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
    public void AddButton(string buttonText, UnityEngine.Events.UnityAction onClickAction, int menuId, string translationId)
    {
        // Instantiate the button prefab
        GameObject newButton = Instantiate(buttonPrefab, menus[menuId]);

        // Set the button text
        GameObject buttonTextComponent = newButton.transform.Find("Text").gameObject;
        Text text = buttonTextComponent.GetComponent<Text>();
        text.text = buttonText;

        // Translate button text
        I18nTextTranslator translator = buttonTextComponent.GetComponent<I18nTextTranslator>();
        translator.textId = translationId;

        // Add the click action to the button
        Button buttonComponent = newButton.GetComponent<Button>();
        buttonComponent.onClick.AddListener(onClickAction);

        // Add the button to the correct menu list in the dictionary
        if (!menuButtons.ContainsKey(menuId))
        {
            menuButtons[menuId] = new List<GameObject>();
        }

        // Handle navigation setup
        int buttonIndex = menuButtons[menuId].Count;

        if (buttonIndex > 0)
        {
            // Get the previous button
            GameObject previousButton = menuButtons[menuId][buttonIndex - 1];
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

        // Add the new button to the list
        menuButtons[menuId].Add(newButton);
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

    // Clicks the currently selected button
    private void ClickSelectedButton()
    {
        GameObject currentSelected = EventSystem.current.currentSelectedGameObject;
        if (currentSelected != null)
        {
            Button buttonComponent = currentSelected.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.onClick.Invoke();
            }
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

    public void ChangeMenu(int menuId)
    {
        // add current menu ID to history
        if (currentMenuId != menuId && !isNavigatingBack)
        {
            menuHistory.Push(currentMenuId);
        }

        foreach (Transform menu in menus)
        {
            if (menu != menus[menuId])
            {
                menu.gameObject.SetActive(false);
            }
        }

        menus[menuId].gameObject.SetActive(true);

        currentMenuId = menuId;

        if (currentScrollRect == null && currentPopup == null)
        {
            if (menuButtons.ContainsKey(menuId) && menuButtons[menuId].Count > 0)
            {
                // Enable first button visual
                Button newButton = menuButtons[menuId][0].GetComponent<Button>();
                newButton.Select();

                currentButton = newButton;

                buttonAudio.Play();
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
}