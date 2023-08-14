using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_Chicken : MonoBehaviour
{
    public float speed = 15;            //�� ���ǵ�
    public float deathTime = 8;         //�� ����
                                        // SimpleSonarShader_Object kkokkioFactory;

    Collision col;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        //kkokkioFactory = GameObject.FindFirstObjectByType<SimpleSonarShader_Object>();


        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        Invoke("DeathChicken", deathTime); // deathTime�� �Ŀ� �� �ı�

        Invoke("FindWitch", 1);

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

    }
    void DeathChicken()
    {
        Destroy(gameObject);
    }

    // ���డ ��ó�� ������ ���.
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

            //�� �����Ҹ� ����Ʈ    
            // �ò����� ���ʹ�.
            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                SimpleSonarShader_Object kkokkioFactory = hitInfo.transform.GetComponentInParent<SimpleSonarShader_Object>();
                if (kkokkioFactory)
                {
                    for(int i=0; i<4; i++)
                    kkokkioFactory.StartSonarRing(hitInfo.point, 20);
                }
                    
            }

        }
    }
}
