/*
If u decompiled this game, you're either MysticTortoise or a random guy. Take a look at this chaotic code lmao
*/

using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using WiiU = UnityEngine.WiiU;
using TMPro;

public class NightPlayer : MonoBehaviour
{
	public GameObject[] customNightRewards;

	public AudioSource DisruputedSound;
	public bool disturbLoop;
	public AIManager aiManager;
	public StateManager stateManager;
	[Header("Audio")]
	public AudioSource Jumpscare;
	public AudioSource phoneCall;
	public AudioSource NoFlashlightBatterys;
	public AudioSource jackInTheBox;
	public AudioSource WindUpSound;
	public GameObject BBSounds;
	public AudioSource ErrorSound;
	public AudioSource VentCrawl;
	public AudioSource Mangle;
	public AudioSource StareSound;
	public AudioSource With2;

	public int With2Used = 0; // 1 = Tbonnie - 2 = TBonnie - 3 = TChica - 4 = WBonnie - 5 = WFreddy - 6 = WChica - 7 = Foxy
	public int CamWatchAI = 0; // 1 = Tbonnie - 2 = TBonnie - 3 = TChica - 4 = WBonnie - 5 = WFreddy - 6 = WChica - 7 = Foxy

	[Header("Info")]
	public int[] NewandShinyAI;
	public int[] DoubleTroubleAI;
	public int[] NightofMisfitsAI;
	public int[] FoxyFoxyAI;
	public int[] LadiesNightAI;
	public int[] FreddysCircusAI;
	public int[] CupcakeChallengeAI;
	public int[] FazbearFeverAI;
	public int[] GoldenFreddyChallengeAI;
	public int[] currentAI;

	public int currentNight;
    public int currentTime;
    public int currentCam = 09;

	public TMP_Text timeText;

	public bool isJumpscared = false;

    public float TimeMultiplier;
	public bool isNight7;
	public int PuppetAI;
	public int GoldenFreddyAI;
	public int MangleAI;
	public int BalloonBoyAI;
	public int PaperpalsAI;
	public int ToyBonnieAI;
	public int ToyChicaAI;
	public int ToyFreddyAI;
	public int WitheredBonnieAI;
	public int WitheredChicaAI;
	public int WitheredFreddyAI;
	public int WitheredFoxyAI;
	//private Dictionary<int, Dictionary<string, Action>> nightEvents;
	public float PuppetTime = 30f;
	public float PuppetDeathTimer = 15f;
	public int previousFullNessCircleIndex = -1; // Initialized to -1 to indicate no previous sprite
	public bool canDeathSeqeuence = true;
	public float puppetEndoChance;
	public int MangleCamera = 12;
	public int BBCamera = 10;
	public int PaperpalsCamera = 4;
	public int ToyBonnieCamera = 9;
	public int ToyChicaCamera = 9;
	public int ToyFreddyCamera = 9;
	public int WitheredBonnieCamera = 8;
	public int WitheredChicaCamera = 8;
	public int WitheredFreddyCamera = 8;
	public int WitheredFoxyCamera = 8;
	public float GoldenFreddyMovement = 1.67f;
	public float GoldenFreddyCameraTime = 5f;
	public bool GoldenFreddyPrepared = false;
	public bool GoldenFreddyInOffice = false;
	public bool GoldenFreddyInHall = false;
	public float GoldenFreddyrandNum;
	public float MangleMovement = 5f;
	public float BBMovement = 5f;
	public float PaperpalsMovement = 5f;
	public float ToyBonnieMovement = 5f;
	public float ToyBonnieMaskTimer = 1f;
	public bool ToyBonniePrepared;
	public float ToyBonnieBlackoutTime;
	public float ToyChicaMovement = 5f;
	public bool ToyChicaPrepared;
	public float ToyChicaMaskTimer = 1f;
	public float ToyFreddyMovement = 5f;
	public float WitheredBonnieMovement = 5f;
	public float WitheredChicaMovement = 5f;
	public float WitheredFreddyMovement = 5f;
	public float WitheredFoxyMovement = 5f;
	public bool WitheredFoxyPrepared;
	public bool MangleFlashed;
	public float MangleMaskTimer = 1f;
	public bool ManglePrepared;
	public bool BBFlashed;
	public float BBMaskTimer = 1f;
	public bool PaperpalsFlashed;
	public bool ToyBonnieFlashed;
	public bool ToyChicaFlashed;
	public bool ToyFreddyFlashed;
	public bool WitheredBonnieFlashed;
	public bool WitheredChicaFlashed;
	public bool WitheredFreddyFlashed;
	public bool WitheredFreddyPrepared;
	public bool WitheredFoxyFlashed;
	public bool BlackoutActive;
	public string currentBlackout;
	public bool BlackoutPrepared;

	[Header("Cams")]
	public Animator Screen;
	public GameObject SignalDisrupted;
	public Image MainCameraBG;
	public Image RoomName;
	public Sprite[] RoomNames;
	public Sprite[] FlashlightedCams;
	public Sprite[] DefaultCams;
	public Sprite[] PuppetCameraSprites;
	public Sprite PuppetCameraEndoSprite;
	public Sprite LeftVentEndoSprite;
	public AudioSource SwitchCameraSound;
	public Animator WhiteStripes;
	public Sprite[] CircleSprites;
	public Sprite[] Cam9Sprites;
	public Sprite[] Cam8Sprites;
	public Sprite[] ToyBonnieDefaultCams;
	public Sprite[] ToyBonnieFlashlightCams;
	public Sprite[] ToyChicaDefaultCams;
	public Sprite[] ToyChicaFlashlightCams;
	public Sprite[] ToyFreddyDefaultCams;
	public Sprite[] ToyFreddyFlashlightCams;
	public Sprite[] BBDefaultCams;
	public Sprite[] BBFlashlightCams;
	public Sprite[] GoldenFreddyDefaultCams;
	public Sprite[] GoldenFreddyFlashlightCams;
	public Sprite[] WitheredFoxyDefaultCams;
	public Sprite[] WitheredFoxyFlashlightCams;
	public Sprite[] MangleDefaultCams;
	public Sprite[] MangleFlashlightCams;
	public Sprite[] WitheredFreddyDefaultCams;
	public Sprite[] WitheredFreddyFlashlightCams;
	public Sprite[] WitheredChicaDefaultCams;
	public Sprite[] WitheredChicaFlashlightCams;
	public Sprite[] WitheredBonnieDefaultCams;
	public Sprite[] WitheredBonnieFlashlightCams;
	public GameObject[] MangleInRooms;

	[Header("Flashlight")]

	
	public Sprite Battery4Bars;
	public Sprite Battery3Bars;
	public Sprite Battery2Bars;
	public Sprite Battery1Bars;
	public Sprite Battery0Bars;
	public Image BatteryImage;

	[Header("Game Animations")]
	public GameObject[] Plushies;
	public GameObject Ribbons;
	public GameObject JJ;
	public GameObject RWQOffice;
	public GameObject BaloonBoy;
	public GameObject GoldenFreddyOffice;
	public GameObject PaperpalsOffice;
	public GameObject MangleOffice;
	public GameObject WitheredFreddyOffice;
	public GameObject WitheredChicaOffice;
	public GameObject WitheredBonnieOffice;
	public Image Blackout;
	public Animator BlackOutAnim;
	public Image ToyBonnieBlackout;
	public Image ToyChicaBlackout;
	public GameObject ToyFreddyOffice;
	private float RWQCrashTimer;
	private bool RWQActive;
	public Animator JumpscareAnimator;

	[Header("Lights system")]
	// Office image
    public Image officeImage;

	// Office sprites
    public Sprite defaultOffice;
    public Sprite leftLightOffice;
    public Sprite centerLightOffice;
    public Sprite rightLightOffice;

	// Buttons images
	public Image leftButtonImage;
	public Image rightButtonImage;

	// Buttons sprites
	public Sprite leftButtonOff;
	public Sprite leftButtonOn;
	public Sprite rightButtonOff;
	public Sprite rightButtonOn;

	[Header("State")]
    public string state = "Office";

	private MusicBox musicBox;
	private MaskManager maskManager;
	private LightsManager lightsManager;
	private MonitorManager monitorManager;
	private ControllersRumble controllersRumble;

	public float AMTime = 0;
	public int currentHour = 0;
	private bool isHourComplete = false;
    private const float HourDuration = 70f;

    // References to WiiU controllers
    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    void Start()
	{
        // Access the WiiU GamePad and Remote
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

		// Get scripts
		musicBox = FindObjectOfType<MusicBox>();
		maskManager = FindObjectOfType<MaskManager>();
		lightsManager = FindObjectOfType<LightsManager>();
		monitorManager = FindObjectOfType<MonitorManager>();
		controllersRumble = FindObjectOfType<ControllersRumble>();

		// Assign current night at start
        currentNight = SaveManager.LoadNightNumber();

		if (Random.value < 0.01) {PaperpalsAI = 1;}
		PuppetAI = Mathf.Clamp(PuppetAI,0, 6);
		WitheredFoxyAI = Mathf.Clamp(WitheredFoxyAI,0, 17);

		// Display rewards on office desk
		for (int i = 0; i < customNightRewards.Length; i++)
		{
			if (customNightRewards[i] != null)
			{
                customNightRewards[i].SetActive(SaveManager.LoadDoneMode(i));
            }
		}
    }

