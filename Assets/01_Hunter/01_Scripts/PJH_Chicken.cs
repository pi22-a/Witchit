using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_Chicken : MonoBehaviour
{
    public GameObject kkokkioFactory;
    Rigidbody rb;
    public float speed = 15;
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);

        rb.AddTorque(Vector3.right * 50, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        // 30m를 달려가면 멈추고싶다.
        //if(this.transform.forward = Vector3(1,1,1))
        {

        }
        // 닭이 우는 함수 시작
        StartCoroutine(IEBoom());
    }

    // 마녀가 근처에 있으면 운다.
    IEnumerator IEBoom()
    {
        
        yield return new WaitForSeconds(8);
        
        


        // 반경 3M 안의 충돌체중에 마녀가 있다면
        int layer = 1 << LayerMask.NameToLayer("Witch");
        Collider[] cols = Physics.OverlapSphere(transform.position, 3, layer);
        for (int i = 0; i < cols.Length; i++)
        {
            // 시끄럽게 울고싶다.
            GameObject explosion = Instantiate(kkokkioFactory);
            explosion.transform.position = transform.position;
        }
        // 닭도 파괴하고싶다.
        Destroy(this.gameObject);

        
    }
}
