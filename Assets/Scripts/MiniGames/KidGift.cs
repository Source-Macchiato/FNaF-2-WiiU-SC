using UnityEngine;

public class KidGift : MonoBehaviour
{
    public GameObject gift;
    public GameObject mask;

    private bool giftEnabled = false;
    private bool maskEnabled = false;

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
            if (!giftEnabled)
            {
                gift.SetActive(true);

                giveGiftsController.score += 100;
                giveGiftsController.UpdateScoreText();

                giftEnabled = true;
            }
            else
            {
                if (!maskEnabled && giveGiftsController.score >= 400)
                {
                    mask.SetActive(true);

                    giveGiftsController.score += 100;
                    giveGiftsController.UpdateScoreText();

                    maskEnabled = true;
                }
            }
        }
    }
}
