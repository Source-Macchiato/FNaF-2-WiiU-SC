using UnityEngine;
using RTLTMPro;

public class DisplayNightNumber : MonoBehaviour
{
	private int nightNumber;
	private RTLTextMeshPro nightNumberText;

	void Start()
	{
		nightNumber = SaveManager.LoadNightNumber();
		nightNumberText = GetComponent<RTLTextMeshPro>();

		nightNumberText.text = (nightNumber + 1).ToString();
	}
}
