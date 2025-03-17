using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GiveGiftsController : MonoBehaviour
{
    public float playerSpeed = 1f;
    
    public GameObject player;
    public GameObject gameContainer;
    public GameObject kidFive;
    
    public Sprite[] puppetSprites;
    
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI giveGiftsText;
    
    public Animator JumpscareAnimator;
    public Animator linesAnimator;

    public AudioSource Jumpscare;
    public AudioSource staticAudio;
    public AudioSource cakeAudio;
    public AudioSource effectAudio;
    
    public KidGift[] kidGifts;
    
    private bool secondPhaseStarted = false;

    public int score = 0;

    // Scripts
    PlayerMovement playerMovement;

    void Start()
    {
        // Get scripts
        playerMovement = FindObjectOfType<PlayerMovement>();

        kidFive.SetActive(false);
        staticAudio.volume = 0.4f;

        UpdateScoreText();
    }

    void Update()
    {
        HandleBearMovement();
    }

    void HandleBearMovement()
    {
        if (playerMovement.isMoving)
        {
            Vector3 newPosition = player.transform.position;

            // Normalize direction to prevent faster diagonal movement
            Vector2 direction = playerMovement.playerDirection.normalized;
            newPosition.x += direction.x * playerSpeed * Time.deltaTime;
            newPosition.y += direction.y * playerSpeed * Time.deltaTime;

            player.transform.position = newPosition;

            // Use right sprite based on the last horizontal movement direction
            if (playerSpeed > 0f)
            {
                if (direction.x < -0.1f)
                {
                    player.GetComponent<Image>().sprite = puppetSprites[1];
                }
                else if (direction.x > 0.1f)
                {
                    player.GetComponent<Image>().sprite = puppetSprites[0];
                }
            }
        }
    }

    public void UpdateScoreText()
    {
        scoreText.text = score.ToString("D4");
    }

    public void PlayEndSequence()
    {
        StartCoroutine(EndSequence());
    }

    public IEnumerator SecondPhase()
    {
        if (secondPhaseStarted)
        {
            yield break;
        }

        yield return new WaitForSeconds(1f);

        foreach (KidGift kidGift in kidGifts)
        {
            if (kidGift.isTriggered)
            {
                yield break;
            }
        }

        foreach (KidGift kidGift in kidGifts)
        {
            kidGift.gift.SetActive(false);
            kidGift.secondPhase = true;
        }

        giveGiftsText.text = "Give Life.";

        secondPhaseStarted = true;
    }

    IEnumerator EndSequence()
    {
        kidFive.SetActive(true);

        yield return new WaitForSeconds(1f / 20);

        kidFive.SetActive(false);

        score = Random.Range(0, 9999);
        UpdateScoreText();

        Jumpscare.Play();
        JumpscareAnimator.Play("GoldenFreddy");

        yield return new WaitForSeconds(0.6f);

        gameContainer.SetActive(false);
        Jumpscare.Stop();
        JumpscareAnimator.gameObject.SetActive(false);
        linesAnimator.Play("Red");
        staticAudio.volume = 1f;

        yield return new WaitForSeconds(2f);

        SceneManager.LoadSceneAsync("MainMenu");
    }
}