	private bool IsAIMatching(int[] challengeAI)
	{
    if (challengeAI.Length != currentAI.Length) 
    {
        return false;
    }

    for (int i = 0; i < currentAI.Length; i++)
    {
        if (challengeAI[i] != currentAI[i])
        {
            return false;
        }
    }

    return true;
	}

    public void UpdateTime()
    {
        if (currentTime > 6)
            return;
        AMTime += Time.deltaTime;

        if (AMTime >= HourDuration * TimeMultiplier && !isHourComplete)
        {
            isHourComplete = true;
            AMTime = 0f; 
            currentTime++; 

            if (currentTime == 0)
                timeText.text = "12";
            else
                timeText.text = currentTime.ToString();

            aiManager.TimedEvents();
        }
        if (AMTime < HourDuration * TimeMultiplier)
        {
            isHourComplete = false;
        }
    }


    

	private IEnumerator JumpscareSequence()
	{
		Debug.Log("Death!! State: "+ state);

		isJumpscared = true;

		controllersRumble.TriggerRumble(0.45f);

		yield return new WaitForSeconds(0.45f);

		Jumpscare.Stop();

		SceneManager.LoadScene("GameOver");
	}
    
    void Update()
	{
		ToyFreddyAI = aiManager.GetAILevel("ToyFreddyAI");
		ToyBonnieAI = aiManager.GetAILevel("ToyBonnieAI");
		ToyChicaAI = aiManager.GetAILevel("ToyChicaAI");
		WitheredFreddyAI = aiManager.GetAILevel("WitheredFreddyAI");
		WitheredBonnieAI = aiManager.GetAILevel("WitheredBonnieAI");
		WitheredChicaAI = aiManager.GetAILevel("WitheredChicaAI");
		WitheredFoxyAI  = aiManager.GetAILevel("WitheredFoxyAI");
		GoldenFreddyAI = aiManager.GetAILevel("GoldenFreddyAI");
		MangleAI = aiManager.GetAILevel("MangleAI");
		BalloonBoyAI = aiManager.GetAILevel("BalloonBoyAI");
		PaperpalsAI = aiManager.GetAILevel("PaperpalsAI");
		UpdateTime();
		InputFunction();
		StateChecks();
		UpdateBatteryUI();
		PuppetBox();
		MovementOpportunityMain();
		MovementOpportunityHandler();
		StartCoroutine(GoldenFreddyFunction());

		if (currentCam == MangleCamera)
        {
            CamWatchAI = 9;
        }
        else if (currentCam == BBCamera)
        {
            CamWatchAI = 8;
        }
        
        else if (currentCam == ToyBonnieCamera)
        {
            CamWatchAI = 1;
        }
        
        else if (currentCam == ToyChicaCamera)
        {
            CamWatchAI = 3;
        }
        
        else if (currentCam == ToyFreddyCamera)
        {
            CamWatchAI = 2;
        }
        
        else if (currentCam == WitheredBonnieCamera)
        {
            CamWatchAI = 4;
        }
        
        else if (currentCam == WitheredChicaCamera)
        {
            CamWatchAI = 6;
        }
        
        else if (currentCam == WitheredFreddyCamera)
        {
            CamWatchAI = 5;
        }
        
        else if (currentCam == WitheredFoxyCamera)
        {
            CamWatchAI = 7;
        }
		else
		{
			CamWatchAI = 0;
		}
        


		if (Application.isEditor)
		{
			Debug.Log("CamWatchAI NightPlayer! : " + CamWatchAI);
			Debug.Log("current cam "+ currentCam);
			Debug.Log("freddy : " + ToyFreddyCamera);
		}
		

		//if player is on cam 1,2, 3, 4, 5, or 6, play Idle
		if(new int[] { 1, 2, 3, 4, 5, 6 }.Contains(currentCam))
		{
			Screen.Play("Idle");
		}
		else {
			Screen.Play("Background");
		}
		

		// Hallway sound manager

		//Bonnie
		if (ToyBonnieCamera == 1 && With2.isPlaying && With2Used == 1)
		{
			With2.Stop();
		}

		if(ToyFreddyCamera == 14 && With2Used == 0)
		{
			With2Used = 2;
			With2.Play();
		}
		else if(ToyFreddyCamera != 14 && With2.isPlaying && With2Used == 2)
		{
			With2Used = 0;
			With2.Stop();
		}

		//Foxy
		if(WitheredFoxyCamera == 14 && With2Used == 0)
		{
			With2Used = 7;
			With2.Play();
		}
		else if(WitheredFoxyCamera != 14 && With2.isPlaying && With2Used == 7)
		{
			With2Used = 0;
			With2.Stop();
		}

		//WBonnie
		if(WitheredBonnieCamera == 14 && With2Used == 0)
		{
			With2Used = 4;
			With2.Play();
		}
		else if(WitheredBonnieCamera != 14 && With2.isPlaying && With2Used == 4)
		{
			With2Used = 0;
			With2.Stop();
		}

		//Freddy
		if(WitheredFreddyCamera == 14 && With2Used == 0)
		{
			With2Used = 5;
			With2.Play();
		}
		else if(WitheredFreddyCamera != 14 && With2.isPlaying && With2Used == 5)
		{
			With2Used = 0;
			With2.Stop();
		}

		//Chica
		if(WitheredChicaCamera == 14 && With2Used == 0)
		{
			With2Used = 6;
			With2.Play();
		}
		else if(WitheredChicaCamera != 14 && With2.isPlaying && With2Used == 6)
		{
			With2Used = 0;
			With2.Stop();
		}
    }

	IEnumerator GoldenFreddyFunction()
	{
		if (GoldenFreddyInOffice)
		{
			GoldenFreddyMovement -= Time.deltaTime;
			if (GoldenFreddyMovement <= 0f)
			{
				if (monitorManager.isMonitorActive)
			{
				//StartCoroutine(MonitorDownIE());
				yield return new WaitForSeconds(0.183f);
				JumpscareAnimator.Play("GoldenFreddy");
				Jumpscare.Play();
				StartCoroutine(JumpscareSequence());
			}
			else if (!monitorManager.isMonitorActive && !maskManager.isActiveAndEnabled || state == "OfficeBlackout")
			{
				JumpscareAnimator.Play("GoldenFreddy");
				Jumpscare.Play();
				StartCoroutine(JumpscareSequence());
			}
			}
		}
	}

