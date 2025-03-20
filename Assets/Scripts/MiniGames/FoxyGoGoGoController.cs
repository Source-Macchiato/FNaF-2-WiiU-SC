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
    public GameObject gameContainer;

    private RectTransform playerRect;
    private Animator playerAnimator;

    public Image[] Children;
    public Sprite DeadChild;

    public RectTransform mapRect;

    public GameObject fireworksContainer;
    public GameObject[] fireworks;
    public GameObject purpleGuy;
    public GameObject arrow;

    public Animator JumpscareAnimator;
    public Animator linesAnimator;

    public AudioSource Jumpscare;
    public AudioSource staticAudio;
    public AudioSource popAudio;

    public TextMeshProUGUI getReadyText;

    // Initial positions for reset
    private Vector3 initialPlayerPosition;

    // Movement lock to control when the bear can move
    private bool canMove = false;
    private bool reachedEventPosition = false;
    private bool reducedPlayerSpeed = false;

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
        arrow.SetActive(false);

        staticAudio.volume = 0.4f;

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

        arrow.SetActive(false);

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

        arrow.SetActive(true);
        
        canMove = true;
    }

    IEnumerator GoGoGoAnimation()
    {
        while (phaseId == 2 || (roomId != 1 || playerRect.localPosition.x <= -230f))
        {
            getReadyText.text = "Go! Go! Go!";

            yield return new WaitForSeconds(0.1f);

            if (phaseId != 2 && roomId == 1 && playerRect.localPosition.x > -230f)
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
        // Switch between rooms
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
        
        if (roomId == 1 && playerRect.localPosition.x > -230f && phaseId != 2)
        {
            if (!reachedEventPosition)
            {
                reachedEventPosition = true;

                getReadyText.text = "Hurray!";

                StartCoroutine(Firework());
            }
        }
        else if (roomId == 1 && playerRect.localPosition.x > -113f && phaseId == 2)
        {
            if (!reachedEventPosition)
            {
                reachedEventPosition = true;

                StartCoroutine(EndSequence());
            }
        }

        if (roomId == 1 && phaseId == 2)
        {
            if (!reducedPlayerSpeed)
            {
                playerSpeed /= playerSpeed;

                reducedPlayerSpeed = true;
            }
        }
    }

    IEnumerator Firework()
    {
        canMove = false;

        fireworksContainer.SetActive(true);

        StartCoroutine(Fireworks());

        yield return new WaitForSeconds(5f);

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

        gameContainer.SetActive(false);
        Jumpscare.Stop();
        JumpscareAnimator.gameObject.SetActive(false);
        staticAudio.volume = 1f;
        linesAnimator.Play("Red");

        yield return new WaitForSeconds(2f);

        SceneManager.LoadSceneAsync("MainMenu");
    }

    IEnumerator Fireworks()
    {
        int index = 0;

        fireworksContainer.SetActive(true);

        // Disable all fireworks
        foreach (GameObject firework in fireworks)
        {
            firework.SetActive(false);
        }

        // Display fireworks
        while (index < fireworks.Length)
        {
            fireworks[index].SetActive(true);

            popAudio.Play();

            index++;

            yield return new WaitForSeconds(0.5f);
        }
    }
}
