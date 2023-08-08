using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            potatoGauge -= 1;
            print(potatoGauge);
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
                GameObject potato = Instantiate(potatoFactory);
                potato.transform.position = firePosition.position;
                potato.transform.forward = firePosition.forward;
                potatoGauge = potatoGauge + 100;
            }
            print(potatoGauge);
        }
        //우클릭시 치킨발사
        if (Input.GetButtonDown("Fire2"))
        {
            GameObject chicken = Instantiate(chickenFactory);
            chicken.transform.position = firePosition.position;
            chicken.transform.forward = firePosition.forward;
        }
        //Q클릭시 흡수 발사
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject vacuumTrap = Instantiate(vacuumTrapFactory);
            vacuumTrap.transform.position = firePosition.position;
            vacuumTrap.transform.forward = firePosition.forward;
        }
        //좌 Ctrl 클릭시 바디슬램
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            //잠깐 멈추고 빠르게 떨어진다. (애니메이션 적용)
            //일정 높이 이상일때 발동한다. (애니메이션으로 구별)
            //높이에 비례해서 공격 범위가 늘어난다 (프로토때 미적용)
            Debug.Log("1111");
            // 반경 3M 안의 충돌체중에 마녀를 찾는다.
            int layerMask = (1 << witchLayer);
            Collider[] cols = Physics.OverlapSphere(transform.position, range, layerMask);
            for (int i = 0; i < cols.Length; i++)
            {
                // 데미지를 n 만큼 주고싶다.
                int n = 5;
                cols[i].GetComponent<PEA_WitchHP>().Damage(n);
            }
        }
        //V클릭시 전범위 공격
        if (Input.GetKeyDown(KeyCode.V))
        {
            
        }
    }

    public void HitWitch(int n)
    {
        //Witch.GetComponent<PEA_WitchHP>().Damage(n);
    }

   


}
