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

    // 1�ʿ� 2�� ���� ȸ��
    private void RecoveryMP()
    {
        recoveryMP += mpRecoveryAmountPerSecond * Time.deltaTime;

        if(recoveryMP >= 1f)
        {
            mp++;
            recoveryMP--;
        }
    }

    // ���� ���
    public void UseMP(int consumption)
    {
        mp -= consumption;
    }
}
