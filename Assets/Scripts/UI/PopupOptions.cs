using UnityEngine;

public class PopupOptions : MonoBehaviour
{
	AnalyticsData analyticsData;

	void Start()
	{
		analyticsData = FindObjectOfType<AnalyticsData>();
	}
	
	void Update()
	{
		
	}

	public void ShareAnalytics(bool share)
	{
		analyticsData.ShareAnalytics(share);
	}
}
