using UnityEngine;
using UnityEngine.UI;
using WiiU = UnityEngine.WiiU;
public class DebugMode : MonoBehaviour {
    public NightPlayer nightPlayer;
    public GameObject DebugObject;
    public bool DebugModeActive;

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

    private void Start() {
        //gamepad shit
        //gamePad = WiiU.GamePad.access;
    }
    private void Update()
    {
        //some wiiu shit with some gamepad shit idk
        //WiiU.GamePadState gamePadState = gamePad.state;
        /**     --UNFINISHED--
        if(gamePadState.gamePadErr == WiiU.GamePadError.None)
        {
            if (gamePadState.IsTriggered(WiiU.GamePadButton.L) && gamePadState.IsTriggered(WiiU.GamePadButton.R))
            {
            DebugObject.SetActive(true);
            SetDebug();
            }
            else{DebugObject.SetActive(false);}
        } **/
        if(DebugModeActive)
        {
            DebugObject.SetActive(true);
            SetDebug();
        }
        else{DebugObject.SetActive(false);}
        
    }

    //SetDebug to start the debug if 
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
        LevelToyFreddy.text = nightPlayer.ToyFreddyAI.ToString();
        LevelToyBonnie.text = nightPlayer.ToyBonnieAI.ToString();
        LevelToyChica.text = nightPlayer.ToyChicaAI.ToString();
        LevelWFreddy.text = nightPlayer.WitheredFreddyAI.ToString();
        LevelWBonnie.text = nightPlayer.WitheredBonnieAI.ToString();
        LevelWChica.text = nightPlayer.WitheredChicaAI.ToString();
        LevelWFoxy.text = nightPlayer.WitheredFoxyAI.ToString();
        LevelMangle.text = nightPlayer.MangleAI.ToString();
        LevelBaloonBoy.text = nightPlayer.BalloonBoyAI.ToString();
        LevelPaperpals.text = nightPlayer.PaperpalsAI.ToString();
        LevelPuppet.text = nightPlayer.PuppetAI.ToString();

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


    }
    
}