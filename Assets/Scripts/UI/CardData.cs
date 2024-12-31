using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardData : MonoBehaviour
{
    public string cardId;
    public Image cursorImage;
    private Color originalColor;
    private Coroutine colorChangeCoroutine;

    void Start()
    {
        // Keep original color
        originalColor = cursorImage.color;
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            // Display cursor
            Color color = cursorImage.color;
            color.a = 1f;
            cursorImage.color = color;
        }
        else
        {
            // Hide cursor
            Color color = cursorImage.color;
            color.a = 0f;
            cursorImage.color = color;

            // Stop coroutine if it is playing and reset color
            if (colorChangeCoroutine != null)
            {
                StopCoroutine(colorChangeCoroutine);
                colorChangeCoroutine = null;
                // Ensure the color is reset when deselected
                cursorImage.color = originalColor;
            }
        }
    }

    public void SelectedCardAnimation()
    {
        colorChangeCoroutine = StartCoroutine(ChangeColorCoroutine());
    }

    private IEnumerator ChangeColorCoroutine()
    {
        // Change color to yellow
        cursorImage.color = Color.yellow;

        yield return new WaitForSeconds(0.2f);

        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            cursorImage.color = originalColor;
        }
    }
}