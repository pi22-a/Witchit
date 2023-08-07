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
    public GameObject vacuumTrapFactory; 
    //발사 위치
    public Transform firePosition;      // 감자/스킬이 나가는 위치
    //Witch 가져오기
    public GameObject Witch;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //좌클릭시
        if (Input.GetButtonDown("Fire1"))
        {           
            GameObject potato = Instantiate(potatoFactory);
            potato.transform.position = firePosition.position;
            potato.transform.forward = firePosition.forward;
        }
        //우클릭시
        if (Input.GetButtonDown("Fire2"))
        {
            GameObject chicken = Instantiate(chickenFactory);
            chicken.transform.position = firePosition.position;
            chicken.transform.forward = firePosition.forward;
        }
        //Q클릭시
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject vacuumTrap = Instantiate(vacuumTrapFactory);
            vacuumTrap.transform.position = firePosition.position;
            vacuumTrap.transform.forward = firePosition.forward;
        }
        //좌 Ctrl 클릭시
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            //잠깐 멈추고
            //빠르게 떨어진다.
            //일정 높이 이상일때 발동한다.
            //높이에 비례해서 공격 범위가 늘어난다 (프로토때 미적용)
            // 반경 3M 안의 충돌체중에 마녀가 있다면
            int layer = 1 << LayerMask.NameToLayer("Witch");
            
            Collider[] cols = Physics.OverlapSphere(transform.position, 3, layer);
            for (int i = 0; i < cols.Length; i++)
            {
                // 데미지를 n 만큼 주고싶다.
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
