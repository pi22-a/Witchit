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
    public GameObject vacuumTrapFactory; // �������
    //�߻� ��ġ
    public Transform firePosition;      // ����/��ų�� ������ ��ġ
    //Witch ��������
    public float range = 5;             // �ٵ𽽷� ����

    private int potatoGauge = 0;

    LayerMask witchLayer;

    private void Awake()
    {

        
    }

    public enum State
    {
        Idle,
        Run,
        Attack,
        Jump,
        MeleeAttack,
    }
    void Start()
    {
        // Witch���̾� ����
        witchLayer = LayerMask.NameToLayer("Witch");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        StartCoroutine(PotatoGauge());
    }

    IEnumerator PotatoGauge()
    {
        while (true)
        {
            Potato();
            yield return new WaitForSecondsRealtime(0.01f);
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
                potatoGauge = potatoGauge + 100;
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
        
        //VŬ���� ������ ����
        if (Input.GetKeyDown(KeyCode.V))
        {
            
        }
    }

    public void HitWitch(int n)
    {
        //Witch.GetComponent<PEA_WitchHP>().Damage(n);
    }

   


}
