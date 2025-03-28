﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardSwitcherData : MonoBehaviour
{
    public string id;
    public int difficultyValue = 0;
	public int minValue = 0;
	public int maxValue = 20;

	public TextMeshProUGUI valueText;
    public Image coverImage;
    public GameObject cursorObject;

	void Start()
	{
        UpdateCardSwitcher();
	}

    void Update()
    {
        cursorObject.SetActive(EventSystem.current.currentSelectedGameObject == gameObject);
    }

    public void IncreaseDifficulty()
    {
        if (difficultyValue >= minValue && difficultyValue < maxValue)
        {
            difficultyValue++;

            UpdateCardSwitcher();
        }
    }

    public void DecreaseDifficulty()
    {
        if (difficultyValue > minValue && difficultyValue <= maxValue)
        {
            difficultyValue--;

            UpdateCardSwitcher();
        }
    }

    public void UpdateCardSwitcher()
    {
        valueText.text = difficultyValue.ToString();

        ChangeCoverOpacity(difficultyValue > 0 ? 1f : 0.2f);
    }

    private void ChangeCoverOpacity(float opacity)
    {
        Color color = coverImage.color;
        color.a = opacity;

        coverImage.color = color;
    }
}
