using UnityEngine;
using UnityEngine.SceneManagement;

public class RenderMovie : MonoBehaviour
{
    public AudioSource cafeSound;
    public MovieTexture movTexture;
    bool hasStarted = false;

    void Start()
    {
        movTexture.Play();
        cafeSound.Play();
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