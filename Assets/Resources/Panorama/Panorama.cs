using UnityEngine;

public class Panorama : MonoBehaviour
{
    private Shader _shader;
    protected Material _mat;

    private void Awake()
    {
        _shader = Shader.Find("Custom/Panorama");
        _mat = new Material(_shader);
        Init();
    }

    protected virtual void Init()
    {

    }

    private void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {

    }

    private void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, _mat);
    }
}
