using UnityEngine;

public class ChangeRoomName : MonoBehaviour
{
	// Scripts
	I18nTextTranslator i18nTextTranslator;

	void Start()
	{
		// Get scripts
		i18nTextTranslator = GetComponent<I18nTextTranslator>(); // Get script in current game object
	}
	
	public void ChangeRoomId(string id)
	{
		i18nTextTranslator.textId = id;
		i18nTextTranslator.UpdateText();
	}
}
