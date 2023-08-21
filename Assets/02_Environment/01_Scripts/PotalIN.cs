using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotalIN : MonoBehaviour
{
    public Transform PotalPosition;
    public Transform player;

    private List<Collider> overlappingColliders = new List<Collider>();

    private void Update()
    {
        foreach (Collider collider in overlappingColliders)
        {
            Debug.Log("D");
            Vector3 portalToPlayer = collider.transform.position - transform.position;
            float rotationdiff = -Quaternion.Angle(transform.rotation, PotalPosition.rotation);
            //rotationdiff += 180;
            collider.transform.Rotate(Vector3.up, rotationdiff);
            Vector3 positionOffset = Quaternion.Euler(0f, rotationdiff, 0f) * portalToPlayer;

            CharacterController CC = collider.GetComponent<CharacterController>();
            if (CC != null)
            {
                Debug.Log("S");
                CC.enabled = false;
            }

            collider.transform.position = PotalPosition.position + positionOffset;
            //collider.transform. = PotalPosition.forward;

            if (CC != null)
            {
                Debug.Log("s");
                CC.enabled = true;
            }

            Debug.Log("dd");
        }

        overlappingColliders.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!overlappingColliders.Contains(other))
        {
            overlappingColliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (overlappingColliders.Contains(other))
        {
            overlappingColliders.Remove(other);
        }
    }
}

