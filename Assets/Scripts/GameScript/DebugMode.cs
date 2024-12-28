using UnityEngine;
using UnityEngine.UI;
public class DebugMode : MonoBehaviour {
    public NightPlayer nightPlayer;
    public GameObject DebugObject;
    public bool DebugModeActive;
    public Text ToyFreddy;
    public Text ToyBonnie;
    public Text ToyChica;
    public Text WFreddy;
    public Text WBonnie;
    public Text WChica;
    public Text WFoxy;
    public Text Mangle;
    public Text BaloonBoy;
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