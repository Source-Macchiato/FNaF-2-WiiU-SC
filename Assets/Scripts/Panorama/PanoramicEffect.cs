using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PanoramicEffect : MonoBehaviour
{
    [Header("Shader Material")]
    public Material panoramicMaterial;

    [Header("Effect Controls")]
    public bool enableEffect = true;

    [Range(0f, 1f)]
    public float curveAmount = 0.35f;

    void Start()
    {
        enableEffect = SaveManager.saveData.settings.panoramaEffect;
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (enableEffect && panoramicMaterial != null)
        {
            panoramicMaterial.SetFloat("_CurveAmount", curveAmount);
            Graphics.Blit(src, dest, panoramicMaterial);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}