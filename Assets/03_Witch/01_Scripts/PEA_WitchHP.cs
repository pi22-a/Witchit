using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PEA_WitchHP : MonoBehaviour
{
    private int hp = 0;
    private bool isDead = false;

    private readonly int maxHp = 50;

    //public Image hpImage; ������ ���� ����. �պ��� ���

    void Start()
    {
        hp = maxHp;
        //hpImage.fillAmount = 1; ������ ���� ����. �պ��� ���
    }

    void Update()
    {
        
    }

    public void Damage(int damage)
    {
        hp -= damage;
        //hpImage.fillAmount = hp / maxHp; ������ ���� ����. �պ��� ���

        if (hp <= 0)
        {
            isDead = true;
        }
    }
}
