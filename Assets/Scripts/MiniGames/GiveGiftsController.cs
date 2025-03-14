using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GiveGiftsController : MonoBehaviour
{
    public float BearSpeed = 5f;
    public GameObject player;
    public GameObject[] Walls; // Array of wall GameObjects
    public GameObject[] Children;
    public GameObject[] Hats;
    public GameObject[] Gifts;
    public float Proximity = 50f;
    public Image StateImage;
    public Sprite[] puppetSprites;
    public Sprite GiveLife;
    public TextMeshProUGUI ScoreText;
    public Animator JumpscareAnimator;
    public AudioSource Jumpscare;

    private int score = 0;
    private int childrenWithHats = 0;
    private bool givingHats = false;

    // Scripts
    PlayerMovement bearMovement;

    void Start()
    {
        // Get scripts
        bearMovement = FindObjectOfType<PlayerMovement>();

        UpdateScoreText();

        // Disable all gifts when game starts
        foreach (GameObject gift in Gifts)
        {
            gift.SetActive(false);
        }
    }

    void Update()
    {
        HandleBearMovement();
        CheckProximityToChildren();
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

    void CheckProximityToChildren()
    {
        for (int i = 0; i < Children.Length; i++)
        {
            if (Children[i] != null && Vector3.Distance(player.transform.position, Children[i].transform.position) <= Proximity)
            {
                if (!givingHats && !Gifts[i].activeSelf) // Gift the child if not already given
                {
                    Gifts[i].SetActive(true);
                    score += 100;
                    UpdateScoreText();

                    if (score == 400)
                    {
                        givingHats = true;
                        StartCoroutine(RestartMinigame());
                    }
                }
                else if (givingHats && !Hats[i].activeSelf) // Give hat to the child if in proximity
                {
                    Hats[i].SetActive(true);
                    score += 100;
                    UpdateScoreText();
                    childrenWithHats++;

                    if (childrenWithHats >= Children.Length)
                    {
                        TriggerAllChildrenHaveHats();
                    }

                    if (score == 800)
                    {
                        StartCoroutine(TriggerEndGame());
                    }
                }
            }
        }
    }

    void UpdateScoreText()
    {
        ScoreText.text = score.ToString("D4");
    }

    IEnumerator RestartMinigame()
    {
        yield return new WaitForSeconds(0.7f);

        // Deactivate all gifts
        foreach (GameObject gift in Gifts)
        {
            gift.SetActive(false);
        }

        // Reset the childrenWithHats counter for the next phase
        StateImage.sprite = GiveLife;
        childrenWithHats = 0;
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
