using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_WitchMovement : MonoBehaviour
{
    // 戚疑 淫恵 痕呪
    private float x = 0f;
    private float z = 0f;
    private float speed = 5f;
    private Vector3 moveDir = Vector3.zero;

    // 繊覗 淫恵 痕呪
    private float jumpPower = 6f;
    private bool isJumping = false;

    // 噺穿 淫恵 痕呪
    private float lerpSpeed = 10f;
    private Vector3 forwardVector = Vector3.zero;                 // 原橿亜 郊虞瑳 蒋号狽 

    // 覗遇乞球 噺穿 淫恵 痕呪
    private float angularSpeed = 0f;
    private float maxAngularSpeed = 360f;
    //private Vector3 angularSpeed = Vector3.zero;
    private PEA_WitchSkill witchSkill;

    // 蕉艦五戚芝 淫恵 痕呪
    private enum AnimState
    {
        Idle,
        Walk,
        Run,
        Jump,
    }

    AnimState animState = AnimState.Idle;

    // 紫遂拝 陳匂獲闘 痕呪
    private Rigidbody rig = null;
    private Animator anim = null;

    // 拭巨斗拭辞 尻衣背匝 痕呪
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
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                probBodyRidigbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                print("つつつ");
                ProbStabilization();
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                probBodyRidigbody.constraints = RigidbodyConstraints.None;
            }
            else
            {
                print("たたたたた");
                ProbRotateByMove();
            }
        }
        Jump();
        SetAnimation();
    }

    // 蒋及疎酔 戚疑
    private void Move()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        moveDir = new Vector3(x, 0, z).normalized;

        transform.Translate(moveDir * speed * Time.deltaTime);
    }

    // 戚疑 号狽拭 魚虞 倖搭税 蒋号狽 竺舛
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

    // 倖搭 噺穿
    private void Rotate()
    {
        // 戚疑拝 凶拭澗 朝五虞 蒋号狽 == 鎧 蒋号狽
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
                Vector3 probEulerAngles = probBodyRidigbody.transform.eulerAngles;
                transform.eulerAngles = new Vector3(0, cameraAnchor.eulerAngles.y, 0);
                probBodyRidigbody.transform.eulerAngles = probEulerAngles;
            }
        }
    }

    public void SetProbRigidbody(Rigidbody rig)
    {
        probBodyRidigbody = rig;
    }

    // 覗遇析 凶 戚疑 号狽拭 魚虞 噺穿(閏君蟹巨奄)
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

    // 繊覗
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

    // 覗遇 照舛鉢
    private void ProbStabilization()
    {
        if(probCollider.transform.localEulerAngles.x <= 1 && probCollider.transform.localEulerAngles.y <= 1 && probCollider.transform.localEulerAngles.z <= 1)
        {
            probCollider.transform.localEulerAngles = Vector3.zero;
        }
        else
        {
            probCollider.transform.rotation = Quaternion.Lerp(probCollider.transform.rotation, probBody.transform.rotation, lerpSpeed * Time.deltaTime);
        }
        transform.position = new Vector3(transform.position.x, probCollider.transform.position.y, transform.position.z);
        probBodyRidigbody.transform.localPosition = Vector3.zero;
        probBody.GetChild(1).localPosition = Vector3.zero;
        probBody.GetChild(1).rotation = probBodyRidigbody.transform.rotation;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if( isJumping && collision.contacts[0].point.y < transform.position.y)
        {
            isJumping = false;
        }
    }
}

