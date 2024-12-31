using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardData : MonoBehaviour
{
	public string cardId;
	public Image cursorImage;
	
	void Update()
	{
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            Color color = cursorImage.color;
            color.a = 1f;
            cursorImage.color = color;
        }
        else
        {
            Color color = cursorImage.color;
            color.a = 0f;
            cursorImage.color = color;
        }

    }
}