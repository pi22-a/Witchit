////using System.Collections;
////using System.Collections.Generic;
////using UnityEngine;

////public class ConstraintsOFF : MonoBehaviour
////{
////    private Rigidbody rig; // Rigidbody ����
////    private Collider[] colliders; // ��� Collider �迭

////    private void Start()
////    {
////        rig = GetComponent<Rigidbody>(); // Rigidbody ��������
////        colliders = GetComponentsInChildren<Collider>(); // ��� Collider ��������
////    }

////    private void OnCollisionEnter(Collision collision)
////    {
////        if (collision.gameObject.name == "Hunter")
////        {
////            foreach (Collider col in colliders)
////            {
////                Debug.Log("d");
////                col.attachedRigidbody.constraints = RigidbodyConstraints.None; // �ݶ��̴��� ����� Rigidbody�� ���� ���� ����
////            }
////        }
////    }
////}
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ConstraintsOFF : MonoBehaviour
//{
//    private Rigidbody rig; // Rigidbody ����

//    private void Start()
//    {
//        rig = GetComponentInChildren<Rigidbody>(); // �ڽ� ������Ʈ�� �ִ� Rigidbody ��������
//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.name == "Hunter")
//        {
//            Debug.Log("d");

//            rig.constraints = RigidbodyConstraints.None; // Rigidbody�� ���� ���� ���� (Freeze Position�� Rotation)
//        }
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstraintsOFF : MonoBehaviour
{
    public Collider[] colliders; // Collider �迭

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Hunter")
        {
            foreach (Collider col in colliders)
            {
                Rigidbody rb = col.attachedRigidbody;
                if (rb != null)
                {
                    rb.constraints = RigidbodyConstraints.None; // �ش� Collider�� ����� Rigidbody�� ���� ���� ����
                }
            }
        }
    }
}

