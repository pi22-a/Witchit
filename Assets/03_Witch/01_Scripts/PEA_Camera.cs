using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_Camera : MonoBehaviour
{
    // ī�޶� �̵��� ���� ����
    private float posLerpSpeed = 10f;

    // �÷��̾� ���¿� ���� ����
    private bool isChanged = false;                                  // �����ϸ� ȭ��� ������ ��ġ�� �޶���
    private Vector3 defaultCamPos = Vector3.zero;                    // ���� ����� �� �� ���� �÷��̾�� ī�޶��� �Ÿ�
    private Vector3 probModeCamPos = Vector3.zero;                   // �������� �������� �� �� ���� �÷��̾�� ī�޶��� �Ÿ�

    // ���콺 �Է°� ���� ����
    private float mouseX = 0f;
    private float mouseY = 0f;

    // �����Ϳ��� �������� ����
    public Transform player;

    void Start()
    {
        // ������ ���� �������� �������� ���� ��ġ�� ����
        defaultCamPos = transform.localPosition - player.transform.position;
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
    private void SetCamPos()
    {
        if (!isChanged)
        {
            Camera.main.transform.localPosition = defaultCamPos; 
        }
        else
        {
            Camera.main.transform.localPosition = probModeCamPos;
        }
    }

    private void FollowPlayer()
    {
        if (!isChanged)
        {
            //Vector3 fixedPos = player.position + defaultCamPos;
            transform.position = Vector3.Lerp(transform.position, player.position, posLerpSpeed * Time.deltaTime);
        }
    }

    private void Rotate()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");

        transform.localEulerAngles += new Vector3(-mouseY, mouseX, 0);
    }
}
