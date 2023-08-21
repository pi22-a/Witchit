using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PJH_Bullet : MonoBehaviourPun
{
    //총알 속력
    public float speed = 20;


    void Start()
    {
        /*
        //내가 만든 Player 가 아닐때
        if (photonView.IsMine == false)
        {
            //PlayerFire 컴포넌트를 비활성화
            this.enabled = false;
        }*/
    }

    void Update()
    {
        //계속 앞으로 가고 싶다.
        transform.position += transform.forward * speed * Time.deltaTime;

    }

    
    private void OnTriggerEnter(Collider other)
    {
        // tag가 Witch일때
        if (other.transform.CompareTag("Witch"))
        {
            // 데미지를 n 만큼 주고싶다.
            int n = 5;
            other.transform.parent.parent.GetComponent<PEA_WitchHP>().Damage(n);
            print("데미지를 주었습니다.");
        }
        /*
        //내가 쏜 총알만 
        if (photonView.IsMine)
        {
            //나를 파괴하자
            PhotonNetwork.Destroy(gameObject);
        }*/
        Destroy(gameObject);
    }
}


