using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_CameraTest : MonoBehaviour
{
    private float x = 0f;
    private float y = 0f;

    public float offsetX;
    public float offsetY;
    public float offsetZ;

    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 fixedPos = new Vector3(target.transform.position.x + offsetX,
            target.transform.position.y + offsetY,
            target.transform.position.z + offsetZ);

        transform.position = Vector3.Lerp(transform.position, fixedPos, Time.deltaTime * 10f);

        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");

        Camera.main.transform.Rotate(-y, x, 0 );

    }
}
