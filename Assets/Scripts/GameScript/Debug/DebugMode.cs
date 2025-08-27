using UnityEngine;
using UnityEngine.UI;
using WiiU = UnityEngine.WiiU;

public class DebugMode : MonoBehaviour
{
    public NightPlayer nightPlayer;
    public LightsManager lightsManager;
    public GameObject DebugObject;
    public bool debugModeActive = false;

    //Text of the cam number of each animatronics
    [Header("Animatronics Cams Info")]
    public Text ToyFreddy;
    public Text ToyBonnie;
    public Text ToyChica;
    public Text WFreddy;
    public Text WBonnie;
    public Text WChica;
    public Text WFoxy;
    public Text Mangle;
    public Text BaloonBoy;
    [Header("AI Level")]
    public Text LevelToyFreddy;
    public Text LevelToyBonnie;
    public Text LevelToyChica;
    public Text LevelWFreddy;
    public Text LevelWBonnie;
    public Text LevelWChica;
    public Text LevelWFoxy;
    public Text LevelMangle;
    public Text LevelBaloonBoy;
    public Text LevelPaperpals;
    public Text LevelPuppet;

    [Header("State")]
    public Text StateText;

    [Header("MusicBox")]
    public Text MusicBox;
    public Text PuppetTimerDeath;

    //FPS Counter
    [Header("FPS Counter")]
    public Text fpsText; 
    private float deltaTime;

    [Header("Performance Info")]
    public Text memoryUsageText;
    public Text activeGameObjectsText;
    public Text BlackOut;

    // References to WiiU controllers
    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    private void Start()
    {
        // Access the WiiU GamePad and Remote
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

        ToggleDebugMode(false);
    }
    private void Update()
    {
        // Get the current state of the GamePad and Remote
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        if (gamePadState.gamePadErr == WiiU.GamePadError.None)
        {
            if (gamePadState.IsTriggered(WiiU.GamePadButton.L) && gamePadState.IsTriggered(WiiU.GamePadButton.R))
            {
                ToggleDebugMode(!debugModeActive);
            }
        }

        switch (remoteState.devType)
        {
            case WiiU.RemoteDevType.ProController:
                if (remoteState.pro.IsTriggered(WiiU.ProControllerButton.L) && remoteState.pro.IsTriggered(WiiU.ProControllerButton.R))
                {
                    ToggleDebugMode(!debugModeActive);
                }
                break;
            case WiiU.RemoteDevType.Classic:
                if (remoteState.classic.IsTriggered(WiiU.ClassicButton.L) && remoteState.classic.IsTriggered(WiiU.ClassicButton.R))
                {
                    ToggleDebugMode(!debugModeActive);
                }
                break;
            default:
                break;
        }

        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                ToggleDebugMode(!debugModeActive);
            }
        }

        // Call SetDebug every frames if toggle mode is active
        if (debugModeActive)
        {
            SetDebug();
        }
    }

    // SetDebug to start the debug if 
    void SetDebug()
    {
        // set Animatronics Cams Info on each text
        ToyFreddy.text = nightPlayer.ToyFreddyCamera.ToString();
        ToyBonnie.text = nightPlayer.ToyBonnieCamera.ToString();
        ToyChica.text = nightPlayer.ToyChicaCamera.ToString();
        WFreddy.text = nightPlayer.WitheredFreddyCamera.ToString();
        WBonnie.text = nightPlayer.WitheredBonnieCamera.ToString();
        WChica.text = nightPlayer.WitheredChicaCamera.ToString();
        WFoxy.text = nightPlayer.WitheredFoxyCamera.ToString();
        Mangle.text = nightPlayer.MangleCamera.ToString();
        BaloonBoy.text = nightPlayer.BBCamera.ToString();

        //AI level
        LevelToyFreddy.text = NightPlayer.toyFreddyDifficulty.ToString();
        LevelToyBonnie.text = NightPlayer.toyBonnieDifficulty.ToString();
        LevelToyChica.text = NightPlayer.toyChicaDifficulty.ToString();
        LevelWFreddy.text = NightPlayer.witheredFreddyDifficulty.ToString();
        LevelWBonnie.text = NightPlayer.witheredBonnieDifficulty.ToString();
        LevelWChica.text = NightPlayer.witheredChicaDifficulty.ToString();
        LevelWFoxy.text = NightPlayer.witheredFoxyDifficulty.ToString();
        LevelMangle.text = NightPlayer.mangleDifficulty.ToString();
        LevelBaloonBoy.text = NightPlayer.bbDifficulty.ToString();
        LevelPaperpals.text = NightPlayer.paperpalsDifficulty.ToString();
        LevelPuppet.text = NightPlayer.puppetDifficulty.ToString();

        //State of the player
        StateText.text = lightsManager.currentFlashlightDuration.ToString();

        //MusicBox Timer
        MusicBox.text = nightPlayer.PuppetTime.ToString();
        //Puppet Deatth Timer
        PuppetTimerDeath.text = nightPlayer.PuppetDeathTimer.ToString();

        //time elapse each frames
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        //FPS calc
        float fps = 1.0f / deltaTime;

        //display the FPS
        fpsText.text = Mathf.Ceil(fps).ToString();

        //Memory Usage
        long memoryUsage = System.GC.GetTotalMemory(false) / (1024 * 1024);
        memoryUsageText.text = ""+memoryUsage.ToString()+"Mb";

        //ActiveGameObject
        int activeObjects = FindObjectsOfType<GameObject>().Length;
        activeGameObjectsText.text = activeObjects.ToString();

        //blackout
        BlackOut.text = nightPlayer.currentBlackout.ToString();


    }

    void ToggleDebugMode(bool condition)
    {
        debugModeActive = condition;

        DebugObject.SetActive(debugModeActive);
    }
}