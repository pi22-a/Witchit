using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_Bullet : MonoBehaviour
{
    //�Ѿ� �ӷ�
    public float speed = 20;

    //����ȿ������
    //public GameObject exploFactory;

    void Start()
    {

    }

    void Update()
    {
        //��� ������ ���� �ʹ�.
        transform.position += transform.forward * speed * Time.deltaTime;

    }

    private void OnTriggerEnter(Collider other)
    {
        //����ȿ�����忡�� ����ȿ���� ������.
        //GameObject explo = Instantiate(exploFactory);
        //���� ȿ���� ���� ��ġ�� ����.
        //explo.transform.position = transform.position;
        //���� ȿ������ ParticleSystem �� ��������.
        //ParticleSystem ps = explo.GetComponent<ParticleSystem>();
        //������ ParticleSystem �� ����� Play �� ���� ����.
        //ps.Play();
        //2�ʵڿ� explo �� �ı�����.
        //Destroy(explo, 2);

        //���� �ı�����
        Destroy(gameObject);

        ////���� �� �Ѿ˸� 
        //if (photonView.IsMine)
        //{
        //    //���� �ı�����
        //    PhotonNetwork.Destroy(gameObject);
        //}
    }
}


