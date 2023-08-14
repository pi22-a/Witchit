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
    private float lerpSpeed = 10f;
    private Vector3 forwardVector = Vector3.zero;                 // ���డ �ٶ� �չ��� 

    // ������� ȸ�� ���� ����
    private float angularSpeed = 0f;
    private float maxAngularSpeed = 360f;
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
    public Transform witchBody;
    public Transform probBody;
    public Transform cameraAnchor;
    public Rigidbody probBodyRidigbody;
    public MeshCollider probCollider;

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
                witchBody.forward = Vector3.Lerp(witchBody.forward, forwardVector, lerpSpeed * Time.deltaTime);
                transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, new Vector3(0, cameraAnchor.eulerAngles.y, 0), lerpSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 probEulerAngles =  probBodyRidigbody.transform.eulerAngles;
                transform.eulerAngles = new Vector3(0, cameraAnchor.eulerAngles.y, 0);
                probBodyRidigbody.transform.eulerAngles = probEulerAngles;
            }
        }
    }

    public void SetProbRigidbody(Rigidbody rig)
    {
        probBodyRidigbody = rig;
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

        //probBodyRidigbody.angularVelocity = new Vector3(moveDir.z, 0, -moveDir.x) * angularSpeed;
        //probCollider.transform.rotation = probBodyRidigbody.transform.rotation;
        //probBodyRidigbody.transform.position = transform.position;

        probBodyRidigbody.transform.Rotate(((transform.forward * -moveDir.x) + (transform.right * moveDir.z).normalized) * maxAngularSpeed * Time.deltaTime);
        transform.position = probBodyRidigbody.transform.position;
        probBodyRidigbody.transform.localPosition = Vector3.zero;
        probBody.GetChild(1).localPosition = Vector3.zero;
        probBody.GetChild(1).rotation = probBodyRidigbody.transform.rotation;

    }

    // ����
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            if (!witchSkill.IsChanged)
            {
                rig.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                isJumping = true;
                animState = AnimState.Jump;
                anim.SetTrigger("Jump");
            }
            else
            {
                probBodyRidigbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                isJumping = true;
            }
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
            case AnimState.Jump:
                if (!isJumping)
                {
                    anim.SetTrigger("JumpEnd");
                    if (moveDir != Vector3.zero)
                    {
                        animState = AnimState.Walk;
                        anim.SetTrigger("Walk");
                    }
                    else
                    {
                        animState = AnimState.Idle;
                        anim.SetTrigger("Idle");
                    }
                }
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if( isJumping && collision.contacts[0].point.y < transform.position.y)
        {
            isJumping = false;
        }
    }
}