	void MovementOpportunityHandler() // 1234
	{
		if (GoldenFreddyCameraTime <= 0f)
		{
			GoldenFreddyPrepared = true;
		}
		if (PaperpalsMovement <= 0f && currentCam != 4)
		{
			int randNum = Random.Range(0, 20);
			if (PaperpalsAI >= randNum)
			{
				PaperpalsOffice.SetActive(true);
				PaperpalsMovement = 5f;
				PaperpalsAI = 0;
			}
		}
		if (ToyBonnieMovement <= 0f)
		{
			int randNum = Random.Range(0, 20);
			if (ToyBonnieAI >= randNum || ToyBonnieAI == randNum)
			{
				if(CamWatchAI != 1)
				{
					switch (ToyBonnieCamera)
				{
				case 9:
				ToyBonnieCamera = 3;
				StartCoroutine(DisruptCamera(9));
				StartCoroutine(DisruptCamera(3));
				break;
				case 3:
				ToyBonnieCamera = 4;
				StartCoroutine(DisruptCamera(3));
				StartCoroutine(DisruptCamera(4));
				break;
				case 4:
				ToyBonnieCamera = 2;
				With2.Play();
				StartCoroutine(DisruptCamera(4));
				StartCoroutine(DisruptCamera(2));
				break;
				case 2:
				ToyBonnieCamera = 6;
				VentCrawl.Play();
				StartCoroutine(DisruptCamera(2));
				StartCoroutine(DisruptCamera(6));
				break;
				case 6:
				ToyBonnieCamera = 13;
				VentCrawl.Play();
				StartCoroutine(DisruptCamera(6));
				break;
				case 13:
				StartCoroutine(ToyBonnieFunction(true));
				ToyBonniePrepared = true;
				break;
				}
				}
			}
			else
			{
				if (ToyBonnieCamera == 13)
				{
					StartCoroutine(ToyBonnieFunction(false));
				}
			}
			ToyBonnieMovement = 5f;
		}
		if (ToyChicaMovement <= 0f)
		{
			int randNum = Random.Range(0, 20);
			if (ToyChicaAI >= randNum || ToyChicaAI == randNum)
			{
				if(CamWatchAI != 3)
				{
				switch (ToyChicaCamera)
				{
				case 9:
				ToyChicaCamera = 7;
				StartCoroutine(DisruptCamera(Random.Range(7, 10)));
				StartCoroutine(DisruptCamera(Random.Range(7, 10)));
				break;
				case 7:
				ToyChicaCamera = 4;
				StartCoroutine(DisruptCamera(Random.Range(4, 7)));
				break;
				case 4:
				ToyChicaCamera = 14;
				StartCoroutine(DisruptCamera(Random.Range(4, 7)));
				break;
				case 14:
				ToyChicaCamera = 1;
				StartCoroutine(DisruptCamera(Random.Range(1, 4)));
				break;
				case 1:
				ToyChicaCamera = 5;
				StartCoroutine(DisruptCamera(Random.Range(1, 4)));
				StartCoroutine(DisruptCamera(Random.Range(5, 7)));
				break;
				case 5:
				ToyChicaCamera = 13;
				StartCoroutine(DisruptCamera(Random.Range(5, 7)));
				break;
				case 13:
				StartCoroutine(ToyChicaFunction(true));
				ToyChicaPrepared = true;
				break;
				}
				}
			}
			else
			{
				if (ToyChicaCamera == 13)
				{
					StartCoroutine(ToyChicaFunction(false));
				}
			}
			ToyChicaMovement = 5f;
		}
		if (ToyFreddyMovement <= 0f)
		{
			int randNum = Random.Range(0, 20);
			if (ToyFreddyAI >= randNum || ToyFreddyAI == randNum)
			{
				if(CamWatchAI != 2)
				{
				switch (ToyFreddyCamera)
				{
				case 9:
				ToyFreddyCamera = 10;
				while (ToyBonnieCamera == 9 && ToyChicaCamera == 9)
				{
					StartCoroutine(DisruptCamera(Random.Range(9, 11)));
				}
				StartCoroutine(DisruptCamera(Random.Range(9, 11)));
				break;
				case 10:
				ToyFreddyCamera = 14;
				StartCoroutine(DisruptCamera(Random.Range(10, 13)));
				break;
				case 14:
				ToyFreddyCamera = 15;
				break;
				case 15:
				ToyFreddyCamera = 13;
				StartCoroutine(PrepareBlackout("ToyFreddy"));
				break;
				}
				}
			}
			ToyFreddyMovement = 5f;
		}
		if (MangleMovement <= 0f)
		{
			int randNum = Random.Range(0, 20);
			if (MangleAI >= randNum || MangleAI == randNum)
			{
				if(CamWatchAI != 9)
				{
					switch (MangleCamera)
				{
					case 12:
					MangleCamera = 11;
					StartCoroutine(DisruptCamera(Random.Range(11, 13)));
					StartCoroutine(DisruptCamera(Random.Range(11, 13)));
					break;
					case 11:
					MangleCamera = 10;
					StartCoroutine(DisruptCamera(Random.Range(10, 12)));
					StartCoroutine(DisruptCamera(Random.Range(10, 12)));
					break;
					case 10:
					MangleCamera = 7;
					StartCoroutine(DisruptCamera(Random.Range(7, 10)));
					StartCoroutine(DisruptCamera(Random.Range(7, 10)));
					break;
					case 7:
					MangleCamera = 1;
					StartCoroutine(DisruptCamera(Random.Range(1, 4)));
					StartCoroutine(DisruptCamera(Random.Range(1, 7)));
					break;
					case 1:
					MangleCamera = 6;
					StartCoroutine(DisruptCamera(Random.Range(1, 4)));
					StartCoroutine(DisruptCamera(Random.Range(1, 4)));
					break;
					case 6:
					MangleCamera = 13;
					StartCoroutine(DisruptCamera(Random.Range(6, 8)));
					break;
					case 13:
					if (!maskManager.isMaskActive)
					{
						MangleCamera = 16;
						ManglePrepared = true;
					}
					else
					{
						MangleCamera = 12;
					}
					break;
				}
				}
			}
			MangleMovement = 5f;
		}
		if (WitheredFreddyMovement <= 0f)
		{
		    int randNum = Random.Range(0, 20);

		    if (WitheredFreddyAI >= randNum || WitheredFreddyAI == randNum)
		    {
		        if (CamWatchAI != 5)
		        {
		            switch (WitheredFreddyCamera)
		            {
		                case 8:
		                    WitheredFreddyCamera = 7;
		                    StartCoroutine(DisruptCamera(Random.Range(7, 10)));
		                    StartCoroutine(DisruptCamera(Random.Range(7, 10)));
		                    break;

		                case 7:
		                    WitheredFreddyCamera = 3;
		                    StartCoroutine(DisruptCamera(Random.Range(3, 7)));
		                    StartCoroutine(DisruptCamera(Random.Range(3, 7)));
		                    break;

		                case 3:
		                    WitheredFreddyCamera = 14;
		                    StartCoroutine(DisruptCamera(Random.Range(3, 7)));
		                    break;

		                case 14:

		                    StartCoroutine(PrepareBlackout("WitheredFreddy"));
		                    WitheredFreddyCamera = 20;

		                    break;
		            }
		        }
		    }
		    WitheredFreddyMovement = 5f;
		}
		if (WitheredBonnieMovement <= 0f)
		{
			int randNum = Random.Range(0, 20);
			if (WitheredBonnieAI >= randNum || WitheredBonnieAI == randNum)
			{
				if(CamWatchAI != 4)
				{
					switch (WitheredBonnieCamera)
				{
				case 8:
				WitheredBonnieCamera = 7;
				StartCoroutine(DisruptCamera(Random.Range(7, 10)));
				StartCoroutine(DisruptCamera(Random.Range(7, 10)));
				break;
				case 7:
				WitheredBonnieCamera = 14;
				StartCoroutine(DisruptCamera(Random.Range(7, 10)));
				break;
				case 14:
				WitheredBonnieCamera = 1;
				StartCoroutine(DisruptCamera(Random.Range(1, 4)));
				break;
				case 1:
				WitheredBonnieCamera = 5;
				StartCoroutine(DisruptCamera(Random.Range(1, 4)));
				StartCoroutine(DisruptCamera(Random.Range(5, 7)));
				break;
				case 5:
				StartCoroutine(PrepareBlackout("WitheredBonnie"));
				WitheredBonnieCamera = 20;
				break;
				}
				}
			}
			WitheredBonnieMovement = 5f;
		}
		if (WitheredChicaMovement <= 0f)
		{
			int randNum = Random.Range(0, 20);
			if (WitheredChicaAI >= randNum || WitheredChicaAI == randNum)
			{
				if(CamWatchAI != 6)
				{
				switch (WitheredChicaCamera)
				{
				case 8:
				WitheredChicaCamera = 4;
				StartCoroutine(DisruptCamera(Random.Range(4, 7)));
				StartCoroutine(DisruptCamera(Random.Range(4, 7)));
				break;
				case 4:
				WitheredChicaCamera = 2;
				StartCoroutine(DisruptCamera(Random.Range(4, 7)));
				StartCoroutine(DisruptCamera(Random.Range(2, 4)));
				break;
				case 2:
				WitheredChicaCamera = 6;
				StartCoroutine(DisruptCamera(Random.Range(2, 4)));
				StartCoroutine(DisruptCamera(Random.Range(2, 4)));
				break;
				case 6:
				StartCoroutine(PrepareBlackout("WitheredChica"));
				WitheredChicaCamera = 20;
				StartCoroutine(DisruptCamera(Random.Range(6, 8)));
				break;
				}
				}
				
			}
			WitheredChicaMovement = 5f;
		}
		if (WitheredFoxyMovement <= 0f)
		{
			int randNum = Random.Range(0, 20);
			if (WitheredFoxyAI >= randNum || WitheredFoxyAI == randNum)
			{
				if(CamWatchAI != 7)
				{
				switch (WitheredFoxyCamera)
				{
				case 8:
				WitheredFoxyCamera = 18;
				StartCoroutine(DisruptCamera(Random.Range(18, 20)));
				break;
				case 18:
				WitheredFoxyCamera = 19;
				break;
				case 19:
				WitheredFoxyCamera = 14;
				break;
				case 14:
				WitheredFoxyPrepared = true;
				WitheredFoxyCamera = 20;
				break;
				}
				}
			}
			WitheredFoxyMovement = 5f;
		}
		if (BBMovement <= 0f)
		{
			int randNum = Random.Range(0, 20);
			if (BalloonBoyAI >= randNum || BalloonBoyAI == randNum)
			{
				AudioSource[] audioSources = BBSounds.GetComponents<AudioSource>();

    			// Check if there are any AudioSource components
    			if (audioSources.Length > 0 && BBCamera != 16 && BBCamera != 15)
    			{
        			// Generate a random index
        			int randomIndex = Random.Range(0, audioSources.Length);

        			// Play the selected AudioSource
        			audioSources[randomIndex].Play();
    			}
				if(CamWatchAI != 8)
				{
					switch (BBCamera)
					{
					case 10:
					BBCamera = 18;
					break;
					case 18:
					BBCamera = 19;
					break;
					case 19:
					BBCamera = 5;
					StartCoroutine(DisruptCamera(Random.Range(5, 7)));
					VentCrawl.Play();
					break;
					case 5:
					StartCoroutine(DisruptCamera(Random.Range(5, 7)));
					BBCamera = 13;
					break;
					case 13:
					if (!maskManager.isMaskActive)
					{
						BBCamera = 15;
					}
					else
					{
						BBCamera = 10;
					}
					break;
					}
				}
			}
			BBMovement = 5f;
		}
	}

