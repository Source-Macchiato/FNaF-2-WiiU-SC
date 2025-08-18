using UnityEngine;

public class LayoutManager : MonoBehaviour
{
    private int layoutId;

    public GameObject minimap;
    public GameObject roomName;
    public GameObject musicBox;

    [Header("Screens")]
    public GameObject[] screenOffice;
    public GameObject[] screenMonitor;
    public GameObject[] screenUI;
    public GameObject[] screenMinimap;
    public GameObject[] screenSubtitles;
    public GameObject[] screenPointer;

    void Start()
    {
        switch (SaveManager.saveData.settings.layoutId)
        {
            case 0:
                TVOnly();
                break;
            case 1:
                TVGamepadClassic();
                break;
            case 2:
                TVGamepadAlternative();
                break;
            case 3:
                GamepadOnly();
                break;
            default:
                TVGamepadClassic();
                break;
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

        screenPointer[0].SetActive(true);
        screenPointer[1].SetActive(false);

        // Minimap position
        minimap.transform.localScale = new Vector3(1f, 1f, 1f);
        minimap.transform.localPosition = new Vector3(223.93f, -112.1f, 0f);

        // Room name position
        roomName.GetComponent<RectTransform>().pivot = new Vector2(0f, 0.5f);
        roomName.transform.localScale = new Vector3(1f, 1f, 1f);
        roomName.transform.localPosition = new Vector3(38.64502f, 61.5f, 0f);

        // Music box position
        musicBox.transform.localPosition = new Vector3(-102.3f, -213.7f, 0);
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

        screenPointer[0].SetActive(true);
        screenPointer[1].SetActive(false);

        // Minimap position
        minimap.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        minimap.transform.localPosition = Vector3.zero;

        // Room name position
        roomName.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        roomName.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        roomName.transform.localPosition = new Vector3(0f, 248f, 0f);

        // Music box position
        musicBox.transform.localPosition = new Vector3(350f, -200f, 0);
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

        screenPointer[0].SetActive(true);
        screenPointer[1].SetActive(false);

        // Minimap position
        minimap.transform.localScale = new Vector3(1f, 1f, 1f);
        minimap.transform.localPosition = new Vector3(223.93f, -112.1f, 0f);

        // Room name position
        roomName.GetComponent<RectTransform>().pivot = new Vector2(0f, 0.5f);
        roomName.transform.localScale = new Vector3(1f, 1f, 1f);
        roomName.transform.localPosition = new Vector3(38.64502f, 61.5f, 0f);

        // Music box position
        musicBox.transform.localPosition = new Vector3(-102.3f, -213.7f, 0);
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

        screenPointer[0].SetActive(false);
        screenPointer[1].SetActive(true);

        // Minimap position
        minimap.transform.localScale = new Vector3(1f, 1f, 1f);
        minimap.transform.localPosition = new Vector3(223.93f, -112.1f, 0f);

        // Room name position
        roomName.transform.localScale = new Vector3(1f, 1f, 1f);
        roomName.transform.localPosition = new Vector3(38.64502f, 61.5f, 0f);

        // Music box position
        musicBox.transform.localPosition = new Vector3(-102.3f, -213.7f, 0);
    }
}