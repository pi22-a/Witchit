using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PJH_HunterFire : MonoBehaviour
{
    //감자 공장
    public GameObject potatoFactory;
    //닭 공장
    public GameObject chickenFactory;
    //흡수 공장

    //발사 위치
    public Transform firePosition;
    void Start()
    {
        
    }

    void Update()
    {
        //좌클릭시
        if (Input.GetButtonDown("Fire1"))
        {           
            GameObject potato = Instantiate(potatoFactory);
            potato.transform.position = firePosition.position;
            potato.transform.forward = firePosition.forward;
        }
        //우클릭시
        if (Input.GetButtonDown("Fire2"))
        {
            GameObject chicken = Instantiate(chickenFactory);
            chicken.transform.position = firePosition.position;
            chicken.transform.forward = firePosition.forward;
        }
        //Q클릭시
        if (Input.GetKeyDown(KeyCode.Q))
        {
            GameObject chicken = Instantiate(chickenFactory);
            chicken.transform.position = firePosition.position;
            chicken.transform.forward = firePosition.forward;
        }
        //좌 Ctrl 클릭시
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            GameObject chicken = Instantiate(chickenFactory);
            chicken.transform.position = firePosition.position;
            chicken.transform.forward = firePosition.forward;
        }
    }

   


}
