using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_WitchMovement : MonoBehaviour
{
    // �̵� ���� ����
    private float x = 0f;
    private float z = 0f;
    private float speed = 5f;
    private Vector3 moveDir = Vector3.zero;

    // ���� ���� ����
    private float jumpPower = 6f;
    private bool isJumping = false;

    // ȸ�� ���� ����
    private float lerpSpeed = 30f;
    private Vector3 forwardVector = Vector3.zero;                 // ���డ �ٶ� �չ��� 

    // ����� ������Ʈ ����
    private Rigidbody rig = null;

    // �����Ϳ��� �������� ����
    public Transform body;
    public Transform cameraAnchor;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
        Rotate();
        Jump();
    }

    // �յ��¿� �̵�
    private void Move()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        moveDir = new Vector3(x, 0, z).normalized;

        transform.Translate(moveDir * speed * Time.deltaTime);
    }

    // �̵� ���⿡ ���� ������ �չ��� ����
    private void SetForwardVector()
    {
         if(x != 0)
         {
            if(x > 0)
            {
                if( z == 0)
                {
                    forwardVector = transform.right;
                }
                else if(z > 0)
                {
                    forwardVector = transform.forward + transform.right;
                }
                else if(z < 0)
                {
                    forwardVector = -transform.forward + transform.right;
                }

            }
            else
            {
                if (z == 0)
                {
                    forwardVector = -transform.right;
                }
                else if (z > 0)
                {
                    forwardVector = transform.forward - transform.right;
                }
                else if (z < 0)
                {
                    forwardVector = -transform.forward - transform.right;
                }
            }
         }
        else
        {
             if (z > 0)
             {
                forwardVector = transform.forward;
             }
             else if (z < 0)
             {
                forwardVector = -transform.forward;
             }
        }
    }

    // ���� ȸ��
    private void Rotate()
    {
        // �̵��� ������ ī�޶� �չ��� == �� �չ���
        if(moveDir != Vector3.zero)
        {
            SetForwardVector();
            body.forward = Vector3.Lerp(body.forward, forwardVector, lerpSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, cameraAnchor.eulerAngles.y, 0);
        }
    }

    // ����
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            rig.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            isJumping = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].point.y < transform.position.y)
        {
            isJumping = false;
        }
    }
}

