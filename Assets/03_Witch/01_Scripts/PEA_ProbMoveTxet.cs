using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_ProbMoveTxet : MonoBehaviour
{
    private float x = 0f;
    private float z = 0f;
    private float speed = 5f;
    private float angularSpeed = 0f;
    private readonly float maxAngularSpeed = 200f;
    private Vector3 moveDir = Vector3.zero;
    //private Vector3 angularSpeed = Vector3.zero;
    public Rigidbody rig;

    void Start()
    {
        //rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        moveDir = new Vector3(x, 0, z).normalized;

        transform.parent.position += moveDir * speed * Time.deltaTime;

        if(moveDir != Vector3.zero)
        {
            if(angularSpeed < maxAngularSpeed)
            {
                angularSpeed += Time.deltaTime * 10f;
            }
            else
            {
                angularSpeed = maxAngularSpeed;
            }
        }
        else
        {
            if(angularSpeed > 0)
            {
                angularSpeed -= Time.deltaTime * 20f;
            }
            else
            {
                angularSpeed = 0f;
            }

        }

        //angularSpeed = Vector3.Lerp(angularSpeed, new Vector3(moveDir.z, 0, -moveDir.x) * maxAngularSpeed, 10 * Time.deltaTime);

        //transform.localEulerAngles += angularSpeed * Time.deltaTime;
        //transform.localEulerAngles += transform.parent.localEulerAngles + (angularSpeed * Time.deltaTime);
        //rig.angularVelocity = new Vector3(moveDir.z, 0 , -moveDir.x)* angularSpeed;
        //transform.Rotate((Camera.main.transform.right * moveDir.z + Camera.main.transform.forward * moveDir.x) * maxAngularSpeed * Time.deltaTime);
    }
}
