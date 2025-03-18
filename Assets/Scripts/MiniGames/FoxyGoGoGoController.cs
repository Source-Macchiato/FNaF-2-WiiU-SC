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
    public GameObject Fireworks;
    public GameObject PurpleGuy;
	public Animator JumpscareAnimator;
	public AudioSource Jumpscare;

    // X position at which the MoveableObject starts moving
    public float TriggerXPosition = 5f;

    // Array of sprites for different states
    public TextMeshProUGUI getReadyText;

    // Initial positions for reset
    private Vector3 initialPlayerPosition;

    // Movement lock to control when the bear can move
    private bool canMove = false;

    private int resetCount;
    private int roomId = 0;

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

        // Start the game with the initial state
        StartCoroutine(InitialState());
    }

    // Update is called once per frame
    void Update()
    {
        HandleBearMovement();
        CheckPlayerPosition();
    }

    // Coroutine to handle the initial state and resets
    IEnumerator InitialState()
    {
        canMove = false;

        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Set the state to "Go" and unlock bear movement after 2 seconds
        getReadyText.text = "Go! Go! Go!";

        yield return new WaitForSeconds(1f);
        
        canMove = true;
    }

    // Resets the game state
    public void ResetGame()
    {
        resetCount++;
        if (resetCount == 2)
        {
            PurpleGuy.SetActive(true);
            foreach (Image child in Children)
            {
                child.sprite = DeadChild;
            }
        }
        // Reset bear and movable object positions
        mapRect.localPosition = Vector3.zero;
        player.transform.position = initialPlayerPosition;

        // Start the initial state coroutine
        StartCoroutine(InitialState());
    }

    // Handles the bear's movement
    void HandleBearMovement()
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

    // Checks the bear's position to trigger actions based on its X position
    void CheckPlayerPosition()
    {
        // Only activate the trigger if it hasn't been activated for this reset
        if (roomId == 0 && playerRect.localPosition.x >= 458f)
        {
            // Move the movable object by the specified amount on the X axis
            mapRect.localPosition = new Vector3(-858f, 0f, 0f);

            Vector3 newPlayerPosition = playerRect.localPosition;
            newPlayerPosition.x -= 858f;
            playerRect.localPosition = newPlayerPosition;

            // Set StateImage sprite to "Hurray"
            getReadyText.text = "Hurray!";

            StartCoroutine(Firework());
        }
    }

    IEnumerator Firework()
    {
        yield return new WaitForSeconds(0.8f);

        if (resetCount != 2)
        {
            Fireworks.SetActive(true);
        }

        yield return new WaitForSeconds(2.2f);
        
        if (resetCount != 2)
        {
            ResetGame();
            Fireworks.SetActive(false);
        }
		else
		{
			JumpscareAnimator.Play("WitheredFoxy");
			Jumpscare.Play();
			yield return new WaitForSeconds(0.24f);
			SceneManager.LoadScene("MainMenu");
		}
    }
}
