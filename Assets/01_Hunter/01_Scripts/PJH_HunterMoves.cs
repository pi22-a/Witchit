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

    LayerMask witchLayer;
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
        // Witch레이어 설정
        witchLayer = LayerMask.NameToLayer("Witch");
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
            this.GetComponent<Rigidbody>().isKinematic = false;
        }
        
        //스페이바를 누르면 점프를 하고 싶다.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //yVelocity 에 jumpPower 를 셋팅
            yVelocity = jumpPower;
        }
        
        //yVelocity 를 중력만큼 감소시키자
        yVelocity += gravity * Time.deltaTime;
        //좌 Ctrl 클릭시 바디슬램
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
        print("11");
            //if(yVelocity > 2) 
            {
                //잠깐 멈추고 빠르게 떨어진다. (애니메이션 적용)
                this.GetComponent<Rigidbody>().isKinematic = true;
                yVelocity = 2 * yVelocity; //여기 수정 일정 높이 이상인걸 따야함.
                //일정 높이 이상일때 발동한다. (애니메이션으로 구별)
                //높이에 비례해서 공격 범위가 늘어난다 (프로토때 미적용)
                // 반경 3M 안의 충돌체중에 마녀를 찾는다.
                int layerMask = (1 << witchLayer);
                Collider[] cols = Physics.OverlapSphere(transform.position, 3, layerMask);
                for (int i = 0; i < cols.Length; i++)
                {
                    // 데미지를 n 만큼 주고싶다.
                    int n = 5;
                    cols[i].GetComponent<PEA_WitchHP>().Damage(n);
                }
            }
            
        }
        
        
        
        //yVelocity 값을 dir 의 y 값에 셋팅
        dir.y = yVelocity;
        
        //3. 그방향으로 움직이자.
        //transform.position += dir * speed * Time.deltaTime;
        cc.Move(dir * speed * Time.deltaTime);
        
    }

    
}