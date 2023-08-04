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

    // 사용할 컴포넌트 변수
    private Rigidbody rig = null;

    // 에디터에서 연결해줄 변수
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
            body.forward = Vector3.Lerp(body.forward, forwardVector, lerpSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, cameraAnchor.eulerAngles.y, 0);
        }
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].point.y < transform.position.y)
        {
            isJumping = false;
        }
    }
}

