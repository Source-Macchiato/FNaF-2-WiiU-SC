using UnityEngine;

public class LayoutManager : MonoBehaviour
{
    private int layoutId;

    public GameObject minimap;
    public GameObject roomName;
    public GameObject subtitles;

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

        // Minimap position
        minimap.transform.localScale = new Vector3(1f, 1f, 1f);
        minimap.transform.localPosition = new Vector3(223.93f, -112.1f, 0f);

        // Room name position
        roomName.GetComponent<RectTransform>().pivot = new Vector2(0f, 0.5f);
        roomName.transform.localScale = new Vector3(1f, 1f, 1f);
        roomName.transform.localPosition = new Vector3(38.64502f, 61.5f, 0f);
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

        // Minimap position
        minimap.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        minimap.transform.localPosition = Vector3.zero;

        // Room name position
        roomName.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        roomName.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        roomName.transform.localPosition = new Vector3(0f, 248f, 0f);
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

        screenSubtitles[0].SetActive(true);
        screenSubtitles[1].SetActive(false);

        // Minimap position
        minimap.transform.localScale = new Vector3(1f, 1f, 1f);
        minimap.transform.localPosition = new Vector3(223.93f, -112.1f, 0f);

        // Room name position
        roomName.GetComponent<RectTransform>().pivot = new Vector2(0f, 0.5f);
        roomName.transform.localScale = new Vector3(1f, 1f, 1f);
        roomName.transform.localPosition = new Vector3(38.64502f, 61.5f, 0f);
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

        // Minimap position
        minimap.transform.localScale = new Vector3(1f, 1f, 1f);
        minimap.transform.localPosition = new Vector3(223.93f, -112.1f, 0f);

        // Room name position
        roomName.transform.localScale = new Vector3(1f, 1f, 1f);
        roomName.transform.localPosition = new Vector3(38.64502f, 61.5f, 0f);
    }
}