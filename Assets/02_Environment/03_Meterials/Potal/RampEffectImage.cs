using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class RampEffectImage : MonoBehaviour
{
    public Material rampMaterial;
    //public Texture2D rampTexture;

    [Range(0f, 1f)]
    public float rampAmount;

    public AnimationCurve rampCurve = AnimationCurve.Linear(0.0f, 0.0f, 1.0f, 1.0f);
    public Gradient rampColor;
    Texture2D dynamicTex = null;
    //public Material shadowMaterial;
    //}
    void Start()
    {

    }

    void Update()
    {
#if UNITY_EDITOR
        if (dynamicTex == null)
        {
            dynamicTex = new Texture2D(256, 1, TextureFormat.RGBA32, false);
        }
        Color[] ColorBuffer = dynamicTex.GetPixels();
        for (int i = 0; i < 256; i++)
        {
            ColorBuffer[i].r = rampColor.Evaluate(i / 255.0f).r;
            ColorBuffer[i].g = rampColor.Evaluate(i / 255.0f).g;
            ColorBuffer[i].b = rampColor.Evaluate(i / 255.0f).b;
            ColorBuffer[i].a = rampCurve.Evaluate(i / 255.0f);
        }
        dynamicTex.SetPixels(ColorBuffer);
        dynamicTex.Apply();
        dynamicTex.wrapMode = TextureWrapMode.Clamp;
        rampMaterial.SetTexture("_RampTex", dynamicTex);
#endif
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        rampMaterial.SetFloat("_RampAmount", rampAmount);
        //rampMaterial.SetTexture("_RampTex", rampTexture);
        Graphics.Blit(source, destination, rampMaterial);
        //    Graphics.Blit(source, destination,shadowMaterial)
        //}
    }
}
