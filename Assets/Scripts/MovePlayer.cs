using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Vector3 postion = transform.position;
            postion.z -= 0.1f;
            transform.position = postion;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            Vector3 postion = transform.position;
            postion.z += 0.1f;
            transform.position = postion;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 postion = transform.position;
            postion.x -= 0.1f;
            transform.position = postion;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 postion = transform.position;
            postion.x += 0.1f;
            transform.position = postion;
        }
    }
}