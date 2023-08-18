using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_Camera : MonoBehaviour
{
    // 카메라 이동에 관한 변수
    private float posLerpSpeed = 10f;

    // 카메러 회전 관련 변수
    private float mouseX = 0f;
    private float mouseY = 0f;
    private readonly float rotMinY = 30f;
    private readonly float rotMaxY = -30f;

    // 플레이어 상태에 관한 변수
    private Vector3 defaultCamPos = new Vector3(1.5f, 3f, -5f);                    // 마녀 모습일 때 각 축의 플레이어와 카메라의 거리
    private Vector3 probModeCamPos = new Vector3(0f, 3f, -5f);                     // 프랍으로 변신했을 때 각 축의 플레이어와 카메라의 거리

    // 에디터에서 연결해줄 변수
    public Transform player;
    public PEA_WitchHP witchHP;
    public PEA_WitchSkill witchSkill;

    void Start()
    {
        Camera.main.transform.localPosition = defaultCamPos;
        Camera.main.transform.localEulerAngles = new Vector3(15f, 0f, 0f);
    }

    void Update()
    {
        if(player != null)
        {
            FollowPlayer();
            Rotate();
        }
    }

    public void SetPlayer(Transform playerTr)
    {
        player = playerTr;
        witchHP = player.GetComponent<PEA_WitchHP>();
        witchSkill = player.GetComponent<PEA_WitchSkill>();
        witchSkill.SetPeaCamera(this);
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

        //Vector3 camEuler = transform.localRotation.eulerAngles + new Vector3(-mouseY, mouseX, 0);
        ////print(camEuler.x);
        //camEuler.x = Mathf.Clamp(camEuler.x, rotMinY, rotMaxY);
        //transform.localRotation = Quaternion.Euler(camEuler);
    }
}
