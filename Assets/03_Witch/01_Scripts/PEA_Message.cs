using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PEA_Message : MonoBehaviour
{
    private float curTime = 0f;
    private float startTime = 3f;
    private float speed = 3f;
    private TMP_Text text = null;
    private Color color = new Color();

    void Start()
    {
        text = GetComponent<TMP_Text>();
        color = text.color;
    }

    void Update()
    {
        if(curTime < startTime)
        {
            curTime += Time.deltaTime;
        }
        else
        {
            color.a -= speed * Time.deltaTime;
            text.color = color;

            if(color.a <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
