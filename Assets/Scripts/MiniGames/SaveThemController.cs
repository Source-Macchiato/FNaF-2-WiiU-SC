using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveThemController : MonoBehaviour
{
    // Speed at which the bear moves
    private float playerSpeed = 1.2f;

    public GameObject player;

    // Reference to the bear's animator
    public Animator BearAnimator;

    public RectTransform mapRect;
    public RectTransform playerRect;

    // Array of audio sources for letter sounds
    public AudioSource[] letterAudios;
    private int currentAudioIndex = 0;

    // Reference to PuppetAnimator
    public Animator puppetAnimator;

    public Vector2Int currentRoom;

    public List<RoomStep> animationSequence = new List<RoomStep>()
    {
        new RoomStep(-1, -1, "Up"),
        new RoomStep(-1,  0, "Up"),
        new RoomStep(-1,  1, "Right"),
        new RoomStep( 0,  1, "Right"),
        new RoomStep( 1,  1, "Down"),
        new RoomStep( 1,  0, "Right")
    };

    // Scripts
    PlayerMovement playerMovement;

    void Start()
    {
        // Get scripts
        playerMovement = FindObjectOfType<PlayerMovement>();

        SavePlayedMinigameAndUnlockAchievement();

        StartCoroutine(PlayAudioSequence());
        StartCoroutine(RandomEnd());

        PlayerSpawnPosition();
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerMovement();
        HandleRoomTransition();
    }

    // Handles the bear's movement
    void HandlePlayerMovement()
    {
        if (playerMovement.isMoving)
        {
            Vector3 newPosition = player.transform.position;

            // Normalize direction to prevent faster diagonal movement
            Vector2 direction = playerMovement.playerDirection.normalized;
            newPosition.x += direction.x * playerSpeed * Time.deltaTime;
            newPosition.y += direction.y * playerSpeed * Time.deltaTime;

            player.transform.position = newPosition;

            if (direction.x < -0.1f)
            {
                BearAnimator.Play("BearLeft");
            }
            else if (direction.x > 0.1f)
            {
                BearAnimator.Play("BearRight");
            }
            else if (direction.y < -0.1f)
            {
                BearAnimator.Play("BearDown");
            }
            else if (direction.y > 0.1f)
            {
                BearAnimator.Play("BearUp");
            }
        }
    }

    void HandleRoomTransition()
    {
        if (playerRect.localPosition.y >= 270f) // Up
        {
            playerRect.localPosition = new Vector3(0f, -190f, 0f);

            mapRect.localPosition -= new Vector3(0f, 720f, 0f);

            currentRoom.y += 1;

            PuppetAnimationSequence();
        }
        else if (playerRect.localPosition.x <= -420f) // Left
        {
            playerRect.localPosition = new Vector3(320f, 0f, 0f);

            mapRect.localPosition += new Vector3(960f, 0f, 0f);

            currentRoom.x -= 1;

            PuppetAnimationSequence();
        }
        else if (playerRect.localPosition.y <= -270f) // Down
        {
            playerRect.localPosition = new Vector3(0f, 190f, 0f);

            mapRect.localPosition += new Vector3(0f, 720f, 0f);

            currentRoom.y -= 1;

            PuppetAnimationSequence();
        }
        else if (playerRect.localPosition.x >= 420f) // Right
        {
            playerRect.localPosition = new Vector3(-320f, 0f, 0f);

            mapRect.localPosition -= new Vector3(960f, 0f, 0f);

            currentRoom.x += 1;

            PuppetAnimationSequence();
        }
    }

    void PuppetAnimationSequence()
    {
        bool found = false;

        for (int i = 0; i < animationSequence.Count; i++)
        {
            RoomStep step = animationSequence[i];

            if (step.roomPosition.Equals(currentRoom))
            {
                found = true;

                for (int j = 0; j <= i; j++)
                {
                    if (!animationSequence[j].played)
                    {
                        puppetAnimator.Play(animationSequence[j].animationName, 0, 0f);
                        animationSequence[j].played = true;
                    }
                    else
                    {
                        puppetAnimator.Play("Idle");
                    }
                }

                break;
            }
        }
        
        if (!found)
        {
            puppetAnimator.Play("Idle");
        }
    }

    private void PlayerSpawnPosition()
    {
        int positionIndex = Random.Range(0, 4);

        if (positionIndex == 0)
        {
            playerRect.localPosition = new Vector3(-6.3f, 17.8f, 0);

            mapRect.localPosition = new Vector3(960f, 1440f, 0);

            currentRoom = new Vector2Int(-1, -2);
        }
        else if (positionIndex == 1)
        {
            playerRect.localPosition = new Vector3(-114.2f, -6.7f, 0);

            mapRect.localPosition = new Vector3(0, 720f, 0);

            currentRoom = new Vector2Int(0, -1);
        }
        else if (positionIndex == 2)
        {
            playerRect.localPosition = new Vector3(120.5f, -11.2f, 0);

            mapRect.localPosition = new Vector3(1920f, 0, 0);

            currentRoom = new Vector2Int(-2, 0);
        }
        else if (positionIndex == 3)
        {
            playerRect.localPosition = new Vector3(-7.5f, 18.2f, 0);

            mapRect.localPosition = new Vector3(1920f, -720f, 0);

            currentRoom = new Vector2Int(-2, 1);
        }
    }

    IEnumerator PlayAudioSequence()
    {
        while (true)
        {
            // Wait for the specified time before playing audio
            yield return new WaitForSeconds(3f);

            // Play the current audio source
            letterAudios[currentAudioIndex].Play();

            // Move to the next audio source
            currentAudioIndex = (currentAudioIndex + 1) % letterAudios.Length;
        }
    }

    IEnumerator RandomEnd()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);

            if (Random.Range(0, 3) == 0)
            {
                MiniGamesLevelLoader.LoadScene("MainMenu");
            }
        }
    }

    private void SavePlayedMinigameAndUnlockAchievement()
    {
        if (!SaveManager.saveData.game.playedMinigames[0])
        {
            SaveManager.saveData.game.ChangePlayedMinigameStatus(0, true);
        }

        if (SaveManager.saveData.game.playedMinigames[0] &&
            SaveManager.saveData.game.playedMinigames[1] &&
            SaveManager.saveData.game.playedMinigames[2] &&
            SaveManager.saveData.game.playedMinigames[3])
        {
            if (MedalsManager.medalsManager != null)
            {
                MedalsManager.medalsManager.UnlockAchievement(Achievements.achievements.YOUTRIED);
            }
        }

        SaveManager.Save();
    }
}

public class RoomStep
{
    public Vector2Int roomPosition;
    public string animationName;
    public bool played = false;

    public RoomStep(int x, int y, string anim)
    {
        roomPosition = new Vector2Int(x, y);
        animationName = anim;
    }
}