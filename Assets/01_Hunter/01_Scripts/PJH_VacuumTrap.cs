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
        // �ݰ� 3M ���� �浹ü�߿� ���డ �ִٸ�
        int layer = 1 << LayerMask.NameToLayer("Witch");
        Collider[] cols = Physics.OverlapSphere(transform.position, 3, layer);
        if (cols.Length > 0)
        {
            // ���డ �ִ�.
            isWitch = true;
            
        }

        if (isWitch)
        {

            GameObject Vacuum = Instantiate(VacuumFactory);
            Vacuum.transform.position = transform.position;
            // ���ฦ ��� ��ġ�� ����.
            for (int i = 0; i <cols.Length; i++)
            {
                cols[0].transform.position = Vector3.MoveTowards(cols[0].transform.position, gameObject.transform.position, 0.1f);
            }
            
        }
    }
}
