using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_HunterRot : MonoBehaviour
{
    //������ ȸ�� ��
    float rotX;
    float rotY;

    //ȸ�� �ӷ�
    float rotSpeed = 200;

    //ī�޶� Transform
    public Transform trCam;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        


        //���콺�� �����ӵ��� �÷��̸� �¿� ȸ���ϰ�
        //ī�޶� ���Ʒ� ȸ���ϰ� �ʹ�.

        //1. ���콺 �Է��� ����.
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        //2. ���콺�� ������ ���� ����
        rotX += mx * rotSpeed * Time.deltaTime;
        rotY += my * rotSpeed * Time.deltaTime;

        //3. ������ ����ŭ ȸ�� ��Ű��.
        transform.localEulerAngles = new Vector3(0, rotX, 0);
        trCam.localEulerAngles = new Vector3(-rotY, 0, 0);
    }
}
