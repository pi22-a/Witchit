using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalIN : MonoBehaviour
{
    public Transform PotalPosition;
    private void OnTriggerEnter(Collider other)
    {
        Transform ParentTransform = other.transform;
        while (true)
        {

        }
    }
}
