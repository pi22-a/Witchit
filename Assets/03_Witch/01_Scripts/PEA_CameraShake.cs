using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_CameraShake : MonoBehaviour
{
    private float shakeTime = 0.5f;
    private float shakeSpeed = 2f;
    private float shakeAmount = 3f;

    private Coroutine coroutine = null;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void ShakeCamera()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        coroutine = StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        Vector3 originPos = Camera.main.transform.localPosition;
        float elapsedTime = 0f;

        while(elapsedTime < shakeTime)
        {
            Vector3 randomPoint = originPos + Random.insideUnitSphere * shakeAmount;
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, randomPoint, Time.deltaTime * shakeSpeed);

            yield return null;

            elapsedTime += Time.deltaTime;
        }

        Camera.main.transform.localPosition = originPos;
    }
}
