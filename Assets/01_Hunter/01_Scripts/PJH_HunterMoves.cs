using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������� �Է¿����� �յ��¿�� �̵��ϰ�ʹ�.
// ����ڰ� ������ư�� ������ ������ �ٰ�ʹ�.
// �ִ� ���� Ƚ���� ���ؼ� ������ �����ϰ� �ϰ�ʹ�.
public class PJH_HunterMoves : MonoBehaviour
{
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
        // Witch���̾� ����
        witchLayer = LayerMask.NameToLayer("Witch");
        //Character Controller ��������
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
        //W, S, A, D Ű�� ������ �յ��¿�� �����̰� �ʹ�.

        //1. ������� �Է��� ����.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
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
            this.GetComponent<Rigidbody>().isKinematic = false;
        }
        
        //�����̹ٸ� ������ ������ �ϰ� �ʹ�.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //yVelocity �� jumpPower �� ����
            yVelocity = jumpPower;
        }
        
        //yVelocity �� �߷¸�ŭ ���ҽ�Ű��
        yVelocity += gravity * Time.deltaTime;
        //�� Ctrl Ŭ���� �ٵ𽽷�
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
        print("11");
            //if(yVelocity > 2) 
            {
                //��� ���߰� ������ ��������. (�ִϸ��̼� ����)
                this.GetComponent<Rigidbody>().isKinematic = true;
                yVelocity = 2 * yVelocity; //���� ���� ���� ���� �̻��ΰ� ������.
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
        
    }

    
}