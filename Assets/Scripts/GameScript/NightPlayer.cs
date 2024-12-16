using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using WiiU = UnityEngine.WiiU;
using TMPro;

public class NightPlayer : MonoBehaviour {

	[Header("Movement")]

    public Transform[] ObjectsToMove;
    public float[] MovingSpeeds = new float[] { -250, -250 };
    public float[] MaxXMovement;
    public float[] MinXMovement;

	[Header("Audio")]
	public AudioSource Jumpscare;
	public AudioSource phoneCall;
	public AudioSource NoFlashlightBatterys;
	public GameObject MuteCallButton;
	private bool CamsActive;
	public AudioSource puppetsMusic;
	public AudioSource jackInTheBox;
	public AudioSource WindUpSound;
	public GameObject BBSounds;
	public AudioSource ErrorSound;
	public AudioSource VentCrawl;
	public AudioSource Mangle;

	[Header("Buttons")]
	public GameObject CameraButton;
	public GameObject MaskButton;
	private bool WindUpBeingHeld;

	[Header("Info")]
	public WinGame WG;
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
    public float TimeMultiplier;
	public bool isNight7;
	public int PuppetAI;
	public int GoldenFreddyAI;
	public int MangleAI;
	public int BBAI;
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
	public string PuppetWarningState;
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
	public Animator MonitorAnimator;
	public GameObject SignalDisrupted;
	public AudioSource MonitorUp;
	public AudioSource MonitorDown;
	public AudioSource OnCams;
	public GameObject CameraUI;
	public GameObject MainCameras;
	public Image MainCameraBG;
	public Image RoomName;
	public Sprite[] RoomNames;
	public Sprite[] FlashlightedCams;
	public Sprite[] DefaultCams;
	public Sprite[] PuppetCameraSprites;
	public Sprite PuppetCameraEndoSprite;
	public Sprite LeftVentEndoSprite;
	private int currentCam = 09;
	public AudioSource SwitchCameraSound;
	public GameObject WindUpButton;
	public Image FullnessCircle;
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

	[Header("Mask")]

	public AudioSource PutOnMask;
	public AudioSource PutDownMask;
	public AudioSource DeepBreathing;
	public Animator MaskAnimator;
	private bool maskActive;

	[Header("Flashlight")]

	public float FlashlightDuration;
	private float currentFlashlightDuration;
	public AudioSource FlashLightAudio;
	private bool flashlightActive;
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
	public Image ToyBonnieBlackout;
	public Image ToyChicaBlackout;
	public GameObject ToyFreddyOffice;
	private float RWQCrashTimer;
	private bool RWQActive;
	public Animator JumpscareAnimator;
	public Animator PuppetWarning;
	public Image MainOfficeImage;
	public Image LeftButtonImage;
	public Image RightButtonImage;
	public Sprite OfficeFlashlightSprite;
	public Sprite OfficeLeftFlashlightSprite;
	public Sprite OfficeRightFlashlightSprite;
	public Sprite LeftButtonLit;
	public Sprite LeftButtonUnLit;
	public Sprite RightButtonLit;
	public Sprite RightButtonUnLit;
	private Sprite MainOfficeDefaultSprite;

	public string state = "Office";
	public string flashLightTarget = "MainHallway";

	private MoveInOffice moveInOffice;

    // References to WiiU controllers
    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    void Start()
	{
        // Access the WiiU GamePad and Remote
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

        moveInOffice = FindObjectOfType<MoveInOffice>();

        currentNight = 3;
        //currentNight = SaveManager.LoadNightNumber() tu es homosexuel

        MainOfficeDefaultSprite = MainOfficeImage.sprite;
		currentFlashlightDuration = FlashlightDuration;
		StartCoroutine(TimeCoroutine());
        CameraUI.SetActive(false); // Disable minimap when game starts

        if (isNight7)
		{
			// Alyx modify this plz
			// ok
			GoldenFreddyAI = DataManager.GetValue<int>("GFAICN", "data:/");
			MangleAI = DataManager.GetValue<int>("MAICN", "data:/");
			BBAI = DataManager.GetValue<int>("BBAICN", "data:/");
			ToyBonnieAI = DataManager.GetValue<int>("TBAICN", "data:/");
			ToyChicaAI = DataManager.GetValue<int>("TCAICN", "data:/");
			ToyFreddyAI = DataManager.GetValue<int>("TFAICN", "data:/");
			WitheredBonnieAI = DataManager.GetValue<int>("WBAICN", "data:/");
			WitheredChicaAI = DataManager.GetValue<int>("WCAICN", "data:/");
			WitheredFreddyAI = DataManager.GetValue<int>("WFAICN", "data:/");
			WitheredFoxyAI = DataManager.GetValue<int>("WFOAICN", "data:/");
		}
		if (Random.value < 0.01) {PaperpalsAI = 1;}
		PuppetAI = Mathf.Clamp(PuppetAI,0, 6);
		WitheredFoxyAI = Mathf.Clamp(WitheredFoxyAI,0, 17);
		SetActivePlushies();
		if (isNight7)
		{
			CheckAndHandleChallenges();
		}

		MovingSpeeds = new float[] { -250, -250	};
    }

	public void SetCurrentAI()
	{
    currentAI = new int[]
    {
        GoldenFreddyAI,
        MangleAI,
        BBAI,
        ToyBonnieAI,
        ToyChicaAI,
        ToyFreddyAI,
        WitheredBonnieAI,
        WitheredChicaAI,
        WitheredFreddyAI,
        WitheredFoxyAI
    };
	}

