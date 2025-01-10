using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Minigame2Part2Controller : MonoBehaviour
{
    public float BearSpeed = 5f;
    public GameObject Bear;
    public float MinX = 0f;
    public float MaxX = 400f;
    public float MinY = 0f;
    public float MaxY = 240f;
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

    void Start()
    {
        UpdateScoreText();
    }

    void Update()
    {
        HandleBearMovement();
        CheckProximityToChildren();
    }

    void HandleBearMovement()
    {
        Vector3 newPosition = Bear.transform.position;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            newPosition.x += BearSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition.x -= BearSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            newPosition.y += BearSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            newPosition.y -= BearSpeed * Time.deltaTime;
        }

        newPosition.x = Mathf.Clamp(newPosition.x, MinX, MaxX);
        newPosition.y = Mathf.Clamp(newPosition.y, MinY, MaxY);

        Bear.transform.position = newPosition;
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
		SceneManager.LoadScene("MainMenuLoader");
        // For example, load a new scene or display a victory screen.
    }
}
