using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]
    public Image images_Gauge;

    LayerMask witchLayer;

    Animator anim;
    private void Awake()
    {

        
    }

    void Start()
    {

        anim = GetComponentInChildren<Animator>();
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
            potatoGauge -= 4;
            //print(potatoGauge);
            images_Gauge.fillAmount -= (float)0.004;
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
                anim.SetTrigger("Fire");
                GameObject potato = Instantiate(potatoFactory);
                potato.transform.position = firePosition.position;
                potato.transform.forward = firePosition.forward;
                potatoGauge = potatoGauge + 150;
                images_Gauge.fillAmount += (float)0.15;
            }
            //print(potatoGauge);
        }
        //��Ŭ���� ġŲ�߻�
        if (Input.GetButtonDown("Fire2"))
        {
            anim.SetTrigger("Fire");
            GameObject chicken = Instantiate(chickenFactory);
            chicken.transform.position = firePosition.position;
            chicken.transform.forward = firePosition.forward;
        }
        //QŬ���� ��� �߻�
        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.SetTrigger("Fire");
            GameObject vacuumTrap = Instantiate(vacuumTrapFactory);
            vacuumTrap.transform.position = firePosition.position;
            vacuumTrap.transform.forward = firePosition.forward;
        }
        
        //VŬ���� �չ��� ����
        if (Input.GetKeyDown(KeyCode.V))
        {
            anim.SetTrigger("MeleeAttack");
            
        }
    }
    

    public void HitWitch(int n)
    {
        //Witch.GetComponent<PEA_WitchHP>().Damage(n);
    }

   


}
