using System.Collections;
using System.Collections.Generic;
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

    // Movement bounds for the bear
    public float MaxX = 10f;
    public float MinX = -10f;
    public float MaxY = 5f;
    public float MinY = -5f;

    // Speed at which the bear moves
    public float BearSpeed = 5f;

    // Variable to track the last horizontal movement direction
    private bool movingRight = true;

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

    // Start is called before the first frame update
    void Start()
    {
        // Initialize hunger values for each kid
        for (int i = 0; i < Hunger.Length; i++)
        {
            Hunger[i] = MaxHunger;
        }

        // Start the audio sequence
        StartCoroutine(PlayAudioSequence());
    }

    IEnumerator Main()
    {
        yield return new WaitForSeconds(27f);
        JumpscareAnimator.gameObject.SetActive(true);
        JumpscareAnimator.Play("Puppet");
		Jumpscare.Play();
        yield return new WaitForSeconds(0.24f);
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

    // Handles the bear's movement within the defined bounds
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

        // Clamp the bear's position to stay within the defined bounds
        newPosition.x = Mathf.Clamp(newPosition.x, MinX, MaxX);
        newPosition.y = Mathf.Clamp(newPosition.y, MinY, MaxY);

        // Update the bear's position
        Bear.transform.position = newPosition;

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

    // Updates the bear's speed over time
    void UpdateBearSpeed()
    {
        if (elapsedTime >= 14f && elapsedTime < 17f)
        {
            // Gradually reduce the bear's speed from 14 seconds to 17 seconds
            BearSpeed = Mathf.Lerp(5f, 0f, (elapsedTime - 14f) / 3f);
        }
        else if (elapsedTime >= 17f)
        {
            // Set the bear's speed to 0 at 17 seconds
            BearSpeed = 0f;
        }
    }
}
