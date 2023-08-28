using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_Prob : MonoBehaviour
{
    private float colorLerpSpeed = 3f;
    private Color hitColor = new Color(1, 0.5f, 0.5f, 1f);
    private Material material;
    private Coroutine coroutine;
    private Rigidbody rig;

    void Start()
    {
        material = GetComponent<MeshRenderer>().materials[0];
        rig = GetComponent<Rigidbody>();
        rig.useGravity = false;
        rig.constraints = RigidbodyConstraints.FreezeAll;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            OnHitPotato();
        }
    }

    private void OnHitPotato()
    {
        material.color = hitColor;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
        coroutine = StartCoroutine( ReturnColor());
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            OnHitPotato();
        }

        if (!collision.transform.CompareTag("Ground"))
        {
            rig.useGravity = true;
            rig.constraints = RigidbodyConstraints.None;
        }
    }

    IEnumerator ReturnColor()
    {
        while (true)
        {
            material.color = Color.Lerp(material.color, Color.white, colorLerpSpeed * Time.deltaTime);

            if(material.color.g >= 0.98f && material.color.b >= 0.98f)
            {
                material.color = Color.white;
                break;
            }

            yield return new WaitForSeconds(Time.deltaTime);
        }

        coroutine = null;
        yield return null;
    }
}
