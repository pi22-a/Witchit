using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_HunterRot : MonoBehaviour
{
    //누적된 회전 값
    float rotX;
    float rotY;

    //회전 속력
    float rotSpeed = 200;

    //카메라 Transform
    public Transform trCam;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        


        //마우스의 움직임따라 플레이를 좌우 회전하고
        //카메라를 위아래 회전하고 싶다.

        //1. 마우스 입력을 받자.
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        //2. 마우스의 움직임 값을 누적
        rotX += mx * rotSpeed * Time.deltaTime;
        rotY += my * rotSpeed * Time.deltaTime;

        //3. 누적된 값만큼 회전 시키자.
        transform.localEulerAngles = new Vector3(0, rotX, 0);
        trCam.localEulerAngles = new Vector3(-rotY, 0, 0);
    }
}
