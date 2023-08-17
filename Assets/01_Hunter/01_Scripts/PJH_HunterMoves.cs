using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������� �Է¿����� �յ��¿�� �̵��ϰ�ʹ�.
// ����ڰ� ������ư�� ������ ������ �ٰ�ʹ�.
// �ִ� ���� Ƚ���� ���ؼ� ������ �����ϰ� �ϰ�ʹ�.
public class PJH_HunterMoves : MonoBehaviour
{
    enum State
    {
        Move,
        Confusion,  // ȥ��
    }

    State state;


    //�ӷ� 
    float speed = 5;

    //Character Controller ���� ����
    CharacterController cc;

    //���� �Ŀ�
    float jumpPower = 5;
    //�߷�
    float gravity = -9.81f;
    //y �ӷ�
    float yVelocity = 0;

    Animator anim;

    LayerMask witchLayer;
    //�������� �Ѿ���� ��ġ��
    //Vector3 receivePos;
    //�������� �Ѿ���� ȸ����
    //Quaternion receiveRot = Quaternion.identity;
    //�����ϴ� �ӷ�
    //float lerpSpeed = 50;


    void Start()
    {
        // Witch���̾� ����
        witchLayer = LayerMask.NameToLayer("Witch");
        //Character Controller ��������
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

        anim.SetFloat("Horizontal", h);
        anim.SetFloat("Vertical", v);

        //���࿡ ���� ����ִٸ�
        if (cc.isGrounded == true)
        {
            //yVeloctiy �� 0 ���� ����
            yVelocity = 0;
            this.GetComponent<Rigidbody>().isKinematic = false;
        }

        //�����̹ٸ� ������ ������ �ϰ� �ʹ�.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("Jump");
            //yVelocity �� jumpPower �� ����
            yVelocity = jumpPower;
        }

        //yVelocity �� �߷¸�ŭ ���ҽ�Ű��
        yVelocity += gravity * Time.deltaTime;
        //�� Ctrl Ŭ���� �ٵ𽽷�
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            //bool a;
            //�����̿� col �ְ� �װ� ���� �΋H���� �׶� ��ó Ž���ϰ�, ������ �ο� ����
            //
            if (gameObject.transform.position.y > 3)
            {
                anim.SetTrigger("BodySlam");
                //��� ���߰� ������ ��������. (�ִϸ��̼� ����)
                this.GetComponent<Rigidbody>().isKinematic = true;
                yVelocity = -20; //���� ���� ���� ���� �̻��ΰ� ������.
                //���� ���� �̻��϶� �ߵ��Ѵ�. (�ִϸ��̼����� ����)
                //���̿� ����ؼ� ���� ������ �þ�� (�����䶧 ������)
                // �ݰ� 3M ���� �浹ü�߿� ���ฦ ã�´�.
                int layerMask = (1 << witchLayer);
                Collider[] cols = Physics.OverlapSphere(transform.position, 3, layerMask);
                for (int i = 0; i < cols.Length; i++)
                {
                    // �������� n ��ŭ �ְ�ʹ�.
                    int n = 5;
                    cols[i].GetComponent<PEA_WitchHP>().Damage(n);
                }
            }

        }



        //yVelocity ���� dir �� y ���� ����
        dir.y = yVelocity;

        //3. �׹������� ��������.
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
        // ������ ������ �ߵ� (�������� ����)
        // ������ �÷��̾� �۵� ���ϰ��� (3��)

        // �������� �����̰� ��
        StartCoroutine(MushroomMove());
        // �ٽ� �������� ��Ʈ�ѱ� �ο�.
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