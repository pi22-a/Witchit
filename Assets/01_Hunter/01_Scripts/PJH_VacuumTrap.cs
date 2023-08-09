using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_VacuumTrap : MonoBehaviour
{
    public GameObject VacuumFactory;    //��� ����Ʈ    
    public float speed = 15;            //��� ���ǵ�
    public float deathTime = 8;         //��� ����
    public float range = 5;             //��� ����

    Rigidbody rb;

    bool b;
    LayerMask witchLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        // Witch���̾� ����
        witchLayer = LayerMask.NameToLayer("Witch");
        // deathTime�� �Ŀ� ��� �ı�
        Invoke("DeathVacuumTrap", deathTime); 
        // ����ϴ� �Լ� ȣ��
        StartCoroutine(FindWitch());
    }

    // Update is called once per frame
    void Update()
    {
        if(b)
        {
            // �ݰ� range ���� �浹ü�߿� ���ฦ ã�´�.
            int layerMask = (1 << witchLayer);
            Collider[] cols = Physics.OverlapSphere(transform.position, range, layerMask);
            for (int i=0; i < cols.Length; i++)
            {
                // ���ฦ ��� ��ġ�� ����.
                cols[i].transform.position = Vector3.MoveTowards(cols[i].transform.position, gameObject.transform.position, 0.1f);
            }
            
        }
        
    }
    void DeathVacuumTrap()
    {
        Destroy(gameObject);
    }

    // ���డ ��ó�� ������ ����Ѵ�.
    IEnumerator FindWitch()
    {
        // 1�� �Ŀ� �ߵ�
        yield return new WaitForSeconds(1);
        b = true;
        // �������Ʈ ����
        GameObject Vacuum = Instantiate(VacuumFactory);
        Vacuum.transform.position = transform.position;
    }
}
