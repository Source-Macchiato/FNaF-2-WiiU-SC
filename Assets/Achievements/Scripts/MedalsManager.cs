using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MedalsManager : MonoBehaviour {

	public static MedalsManager medalsManager;
    public static Achievements achievements;

    private bool isProcessingQueue = false;

    [Header("BrewConnect")]
    [SerializeField] private string organizationToken;
    [SerializeField] private string projectToken;

    [Header("Popup settings")]
    public float durationPopup;
    public Image medalPopup;
    public Text titleText;
    public Text descText;
    public Text achievementObtained;
    public Image icon;

    public Animator medalAnim;

    public bool isShowing = false;

    public List<Achievements.achievements> unlockedAchievements = new List<Achievements.achievements>();
    [Header("Things in wait list")]
    private List<AchievementDisplayData> achievementsToDisplay = new List<AchievementDisplayData>();

    [Header("Dark Mode")]
    public bool darkMode = false;
    public Color normalText;
    public Color darkText;
    public Color normalPopup;
    public Color darkPopup;

    void Awake()
	{
        if (medalsManager != null)
        {
            Destroy(this.gameObject);
            return;
        }
		medalsManager = this;
        DontDestroyOnLoad(this);
	}

	void Start()
    {
		medalAnim.SetBool("show", false);

        if (darkMode)
        {
            medalPopup.color = darkPopup;
            titleText.color = darkText;
            descText.color = darkText;
            achievementObtained.color = darkText;
        }
        else
        {
            medalPopup.color = normalPopup;
            titleText.color = normalText;
            descText.color = normalText;
            achievementObtained.color = normalText;
        }

        for (int i = 0; i < SaveManager.saveData.achievements.Length; i++)
        {
            if (SaveManager.saveData.achievements[i])
            {
                StartCoroutine(PublishAchievementIE((Achievements.achievements)i));
            }
        }
    }

    public void UnlockAchievement(Achievements.achievements key)
    {
        SaveManager.saveData.UnlockAchievement(key);
        SaveManager.Save();

        StartCoroutine(PublishAchievementIE(key));
    }

    private IEnumerator ShowAchievementIE(string title, string description, Sprite iconSprite = null)
    {
        isShowing = true;

        titleText.text = title;
        descText.text = description;
        
        if (iconSprite != null)
        {
            icon.sprite = iconSprite;
        }

        medalAnim.SetBool("show", true);

        yield return new WaitForSeconds(durationPopup);

        medalAnim.SetBool("show", false);

        yield return new WaitForSeconds(0.7f);

        isShowing = false;
    }

    private IEnumerator PublishAchievementIE(Achievements.achievements key)
    {
        if (string.IsNullOrEmpty(SaveManager.token))
        {
            yield break;
        }

        string url = "https://api.brew-connect.com/v1/online/unlock_achievement";
        string json = "{" +
            "\"user_token\": \"" + SaveManager.token + "\"," +
            "\"organization_token\": \"" + organizationToken + "\"," +
            "\"project_token\": \"" + projectToken + "\"," +
            "\"achievement_key\": \"" + key + "\"" +
        "}";
        byte[] post = Encoding.UTF8.GetBytes(json);
        
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("content-Type", "application/json");

        using (WWW www = new WWW(url, post, headers))
        {
            yield return www;

            string jsonResponse = www.text;
            UnlockAchievementResponse response = JsonUtility.FromJson<UnlockAchievementResponse>(jsonResponse);

            Debug.Log(response.message[0]);

            if (StatusCode(www) == 201)
            {
                Sprite downloadedIcon = null;

                if (!string.IsNullOrEmpty(response.achievement.icon_url))
                {
                    yield return StartCoroutine(LoadSpriteFromURL(response.achievement.icon_url, (sprite) =>
                    {
                        downloadedIcon = sprite;
                    }));
                }

                achievementsToDisplay.Add(new AchievementDisplayData
                {
                    key = key,
                    title = response.achievement.title,
                    description = response.achievement.description,
                    icon = downloadedIcon
                });

                if (!isProcessingQueue)
                {
                    StartCoroutine(ProcessQueue());
                }
            }
        }
    }

    private int StatusCode(WWW www)
    {
        string statusLine;
        if (www.responseHeaders.TryGetValue("STATUS", out statusLine))
        {
            string[] parts = statusLine.Split(' ');
            int statusCode;
            if (parts.Length > 1 && int.TryParse(parts[1], out statusCode))
            {
                return statusCode;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }

    private IEnumerator LoadSpriteFromURL(string url, Action<Sprite> callback)
    {
        using (WWW www = new WWW(url))
        {
            yield return www;

            if (StatusCode(www) != 200)
            {
                Debug.LogError("Error while downloading image: " + www.error);
                callback.Invoke(null);
                yield break;
            }

            Texture2D texture = www.texture;

            if (texture == null)
            {
                Debug.LogError("Invalid texture downloaded from URL.");

                callback.Invoke(null);
                yield break;
            }

            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
                );

            callback.Invoke(sprite);
        }
    }

    private IEnumerator ProcessQueue()
    {
        isProcessingQueue = true;

        while (achievementsToDisplay.Count > 0)
        {
            var data = achievementsToDisplay[0];
            yield return StartCoroutine(ShowAchievementIE(data.title, data.description, data.icon));

            achievementsToDisplay.RemoveAt(0);
        }

        isProcessingQueue = false;
    }
}

[Serializable]
public class UnlockAchievementResponse
{
    public string[] message;
    public AchievementsResponse achievement;
}

[Serializable]
public class AchievementsResponse
{
    public string title;
    public string description;
    public string icon_url;
}

[Serializable]
public class AchievementDisplayData
{
    public Achievements.achievements key;
    public string title;
    public string description;
    public Sprite icon;
}