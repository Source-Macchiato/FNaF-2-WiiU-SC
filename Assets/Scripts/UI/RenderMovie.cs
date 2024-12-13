﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class RenderMovie : MonoBehaviour
{
    public AudioSource SrcMacchiatoSound;
    public MovieTexture movTexture;
    bool hasStarted = false;

    void Start()
    {
        movTexture.Play();
        SrcMacchiatoSound.Play();
    }

    void Update()
    {
        if (!hasStarted && movTexture.isPlaying)
        {
            hasStarted = true;

        }

        if (hasStarted && !movTexture.isPlaying)
        {
            SceneManager.LoadScene("Warning");
        }
    }
}