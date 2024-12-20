using System.Collections;
using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class LoadDubbing : MonoBehaviour
{
    public AudioSource phoneCallAudio;
    private string bundleName;
    private string audioName;
    private float nightNumber;
    private string dubbingLanguage;

	void Start()
    {
        WiiU.AudioSourceOutput.Assign(phoneCallAudio, WiiU.AudioOutput.TV | WiiU.AudioOutput.GamePad);
        nightNumber = SaveManager.LoadNightNumber();

        if (nightNumber >= 0 && nightNumber <= 5)
        {
            // Get dubbing language
            dubbingLanguage = SaveManager.LoadDubbingLanguage();

            // Assign bundleName and audioName variables
            if (dubbingLanguage == null || dubbingLanguage == "en")
            {
                bundleName = "vo-language-pack";

                audioName = "VO-Call" + (nightNumber + 1);
            }
            else
            {
                bundleName = dubbingLanguage + "-language-pack";

                audioName = dubbingLanguage.ToUpper() + "-Call" + (nightNumber + 1);
            }

            // Play the dubbing
            StartCoroutine(PlayAudio(bundleName, audioName));
        } 
    }

    private IEnumerator PlayAudio(string assetBundleName, string objectNameToLoad)
    {
        // Check if AssetBundle is already in memory
        AssetBundle assetBundle = null;
        foreach (var loadedBundle in AssetBundle.GetAllLoadedAssetBundles())
        {
            if (loadedBundle.name == assetBundleName)
            {
                assetBundle = loadedBundle;
                break;
            }
        }

        // Load AssetBundle if not in memory
        if (assetBundle == null)
        {
            string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "AssetBundles");
            filePath = System.IO.Path.Combine(filePath, assetBundleName);
            var assetBundleCreateRequest = AssetBundle.LoadFromFileAsync(filePath);
            yield return assetBundleCreateRequest;

            assetBundle = assetBundleCreateRequest.assetBundle;
            if (assetBundle == null)
            {
                Debug.LogError("Failed to load AssetBundle: " + assetBundleName);
                yield break;
            }
        }

        // Load AudioClip from AssetBundle
        AssetBundleRequest asset = assetBundle.LoadAssetAsync<AudioClip>(objectNameToLoad);
        yield return asset;

        // Assign AudioClip to AudioSource and play audio
        AudioClip loadedAsset = asset.asset as AudioClip;
        if (loadedAsset != null)
        {
            phoneCallAudio.clip = loadedAsset;
            phoneCallAudio.Play();
        }
        else
        {
            Debug.LogError("Failed to load AudioClip: " + objectNameToLoad);
        }
    }
}