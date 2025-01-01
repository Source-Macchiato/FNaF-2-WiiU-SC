﻿using System.Collections.Generic;
using System.IO;
using UnityEngine;
using RTLTMPro;

public class SubtitlesManager : MonoBehaviour
{
    public RTLTextMeshPro subtitlesText;

    private List<string> subtitleIdentifiers;
    private List<float> displayDurations;

    private float displayStartTime;
    private int currentIndex = 0;
    private bool isDelayOver = false;

    private int nightNumber;

    void Start()
    {
        subtitleIdentifiers = new List<string>();
        displayDurations = new List<float>();

        nightNumber = SaveManager.LoadNightNumber();

        if (nightNumber >= 0 && nightNumber <= 5)
        {
            LoadSubtitlesFromCSV("Data/night" + (nightNumber + 1));
        }

        displayStartTime = Time.timeSinceLevelLoad;
    }

    void Update()
    {
        if (nightNumber >= 0 && nightNumber <= 5)
        {
            if (!isDelayOver)
            {
                if (Time.timeSinceLevelLoad >= displayStartTime)
                {
                    isDelayOver = true;
                    DisplaySubtitle();
                }
                return;
            }

            if (currentIndex >= subtitleIdentifiers.Count)
            {
                return;
            }

            if (Time.timeSinceLevelLoad >= displayStartTime + displayDurations[currentIndex])
            {
                currentIndex++;

                if (currentIndex < subtitleIdentifiers.Count)
                {
                    DisplaySubtitle();
                }
                else
                {
                    subtitlesText.text = null;
                }
            }
        }
    }

    void LoadSubtitlesFromCSV(string filePath)
    {
        TextAsset csvFile = Resources.Load<TextAsset>(filePath);

        if (csvFile == null)
        {
            Debug.Log("CSV file not found at " + filePath);
            return;
        }

        using (StringReader reader = new StringReader(csvFile.text))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 2)
                {
                    subtitleIdentifiers.Add(parts[0]);

                    float duration;
                    if (float.TryParse(parts[1], out duration))
                    {
                        displayDurations.Add(duration);
                    }
                    else
                    {
                        Debug.LogWarning("Invalid duration format in line: " + line);
                    }
                }
                else
                {
                    Debug.LogWarning("Invalid CSV line format: " + line);
                }
            }
        }
    }

    void DisplaySubtitle()
    {
        string translatedText = GetTranslatedText(subtitleIdentifiers[currentIndex]);

        subtitlesText.text = translatedText;

        displayStartTime = Time.timeSinceLevelLoad;
    }

    string GetTranslatedText(string identifier)
    {
        if (I18n.Texts.ContainsKey(identifier))
        {
            return I18n.Texts[identifier];
        }
        else
        {
            return identifier;
        }
    }
}