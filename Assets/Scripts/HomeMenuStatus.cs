using UnityEngine;
using WiiU = UnityEngine.WiiU;

public class HomeMenuStatus : MonoBehaviour
{
	public bool enableHomeMenu = false;

	void Update()
	{
		WiiU.Core.homeMenuEnabled = enableHomeMenu && !SaveGameState.isSaving;
	}
}
