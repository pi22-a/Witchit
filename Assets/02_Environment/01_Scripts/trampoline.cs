using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trampoline : MonoBehaviour
{
    PJH_HunterMoves HMT;
    PEA_WitchMovement WMT;
    
    //충돌시 캐릭터의 yVelocity에 영향을 줘서 점프 높이를 다르게 해야함 = yVelocity public으로 바꾸게하기. 
    private void Start()
    {
        //HMT = PJH_HunterMoves.instance;
        //WMT = PEA_WitchMovement.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        //해당 스크립트에 있는 속도 변화를 불러오기. 
        //TrampolineH, W는 yVelocity의 속도변화만 써두면됨. 
        if (other.gameObject.CompareTag("Hunter"))
        {
            //HMT.TrampolineH();
            // "Hunter" 태그가 붙은 오브젝트와 충돌했을 때
            Rigidbody rb = GetComponent<Rigidbody>();

            // 위로 힘을 가함
            Vector3 forceDirection = Vector3.up;
            rb.AddForce(forceDirection * upwardForce, ForceMode.Impulse);
        }
        if (other.gameObject.CompareTag("Witch"))
        {
            //WMT.TrampolineW();
            Rigidbody rb = GetComponent<Rigidbody>();

            // 위로 힘을 가함
            Vector3 forceDirection = Vector3.up;
            rb.AddForce(forceDirection * upwardForce, ForceMode.Impulse);
        }
    }


    //충돌시 캐릭터와 prop에 addforce하는
    public float upwardForce = 10.0f;

    //private void OnCollisionEnter(Collision collision)
    //{
    //        Debug.Log("d");
    //    if (collision.gameObject.CompareTag("Hunter"))
    //    {
    //        // "Hunter" 태그가 붙은 오브젝트와 충돌했을 때
    //        Rigidbody rb = GetComponent<Rigidbody>();

    //        // 위로 힘을 가함
    //        Vector3 forceDirection = Vector3.up;
    //        rb.AddForce(forceDirection * upwardForce, ForceMode.Impulse);
    //    }
    //}
}
