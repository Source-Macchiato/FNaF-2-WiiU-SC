using UnityEngine;
using UnityEngine.UI;
using WiiU = UnityEngine.WiiU;

public class MusicBox : MonoBehaviour
{
	public Image progress;

	private float unwindTime;
    private float currentUnwindTime;
    private bool windUpMusicBox = false;

    // References to WiiU controllers
    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    void Start()
	{
        // Access the WiiU GamePad and Remote
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);

        // Assign unwind time
        unwindTime = UnwindSpeed();
        currentUnwindTime = unwindTime;
    }
	
	void Update()
	{
        // Get the current state of the GamePad and Remote
        WiiU.GamePadState gamePadState = gamePad.state;
        WiiU.RemoteState remoteState = remote.state;

        if (gamePadState.gamePadErr == WiiU.GamePadError.None)
        {
            if (gamePadState.IsPressed(WiiU.GamePadButton.B))
            {
                windUpMusicBox = true;
            }
            else
            {
                windUpMusicBox = false;
            }
        }

        if (Application.isEditor)
        {
            if (Input.GetKey(KeyCode.B))
            {
                windUpMusicBox = true;
            }
            else
            {
                windUpMusicBox = false;
            }
        }

        WindUpMusicBox();
        UnwindMusicBox();
        UpdateProgressFill();
    }

    private void WindUpMusicBox()
    {
        if (windUpMusicBox)
        {
            Debug.Log("Is winding up");

            currentUnwindTime += Time.deltaTime * 20f;
            currentUnwindTime = Mathf.Clamp(currentUnwindTime, 0f, unwindTime);
        }
    }

    private void UnwindMusicBox()
    {
        if (!windUpMusicBox)
        {
            Debug.Log("Is unwinding");

            currentUnwindTime -= Time.deltaTime;
            currentUnwindTime = Mathf.Clamp(currentUnwindTime, 0f, unwindTime);
        }
    }

    private void UpdateProgressFill()
    {
        // Calculer le pourcentage de déchargement
        float percentage = currentUnwindTime / unwindTime;

        // Assurez-vous que le fillAmount ne descend pas en dessous de 0.01
        progress.fillAmount = Mathf.Max(percentage, 0.01f);
    }

    private float UnwindSpeed()
    {
        int nightNumber = SaveManager.LoadNightNumber();

        if (nightNumber == 0 || nightNumber == 1) // Night 1 or 2
        {
            return 50f;
        }
        else if (nightNumber == 2) // Night 3
        {
            return 33.33f;
        }
        else if (nightNumber == 3) // Night 4
        {
            return 25f;
        }
        else if (nightNumber == 4) // Night 5
        {
            return 20f;
        }
        else if (nightNumber == 5 || nightNumber == 6) // Night 6 or 7
        {
            return 16.67f;
        }
        else
        {
            return 0f;
        }
    }
}