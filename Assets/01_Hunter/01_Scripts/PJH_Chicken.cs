using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_Chicken : MonoBehaviour
{
    public GameObject wingFactory;      //날개짓 이펙트          
    public GameObject kkokkioFactory;   //닭 울음 이펙트
    public float speed = 15;            //닭 스피드
    public float deathTime = 8;         //닭 수명
    public float range = 5;             //울음 범위
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
        // Witch레이어 설정
        witchLayer = LayerMask.NameToLayer("Witch");
        // deathTime초 후에 닭 파괴
        Invoke(nameof(DeathChicken), deathTime);
        // 닭 울음 이펙트 호출
        Invoke(nameof(FindWitch), 1);

        //kkokkioFactory = GameObject.FindFirstObjectByType<SimpleSonarShader_Object>();
        //내가 만든 Player 가 아닐때
        /*
        if (photonView.IsMine == false)
        {
            //PlayerFire 컴포넌트를 비활성화
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
        //내가 쏜 총알만 
        if (photonView.IsMine)
        {
            //나를 파괴하자
            PhotonNetwork.Destroy(gameObject);
        }*/
        Destroy(gameObject);
    }


    // 마녀가 근처에 있으면 운다.
    public void FindWitch()
    {
        // 반경 3M 안의 충돌체중에 마녀가 있다면
        int layer = 1 << witchLayer;
        cols = Physics.OverlapSphere(transform.position, range, layer);
        if (cols.Length > 0)
        {
            // 마녀가 있다.
            isWitch = true;

        }
        if (isWitch)
        {
            Chicken = Instantiate(kkokkioFactory, gameObject.transform);

        }
    
    }
}
