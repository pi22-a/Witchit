using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_MeleeWeapon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        // tag�� Witch�϶�
        if (other.transform.CompareTag("Witch"))
        {
            // �������� n ��ŭ �ְ�ʹ�.
            int n = 5;
            other.transform.parent.parent.GetComponent<PEA_WitchHP>().Damage(n);
            print("�������� �־����ϴ�.");
        }

    }

}
