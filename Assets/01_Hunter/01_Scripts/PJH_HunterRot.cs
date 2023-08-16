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
    public GameObject trCam;

    public GameObject trCam1;

    public GameObject trSpine;
    void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (trCam1.activeSelf == false)
            {
                //trCam.SetActive(false);
                trCam1.SetActive(true);
            }
            else if (trCam.activeSelf == true)
            {
                //trCam.SetActive(true);
                trCam1.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        


        //���콺�� �����ӵ��� �÷��̾ �¿� ȸ���ϰ�
        //ī�޶� ���Ʒ� ȸ���ϰ� �ʹ�.

        //1. ���콺 �Է��� ����.    
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        //2. ���콺�� ������ ���� ����
        rotX += mx * rotSpeed * Time.deltaTime;
        rotY += my * rotSpeed * Time.deltaTime;

        rotY = Mathf.Clamp(rotY, -55, 55);

        //3. ������ ����ŭ ȸ�� ��Ű��.
        transform.localEulerAngles = new Vector3(0, rotX, 0);
        trSpine.transform.localEulerAngles = new Vector3 (
            trSpine.transform.localEulerAngles.x, trSpine.transform.localEulerAngles.y, rotY);
        trCam.transform.localEulerAngles = new Vector3(-rotY ,0,0);


    }

   
}
