using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PJH_HunterFire : MonoBehaviour
{
    //폭탄 공장
    public GameObject bombFactory;
    //파편 공장
    //public GameObject fragmentFactory;

    //발사 위치
    public Transform firePosition;
    void Start()
    {
        
    }

    void Update()
    {
        //1번키를 누르면
        if (Input.GetButtonDown("Fire1"))
        {
            //FireBulletByInstantiate();


            GameObject potato = Instantiate(bombFactory);
            potato.transform.position = firePosition.position;
            potato.transform.forward = firePosition.forward;

            //폭탄공장에서 폭탄을 만든다

        }
    }

    void FireBulletByInstantiate()
    {
        //만들어진 폭탄을 카메라 앞방향으로 1만큼 떨어진 지점에 놓는다.
        Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward * 1;

        //만들어진 총알의 앞방향을 카메라가 보는 방향으로 설정
        Quaternion rot = Camera.main.transform.rotation;

       
    }


}
