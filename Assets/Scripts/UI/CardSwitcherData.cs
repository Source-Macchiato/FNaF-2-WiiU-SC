using UnityEngine;
using TMPro;

public class CardSwitcherData : MonoBehaviour
{
    public int difficultyValue = 0;
	public int minValue = 0;
	public int maxValue = 20;

	public TextMeshProUGUI valueText;

	void Start()
	{
        UpdateCardSwitcher();	
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
    }
}
