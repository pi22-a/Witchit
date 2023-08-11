using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_WitchMovement : MonoBehaviour
{
    // 이동 관련 변수
    private float x = 0f;
    private float z = 0f;
    private float speed = 5f;
    private Vector3 moveDir = Vector3.zero;

    // 점프 관련 변수
    private float jumpPower = 6f;
    private bool isJumping = false;

    // 회전 관련 변수
    private float lerpSpeed = 30f;
    private Vector3 forwardVector = Vector3.zero;                 // 마녀가 바라볼 앞방향 

    // 프랍모드 회전 관련 변수
    private float angularSpeed = 0f;
    private float maxAngularSpeed = 30f;
    //private Vector3 angularSpeed = Vector3.zero;
    private PEA_WitchSkill witchSkill;

    // 애니메이션 관련 변수
    private enum AnimState
    {
        Idle,
        Walk,
        Run,
        Jump,
    }

    AnimState animState = AnimState.Idle;

    // 사용할 컴포넌트 변수
    private Rigidbody rig = null;
    private Animator anim = null;

    // 에디터에서 연결해줄 변수
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

    // 앞뒤좌우 이동
    private void Move()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        moveDir = new Vector3(x, 0, z).normalized;

        transform.Translate(moveDir * speed * Time.deltaTime);
    }

    // 이동 방향에 따라 몸통의 앞방향 설정
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

    // 몸통 회전
    private void Rotate()
    {
        // 이동할 때에는 카메라 앞방향 == 내 앞방향
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

    // 프랍일 때 이동 방향에 따라 회전(굴러나디기)
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

    // 점프
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

