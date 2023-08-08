using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_Chicken : MonoBehaviour
{
    public GameObject kkokkioFactory;   //닭 울음소리 이펙트    
    public float speed = 15;            //닭 스피드
    public float deathTime = 8;         //닭 수명

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        Invoke("DeathChicken", deathTime); // deathTime초 후에 닭 파괴

    }
    /*
    public IEnumerator MoveToPosition(Transform transform, Vector3 position, float timeToMove)
    {
        var currentPos = transform.position;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            transform.position = Vector3.Lerp(currentPos, position, t);
            yield return null;
        }
    }
    */
    // Update is called once per frame
    void Update()
    {
        // 닭이 우는 함수 시작
        StartCoroutine(FindWitch());
    }
    void DeathChicken()
    {
        Destroy(gameObject);
    }

    // 마녀가 근처에 있으면 운다.
    IEnumerator FindWitch()
    {
        
        yield return new WaitForSeconds(1);
        
        // 반경 3M 안의 충돌체중에 마녀가 있다면
        int layer = 1 << LayerMask.NameToLayer("Witch");
        Collider[] cols = Physics.OverlapSphere(transform.position, 8, layer);
        for (int i = 0; i < cols.Length; i++)
        {
            // 시끄럽게 울고싶다.
            GameObject kkokkio = Instantiate(kkokkioFactory);
            kkokkio.transform.position = transform.position;
        }        
    }
}
