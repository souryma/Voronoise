using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    [Range(50, 500)]public float speed = 100;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontal, 0, vertical);
        movementDirection.Normalize();

        transform.position += movementDirection * Time.deltaTime * speed;

        if (Input.GetAxis("LeftTriggerXbox") == 1.0f)
        {
            speed -= 5;
            if (speed < 1)
                speed = 1;
        }

        if (Input.GetAxis("RightTriggerXbox") == 1.0f)
        {
            speed += 5;
            if (speed > 1000)
                speed = 999;
        }

        // Relocate the player if he's out of bounds
        if (transform.position.z > -500)
        {
            Vector3 position = transform.position;
            position.z -= 1000;
            transform.position = position;
        }
        if (transform.position.z < -1500)
        {
            Vector3 position = transform.position;
            position.z += 1000;
            transform.position = position;
        }
        if (transform.position.x < 100)
        {
            Vector3 position = transform.position;
            position.x += 1800;
            transform.position = position;
        }
        if (transform.position.x > 1900)
        {
            Vector3 position = transform.position;
            position.x -= 1800;
            transform.position = position;
        }
    }
}