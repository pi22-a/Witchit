using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_WatchCamera : MonoBehaviour
{
    private float x = 0f;
    private float z = 0f;
    private float mouseX = 0F;
    private float mouseY = 0f;
    private float speed = 10f;
    private Vector3 moveDir = Vector3.zero;

    void Update()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        print("dd");
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        moveDir = new Vector3(x, 0, z).normalized;

        transform.Translate(moveDir * speed * Time.deltaTime);
    }

    private void Rotate()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        transform.localEulerAngles += new Vector3(-mouseY, mouseX, 0);
    }
}
