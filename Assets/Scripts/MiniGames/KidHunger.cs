using UnityEngine;

public class KidHunger : MonoBehaviour
{
	public int kidIndex;
	private Minigame1Controller takeCakeController;
    private bool isTriggered = false;

	void Start()
	{
		takeCakeController = FindObjectOfType<Minigame1Controller>();
	}

    void Update()
    {
        if (isTriggered && takeCakeController.Hunger[kidIndex] < 10f)
        {
            takeCakeController.Hunger[kidIndex] = 20f;
            takeCakeController.cakeAudio.Play();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTriggered = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTriggered = false;
        }
    }
}
