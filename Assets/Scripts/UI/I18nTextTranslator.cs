using RTLTMPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class I18nTextTranslator : MonoBehaviour
{
    public string textId;
    private Text textComponent;
    private RTLTextMeshPro tmpTextComponent;
    private string currentLanguage;

    [Header("Fonts")]
    public TMP_FontAsset mainFont;
    public TMP_FontAsset arabicFont;

    void Start()
    {
        textComponent = GetComponent<Text>();
        tmpTextComponent = GetComponent<RTLTextMeshPro>();

        if (textComponent == null && tmpTextComponent == null)
        {
            return;
        }

        if (I18n.Texts == null)
        {
            return;
        }

        // Get language
        currentLanguage = I18n.GetLanguage();

        // Load text
        UpdateText();
    }

    void Update()
    {
        // Check if current language is not th I18n language
        if (currentLanguage != I18n.GetLanguage())
        {
            // Reload text
            UpdateText();

            // Set I18n language as current language
            currentLanguage = I18n.GetLanguage();
        }
    }

    public void UpdateText()
    {
        if (string.IsNullOrEmpty(textId))
        {
            return;
        }

        string translatedText = GetTranslatedText();

        if (textComponent != null)
        {
            textComponent.text = translatedText;
        }

        if (tmpTextComponent != null)
        {
            tmpTextComponent.text = translatedText;

            if (I18n.GetLanguage() == "ar")
            {
                if (arabicFont != null)
                {
                    tmpTextComponent.font = arabicFont;
                }
            }
            else
            {
                if (mainFont != null)
                {
                    tmpTextComponent.font = mainFont;
                }
            }
        }
    }

    string GetTranslatedText()
    {
        string translatedText;

        if (I18n.Texts.TryGetValue(textId, out translatedText))
        {
            return translatedText;
        }
        else
        {
            return textId;
        }
    }
}