	public void CheckAndHandleChallenges()
	{
    SetCurrentAI();

    if (IsAIMatching(NewandShinyAI))
    {
        WG.WhatChallengeIsThis = "New And Shiny Completed";
    }
    else if (IsAIMatching(DoubleTroubleAI))
    {
        WG.WhatChallengeIsThis = "Double Trouble Completed";
    }
    else if (IsAIMatching(NightofMisfitsAI))
    {
        WG.WhatChallengeIsThis = "Night of Misfits Completed";
    }
    else if (IsAIMatching(FoxyFoxyAI))
    {
        WG.WhatChallengeIsThis = "Foxy Foxy Completed";
    }
    else if (IsAIMatching(LadiesNightAI))
    {
        WG.WhatChallengeIsThis = "Ladies Night Completed";
    }
    else if (IsAIMatching(FreddysCircusAI))
    {
        WG.WhatChallengeIsThis = "Freddy's Circus Completed";
    }
    else if (IsAIMatching(CupcakeChallengeAI))
    {
        WG.WhatChallengeIsThis = "Cupcake Challenge Completed";
    }
    else if (IsAIMatching(FazbearFeverAI))
    {
        WG.WhatChallengeIsThis = "Fazbear Fever Completed";
    }
    else if (IsAIMatching(GoldenFreddyChallengeAI))
    {
        WG.WhatChallengeIsThis = "Golden Freddy Mode Completed";
    }
    else
    {
        Debug.Log("No matching challenge found for the current AI setup.");
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

	void SetActivePlushies()
    {
        // Assuming Plushies[0] = Toy Bonnie Figure, Plushies[1] = Bonnie Plush, etc.

        if (DataManager.GetValue<bool>("New And Shiny Completed", "data:/"))
        {
            Plushies[0].SetActive(true); // Toy Bonnie action figure
        }
        if (DataManager.GetValue<bool>("Double Trouble Completed", "data:/"))
        {
            Plushies[1].SetActive(true); // Bonnie plush
        }
        if (DataManager.GetValue<bool>("Night of Misfits Completed", "data:/"))
        {
            Plushies[2].SetActive(true); // BB plush
        }
        if (DataManager.GetValue<bool>("Foxy Foxy Completed", "data:/"))
        {
            Plushies[3].SetActive(true); // Foxy plush
        }
        if (DataManager.GetValue<bool>("Ladies Night Completed", "data:/"))
        {
            Plushies[4].SetActive(true); // Chica Plush
        }
        if (DataManager.GetValue<bool>("Freddy's Circus Completed", "data:/"))
        {
            Plushies[5].SetActive(true); // Freddy Plush
        }
        if (DataManager.GetValue<bool>("Cupcake Challenge Completed", "data:/"))
        {
            Plushies[6].SetActive(true); // Chica's Cupcake
        }
        if (DataManager.GetValue<bool>("Fazbear Fever Completed", "data:/"))
        {
            Plushies[7].SetActive(true); // Withered Freddy's microphone
        }
        if (DataManager.GetValue<bool>("Golden Freddy Mode Completed", "data:/"))
        {
            Plushies[8].SetActive(true); // Golden Freddy Plush
        }
    }

    IEnumerator TimeCoroutine()
    {
        for (int i = 0; i <= 6; i++)
        {
            currentTime = i;
            TimedEvents();
            yield return new WaitForSeconds(70f * TimeMultiplier);
        }
    }

    void TimedEvents()
	{
		// When night is finished load 6AM scene
		if (currentTime == 6)
		{
			SceneManager.LoadScene("6AM");
		}

		if (currentTime == 0 && currentNight == 2)
		{
			GoldenFreddyAI = Random.Range(0, 1);
        }
		if (currentTime == 0 && currentNight == 3 || currentNight == 4)
		{
            GoldenFreddyAI = Random.Range(0, 1) * 100;
        }
		if (currentTime == 0 && currentNight == 5)
		{
			GoldenFreddyAI = Random.Range(0, 1) * 10;
		}
		if (currentTime == 1 && currentNight == 2)
		{
			ToyBonnieAI = 1;
			WitheredChicaAI = 4;
			WitheredFreddyAI = 3;
			WitheredFoxyAI = 3;
			ToyChicaAI = 1;
		}
		if (currentTime == 2 && currentNight == 3)
		{
			WitheredChicaAI = 4;
			WitheredFreddyAI = 3;
			WitheredBonnieAI = 4;
			ToyBonnieAI = 1;
		}
		if (currentTime == 1 && currentNight == 4)
		{
			WitheredFoxyAI = 7;
			ToyFreddyAI = 1;
			MangleAI = 10;
			WitheredChicaAI = 10;
			WitheredFreddyAI = 5;
			WitheredBonnieAI = 5;
		}
		if (currentTime == 2 && currentNight == 5)
		{
			ToyBonnieAI = 5;
			GoldenFreddyAI = 3;
			WitheredFoxyAI = 15;
			ToyFreddyAI = 5;
			ToyChicaAI = 5;
			MangleAI = 10;
			BBAI = 9;
			WitheredChicaAI = 10;
			WitheredFreddyAI = 10;
			WitheredBonnieAI = 10;
		}

		if (currentTime == 1 && currentNight == 0)
		{
			PuppetAI = 1;
		}
		if (currentTime == 2 && currentNight == 0)
		{
			ToyBonnieAI = 2;
			ToyChicaAI = 2;
		}
		else if (currentTime == 3 && currentNight == 0)
		{
			ToyBonnieAI = 3;
			ToyChicaAI = 2;
			ToyFreddyAI = 2;
		}

		if (currentTime == 1 && currentNight == 1)
		{
			ToyBonnieAI = 3;
			ToyChicaAI = 3;
			ToyFreddyAI = 2;
			WitheredFoxyAI = 1;
			MangleAI = 3;
			BBAI = 3;
			GoldenFreddyAI = Random.Range(0, 1);
		}
	}

	IEnumerator JumpscareSequence()
	{
		Debug.Log("Death!! State: "+ state);
		state = "Jumpscare";
		yield return new WaitForSeconds(0.45f);
		Jumpscare.Stop();
		SceneManager.LoadScene("GameOver");
	}
    
    void Update()
	{
		InputFunction();
		StateChecks();
		UpdateBatteryUI();
		PuppetBox();
		MovementOpportunityMain();
		MovementOpportunityHandler();
		StartCoroutine(GoldenFreddyFunction());
    }

	IEnumerator GoldenFreddyFunction()
	{
		if (GoldenFreddyInOffice)
		{
			GoldenFreddyMovement -= Time.deltaTime;
			if (GoldenFreddyMovement <= 0f)
			{
				if (state == "Cameras" || state == "MonitorUp")
			{
				StartCoroutine(MonitorDownIE());
				yield return new WaitForSeconds(0.183f);
				JumpscareAnimator.Play("GoldenFreddy");
				Jumpscare.Play();
				StartCoroutine(JumpscareSequence());
			}
			else if (state == "MonitorDown" || state == "Office" || state == "OfficeBlackout")
			{
				JumpscareAnimator.Play("GoldenFreddy");
				Jumpscare.Play();
				StartCoroutine(JumpscareSequence());
			}
			}
		}
	}

	void MovementOpportunityHandler()
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
				StartCoroutine(DisruptCamera(4));
				StartCoroutine(DisruptCamera(2));
				break;
				case 2:
				ToyBonnieCamera = 6;
				StartCoroutine(DisruptCamera(2));
				StartCoroutine(DisruptCamera(6));
				break;
				case 6:
				ToyBonnieCamera = 13;
				StartCoroutine(DisruptCamera(6));
				break;
				case 13:
				StartCoroutine(ToyBonnieFunction(true));
				ToyBonniePrepared = true;
				break;
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
				switch (ToyChicaCamera)
				{
				case 9:
				ToyChicaCamera = 7;
				StartCoroutine(DisruptCamera(9));
				StartCoroutine(DisruptCamera(7));
				break;
				case 7:
				ToyChicaCamera = 4;
				StartCoroutine(DisruptCamera(7));
				break;
				case 4:
				ToyChicaCamera = 14;
				StartCoroutine(DisruptCamera(4));
				break;
				case 14:
				ToyChicaCamera = 1;
				StartCoroutine(DisruptCamera(1));
				break;
				case 1:
				ToyChicaCamera = 5;
				StartCoroutine(DisruptCamera(1));
				StartCoroutine(DisruptCamera(5));
				break;
				case 5:
				ToyChicaCamera = 13;
				StartCoroutine(DisruptCamera(5));
				break;
				case 13:
				StartCoroutine(ToyChicaFunction(true));
				ToyChicaPrepared = true;
				break;
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
				switch (ToyFreddyCamera)
				{
				case 9:
				ToyFreddyCamera = 10;
				StartCoroutine(DisruptCamera(9));
				StartCoroutine(DisruptCamera(10));
				break;
				case 10:
				ToyFreddyCamera = 14;
				StartCoroutine(DisruptCamera(10));
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
			ToyFreddyMovement = 5f;
		}
		if (MangleMovement <= 0f)
		{
			int randNum = Random.Range(0, 20);
			if (MangleAI >= randNum || MangleAI == randNum)
			{
				switch (MangleCamera)
				{
					case 12:
					MangleCamera = 11;
					StartCoroutine(DisruptCamera(12));
					StartCoroutine(DisruptCamera(11));
					break;
					case 11:
					MangleCamera = 10;
					StartCoroutine(DisruptCamera(10));
					StartCoroutine(DisruptCamera(11));
					break;
					case 10:
					MangleCamera = 7;
					StartCoroutine(DisruptCamera(10));
					StartCoroutine(DisruptCamera(7));
					break;
					case 7:
					MangleCamera = 1;
					StartCoroutine(DisruptCamera(1));
					StartCoroutine(DisruptCamera(7));
					break;
					case 1:
					MangleCamera = 6;
					StartCoroutine(DisruptCamera(6));
					StartCoroutine(DisruptCamera(1));
					break;
					case 6:
					MangleCamera = 13;
					StartCoroutine(DisruptCamera(6));
					break;
					case 13:
					if (maskActive != true)
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
			MangleMovement = 5f;
		}
		if (WitheredFreddyMovement <= 0f)
		{
			int randNum = Random.Range(0, 20);
			if (WitheredFreddyAI >= randNum || WitheredFreddyAI == randNum)
			{
				switch (WitheredFreddyCamera)
				{
				case 8:
				WitheredFreddyCamera = 7;
				StartCoroutine(DisruptCamera(8));
				StartCoroutine(DisruptCamera(7));
				break;
				case 7:
				WitheredFreddyCamera = 3;
				StartCoroutine(DisruptCamera(3));
				StartCoroutine(DisruptCamera(7));
				break;
				case 3:
				WitheredFreddyCamera = 14;
				StartCoroutine(DisruptCamera(3));
				break;
				case 14:
				StartCoroutine(PrepareBlackout("WitheredFreddy"));
				WitheredFreddyCamera = 20;
				break;
				}
			}
			WitheredFreddyMovement = 5f;
		}
		if (WitheredBonnieMovement <= 0f)
		{
			int randNum = Random.Range(0, 20);
			if (WitheredBonnieAI >= randNum || WitheredBonnieAI == randNum)
			{
				switch (WitheredBonnieCamera)
				{
				case 8:
				WitheredBonnieCamera = 7;
				StartCoroutine(DisruptCamera(8));
				StartCoroutine(DisruptCamera(7));
				break;
				case 7:
				WitheredBonnieCamera = 14;
				StartCoroutine(DisruptCamera(7));
				break;
				case 14:
				WitheredBonnieCamera = 1;
				StartCoroutine(DisruptCamera(1));
				break;
				case 1:
				WitheredBonnieCamera = 5;
				StartCoroutine(DisruptCamera(1));
				StartCoroutine(DisruptCamera(5));
				break;
				case 5:
				StartCoroutine(PrepareBlackout("WitheredBonnie"));
				WitheredBonnieCamera = 20;
				break;
				}
			}
			WitheredBonnieMovement = 5f;
		}
		if (WitheredChicaMovement <= 0f)
		{
			int randNum = Random.Range(0, 20);
			if (WitheredChicaAI >= randNum || WitheredChicaAI == randNum)
			{
				switch (WitheredChicaCamera)
				{
				case 8:
				WitheredChicaCamera = 4;
				StartCoroutine(DisruptCamera(8));
				StartCoroutine(DisruptCamera(4));
				break;
				case 4:
				WitheredChicaCamera = 2;
				StartCoroutine(DisruptCamera(2));
				StartCoroutine(DisruptCamera(4));
				break;
				case 2:
				WitheredChicaCamera = 6;
				StartCoroutine(DisruptCamera(2));
				StartCoroutine(DisruptCamera(6));
				break;
				case 6:
				StartCoroutine(PrepareBlackout("WitheredChica"));
				WitheredChicaCamera = 20;
				StartCoroutine(DisruptCamera(6));
				break;
				}
			}
			WitheredChicaMovement = 5f;
		}
		if (WitheredFoxyMovement <= 0f)
		{
			int randNum = Random.Range(0, 20);
			if (WitheredFoxyAI >= randNum || WitheredFoxyAI == randNum)
			{
				switch (WitheredFoxyCamera)
				{
				case 8:
				WitheredFoxyCamera = 18;
				StartCoroutine(DisruptCamera(8));
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
			WitheredFoxyMovement = 5f;
		}
		if (BBMovement <= 0f)
		{
			int randNum = Random.Range(0, 20);
			if (BBAI >= randNum || BBAI == randNum)
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
				StartCoroutine(DisruptCamera(5));
				VentCrawl.Play();
				break;
				case 5:
				StartCoroutine(DisruptCamera(5));
				BBCamera = 13;
				break;
				case 13:
				if (maskActive != true)
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
		if (ToyBonnieAI >= 1)
		{
			ToyBonnieMovement -= Time.deltaTime;
		}
		if (ToyChicaAI >= 1)
		{
			ToyChicaMovement -= Time.deltaTime;
		}
		if (ToyFreddyAI >= 1)
		{
			ToyFreddyMovement -= Time.deltaTime;
		}
		if (MangleAI >= 1)
		{
			MangleMovement -= Time.deltaTime;
		}
		if (PaperpalsAI >= 1)
		{
			ToyFreddyMovement -= Time.deltaTime;
		}
		if (WitheredFreddyAI >= 1)
		{
			WitheredFreddyMovement -= Time.deltaTime;
		}
		if (WitheredBonnieAI >= 1)
		{
			WitheredBonnieMovement -= Time.deltaTime;
		}
		if (WitheredChicaAI >= 1)
		{
			WitheredChicaMovement -= Time.deltaTime;
		}
		if (WitheredFoxyAI >= 1)
		{
			WitheredFoxyMovement -= Time.deltaTime;
		}
		if (GoldenFreddyAI >= 1 && state == "Cameras")
		{
			GoldenFreddyCameraTime -= Time.deltaTime;
		}
		if (BBAI >= 1)
		{
			BBMovement -= Time.deltaTime;
		}
	}

	IEnumerator ToyBonnieFunction(bool isMO)
	{
		if (ToyBonnieCamera == 13)
		{
			if (state == "OfficeMask" && BlackoutActive == false)
			{
				ToyBonnieCamera = 9;
				ToyBonnieMovement = 11f;
				ToyBonniePrepared = false;
				StartCoroutine(BlackoutCoroutine("ToyBonnie"));
			}
			else if (state == "Cameras" && isMO)
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
			if (state == "OfficeMask" && BlackoutActive == false)
			{
				ToyChicaPrepared = false;
				ToyChicaCamera = 9;
				ToyChicaMovement = 11f;
				//StartCoroutine(BlackoutCoroutine("ToyChica"));
			}
			else if (state == "Cameras" && isMO)
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
            if (state == "MonitorDown")
            {
                activeBlackout = true;
                StartCoroutine(BlackoutCoroutine());
                if (Animatronic == "ToyFreddy")
				{
					ToyFreddyOffice.SetActive(true);
				}
				else if (Animatronic == "WitheredFreddy")
				{
					WitheredFreddyOffice.SetActive(true);
				}
				else if (Animatronic == "WitheredChica")
				{
					WitheredChicaOffice.SetActive(true);
				}
				else if (Animatronic == "WitheredBonnie")
				{
					WitheredBonnieOffice.SetActive(true);
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
            if (state == "OfficeMask")
            {
                timeForMask -= Time.deltaTime;
            }
            else if (state == "MonitorDown" || state == "Office")
            {
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
            if (state == "Cameras" || state == "MonitorUp")
            {
                StartCoroutine(MonitorDownIE());
                yield return new WaitForSeconds(0.183f);
                JumpscareAnimator.Play(Animatronic);
                Jumpscare.Play();
                StartCoroutine(JumpscareSequence());
            }
            else if (state == "MonitorDown" || state == "Office" || state == "OfficeBlackout")
            {
                JumpscareAnimator.Play(Animatronic);
                Jumpscare.Play();
                StartCoroutine(JumpscareSequence());
            }
			else if (state == "OfficeMask")
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

	flashlightActive = false;
	FlashLightAudio.mute = true;
	NoFlashlightBatterys.mute = false;

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
				if (WindUpBeingHeld && currentCam == 11 && state == "Cameras")
    			{
        			PuppetTime += Time.deltaTime * 2.4f;
        			PuppetDeathTimer = 15f;
    			}
    			else
    			{
        			PuppetTime -= Time.deltaTime * ((float)PuppetAI * 0.16f);
    			}
				Color color = FullnessCircle.color;
        		color.a = 1f;
        		FullnessCircle.color = color;
			}
			else
			{
				puppetsMusic.mute = true;
				Color color = FullnessCircle.color;
        		color.a = 0f;
        		FullnessCircle.color = color;
				if (PuppetDeathTimer >= 0.01f)
				{
					PuppetDeathTimer -= Time.deltaTime;
					PuppetDeathTimer = Mathf.Clamp(PuppetDeathTimer, 0f, 15f);
				}
				else
				{
					StartCoroutine(PuppetDeathSequence());
				}

				if (WindUpBeingHeld)
				{
					PuppetTime += Time.deltaTime * 1.5f;
					PuppetDeathTimer = 15f;
					puppetsMusic.mute = false;
				}
			}

			PuppetTime = Mathf.Clamp(PuppetTime, 0f, 30f);

			// Calculate the sprite index based on currentValue
        	int numberOfSprites = CircleSprites.Length;
        	int spriteIndex = Mathf.FloorToInt((PuppetTime / 30f) * (numberOfSprites - 1));

        	// Clamp the index to ensure it is within bounds
        	spriteIndex = Mathf.Clamp(spriteIndex, 0, numberOfSprites - 1);

        	// Check if there was a sprite change
        	if (spriteIndex != previousFullNessCircleIndex)
        	{
            // Check if the change was positive
            if (spriteIndex > previousFullNessCircleIndex)
            {
                Debug.Log("Sprite index increased");
                if (WindUpBeingHeld)
				{
					WindUpSound.Play();
				}
            }
            else
            {
                Debug.Log("Sprite index decreased");
                // Add additional logic for negative change here
            }

            // Set the displayed sprite
            FullnessCircle.sprite = CircleSprites[spriteIndex];

            // Update the previous sprite index
            previousFullNessCircleIndex = spriteIndex;
        	}

			float percentage = (PuppetTime / 30f) * 100f;

			if (percentage <= 100f && percentage >= 50f)
			{
				PuppetWarning.Play("WarningIdle");
				PuppetWarningState = "WarningIdle";
			}
			if (percentage <= 50f && percentage >= 25f && PuppetWarningState != "WarningLight")
			{
				PuppetWarning.Play("WarningLight");
				PuppetWarningState = "WarningLight";
			}
			if (percentage <= 25f && PuppetWarningState != "WarningHeavy")
			{
				PuppetWarning.Play("WarningHeavy");
				PuppetWarningState = "WarningHeavy";
			}
		}
	}

	IEnumerator PuppetDeathSequence()
	{
		Debug.Log(state);
		if (state != "Jumpscare" && canDeathSeqeuence == true) {
		canDeathSeqeuence = false;
		puppetsMusic.Stop();
		jackInTheBox.Play();
		yield return new WaitForSeconds(5f);
		if (state == "Cameras" || state == "MonitorUp")
		{
			StartCoroutine(MonitorDownIE());
			yield return new WaitForSeconds(0.183f);
			JumpscareAnimator.Play("Puppet");
			Jumpscare.Play();
		}
		else if (state == "MonitorDown" || state == "Office" || state == "OfficeBlackout")
		{
			JumpscareAnimator.Play("Puppet");
			Jumpscare.Play();
		}
		else if (state == "OfficeMask")
		{
			Mask();
			JumpscareAnimator.Play("Puppet");
			Jumpscare.Play();
		}
		state = "Jumpscare";
		StartCoroutine(JumpscareSequence());
	}}

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
		if (flashlightActive)
		{
			currentFlashlightDuration -= Time.deltaTime;
			DeactivateRWQ();
		}
		if (state == "Office" && flashlightActive && currentFlashlightDuration >= 0.01)
		{
			switch (flashLightTarget)
			{
				case "MainHallway":
				if (WitheredFoxyCamera == 14 && WitheredBonnieCamera == 14)
				{
					MainCameraBG.sprite = WitheredBonnieFlashlightCams[14];
				}
				else if (WitheredFoxyCamera == 14)
				{
					MainOfficeImage.sprite = WitheredFoxyFlashlightCams[13];
				}
				else if (WitheredFreddyCamera == 14)
				{
					MainOfficeImage.sprite = WitheredFreddyFlashlightCams[13];
				}
				else if (WitheredBonnieCamera == 14)
				{
					MainOfficeImage.sprite = WitheredBonnieFlashlightCams[13];
					//Debug.Log(WitheredBonnieCamera + WitheredBonnieFlashlightCams[13].name + MainCameraBG.sprite.name);
				}
				else if (ToyFreddyCamera == 14)
				{
					MainOfficeImage.sprite = ToyFreddyFlashlightCams[13];
				}
				else if (ToyFreddyCamera == 15)
				{
					MainOfficeImage.sprite = ToyFreddyFlashlightCams[14];
				}
				else if (ToyChicaCamera == 14)
				{
					MainOfficeImage.sprite = ToyChicaFlashlightCams[13];
				}
				else
				{
					MainOfficeImage.sprite = OfficeFlashlightSprite;
				}
				if (GoldenFreddyAI >= 1)
				{
					if (GoldenFreddyAI >= GoldenFreddyrandNum && GoldenFreddyCameraTime <= 0 && GoldenFreddyPrepared == true)
					{
						GoldenFreddyInOffice = true;
						MainOfficeImage.sprite = GoldenFreddyFlashlightCams[13];
						GoldenFreddyInHall = true;
						GoldenFreddyPrepared = false;
					}
					else
					{
						MainOfficeImage.sprite = OfficeFlashlightSprite;
						Debug.Log(GoldenFreddyrandNum.ToString() + GoldenFreddyAI.ToString() + GoldenFreddyCameraTime.ToString() + GoldenFreddyInOffice.ToString() + GoldenFreddyPrepared.ToString());
					}
				}
				LeftButtonImage.sprite = LeftButtonUnLit;
				RightButtonImage.sprite = RightButtonUnLit;
				break;
				case "LeftButton":
				if (ToyChicaCamera == 13)
				{
					MainOfficeImage.sprite = ToyChicaFlashlightCams[12];
				}
				else if (BBCamera == 13)
				{
					MainOfficeImage.sprite = BBFlashlightCams[12];
				}
				else
				{
					MainOfficeImage.sprite = OfficeLeftFlashlightSprite;
				}
				LeftButtonImage.sprite = LeftButtonLit;
				RightButtonImage.sprite = RightButtonUnLit;
				break;
				case "RightButton":
				if (ToyBonnieCamera == 13)
				{
					MainOfficeImage.sprite = ToyBonnieFlashlightCams[12];
				}
				else if (MangleCamera == 13)
				{
					MainOfficeImage.sprite = MangleFlashlightCams[12];
				}
				else
				{
					MainOfficeImage.sprite = OfficeRightFlashlightSprite;
				}
				LeftButtonImage.sprite = LeftButtonUnLit;
				RightButtonImage.sprite = RightButtonLit;
				break;
			}
		}
		else if (state == "Office" && !flashlightActive)
		{
			MainOfficeImage.sprite = MainOfficeDefaultSprite;
			LeftButtonImage.sprite = LeftButtonUnLit;
			RightButtonImage.sprite = RightButtonUnLit;
		}
		else if (state == "Cameras" && flashlightActive && currentFlashlightDuration >= 0.01 && currentCam == 11)
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
        		MainCameraBG.sprite = PuppetCameraSprites[spriteIndex-1];
    		}
		}
		/*else if (state == "Cameras" && flashlightActive && currentFlashlightDuration >= 0.01 && currentCam == 5)
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
		else if (state == "Cameras" && currentCam == 4)
		{
			if (flashlightActive)
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
					MainCameraBG.sprite = FlashlightedCams[currentCam-1];
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
					MainCameraBG.sprite = DefaultCams[currentCam-1];
				}
			}
		}
		else if (state == "Cameras" && currentCam == 12 && flashlightActive)
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
				MainCameraBG.sprite = FlashlightedCams[currentCam-1];
			}
		}
		else if (state == "Cameras" && currentCam == 10)
		{
			if (!flashlightActive)
			{
				Debug.Log("ToyFreddy Cam : " + ToyFreddyCamera);
				if (ToyFreddyCamera == 10)
				{
					MainCameraBG.sprite = ToyFreddyDefaultCams[9];
				}
				else
				{
					MainCameraBG.sprite = DefaultCams[currentCam-1];
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
					MainCameraBG.sprite = FlashlightedCams[currentCam-1];
				}
			}
		}
		else if (state == "Cameras" && currentCam == 3)
		{
			if (flashlightActive)
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
					MainCameraBG.sprite = FlashlightedCams[currentCam-1];
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
					MainCameraBG.sprite = DefaultCams[currentCam-1];
				}
			}
		}
		else if (state == "Cameras" && currentCam == 2)
		{
			if (flashlightActive)
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
					MainCameraBG.sprite = FlashlightedCams[currentCam-1];
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
					MainCameraBG.sprite = DefaultCams[currentCam-1];
				}
			}
		}
		else if (state == "Cameras" && currentCam == 6)
		{
			if (flashlightActive)
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
					MainCameraBG.sprite = FlashlightedCams[currentCam-1];
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
					MainCameraBG.sprite = DefaultCams[currentCam-1];
				}
			}
		}
		else if (state == "Cameras" && currentCam == 7)
		{
			if (!flashlightActive)
			{
				if (ToyChicaCamera == 7)
				{
					MainCameraBG.sprite = ToyChicaDefaultCams[6];
				}
				else
				{
					MainCameraBG.sprite = DefaultCams[currentCam-1];
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
					MainCameraBG.sprite = FlashlightedCams[currentCam-1];
				}
			}
		}
		else if (state == "Cameras" && currentCam == 1)
		{
			if (!flashlightActive)
			{
				if (ToyChicaCamera == 1)
				{
					MainCameraBG.sprite = ToyChicaDefaultCams[0];
				}
				else
				{
					MainCameraBG.sprite = DefaultCams[currentCam-1];
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
					MainCameraBG.sprite = FlashlightedCams[currentCam-1];
				}
			}
			Debug.Log(flashlightActive + MainCameraBG.sprite.name + "Bonnie Cam: " + WitheredBonnieCamera);
		}
		else if (state == "Cameras" && currentCam == 5)
		{
			if (!flashlightActive)
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
					MainCameraBG.sprite = DefaultCams[currentCam-1];
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
					MainCameraBG.sprite = FlashlightedCams[currentCam-1];
				}
			}
		}
		else if (state == "Cameras" && currentCam == 8 && flashlightActive)
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
				MainCameraBG.sprite = FlashlightedCams[currentCam-1];
			}
			else
			{
				MainCameraBG.sprite = MainCameraBG.sprite = Cam8Sprites[2];
			}
		}
		else if (state == "Cameras" && currentCam == 9)
		{
			if (!flashlightActive)
			{
				if (ToyBonnieCamera == 9 && ToyChicaCamera == 9 && ToyFreddyCamera == 9)
				{
					MainCameraBG.sprite = DefaultCams[currentCam-1];
				}
				else if (ToyBonnieCamera !=9 && ToyChicaCamera == 9 && ToyFreddyCamera == 9) 
				{
					MainCameraBG.sprite = Cam9Sprites[0];
				}
				else if (ToyBonnieCamera !=9 && ToyChicaCamera != 9 && ToyFreddyCamera == 9) 
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
					MainCameraBG.sprite = FlashlightedCams[currentCam-1];
				}
				else if (ToyBonnieCamera !=9 && ToyChicaCamera == 9 && ToyFreddyCamera == 9) 
				{
					MainCameraBG.sprite = Cam9Sprites[1];
				}
				else if (ToyBonnieCamera !=9 && ToyChicaCamera != 9 && ToyFreddyCamera == 9) 
				{
					MainCameraBG.sprite = Cam9Sprites[3];
				}
				else
				{
					MainCameraBG.sprite = Cam9Sprites[4];
				}
			}
		}
		else if (state == "Cameras" && flashlightActive && currentFlashlightDuration >= 0.01)
		{
			MainCameraBG.sprite = FlashlightedCams[currentCam-1];
		}
		else if (state == "Cameras" && !flashlightActive)
		{
			MainCameraBG.sprite = DefaultCams[currentCam-1];
		}

		if (state == "Office")
		{
    		float positionX = ObjectsToMove[0].localPosition.x;

    		if (positionX <= (moveInOffice.leftEdge - 160) && positionX >= (moveInOffice.rightEdge + 160))
    		{
        		flashLightTarget = "MainHallway";
    		}
    		else if (positionX > (moveInOffice.leftEdge - 160))
    		{
        		flashLightTarget = "LeftButton";
    		}
    		else if (positionX < (moveInOffice.rightEdge + 160))
    		{
        		flashLightTarget = "RightButton";
    		}
		}

		ToyBonnieMaskTimer -= Time.deltaTime;
		ToyChicaMaskTimer -= Time.deltaTime;
		BBMaskTimer -= Time.deltaTime;
		MangleMaskTimer -= Time.deltaTime;
		if (ToyBonnieMaskTimer < 0f)
		{
			ToyBonnieMaskTimer = 1f;
			if (maskActive && ToyBonnieCamera == 13 && Random.value < 0.49f)
			{
				StartCoroutine(ToyBonnieFunction(false));
			}
		}
		if (ToyChicaMaskTimer < 0f)
		{
			ToyChicaMaskTimer = 1f;
			if (maskActive && ToyChicaCamera == 13 && Random.value < 0.1f)
			{
				StartCoroutine(ToyChicaFunction(false));
			}
		}
		if (BBMaskTimer < 0f)
		{
			BBMaskTimer = 1f;
			if (maskActive && BBCamera == 13 && Random.value < 0.1f)
			{
				BBCamera = 10;
				BBMovement = 5f;
			}
		}
		if (MangleMaskTimer < 0f)
		{
			MangleMaskTimer = 1f;
			if (maskActive && MangleCamera == 13 && Random.value < 0.1f)
			{
				MangleCamera = 12;
				MangleMovement = 5f;
			}
		}

		MangleInRooms[9].SetActive(false);
		MangleInRooms[10].SetActive(false);
		MangleInRooms[6].SetActive(false);
		MangleInRooms[0].SetActive(false);
		if (currentCam == MangleCamera && MangleCamera != 12 && state == "Cameras")
		{
			Mangle.mute = false;
		}
		else if (currentCam != MangleCamera && MangleCamera != 16 && state == "Cameras")
		{
			Mangle.mute = true;
		}
		else if (MangleCamera != 16 && state == "Office")
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
			MainCameraBG.gameObject.SetActive(false);
			SignalDisrupted.SetActive(true);
			yield return new WaitForSeconds(3f);
			MainCameraBG.gameObject.SetActive(true);
			SignalDisrupted.SetActive(false);
		}
	}

	private IEnumerator BlackoutCoroutine()
    {
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
		ToyChicaBlackout.gameObject.SetActive(false);
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

			if (gamePadState.IsTriggered(WiiU.GamePadButton.L))
			{
				CameraManager();
			}

			if (gamePadState.IsTriggered(WiiU.GamePadButton.R))
			{
				MaskManager();
			}

			// Is pressed
			if (gamePadState.IsPressed(WiiU.GamePadButton.A))
			{
				EnableFlashLight();
			}

			// Is released
			if (gamePadState.IsReleased(WiiU.GamePadButton.A))
			{
				IsGoldenFreddyInHall();
				DisableFlashLight();
			}
		}

		// Handle keyboard inputs
        if (Input.GetKeyDown(KeyCode.A))
        {
            HandleCameraAndFoxyStates();
        }

        if (Input.GetKey(KeyCode.A))
        {
			EnableFlashLight();
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
			IsGoldenFreddyInHall();
			DisableFlashLight();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            CameraManager();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
			MaskManager();
        }
    }

    // Input functions
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
            if (state == "Cameras")
			{
				FlashCam(currentCam);
			}

            if (state == "Office" && flashLightTarget == "MainHallway")
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

	private void EnableFlashLight()
	{
		if (state != "Jumpscare")
		{
            if (currentFlashlightDuration >= 0.01)
            {
                flashlightActive = true;
                FlashLightAudio.mute = false;
            }
        }
	}

	private void DisableFlashLight()
	{
        flashlightActive = false;
        FlashLightAudio.mute = true;
        NoFlashlightBatterys.mute = true;
    }

	private void IsGoldenFreddyInHall()
	{
		if (BBCamera != 16 && flashLightTarget == "MainHallway" && state == "office")
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

	private void MaskManager()
	{
		if (MaskButton.activeSelf && state != "Jumpscare")
		{
            Mask();
        }
	}

	private void CameraManager()
	{
		if (CameraButton.activeSelf && state != "Jumpscare")
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
                Cams();
            }
        }
	}
	// ---

    void FlashCam(int camNumber)
	{
    if (ToyBonnieCamera == camNumber)
    {
        if (!ToyBonnieFlashed) {StartCoroutine(FlashAnimatronic("ToyBonnie"));}
    }
    if (ToyChicaCamera == camNumber)
    {
        if (!ToyChicaFlashed) {StartCoroutine(FlashAnimatronic("ToyChica"));}
    }
    if (ToyFreddyCamera == camNumber)
    {
        if (!ToyFreddyFlashed) {StartCoroutine(FlashAnimatronic("ToyFreddy"));}
    }
    if (MangleCamera == camNumber)
    {
        if (!MangleFlashed) {StartCoroutine(FlashAnimatronic("Mangle"));}
    }
    if (PaperpalsCamera == camNumber)
    {
        if (!PaperpalsFlashed) {StartCoroutine(FlashAnimatronic("Paperpals"));}
    }
    if (WitheredFreddyCamera == camNumber)
    {
        if (!WitheredFreddyFlashed) {StartCoroutine(FlashAnimatronic("WitheredFreddy"));}
    }
    if (WitheredBonnieCamera == camNumber)
    {
        if (!WitheredFreddyFlashed) {StartCoroutine(FlashAnimatronic("WitheredBonnie"));}
    }
    if (WitheredChicaCamera == camNumber)
    {
        if (!WitheredFreddyFlashed) {StartCoroutine(FlashAnimatronic("WitheredChica"));}
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
		if (state != "Jumpscare" && state != "Cameras" && state != "MonitorUp")
		{
		maskActive = !maskActive;
		MaskAnimator.gameObject.SetActive(true);
		if (GoldenFreddyInOffice)
		{
			GoldenFreddyOffice.SetActive(false);
			GoldenFreddyCameraTime = 5f;
			GoldenFreddyMovement = 1.67f;
			GoldenFreddyInOffice = false;
		}
		if (maskActive)
		{
			MaskAnimator.Play("MaskDown");
			PutOnMask.Play();
			state = "OfficeMask";
			StartCoroutine(HeavyBreathing());
			CameraButton.SetActive(false);
		}
		else
		{
			MaskAnimator.Play("MaskUp");
			PutDownMask.Play();
			state = "Office";
			DeepBreathing.mute = true;
			CameraButton.SetActive(true);
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

	public void Cams()
	{
		if (state != "OfficeMask" && maskActive == false)
		{
			Debug.Log("Cameras!!! State: "+state);
		CamsActive = !CamsActive;
		MonitorAnimator.gameObject.SetActive(true);
		if (CamsActive)
		{
			state = "MonitorUp";
			MaskButton.SetActive(false);
			CameraButton.SetActive(false);
			MonitorUp.Play();
			MonitorAnimator.Play("MonitorUp");
			StartCoroutine(MonitorUpIE());
		}
		else
		{
			state = "MonitorDown";
			MaskButton.SetActive(true);
			CameraButton.SetActive(false);
			MonitorDown.Play();
			StartCoroutine(MonitorDownIE());
		}
		}
	}

	public void SwitchCam(int Camera)
	{
		MainCameras.SetActive(false);
		MainCameras.SetActive(true);
		currentCam = Camera;
		SwitchCameraSound.Play();
		puppetsMusic.volume = 0f;
		WindUpButton.SetActive(false);
		RoomName.sprite = RoomNames[Camera-1];
		MainCameraBG.gameObject.SetActive(true);
		SignalDisrupted.SetActive(false);
		switch (Camera)
		{
			case 11:
			puppetsMusic.volume = 1f;
			WindUpButton.SetActive(true);
			break;
			case 12:
			case 10:
			case 09:
			puppetsMusic.volume = 0.5f;
			break;
		}
		if (flashlightActive)
		{
			MainCameraBG.sprite = FlashlightedCams[Camera-1];
		}
		else
		{
			MainCameraBG.sprite = DefaultCams[Camera-1];
		}
	}

	IEnumerator MonitorUpIE()
	{
		yield return new WaitForSeconds(0.276f);
		JJ.SetActive(false);
		puppetsMusic.mute = false;
		CameraButton.SetActive(true);
		CameraUI.SetActive(true);
		MainCameras.SetActive(true);
		state = "Cameras";
		MonitorAnimator.gameObject.SetActive(false);
		OnCams.mute = false;
		SwitchCameraSound.Play();
		if (BBCamera == 15)
		{
			StartCoroutine(BaloonBoyInOffice());
		}
	}

	IEnumerator MonitorDownIE()
	{
		MonitorAnimator.gameObject.SetActive(true);
		OnCams.mute = true;
		puppetsMusic.mute = true;
		CameraUI.SetActive(false);
		MainCameras.SetActive(false);
		MonitorAnimator.Play("MonitorDown");
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
			StartCoroutine(FoxyJumpscare());
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
		yield return new WaitForSeconds(0.183f);
		CameraButton.SetActive(true);
		if (state != "OfficeMask")
		{
			Debug.Log(state);
			state = "Office";
		}
		MonitorAnimator.gameObject.SetActive(false);
	}

	IEnumerator MangleDeath()
	{
		yield return new WaitForSeconds(Random.Range(23f, 60f));
		if (state == "Cameras" || state == "MonitorUp")
        {
            StartCoroutine(MonitorDownIE());
            yield return new WaitForSeconds(0.183f);
            JumpscareAnimator.Play("Mangle");
            Jumpscare.Play();
            StartCoroutine(JumpscareSequence());
			MangleOffice.SetActive(false);
        }
        else if (state == "MonitorDown" || state == "Office" || state == "OfficeBlackout")
        {
            JumpscareAnimator.Play("Mangle");
            Jumpscare.Play();
            StartCoroutine(JumpscareSequence());
        }
		else if (state == "OfficeMask")
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

	IEnumerator FoxyJumpscare()
	{
		if (state == "Cameras" || state == "MonitorUp")
            {
                StartCoroutine(MonitorDownIE());
                yield return new WaitForSeconds(0.183f);
                JumpscareAnimator.Play("WitheredFoxy");
                Jumpscare.Play();
                StartCoroutine(JumpscareSequence());
            }
            else if (state == "MonitorDown" || state == "Office" || state == "OfficeBlackout")
            {
                JumpscareAnimator.Play("WitheredFoxy");
                Jumpscare.Play();
                StartCoroutine(JumpscareSequence());
            }
			else if (state == "OfficeMask")
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

	IEnumerator HeavyBreathing()
	{
		yield return new WaitForSeconds(0.367f);
		if (maskActive) {DeepBreathing.mute = false;}
	}

	public void MuteCall()
	{
		phoneCall.mute = true;
		MuteCallButton.SetActive(false);
	}

	void UpdateBatteryUI()
    {
        float threshold = FlashlightDuration / 5;
        
        if (currentFlashlightDuration <= 0)
        {
            BatteryImage.sprite = Battery0Bars;
        }
        else if (currentFlashlightDuration <= threshold)
        {
            BatteryImage.sprite = Battery1Bars;
        }
        else if (currentFlashlightDuration <= threshold * 2)
        {
            BatteryImage.sprite = Battery2Bars;
        }
        else if (currentFlashlightDuration <= threshold * 3)
        {
            BatteryImage.sprite = Battery3Bars;
        }
        else if (currentFlashlightDuration <= threshold * 4)
        {
            BatteryImage.sprite = Battery4Bars;
        }
    }
}
