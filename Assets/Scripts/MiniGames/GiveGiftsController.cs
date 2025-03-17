using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GiveGiftsController : MonoBehaviour
{
    public float BearSpeed = 5f;
    public GameObject player;
    public float Proximity = 50f;
    public Image StateImage;
    public Sprite[] puppetSprites;
    public Sprite GiveLife;
    public TextMeshProUGUI ScoreText;
    public Animator JumpscareAnimator;
    public AudioSource Jumpscare;

    public int score = 0;

    // Scripts
    PlayerMovement bearMovement;

    void Start()
    {
        // Get scripts
        bearMovement = FindObjectOfType<PlayerMovement>();

        UpdateScoreText();
    }

    void Update()
    {
        HandleBearMovement();
    }

    void HandleBearMovement()
    {
        if (bearMovement.isMoving)
        {
            Vector3 newPosition = player.transform.position;

            // Normalize direction to prevent faster diagonal movement
            Vector2 direction = bearMovement.playerDirection.normalized;
            newPosition.x += direction.x * BearSpeed * Time.deltaTime;
            newPosition.y += direction.y * BearSpeed * Time.deltaTime;

            player.transform.position = newPosition;

            // Use right sprite based on the last horizontal movement direction
            if (BearSpeed > 0f)
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
        ScoreText.text = score.ToString("D4");
    }

    IEnumerator RestartMinigame()
    {
        yield return new WaitForSeconds(0.7f);

        // Reset the childrenWithHats counter for the next phase
        StateImage.sprite = GiveLife;
    }

    void TriggerAllChildrenHaveHats()
    {
        Debug.Log("All children have received hats!");
        // Additional logic for when all children have hats
    }

    IEnumerator TriggerEndGame()
    {
        Debug.Log("Game Over! Final score: " + score);
        Jumpscare.Play();
        JumpscareAnimator.Play("GoldenFreddy");
        yield return new WaitForSeconds(0.24f);
        SceneManager.LoadScene("MainMenu");
    }
}
