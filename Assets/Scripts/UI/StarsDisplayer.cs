using UnityEngine;
using UnityEngine.UI;

public class StarsDisplayer : MonoBehaviour
{
	public GameObject[] starContainers;
	public GameObject[] mainButtons;

	void Start()
	{
		// Enable or disable stars based on save 
		for (int i = 0; i < starContainers.Length; i++)
		{
			starContainers[i].SetActive(SaveManager.LoadUnlockedStars(i));
		}

		mainButtons[2].SetActive(SaveManager.LoadUnlockedStars(0));
		mainButtons[3].SetActive(SaveManager.LoadUnlockedStars(1));

        if (SaveManager.LoadUnlockedStars(1) || SaveManager.LoadUnlockedStars(2))
        {
            Navigation newGameButtonNavigation = mainButtons[0].GetComponent<Button>().navigation;
            newGameButtonNavigation.selectOnUp = mainButtons[3].GetComponent<Button>();
            newGameButtonNavigation.selectOnDown = mainButtons[1].GetComponent<Button>();
            mainButtons[0].GetComponent<Button>().navigation = newGameButtonNavigation;

            Navigation continueButtonNavigation = mainButtons[1].GetComponent<Button>().navigation;
            continueButtonNavigation.selectOnUp = mainButtons[0].GetComponent<Button>();
            continueButtonNavigation.selectOnDown = mainButtons[2].GetComponent<Button>();
            mainButtons[1].GetComponent<Button>().navigation = continueButtonNavigation;

            Navigation sixthNightButtonNavigation = mainButtons[2].GetComponent<Button>().navigation;
            sixthNightButtonNavigation.selectOnUp = mainButtons[1].GetComponent<Button>();
            sixthNightButtonNavigation.selectOnDown = mainButtons[3].GetComponent<Button>();
            mainButtons[2].GetComponent<Button>().navigation = sixthNightButtonNavigation;

            Navigation customNightButtonNavigation = mainButtons[3].GetComponent<Button>().navigation;
            customNightButtonNavigation.selectOnUp = mainButtons[2].GetComponent<Button>();
            customNightButtonNavigation.selectOnDown = mainButtons[0].GetComponent<Button>();
            mainButtons[3].GetComponent<Button>().navigation = customNightButtonNavigation;
        }
        else if (SaveManager.LoadUnlockedStars(0))
        {
            Navigation newGameButtonNavigation = mainButtons[0].GetComponent<Button>().navigation;
            newGameButtonNavigation.selectOnUp = mainButtons[2].GetComponent<Button>();
            newGameButtonNavigation.selectOnDown = mainButtons[1].GetComponent<Button>();
            mainButtons[0].GetComponent<Button>().navigation = newGameButtonNavigation;

            Navigation continueButtonNavigation = mainButtons[1].GetComponent<Button>().navigation;
            continueButtonNavigation.selectOnUp = mainButtons[0].GetComponent<Button>();
            continueButtonNavigation.selectOnDown = mainButtons[2].GetComponent<Button>();
            mainButtons[1].GetComponent<Button>().navigation = continueButtonNavigation;

            Navigation sixthNightButtonNavigation = mainButtons[2].GetComponent<Button>().navigation;
            sixthNightButtonNavigation.selectOnUp = mainButtons[1].GetComponent<Button>();
            sixthNightButtonNavigation.selectOnDown = mainButtons[0].GetComponent<Button>();
            mainButtons[2].GetComponent<Button>().navigation = sixthNightButtonNavigation;
        }
        else
		{
            Navigation newGameButtonNavigation = mainButtons[0].GetComponent<Button>().navigation;
            newGameButtonNavigation.selectOnUp = mainButtons[1].GetComponent<Button>();
            newGameButtonNavigation.selectOnDown = mainButtons[1].GetComponent<Button>();
            mainButtons[0].GetComponent<Button>().navigation = newGameButtonNavigation;

            Navigation continueButtonNavigation = mainButtons[1].GetComponent<Button>().navigation;
            continueButtonNavigation.selectOnUp = mainButtons[0].GetComponent<Button>();
            continueButtonNavigation.selectOnDown = mainButtons[0].GetComponent<Button>();
            mainButtons[1].GetComponent<Button>().navigation = continueButtonNavigation;
        }
	}
}
