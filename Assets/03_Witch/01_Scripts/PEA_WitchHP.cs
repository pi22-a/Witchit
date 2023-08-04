using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_WitchHP : MonoBehaviour
{
    private int hp = 0;
    private bool isDead = false;

    private readonly int maxHp = 50;

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(int damage)
    {
        hp -= damage;

        if(hp <= 0)
        {
            isDead = true;
            print("Witch Die");
        }
    }
}
