using UnityEngine;

public class KidGift : MonoBehaviour
{
    public GameObject gift;
    public GameObject mask;

    private bool giftEnabled = false;
    private bool maskEnabled = false;
    public bool secondPhase = false;
    public bool isTriggered = false;

    private GiveGiftsController giveGiftsController;

	void Start()
	{
        giveGiftsController = FindObjectOfType<GiveGiftsController>();

        gift.SetActive(false);
        mask.SetActive(false);
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTriggered = true;

            if (!giftEnabled)
            {
                gift.SetActive(true);

                giveGiftsController.score += 100;
                giveGiftsController.UpdateScoreText();

                giftEnabled = true;
            }
            else
            {
                if (!maskEnabled && secondPhase)
                {
                    mask.SetActive(true);

                    giveGiftsController.score += 100;
                    giveGiftsController.UpdateScoreText();

                    if (giveGiftsController.score == 800)
                    {
                        StartCoroutine(giveGiftsController.EndGame());
                    }

                    maskEnabled = true;
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTriggered = false;

            if (giveGiftsController.score == 400)
            {
                StartCoroutine(giveGiftsController.SecondPhase());
            }
        }
    }
}
