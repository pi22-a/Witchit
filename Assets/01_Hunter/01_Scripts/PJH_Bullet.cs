using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PJH_Bullet : MonoBehaviourPun
{
    //�Ѿ� �ӷ�
    public float speed = 20;


    void Start()
    {
        /*
        //���� ���� Player �� �ƴҶ�
        if (photonView.IsMine == false)
        {
            //PlayerFire ������Ʈ�� ��Ȱ��ȭ
            this.enabled = false;
        }*/
    }

    void Update()
    {
        //��� ������ ���� �ʹ�.
        transform.position += transform.forward * speed * Time.deltaTime;

    }

    
    private void OnTriggerEnter(Collider other)
    {
        // tag�� Witch�϶�
        if (other.transform.CompareTag("Witch"))
        {
            // �������� n ��ŭ �ְ�ʹ�.
            int n = 5;
            other.transform.parent.parent.GetComponent<PEA_WitchHP>().Damage(n);
            print("�������� �־����ϴ�.");
        }
        /*
        //���� �� �Ѿ˸� 
        if (photonView.IsMine)
        {
            //���� �ı�����
            PhotonNetwork.Destroy(gameObject);
        }*/
        Destroy(gameObject);
    }
}


