using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_VacuumTrap : MonoBehaviour
{
    public GameObject VacuumFactory;    //흡수 이펙트    
    public float speed = 15;            //흡수 스피드
    public float deathTime = 8;         //흡수 수명
    public float range = 5;             //흡수 범위
    bool isWitch = false;
    Rigidbody rb;
    Collider[] cols;
    bool b;
    LayerMask witchLayer;
    GameObject Vacuum;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        // Witch레이어 설정
        witchLayer = LayerMask.NameToLayer("Witch");
        // deathTime초 후에 흡수 파괴
        Invoke(nameof(DeathVacuumTrap), deathTime);
        // 흡수이펙트 호출
        Invoke(nameof(FindWitch), 1);

    }

    // Update is called once per frame
    void Update()
    {
        if (isWitch)
        {
            Vacuum.transform.position = transform.position;
            for (int i = 0; i < cols.Length; i++)
            {
                // 마녀를 흡수 위치로 당긴다.
                cols[i].transform.root.position = Vector3.MoveTowards(cols[i].transform.root.position, gameObject.transform.position, 0.5f);
            }

        }

    }
    void DeathVacuumTrap()
    {
        Destroy(gameObject);
        //Destroy(VacuumFactory);
    }

    public void FindWitch()
    {        
        // 반경 3M 안의 충돌체중에 마녀가 있다면
        int layer = 1 << witchLayer;
        cols = Physics.OverlapSphere(transform.position, range, layer);
        if (cols.Length > 0)
        {
            // 마녀가 있다.
            isWitch = true;
            
        }
        if(isWitch)
        {
            Vacuum = Instantiate(VacuumFactory);

        }

        
    }
}
