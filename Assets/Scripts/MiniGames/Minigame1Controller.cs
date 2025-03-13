using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Minigame1Controller : MonoBehaviour
{
    // Animator array to control each kid's animations
    public Animator[] Kids;

    // Hunger array to store the hunger level of each kid
    public float[] Hunger;

    // Min and Max values for hunger
    public float MinimumHunger = 0f;
    public float MaxHunger = 100f;

    // Rate at which hunger decreases per second
    public float HungerRegressionRate = 1f;

    // Reference to the bear GameObject
    public GameObject Bear;

    // Bear's Animator
    public Animator BearAnimator;

    // Speed at which the bear moves
    public float BearSpeed = 1f;

    // Distance within which the bear maximizes the kids' hunger
    public float BearProximityThreshold = 2f;

    // Time elapsed since the start of the game
    private float elapsedTime = 0f;

    // Jumpscare animator
    public Animator JumpscareAnimator;

    // Audio sources for the sequence S-A-V-E-H-I-M
    public AudioSource[] AudioSources;
    public AudioSource Jumpscare;

    // Index to track the current audio source
    private int currentAudioIndex = 0;

    // Time between each audio source playback
    public float timeBetweenAudio = 1f;

    // Wall GameObjects for boundaries
    public GameObject[] Walls;

    private Rigidbody2D bearRigidbody;

    // Scripts
    BearMovement bearMovement;

    // Start is called before the first frame update
    void Start()
    {
        // Get scripts
        bearMovement = FindObjectOfType<BearMovement>();

        // Initialize hunger values for each kid
        for (int i = 0; i < Hunger.Length; i++)
        {
            Hunger[i] = MaxHunger;
        }

        // Get the Rigidbody2D component for the bear
        bearRigidbody = Bear.GetComponent<Rigidbody2D>();

        // Start the audio sequence
        StartCoroutine(PlayAudioSequence());
    }

    IEnumerator Main()
    {
        yield return new WaitForSeconds(50f);
        JumpscareAnimator.gameObject.SetActive(true);
        JumpscareAnimator.Play("Puppet");
        Jumpscare.Play();
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }

    // Coroutine to play audio sources in sequence
    IEnumerator PlayAudioSequence()
    {
        while (true)
        {
            // Play the current audio source
            AudioSources[currentAudioIndex].Play();

            // Wait for the specified time before playing the next one
            yield return new WaitForSeconds(timeBetweenAudio);

            // Move to the next audio source
            currentAudioIndex = (currentAudioIndex + 1) % AudioSources.Length;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update time elapsed
        elapsedTime += Time.deltaTime;

        // Update bear speed based on time elapsed
        UpdateBearSpeed();

        // Update each kid's hunger and animation
        for (int i = 0; i < Kids.Length; i++)
        {
            // Regress hunger over time
            Hunger[i] = Mathf.Max(MinimumHunger, Hunger[i] - HungerRegressionRate * Time.deltaTime);

            // Check if the bear is close to this kid
            if (Vector3.Distance(Bear.transform.position, Kids[i].transform.position) <= BearProximityThreshold)
            {
                Hunger[i] = MaxHunger; // Maximize hunger if the bear is close
            }

            // Determine animation based on hunger level
            if (Hunger[i] > 75f)
            {
                Kids[i].Play("KidHappy");
            }
            else if (Hunger[i] > 25f)
            {
                Kids[i].Play("KidMedium");
            }
            else
            {
                Kids[i].Play("KidAngry");
            }
        }

        // Handle bear movement with arrow keys
        HandleBearMovement();
    }

    // Handles the bear's movement within the walls
    void HandleBearMovement()
    {
        // Update the bear's position
        if (bearMovement.isMoving)
        {
            Vector3 newPosition = Bear.transform.position;

            // Normalize direction to prevent faster diagonal movement
            Vector2 direction = bearMovement.playerDirection.normalized;
            newPosition.x += direction.x * BearSpeed * Time.deltaTime;
            newPosition.y += direction.y * BearSpeed * Time.deltaTime;

            Bear.transform.position = newPosition;

            // Play the appropriate animation based on the last horizontal movement direction
            if (BearSpeed > 0f)
            {
                if (bearMovement.playerDirection == Vector2.left)
                {
                    BearAnimator.Play("BearLeft");
                }
                else if (bearMovement.playerDirection == Vector2.right)
                {
                    BearAnimator.Play("BearRight");
                }
            }
        }
    }

    // Prevent the bear from passing through walls
    void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (GameObject wall in Walls)
        {
            if (collision.gameObject == wall)
            {
                bearRigidbody.velocity = Vector2.zero;
                break;
            }
        }
    }

    // Updates the bear's speed over time
    void UpdateBearSpeed()
    {
        if (elapsedTime >= 20f)
        {
            float t = (elapsedTime - 20f) / (45f - 20f);
            BearSpeed = Mathf.Lerp(1f, 0f, t);
        }
    }
}