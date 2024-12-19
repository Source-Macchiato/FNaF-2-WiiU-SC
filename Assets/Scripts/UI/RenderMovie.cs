﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RenderMovie : MonoBehaviour
{
    public AudioSource cafeSound;
    public MovieTexture movTexture;

    void Start()
    {
        movTexture.Play();
        cafeSound.Play();

        StartCoroutine(LoadNextScene());
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(cafeSound.clip.length);

        SceneManager.LoadScene("Warning");
    }
}