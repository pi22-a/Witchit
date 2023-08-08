using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_ProbDissolve : MonoBehaviour
{
    private float t = 0f;
    private float cutoff = 0f;
    private float dissolveSpeed = 5f;
    private Coroutine coroutine = null;
    private Material[] mat;
    private MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        mat = meshRenderer.materials;
    }

    void Update()
    {
        
    }

    public void ProbDissolve(float afterTime = 0f)
    {;
        if (coroutine == null)
        {
            coroutine = StartCoroutine(Dissolve(afterTime));
        }
    }

    IEnumerator Dissolve(float afterTime)
    {
        tag = "Untagged";

        while(t < afterTime)
        {
            t += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        t = 0f;

        while (mat[0].GetFloat("_Cutoff") < 1)
        {
            cutoff += Time.deltaTime * dissolveSpeed;
            foreach (Material m in mat)
            {
                m.SetFloat("_Cutoff", cutoff);
            }
            t += Time.deltaTime;

            meshRenderer.materials = mat;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        t = 0f;
        Destroy(gameObject);
        yield return null;
    }
}
