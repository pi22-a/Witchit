using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_VacuumTrap : MonoBehaviour
{
    public GameObject VacuumFactory;    //��� ����Ʈ    
    public float speed = 15;            //��� ���ǵ�
    public float deathTime = 8;         //��� ����
    public GameObject Witch;            //��� ���� ��� : ����

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        Invoke("DeathVacuumTrap", deathTime); // deathTime�� �Ŀ� ��� �ı�
    }

    // Update is called once per frame
    void Update()
    {
        // ���� ��� �Լ� ����
        StartCoroutine(FindWitch());
    }
    void DeathVacuumTrap()
    {
        Destroy(gameObject);
    }

    // ���డ ��ó�� ������ ����Ѵ�.
    IEnumerator FindWitch()
    {

        yield return new WaitForSeconds(1);

        // �ݰ� 3M ���� �浹ü�߿� ���డ �ִٸ�
        int layer = 1 << LayerMask.NameToLayer("Witch");
        Collider[] cols = Physics.OverlapSphere(transform.position, 20, layer);
        for (int i = 0; i < cols.Length; i++)
        {
            // �������Ʈ ����
            GameObject kkokkio = Instantiate(VacuumFactory);
            kkokkio.transform.position = transform.position;
            // ���ฦ �ʴ� n��ŭ ��� ��ġ�� ����.
            Witch.transform.position = Vector3.MoveTowards(gameObject.transform.position, this.transform.position, 0.1f);
        }
    }
}
