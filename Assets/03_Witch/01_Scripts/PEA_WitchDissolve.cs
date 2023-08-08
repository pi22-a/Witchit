using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_WitchDissolve : MonoBehaviour
{
    private float t = 0f;
    private float speed = 10f;
    private bool isDissolving = false;                           // 디졸브 중인지
    private bool visible = true;                                 // true : 점점 보여짐, false : 점점 사라짐

    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if (isDissolving)
        {
            Dissolve();
        }
    }

    public void WitchDissolve(bool visible)
    {
        this.visible = visible;
        isDissolving = true;
    }

    private void Dissolve()
    {
        Material[] mats = meshRenderer.materials;

        if (visible)
        {
            mats[0].SetFloat("_Cutoff", t * speed);
            t += Time.deltaTime;


            if (mats[0].GetFloat("_Cutoff") >= 1)
            {
                isDissolving = false;
            }
        }
        else
        {
            mats[0].SetFloat("_Cutoff", t * -speed);
            t += Time.deltaTime;


            if (mats[0].GetFloat("_Cutoff") <= 0)
            {
                isDissolving = false;
            }
        }
        meshRenderer.materials = mats;
    }

}
