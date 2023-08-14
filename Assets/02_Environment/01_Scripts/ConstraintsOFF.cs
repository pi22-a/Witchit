////using System.Collections;
////using System.Collections.Generic;
////using UnityEngine;

////public class ConstraintsOFF : MonoBehaviour
////{
////    private Rigidbody rig; // Rigidbody 변수
////    private Collider[] colliders; // 모든 Collider 배열

////    private void Start()
////    {
////        rig = GetComponent<Rigidbody>(); // Rigidbody 가져오기
////        colliders = GetComponentsInChildren<Collider>(); // 모든 Collider 가져오기
////    }

////    private void OnCollisionEnter(Collision collision)
////    {
////        if (collision.gameObject.name == "Hunter")
////        {
////            foreach (Collider col in colliders)
////            {
////                Debug.Log("d");
////                col.attachedRigidbody.constraints = RigidbodyConstraints.None; // 콜라이더에 연결된 Rigidbody의 제약 조건 해제
////            }
////        }
////    }
////}
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ConstraintsOFF : MonoBehaviour
//{
//    private Rigidbody rig; // Rigidbody 변수

//    private void Start()
//    {
//        rig = GetComponentInChildren<Rigidbody>(); // 자식 오브젝트에 있는 Rigidbody 가져오기
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.name == "Hunter")
//        {
//            Debug.Log("d");

//            rig.constraints = RigidbodyConstraints.None; // Rigidbody의 제약 조건 해제 (Freeze Position과 Rotation)
//        }
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintsOFF : MonoBehaviour
{
    public Collider[] colliders; // Collider 배열

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Hunter")
        {
            foreach (Collider col in colliders)
            {
                Rigidbody rb = col.attachedRigidbody;
                if (rb != null)
                {
                    rb.constraints = RigidbodyConstraints.None; // 해당 Collider에 연결된 Rigidbody의 제약 조건 해제
                }
            }
        }
    }
}