	void MovementOpportunityMain()
	{
	    if (BlackoutActive)
	    {
	        ToyBonnieBlackoutTime += Time.deltaTime;
	        if (ToyBonnieBlackoutTime >= 1f)
	        {
	            ToyBonnieBlackoutTime = 0f;
	            if (Random.value < 0.33333333f)
	            {
	                ToyBonnieCamera = 9;
	                ToyBonnieMovement = 10f;
	            }
	        }
	    }
	    else
	    {
	        ToyBonnieBlackoutTime = 0f;
	    }

	    // Reduce movement timers for active animatronics
	    if (ToyBonnieAI >= 1) ToyBonnieMovement -= Time.deltaTime;
	    if (ToyChicaAI >= 1) ToyChicaMovement -= Time.deltaTime;
	    if (ToyFreddyAI >= 1) ToyFreddyMovement -= Time.deltaTime;
	    if (MangleAI >= 1) MangleMovement -= Time.deltaTime;
	    if (PaperpalsAI >= 1) ToyFreddyMovement -= Time.deltaTime; // PaperpalsAI should not affect ToyFreddyMovement ?????? - shiro
	    if (WitheredFreddyAI >= 1) WitheredFreddyMovement -= Time.deltaTime;
	    if (WitheredBonnieAI >= 1) WitheredBonnieMovement -= Time.deltaTime;
	    if (WitheredChicaAI >= 1) WitheredChicaMovement -= Time.deltaTime;
	    if (WitheredFoxyAI >= 1) WitheredFoxyMovement -= Time.deltaTime;
	    if (GoldenFreddyAI >= 1 && monitorManager.isMonitorActive) GoldenFreddyCameraTime -= Time.deltaTime;
	    if (BalloonBoyAI >= 1) BBMovement -= Time.deltaTime;
	}

	IEnumerator ToyBonnieFunction(bool isMO)
	{
		if (ToyBonnieCamera == 13)
		{
			if (maskManager.isMaskActive && BlackoutActive == false)
			{
				ToyBonnieCamera = 9;
				ToyBonnieMovement = 11f;
				ToyBonniePrepared = false;
				StartCoroutine(BlackoutCoroutine("ToyBonnie"));
			}
			else if (monitorManager.isMonitorActive && isMO)
			{
				yield return new WaitForSeconds(0.183f);
				JumpscareAnimator.Play("ToyBonnie");
				Jumpscare.Play();
				StartCoroutine(JumpscareSequence());
			}
		}
	}

	IEnumerator ToyChicaFunction(bool isMO)
	{
		if (ToyChicaCamera == 13)
		{
			if (maskManager.isMaskActive && BlackoutActive == false)
			{
				ToyChicaPrepared = false;
				ToyChicaCamera = 9;
				ToyChicaMovement = 11f;
				//StartCoroutine(BlackoutCoroutine("ToyChica"));
			}
			else if (monitorManager.isMonitorActive && isMO)
			{
				yield return new WaitForSeconds(0.183f);
				JumpscareAnimator.Play("ToyChica");
				Jumpscare.Play();
				StartCoroutine(JumpscareSequence());
			}
		}
	}

	IEnumerator PrepareBlackout(string Animatronic)
	{
    if (!BlackoutPrepared)
    {
        BlackoutPrepared = true;
        bool activeBlackout = false;
        bool preparedJumpscare = false;
        float timeForMask = 0f;
        float timeElapsed = 0f;


		while (!activeBlackout)
		{
		    if (stateManager.PlayerState == 2)
		    {
		        activeBlackout = true;
		        StartCoroutine(BlackoutCoroutine());
		        switch (Animatronic)
		        {
		            case "ToyFreddy":
		                ToyFreddyOffice.SetActive(true);
						BlackOutAnim.Play("BlackOut");
						StareSound.Play();
						StartCoroutine(PlayStareSound());
		                break;
		            case "WitheredFreddy":
		                WitheredFreddyOffice.SetActive(true);
						BlackOutAnim.Play("BlackOut");
						StareSound.Play();
						StartCoroutine(PlayStareSound());
		                break;
		            case "WitheredChica":
		                WitheredChicaOffice.SetActive(true);
						BlackOutAnim.Play("BlackOut");
						StareSound.Play();
						StartCoroutine(PlayStareSound());
		                break;
		            case "WitheredBonnie":
		                WitheredBonnieOffice.SetActive(true);
						BlackOutAnim.Play("BlackOut");
						StareSound.Play();
						StartCoroutine(PlayStareSound());
		                break;
		        }
		    }

		    yield return null; // Wait for the next frame before checking again
		}

        while (timeElapsed < 4f)
        {
			if (timeForMask >= 0.7f)
            {
                preparedJumpscare = true;
            }
            if (maskManager.isMaskActive)
            {
                timeForMask -= Time.deltaTime;
            }

			timeForMask += Time.deltaTime;

            timeElapsed += Time.deltaTime;
            yield return null; // Continue checking while waiting
        }

        ToyFreddyOffice.SetActive(false);
		WitheredBonnieOffice.SetActive(false);
		WitheredChicaOffice.SetActive(false);
		WitheredFreddyOffice.SetActive(false);

        yield return new WaitForSeconds(3f); // Wait for additional 3 seconds
		Debug.Log(preparedJumpscare + " state: " + state + timeForMask);

        if (preparedJumpscare)
        {
            if (!maskManager.isMaskActive && monitorManager.isMonitorActive)
            {
                yield return new WaitForSeconds(0.183f);
                JumpscareAnimator.Play(Animatronic);
                Jumpscare.Play();
                StartCoroutine(JumpscareSequence());
            }
            else if (!maskManager.isMaskActive && !monitorManager.isMonitorActive || !maskManager.isMaskActive && state == "OfficeBlackout")
            {

                JumpscareAnimator.Play(Animatronic);
                Jumpscare.Play();
                StartCoroutine(JumpscareSequence());
            }
			else if (maskManager.isMaskActive)
            {
				Mask();
                JumpscareAnimator.Play(Animatronic);
                Jumpscare.Play();
                StartCoroutine(JumpscareSequence());
            }
			else
			{
				JumpscareAnimator.Play(Animatronic);
                Jumpscare.Play();
                StartCoroutine(JumpscareSequence());
				Debug.Log(state);
			}
        }
		else
		{
			switch (Animatronic)
			{
				case "ToyFreddy":
				ToyFreddyCamera = 9;
				ToyFreddyMovement = 8f;
				break;
				case "WitheredFreddy":
				WitheredFreddyCamera = 8;
				WitheredFreddyMovement = 8f;
				break;
				case "WitheredChica":
				WitheredChicaCamera = 8;
				WitheredChicaMovement = 8f;
				break;
				case "WitheredBonnie":
				WitheredBonnieCamera = 8;
				WitheredBonnieMovement = 8f;
				break;
			}
			BlackoutPrepared = false;
		}
    }
	}

	IEnumerator BaloonBoyInOffice()
	{
		// Set up the initial state for BaloonBoy
		BBCamera = 16;
		BaloonBoy.SetActive(true);

		// Get the specific AudioSource component
		AudioSource bbSound = BBSounds.GetComponents<AudioSource>()[2];

		// Play the sound every 3 seconds for 15 seconds
		float duration = 15f;
		float interval = 1.8f;
		float elapsedTime = 0f;

		while (elapsedTime < duration && BaloonBoy.activeSelf)
		{
			// Play the sound
			bbSound.Play();

			// Wait for the interval
			yield return new WaitForSeconds(interval);

			// Update the elapsed time
			elapsedTime += interval;
		}

		BBCamera = 10;
		BBMovement = 9f;
	}

	void PuppetBox()
	{
		if (PuppetAI != 0)
		{
			if (PuppetTime != 0f)
			{
				if (!musicBox.isWindUpEmpty)
    			{
        			PuppetTime += Time.deltaTime * 2.4f;
        			PuppetDeathTimer = 15f;
    			}
    			else
    			{
        			PuppetTime -= Time.deltaTime * ((float)PuppetAI * 0.16f);
    			}
			}
			else
			{
				musicBox.danger.GetComponent<Animator>().SetBool("CharacterMoved", true);

				if (PuppetDeathTimer >= 0.01f)
				{
					PuppetDeathTimer -= Time.deltaTime;
					PuppetDeathTimer = Mathf.Clamp(PuppetDeathTimer, 0f, 15f);
				}
				else
				{
					StartCoroutine(PuppetDeathSequence());
				}

				if (!musicBox.isWindUpEmpty)
				{
					PuppetTime += Time.deltaTime * 1.5f;
					PuppetDeathTimer = 15f;
				}
			}
		}
	}

