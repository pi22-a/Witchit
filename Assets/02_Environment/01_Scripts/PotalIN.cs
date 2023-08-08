using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalIN : MonoBehaviour
{
    public Transform PotalPosition;
    private void OnTriggerEnter(Collider _col)
    {
        Debug.Log("Ãæµ¹");
        if (_col.gameObject.tag == "Hunter")
        {
            Transform ParentTransform = _col.transform;
            while (true)
            {
                if (ParentTransform.parent == null) break;
                else ParentTransform = ParentTransform.parent;
            }
            ParentTransform.position = PotalPosition.position;
            ParentTransform.rotation = PotalPosition.rotation;

        }

    }
}
