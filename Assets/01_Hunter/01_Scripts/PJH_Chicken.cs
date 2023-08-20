using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PJH_Chicken : MonoBehaviourPun
{
    public float speed = 15;            //닭 스피드
    public float deathTime = 8;         //닭 수명
                                        // SimpleSonarShader_Object kkokkioFactory;

    Collision col;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        //kkokkioFactory = GameObject.FindFirstObjectByType<SimpleSonarShader_Object>();
        //내가 만든 Player 가 아닐때
        /*
        if (photonView.IsMine == false)
        {
            //PlayerFire 컴포넌트를 비활성화
            this.enabled = false;
        }
        */
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        Invoke("DeathChicken", deathTime); // deathTime초 후에 닭 파괴

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
        //내가 쏜 총알만 
        if (photonView.IsMine)
        {
            //나를 파괴하자
            PhotonNetwork.Destroy(gameObject);
        }*/
        Destroy(gameObject);
    }

    
    // 마녀가 근처에 있으면 운다.
    void FindWitch()
    {
        bool isWitch = false;
        // 반경 3M 안의 충돌체중에 마녀가 있다면
        int layer = 1 << LayerMask.NameToLayer("Witch");
        Collider[] cols = Physics.OverlapSphere(transform.position, 3, layer);
        if (cols.Length > 0)
        {
            // 마녀가 있다.
            isWitch = true;
        }

        if (isWitch)
        {

            //닭 울음소리 이펙트    
            // 시끄럽게 울고싶다.
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
