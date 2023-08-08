using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용자의 입력에따라 앞뒤좌우로 이동하고싶다.
// 사용자가 점프버튼을 누르면 점프를 뛰고싶다.
// 최대 점프 횟수를 정해서 여러번 점프하게 하고싶다.
public class PJH_HunterMoves : MonoBehaviour
{
    //속력 
    float speed = 5;

    //Character Controller 담을 변수
    CharacterController cc;

    //점프 파워
    float jumpPower = 5;
    //중력
    float gravity = -9.81f;
    //y 속력
    float yVelocity = 0;

    Animator anim;

    //서버에서 넘어오는 위치값
    //Vector3 receivePos;
    //서버에서 넘어오는 회전값
    //Quaternion receiveRot = Quaternion.identity;
    //보정하는 속력
    //float lerpSpeed = 50;

    public enum State
    {
        Idle,
        Run,
        Attack,
        Jump,
        MeleeAttack,
    }
    public State state;
    void Start()
    {
        //Character Controller 가져오자
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        /*
        switch (state)
        {
            case State.Idle: PJH_HunterAnimation(); break;
            case State.Run: UpdateMove(); break;
            case State.Attack: UpdateAttack(); break;
        }*/
        //W, S, A, D 키를 누르면 앞뒤좌우로 움직이고 싶다.

        //1. 사용자의 입력을 받자.
        float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            //2. 방향을 만든다.
            //좌우
            Vector3 dirH = transform.right * h;
            //앞뒤
            Vector3 dirV = transform.forward * v;
            //최종
            Vector3 dir = dirH + dirV;
            dir.Normalize();

            //만약에 땅에 닿아있다면
            if (cc.isGrounded == true)
            {
                //yVeloctiy 를 0 으로 하자
                yVelocity = 0;
            }

            //스페이바를 누르면 점프를 하고 싶다.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //yVelocity 에 jumpPower 를 셋팅
                yVelocity = jumpPower;
            }

            //yVelocity 를 중력만큼 감소시키자
            yVelocity += gravity * Time.deltaTime;

            //yVelocity 값을 dir 의 y 값에 셋팅
            dir.y = yVelocity;

            //3. 그방향으로 움직이자.
            //transform.position += dir * speed * Time.deltaTime;
            cc.Move(dir * speed * Time.deltaTime);
        
    }

    
}