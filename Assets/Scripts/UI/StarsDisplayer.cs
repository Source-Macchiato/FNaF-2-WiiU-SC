using UnityEngine;

public class StarsDisplayer : MonoBehaviour
{
	public GameObject[] starContainers;

	void Start()
	{
		// Enable or disable stars based on save 
		for (int i = 0; i < starContainers.Length; i++)
		{
			starContainers[i].SetActive(SaveManager.LoadUnlockedStars(i));
		}
	}
}
