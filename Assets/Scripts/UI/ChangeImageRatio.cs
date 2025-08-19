using UnityEngine;
using UnityEngine.UI;

public class ChangeImageRatio : MonoBehaviour
{
	[SerializeField] private AspectRatioFitter[] aspectRatioFitters;
    private GamepadClickAdapter gamepadClickAdapter;

    void Start()
    {
        gamepadClickAdapter = FindObjectOfType<GamepadClickAdapter>();

        ChangeRatio(SaveManager.saveData.settings.ratioId);
    }

	public void ChangeRatioFromSwitcher(SwitcherData switcherData)
	{
		if (switcherData != null)
		{
            ChangeRatio(switcherData.currentOptionId);
        }
	}

	public void ChangeRatio(int ratioId)
	{
		if (ratioId == 1)
		{
            foreach (AspectRatioFitter arf in aspectRatioFitters)
            {
                if (arf != null)
                {
                    arf.aspectRatio = 4f / 3;
                }
            }

            if (gamepadClickAdapter != null)
            {
                gamepadClickAdapter.canvasResolution = new Vector2(960f, 720f);
            }
		}
		else
		{
            foreach (AspectRatioFitter arf in aspectRatioFitters)
            {
                if (arf != null)
                {
                    arf.aspectRatio = 16f / 9;
                }
            }

            if (gamepadClickAdapter != null)
            {
                gamepadClickAdapter.canvasResolution = new Vector2(1280f, 720f);
            }
        }
	}
}
