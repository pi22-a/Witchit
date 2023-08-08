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
        StartCoroutine(FindWitch());
    }

    // Update is called once per frame
    void Update()
    {
        if(b)
        {
            // 반경 range 안의 충돌체중에 마녀를 찾는다.
            int layerMask = (1 << witchLayer);
            Collider[] cols = Physics.OverlapSphere(transform.position, range, layerMask);
            for (int i=0; i < cols.Length; i++)
            {
                // 마녀를 흡수 위치로 당긴다.
                cols[i].transform.position = Vector3.MoveTowards(cols[i].transform.position, gameObject.transform.position, 0.1f);
            }
            
        }
        
    }
    void DeathVacuumTrap()
    {
        Destroy(gameObject);
    }

    // 마녀가 근처에 있으면 흡수한다.
    IEnumerator FindWitch()
    {
        // 1초 후에 발동
        yield return new WaitForSeconds(1);
        b = true;
        // 흡수이펙트 생성
        GameObject Vacuum = Instantiate(VacuumFactory);
        Vacuum.transform.position = transform.position;
    }
}
