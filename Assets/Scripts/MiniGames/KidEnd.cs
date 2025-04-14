using UnityEngine;

public class KidEnd : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            MiniGamesLevelLoader.LoadScene("MainMenu");
        }
    }
}
