using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PEA_GameSceneUI : MonoBehaviour
{
    public static PEA_GameSceneUI instance = null;

    public GameObject selectTeam;
    public GameObject witchTeam;
    public GameObject hunterTeam;
    public TMP_Text witchCountText;
    public TMP_Text hunterCountText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void OnClickMenu()
    {

    }

    public void OnClickWitchTeam()
    {
        GameManager.instance.TeamToWitch();
        //SetTeamCountText(GameManager.instance.TeamToWitch());
    }

    public void OnClickHunterTeam()
    {
        GameManager.instance.TeamToHunter();
        //SetTeamCountText(GameManager.instance.TeamToHunter());
    }

    public void SetTeamCountText(ExitGames.Client.Photon.Hashtable hash)
    {
        witchCountText.text = "Player : " + (int)hash["Witch_Count"];
        hunterCountText.text = "Player : " + (int)hash["Hunter_Count"];
    }

    public void OnClickReady()
    {
        GameManager.instance.OnClickReady();
    }

    public void OnClickChangeTeam()
    {
        selectTeam.SetActive(true);
        witchTeam.SetActive(false);
        hunterTeam.SetActive(false);

        SetTeamCountText(GameManager.instance.OnClickChangeTeam());
    }

}
