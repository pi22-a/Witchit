using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RenderTest : MonoBehaviour
{
    public Material dmaterial;
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, dmaterial);
    }
}
