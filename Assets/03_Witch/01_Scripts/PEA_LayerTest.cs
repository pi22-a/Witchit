using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_LayerTest : MonoBehaviour
{
    void Start()
    {
        Camera.main.cullingMask = ~(1<<7);
    }

    void Update()
    {
        
    }
}
