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

    private int potatoGauge = 0;

    private void Awake()
    {

        
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(PotatoGauge());
    }

    IEnumerator PotatoGauge()
    {
        while (true)
        {
            Potato();
            yield return new WaitForSecondsRealtime(0.05f);
        }
        
    }
    void Potato()
    {
        if (potatoGauge > 0)
        {
            potatoGauge -= 1;
            print(potatoGauge);
        }
       
    }



    void Update()
    {
       
        //��Ŭ���� ���ڹ߻�
        if (Input.GetButtonDown("Fire1"))
        {
            if (potatoGauge > 1000) //�������� 100 �̻��϶� �������
            {
                print("�� �� �����ϴ�." + potatoGauge);
            }
            else
            {
                GameObject potato = Instantiate(potatoFactory);
                potato.transform.position = firePosition.position;
                potato.transform.forward = firePosition.forward;
                potatoGauge = potatoGauge + 50;
            }
            print(potatoGauge);
        }
        //��Ŭ���� ġŲ�߻�
        if (Input.GetButtonDown("Fire2"))
        {
            GameObject chicken = Instantiate(chickenFactory);
            chicken.transform.position = firePosition.position;
            chicken.transform.forward = firePosition.forward;
        }
        //QŬ���� ��� �߻�
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject vacuumTrap = Instantiate(vacuumTrapFactory);
            vacuumTrap.transform.position = firePosition.position;
            vacuumTrap.transform.forward = firePosition.forward;
        }
        //�� Ctrl Ŭ���� �ٵ𽽷�
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
                HitWitch(n);
            }
        }
    }

    public void HitWitch(int n)
    {
        Witch.GetComponent<PEA_WitchHP>().Damage(n);
    }

   


}
