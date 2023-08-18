using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PJH_HunterFire : MonoBehaviour
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
    public float range = 5;             // 바디슬램 범위

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
        // Witch레이어 설정
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

       
        //좌클릭시 감자발사
        if (Input.GetButtonDown("Fire1"))
        {
            if (potatoGauge > 1000) //게이지가 100 이상일때 쏠수없음
            {
                print("쏠 수 없습니다." + potatoGauge);
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
        //우클릭시 치킨발사
        if (Input.GetButtonDown("Fire2"))
        {
            anim.SetTrigger("Fire");
            GameObject chicken = Instantiate(chickenFactory);
            chicken.transform.position = firePosition.position;
            chicken.transform.forward = firePosition.forward;
        }
        //Q클릭시 흡수 발사
        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.SetTrigger("Fire");
            GameObject vacuumTrap = Instantiate(vacuumTrapFactory);
            vacuumTrap.transform.position = firePosition.position;
            vacuumTrap.transform.forward = firePosition.forward;
        }
        
        //V클릭시 앞범위 공격
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
