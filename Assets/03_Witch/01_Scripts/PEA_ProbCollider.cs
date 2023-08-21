using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_ProbCollider : MonoBehaviour
{
    private float x = 0f;
    private float z = 0f;
    private float angularSpeed = 0f;
    private readonly float maxAngularSpeed = 30f;
    private Vector3 moveDir = Vector3.zero;
    private Rigidbody rig = null;

    private  Transform probModel;

    public PEA_WitchSkill witchSkill;


    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (witchSkill.IsChanged)
        {
            x = Input.GetAxis("Horizontal");
            z = Input.GetAxis("Vertical");

            moveDir = new Vector3(x, 0, z).normalized;

            ProbRotateByMove();
        }
    }

    // ������ �� �̵� ���⿡ ���� ȸ��(���������)
    private void ProbRotateByMove()
    {
        if (moveDir != Vector3.zero)
        {
            if (angularSpeed < maxAngularSpeed)
            {
                angularSpeed += Time.deltaTime * 20f;
            }
            else
            {
                angularSpeed = maxAngularSpeed;
            }
        }
        else
        {
            if (angularSpeed > 0)
            {
                angularSpeed -= Time.deltaTime * 30f;
            }
            else
            {
                angularSpeed = 0f;
            }
        }


        //rig.angularVelocity = moveDir * angularSpeed;
        transform.Rotate(((Camera.main.transform.forward * -moveDir.x) + (Camera.main.transform.right * moveDir.z).normalized) * maxAngularSpeed * Time.deltaTime);
        //transform.position = probBodyRidigbody.transform.position;
        //probBodyRidigbody.transform.localPosition = Vector3.zero;
        //probBody.GetChild(1).localPosition = Vector3.zero;
        //probBody.GetChild(1).rotation = probBodyRidigbody.transform.rotation;
    }
}
