using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PEA_WitchSphere : MonoBehaviourPun
{
    private float speed = 30f;

    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    void Update()
    {
        transform.localScale += Vector3.one * speed * Time.deltaTime;
    }
}
