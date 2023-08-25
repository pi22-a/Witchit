using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PJH_HunterFire : MonoBehaviourPun
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
    //public float range = 5;             // �ٵ𽽷� ����
    public ScrollRect scrollRectSkill;
    public ScrollRect scrollRectSkill1;

    public GameObject vacuumTrapUIFactory;
    public GameObject chickenUIFactory;

    private int potatoGauge = 0;
    [SerializeField]
    public Image images_Gauge;

    LayerMask witchLayer;

    Animator anim;
    PJH_SkillItem item1;
    PJH_SkillItem1 item2;

    private void Awake()
    {
        GameObject ui1 = Instantiate(vacuumTrapUIFactory, scrollRectSkill.content);
        item1 = ui1.GetComponent<PJH_SkillItem>();
        //ui1.transform.parent = scrollRectSkill.content;

        GameObject ui2 = Instantiate(chickenUIFactory, scrollRectSkill1.content);
        item2 = ui2.GetComponent<PJH_SkillItem1>();
        //ui2.transform.parent = scrollRectSkill.content;

    }
    void Start()
    {
        //���� ���� Player �� �ƴҶ�
        if (photonView.IsMine == false)
        {
            //PlayerFire ������Ʈ�� ��Ȱ��ȭ
            this.enabled = false;
        }

        anim = GetComponentInChildren<Animator>();
        // Witch���̾� ����
        witchLayer = LayerMask.NameToLayer("Witch");
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
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
                //�ѽ�� �ִϸ��̼� + ������ ����
                photonView.RPC(nameof(SetAnimTrigger), RpcTarget.All, "Fire");
                potatoGauge = potatoGauge + 150;
                images_Gauge.fillAmount += (float)0.15;

                //��Ʈ��ŷ
                Vector3 pos = firePosition.position;

                Vector3 forward = firePosition.forward;

                photonView.RPC(nameof(FirePotatoByRPC), RpcTarget.All, pos, forward);

                
            }
        }

        //QŬ���� ��� �߻�
        if (item1.CanDoIt() && Input.GetKeyDown(KeyCode.Q))
        {
            item1.UseSkill();

            photonView.RPC(nameof(SetAnimTrigger), RpcTarget.All, "Fire");
            //��Ʈ��ŷ
            Vector3 pos = firePosition.position;
            Vector3 forward = firePosition.forward;
            photonView.RPC(nameof(FireVacuumByRPC), RpcTarget.All, pos, forward);

        }

        //��Ŭ���� ġŲ�߻�
        if (item2.CanDoIt() && Input.GetButtonDown("Fire2"))
        {
            item2.UseSkill();

            photonView.RPC(nameof(SetAnimTrigger), RpcTarget.All, "Fire");           
           //��Ʈ��ŷ
           Vector3 pos = firePosition.position;
           Vector3 forward = firePosition.forward;
           photonView.RPC(nameof(FireChickenByRPC), RpcTarget.All, pos, forward);            
        }
       
        
        //VŬ���� �չ��� ����
        if (Input.GetKeyDown(KeyCode.V))
        {
            anim.SetTrigger("MeleeAttack");
            
        }
    }

    [PunRPC]
    private void SetAnimTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    [PunRPC]
    void FirePotatoByRPC(Vector3 firePos, Vector3 fireFoward)
    {
        GameObject potato = Instantiate(potatoFactory);
        potato.transform.position = firePos;
        potato.transform.forward = fireFoward;
    }

    [PunRPC]
    void FireChickenByRPC(Vector3 firePos, Vector3 fireFoward)
    {
        GameObject chicken = Instantiate(chickenFactory);
        chicken.transform.position = firePos;
        chicken.transform.forward = fireFoward;
    }

    [PunRPC]
    void FireVacuumByRPC(Vector3 firePos, Vector3 fireFoward)
    {
        GameObject vacuum = Instantiate(vacuumTrapFactory);
        vacuum.transform.position = firePos;
        vacuum.transform.forward = fireFoward;
    }

    public void HitWitch(int n)
    {
        //Witch.GetComponent<PEA_WitchHP>().Damage(n);
    }

   


}
