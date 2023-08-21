using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PJH_HunterRot : MonoBehaviourPun, IPunObservable
{
    //������ ȸ�� ��
    float rotX;
    float rotY;

    //ȸ�� �ӷ�
    float rotSpeed = 200;

    //ī�޶� Transform
    public GameObject trCam;

    public GameObject trCam1;

    public GameObject trCam2;


    public GameObject trSpine;
    void Start()
    {
        //���� ������ Player �϶��� ī�޶� Ȱ��ȭ ����
        if (photonView.IsMine)
        {
            //trCam.gameObject.SetActive(true);
            trCam.SetActive(true);
        }
    }
    private void Update()
    {
        //������ �ƴҶ� �Լ��� ������
        if (photonView.IsMine == false) return;

        //���࿡ ���콺 Ŀ���� Ȱ��ȭ �Ǿ� ������ �Լ��� ������
        //if (Cursor.visible == true) return;

        // ī�޶� 1��Ī / 3��Ī ��ȯ
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (trCam1.activeSelf == false)
            {
                trCam2.SetActive(false);
                trCam1.SetActive(true);
            }
            else if (trCam1.activeSelf == true)
            {
                trCam2.SetActive(true);
                trCam1.SetActive(false);
            }
        }
        //1. ���콺 �Է��� ����.    
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        //2. ���콺�� ������ ���� ����
        rotX += mx * rotSpeed * Time.deltaTime;
        rotY += my * rotSpeed * Time.deltaTime;

        rotY = Mathf.Clamp(rotY, -55, 55);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.localEulerAngles = new Vector3(0, rotX, 0);
        trSpine.transform.localEulerAngles = new Vector3(trSpine.transform.localEulerAngles.x, trSpine.transform.localEulerAngles.y, rotY);
        trCam.transform.localEulerAngles = new Vector3(-rotY, 0, 0);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rotX);
            stream.SendNext(rotY);
            stream.SendNext(transform.localEulerAngles);
            stream.SendNext(trSpine.transform.localEulerAngles);
            stream.SendNext(trCam.transform.localEulerAngles);
        }
        else if (stream.IsReading)
        {
            rotX = (float)stream.ReceiveNext();
            rotY = (float)stream.ReceiveNext();
        }
    }
}
