using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_Chicken : MonoBehaviour
{
    public GameObject wingFactory;      //������ ����Ʈ          
    public GameObject kkokkioFactory;   //�� ���� ����Ʈ
    public float speed = 15;            //�� ���ǵ�
    public float deathTime = 8;         //�� ����
    public float range = 5;             //���� ����
    bool isWitch = false;
    Rigidbody rb;
    Collider[] cols;
    bool b;
    LayerMask witchLayer;
    GameObject Chicken;
    GameObject Chicken1;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        // Witch���̾� ����
        witchLayer = LayerMask.NameToLayer("Witch");
        // deathTime�� �Ŀ� �� �ı�
        Invoke(nameof(DeathChicken), deathTime);
        // �� ���� ����Ʈ ȣ��
        Invoke(nameof(FindWitch), 1);

        //kkokkioFactory = GameObject.FindFirstObjectByType<SimpleSonarShader_Object>();
        //���� ���� Player �� �ƴҶ�
        /*
        if (photonView.IsMine == false)
        {
            //PlayerFire ������Ʈ�� ��Ȱ��ȭ
            this.enabled = false;
        }
        */
        Chicken1 = Instantiate(wingFactory, gameObject.transform);

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

    void Update()
    {
        
    }
    
    void DeathChicken()
    {
        /*
        //���� �� �Ѿ˸� 
        if (photonView.IsMine)
        {
            //���� �ı�����
            PhotonNetwork.Destroy(gameObject);
        }*/
        Destroy(gameObject);
    }


    // ���డ ��ó�� ������ ���.
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
        if (isWitch)
        {
            Chicken = Instantiate(kkokkioFactory, gameObject.transform);

        }
    
    }
}
