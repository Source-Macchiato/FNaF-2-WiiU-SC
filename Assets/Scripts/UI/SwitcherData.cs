using UnityEngine;
using RTLTMPro;

public class SwitcherData : MonoBehaviour
{
    public string switcherId;
    public int currentOptionId;
    public string[] optionsName;

    public RTLTextMeshPro text;
    public I18nTextTranslator i18nTextTranslator;

    void Start()
    {
        UpdateText();
    }

    public void IncreaseOptions()
    {
        if (currentOptionId >= 0 && currentOptionId < optionsName.Length - 1)
        {
            currentOptionId++;

            UpdateText();
        }
    }

    public void DecreaseOptions()
    {
        if (currentOptionId > 0 && currentOptionId <= optionsName.Length - 1)
        {
            currentOptionId--;

            UpdateText();
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