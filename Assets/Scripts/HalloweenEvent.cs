using System;
using UnityEngine;

public class HalloweenEvent : MonoBehaviour
{
	public GameObject lights;
	public GameObject pumpkin;

	void Start()
	{
		DateTime currentDate = DateTime.Now;

		if (currentDate.Day == 31 && currentDate.Month == 10)
		{
			lights.SetActive(true);
			pumpkin.SetActive(true);
		}
		else
		{
			lights.SetActive(false);
			pumpkin.SetActive(false);
		}
	}
}