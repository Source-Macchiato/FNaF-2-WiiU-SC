using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGamesLevelLoader : MonoBehaviour
{
    public static string sceneToLoad;

    void Start()
    {
        if (sceneToLoad != null)
        {
            StartCoroutine(LoadNextScene());
        }
    }

    private IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadSceneAsync(sceneToLoad);
    }

    public static void LoadScene(string scene)
    {
        sceneToLoad = scene;

        SceneManager.LoadSceneAsync("Loading");
    }
}