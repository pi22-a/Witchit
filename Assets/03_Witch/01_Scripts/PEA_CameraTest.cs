using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_CameraTest : MonoBehaviour
{
    private float x = 0f;
    private float y = 0f;
    private float mouseX = 0f;
    private float mouseY = 0f;
    private float rotSpeed = 200f;

    public float offsetX;
    public float offsetY;
    public float offsetZ;

    public GameObject target;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 fixedPos = new Vector3(target.transform.position.x + offsetX,
            target.transform.position.y + offsetY,
            target.transform.position.z + offsetZ);

        transform.position = Vector3.Lerp(transform.position, fixedPos, Time.deltaTime * 10f);

        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");
        mouseX += x * rotSpeed * Time.deltaTime;
        mouseY += y * rotSpeed * Time.deltaTime;

        mouseY = Mathf.Clamp(mouseY, -60f, 60f);

        transform.eulerAngles = new Vector3(-mouseY, mouseX, 0);
    }
}
