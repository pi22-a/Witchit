using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_HunterRot : MonoBehaviour
{
    float rx;
    float ry;
    public float rotSpeed = 200;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 1. ������� �Է¿�����
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");
        // 2. X�� Y���� ���� �����ϰ�
        rx += my * rotSpeed * Time.deltaTime;
        ry += mx * rotSpeed * Time.deltaTime;
        // 3. rx������ ������ �����ϰ�ʹ�.
        rx = Mathf.Clamp(rx, -75, 75);
        // 4. �� ���������� ȸ���� �ϰ�ʹ�.
        transform.eulerAngles = new Vector3(-rx, ry, 0);
    }
}
