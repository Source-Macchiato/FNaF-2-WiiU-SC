using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DreamManager : MonoBehaviour
{
	public RectTransform officeRect;
    public TextMeshProUGUI statusText;
    public GameObject stripsContainer;
    public GameObject statusContainer;
	private float lastOfficePositionX;
    private int nightNumber;
    private int introDreamPlayed;

    [Header("Images")]
    public Image chicaImage;
    public Image bonnieImage;
    public Image goldenFreddyImage;
    public Image puppetImage;

    [Header("Sprites")]
    public Sprite[] chicaSprites;
    public Sprite[] bonnieSprites;

    [Header("Audios")]
    public AudioSource scarySpaceAudio;
    public AudioSource childrenLaughtingAudio;
	public AudioSource machineTurnAudio;
    public AudioSource robotAudio;
    public AudioSource staticEndAudio;

    // Scripts
    MoveInOffice moveInOffice;

    void Start()
	{
        // Get scripts
        moveInOffice = FindObjectOfType<MoveInOffice>();

        // Load
        nightNumber = SaveManager.LoadNightNumber();
        introDreamPlayed = SaveManager.LoadIntroDreamPlayed();

        // Assign last office horizontal position
        lastOfficePositionX = officeRect.anchoredPosition.x;

        stripsContainer.SetActive(false);
        statusContainer.SetActive(false);

        CharactersStatus();

        StartCoroutine(DreamEvents());
    }
	
	void Update()
	{
        NoiseWhenMoving();
	}

    private IEnumerator DreamEvents()
    {
        yield return new WaitForSeconds(30f);

        // Play robot audio
        if (robotAudio.isPlaying == false)
        {
            robotAudio.Play();
        }

        yield return new WaitForSeconds(2f);

        // Stop robot audio
        if (robotAudio.isPlaying == true)
        {
            robotAudio.Stop();
        }

        // Play static audio
        if (staticEndAudio.isPlaying == false)
        {
            staticEndAudio.Play();
        }

        // Display strips
        if (stripsContainer.activeSelf == false)
        {
            stripsContainer.SetActive(true);
        }

        yield return new WaitForSeconds(2f);

        // Stop static audio
        if (staticEndAudio.isPlaying == true)
        {
            staticEndAudio.Stop();
        }

        // Hide strips
        if (stripsContainer.activeSelf == true)
        {
            stripsContainer.SetActive(false);
        }

        // Stop scary space audio
        if (scarySpaceAudio.isPlaying == true)
        {
            scarySpaceAudio.Stop();
        }

        // Stop children laughting audio
        if (childrenLaughtingAudio.isPlaying == true)
        {
            childrenLaughtingAudio.Stop();
        }

        // Disable moving in office
        if (moveInOffice.canMove == true)
        {
            moveInOffice.canMove = false;
        }

        // Enable status container
        if (statusContainer.activeSelf == false)
        {
            statusContainer.SetActive(true);
        }

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene("MainMenu");
    }

	private void NoiseWhenMoving()
	{
        // Check if office moved
        if (officeRect.anchoredPosition.x != lastOfficePositionX)
        {
            // If audio is stopped play it
            if (machineTurnAudio.isPlaying == false)
            {
                machineTurnAudio.Play();
            }

            // Assign last office horizontal position
            lastOfficePositionX = officeRect.anchoredPosition.x;
        }
        else
        {
            // If audio is playing stop it
            if (machineTurnAudio.isPlaying == true)
            {
                machineTurnAudio.Stop();
            }
        }
    }

    private void CharactersStatus()
    {
        if (introDreamPlayed == 0)
        {
            chicaImage.sprite = chicaSprites[0];
            bonnieImage.sprite = bonnieSprites[0];
            goldenFreddyImage.enabled = false;
            puppetImage.enabled = false;

            statusText.text = "err";
        }
        if (nightNumber == 1) // If night 2
        {
            chicaImage.sprite = chicaSprites[1];
            bonnieImage.sprite = bonnieSprites[1];
            goldenFreddyImage.enabled = false;
            puppetImage.enabled = false;

            statusText.text = "it's me";
        }
        else if (nightNumber == 2) // If night 3
        {
            chicaImage.sprite = chicaSprites[2];
            bonnieImage.sprite = bonnieSprites[3];
            goldenFreddyImage.enabled = true;
            puppetImage.enabled = false;

            statusText.text = "it's me";
        }
        else if (nightNumber == 3) // If night 4
        {
            chicaImage.sprite = chicaSprites[3];
            bonnieImage.sprite = bonnieSprites[3];
            goldenFreddyImage.enabled = false;
            puppetImage.enabled = true;

            statusText.text = "it's me";
        }
    }
}