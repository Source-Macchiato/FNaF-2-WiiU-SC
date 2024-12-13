using UnityEngine;
using TMPro;

public class SwitcherData : MonoBehaviour
{
    public string switcherId;
    public int currentOptionId;
    public string[] optionsName;

    public TMP_Text text;

    void Update()
    {
        if (text != null)
        {
            if (text.text != optionsName[currentOptionId] || text.text == null)
            {
                text.text = optionsName[currentOptionId];
            }
        }
    }
}