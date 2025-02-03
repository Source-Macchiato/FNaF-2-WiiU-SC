using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSelectionHandler : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        button.OnPointerEnter(null);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        button.OnPointerExit(null);
    }
}
