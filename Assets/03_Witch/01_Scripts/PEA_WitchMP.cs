using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PEA_WitchMP : MonoBehaviour
{
    private float recoveryMP = 0;
    private int mp = 0;

    private readonly int mpRecoveryAmountPerSecond = 2;
    private readonly int maxMp = 100;

    private Coroutine coroutine = null;

    public Image mpImage;

    public int MP
    {
        get { return mp; }
    }

    void Start()
    {
        mp = maxMp;
    }

    void Update()
    {
        if(mp < maxMp)
        {
            RecoveryMP();
        }
    }

    // 1초에 2씩 마나 회복
    private void RecoveryMP()
    {
        recoveryMP += mpRecoveryAmountPerSecond * Time.deltaTime;

        if(recoveryMP >= 1f)
        {
            mp++;
            recoveryMP--;
            if(coroutine == null)
            {
                coroutine = StartCoroutine(IncreaseMP());
            }
        }
    }

    // 마나 사용
    public void UseMP(int consumption)
    {
        mp -= consumption;
        if(coroutine == null)
        {
            coroutine = StartCoroutine(DecreaseMP(consumption));
        }
    }

    IEnumerator IncreaseMP()
    {
        while (mpImage.fillAmount < (float)mp / 100)
        {
            mpImage.fillAmount +=  Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        mpImage.fillAmount = (float)mp / 100;
        coroutine = null;
        yield return null;
    }

    IEnumerator DecreaseMP(int consumption)
    {
        print(consumption + ", " + (float)consumption / 100);
        while(mpImage.fillAmount > (float)mp / 100)
        {
            mpImage.fillAmount -= ((float)consumption * 2 * Time.deltaTime) / 100;
            yield return new WaitForSeconds(Time.deltaTime);
        }

        mpImage.fillAmount = (float)mp / 100;
        coroutine = null;
        yield return null;
    }
}
