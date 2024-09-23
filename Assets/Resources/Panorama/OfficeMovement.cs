using UnityEngine;

public class OfficeMovement : MonoBehaviour
{
    public float speed = 14f;
    public float leftLimit = -8.5f;
    public float rightLimit = 4f;

    void Update()
    {
        float move = 0f;

        if (Input.GetKey(KeyCode.RightArrow))
        {
            move = -speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            move = speed * Time.deltaTime;
        }

        Vector3 currentPosition = transform.position;

        float newPositionX = Mathf.Clamp(currentPosition.x + move, leftLimit, rightLimit);

        transform.position = new Vector3(newPositionX, currentPosition.y, currentPosition.z);
    }
}
