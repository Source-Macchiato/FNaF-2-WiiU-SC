using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class FoxyGoGoGoController : MonoBehaviour
{
    // Speed at which the player moves
    private float playerSpeed = 2f;

    // Reference to the bear GameObject
    public GameObject player;
    private RectTransform playerRect;
    private Animator playerAnimator;

    public Image[] Children;
    public Sprite DeadChild;

    // Array of GameObjects that the bear cannot pass through
    public GameObject[] CollidableObjects;

    // Reference to the movable object
    public RectTransform mapRect;
    public GameObject fireworksContainer;
    public GameObject purpleGuy;
	public Animator JumpscareAnimator;
	public AudioSource Jumpscare;

    // Array of sprites for different states
    public TextMeshProUGUI getReadyText;

    // Initial positions for reset
    private Vector3 initialPlayerPosition;

    // Movement lock to control when the bear can move
    private bool canMove = false;
    private bool reachedEventPosition = false;

    private int roomId = 0;
    private int phaseId = 0;

    // Scripts
    PlayerMovement playerMovement;

    void Start()
    {
        // Get scripts
        playerMovement = FindObjectOfType<PlayerMovement>();

        playerRect = player.GetComponent<RectTransform>();
        playerAnimator = player.GetComponent<Animator>();

        // Store initial positions for reset
        initialPlayerPosition = player.transform.position;

        // Elements to deactivate when scene starts
        purpleGuy.SetActive(false);
        fireworksContainer.SetActive(false);

        // Start the game with the initial state
        StartCoroutine(InitialState());
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerMovement();
        PlayerPositionAndRoomBasedEvents();
    }

    IEnumerator InitialState()
    {
        canMove = false;
        roomId = 0;
        reachedEventPosition = false;

        getReadyText.text = "Get Ready!";

        if (phaseId == 2)
        {
            purpleGuy.SetActive(true);

            foreach (Image child in Children)
            {
                child.sprite = DeadChild;
            }
        }

        yield return new WaitForSeconds(5f);

        StartCoroutine(GoGoGoAnimation());
        
        canMove = true;
    }

    IEnumerator GoGoGoAnimation()
    {
        while (roomId != 1 || playerRect.localPosition.x <= -230f)
        {
            getReadyText.text = "Go! Go! Go!";

            yield return new WaitForSeconds(0.1f);

            if (roomId == 1 && playerRect.localPosition.x > -230f)
            {
                yield break;
            }

            getReadyText.text = string.Empty;

            yield return new WaitForSeconds(0.1f);
        }
    }

    void HandlePlayerMovement()
    {
        // Play the appropriate animation based on the last horizontal movement direction
        if (playerMovement.isMoving && canMove)
        {
            Vector3 newPosition = player.transform.position;

            // Normalize direction to prevent faster diagonal movement
            Vector2 direction = playerMovement.playerDirection.normalized;
            newPosition.x += direction.x * playerSpeed * Time.deltaTime;
            newPosition.y += direction.y * playerSpeed * Time.deltaTime;

            player.transform.position = newPosition;

            // Play the appropriate animation based on the last horizontal movement direction
            if (playerSpeed > 0f)
            {
                if (direction.x < -0.1f)
                {
                    playerAnimator.Play("BearLeft");
                }
                else if (direction.x > 0.1f)
                {
                    playerAnimator.Play("BearRight");
                }
            }
        }
    }

    void PlayerPositionAndRoomBasedEvents()
    {
        if (roomId == 0 && playerRect.localPosition.x >= 458f)
        {
            // Move map
            mapRect.localPosition = new Vector3(-858f, 0f, 0f);

            // Move player position
            Vector3 newPlayerPosition = playerRect.localPosition;
            newPlayerPosition.x -= 858f;
            playerRect.localPosition = newPlayerPosition;            

            roomId = 1;
        }
        else if (roomId == 1 && playerRect.localPosition.x <= -403f)
        {
            // Move map
            mapRect.localPosition = Vector3.zero;

            // move player position
            Vector3 newPlayerPosition = playerRect.localPosition;
            newPlayerPosition.x += 858f;
            playerRect.localPosition = newPlayerPosition;

            roomId = 0;
        }
        else if (roomId == 1 && playerRect.localPosition.x > -230f)
        {
            if (!reachedEventPosition)
            {
                reachedEventPosition = true;

                getReadyText.text = "Hurray!";

                if (phaseId != 2)
                {
                    StartCoroutine(Firework());
                }
                else
                {
                    StartCoroutine(EndSequence());
                }
            }
        }
    }

    IEnumerator Firework()
    {
        canMove = false;

        yield return new WaitForSeconds(0.8f);

        fireworksContainer.SetActive(true);

        yield return new WaitForSeconds(2.2f);

        fireworksContainer.SetActive(false);

        phaseId++;

        // Reset map and player position
        mapRect.localPosition = Vector3.zero;
        player.transform.position = initialPlayerPosition;

        StartCoroutine(InitialState());
    }

    IEnumerator EndSequence()
    {
        Jumpscare.Play();
        JumpscareAnimator.Play("WitheredFoxy");

        yield return new WaitForSeconds(0.6f);

        SceneManager.LoadScene("MainMenu");
    }
}
