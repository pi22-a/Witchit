using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PEA_WitchHP : MonoBehaviourPun
{
    private int hp = 0;
    private bool isDead = false;

    private readonly int maxHp = 50;

    private PEA_WitchSkill witchSkill = null;

    public Image hpImage;
    public GameObject probBody;
    public GameObject witchUI;

    public bool IsDead
    {
        get { return isDead; }
    }

    void Start()
    {
        hp = maxHp;
        hpImage.fillAmount = 1;
        witchSkill = GetComponent<PEA_WitchSkill>();

        if (photonView.IsMine)
        {
            witchUI.SetActive(true);
        }
        else
        {
            witchUI.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Damage(50);
        }
    }

    public void Damage(int damage)
    {
        hp -= damage;
        hpImage.fillAmount = hp / maxHp;

        if (hp <= 0)
        {
            Die();
        }
    }


    private void Die()
    {
        isDead = true;

        if (!witchSkill.IsChanged)
        {
            witchSkill.WitchDissolve(false);
        }
        else
        {
            probBody.transform.GetChild(1).GetComponent<PEA_ProbDissolve>().ProbDissolve();
        }

        transform.GetComponent<PEA_WitchMovement>().enabled = false;
        transform.GetComponent<PEA_WitchSkill>().enabled = false;
        Camera.main.transform.SetParent(null);
        Camera.main.transform.position = transform.position;
        Camera.main.GetComponent<PEA_WatchCamera>().enabled = true;
        witchUI.SetActive(false);
    }
}