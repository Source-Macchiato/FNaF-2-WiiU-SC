using UnityEngine;
using UnityEngine.SceneManagement;
public class AIManager : MonoBehaviour {
    public NightPlayer nightPlayer;
    public int currentTime;
    public int currentNight;

    public float TimeMultiplier;
	public bool isNight7;
	public int PuppetAI;

    //AI list
    	public int ToyFreddyAI;
	public int ToyBonnieAI;
	public int ToyChicaAI;
    	public int WitheredFreddyAI;
	public int WitheredBonnieAI;
	public int WitheredChicaAI;
	public int WitheredFoxyAI; 
    public int GoldenFreddyAI;
    public int MangleAI;
	public int BalloonBoyAI;
	public int PaperpalsAI; 

    private void Start() {
        // Set the AI to 0
        ToyFreddyAI = 0;
        ToyBonnieAI = 0;
        ToyChicaAI = 0;
        WitheredFreddyAI = 0;
        WitheredBonnieAI = 0;
        WitheredChicaAI = 0;
        WitheredFoxyAI = 0;
        GoldenFreddyAI = 0;
        MangleAI = 0;
        BalloonBoyAI = 0;
        PaperpalsAI = 0;
    }
    private void Update() {
        currentTime = nightPlayer.currentTime;
        currentNight = nightPlayer.currentNight;
    }
    public void TimedEvents()
    {
        // Check if it's night 1
        if(currentNight == 0) {
            switch(currentTime)
            {
                case 1:
                ToyBonnieAI = 2;
                ToyChicaAI = 2;
                break;
                case 2:
                ToyFreddyAI = 2;
                ToyBonnieAI = 3;
                //toy chica is still the same
                break;
            }
        }
        if(currentNight == 1) {
            switch(currentTime)
            {
                case 1:
                ToyFreddyAI = 2;
                ToyBonnieAI = 3;
                ToyChicaAI = 3;
                MangleAI = 3;
                BalloonBoyAI = 3;
                WitheredFoxyAI = 3;
                //Golden Freddy ( Random(1000) + 1 ) / 1000
                break;
            }
        }
        if(currentNight == 2) {
            switch(currentTime)
            {
                case 12:
                BalloonBoyAI = 1;
                WitheredBonnieAI = 1;
                WitheredChicaAI = 1;
                WitheredFoxyAI = 2;
                break;
                case 1:
                ToyBonnieAI = 1;
                ToyChicaAI = 1;
                BalloonBoyAI = 2;
                WitheredFreddyAI = 2;
                WitheredBonnieAI = 2;
                WitheredChicaAI = 2;
                WitheredFoxyAI = 3;
                break;
            }
        }
        if(currentNight == 3) {
            switch(currentTime)
            {
                case 12:
                MangleAI = 5;
                BalloonBoyAI = 3;
                WitheredBonnieAI = 1;
                WitheredFoxyAI = 7;
                break;
                case 2:
                ToyBonnieAI = 1;
                MangleAI = 5;
                BalloonBoyAI = 3;
                WitheredFreddyAI = 3;
                WitheredBonnieAI = 4;
                WitheredChicaAI = 4;
                WitheredFoxyAI = 7;
                break;
            }
        }
        if(currentNight == 4) {
            switch(currentTime)
            {
                case 12:
                ToyFreddyAI = 5;
                ToyBonnieAI = 2;
                ToyChicaAI = 2;
                MangleAI = 1;
                BalloonBoyAI = 5;
                WitheredFreddyAI = 2;
                WitheredBonnieAI = 2;
                WitheredChicaAI = 2;
                WitheredFoxyAI = 5;
                break;
                case 1:
                ToyFreddyAI = 1;
                ToyBonnieAI = 2;
                ToyChicaAI = 2;
                MangleAI = 10;
                BalloonBoyAI = 5;
                WitheredFreddyAI = 5;
                WitheredBonnieAI = 5;
                WitheredChicaAI = 5;
                WitheredFoxyAI = 7;
                break;
            }
        }
        if(currentNight == 5) {
            switch(currentTime)
            {
                case 12:
                MangleAI = 3;
                BalloonBoyAI = 5;
                WitheredFreddyAI = 5;
                WitheredBonnieAI = 5;
                WitheredChicaAI = 5;
                WitheredFoxyAI = 10;
                break;
                case 2:
                ToyFreddyAI = 5;
                ToyBonnieAI = 5;
                ToyChicaAI = 5;
                MangleAI = 10;
                BalloonBoyAI = 9;
                WitheredFreddyAI = 10;
                WitheredBonnieAI = 10;
                WitheredChicaAI = 10;
                WitheredFoxyAI = 15;
                break;
            }
            if(currentNight == 6) {
                SceneManager.LoadScene("6AM");
            }
        }
    }
}