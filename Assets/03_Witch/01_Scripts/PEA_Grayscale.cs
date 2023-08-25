using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_Grayscale : MonoBehaviour
{
    public Material shadowMaterial;

    [Range(0,1)]
    public float grayscale = 0f;

    void Start()
    {
        
    }

    void Update()
    {

    }


    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //print("onRenderImage");
        //shadowMaterial.SetFloat("Grayscale", grayscale);
        //print(grayscale + ", " + shadowMaterial.GetFloat("Grayscale"));
        shadowMaterial.SetFloat("_LerpVal", grayscale);
        Graphics.Blit(source, destination, shadowMaterial);
    }
}
