using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CardSwitcherData : MonoBehaviour
{
    public int difficultyValue = 0;
	public int minValue = 0;
	public int maxValue = 20;

	public TextMeshProUGUI valueText;
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
    }
}
