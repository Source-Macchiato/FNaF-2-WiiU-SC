using UnityEngine;
using RTLTMPro;

public class SwitcherData : MonoBehaviour
{
    public string switcherId;
    public int currentOptionId;
    public string[] optionsName;

    public RTLTextMeshPro text;

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