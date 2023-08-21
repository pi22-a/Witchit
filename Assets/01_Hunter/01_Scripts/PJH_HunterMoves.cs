using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// 사용자의 입력에따라 앞뒤좌우로 이동하고싶다.
// 사용자가 점프버튼을 누르면 점프를 뛰고싶다.
// 최대 점프 횟수를 정해서 여러번 점프하게 하고싶다.
public class PJH_HunterMoves : MonoBehaviourPun, IPunObservable
{
    enum State
    {
        Move,
        Confusion,  // 혼란
    }

    State state;

    bool isJump = false;
    public GameObject HunterUI;

    //속력 
    float speed = 5;

    //Character Controller 담을 변수
    CharacterController cc;
    public Transform hunterBody;
    //점프 파워
    float jumpPower = 5;
    //중력
    float gravity = -9.81f;
    //y 속력
    float yVelocity = 0;

    float h, v; //가로, 세로

    Animator anim;

    LayerMask witchLayer;

    //서버에서 넘어오는 위치값
    Vector3 receivePos;
    //서버에서 넘어오는 회전값
    Quaternion receiveRot;
    Quaternion receiveHunterRot;
    //보정하는 속력
    float lerpSpeed = 50;


    void Start()
    {
        // Witch레이어 설정
        witchLayer = LayerMask.NameToLayer("Witch");
        //Character Controller 가져오자
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        state = State.Move;

        if (photonView.IsMine)
        {
            //UI 를 비활성화 하자
            HunterUI.SetActive(true);
        }
        //나의 PhotonView GameManager 에 알려주자
        //GameManager.instance.AddPlayer(photonView);
    }

    void Update()
    {
        if (photonView.IsMine)
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
        //나의 Player 가 아니라면
        else
        {
            //위치 보정
            transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
            //회전 보정
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);            
        }

        anim.SetFloat("Horizontal", h);
        anim.SetFloat("Vertical", v);

        if (Input.GetKeyDown(KeyCode.Y))
        {
            state = State.Confusion;
            Mushroom();
        }
    }


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


        //만약에 땅에 닿아있다면
        if (cc.isGrounded == true)
        {
            //yVeloctiy 를 0 으로 하자
            yVelocity = 0;

            //만약에 점프 중이라면
            if (isJump == true)
            {
                //착지 Trigger 발생
                photonView.RPC(nameof(SetTriggerRpc), RpcTarget.All, "Land");
            }

            //점프 아니라고 설정
            isJump = false;
        }

        //스페이바를 누르면 점프를 하고 싶다.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Jump");
            //yVelocity 에 jumpPower 를 셋팅
            yVelocity = jumpPower;

            photonView.RPC(nameof(SetTriggerRpc), RpcTarget.All, "Jump");
            //점프 중이라고 설정
            isJump = true;
        }

        //yVelocity 를 중력만큼 감소시키자
        yVelocity += gravity * Time.deltaTime;

        //yVelocity 값을 dir 의 y 값에 셋팅
        dir.y = yVelocity;

        //좌 Ctrl 클릭시 바디슬램

        //if (Input.GetKeyDown(KeyCode.LeftControl))
        //{
        //    //bool a;
        //    //엉덩이에 col 넣고 그게 뭐든 부딫히면 그때 근처 탐색하고, 데미지 부여 형식
        //    //
        //    if (gameObject.transform.position.y > 3)
        //    {
        //        anim.SetTrigger("BodySlam");
        //        //잠깐 멈추고 빠르게 떨어진다. (애니메이션 적용)
        //        this.GetComponent<Rigidbody>().isKinematic = true;
        //        yVelocity = -20; //여기 수정 일정 높이 이상인걸 따야함.
        //        //일정 높이 이상일때 발동한다. (애니메이션으로 구별)
        //        //높이에 비례해서 공격 범위가 늘어난다 (프로토때 미적용)
        //        // 반경 3M 안의 충돌체중에 마녀를 찾는다.
        //        int layerMask = (1 << witchLayer);
        //        Collider[] cols = Physics.OverlapSphere(transform.position, 3, layerMask);
        //        for (int i = 0; i < cols.Length; i++)
        //        {
        //            // 데미지를 n 만큼 주고싶다.
        //            int n = 5;
        //            cols[i].GetComponent<PEA_WitchHP>().Damage(n);
        //        }
        //    }
        //   
        //
        //}


        //3. 그방향으로 움직이자.
        //transform.position += dir * speed * Time.deltaTime;
        cc.Move(dir * speed * Time.deltaTime);

        
    }

    public void Mushroom()
    {
        // 버섯에 닿을시 발동 (버섯에서 쏴줌)        
        StartCoroutine(MushroomMove());        
    }
    IEnumerator MushroomMove()
    {
        float time = 0;
        float oneTime = 1;
        // 유저가 플레이어 작동 못하게함 (3초)
        while (time < 3)
        {
            // 랜덤으로 움직이게 함
            h = Random.Range(-1f, 1f);
            v = Random.Range(-1f, 1f);
            yield return new WaitForSeconds(oneTime);

            time += oneTime;

        }
        // 다시 유저에게 컨트롤권 부여.
        state = State.Move;

    }
    [PunRPC]
    void SetTriggerRpc(string parameter)
    {
        anim.SetTrigger(parameter);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //내 Player 라면
        if (stream.IsWriting)
        {
            //나의 위치값을 보낸다.
            stream.SendNext(transform.position);
            //나의 회전값을 보낸다.
            stream.SendNext(transform.rotation);
            stream.SendNext(hunterBody.rotation);
            //h 값 보낸다.
            stream.SendNext(h);
            //v 값 보낸다.
            stream.SendNext(v);
        }
        //내 Player 아니라면
        else
        {
            //위치값을 받자.
            receivePos = (Vector3)stream.ReceiveNext();
            //회전값을 받자.
            receiveRot = (Quaternion)stream.ReceiveNext();
            receiveHunterRot = (Quaternion)stream.ReceiveNext();
            //h 값 받자.
            h = (float)stream.ReceiveNext();
            //v 값 받자.
            v = (float)stream.ReceiveNext();
        }
    }
}