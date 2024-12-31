using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonOnlySelector : MonoBehaviour
{
    GameObject lastSelected;

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (lastSelected && lastSelected.gameObject.activeSelf && lastSelected.GetComponent<Button>() != null && lastSelected.GetComponent<Button>().interactable)
            {
                EventSystem.current.SetSelectedGameObject(lastSelected);
            }
        }
        else
        {
            lastSelected = EventSystem.current.currentSelectedGameObject;
        }
    }
}