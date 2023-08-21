using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_Door : MonoBehaviour
{
    private float speed = 30f;

    private void Start()
    {
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
