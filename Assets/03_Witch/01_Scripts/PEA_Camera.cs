using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_Camera : MonoBehaviour
{
    // ī�޶� �̵��� ���� ����
    private float posLerpSpeed = 10f;

    // ī�޷� ȸ�� ���� ����
    private float mouseX = 0f;
    private float mouseY = 0f;
    private readonly float rotMinY = 30f;
    private readonly float rotMaxY = -30f;

    // �÷��̾� ���¿� ���� ����
    private Vector3 defaultCamPos = new Vector3(1.5f, 3f, -5f);                    // ���� ����� �� �� ���� �÷��̾�� ī�޶��� �Ÿ�
    private Vector3 probModeCamPos = new Vector3(0f, 3f, -5f);                     // �������� �������� �� �� ���� �÷��̾�� ī�޶��� �Ÿ�

    // �����Ϳ��� �������� ����
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

    // ���� <-> ���� ���°� ���ϸ� ȣ��
    // ������ ȭ��� ��ġ�� ����
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
