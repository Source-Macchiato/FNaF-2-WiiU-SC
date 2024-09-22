using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public bool requireVerification = true;
    public Canvas canvasTV;
    public Canvas canvasGamepad;

    public GameObject loadingScreenTV;
    public GameObject loadingScreenGamepad;

    public Slider sliderTV;
    public Slider sliderGamepad;

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadAsynchronously(sceneName));
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operationLoadLevel = SceneManager.LoadSceneAsync(sceneName);

        if (requireVerification)
        {
            if (canvasTV.isActiveAndEnabled)
            {
                loadingScreenTV.SetActive(true);
            }
            else
            {
                loadingScreenGamepad.SetActive(true);
            }
        }
        else
        {
            sliderGamepad.gameObject.SetActive(true);
        }

        while (!operationLoadLevel.isDone)
        {
            float progress = Mathf.Clamp01(operationLoadLevel.progress / 0.9f);

            if (requireVerification)
            {
                if (canvasTV.isActiveAndEnabled)
                {
                    sliderTV.value = progress;
                }
                else
                {
                    sliderGamepad.value = progress;
                }
            }
            else
            {
                sliderGamepad.value = progress;
            }

            yield return null;
        }
    }
}