using System.Collections;
using UnityEngine;

public class VersionChecker : MonoBehaviour
{
    // Scripts
    private MenuManager menuManager;

    [System.Serializable]
    public class VersionData
    {
        public string version;
    }

    private void Start()
    {
        menuManager = FindObjectOfType<MenuManager>();

        StartCoroutine(CheckVersion());
    }

    IEnumerator CheckVersion()
    {
        string url = "https://api.sourcemacchiato.com/v1/fnaf2/metadata";

        using (WWW www = new WWW(url))
        {
            yield return www;

            if (string.IsNullOrEmpty(www.error))
            {
                VersionData data = JsonUtility.FromJson<VersionData>(www.text);
                string onlineVersion = data.version;

                TextAsset localVersionAsset = Resources.Load<TextAsset>("Meta/version");
                string localVersion = localVersionAsset.text;

                if (onlineVersion.Trim() == localVersion.Trim())
                {
                    Debug.Log("Same version number");
                }
                else
                {
                    menuManager.AddPopup("mainmenu.update", 0, null);

                    Debug.Log("Different version number");
                }
            }
            else
            {
                Debug.Log("Network error: " + www.error);
            }
        }
    }
}