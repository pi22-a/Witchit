using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_VacuumTrap : MonoBehaviour
{
    public GameObject VacuumFactory;    //흡수 이펙트    
    public float speed = 15;            //흡수 스피드
    public float deathTime = 8;         //흡수 수명
    public float range = 5;             //흡수 범위

    Rigidbody rb;

    bool b;
    LayerMask witchLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        // Witch레이어 설정
        witchLayer = LayerMask.NameToLayer("Witch");
        // deathTime초 후에 흡수 파괴
        Invoke("DeathVacuumTrap", deathTime); 
        // 흡수하는 함수 호출
        Invoke("FindWitch",1);
    }

    // Update is called once per frame
    void Update()
    {
    
        
    }
    void DeathVacuumTrap()
    {
        Destroy(gameObject);
    }

    void FindWitch()
    {
        bool isWitch = false;
        // 반경 3M 안의 충돌체중에 마녀가 있다면
        int layer = 1 << LayerMask.NameToLayer("Witch");
        Collider[] cols = Physics.OverlapSphere(transform.position, 3, layer);
        if (cols.Length > 0)
        {
            // 마녀가 있다.
            isWitch = true;
            
        }

        if (isWitch)
        {

            GameObject Vacuum = Instantiate(VacuumFactory);
            Vacuum.transform.position = transform.position;
            // 마녀를 흡수 위치로 당긴다.
            for (int i = 0; i <cols.Length; i++)
            {
                cols[0].transform.position = Vector3.MoveTowards(cols[0].transform.position, gameObject.transform.position, 0.1f);
            }
            
        }
    }
}
