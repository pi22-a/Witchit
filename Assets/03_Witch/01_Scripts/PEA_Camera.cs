using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_Camera : MonoBehaviour
{
    // ī�޶� �̵��� ���� ����
    private float posLerpSpeed = 10f;

    // �÷��̾� ���¿� ���� ����
    private Vector3 defaultCamPos = Vector3.zero;                    // ���� ����� �� �� ���� �÷��̾�� ī�޶��� �Ÿ�
    private Vector3 probModeCamPos = Vector3.zero;                   // �������� �������� �� �� ���� �÷��̾�� ī�޶��� �Ÿ�

    // ���콺 �Է°� ���� ����
    private float mouseX = 0f;
    private float mouseY = 0f;

    // �����Ϳ��� �������� ����
    public Transform player;
    public PEA_WitchSkill witchSkill;
    public PEA_WitchHP witchHP;

    void Start()
    {
        // ������ ���� �������� �������� ���� ��ġ�� ����
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
    }
}
