using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_Billboard : MonoBehaviour
{
    void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
