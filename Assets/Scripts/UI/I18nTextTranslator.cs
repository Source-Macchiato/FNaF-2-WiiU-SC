using UnityEngine;
using UnityEngine.UI;

public class I18nTextTranslator : MonoBehaviour
{
    public string textId;
    private Text textComponent;
    private string currentLanguage;

    void Start()
    {
        textComponent = GetComponent<Text>();

        if (textComponent == null)
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
        textComponent.text = translatedText;
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