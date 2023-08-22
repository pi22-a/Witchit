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
    float speed = 10;

    //Character Controller 담을 변수
    CharacterController cc;
    //점프 파워
    float jumpPower = 3;
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
                    h = Input.GetAxis("Horizontal");
                    v = Input.GetAxis("Vertical");
                    Move();
                    break;
                case State.Confusion:
                    Move();
                    break;
            }
        }
        //나의 Player 가 아니라면
        else
        {
            cc.enabled = false;
            //위치 보정
            transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
            //회전 보정
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, lerpSpeed * Time.deltaTime);
            cc.enabled = true;
        }

        anim.SetFloat("Horizontal", h);
        anim.SetFloat("Vertical", v);

        if (Input.GetKeyDown(KeyCode.Y))
        {
            state = State.Confusion;
            Mushroom();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(photonView.IsMine)
        {
            if (other.transform.tag == "Mushroom")
            {
                state = State.Confusion;
                Mushroom();
            }
        }
        
    }

    void Move()
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

        //3. 그방향으로 움직이자.
        //transform.position += dir * speed * Time.deltaTime;
        //anim.SetFloat("Horizontal", h);
        //photonView.RPC(nameof(SetTriggerRpc), RpcTarget.All, "Horizontal",h);
        //anim.SetFloat("Vertical", v);
        //photonView.RPC(nameof(SetTriggerRpc), RpcTarget.All, "Vertical", v);
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
            //h 값 받자.
            h = (float)stream.ReceiveNext();
            //v 값 받자.
            v = (float)stream.ReceiveNext();
        }
    }
}