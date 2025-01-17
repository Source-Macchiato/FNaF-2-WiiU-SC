using UnityEngine;
using UnityEngine.EventSystems;

public class PopupOptions : MonoBehaviour
{
	public GameObject[] buttons;
	public GameObject[] cursors;

	AnalyticsData analyticsData;

	void Start()
	{
		analyticsData = FindObjectOfType<AnalyticsData>();
	}
	
	void Update()
	{
		for (int i = 0; i < buttons.Length; i++)
		{
			cursors[i].SetActive(EventSystem.current.currentSelectedGameObject == buttons[i]);
        }
	}

	public void ShareAnalytics(bool share)
	{
		analyticsData.ShareAnalytics(share);
	}
}
