using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PJH_HunterRot : MonoBehaviourPun
{
    //누적된 회전 값
    float rotX;
    float rotY;

    //회전 속력
    float rotSpeed = 200;

    //카메라 Transform
    public GameObject trCam;

    public GameObject trCam1;

    public GameObject trCam2;


    public GameObject trSpine;
    void Start()
    {
        //내가 생성한 Player 일때만 카메라를 활성화 하자
        if (photonView.IsMine)
        {
            //trCam.gameObject.SetActive(true);
            trCam.SetActive(true);
        }
    }
    private void Update()
    {
        //내것이 아닐때 함수를 나가자
        if (photonView.IsMine == false) return;

        //만약에 마우스 커서가 활성화 되어 있으면 함수를 나가자
        if (Cursor.visible == true) return;

        // 카메라 1인칭 / 3인칭 변환
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
    }

    // Update is called once per frame
    void LateUpdate()
    {
        


        //마우스의 움직임따라 플레이어를 좌우 회전하고
        //카메라를 위아래 회전하고 싶다.

        //1. 마우스 입력을 받자.    
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        //2. 마우스의 움직임 값을 누적
        rotX += mx * rotSpeed * Time.deltaTime;
        rotY += my * rotSpeed * Time.deltaTime;

        rotY = Mathf.Clamp(rotY, -55, 55);

        //3. 누적된 값만큼 회전 시키자.
        transform.localEulerAngles = new Vector3(0, rotX, 0);
        trSpine.transform.localEulerAngles = new Vector3 (
            trSpine.transform.localEulerAngles.x, trSpine.transform.localEulerAngles.y, rotY);
        trCam.transform.localEulerAngles = new Vector3(-rotY ,0,0);


    }

   
}
