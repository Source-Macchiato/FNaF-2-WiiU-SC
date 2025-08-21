using UnityEngine;
using WiiU = UnityEngine.WiiU;

[RequireComponent(typeof(AudioSource))]
public class AssignAudio : MonoBehaviour
{
    [Header("TV Only")]
    [SerializeField] private bool tvOnlyTV = false;
    [SerializeField] private bool tvOnlyGamepad = false;

    [Header("TV GamePad Classic")]
    [SerializeField] private bool tvGamepadClassicTV = false;
    [SerializeField] private bool tvGamepadClassicGamepad = false;

    [Header("TV GamePad Alternative")]
    [SerializeField] private bool tvGamepadAltTV = false;
    [SerializeField] private bool tvGamepadAltGamepad = false;

    [Header("GamePad Only")]
    [SerializeField] private bool gamepadOnlyTV = false;
    [SerializeField] private bool gamepadOnlyGamepad = false;

    private AudioSource audioSource;

    private WiiU.AudioOutput output;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        switch (SaveManager.saveData.settings.layoutId)
        {
            case 0:
                output = ComposeOutput(tvOnlyTV, tvOnlyGamepad);

                break;
            case 1:
                output = ComposeOutput(tvGamepadClassicTV, tvGamepadClassicGamepad);

                break;
            case 2:
                output = ComposeOutput(tvGamepadAltTV, tvGamepadAltGamepad);

                break;
            case 3:
                output = ComposeOutput(gamepadOnlyTV, gamepadOnlyGamepad);

                break;
            default:
                output = WiiU.AudioOutput.TV | WiiU.AudioOutput.GamePad;

                break;
        }

        WiiU.AudioSourceOutput.Assign(audioSource, output);
    }

    private WiiU.AudioOutput ComposeOutput(bool toTV, bool toGamepad)
    {
        if (toTV && toGamepad)
        {
            return WiiU.AudioOutput.TV | WiiU.AudioOutput.GamePad;
        }
        else if (toTV)
        {
            return WiiU.AudioOutput.TV;
        }
        else if (toGamepad)
        {
            return WiiU.AudioOutput.GamePad;
        }
        else
        {
            return 0;
        }
    }
}