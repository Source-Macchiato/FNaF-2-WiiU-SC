using UnityEngine;
using UnityEngine.UI;
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
    private void Update()
    {
        if(DebugModeActive)
        {
            DebugObject.SetActive(true);
            SetDebug();
        }
        else {DebugObject.SetActive(false);}
    }
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