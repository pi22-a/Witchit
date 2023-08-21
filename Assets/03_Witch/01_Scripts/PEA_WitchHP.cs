using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class PEA_WitchHP : MonoBehaviourPun
{
    private int hp = 0;
    private bool isDead = false;

    private readonly int maxHp = 50;

    private PEA_WitchSkill witchSkill = null;
    private AudioSource audioSource = null;

    public Image hpImage;
    public TMP_Text hpText;
    public GameObject probBody;
    public GameObject witchUI;
    public AudioClip damageSound;
    public ParticleSystem purpleEffect;

    public bool IsDead
    {
        get { return isDead; }
    }

    void Start()
    {
        hp = maxHp;
        hpImage.fillAmount = 1;
        witchSkill = GetComponent<PEA_WitchSkill>();
        audioSource = GetComponent<AudioSource>();

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
        if (photonView.IsMine)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Damage(10);
            }
        }
    }

    public void Damage(int damage)
    {
        hp -= damage;
        hpImage.fillAmount = (float)hp / maxHp;
        hpText.text = hp + "HP";
        audioSource.PlayOneShot(damageSound);
        purpleEffect.Play();

        if (hp <= 0 && !isDead)
        {
            //Die();
            photonView.RPC(nameof(Die), RpcTarget.All);
            GameManager.instance.WitchDie();
            Camera.main.transform.SetParent(null);
            Camera.main.transform.position = transform.position;
            Camera.main.gameObject.AddComponent<PEA_WatchCamera>();
        }
    }

    [PunRPC]
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
        //Camera.main.transform.SetParent(null);
        //Camera.main.transform.position = transform.position;
        //Camera.main.GetComponent<PEA_WatchCamera>().enabled = true;
        witchUI.SetActive(false);
    }
}