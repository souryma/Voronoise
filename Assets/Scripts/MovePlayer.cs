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
    }
}