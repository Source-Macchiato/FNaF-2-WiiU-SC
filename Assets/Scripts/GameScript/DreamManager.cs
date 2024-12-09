using UnityEngine;

public class DreamManager : MonoBehaviour
{
	public RectTransform officeRect;
	private float lastOfficePositionX;

	[Header("Audios")]
	public AudioSource machineTurnAudio;

	void Start()
	{
        // Assign last office horizontal position
        lastOfficePositionX = officeRect.anchoredPosition.x;
    }
	
	void Update()
	{
        NoiseWhenMoving();
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
}