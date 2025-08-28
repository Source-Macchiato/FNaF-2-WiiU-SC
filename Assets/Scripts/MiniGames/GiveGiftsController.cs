using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GiveGiftsController : MonoBehaviour
{
    public float playerSpeed = 1f;
    
    public GameObject player;
    public GameObject gameContainer;
    public GameObject kidFive;
    
    public Sprite[] puppetSprites;
    
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI giveGiftsText;
    
    public Animator JumpscareAnimator;
    public Animator linesAnimator;

    public AudioSource Jumpscare;
    public AudioSource cakeAudio;
    public AudioSource effectAudio;
    public AudioSource[] saveThemAudios;
    
    public KidGift[] kidGifts;
    
    private bool secondPhaseStarted = false;

    public int score = 0;
    private int currentSaveThemAudioIndex = 0;

    // Scripts
    PlayerMovement playerMovement;
    ControllersRumble controllersRumble;

    void Start()
    {
        // Get scripts
        playerMovement = FindObjectOfType<PlayerMovement>();
        controllersRumble = FindObjectOfType<ControllersRumble>();

        kidFive.SetActive(false);

        UpdateScoreText();

        SavePlayedMinigameAndUnlockAchievement();

        StartCoroutine(SaveThemAudioSequence());
    }

    void Update()
    {
        HandlePlayerMovement();
    }

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

            // Use right sprite based on the last horizontal movement direction
            if (playerSpeed > 0f)
            {
                if (direction.x < -0.1f)
                {
                    player.GetComponent<Image>().sprite = puppetSprites[1];
                }
                else if (direction.x > 0.1f)
                {
                    player.GetComponent<Image>().sprite = puppetSprites[0];
                }
            }
        }
    }

    public void UpdateScoreText()
    {
        scoreText.text = score.ToString("D4");
    }

    public void PlayEndSequence()
    {
        StartCoroutine(EndSequence());
    }

    private IEnumerator SaveThemAudioSequence()
    {
        while (score < 800)
        {
            // Wait for the specified time before playing audio
            yield return new WaitForSeconds(1.6f);

            // Just in case the score changed after the delay
            if (score >= 800)
            {
                yield break;
            }

            // Play the current audio source
            saveThemAudios[currentSaveThemAudioIndex].Play();

            // Move to the next audio source
            currentSaveThemAudioIndex = (currentSaveThemAudioIndex + 1) % saveThemAudios.Length;
        }
    }

    public IEnumerator SecondPhase()
    {
        if (secondPhaseStarted)
        {
            yield break;
        }

        yield return new WaitForSeconds(1f);

        foreach (KidGift kidGift in kidGifts)
        {
            if (kidGift.isTriggered)
            {
                yield break;
            }
        }

        foreach (KidGift kidGift in kidGifts)
        {
            kidGift.gift.SetActive(false);
            kidGift.secondPhase = true;
        }

        giveGiftsText.text = "Give Life.";

        secondPhaseStarted = true;
    }

    IEnumerator EndSequence()
    {
        kidFive.SetActive(true);

        yield return new WaitForSeconds(1f / 20);

        kidFive.SetActive(false);

        score = Random.Range(0, 9999);
        UpdateScoreText();

        Jumpscare.Play();
        JumpscareAnimator.Play("GoldenFreddy");
        controllersRumble.TriggerRumble(9);

        yield return new WaitForSeconds(0.6f);

        gameContainer.SetActive(false);
        Jumpscare.Stop();
        JumpscareAnimator.gameObject.SetActive(false);

        MiniGamesLevelLoader.LoadScene("MainMenu");
    }

    private void SavePlayedMinigameAndUnlockAchievement()
    {
        if (!SaveManager.saveData.game.playedMinigames[1])
        {
            SaveManager.saveData.game.ChangePlayedMinigameStatus(1, true);
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