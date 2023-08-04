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

    //�������� �Ѿ���� ��ġ��
    //Vector3 receivePos;
    //�������� �Ѿ���� ȸ����
    //Quaternion receiveRot = Quaternion.identity;
    //�����ϴ� �ӷ�
    //float lerpSpeed = 50;

    void Start()
    {
        //Character Controller ��������
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        
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
            }

            //�����̹ٸ� ������ ������ �ϰ� �ʹ�.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //yVelocity �� jumpPower �� ����
                yVelocity = jumpPower;
            }

            //yVelocity �� �߷¸�ŭ ���ҽ�Ű��
            yVelocity += gravity * Time.deltaTime;

            //yVelocity ���� dir �� y ���� ����
            dir.y = yVelocity;

            //3. �׹������� ��������.
            //transform.position += dir * speed * Time.deltaTime;
            cc.Move(dir * speed * Time.deltaTime);
        
    }

    
}