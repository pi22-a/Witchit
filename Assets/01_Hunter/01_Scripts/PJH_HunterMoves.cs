using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 사용자의 입력에따라 앞뒤좌우로 이동하고싶다.
// 사용자가 점프버튼을 누르면 점프를 뛰고싶다.
// 최대 점프 횟수를 정해서 여러번 점프하게 하고싶다.
public class PJH_HunterMoves : MonoBehaviour
{
    enum State
    {
        Move,
        Confusion,  // 혼란
    }

    State state;


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


    void Start()
    {
        // Witch레이어 설정
        witchLayer = LayerMask.NameToLayer("Witch");
        //Character Controller 가져오자
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        state = State.Move;
    }

    void Update()
    {
        switch (state)
        {
            case State.Move:
                Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                break;
            case State.Confusion:
                Move(h, v);
                break;
        }
    }
    float h, v;


    void Move(float h, float v)
    {
        //W, S, A, D 키를 누르면 앞뒤좌우로 움직이고 싶다.

        //1. 사용자의 입력을 받자.


        //2. 방향을 만든다.
        //좌우
        Vector3 dirH = transform.right * h;
        //앞뒤
        Vector3 dirV = transform.forward * v;
        //최종
        Vector3 dir = dirH + dirV;
        dir.Normalize();

        anim.SetFloat("Horizontal", h);
        anim.SetFloat("Vertical", v);

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
            anim.SetTrigger("Jump");
            //yVelocity 에 jumpPower 를 셋팅
            yVelocity = jumpPower;
        }

        //yVelocity 를 중력만큼 감소시키자
        yVelocity += gravity * Time.deltaTime;
        //좌 Ctrl 클릭시 바디슬램
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            //bool a;
            //엉덩이에 col 넣고 그게 뭐든 부딫히면 그때 근처 탐색하고, 데미지 부여 형식
            //
            if (gameObject.transform.position.y > 3)
            {
                anim.SetTrigger("BodySlam");
                //잠깐 멈추고 빠르게 떨어진다. (애니메이션 적용)
                this.GetComponent<Rigidbody>().isKinematic = true;
                yVelocity = -20; //여기 수정 일정 높이 이상인걸 따야함.
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

        if (Input.GetKeyDown(KeyCode.U))
        {
            Mushroom();
            state = State.Confusion;
        }
    }

    public void Mushroom()
    {
        // 버섯에 닿을시 발동 (버섯에서 쏴줌)
        // 유저가 플레이어 작동 못하게함 (3초)

        // 랜덤으로 움직이게 함
        StartCoroutine(MushroomMove());
        // 다시 유저에게 컨트롤권 부여.
    }
    IEnumerator MushroomMove()
    {
        float time = 0;
        float oneTime = 1;
        while (time >= 3)
        {
            h = Random.Range(-1f, 1f);
            v = Random.Range(-1f, 1f);
            yield return new WaitForSeconds(oneTime);

            time += oneTime;

        }

        state = State.Move;

    }
}