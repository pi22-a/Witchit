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
    public ParticleSystem dieEffect;
    public GameObject nicknameCanvas;
    public GameObject dieSphere;
    public PEA_Camera cam;
    public GameObject message;
    public Transform messageWindow;

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
        cam = Camera.main.transform.parent.GetComponent<PEA_Camera>();

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
        //if (photonView.IsMine)
        //{
        //    if (Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        Damage(10);
        //    }
        //}
    }

    public void Damage(int damage, string hunterNickname = "")
    { 
        hp -= damage;
        hpImage.fillAmount = (float)hp / maxHp;
        hpText.text = hp + "HP";
        audioSource.PlayOneShot(damageSound);

        if (hp <= 0 && !isDead)
        {
            //Die();
            if(photonView.IsMine)
            {
                cam.Die();
            }
            photonView.RPC(nameof(Die), RpcTarget.All);
            GameManager.instance.WitchDie();
            Camera.main.transform.SetParent(null);
            Camera.main.transform.position = transform.position;
            // Camera.main.gameObject.AddComponent<PEA_WatchCamera>();

            print(hunterNickname + " 이/가 " + photonView.Owner.NickName + "을/를 죽였습니다");
            photonView.RPC(nameof(DieMessage), RpcTarget.All, hunterNickname, photonView.Owner.NickName);
        }
        else
        {
            photonView.RPC(nameof(DamageEffect), RpcTarget.All);
        }
    }

    [PunRPC]
    private void DieMessage(string hunterNickname, string witchNickname )
    {
        GameManager.instance.ShowDieMessage(hunterNickname, witchNickname);
    }

    [PunRPC]
    private void DamageEffect()
    {
        purpleEffect.Play();
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
        dieEffect.Play();
        nicknameCanvas.SetActive(false);
        dieSphere.SetActive(true);
        witchUI.SetActive(false);
    }
}