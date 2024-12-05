using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class I18n
{
    public static Dictionary<string, string> Texts { get; private set; }

    static I18n()
    {
        LoadLanguage();
    }

    public static void LoadLanguage()
    {
        if (Texts == null)
        {
            Texts = new Dictionary<string, string>();
        }

        Texts.Clear();

        string lang;

        string languagePlayerPrefs = SaveManager.LoadLanguage();

        if (languagePlayerPrefs == "English")
        {
            lang = "en";
        }
        else if (languagePlayerPrefs == "French")
        {
            lang = "fr";
        }
        else if (languagePlayerPrefs == "Spanish")
        {
            lang = "es";
        }
        else if (languagePlayerPrefs == "Italian")
        {
            lang = "it";
        }
        else
        {
            lang = Get2LetterISOCodeFromSystemLanguage().ToLower();
        }

        string filePath = "I18n/" + lang;

        TextAsset csvFile = Resources.Load<TextAsset>(filePath);
        if (csvFile == null)
        {
            Debug.LogError("Translation file not found: " + filePath);
            return;
        }

        ParseCsv(csvFile.text);
    }

    private static void ParseCsv(string csvContent)
    {
        using (StringReader reader = new StringReader(csvContent))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                // Gérer les champs CSV correctement avec Regex pour séparer clé et valeur
                string[] parts = ParseCsvLine(line);
                if (parts.Length != 2)
                {
                    Debug.LogError("Invalid line format in CSV: " + line);
                    continue;
                }

                string key = parts[0].Trim();
                string value = parts[1].Trim();

                if (!Texts.ContainsKey(key))
                {
                    Texts[key] = value;
                }
            }
        }
    }

    private static string[] ParseCsvLine(string line)
    {
        List<string> result = new List<string>();
        bool insideQuotes = false;
        string currentField = "";

        for (int i = 0; i < line.Length; i++)
        {
            char c = line[i];

            if (c == '"' && (currentField.Length == 0 || (currentField[currentField.Length - 1] != '\\')))
            {
                // Invert state when a quote is found
                insideQuotes = !insideQuotes;
            }
            else if (c == ',' && !insideQuotes)
            {
                // Add current field if comma is found outside quotes
                result.Add(currentField);
                currentField = "";
            }
            else
            {
                // Add character to current field
                currentField += c;
            }
        }

        // Add last field
        if (!string.IsNullOrEmpty(currentField))
        {
            result.Add(currentField);
        }

        return result.ToArray();
    }

    public static string GetLanguage()
    {
        string languagePlayerPrefs = SaveManager.LoadLanguage();

        if (languagePlayerPrefs == "English")
        {
            return "en";
        }
        else if (languagePlayerPrefs == "French")
        {
            return "fr";
        }
        else if (languagePlayerPrefs == "Spanish")
        {
            return "es";
        }
        else if (languagePlayerPrefs == "Italian")
        {
            return "it";
        }
        else
        {
            return Get2LetterISOCodeFromSystemLanguage().ToLower();
        }
    }

    public static string Get2LetterISOCodeFromSystemLanguage()
    {
        SystemLanguage lang = Application.systemLanguage;
        string res = "EN";
        switch (lang)
        {
            case SystemLanguage.English: res = "EN"; break;
            case SystemLanguage.French: res = "FR"; break;
            case SystemLanguage.Spanish: res = "ES"; break;
            case SystemLanguage.Italian: res = "IT"; break;
        }
        return res;
    }
}