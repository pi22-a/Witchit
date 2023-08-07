using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_VacuumTrap : MonoBehaviour
{
    public GameObject VacuumFactory;    //흡수 이펙트    
    public float speed = 15;            //흡수 스피드
    public float deathTime = 8;         //흡수 수명
    public GameObject Witch;            //흡수 당할 대상 : 마녀

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        Invoke("DeathVacuumTrap", deathTime); // deathTime초 후에 흡수 파괴
    }

    // Update is called once per frame
    void Update()
    {
        // 닭이 우는 함수 시작
        StartCoroutine(FindWitch());
    }
    void DeathVacuumTrap()
    {
        Destroy(gameObject);
    }

    // 마녀가 근처에 있으면 흡수한다.
    IEnumerator FindWitch()
    {

        yield return new WaitForSeconds(1);

        // 반경 3M 안의 충돌체중에 마녀가 있다면
        int layer = 1 << LayerMask.NameToLayer("Witch");
        Collider[] cols = Physics.OverlapSphere(transform.position, 20, layer);
        for (int i = 0; i < cols.Length; i++)
        {
            // 흡수이펙트 생성
            GameObject kkokkio = Instantiate(VacuumFactory);
            kkokkio.transform.position = transform.position;
            // 마녀를 초당 n만큼 흡수 위치로 당긴다.
            Witch.transform.position = Vector3.MoveTowards(gameObject.transform.position, this.transform.position, 0.1f);
        }
    }
}
