using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_WitchHP : MonoBehaviour
{
    private int hp = 0;
    private bool isDead = false;

    private readonly int maxHp = 50;
    //박정훈이 추가함. 싱글톤 패턴 사용을 위해
    public static PEA_WitchHP instance; 

    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
    }

    //박정훈이 추가함. 싱글톤 패턴 사용을 위해
    private void Awake() 
    {
        instance = this;
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
