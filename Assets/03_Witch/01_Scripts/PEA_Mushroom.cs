using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_Mushroom : MonoBehaviour
{
    private float throwPower = 8f;
    private Vector3 throwDir = Vector3.zero;
    private Rigidbody rig = null;

    public GameObject mushroom;
    public GameObject sphere;
    public bool isMainMushroom = false;

    void Start()
    {
        throwDir = transform.forward + transform.up;
        throwDir.Normalize();
        rig = GetComponent<Rigidbody>();

        if (!isMainMushroom)
        {
            throwPower = 3f; 
        }
        rig.AddForce(throwDir * throwPower, ForceMode.Impulse);
    }

    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Mushroom"))
            return;

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        transform.up = collision.contacts[0].normal;

        rig.velocity = Vector3.zero;
        rig.angularVelocity = Vector3.zero;
        rig.useGravity = false;
        rig.isKinematic = true;

        if (isMainMushroom)
        {
            if(mushroom != null)
            {
                GameObject mushroom1 = Instantiate(mushroom, transform.position + transform.up + transform.right, Quaternion.identity);
                GameObject mushroom2 = Instantiate(mushroom, transform.position + transform.up - transform.right, Quaternion.identity);
                GameObject mushroom3 = Instantiate(mushroom, transform.position + transform.up - transform.forward, Quaternion.identity);

                mushroom1.transform.forward = transform.forward + transform.right;
                mushroom2.transform.forward = transform.forward - transform.right;
                mushroom3.transform.forward = -transform.forward;
            }

        }
    }
}
