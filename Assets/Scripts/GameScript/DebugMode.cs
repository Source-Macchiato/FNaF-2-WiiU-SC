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

    //FPS Counter
    [Header("FPS Counter")]
    public Text fpsText; 
    private float deltaTime;

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
        //time elapse each frames
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        //FPS calc
        float fps = 1.0f / deltaTime;

        //display the FPS
        fpsText.text = Mathf.Ceil(fps).ToString();

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


    }
    
}