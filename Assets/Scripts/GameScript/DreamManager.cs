﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DreamManager : MonoBehaviour
{
	public RectTransform officeRect;
    public TextMeshProUGUI statusText;
	private float lastOfficePositionX;
    private int nightNumber;
    private int introDreamPlayed;

    [Header("Images")]
    public Image chicaImage;
    public Image bonnieImage;
    public Image goldenFreddyImage;
    public Image puppetImage;

    [Header("Sprites")]
    public Sprite[] chicaSprites;
    public Sprite[] bonnieSprites;

	[Header("Audios")]
	public AudioSource machineTurnAudio;

    public float timer;

    // Scripts
    SaveGameState saveGameState;
    SaveManager saveManager;

    void Start()
	{
        // Get scripts
        saveGameState = FindObjectOfType<SaveGameState>();
        saveManager = FindObjectOfType<SaveManager>();

        // Load
        nightNumber = SaveManager.LoadNightNumber();
        introDreamPlayed = SaveManager.LoadIntroDreamPlayed();

        // Assign last office horizontal position
        lastOfficePositionX = officeRect.anchoredPosition.x;

        statusText.enabled = false;

        CharactersStatus();
    }
	
	void Update()
	{
        NoiseWhenMoving();

        if (timer >= 30f)
        {

        }

        timer = Time.time;
	}

	private void NoiseWhenMoving()
	{
        // Check if office moved
        if (officeRect.anchoredPosition.x != lastOfficePositionX)
        {
            // If audio is stopped play it
            if (machineTurnAudio.isPlaying == false)
            {
                machineTurnAudio.Play();
            }

            // Assign last office horizontal position
            lastOfficePositionX = officeRect.anchoredPosition.x;
        }
        else
        {
            // If audio is playing stop it
            if (machineTurnAudio.isPlaying == true)
            {
                machineTurnAudio.Stop();
            }
        }
    }

    private void CharactersStatus()
    {
        if (introDreamPlayed == 0)
        {
            chicaImage.sprite = chicaSprites[0];
            bonnieImage.sprite = bonnieSprites[0];
            goldenFreddyImage.enabled = false;
            puppetImage.enabled = false;

            statusText.text = "err";
        }
        if (nightNumber == 1)
        {
            chicaImage.sprite = chicaSprites[1];
            bonnieImage.sprite = bonnieSprites[1];
            goldenFreddyImage.enabled = false;
            puppetImage.enabled = false;

            statusText.text = "it's me";
        }
        else if (nightNumber == 2)
        {
            chicaImage.sprite = chicaSprites[2];
            bonnieImage.sprite = bonnieSprites[3];
            goldenFreddyImage.enabled = true;
            puppetImage.enabled = false;

            statusText.text = "it's me";
        }
        else if (nightNumber == 3)
        {
            chicaImage.sprite = chicaSprites[3];
            bonnieImage.sprite = bonnieSprites[3];
            goldenFreddyImage.enabled = false;
            puppetImage.enabled = true;

            statusText.text = "it's me";
        }
    }
}