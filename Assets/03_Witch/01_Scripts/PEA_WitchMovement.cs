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

    // ������� ȸ�� ���� ����
    private float angularSpeed = 0f;
    private float maxAngularSpeed = 30f;
    //private Vector3 angularSpeed = Vector3.zero;
    private PEA_WitchSkill witchSkill;

    // �ִϸ��̼� ���� ����
    private enum AnimState
    {
        Idle,
        Walk,
        Run,
        Jump,
    }

    AnimState animState = AnimState.Idle;

    // ����� ������Ʈ ����
    private Rigidbody rig = null;
    private Animator anim = null;

    // �����Ϳ��� �������� ����
    public Transform body;
    public Transform probBody;
    public Transform cameraAnchor;
    public Rigidbody probBodyRidigbody;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        witchSkill = GetComponent<PEA_WitchSkill>();
    }

    void Update()
    {
        Move();
        Rotate();
        if (witchSkill.IsChanged)
        {
            ProbRotateByMove();
        }
        Jump();
        SetAnimation();
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
            if (!witchSkill.IsChanged)
            {
                body.forward = Vector3.Lerp(body.forward, forwardVector, lerpSpeed * Time.deltaTime);
            }
            transform.eulerAngles = new Vector3(0, cameraAnchor.eulerAngles.y, 0);
        }
    }

    public void SetProbRigidbody(Rigidbody rig)
    {
        probBodyRidigbody = rig;
        print(probBodyRidigbody.gameObject.name);
    }

    // ������ �� �̵� ���⿡ ���� ȸ��(���������)
    private void ProbRotateByMove()
    {
        //angularSpeed = Vector3.Lerp(angularSpeed, new Vector3(moveDir.z, 0, -moveDir.x) * maxAngularSpeed, 10 * Time.deltaTime);
        //print(probBodyRidigbody.gameObject.name);
        //probBodyRidigbody.angularVelocity = angularSpeed;

        if (moveDir != Vector3.zero)
        {
            if (angularSpeed < maxAngularSpeed)
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
            if (angularSpeed > 0)
            {
                angularSpeed -= Time.deltaTime * 5f;
            }
            else
            {
                angularSpeed = 0f;
            }
        }

        probBodyRidigbody.angularVelocity = new Vector3(moveDir.z, 0, -moveDir.x) * angularSpeed;
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

    private void SetAnimation()
    {
        switch (animState)
        {
            case AnimState.Idle:
                if(moveDir != Vector3.zero)
                {
                    animState = AnimState.Walk;
                    anim.SetTrigger("Walk");
                }
                break;
            case AnimState.Walk:
                if(moveDir == Vector3.zero)
                {
                    animState = AnimState.Idle;
                    anim.SetTrigger("Idle");
                }
                break;
            case AnimState.Run:              
                break;
            case AnimState.Jump:
                break;
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

