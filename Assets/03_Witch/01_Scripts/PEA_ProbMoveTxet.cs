using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_ProbMoveTxet : MonoBehaviour
{
    private float x = 0f;
    private float z = 0f;
    private float speed = 5f;
    private float maxAngularSpeed = 200f;
    private Vector3 moveDir = Vector3.zero;
    private Vector3 angularSpeed = Vector3.zero;
    private Rigidbody rig;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        x = Input.GetAxis("Horizontal");
        z = Input.GetAxis("Vertical");

        moveDir = new Vector3(x, 0, z).normalized;

        transform.position += moveDir * speed * Time.deltaTime;

        angularSpeed = Vector3.Lerp(angularSpeed, new Vector3(moveDir.z, 0, -moveDir.x) * maxAngularSpeed, 10 * Time.deltaTime);

        //transform.localEulerAngles += angularSpeed * Time.deltaTime;

        rig.angularVelocity = angularSpeed;
    }
}
