using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PEA_WitchMovement : MonoBehaviourPun, IPunObservable
{
    // ���� �ƴ� �÷��̾� ���� ����
    private float receiveLerpSpeed = 50f;
    private Vector3 receivePos;
    private Quaternion receiveRot;
    private Quaternion receiveWitchRot;
    private Quaternion receiveProbRot;

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

    // ���� ����ȭ
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

    [PunRPC]
    private void SetAnimTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
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
}

