﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TakeCakeController : MonoBehaviour
{
    // Animator array to control each kid's animations
    public Animator[] Kids;

    // Hunger array to store the hunger level of each kid
    public float[] Hunger;

    // Reference to the bear GameObject
    public GameObject Bear;
    public GameObject gameContainer;

    // Bear's Animator
    public Animator BearAnimator;

    // Speed at which the bear moves
    private float BearSpeed = 1f;

    // Time elapsed since the start of the game
    public float elapsedTime = 0f;

    // Jumpscare animator
    public Animator JumpscareAnimator;
    public Animator linesAnimator;

    // Audio sources for the sequence S-A-V-E-H-I-M
    public AudioSource[] AudioSources;
    public AudioSource Jumpscare;
    public AudioSource cakeAudio;

    // Index to track the current audio source
    private int currentAudioIndex = 0;

    // Wall GameObjects for boundaries
    public GameObject[] Walls;

    private Rigidbody2D bearRigidbody;

    // Scripts
    PlayerMovement bearMovement;

    // Start is called before the first frame update
    void Start()
    {
        // Get scripts
        bearMovement = FindObjectOfType<PlayerMovement>();

        // Initialize hunger values for each kid
        for (int i = 0; i < Hunger.Length; i++)
        {
            Hunger[i] = Random.Range(10, 20);
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

        yield return new WaitForSeconds(0.6f);

        gameContainer.SetActive(false);
        JumpscareAnimator.gameObject.SetActive(false);
        Jumpscare.Stop();

        MiniGamesLevelLoader.LoadScene("MainMenu");
    }

    // Coroutine to play audio sources in sequence
    IEnumerator PlayAudioSequence()
    {
        while (elapsedTime < 50f)
        {
            // Wait for the specified time before playing audio
            yield return new WaitForSeconds(1.6f);

            // Just in case the elapsed time has been reached after the delay
            if (elapsedTime >= 50f)
            {
                yield break;
            }

            // Play the current audio source
            AudioSources[currentAudioIndex].Play();

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
            Hunger[i] = Mathf.Max(0, Hunger[i] - Time.deltaTime);

            // Determine animation based on hunger level
            if (Hunger[i] > 10f)
            {
                Kids[i].Play("KidHappy");
            }
            else if (Hunger[i] > 0.1f)
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
            if (direction.x < -0.1f)
            {
                BearAnimator.Play("BearLeft");
            }
            else if (direction.x > 0.1f)
            {
                BearAnimator.Play("BearRight");
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