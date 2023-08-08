using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PEA_WitchHP : MonoBehaviour
{
    private int hp = 0;
    private bool isDead = false;

    private readonly int maxHp = 50;

    //public Image hpImage; 박정훈 임의 수정. 합병때 취소

    void Start()
    {
        hp = maxHp;
        //hpImage.fillAmount = 1; 박정훈 임의 수정. 합병때 취소
    }

    void Update()
    {
        
    }

    public void Damage(int damage)
    {
        hp -= damage;
        //hpImage.fillAmount = hp / maxHp; 박정훈 임의 수정. 합병때 취소

        if (hp <= 0)
        {
            isDead = true;
        }
    }
}
