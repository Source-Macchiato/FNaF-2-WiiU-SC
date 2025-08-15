using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RenderMovie : MonoBehaviour
{
    public AudioSource cafeSound;
    public MovieTexture movTexture;

    void Start()
    {
        StartCoroutine(Sequence());
    }

    private IEnumerator Sequence()
    {
        movTexture.Play();
        cafeSound.Play();

        yield return new WaitForSeconds(8f);

        SceneManager.LoadSceneAsync("Warning");
    }
}