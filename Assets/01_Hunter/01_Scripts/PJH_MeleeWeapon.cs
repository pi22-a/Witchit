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
        // tag가 Witch일때
        if (other.transform.CompareTag("Witch"))
        {
            // 데미지를 n 만큼 주고싶다.
            int n = 5;
            other.transform.parent.parent.GetComponent<PEA_WitchHP>().Damage(n);
            print("데미지를 주었습니다.");
        }

    }

}
