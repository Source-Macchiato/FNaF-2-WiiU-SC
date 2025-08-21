using UnityEngine;

public class AchievementsSFX : MonoBehaviour
{
	[SerializeField] private AudioSource audioSource;

	public void PlaySFX()
    {
        audioSource.Play();
    }
}
