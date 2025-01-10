using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Minigame2Controller : MonoBehaviour
{
    // Speed at which the bear moves
    public float BearSpeed = 5f;

    // Reference to the bear GameObject
    public GameObject Bear;

    // Reference to the bear's animator
    public Animator BearAnimator;

    // Reference to the parent object containing all collidable children
    public GameObject CollidableParent;

    // Reference to the movable object
    public GameObject MoveableObject;

    // Array to define the sequence of directions for room transitions
    public string[] DirectionSequence;

    // Variable to track the last movement direction
    private string lastDirection = "Right";

    // Index to track the current direction in the sequence
    private int currentDirectionIndex = 0;

    // Array of audio sources for letter sounds
    public AudioSource[] Letters;

    // Reference to PuppetAnimator
    public Animator PuppetAnimator;

    // Reference to PurpleGuy GameObject
    public GameObject PurpleGuy;

    // Speed at which PurpleGuy moves
    public float PurpleGuySpeedMultiplier = 3f;

    // Distance within which the scene will be loaded
    public float PurpleGuyChaseRange = 100f;

    // Target player position (assuming it's the Bear)
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize playerTransform to the Bear's transform
        playerTransform = Bear.transform;

        // Start the audio loop coroutine
        StartCoroutine(PlayLetterSounds());
    }

    // Update is called once per frame
    void Update()
    {
        HandleBearMovement();
        HandleRoomTransition();
        // Only call this if PurpleGuy is active and currentDirectionIndex is 5
        if (currentDirectionIndex == 5)
        {
            StartCoroutine(PurpleGuyChase());
        }
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
            lastDirection = "Right";
            moved = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition.x -= BearSpeed * Time.deltaTime;
            lastDirection = "Left";
            moved = true;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            newPosition.y += BearSpeed * Time.deltaTime;
            lastDirection = "Up";
            moved = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            newPosition.y -= BearSpeed * Time.deltaTime;
            lastDirection = "Down";
            moved = true;
        }

        // Check for collisions with children of the collidable parent object
        if (!IsCollidingWithChildren(newPosition))
        {
            // Update the bear's position if no collision detected
            Bear.transform.position = newPosition;
        }

        // Play the appropriate animation based on the last movement direction
        if (moved)
        {
            PlayBearAnimation();
        }
    }

    // Plays the appropriate animation based on the last movement direction
    void PlayBearAnimation()
    {
        switch (lastDirection)
        {
            case "Right":
                BearAnimator.Play("BearRight");
                break;
            case "Left":
                BearAnimator.Play("BearLeft");
                break;
            case "Up":
                BearAnimator.Play("BearUp");
                break;
            case "Down":
                BearAnimator.Play("BearDown");
                break;
        }
    }

    // Checks if the bear's new position would collide with any children of the collidable parent object within a 50-pixel range
    bool IsCollidingWithChildren(Vector3 newPosition)
    {
        float pixelToUnit = 1f / 100f; // Assuming 100 pixels per unit
        Vector3 collisionAreaMin = newPosition - new Vector3(50f * pixelToUnit, 50f * pixelToUnit, 0f);
        Vector3 collisionAreaMax = newPosition + new Vector3(50f * pixelToUnit, 50f * pixelToUnit, 0f);

        foreach (Transform child in CollidableParent.transform)
        {
            Collider2D childCollider = child.GetComponent<Collider2D>();
            if (childCollider != null)
            {
                Bounds childBounds = childCollider.bounds;
                if (childBounds.min.x < collisionAreaMax.x && childBounds.max.x > collisionAreaMin.x &&
                    childBounds.min.y < collisionAreaMax.y && childBounds.max.y > collisionAreaMin.y)
                {
                    return true; // Collision detected
                }
            }
        }
        return false; // No collision
    }

    // Handles room transitions based on the bear's position and the current direction in the sequence
    void HandleRoomTransition()
    {
        if (currentDirectionIndex < DirectionSequence.Length)
        {
            string direction = DirectionSequence[currentDirectionIndex];

            if (direction == "Left" && lastDirection == "Left" && Bear.transform.position.x <= 0f)
            {
                MoveableObject.transform.position += new Vector3(400f, 0f, 0f);
                currentDirectionIndex++;
                TransitionEvent();
            }
            else if (direction == "Right" && lastDirection == "Right" && Bear.transform.position.x >= 400f)
            {
                MoveableObject.transform.position -= new Vector3(400f, 0f, 0f);
                currentDirectionIndex++;
                TransitionEvent();
            }
            else if (direction == "Up" && lastDirection == "Up" && Bear.transform.position.y >= 240f)
            {
                MoveableObject.transform.position -= new Vector3(0f, 240f, 0f);
                currentDirectionIndex++;
                TransitionEvent();
            }
            else if (direction == "Down" && lastDirection == "Down" && Bear.transform.position.y <= 0f)
            {
                MoveableObject.transform.position += new Vector3(0f, 240f, 0f);
                currentDirectionIndex++;
                TransitionEvent();
            }
        }
    }

    // Handles the transition event based on the current direction index
    void TransitionEvent()
    {
        switch (currentDirectionIndex)
        {
            case 1:
            case 2:
                PuppetAnimator.Play("PuppetUp 0");
                PuppetAnimator.Play("PuppetUp");
                break;
            case 3:
            case 4:
                PuppetAnimator.Play("PuppetRight 0");
                PuppetAnimator.Play("PuppetRight");
                break;
            case 5:
                // When Index is 5, start the PurpleGuy chase coroutine
                StartCoroutine(PurpleGuyChase());
                break;
        }
    }

    // Coroutine to play letter sounds in sequence at 2.3-second intervals
    IEnumerator PlayLetterSounds()
    {
        while (true)
        {
            for (int i = 0; i < Letters.Length; i++)
            {
                Letters[i].Play();
                yield return new WaitForSeconds(2.3f);
            }
        }
    }

    // Coroutine for PurpleGuy chasing the player
    IEnumerator PurpleGuyChase()
    {
        while (currentDirectionIndex == 5)
        {
            // Move PurpleGuy towards the player
            Vector3 direction = (playerTransform.position - PurpleGuy.transform.position).normalized;
            PurpleGuy.transform.position += direction * BearSpeed * PurpleGuySpeedMultiplier * Time.deltaTime;

            // Check if PurpleGuy is within range to load a new scene
            if (Vector3.Distance(PurpleGuy.transform.position, playerTransform.position) <= PurpleGuyChaseRange)
            {
                // Load the new scene
                SceneManager.LoadScene("DeathMinigame2Part2"); // Replace with the name of your scene
            }

            yield return null; // Continue checking each frame
        }
    }
}
