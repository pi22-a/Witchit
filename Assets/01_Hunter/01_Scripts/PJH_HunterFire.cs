using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PJH_HunterFire : MonoBehaviourPun
{
    //감자 공장
    public GameObject potatoFactory;    // 감자생성
    //닭 공장
    public GameObject chickenFactory;   // 닭생성
    //흡수 공장
    public GameObject vacuumTrapFactory; // 흡수생성
    //발사 위치
    public Transform firePosition;      // 감자/스킬이 나가는 위치
    //Witch 가져오기
    //public float range = 5;             // 바디슬램 범위
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
        //내가 만든 Player 가 아닐때
        if (photonView.IsMine == false)
        {
            //PlayerFire 컴포넌트를 비활성화
            this.enabled = false;
        }

        anim = GetComponentInChildren<Animator>();
        // Witch레이어 설정
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

       
        //좌클릭시 감자발사
        if (Input.GetButtonDown("Fire1"))
        {
            if (potatoGauge > 1000) //게이지가 100 이상일때 쏠수없음
            {
                print("쏠 수 없습니다." + potatoGauge);
            }
            else
            {
                //총쏘는 애니메이션 + 게이지 관리
                photonView.RPC(nameof(SetAnimTrigger), RpcTarget.All, "Fire");
                potatoGauge = potatoGauge + 150;
                images_Gauge.fillAmount += (float)0.15;

                //네트워킹
                Vector3 pos = firePosition.position;

                Vector3 forward = firePosition.forward;

                photonView.RPC(nameof(FirePotatoByRPC), RpcTarget.All, pos, forward);

                
            }
        }

        //Q클릭시 흡수 발사
        if (item1.CanDoIt() && Input.GetKeyDown(KeyCode.Q))
        {
            item1.UseSkill();

            photonView.RPC(nameof(SetAnimTrigger), RpcTarget.All, "Fire");
            //네트워킹
            Vector3 pos = firePosition.position;
            Vector3 forward = firePosition.forward;
            photonView.RPC(nameof(FireVacuumByRPC), RpcTarget.All, pos, forward);

        }

        //우클릭시 치킨발사
        if (item2.CanDoIt() && Input.GetButtonDown("Fire2"))
        {
            item2.UseSkill();

            photonView.RPC(nameof(SetAnimTrigger), RpcTarget.All, "Fire");           
           //네트워킹
           Vector3 pos = firePosition.position;
           Vector3 forward = firePosition.forward;
           photonView.RPC(nameof(FireChickenByRPC), RpcTarget.All, pos, forward);            
        }
       
        
        //V클릭시 앞범위 공격
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
