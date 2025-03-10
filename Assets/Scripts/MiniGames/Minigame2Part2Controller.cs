using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Minigame2Part2Controller : MonoBehaviour
{
    public float BearSpeed = 5f;
    public GameObject Bear;
    public GameObject[] Walls; // Array of wall GameObjects
    public GameObject[] Children;
    public GameObject[] Hats;
    public GameObject[] Gifts;
    public float Proximity = 50f;
    public Image StateImage;
    public Sprite GiveLife;
    public Text ScoreText;
    public Animator JumpscareAnimator;
    public AudioSource Jumpscare;

    private int score = 0;
    private int childrenWithHats = 0;
    private bool givingHats = false;

    // Scripts
    BearMovement bearMovement;

    void Start()
    {
        // Get scripts
        bearMovement = FindObjectOfType<BearMovement>();

        UpdateScoreText();
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
            Vector3 newPosition = Bear.transform.position;

            if (bearMovement.playerDirection == Vector2.up)
            {
                newPosition.y += BearSpeed * Time.deltaTime;
            }
            else if (bearMovement.playerDirection == Vector2.left)
            {
                newPosition.x -= BearSpeed * Time.deltaTime;
            }
            else if (bearMovement.playerDirection == Vector2.down)
            {
                newPosition.y -= BearSpeed * Time.deltaTime;
            }
            else if (bearMovement.playerDirection == Vector2.right)
            {
                newPosition.x += BearSpeed * Time.deltaTime;
            }

            // Apply the movement
            Bear.transform.position = newPosition;

            // have to make a IsCollidingWithObjects like other scripts
            // Check for collisions with walls and reset position if needed
            /*foreach (GameObject wall in Walls)
            {
                if (Bear.GetComponent<Collider2D>().IsTouching(wall.GetComponent<Collider2D>()))
                {
                    Bear.transform.Translate(-movement); // Undo movement if colliding
                    break;
                }
            }*/
        }
    }

    void CheckProximityToChildren()
    {
        for (int i = 0; i < Children.Length; i++)
        {
            if (Children[i] != null && Vector3.Distance(Bear.transform.position, Children[i].transform.position) <= Proximity)
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
