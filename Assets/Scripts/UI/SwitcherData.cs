using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using RTLTMPro;

public class SwitcherData : MonoBehaviour
{
    public string switcherId;
    public int currentOptionId;
    public string[] optionsName;

    public RTLTextMeshPro text;
    public I18nTextTranslator i18nTextTranslator;
    public Image[] inputIcons;

    public UnityEvent events;

    void Start()
    {
        events.Invoke();

        UpdateText();
    }

    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject)
        {
            ChangeImageOpacity(inputIcons[0], currentOptionId > 0 ? 1f : 0.5f);
            ChangeImageOpacity(inputIcons[1], currentOptionId < optionsName.Length - 1 ? 1f : 0.5f);
        }
        else
        {
            foreach (Image inputIcon in inputIcons)
            {
                ChangeImageOpacity(inputIcon, 0.5f);
            }
        }
    }

    public void IncreaseOptions()
    {
        if (currentOptionId >= 0 && currentOptionId < optionsName.Length - 1)
        {
            currentOptionId++;

            events.Invoke();

            UpdateText();
        }
    }

    public void DecreaseOptions()
    {
        if (currentOptionId > 0 && currentOptionId <= optionsName.Length - 1)
        {
            currentOptionId--;

            events.Invoke();

            UpdateText();
        }
    }

    public void UpdateText()
    {
        if (text != null)
        {
            text.text = optionsName[currentOptionId];
        }

        if (i18nTextTranslator != null)
        {
            i18nTextTranslator.textId = optionsName[currentOptionId];
            i18nTextTranslator.UpdateText();
        }
    }

    private void ChangeImageOpacity(Image image, float opacity)
    {
        Color color = image.color;
        color.a = opacity;

        image.color = color;
    }
}