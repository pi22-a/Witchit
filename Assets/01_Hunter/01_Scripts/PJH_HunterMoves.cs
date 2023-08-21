using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

// ������� �Է¿����� �յ��¿�� �̵��ϰ�ʹ�.
// ����ڰ� ������ư�� ������ ������ �ٰ�ʹ�.
// �ִ� ���� Ƚ���� ���ؼ� ������ �����ϰ� �ϰ�ʹ�.
public class PJH_HunterMoves : MonoBehaviourPun, IPunObservable
{
    enum State
    {
        Move,
        Confusion,  // ȥ��
    }

    State state;

    bool isJump = false;
    public GameObject HunterUI;

    //�ӷ� 
    float speed = 5;

    //Character Controller ���� ����
    CharacterController cc;
    public Transform hunterBody;
    //���� �Ŀ�
    float jumpPower = 5;
    //�߷�
    float gravity = -9.81f;
    //y �ӷ�
    float yVelocity = 0;

    float h, v; //����, ����

    Animator anim;

    LayerMask witchLayer;

    //�������� �Ѿ���� ��ġ��
    Vector3 receivePos;
    //�������� �Ѿ���� ȸ����
    Quaternion receiveRot;
    Quaternion receiveHunterRot;
    //�����ϴ� �ӷ�
    float lerpSpeed = 50;


    void Start()
    {
        // Witch���̾� ����
        witchLayer = LayerMask.NameToLayer("Witch");
        //Character Controller ��������
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();

        state = State.Move;

        if (photonView.IsMine)
        {
            //UI �� ��Ȱ��ȭ ����
            HunterUI.SetActive(true);
        }
        //���� PhotonView GameManager �� �˷�����
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
        //���� Player �� �ƴ϶��
        else
        {
            //��ġ ����
            transform.position = Vector3.Lerp(transform.position, receivePos, lerpSpeed * Time.deltaTime);
            //ȸ�� ����
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
        //W, S, A, D Ű�� ������ �յ��¿�� �����̰� �ʹ�.
        //1. ������� �Է��� ����.
        //2. ������ �����.
        //�¿�
        Vector3 dirH = transform.right * h;
        //�յ�
        Vector3 dirV = transform.forward * v;
        //����
        Vector3 dir = dirH + dirV;
        dir.Normalize();


        //���࿡ ���� ����ִٸ�
        if (cc.isGrounded == true)
        {
            //yVeloctiy �� 0 ���� ����
            yVelocity = 0;

            //���࿡ ���� ���̶��
            if (isJump == true)
            {
                //���� Trigger �߻�
                photonView.RPC(nameof(SetTriggerRpc), RpcTarget.All, "Land");
            }

            //���� �ƴ϶�� ����
            isJump = false;
        }

        //�����̹ٸ� ������ ������ �ϰ� �ʹ�.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Jump");
            //yVelocity �� jumpPower �� ����
            yVelocity = jumpPower;

            photonView.RPC(nameof(SetTriggerRpc), RpcTarget.All, "Jump");
            //���� ���̶�� ����
            isJump = true;
        }

        //yVelocity �� �߷¸�ŭ ���ҽ�Ű��
        yVelocity += gravity * Time.deltaTime;

        //yVelocity ���� dir �� y ���� ����
        dir.y = yVelocity;

        //�� Ctrl Ŭ���� �ٵ𽽷�

        //if (Input.GetKeyDown(KeyCode.LeftControl))
        //{
        //    //bool a;
        //    //�����̿� col �ְ� �װ� ���� �΋H���� �׶� ��ó Ž���ϰ�, ������ �ο� ����
        //    //
        //    if (gameObject.transform.position.y > 3)
        //    {
        //        anim.SetTrigger("BodySlam");
        //        //��� ���߰� ������ ��������. (�ִϸ��̼� ����)
        //        this.GetComponent<Rigidbody>().isKinematic = true;
        //        yVelocity = -20; //���� ���� ���� ���� �̻��ΰ� ������.
        //        //���� ���� �̻��϶� �ߵ��Ѵ�. (�ִϸ��̼����� ����)
        //        //���̿� ����ؼ� ���� ������ �þ�� (�����䶧 ������)
        //        // �ݰ� 3M ���� �浹ü�߿� ���ฦ ã�´�.
        //        int layerMask = (1 << witchLayer);
        //        Collider[] cols = Physics.OverlapSphere(transform.position, 3, layerMask);
        //        for (int i = 0; i < cols.Length; i++)
        //        {
        //            // �������� n ��ŭ �ְ�ʹ�.
        //            int n = 5;
        //            cols[i].GetComponent<PEA_WitchHP>().Damage(n);
        //        }
        //    }
        //   
        //
        //}


        //3. �׹������� ��������.
        //transform.position += dir * speed * Time.deltaTime;
        cc.Move(dir * speed * Time.deltaTime);

        
    }

    public void Mushroom()
    {
        // ������ ������ �ߵ� (�������� ����)        
        StartCoroutine(MushroomMove());        
    }
    IEnumerator MushroomMove()
    {
        float time = 0;
        float oneTime = 1;
        // ������ �÷��̾� �۵� ���ϰ��� (3��)
        while (time < 3)
        {
            // �������� �����̰� ��
            h = Random.Range(-1f, 1f);
            v = Random.Range(-1f, 1f);
            yield return new WaitForSeconds(oneTime);

            time += oneTime;

        }
        // �ٽ� �������� ��Ʈ�ѱ� �ο�.
        state = State.Move;

    }
    [PunRPC]
    void SetTriggerRpc(string parameter)
    {
        anim.SetTrigger(parameter);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //�� Player ���
        if (stream.IsWriting)
        {
            //���� ��ġ���� ������.
            stream.SendNext(transform.position);
            //���� ȸ������ ������.
            stream.SendNext(transform.rotation);
            stream.SendNext(hunterBody.rotation);
            //h �� ������.
            stream.SendNext(h);
            //v �� ������.
            stream.SendNext(v);
        }
        //�� Player �ƴ϶��
        else
        {
            //��ġ���� ����.
            receivePos = (Vector3)stream.ReceiveNext();
            //ȸ������ ����.
            receiveRot = (Quaternion)stream.ReceiveNext();
            receiveHunterRot = (Quaternion)stream.ReceiveNext();
            //h �� ����.
            h = (float)stream.ReceiveNext();
            //v �� ����.
            v = (float)stream.ReceiveNext();
        }
    }
}