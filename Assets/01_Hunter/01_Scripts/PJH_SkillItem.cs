using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PJH_SkillItem : MonoBehaviour
{
    public Image imageAlpha;
    public float defaultAlpha = 0;
    public int coolTime = 2;
    // Start is called before the first frame update
    void Start()
    {
        imageAlpha.fillAmount = defaultAlpha;
        if (coolTime <= 0)
        {
            coolTime = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float fill = imageAlpha.fillAmount;
        fill -= Time.deltaTime / coolTime;
        imageAlpha.fillAmount = Mathf.Clamp01(fill);
    }

    // 스킬을 사용할 수 있나요?
    public bool CanDoIt()
    {
        return imageAlpha.fillAmount == 0;
    }

    // 스킬을 쓰세요!
    public void DoIt()
    {
        imageAlpha.fillAmount = 1;
    }
}
