using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Minigame3Controller : MonoBehaviour
{
    // Speed at which the bear moves
    public float BearSpeed = 5f;

    // Reference to the bear GameObject
    public GameObject Bear;

    // Reference to the bear's animator
    public Animator BearAnimator;

    public Image[] Children;
    public Sprite DeadChild;

    // Array of GameObjects that the bear cannot pass through
    public GameObject[] CollidableObjects;

    // Reference to the movable object
    public GameObject MoveableObject;
    public GameObject Fireworks;
    public GameObject PurpleGuy;
	public Animator JumpscareAnimator;
	public AudioSource Jumpscare;

    // Amount to move the MoveableObject on the X axis
    public float MoveBy = 5f;

    // X position at which the MoveableObject starts moving
    public float TriggerXPosition = 5f;

    // Image for displaying state
    public Image StateImage;

    // Array of sprites for different states
    public Sprite[] StateSprites;

    // Variable to track the last horizontal movement direction
    private bool movingRight = true;

    // Initial positions for reset
    private Vector3 initialBearPosition;
    private Vector3 initialMoveableObjectPosition;

    // Movement lock to control when the bear can move
    private bool canMove = false;

    // Flag to check if the trigger has been activated for the current reset
    private bool triggerActivated = false;

    private int resetCount;

    // Start is called before the first frame update
    void Start()
    {
        // Store initial positions for reset
        initialBearPosition = Bear.transform.localPosition; // Use localPosition because Bear is a child of MoveableObject
        initialMoveableObjectPosition = MoveableObject.transform.position;

        // Start the game with the initial state
        StartCoroutine(InitialState());
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            HandleBearMovement();
            CheckBearPosition();
        }
    }

    // Coroutine to handle the initial state and resets
    IEnumerator InitialState()
    {
        // Set the state to "Wait" and lock bear movement
        StateImage.sprite = StateSprites[0];
        canMove = false;

        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Set the state to "Go" and unlock bear movement after 1 second
        StateImage.sprite = StateSprites[1];
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
        MoveableObject.transform.position = initialMoveableObjectPosition;
        Bear.transform.localPosition = initialBearPosition;

        // Reset the trigger flag
        triggerActivated = false;

        // Start the initial state coroutine
        StartCoroutine(InitialState());
    }

    // Handles the bear's movement
    void HandleBearMovement()
    {
        Vector3 newPosition = Bear.transform.position;
        bool moved = false;

        // Check for arrow key inputs and move the bear accordingly
        if (Input.GetKey(KeyCode.RightArrow))
        {
            newPosition.x += BearSpeed * Time.deltaTime;
            movingRight = true;
            moved = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition.x -= BearSpeed * Time.deltaTime;
            movingRight = false;
            moved = true;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            newPosition.y += BearSpeed * Time.deltaTime;
            moved = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            newPosition.y -= BearSpeed * Time.deltaTime;
            moved = true;
        }

        // Check for collisions with collidable objects within a 50-pixel range
        if (!IsCollidingWithObjects(newPosition))
        {
            // Update the bear's position if no collision detected
            Bear.transform.position = newPosition;
        }

        // Play the appropriate animation based on the last horizontal movement direction
        if (moved)
        {
            if (movingRight)
            {
                BearAnimator.Play("BearRight");
            }
            else
            {
                BearAnimator.Play("BearLeft");
            }
        }
    }

    // Checks if the bear's new position would collide with any collidable objects within a 50-pixel range
    bool IsCollidingWithObjects(Vector3 newPosition)
    {
        float pixelToUnit = 1f / 100f; // Assuming 100 pixels per unit
        Vector3 collisionAreaMin = newPosition - new Vector3(50f * pixelToUnit, 50f * pixelToUnit, 0f);
        Vector3 collisionAreaMax = newPosition + new Vector3(50f * pixelToUnit, 50f * pixelToUnit, 0f);

        foreach (GameObject obj in CollidableObjects)
        {
            Bounds objBounds = obj.GetComponent<Collider2D>().bounds;
            if (objBounds.min.x < collisionAreaMax.x && objBounds.max.x > collisionAreaMin.x &&
                objBounds.min.y < collisionAreaMax.y && objBounds.max.y > collisionAreaMin.y)
            {
                return true; // Collision detected
            }
        }
        return false; // No collision
    }

    // Checks the bear's position to trigger actions based on its X position
    void CheckBearPosition()
    {
        // Only activate the trigger if it hasn't been activated for this reset
        if (!triggerActivated && Bear.transform.position.x > TriggerXPosition)
        {
            // Move the movable object by the specified amount on the X axis
            MoveableObject.transform.position += new Vector3(MoveBy, 0f, 0f);

            // Set StateImage sprite to "Hurray"
            StateImage.sprite = StateSprites[2];

            // Mark the trigger as activated to prevent reactivation
            triggerActivated = true;
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
