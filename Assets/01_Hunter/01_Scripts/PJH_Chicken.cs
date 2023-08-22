using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PJH_Chicken : MonoBehaviourPun
{
    public float speed = 15;            //�� ���ǵ�
    public float deathTime = 8;         //�� ����
    public GameObject kkokkioFactory;    // SimpleSonarShader_Object kkokkioFactory;

    GameObject Chicken;
    Collision col;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        //kkokkioFactory = GameObject.FindFirstObjectByType<SimpleSonarShader_Object>();
        //���� ���� Player �� �ƴҶ�
        /*
        if (photonView.IsMine == false)
        {
            //PlayerFire ������Ʈ�� ��Ȱ��ȭ
            this.enabled = false;
        }
        */
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
    void FindWitch()
    {
        bool isWitch = false;
        // �ݰ� 3M ���� �浹ü�߿� ���డ �ִٸ�
        int layer = 1 << LayerMask.NameToLayer("Witch");
        Collider[] cols = Physics.OverlapSphere(transform.position, 8, layer);
        if (cols.Length > 0)
        {
            // ���డ �ִ�.
            isWitch = true;
        }

        if (isWitch)
        {
            Chicken = Instantiate(kkokkioFactory);
        }
    }
}
