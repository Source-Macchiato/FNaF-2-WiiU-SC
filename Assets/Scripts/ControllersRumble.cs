using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class ControllersRumble : MonoBehaviour
{
    private const int MAX_PATTERN_CHUNK = 15;

    private bool rumbling = false;
    private int remainingPatternBytes = 0;
    private int chunkFramesLeft = 0;

    WiiU.GamePad gamePad;
    WiiU.Remote remote;

    void Start()
    {
        gamePad = WiiU.GamePad.access;
        remote = WiiU.Remote.Access(0);
    }

    void Update()
    {
        if (rumbling)
        {
            if (chunkFramesLeft > 0)
            {
                chunkFramesLeft--;
            }
            else
            {
                if (remainingPatternBytes > 0)
                {
                    SendNextChunk();
                }
                else
                {
                    rumbling = false;
                }
            }
        }
    }

    public void TriggerRumble(int patternLength, string log = "")
    {
        if (!rumbling)
        {
            if (!string.IsNullOrEmpty(log))
            {
                Debug.Log(log);
            }

            remainingPatternBytes = patternLength;
            chunkFramesLeft = 0;
            rumbling = true;
        }
    }

    private void SendNextChunk()
    {
        int chunkSize = Mathf.Min(remainingPatternBytes, MAX_PATTERN_CHUNK);
        byte[] pattern = new byte[chunkSize];

        for (int i = 0; i < chunkSize; ++i)
        {
            pattern[i] = 0xff;
        }

        gamePad.ControlMotor(pattern, chunkSize * 8);
        remote.PlayRumblePattern(pattern, chunkSize * 8);

        remainingPatternBytes -= chunkSize;
        chunkFramesLeft = chunkSize;
    }
}