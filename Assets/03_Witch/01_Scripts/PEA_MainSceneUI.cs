using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_MainSceneUI : MonoBehaviour
{
    public GameObject mainButtons;
    public GameObject playButtons;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnClickPlayButton()
    {
        mainButtons.SetActive(false);
        playButtons.SetActive(true);
    }

    public void OnClickBackButton()
    {
        mainButtons.SetActive(true);
        playButtons.SetActive(false);
    }
}
