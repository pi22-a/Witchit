using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PJH_HunterFire : MonoBehaviour
{
    //��ź ����
    public GameObject bombFactory;
    //���� ����
    //public GameObject fragmentFactory;

    //�߻� ��ġ
    public Transform firePosition;
    void Start()
    {
        
    }

    void Update()
    {
        //1��Ű�� ������
        if (Input.GetButtonDown("Fire1"))
        {
            //FireBulletByInstantiate();


            GameObject potato = Instantiate(bombFactory);
            potato.transform.position = firePosition.position;
            potato.transform.forward = firePosition.forward;

            //��ź���忡�� ��ź�� �����

        }
    }

    void FireBulletByInstantiate()
    {
        //������� ��ź�� ī�޶� �չ������� 1��ŭ ������ ������ ���´�.
        Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * 1;

        //������� �Ѿ��� �չ����� ī�޶� ���� �������� ����
        Quaternion rot = Camera.main.transform.rotation;

       
    }


}
