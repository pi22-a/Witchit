using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trampoline : MonoBehaviour
{
    PJH_HunterMoves HMT;
    PEA_WitchMovement WMT;
    
    //�浹�� ĳ������ yVelocity�� ������ �༭ ���� ���̸� �ٸ��� �ؾ��� = yVelocity public���� �ٲٰ��ϱ�. 
    private void Start()
    {
        //HMT = PJH_HunterMoves.instance;
        //WMT = PEA_WitchMovement.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        //�ش� ��ũ��Ʈ�� �ִ� �ӵ� ��ȭ�� �ҷ�����. 
        //TrampolineH, W�� yVelocity�� �ӵ���ȭ�� ��θ��. 
        if (other.gameObject.CompareTag("Hunter"))
        {
            //HMT.TrampolineH();
            // "Hunter" �±װ� ���� ������Ʈ�� �浹���� ��
            Rigidbody rb = GetComponent<Rigidbody>();

            // ���� ���� ����
            Vector3 forceDirection = Vector3.up;
            rb.AddForce(forceDirection * upwardForce, ForceMode.Impulse);
        }
        if (other.gameObject.CompareTag("Witch"))
        {
            //WMT.TrampolineW();
            Rigidbody rb = GetComponent<Rigidbody>();

            // ���� ���� ����
            Vector3 forceDirection = Vector3.up;
            rb.AddForce(forceDirection * upwardForce, ForceMode.Impulse);
        }
    }


    //�浹�� ĳ���Ϳ� prop�� addforce�ϴ�
    public float upwardForce = 10.0f;

    //private void OnCollisionEnter(Collision collision)
    //{
    //        Debug.Log("d");
    //    if (collision.gameObject.CompareTag("Hunter"))
    //    {
    //        // "Hunter" �±װ� ���� ������Ʈ�� �浹���� ��
    //        Rigidbody rb = GetComponent<Rigidbody>();

    //        // ���� ���� ����
    //        Vector3 forceDirection = Vector3.up;
    //        rb.AddForce(forceDirection * upwardForce, ForceMode.Impulse);
    //    }
    //}
}
