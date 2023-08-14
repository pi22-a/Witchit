using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextrueSetup : MonoBehaviour
{
    public Camera cameraA_I;
    public Material cameraMatA_I;

    private void Start()
    {
        if(cameraA_I.targetTexture != null)
        {
            cameraA_I.targetTexture.Release();
        }
        cameraA_I.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        cameraMatA_I.mainTexture = cameraA_I.targetTexture;
    }
}
