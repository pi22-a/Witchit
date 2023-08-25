using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_Camera : MonoBehaviour
{
    // ī�޶� �̵��� ���� ����
    private float posLerpSpeed = 10f;

    // ī�޷� ȸ�� ���� ����
    private float mx = 0f;
    private float my = 0f;
    private float mouseX = 0f;
    private float mouseY = 0f;
    private readonly float rotMinY = -45f;
    private readonly float rotMaxY = 45f;
    private readonly float rotSpeed = 150f;

    // �÷��̾� ���¿� ���� ����
    private Vector3 defaultCamPos = new Vector3(1.5f, 3f, -5f);                    // ���� ����� �� �� ���� �÷��̾�� ī�޶��� �Ÿ�
    private Vector3 probModeCamPos = new Vector3(0f, 3f, -5f);                     // �������� �������� �� �� ���� �÷��̾�� ī�޶��� �Ÿ�

    private PEA_Grayscale grayScale = null;
    private float graySpeed = 5f;

    // �����Ϳ��� �������� ����
    public Transform player;
    public PEA_WitchHP witchHP;
    public PEA_WitchSkill witchSkill;

    void Start()
    {
        Camera.main.transform.localPosition = defaultCamPos;
        Camera.main.transform.localEulerAngles = new Vector3(15f, 0f, 0f);
        grayScale =  Camera.main.GetComponent<PEA_Grayscale>();
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

        mx += mouseX * rotSpeed * Time.deltaTime;
        my += mouseY * rotSpeed * Time.deltaTime;

        my = Mathf.Clamp(my, rotMinY, rotMaxY);

        transform.rotation = Quaternion.Euler(new Vector3(-my, mx, 0));
    }

    public void Die()
    {
        StartCoroutine(IDie());
    }

    IEnumerator IDie()
    {
        while(grayScale.grayscale < 1)
        {
            grayScale.grayscale += graySpeed * Time.deltaTime;
            yield return  new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(2f);

        while(grayScale.grayscale > 0)
        {
            grayScale.grayscale -= graySpeed * Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        Camera.main.gameObject.AddComponent<PEA_WatchCamera>();
        yield return null;
    }
}
