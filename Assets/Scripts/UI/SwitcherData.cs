using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SwitcherData : MonoBehaviour
{
    public string switcherId;
    public int currentOptionId;
    public string[] optionsName;

    private Text textComponent;
    private TMP_Text tmpTextComponent;

    void Start()
    {
        textComponent = transform.Find("Text").GetComponent<Text>();
        tmpTextComponent = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (textComponent != null)
        {
            if (textComponent.text != optionsName[currentOptionId] || textComponent.text == null)
            {
                textComponent.text = optionsName[currentOptionId];
            }
        }

        if (tmpTextComponent != null)
        {
            if (tmpTextComponent.text != optionsName[currentOptionId] || tmpTextComponent.text == null)
            {
                tmpTextComponent.text = optionsName[currentOptionId];
            }
        }
    }
}