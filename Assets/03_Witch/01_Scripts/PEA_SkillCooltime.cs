using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PEA_SkillCooltime : MonoBehaviour
{
    private float curTime = 0f;
    private bool available = true;

    public Image black;

    public float cooltime;

    public bool Available
    {
        get { return available; }
    }

    void Start()
    {
        //black = transform.GetChild(0).GetComponent<Image>();
    }

    void Update()
    {
        if (!available)
        {
            Cooltime();
        }
    }

    public void UseSkill()
    {
        available = false;
        black.fillAmount = 1f;
    }

    private void Cooltime()
    {
        curTime += Time.deltaTime;

        black.fillAmount = 1 - (curTime / cooltime);

        if(curTime >= cooltime)
        {
            curTime = 0f;
            available = true;
        }
    }
}