	IEnumerator PuppetDeathSequence()
	{
		Debug.Log(state);
		if (!isJumpscared && canDeathSeqeuence == true)
		{
			canDeathSeqeuence = false;
			jackInTheBox.Play();
			yield return new WaitForSeconds(5f);

			if (!maskManager.isMaskActive && monitorManager.isMonitorActive)
			{
				//StartCoroutine(MonitorDownIE());
				yield return new WaitForSeconds(0.183f);
				JumpscareAnimator.Play("Puppet");
				Jumpscare.Play();
			}
			else if (!maskManager.isMaskActive && !monitorManager.isMonitorActive || !maskManager.isMaskActive && state == "OfficeBlackout")
			{
				JumpscareAnimator.Play("Puppet");
				Jumpscare.Play();
			}
			else if (maskManager.isMaskActive)
			{
				JumpscareAnimator.Play("Puppet");
				Jumpscare.Play();
			}

			StartCoroutine(JumpscareSequence());
		}
	}

	void StateChecks()
	{
		if (RWQActive)
		{
			RWQCrashTimer -= Time.deltaTime;
			if (RWQCrashTimer <= 0f)
			{
			//	UnityEngine.N3DS.Debug.Crash("RWQFSFASXC");
			}
		}
		if (lightsManager.isLightActive)
		{
			DeactivateRWQ();
		}

		// System for change office sprites
		if (!maskManager.isMaskActive && !monitorManager.isMonitorActive && lightsManager.isLightActive)
		{
			if (lightsManager.leftLightEnabled) // Left light
			{
				if (ToyChicaCamera == 13)
				{
					officeImage.sprite = ToyChicaFlashlightCams[12];
				}
				else if (BBCamera == 13)
				{
					officeImage.sprite = BBFlashlightCams[12];
				}
				else
				{
					officeImage.sprite = leftLightOffice;
				}

				// When enable the left button sprite and disable the right button sprite
				leftButtonImage.sprite = leftButtonOn;
				rightButtonImage.sprite = rightButtonOff;
			}
			else if (lightsManager.centerLightEnabled) // Center light
			{
				if (WitheredFoxyCamera == 14 && WitheredBonnieCamera == 14)
				{
					officeImage.sprite = WitheredBonnieFlashlightCams[14];
				}
				else if (WitheredFoxyCamera == 14)
				{
					officeImage.sprite = WitheredFoxyFlashlightCams[13];
				}
				else if (WitheredFreddyCamera == 14)
				{
					officeImage.sprite = WitheredFreddyFlashlightCams[13];
				}
				else if (WitheredBonnieCamera == 14)
				{
					officeImage.sprite = WitheredBonnieFlashlightCams[13];
				}
				else if (ToyFreddyCamera == 14)
				{
					officeImage.sprite = ToyFreddyFlashlightCams[13];
				}
				else if (ToyFreddyCamera == 15)
				{
					officeImage.sprite = ToyFreddyFlashlightCams[14];
				}
				else if (ToyChicaCamera == 14)
				{
					officeImage.sprite = ToyChicaFlashlightCams[13];
				}
				else
				{
					officeImage.sprite = centerLightOffice;
				}

				if (GoldenFreddyAI >= 1)
				{
					if (GoldenFreddyAI >= GoldenFreddyrandNum && GoldenFreddyCameraTime <= 0 && GoldenFreddyPrepared == true)
					{
						GoldenFreddyInOffice = true;
						officeImage.sprite = GoldenFreddyFlashlightCams[13];
						GoldenFreddyInHall = true;
						GoldenFreddyPrepared = false;
					}
					else
					{
						officeImage.sprite = centerLightOffice;
						Debug.Log(GoldenFreddyrandNum.ToString() + GoldenFreddyAI.ToString() + GoldenFreddyCameraTime.ToString() + GoldenFreddyInOffice.ToString() + GoldenFreddyPrepared.ToString());
					}
				}

				// We disable all buttons sprites
				leftButtonImage.sprite = leftButtonOff;
				rightButtonImage.sprite = rightButtonOff;
			}
			else if (lightsManager.rightLightEnabled)
			{
				if (ToyBonnieCamera == 13)
				{
					officeImage.sprite = ToyBonnieFlashlightCams[12];
				}
				else if (MangleCamera == 13)
				{
					officeImage.sprite = MangleFlashlightCams[12];
				}
				else
				{
					officeImage.sprite = rightLightOffice;
				}

				// When enable the right button sprite and disable the left button sprite
				leftButtonImage.sprite = leftButtonOff;
				rightButtonImage.sprite = rightButtonOn;
			}
		}
		else if (!maskManager.isMaskActive && monitorManager.isMonitorActive && lightsManager.isLightActive && currentCam == 11)
		{
			// Check for the special case when puppetDeathTimer is 0f
			if (PuppetDeathTimer <= 0.01f || PuppetDeathTimer == 0.01f)
			{
				// 9/10 chance to select puppetSprites[3], 1/10 chance to select puppetCameraEndoSprite
				if (puppetEndoChance < 0.1f)
				{
					MainCameraBG.sprite = PuppetCameraEndoSprite;
				}
				else
				{
					MainCameraBG.sprite = PuppetCameraSprites[4];
				}
			}
			else
			{
				// Calculate the sprite index based on puppetDeathTimer
				int spriteIndex = Mathf.FloorToInt((15f - PuppetDeathTimer) / 1f); // This maps 5 -> 0, 4 -> 1, 3 -> 2, etc.
				Debug.Log(PuppetDeathTimer);

				// Ensure the sprite index is within bounds
				spriteIndex = Mathf.Clamp(spriteIndex, 1, PuppetCameraSprites.Length - 1);


				// Set the displayed sprite based on the calculated index
				MainCameraBG.sprite = PuppetCameraSprites[spriteIndex - 1];
			}
		}
        /*else if (!maskManager.isMaskActive && isMonitorActive && flashlightActive && currentCam == 5)
		{
			if (puppetEndoChance < 0.001)
			{
				MainCameraBG.sprite = LeftVentEndoSprite;
			}
			else
			{
				MainCameraBG.sprite = FlashlightedCams[currentCam-1];
			}
		}*/
        else if (!maskManager.isMaskActive && monitorManager.isMonitorActive && currentCam == 4)
		{
			if (lightsManager.isLightActive)
			{
				if (ToyBonnieCamera == 4)
				{
					MainCameraBG.sprite = ToyBonnieFlashlightCams[3];
				}
				else if (ToyChicaCamera == 4)
				{
					MainCameraBG.sprite = ToyChicaFlashlightCams[3];
				}
				else if (WitheredChicaCamera == 4)
				{
					MainCameraBG.sprite = WitheredChicaFlashlightCams[3];
				}
				else
				{
					MainCameraBG.sprite = FlashlightedCams[currentCam - 1];
				}
			}
			else
			{
				if (ToyBonnieCamera == 4)
				{
					MainCameraBG.sprite = ToyBonnieDefaultCams[3];
				}
				else if (ToyChicaCamera == 4)
				{
					MainCameraBG.sprite = ToyChicaDefaultCams[3];
				}
				else
				{
					MainCameraBG.sprite = DefaultCams[currentCam - 1];
				}
			}
		}
		else if (!maskManager.isMaskActive && monitorManager.isMonitorActive && currentCam == 12 && lightsManager.isLightActive)
		{
			if (MangleCamera != 12)
			{
				MainCameraBG.sprite = MangleFlashlightCams[11];
			}
			else if (MangleCamera == 6)
			{
				MainCameraBG.sprite = MangleFlashlightCams[5];
			}
			else if (MangleCamera == 12)
			{
				MainCameraBG.sprite = FlashlightedCams[currentCam - 1];
			}
		}
		else if (!maskManager.isMaskActive && monitorManager.isMonitorActive && currentCam == 10)
		{
			if (!lightsManager.isLightActive)
			{
				Debug.Log("ToyFreddy Cam : " + ToyFreddyCamera);
				if (ToyFreddyCamera == 10)
				{
					MainCameraBG.sprite = ToyFreddyDefaultCams[9];
				}
				else
				{
					MainCameraBG.sprite = DefaultCams[currentCam - 1];
				}
			}
			else
			{
				if (ToyFreddyCamera == 10)
				{
					MainCameraBG.sprite = ToyFreddyFlashlightCams[9];
				}
				else
				{
					MainCameraBG.sprite = FlashlightedCams[currentCam - 1];
				}
			}
		}
		else if (!maskManager.isMaskActive && monitorManager.isMonitorActive && currentCam == 3)
		{
			if (lightsManager.isLightActive)
			{
				if (ToyBonnieCamera == 3)
				{
					MainCameraBG.sprite = ToyBonnieFlashlightCams[2];
				}
				else if (WitheredFreddyCamera == 3)
				{
					MainCameraBG.sprite = WitheredFreddyFlashlightCams[2];
				}
				else
				{
					MainCameraBG.sprite = FlashlightedCams[currentCam - 1];
				}
			}
			else
			{
				if (ToyBonnieCamera == 3)
				{
					MainCameraBG.sprite = ToyBonnieDefaultCams[2];
				}
				else if (WitheredFreddyCamera == 3)
				{
					MainCameraBG.sprite = WitheredFreddyDefaultCams[2];
				}
				else
				{
					MainCameraBG.sprite = DefaultCams[currentCam - 1];
				}
			}
		}
		else if (!maskManager.isMaskActive && monitorManager.isMonitorActive && currentCam == 2)
		{
			if (lightsManager.isLightActive)
			{
				if (WitheredChicaCamera == 2)
				{
					MainCameraBG.sprite = WitheredChicaFlashlightCams[1];
				}
				else if (ToyBonnieCamera == 2)
				{
					MainCameraBG.sprite = ToyBonnieFlashlightCams[1];
				}
				else
				{
					MainCameraBG.sprite = FlashlightedCams[currentCam - 1];
				}
			}
			else
			{
				if (WitheredChicaCamera == 2)
				{
					MainCameraBG.sprite = WitheredChicaDefaultCams[1];
				}
				else if (ToyBonnieCamera == 2)
				{
					MainCameraBG.sprite = ToyBonnieDefaultCams[1];
				}
				else
				{
					MainCameraBG.sprite = DefaultCams[currentCam - 1];
				}
			}
		}
		else if (!maskManager.isMaskActive && monitorManager.isMonitorActive && currentCam == 6)
		{
			if (lightsManager.isLightActive)
			{
				if (ToyBonnieCamera == 6)
				{
					MainCameraBG.sprite = ToyBonnieFlashlightCams[5];
				}
				else if (MangleCamera == 6)
				{
					MainCameraBG.sprite = MangleFlashlightCams[5];
				}
				else if (WitheredChicaCamera == 6)
				{
					MainCameraBG.sprite = WitheredChicaFlashlightCams[4];
				}
				else
				{
					MainCameraBG.sprite = FlashlightedCams[currentCam - 1];
				}
			}
			else
			{
				if (ToyBonnieCamera == 6)
				{
					MainCameraBG.sprite = ToyBonnieDefaultCams[5];
				}
				else
				{
					MainCameraBG.sprite = DefaultCams[currentCam - 1];
				}
			}
		}
		else if (!maskManager.isMaskActive && monitorManager.isMonitorActive && currentCam == 7)
		{
			if (!lightsManager.isLightActive)
			{
				if (ToyChicaCamera == 7)
				{
					MainCameraBG.sprite = ToyChicaDefaultCams[6];
				}
				else
				{
					MainCameraBG.sprite = DefaultCams[currentCam - 1];
				}
			}
			else
			{
				if (ToyChicaCamera == 7)
				{
					MainCameraBG.sprite = ToyChicaFlashlightCams[6];
				}
				else if (WitheredFreddyCamera == 7)
				{
					MainCameraBG.sprite = WitheredFreddyFlashlightCams[6];
				}
				else if (WitheredBonnieCamera == 7)
				{
					MainCameraBG.sprite = WitheredBonnieFlashlightCams[6];
				}
				else
				{
					MainCameraBG.sprite = FlashlightedCams[currentCam - 1];
				}
			}
		}
		else if (!maskManager.isMaskActive && monitorManager.isMonitorActive && currentCam == 1)
		{
			if (!lightsManager.isLightActive)
			{
				if (ToyChicaCamera == 1)
				{
					MainCameraBG.sprite = ToyChicaDefaultCams[0];
				}
				else
				{
					MainCameraBG.sprite = DefaultCams[currentCam - 1];
				}
			}
			else
			{
				if (ToyChicaCamera == 1)
				{
					MainCameraBG.sprite = ToyChicaFlashlightCams[0];
				}
				else if (WitheredBonnieCamera == 1)
				{
					MainCameraBG.sprite = WitheredBonnieFlashlightCams[0];
				}
				else
				{
					MainCameraBG.sprite = FlashlightedCams[currentCam - 1];
				}
			}
		}
		else if (!maskManager.isMaskActive && monitorManager.isMonitorActive && currentCam == 5)
		{
			if (!lightsManager.isLightActive)
			{
				if (ToyChicaCamera == 5)
				{
					MainCameraBG.sprite = ToyChicaDefaultCams[4];
				}
				else if (BBCamera == 5)
				{
					MainCameraBG.sprite = BBDefaultCams[4];
				}
				else
				{
					MainCameraBG.sprite = DefaultCams[currentCam - 1];
				}
			}
			else
			{
				if (ToyChicaCamera == 5)
				{
					MainCameraBG.sprite = ToyChicaFlashlightCams[4];
				}
				else if (BBCamera == 5)
				{
					MainCameraBG.sprite = BBFlashlightCams[4];
				}
				else if (WitheredBonnieCamera == 5)
				{
					MainCameraBG.sprite = WitheredBonnieFlashlightCams[4];
				}
				else if (puppetEndoChance < 0.001)
				{
					MainCameraBG.sprite = LeftVentEndoSprite;
				}
				else
				{
					MainCameraBG.sprite = FlashlightedCams[currentCam - 1];
				}
			}
		}
		else if (!maskManager.isMaskActive && monitorManager.isMonitorActive && currentCam == 8 && lightsManager.isLightActive)
		{
			if (WitheredFoxyCamera == 8 && WitheredFoxyAI >= 1)
			{
				MainCameraBG.sprite = WitheredFoxyFlashlightCams[7];
			}
			else if (WitheredFoxyCamera != 8 && WitheredFreddyCamera != 8 && WitheredChicaCamera != 8 && WitheredBonnieCamera != 8)
			{
				if (puppetEndoChance <= 0.1f)
				{
					MainCameraBG.sprite = Cam8Sprites[4];
				}
				else
				{
					MainCameraBG.sprite = Cam8Sprites[2];
				}
			}
			else if (WitheredFoxyCamera != 8 && WitheredFreddyCamera == 8 && WitheredChicaCamera == 8 && WitheredBonnieCamera != 8)
			{
				MainCameraBG.sprite = Cam8Sprites[0];
			}
			else if (WitheredFoxyCamera != 8 && WitheredFreddyCamera == 8 && WitheredChicaCamera != 8 && WitheredBonnieCamera != 8)
			{
				MainCameraBG.sprite = Cam8Sprites[1];
			}
			else if (WitheredFoxyCamera == 8 && WitheredFreddyCamera == 8 && WitheredChicaCamera == 8 && WitheredBonnieCamera == 8)
			{
				MainCameraBG.sprite = FlashlightedCams[currentCam - 1];
			}
			else
			{
				MainCameraBG.sprite = MainCameraBG.sprite = Cam8Sprites[2];
			}
		}
		else if (!maskManager.isMaskActive && monitorManager.isMonitorActive && currentCam == 9)
		{
			if (!lightsManager.isLightActive)
			{
				if (ToyBonnieCamera == 9 && ToyChicaCamera == 9 && ToyFreddyCamera == 9)
				{
					MainCameraBG.sprite = DefaultCams[currentCam - 1];
				}
				else if (ToyBonnieCamera != 9 && ToyChicaCamera == 9 && ToyFreddyCamera == 9)
				{
					MainCameraBG.sprite = Cam9Sprites[0];
				}
				else if (ToyBonnieCamera != 9 && ToyChicaCamera != 9 && ToyFreddyCamera == 9)
				{
					MainCameraBG.sprite = Cam9Sprites[2];
				}
				else
				{
					MainCameraBG.sprite = Cam9Sprites[4];
				}
			}
			else
			{
				if (ToyBonnieCamera == 9 && ToyChicaCamera == 9 && ToyFreddyCamera == 9)
				{
					MainCameraBG.sprite = FlashlightedCams[currentCam - 1];
				}
				else if (ToyBonnieCamera != 9 && ToyChicaCamera == 9 && ToyFreddyCamera == 9)
				{
					MainCameraBG.sprite = Cam9Sprites[1];
				}
				else if (ToyBonnieCamera != 9 && ToyChicaCamera != 9 && ToyFreddyCamera == 9)
				{
					MainCameraBG.sprite = Cam9Sprites[3];
				}
				else
				{
					MainCameraBG.sprite = Cam9Sprites[4];
				}
			}
		}
		else if (!maskManager.isMaskActive && monitorManager.isMonitorActive && lightsManager.isLightActive)
		{
			MainCameraBG.sprite = FlashlightedCams[currentCam - 1];
		}
		else if (!maskManager.isMaskActive && monitorManager.isMonitorActive && !lightsManager.isLightActive)
		{
			MainCameraBG.sprite = DefaultCams[currentCam - 1];
		}
		else // We really should clean this code in later updates because have a script that long is not a good idea for our mental health
		{
			// Set the default sprite to office image
			officeImage.sprite = defaultOffice;

			// We disable all buttons sprites
			leftButtonImage.sprite = leftButtonOff;
			rightButtonImage.sprite = rightButtonOff;
		}

		ToyBonnieMaskTimer -= Time.deltaTime;
		ToyChicaMaskTimer -= Time.deltaTime;
		BBMaskTimer -= Time.deltaTime;
		MangleMaskTimer -= Time.deltaTime;

		if (ToyBonnieMaskTimer < 0f)
		{
			ToyBonnieMaskTimer = 1f;
			if (maskManager.isMaskActive && ToyBonnieCamera == 13 && Random.value < 0.49f)
			{
				StartCoroutine(ToyBonnieFunction(false));
			}
		}
		if (ToyChicaMaskTimer < 0f)
		{
			ToyChicaMaskTimer = 1f;
			if (maskManager.isMaskActive && ToyChicaCamera == 13 && Random.value < 0.1f)
			{
				StartCoroutine(ToyChicaFunction(false));
			}
		}
		if (BBMaskTimer < 0f)
		{
			BBMaskTimer = 1f;
			if (maskManager.isMaskActive && BBCamera == 13 && Random.value < 0.1f)
			{
				BBCamera = 10;
				BBMovement = 5f;
			}
		}
		if (MangleMaskTimer < 0f)
		{
			MangleMaskTimer = 1f;
			if (maskManager.isMaskActive && MangleCamera == 13 && Random.value < 0.1f)
			{
				MangleCamera = 12;
				MangleMovement = 5f;
			}
		}

		MangleInRooms[9].SetActive(false);
		MangleInRooms[10].SetActive(false);
		MangleInRooms[6].SetActive(false);
		MangleInRooms[0].SetActive(false);
		if (currentCam == MangleCamera && MangleCamera != 12 && monitorManager.isMonitorActive)
		{
			Mangle.mute = false;
		}
		else if (currentCam != MangleCamera && MangleCamera != 16 && monitorManager.isMonitorActive)
		{
			Mangle.mute = true;
		}
		else if (MangleCamera != 16 && !maskManager.isMaskActive && !monitorManager.isMonitorActive)
		{
			Mangle.mute = true;
		}
		else if (MangleCamera == 16)
		{
			Mangle.mute = false;
		}
		if (MangleCamera == 11 && currentCam == 11)
		{
			MangleInRooms[10].SetActive(true);
		}
		else if (MangleCamera == 10 && currentCam == 10)
		{
			MangleInRooms[9].SetActive(true);
		}
		else if (MangleCamera == 7 && currentCam == 7)
		{
			MangleInRooms[6].SetActive(true);
		}
		else if (MangleCamera == 1 && currentCam == 1)
		{
			MangleInRooms[0].SetActive(true);
		}

		Ribbons.SetActive(false);
		if (currentCam == 5 || currentCam == 6)
		{
			Ribbons.SetActive(true);
		}

	}

