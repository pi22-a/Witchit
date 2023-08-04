using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PJH_HunterFire : MonoBehaviour
{
    //���� ����
    public GameObject potatoFactory;
    //�� ����
    public GameObject chickenFactory;
    //��� ����

    //�߻� ��ġ
    public Transform firePosition;
    void Start()
    {
        
    }

    void Update()
    {
        //��Ŭ����
        if (Input.GetButtonDown("Fire1"))
        {           
            GameObject potato = Instantiate(potatoFactory);
            potato.transform.position = firePosition.position;
            potato.transform.forward = firePosition.forward;
        }
        //��Ŭ����
        if (Input.GetButtonDown("Fire2"))
        {
            GameObject chicken = Instantiate(chickenFactory);
            chicken.transform.position = firePosition.position;
            chicken.transform.forward = firePosition.forward;
        }
        //QŬ����
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject chicken = Instantiate(chickenFactory);
            chicken.transform.position = firePosition.position;
            chicken.transform.forward = firePosition.forward;
        }
        //�� Ctrl Ŭ����
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            GameObject chicken = Instantiate(chickenFactory);
            chicken.transform.position = firePosition.position;
            chicken.transform.forward = firePosition.forward;
        }
    }

   


}
