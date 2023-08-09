using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalIN : MonoBehaviour
{
    public Transform hunter;
    public Transform PotalPosition;

    private bool IsOverlapping = false;

    private void Update()
    {
        if (IsOverlapping)
        {
            Debug.Log("D");
            Vector3 portalToPlayer = hunter.position - transform.position;
            float dotProduct = Vector3.Dot(transform.up, portalToPlayer);
            if(dotProduct < 0f)
            {
                float rotationdiff = -Quaternion.Angle(transform.rotation, PotalPosition.rotation);
                rotationdiff += 180;
                hunter.Rotate(Vector3.up, rotationdiff);
                Vector3 positionOffset = Quaternion.Euler(0f, rotationdiff, 0f) * portalToPlayer;
                hunter.position = PotalPosition.position + positionOffset;
                IsOverlapping = false;
                Debug.Log("dd");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Hunter")
        {
            IsOverlapping = true;
        }
        //if (other.tag == "Hunter")
        //{
        //    Debug.Log("Ãæµ¹");
        //    Transform ParentTransform = other.transform;
        //    while (true)
        //    {
        //        if (ParentTransform.parent == null) break;
        //        else ParentTransform = ParentTransform.parent;
        //    }
        //    ParentTransform.position = PotalPosition.position;
        //    ParentTransform.rotation = PotalPosition.rotation;

        //}

    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Hunter")
        {
            IsOverlapping = false;
        }
    }
}
