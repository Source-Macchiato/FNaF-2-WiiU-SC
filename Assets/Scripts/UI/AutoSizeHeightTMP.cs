using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class AutoSizeHeightTMP : MonoBehaviour
{
    TextMeshProUGUI tmp;
    RectTransform rt;
    string lastText;
    float lastWidth;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        rt = (RectTransform)transform;
        lastText = tmp.text;
        lastWidth = rt.rect.width;
        UpdateHeight();
    }

    void LateUpdate()
    {
        float width = rt.rect.width;
        string text = tmp.text;
        if (text != lastText || !Mathf.Approximately(width, lastWidth))
        {
            UpdateHeight();
            lastText = text;
            lastWidth = width;
        }
    }

    void UpdateHeight()
    {
        float width = rt.rect.width;
        if (width <= 0f) return;
        float h = tmp.GetPreferredValues(width, 0).y;
        Vector2 sd = rt.sizeDelta;
        sd.y = h;
        rt.sizeDelta = sd;
    }
}
