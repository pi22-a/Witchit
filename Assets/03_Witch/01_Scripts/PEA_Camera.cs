using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_Camera : MonoBehaviour
{
    // 카메라 이동에 관한 변수
    private float posLerpSpeed = 10f;

    // 플레이어 상태에 관한 변수
    private Vector3 defaultCamPos = Vector3.zero;                    // 마녀 모습일 때 각 축의 플레이어와 카메라의 거리
    private Vector3 probModeCamPos = Vector3.zero;                   // 프랍으로 변신했을 때 각 축의 플레이어와 카메라의 거리

    // 마우스 입력값 받을 변수
    private float mouseX = 0f;
    private float mouseY = 0f;

    // 에디터에서 연결해줄 변수
    public Transform player;
    public PEA_WitchSkill witchSkill;
    public PEA_WitchHP witchHP;

    void Start()
    {
        // 마녀일 때와 프랍으로 변신했을 때의 위치값 설정
        defaultCamPos = Camera.main.transform.localPosition;
        probModeCamPos = new Vector3(0, defaultCamPos.y, defaultCamPos.z);
    }

    void Update()
    {
        FollowPlayer();
        Rotate();
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    isChanged = !isChanged;
        //    SetCamPos();
        //    print(isChanged);
        //}
    }

    // 마녀 <-> 프랍 상태가 변하면 호출
    // 마녀의 화면상 위치를 조절
    public  void SetCamPos(bool isChanged)
    {
        if (!isChanged)
        {
            Camera.main.transform.localPosition = defaultCamPos;
            print(defaultCamPos);
        }
        else
        {
            Camera.main.transform.localPosition = probModeCamPos;
        }
    }

    private void FollowPlayer()
    {
        transform.position = Vector3.Lerp(transform.position, player.position, posLerpSpeed * Time.deltaTime);
    }

    private void Rotate()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        transform.localEulerAngles += new Vector3(-mouseY, mouseX, 0);
    }
}
