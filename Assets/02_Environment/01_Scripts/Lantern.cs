using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    Vector3 direction;
    public GameObject player;
    public GameObject Light;
    bool lightONOFF = false;
    void Start()
    {
        lightONOFF = false;
        Light.SetActive(false);
    }

    void Update()
    {
        //거리측정
        direction = player.transform.position - this.transform.position;
        float size = direction.magnitude;
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (size < 1f)
            {
                if (lightONOFF)
                {
                    LanternON();
                }
                else
                {
                    LanternOFF();
                }
            }

        }
        //direction.Normalize();
    }
    void LanternON()
    {
        Light.SetActive(true);
        lightONOFF = true;
    }
    void LanternOFF()
    {
        Light.SetActive(false);
        lightONOFF = false;
    }
}
