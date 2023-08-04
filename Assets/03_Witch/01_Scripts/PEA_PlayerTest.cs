using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_PlayerTest : MonoBehaviour
{
    private float x = 0f;
    private float z = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        transform.position += new Vector3(x, 0, z).normalized * 5 * Time.deltaTime;
    }
}
