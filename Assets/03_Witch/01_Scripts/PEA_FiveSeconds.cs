using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PEA_FiveSeconds : MonoBehaviour
{
    private readonly int hideTime = 5;
    private readonly int seekEndTime = 10;

    private TMP_Text text;
    private Coroutine coroutine = null;

    private void OnEnable()
    {
    }

    private void Awake()
    {
        text = GetComponent<TMP_Text>();        
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void CountDownEmphasis(int countTime)
    {
        //print(coroutine == null);

        if(coroutine == null)
        {
            gameObject.transform.localScale = Vector3.zero;
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
            coroutine = StartCoroutine(ECounDownEmphasis(countTime));
        }
    }

    IEnumerator ECounDownEmphasis(int countTime)
    {
        Vector3 scale = transform.localScale;
        Color textColor = text.color;
        float f = 0;

        for (int i = countTime; i >= 0; i--)
        {
            if (i == 0 && countTime == hideTime)
            {
                GetComponent<TMP_Text>().text = "Seek";
            }
            else if (i == 0 && countTime == seekEndTime)
            {
                GetComponent<TMP_Text>().text = "Time Over!";
            }
            else if (i > 0)
            {
                GetComponent<TMP_Text>().text = i.ToString();
            }

            while (f < 1)
            {
                f += Time.deltaTime * 3f;
                scale = new Vector3(f, f, f);
                textColor.a = f;
                transform.localScale = scale;
                text.color = textColor;

                yield return new WaitForSeconds(Time.deltaTime);
            }

            yield return new WaitForSeconds(0.5f);

            while(f > 0)
            {
                f -= Time.deltaTime * 3f;
                scale.y = f;
                textColor.a = f;
                transform.localScale = scale;
                text.color = textColor;

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        coroutine = null;
        yield return null;
    }
}
