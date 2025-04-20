using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSelectionHandler : MonoBehaviour, ISelectHandler
{
    Button button;
    MenuManager menuManager;

    void Start()
    {
        button = GetComponent<Button>();
        menuManager = FindObjectOfType<MenuManager>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        menuManager.Select(button);
        menuManager.AutoScroll();
        menuManager.ToggleCursorVisibility();
    }
}