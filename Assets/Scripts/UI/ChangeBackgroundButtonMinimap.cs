using UnityEngine;

public class ChangeBackgroundButtonMinimap : MonoBehaviour
{
    public GameObject[] camerasBackground;
    public GameObject minimapPanel;
    private string currentCameraName;

    void Update()
    {
        if (currentCameraName != null)
        {
            foreach (GameObject background in camerasBackground)
            {
                Animator animator = background.GetComponent<Animator>();

                if (background.name == (currentCameraName + "-Background"))
                {
                    animator.Play("Blink");
                }
                else
                {
                    animator.Play("Idle");
                }
            }
        }
    }

    public void ButtonPressed(string cameraName)
    {
        if (currentCameraName != cameraName)
        {
            currentCameraName = cameraName;
        }
    }
}