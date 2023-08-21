using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PJH_SkillItem : MonoBehaviour
{
    public Image imageAlpha;
    public float defaultAlpha = 0;
    public int coolTime = 8;
    // Start is called before the first frame update
    void Start()
    {
        imageAlpha.fillAmount = defaultAlpha;
        if (coolTime <= 0)
        {
            coolTime = 1;
        }
    }

    bool useSKill = false;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            useSKill = true;
            imageAlpha.fillAmount = 1;
        }

        if(useSKill)
        {
            float fill = imageAlpha.fillAmount;
            fill -= Time.deltaTime / coolTime;
            imageAlpha.fillAmount = Mathf.Clamp01(fill);
            
        }
    }

    // ��ų�� ����� �� �ֳ���?
    public bool CanDoIt()
    {
        return imageAlpha.fillAmount == 0;
    }

    // ��ų�� ������!
    public void DoIt()
    {
        imageAlpha.fillAmount = 1;
    }
}
