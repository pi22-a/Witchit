using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PEA_LoadingText : MonoBehaviour
{
    private float curTime = 0f;
    private int textState = 0;
    private TMP_Text loadingText;

    private void OnEnable()
    {
        curTime = 0f;
    }

    void Start()
    {
        loadingText = GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
        curTime += Time.deltaTime;

        if (curTime % 1 < 0.25f )
        {
            if(textState != 1)
            {
                textState = 1;
                loadingText.text = "Loading";
            }
        }
        else if( curTime % 1 < 0.5f)
        {
            if (textState != 2)
            {
                textState = 2;
                loadingText.text = "Loading.";
            }
        }
        else if(curTime % 1 < 0.75f)
        {
            if (textState != 3)
            {
                textState = 3;
                loadingText.text = "Loading..";
            }            
        }
        else if(curTime % 1 >= 0.75f)
        {
            if (textState != 4)
            {
                textState = 4;
                loadingText.text = "Loading...";
            }            
        }
    }
}
