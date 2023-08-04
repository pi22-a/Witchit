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
        // 30m�� �޷����� ���߰�ʹ�.
        //if(this.transform.forward = Vector3(1,1,1))
        {

        }
        // ���� ��� �Լ� ����
        StartCoroutine(IEBoom());
    }

    // ���డ ��ó�� ������ ���.
    IEnumerator IEBoom()
    {
        
        yield return new WaitForSeconds(8);
        
        


        // �ݰ� 3M ���� �浹ü�߿� ���డ �ִٸ�
        int layer = 1 << LayerMask.NameToLayer("Witch");
        Collider[] cols = Physics.OverlapSphere(transform.position, 3, layer);
        for (int i = 0; i < cols.Length; i++)
        {
            // �ò����� ���ʹ�.
            GameObject explosion = Instantiate(kkokkioFactory);
            explosion.transform.position = transform.position;
        }
        // �ߵ� �ı��ϰ�ʹ�.
        Destroy(this.gameObject);

        
    }
}
