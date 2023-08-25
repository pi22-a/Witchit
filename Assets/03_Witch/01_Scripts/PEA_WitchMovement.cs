using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PEA_WitchMovement : MonoBehaviourPun, IPunObservable
{
    // 내가 아닌 플레이어 관련 변수
    private float receiveLerpSpeed = 50f;
    private Vector3 receivePos;
    private Quaternion receiveRot;
    private Quaternion receiveWitchRot;
    private Quaternion receiveProbRot;

    // 이동 관련 변수
    private float x = 0f;
    private float z = 0f;
    private float speed = 8f;
    private Vector3 moveDir = Vector3.zero;

    // 점프 관련 변수
    private float jumpPower = 10f;
    private bool isJumping = false;

    // 회전 관련 변수
    private float lerpSpeed = 10f;
    private Vector3 forwardVector = Vector3.zero;                 // 마녀가 바라볼 앞방향 

    // 프랍모드 회전 관련 변수
    private float angularSpeed = 0f;
    private float maxAngularSpeed = 360f;
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
    public GameObject nickname;
    public Transform witchBody;
    public Transform probBody;
    private Transform cameraAnchor;
    public Rigidbody probBodyRidigbody;
    public MeshCollider probCollider;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        witchSkill = GetComponent<PEA_WitchSkill>();

        if (photonView.IsMine)
        {
            cameraAnchor = new GameObject("CameraAnchor").transform;
            cameraAnchor.gameObject.AddComponent<PEA_Camera>().SetPlayer(transform);
            Camera.main.transform.SetParent(cameraAnchor);
            photonView.RPC(nameof(SetNickname), RpcTarget.All);
            nickname.SetActive(false);
        }
        else
        {
            nickname.SetActive(true);
        }
    }

    void Update()
    {
        if (photonView.IsMine)
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
                    ProbStabilization();
                }
                else if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    probBodyRidigbody.constraints = RigidbodyConstraints.None;
                }
                else
                {
                    ProbRotateByMove();
                }
            }
            Jump();
            SetAnimation();
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, receivePos, receiveLerpSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, receiveRot, receiveLerpSpeed * Time.deltaTime);
            witchBody.transform.rotation = Quaternion.Lerp(witchBody.transform.rotation, receiveWitchRot, receiveLerpSpeed * Time.deltaTime);
            if (witchSkill != null && witchSkill.IsChanged)
            {
                probBody.GetChild(1).rotation = Quaternion.Lerp(probBody.GetChild(1).rotation, receiveProbRot, receiveLerpSpeed * Time.deltaTime);
            }
        }
    }

    [PunRPC]
    private void SetNickname()
    {
        nickname.GetComponentInChildren<TMP_Text>().text = PhotonNetwork.NickName;
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

    // 프랍일 때 이동 방향에 따라 회전(굴러나디기)
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

        probBodyRidigbody.angularVelocity = ((transform.forward * -moveDir.x) + (transform.right * moveDir.z)).normalized * maxAngularSpeed;
        transform.position = probBodyRidigbody.transform.position;
        probBodyRidigbody.transform.localPosition = Vector3.zero;
        probBody.GetChild(1).localPosition = Vector3.zero;
        probBody.GetChild(1).rotation = probBodyRidigbody.transform.rotation;
    }

    // 점프
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            if (!witchSkill.IsChanged)
            {
                rig.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
                isJumping = true;
                animState = AnimState.Jump;
                photonView.RPC(nameof(SetAnimTrigger), RpcTarget.All, "Jump");
                //anim.SetTrigger("Jump");
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
                    photonView.RPC(nameof(SetAnimTrigger), RpcTarget.All, "Walk");
                    //anim.SetTrigger("Walk");
                }
                break;
            case AnimState.Walk:
                if(moveDir == Vector3.zero)
                {
                    animState = AnimState.Idle;
                    photonView.RPC(nameof(SetAnimTrigger), RpcTarget.All, "Idle");
                    //anim.SetTrigger("Idle");
                }
                break;
            case AnimState.Jump:
                if (!isJumping)
                {
                    photonView.RPC(nameof(SetAnimTrigger), RpcTarget.All, "JumpEnd");
                    //anim.SetTrigger("JumpEnd");
                    if (moveDir != Vector3.zero)
                    {
                        animState = AnimState.Walk;
                        photonView.RPC(nameof(SetAnimTrigger), RpcTarget.All, "Walk");
                        //anim.SetTrigger("Walk");
                    }
                    else
                    {
                        animState = AnimState.Idle;
                        photonView.RPC(nameof(SetAnimTrigger), RpcTarget.All, "Idle");
                        //anim.SetTrigger("Idle");
                    }
                }
                break;
        }
    }

    // 프랍 안정화
    private void ProbStabilization()
    {
        if(probCollider.transform.localEulerAngles.x <= 1 && probCollider.transform.localEulerAngles.y <= 1 && probCollider.transform.localEulerAngles.z <= 1)
        {
            probCollider.transform.localEulerAngles = Vector3.zero;
        }
        else
        {
            //probCollider.transform.rotation = Quaternion.Lerp(probCollider.transform.rotation, probBody.transform.rotation, lerpSpeed * Time.deltaTime);
            probCollider.transform.rotation = Quaternion.Lerp(probCollider.transform.rotation, Quaternion.LookRotation(Vector3.up, probBody.transform.right), lerpSpeed * Time.deltaTime);
        }
        transform.position = new Vector3(transform.position.x, probCollider.transform.position.y, transform.position.z);
        probBodyRidigbody.transform.localPosition = Vector3.zero;
        probBody.GetChild(1).localPosition = Vector3.zero;
        probBody.GetChild(1).rotation = probBodyRidigbody.transform.rotation;

    }

    [PunRPC]
    private void SetAnimTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    public void OnGround()
    {
        isJumping = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if( isJumping && collision.contacts[0].point.y < transform.position.y)
        {
            isJumping = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(witchBody.rotation);
            if (witchSkill != null && witchSkill.IsChanged)
            {
                stream.SendNext(probBody.GetChild(1).rotation);
            }
        }
        else
        {
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
            receiveWitchRot = (Quaternion)stream.ReceiveNext();
            if (witchSkill != null && witchSkill.IsChanged)
            {
                receiveProbRot = (Quaternion)stream.ReceiveNext();
            }
        }
    }

    private void OnDisable()
    {
        nickname.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.tag);
        if (other.CompareTag("WitchSphere"))
        {
            Camera.main.GetComponent<PEA_CameraShake>().ShakeCamera();
        }
    }
}

