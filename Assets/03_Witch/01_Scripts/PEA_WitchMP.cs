using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PEA_WitchMP : MonoBehaviour
{
    private float recoveryMP = 0;
    private int mp = 0;

    private readonly int mpRecoveryAmountPerSecond = 2;
    private readonly int maxMp = 100;

    public int MP
    {
        get { return mp; }
    }

    // Start is called before the first frame update
    void Start()
    {
        mp = maxMp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 1초에 2씩 마나 회복
    private void RecoveryMP()
    {
        recoveryMP += mpRecoveryAmountPerSecond * Time.deltaTime;

        if(recoveryMP >= 1f)
        {
            mp++;
            recoveryMP--;
        }
    }

    // 마나 사용
    public void UseMP(int consumption)
    {
        mp -= consumption;
    }
}
