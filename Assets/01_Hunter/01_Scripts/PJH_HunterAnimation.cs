using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PJH_HunterAnimation : MonoBehaviour
{
    PJH_HunterFire hunter;
    // Start is called before the first frame update
    void Awake()
    {
        hunter = GetComponentInParent<PJH_HunterFire>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
