using UnityEngine;
using UnityEngine.Events;
using RTLTMPro;

public class SwitcherData : MonoBehaviour
{
    public string switcherId;
    public int currentOptionId;
    public string[] optionsName;

    public RTLTextMeshPro text;
    public I18nTextTranslator i18nTextTranslator;

    public UnityEvent events;

    void Start()
    {
        UpdateText();

        events.Invoke();
    }

    public void IncreaseOptions()
    {
        if (currentOptionId >= 0 && currentOptionId < optionsName.Length - 1)
        {
            currentOptionId++;

            UpdateText();

            events.Invoke();
        }
    }

    public void DecreaseOptions()
    {
        if (currentOptionId > 0 && currentOptionId <= optionsName.Length - 1)
        {
            currentOptionId--;

            UpdateText();

            events.Invoke();
        }
    }

    void UpdateText()
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
}