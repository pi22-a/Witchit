using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_Chicken : MonoBehaviour
{
    public GameObject kkokkioFactory;   //�� �����Ҹ� ����Ʈ    
    public float speed = 15;            //�� ���ǵ�
    public float deathTime = 8;         //�� ����

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        Invoke("DeathChicken", deathTime); // deathTime�� �Ŀ� �� �ı�

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
        // ���� ��� �Լ� ����
        StartCoroutine(FindWitch());
    }
    void DeathChicken()
    {
        Destroy(gameObject);
    }

    // ���డ ��ó�� ������ ���.
    IEnumerator FindWitch()
    {
        
        yield return new WaitForSeconds(1);
        
        // �ݰ� 3M ���� �浹ü�߿� ���డ �ִٸ�
        int layer = 1 << LayerMask.NameToLayer("Witch");
        Collider[] cols = Physics.OverlapSphere(transform.position, 8, layer);
        for (int i = 0; i < cols.Length; i++)
        {
            // �ò����� ���ʹ�.
            GameObject kkokkio = Instantiate(kkokkioFactory);
            kkokkio.transform.position = transform.position;
        }        
    }
}
