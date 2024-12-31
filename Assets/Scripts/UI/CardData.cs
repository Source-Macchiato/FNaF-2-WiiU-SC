using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardData : MonoBehaviour
{
    public string cardId;
    public Image cursorImage;
    public Animator cursorAnimator;

    void Update()
    {
        cursorImage.enabled = EventSystem.current.currentSelectedGameObject == gameObject;
    }

    public void SelectedCardAnimation()
    {
        cursorAnimator.Play("Selected");
    }
}