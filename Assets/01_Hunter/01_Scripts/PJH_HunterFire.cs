using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PJH_HunterFire : MonoBehaviour
{
    //���� ����
    public GameObject potatoFactory;    // ���ڻ���
    //�� ����
    public GameObject chickenFactory;   // �߻���
    //��� ����
    public GameObject vacuumTrapFactory; 
    //�߻� ��ġ
    public Transform firePosition;      // ����/��ų�� ������ ��ġ
    //Witch ��������
    public GameObject Witch;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
            GameObject vacuumTrap = Instantiate(vacuumTrapFactory);
            vacuumTrap.transform.position = firePosition.position;
            vacuumTrap.transform.forward = firePosition.forward;
        }
        //�� Ctrl Ŭ����
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            //��� ���߰�
            //������ ��������.
            //���� ���� �̻��϶� �ߵ��Ѵ�.
            //���̿� ����ؼ� ���� ������ �þ�� (�����䶧 ������)
            // �ݰ� 3M ���� �浹ü�߿� ���డ �ִٸ�
            int layer = 1 << LayerMask.NameToLayer("Witch");
            
            Collider[] cols = Physics.OverlapSphere(transform.position, 3, layer);
            for (int i = 0; i < cols.Length; i++)
            {
                // �������� n ��ŭ �ְ�ʹ�.
                int n = 5;
                Witch.GetComponent<PEA_WitchHP>().Damage(n);
            }
        }
    }

    public void HitWitch()
    {
        Witch.GetComponent<PEA_WitchHP>().Damage(1);
    }

   


}
