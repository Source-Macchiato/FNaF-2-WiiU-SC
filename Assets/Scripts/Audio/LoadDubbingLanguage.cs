using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct LanguageAudio
{
    public string languageCode;
    public AudioClip[] clips;
}

public class LoadDubbingLanguage : MonoBehaviour
{
    [SerializeField] private AudioSource phoneCallAudio;
    [SerializeField] private LanguageAudio[] languageAudios;

    private Dictionary<string, AudioClip[]> audioLookup;
    private int nightNumber;
    private string dubbingLanguage;

    void Awake()
    {
        audioLookup = new Dictionary<string, AudioClip[]>(StringComparer.OrdinalIgnoreCase);

        foreach (var language in languageAudios)
        {
            if (language.languageCode != string.Empty && !audioLookup.ContainsKey(language.languageCode))
            {
                audioLookup[language.languageCode] = language.clips;
            }
        }
    }

    void Start()
    {
        nightNumber = SaveManager.saveData.game.nightNumber;

        if (nightNumber < 0 || nightNumber > 5)
        {
            Debug.Log("Night number '" + nightNumber + "' don't have audio clips.");
            return;
        }

        dubbingLanguage = SaveManager.saveData.settings.dubbingLanguage;
        string lang = dubbingLanguage == string.Empty ? "en" : dubbingLanguage;

        AudioClip[] clips;

        if (!audioLookup.TryGetValue(lang, out clips) && !audioLookup.TryGetValue("en", out clips))
        {
            Debug.Log("no clip for language '" + lang + "' or fallback 'en'.");
            return;
        }

        if (clips == null || nightNumber >= clips.Length || clips[nightNumber] == null)
        {
            Debug.Log("Missing clip for night number '" + nightNumber + "' in language '" + lang + "'.");
            return;
        }

        phoneCallAudio.clip = clips[nightNumber];
        phoneCallAudio.Play();
    }
}