using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_Chicken : MonoBehaviour
{
    public GameObject expFactory;
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

    }

    // ��򰡿� �ε����� 3�� �Ŀ� �ı��ǰ�ʹ�.
    // �� �� �ݰ� 3M ���� �浹ü�߿� �����ִٸ�
    // �������� 2�� �ְ�ʹ�
    bool isCollisionCheck = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (true == isCollisionCheck)
            return;

        isCollisionCheck = true;

        StartCoroutine(IEBoom());

    }

    IEnumerator IEBoom()
    {
        // �߼Ҹ�����
        yield return new WaitForSeconds(1);
        // �߼Ҹ�����
        yield return new WaitForSeconds(1);
        // �߼Ҹ�����
        yield return new WaitForSeconds(1);


        // ��Ҹ�����
        // �ݰ� 3M ���� �浹ü�߿� ���డ �ִٸ�
        int layer = 1 << LayerMask.NameToLayer("Witch");
        Collider[] cols = Physics.OverlapSphere(transform.position, 3, layer);
        for (int i = 0; i < cols.Length; i++)
        {
            // �������� 2�� �ְ�ʹ�
            
        }
        // ����ź�� �ı��ϰ�ʹ�.
        Destroy(this.gameObject);

        GameObject explosion = Instantiate(expFactory);
        explosion.transform.position = transform.position;
    }
}
