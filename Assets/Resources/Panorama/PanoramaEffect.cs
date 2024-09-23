using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[ExecuteInEditMode]
public class PanoramaEffect : Panorama
{
    [SerializeField]
    private bool _Zoom = true;
    private int zoomValue;
    [SerializeField]
    private float _ZoomMultiplier = 0.5f;
    [SerializeField]
    private bool _pDir;
    private int pdirValue;
    [SerializeField]
    private bool _noWrap;
    private int noWrapValue;

    protected override void OnUpdate()
    {
        _mat.SetVector("_Variables", new Vector4(pdirValue,zoomValue,noWrapValue));
        if (_pDir)
        {
            _mat.SetVector("_PixelSize", new Vector4(_ZoomMultiplier, 0));
        }
        else
        {
            _mat.SetVector("_PixelSize", new Vector4(0, _ZoomMultiplier));
        }
        pdirValue = Convert.ToInt32(_pDir);
        noWrapValue = Convert.ToInt32(_noWrap);
        zoomValue = Convert.ToInt32(_Zoom);

    }
}
