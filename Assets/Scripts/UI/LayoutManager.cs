using UnityEngine;

public class LayoutManager : MonoBehaviour
{
    private int layoutId;

    public GameObject minimap;
    public GameObject[] subtitles;

    [Header("Screens")]
    public GameObject[] screenOffice;
    public GameObject[] screenMonitor;
    public GameObject[] screenUI;
    public GameObject[] screenMinimap;
    public GameObject[] screenSubtitles;

    void Start()
    {
        layoutId = SaveManager.LoadLayoutId();

        if (layoutId == 0)
        {
            TVOnly();
        }
        else if (layoutId == 1)
        {
            TVGamepadClassic();
        }
        else if (layoutId == 2)
        {
            TVGamepadAlternative();
        }
        else if (layoutId == 3)
        {
            GamepadOnly();
        }
    }

    void Update()
    {
        if (layoutId == 0 || layoutId == 2)
        {
            //ChangeSubtitlePosition(movement.camIsUp);
        }
    }

    private void TVOnly()
    {
        screenOffice[0].SetActive(true);
        screenOffice[1].SetActive(false);

        screenMonitor[0].SetActive(true);
        screenMonitor[1].SetActive(false);

        screenUI[0].SetActive(true);
        screenUI[1].SetActive(false);

        screenMinimap[0].SetActive(true);
        screenMinimap[1].SetActive(false);

        screenSubtitles[0].SetActive(true);
        screenSubtitles[1].SetActive(false);

        //minimap.transform.localScale = new Vector3(1f, 1f, 1f);
        //minimap.transform.localPosition = new Vector3(407.7f, -152.4f, 0);
    }

    private void TVGamepadClassic()
    {
        screenOffice[0].SetActive(true);
        screenOffice[1].SetActive(false);

        screenMonitor[0].SetActive(true);
        screenMonitor[1].SetActive(false);

        screenUI[0].SetActive(true);
        screenUI[1].SetActive(false);

        screenMinimap[0].SetActive(false);
        screenMinimap[1].SetActive(true);

        screenSubtitles[0].SetActive(false);
        screenSubtitles[1].SetActive(true);

        //minimap.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        //minimap.transform.localPosition = Vector3.zero;

        //subtitles[0].SetActive(true);
        //subtitles[1].SetActive(false);
        //subtitles[2].SetActive(false);
    }

    private void TVGamepadAlternative()
    {
        screenOffice[0].SetActive(true);
        screenOffice[1].SetActive(false);

        screenMonitor[0].SetActive(false);
        screenMonitor[1].SetActive(true);

        screenUI[0].SetActive(true);
        screenUI[1].SetActive(false);

        screenMinimap[0].SetActive(false);
        screenMinimap[1].SetActive(true);
    }

    private void GamepadOnly()
    {
        screenOffice[0].SetActive(false);
        screenOffice[1].SetActive(true);

        screenMonitor[0].SetActive(false);
        screenMonitor[1].SetActive(true);

        screenUI[0].SetActive(false);
        screenUI[1].SetActive(true);

        screenMinimap[0].SetActive(false);
        screenMinimap[1].SetActive(true);

        screenSubtitles[0].SetActive(false);
        screenSubtitles[1].SetActive(true);

        //minimap.transform.localScale = new Vector3(1f, 1f, 1f);
        //minimap.transform.localPosition = new Vector3(407.7f, -152.4f, 0);
    }

    private void ChangeSubtitlePosition(bool cameraStatus)
    {
        if (cameraStatus)
        {
            subtitles[0].SetActive(false);
            subtitles[1].SetActive(false);
            subtitles[2].SetActive(true);
        }
        else
        {
            subtitles[0].SetActive(false);
            subtitles[1].SetActive(true);
            subtitles[2].SetActive(false);
        }
    }
}