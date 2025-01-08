using UnityEngine;
using RTLTMPro;

public class SwitcherData : MonoBehaviour
{
    public string switcherId;
    public int currentOptionId;
    public string[] optionsName;

    public RTLTextMeshPro text;
    public I18nTextTranslator i18nTextTranslator;

    void Update()
    {
        if (text != null)
        {
            if (text.text != optionsName[currentOptionId] || text.text == null)
            {
                text.text = optionsName[currentOptionId];
            }
        }

        if (i18nTextTranslator != null)
        {
            if (i18nTextTranslator.textId != optionsName[currentOptionId])
            {
                i18nTextTranslator.textId = optionsName[currentOptionId];
                i18nTextTranslator.UpdateText();
            }
        }
    }
}