using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    public GameObject player;
    public GameObject lightObject;
    bool lightONOFF = false;
    public float maxRaycastDistance = 3f;

    void Update()
    {
        Vector3 directionToPlayer = player.transform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, directionToPlayer, out hit, maxRaycastDistance))
        {
            if (hit.collider.gameObject == player && Input.GetKeyDown(KeyCode.E))
            {
                ToggleLantern();
            }
        }
    }

    void ToggleLantern()
    {
        lightONOFF = !lightONOFF;
        lightObject.SetActive(lightONOFF);
    }
}