	IEnumerator DisruptCamera(int camera)
	{
		if (currentCam == camera)
		{
			//54321
			MainCameraBG.gameObject.SetActive(false);
			SignalDisrupted.SetActive(true);
			DisruputedSound.Play();
			yield return new WaitForSeconds(3f);
			MainCameraBG.gameObject.SetActive(true);
			SignalDisrupted.SetActive(false);
			DisruputedSound.Stop();
		}
	}

	private IEnumerator BlackoutCoroutine()
    {
		Debug.Log("BlackOutCoroutine 1 called");
		BlackoutActive = true;
        float duration = 4f;
        float elapsed = 0f;
        Color color = Blackout.color;

        // Gradually increase and decrease alpha over 4 seconds
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.PingPong(elapsed * 2f, 1f); // Oscillates between 0 and 1
            color.a = t;
            Blackout.color = color;
            yield return null;
        }

        // Fully turn black
        color.a = 1f;
        Blackout.color = color;

        // Slowly fade out in 3 seconds
        duration = 3f;
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = 1f - (elapsed / duration);
            Blackout.color = color;
            yield return null;
        }

        // Ensure fully transparent at the end
        color.a = 0f;
        Blackout.color = color;
    }

	private IEnumerator BlackoutCoroutine(string Animatronic)
    {
		Debug.Log("BlackOutCoroutine 2 called");
		BlackoutActive = true;
		currentBlackout = Animatronic;
        float duration = 4f;
        float elapsed = 0f;
        Color color = Blackout.color;

        // Gradually increase and decrease alpha over 4 seconds
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.PingPong(elapsed * 2f, 1f); // Oscillates between 0 and 1
            color.a = t;
            Blackout.color = color;
			switch (Animatronic)
			{
				case "ToyBonnie":
				if (BlackoutActive)
				{
					ToyBonnieBlackout.gameObject.SetActive(true);
					BlackOutAnim.Play("BlackOut");
					StareSound.Play();
					StartCoroutine(PlayStareSound());
				}
				// Target position in local space
       			Vector3 targetPosition = Vector3.zero;

        		// Current local position
        		Vector3 currentPosition = ToyBonnieBlackout.transform.localPosition;

        		// Calculate the new position
        		Vector3 newPosition = Vector3.MoveTowards(currentPosition, targetPosition, 100f * Time.deltaTime);

       			// Update the local position
        		ToyBonnieBlackout.transform.localPosition = newPosition;
				break;
				case "ToyChica":
				if (BlackoutActive)
				{
					ToyChicaBlackout.gameObject.SetActive(true);
				}
				// Target position in local space
       			Vector3 targetPositionC = Vector3.zero;

        		// Current local position
        		Vector3 currentPositionC = ToyChicaBlackout.transform.localPosition;

        		// Calculate the new position
        		Vector3 newPositionC = Vector3.MoveTowards(currentPositionC, targetPositionC, 100f * Time.deltaTime);

       			// Update the local position
        		ToyChicaBlackout.transform.localPosition = newPositionC;
				break;
			}
            yield return null;
        }

        // Fully turn black
        color.a = 1f;
        Blackout.color = color;
		ToyBonnieBlackout.gameObject.SetActive(false);
		//ToyChicaBlackout.gameObject.SetActive(false);
		BlackoutActive = false;

        // Slowly fade out in 3 seconds
        duration = 3f;
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = 1f - (elapsed / duration);
            Blackout.color = color;
            yield return null;
        }

        // Ensure fully transparent at the end
        color.a = 0f;
        Blackout.color = color;
    }

	void InputFunction()
	{
        // Get the current state of the GamePad and Remote
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        // Handle GamePad inputs
		if (gamePadState.gamePadErr == WiiU.GamePadError.None)
		{
			// Is triggered
			if (gamePadState.IsTriggered(WiiU.GamePadButton.A))
			{
				HandleCameraAndFoxyStates();
			}
		}

		// Handle keyboard inputs
        if (Application.isEditor)
		{
            if (Input.GetKeyDown(KeyCode.A))
            {
                HandleCameraAndFoxyStates();
            }
        }
    }

    private void HandleCameraAndFoxyStates()
	{
        if (BBCamera == 16)
		{
            ErrorSound.Play();
        }
		else
		{
            puppetEndoChance = Random.value;
            GoldenFreddyrandNum = Random.Range(0, 20);
            if (monitorManager.isMonitorActive)
			{
				FlashCam(currentCam);
			}

            if (!maskManager.isMaskActive && !monitorManager.isMonitorActive && lightsManager.centerLightEnabled)
            {
                FlashCam(14);
                FlashCam(15);
                if (WitheredFoxyCamera == 14)
                {
                    WitheredFoxyMovement += 1f;
                    if (WitheredFoxyMovement >= 12f)
                    {
                        WitheredFoxyCamera = 8;
                        WitheredFoxyMovement = 9f;
                    }
                }
            }
        }
    }

	private void IsGoldenFreddyInHall()
	{
		if (BBCamera != 16 && lightsManager.centerLightEnabled && !maskManager.isMaskActive && !monitorManager.isMonitorActive)
		{
            if (GoldenFreddyInHall == true)
            {
                GoldenFreddyOffice.SetActive(false);
                GoldenFreddyCameraTime = 5f;
                GoldenFreddyMovement = 1.67f;
                GoldenFreddyInOffice = false;
                GoldenFreddyInHall = false;
                GoldenFreddyPrepared = false;
            }
        }
	}

	private void CameraManager()
	{
		if (!isJumpscared)
		{
            if (ToyBonniePrepared)
            {
                JumpscareAnimator.Play("ToyBonnie");
                Jumpscare.Play();
                StartCoroutine(JumpscareSequence());
            }
            else if (ToyChicaPrepared)
            {
                JumpscareAnimator.Play("ToyChica");
                Jumpscare.Play();
                StartCoroutine(JumpscareSequence());
            }
            else
            {
                // This part was for remove the monitor if jumpscared, now useless

				//dude just delete it if it's useless 😭
            }
        }
	}
	// ---

    void FlashCam(int camNumber) //this code is terrible
	{
		if (ToyBonnieCamera == camNumber)
		{
			if (!ToyBonnieFlashed)
			{
				StartCoroutine(FlashAnimatronic("ToyBonnie"));
			}
		}
		if (ToyChicaCamera == camNumber)
		{
			if (!ToyChicaFlashed)
			{
				StartCoroutine(FlashAnimatronic("ToyChica"));
			}
		}
		if (ToyFreddyCamera == camNumber)
		{
			if (!ToyFreddyFlashed)
			{
				StartCoroutine(FlashAnimatronic("ToyFreddy"));
			}
		}
		if (MangleCamera == camNumber)
		{
			if (!MangleFlashed)
			{
				StartCoroutine(FlashAnimatronic("Mangle"));
			}
		}
		if (PaperpalsCamera == camNumber)
		{
			if (!PaperpalsFlashed)
			{
				StartCoroutine(FlashAnimatronic("Paperpals"));
			}
		}
		if (WitheredFreddyCamera == camNumber)
		{
			if (!WitheredFreddyFlashed)
			{
				StartCoroutine(FlashAnimatronic("WitheredFreddy"));
			}
		}
		if (WitheredBonnieCamera == camNumber)
		{
			if (!WitheredFreddyFlashed)
			{
				StartCoroutine(FlashAnimatronic("WitheredBonnie"));
			}
		}
		if (WitheredChicaCamera == camNumber)
		{
			if (!WitheredFreddyFlashed)
			{
				StartCoroutine(FlashAnimatronic("WitheredChica"));
			}
		}
	}


	IEnumerator FlashAnimatronic(string animatronicName)
	{
		switch (animatronicName)
		{
			case "ToyBonnie":
				ToyBonnieFlashed = true;
				ToyBonnieMovement += 6.66f;
				yield return new WaitForSeconds(6.66f);
				ToyBonnieFlashed = false;
				break;
			case "ToyChica":
				ToyChicaFlashed = true;
				ToyChicaMovement += 6.66f;
				yield return new WaitForSeconds(6.66f);
				ToyChicaFlashed = false;
				break;
			case "ToyFreddy":
				ToyFreddyFlashed = true;
				ToyFreddyMovement += 6.66f;
				yield return new WaitForSeconds(6.66f);
				ToyFreddyFlashed = false;
				break;
			case "Mangle":
				MangleFlashed = true;
				MangleMovement += 6.66f;
				yield return new WaitForSeconds(6.66f);
				MangleFlashed = false;
				break;
			case "Paperpals":
				PaperpalsFlashed = true;
				PaperpalsMovement += 6.66f;
				yield return new WaitForSeconds(6.66f);
				PaperpalsFlashed = false;
				break;
			case "WitheredFreddy":
				WitheredFreddyFlashed = true;
				WitheredFreddyMovement += 6.66f;
				yield return new WaitForSeconds(6.66f);
				WitheredFreddyFlashed = false;
				break;
			case "WitheredBonnie":
				WitheredBonnieFlashed = true;
				WitheredBonnieMovement += 6.66f;
				yield return new WaitForSeconds(6.66f);
				WitheredBonnieFlashed = false;
				break;
			case "WitheredChica":
				WitheredChicaFlashed = true;
				WitheredChicaMovement += 6.66f;
				yield return new WaitForSeconds(6.66f);
				WitheredChicaFlashed = false;
				break;
		}
	}

	public void Mask()
	{
		// Help idk where to move this
		if (!isJumpscared)
		{
			if (GoldenFreddyInOffice)
			{
				GoldenFreddyOffice.SetActive(false);
				GoldenFreddyCameraTime = 5f;
				GoldenFreddyMovement = 1.67f;
				GoldenFreddyInOffice = false;
			}

			if (!maskManager.isMaskActive)
			{
				if (BlackoutActive)
				{
					switch (currentBlackout)
					{
						case "ToyBonnie":
							ToyBonnieCamera = 13;
							StartCoroutine(ToyBonnieFunction(false));
							BlackoutActive = false;
							ToyBonnieBlackout.gameObject.SetActive(false);
							break;
					}
				}
			}
		}
	}

	public void SwitchCam(int Camera)
	{
		currentCam = Camera;
		SwitchCameraSound.Play();
		WhiteStripes.Play("WhiteStrip", -1, 0f);
		RoomName.sprite = RoomNames[Camera-1];
		MainCameraBG.gameObject.SetActive(true);
		SignalDisrupted.SetActive(false);

		if (lightsManager.isLightActive)
		{
			MainCameraBG.sprite = FlashlightedCams[Camera-1];
		}
		else
		{
			MainCameraBG.sprite = DefaultCams[Camera-1];
		}
	}

	// Monitor
	public void ActionsMonitorOn()
	{
        if (BBCamera == 15)
        {
            StartCoroutine(BaloonBoyInOffice());
        }
    }

	public void ActionsMonitorOff()
	{
        if (Random.value < 0.000001f)
        {
			RWQ();
        }
        if (BBCamera != 16)
        {
			BaloonBoy.SetActive(false);
        }
        if (WitheredFoxyPrepared)
        {
			FoxyJumpscare();
        }
        if (ManglePrepared == true)
        {
			ManglePrepared = false;
			MangleOffice.SetActive(true);
			StartCoroutine(MangleDeath());
        }
        else if (GoldenFreddyPrepared)
        {
			int randNum = Random.Range(0, 20);

            if (GoldenFreddyAI >= randNum)
            {
                GoldenFreddyCameraTime = 5f;
                GoldenFreddyInOffice = true;
                GoldenFreddyPrepared = false;
                GoldenFreddyOffice.SetActive(true);
            }
        }
        if (Random.value < 0.05f)
        {
			JJ.SetActive(true);
        }
    }
	// ---

	IEnumerator MangleDeath()
	{
		yield return new WaitForSeconds(Random.Range(23f, 60f));
		if (!maskManager.isMaskActive && monitorManager.isMonitorActive)
        {
            JumpscareAnimator.Play("Mangle");
            Jumpscare.Play();
            StartCoroutine(JumpscareSequence());
			MangleOffice.SetActive(false);
        }
        else if (!maskManager.isMaskActive && !monitorManager.isMonitorActive || !maskManager.isMaskActive && state == "OfficeBlackout")
        {
            JumpscareAnimator.Play("Mangle");
            Jumpscare.Play();
            StartCoroutine(JumpscareSequence());
        }
		else if (maskManager.isMaskActive)
        {
			Mask();
            JumpscareAnimator.Play("Mangle");
            Jumpscare.Play();
            StartCoroutine(JumpscareSequence());
			MangleOffice.SetActive(false);
        }
		else
		{
			JumpscareAnimator.Play("Mangle");
            Jumpscare.Play();
            StartCoroutine(JumpscareSequence());
			Debug.Log(state);
			MangleOffice.SetActive(false);
		}
	}

	void FoxyJumpscare()
	{
		if (!maskManager.isMaskActive && monitorManager.isMonitorActive)
        {
            JumpscareAnimator.Play("WitheredFoxy");
            Jumpscare.Play();
            StartCoroutine(JumpscareSequence());
        }
        else if (!maskManager.isMaskActive && !monitorManager.isMonitorActive || !maskManager.isMaskActive && state == "OfficeBlackout")
        {
            JumpscareAnimator.Play("WitheredFoxy");
            Jumpscare.Play();
            StartCoroutine(JumpscareSequence());
        }
		else if (maskManager.isMaskActive)
        {
			Mask();
            JumpscareAnimator.Play("WitheredFoxy");
            Jumpscare.Play();
            StartCoroutine(JumpscareSequence());
        }
		else
		{
			JumpscareAnimator.Play("WitheredFoxy");
            Jumpscare.Play();
            StartCoroutine(JumpscareSequence());
			Debug.Log(state);
		}
	}

	void RWQ()
	{
		RWQOffice.SetActive(true);
		RWQActive = true;
		RWQCrashTimer = 4f;
	}

	void DeactivateRWQ()
	{
		RWQOffice.SetActive(false);
		RWQActive = false;
		RWQCrashTimer = 4f;
	}

	void UpdateBatteryUI() // The UI doesn't works like that, to fix --	 so, alyx, does this has been fixed ? (shiro)
    {
        float threshold = lightsManager.flashlightDuration / 5;
        
        if (lightsManager.currentFlashlightDuration <= 0)
        {
            BatteryImage.sprite = Battery0Bars;
        }
        else if (lightsManager.currentFlashlightDuration <= threshold)
        {
            BatteryImage.sprite = Battery1Bars;
        }
        else if (lightsManager.currentFlashlightDuration <= threshold * 2)
        {
            BatteryImage.sprite = Battery2Bars;
        }
        else if (lightsManager.currentFlashlightDuration <= threshold * 3)
        {
            BatteryImage.sprite = Battery3Bars;
        }
        else if (lightsManager.currentFlashlightDuration <= threshold * 4)
        {
            BatteryImage.sprite = Battery4Bars;
        }
    }
	private IEnumerator PlayStareSound()
	{
		StareSound.Play();
        yield return new WaitForSeconds(6);
        if (StareSound.isPlaying)
        {
            StareSound.Stop();
        }
	}
}