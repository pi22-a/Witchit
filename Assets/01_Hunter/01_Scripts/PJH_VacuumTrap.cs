using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_VacuumTrap : MonoBehaviour
{
    public GameObject VacuumFactory;    //��� ����Ʈ    
    public float speed = 15;            //��� ���ǵ�
    public float deathTime = 8;         //��� ����
    public float range = 5;             //��� ����
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
        // Witch���̾� ����
        witchLayer = LayerMask.NameToLayer("Witch");
        // deathTime�� �Ŀ� ��� �ı�
        Invoke(nameof(DeathVacuumTrap), deathTime);
        // �������Ʈ ȣ��
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
                // ���ฦ ��� ��ġ�� ����.
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
        // �ݰ� 3M ���� �浹ü�߿� ���డ �ִٸ�
        int layer = 1 << witchLayer;
        cols = Physics.OverlapSphere(transform.position, range, layer);
        if (cols.Length > 0)
        {
            // ���డ �ִ�.
            isWitch = true;
            
        }
        if(isWitch)
        {
            Vacuum = Instantiate(VacuumFactory);

        }

        
    }
}
