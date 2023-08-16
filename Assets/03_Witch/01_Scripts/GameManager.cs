using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    private enum Team
    {
        None,
        Witch,
        Hunter
    }

    private Team team = Team.None;

    private bool isReady = false;

    public Transform spawnPoint;
    public GameObject selectTeam;
    public GameObject witchTeam;

    private void Awake()
    {
        if(instance == null)
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

    public void OnClickWitch()
    {
        if(team != Team.Witch)
        {
            ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;

            hash["Witch_Count"] = (int)hash["Witch_Count"] + 1;

            if(team == Team.Hunter)
            {
                hash["Hunter_Count"] = (int)hash["Hunter_Count"] - 1; 
            }

            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
            team = Team.Witch;
            selectTeam.SetActive(false);
            witchTeam.SetActive(true);
            print("∏∂≥‡∆¿ : " + (int)hash["Witch_Count"] + ", «Â≈Õ∆¿ : " + (int)hash["Hunter_Count"]);
        }
        else
        {
            print("¿ÃπÃ ∏∂≥‡∆¿");
        }
    }

    public void OnClickHunter()
    {
        if(team != Team.Hunter)
        {
            ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;

            hash["Hunter_Count"] = (int)hash["Hunter_Count"] + 1;

            if (team == Team.Witch)
            {
                hash["Witch_Count"] = (int)hash["Witch_Count"] - 1;
            }

            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
            team = Team.Hunter;
            print("∏∂≥‡∆¿ : " + (int)hash["Witch_Count"] + ", «Â≈Õ∆¿ : " + (int)hash["Hunter_Count"]);
        }
        else
        {
            print("¿ÃπÃ «Â≈Õ∆¿");
        }
    }

    public void OnClickReady()
    {
        ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;

        isReady = !isReady;
        hash["Ready_Count"] = (int)hash["Ready_Count"] + (isReady ? 1 : -1);

        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        print("¡ÿ∫Òµ» «√∑π¿ÃæÓ :  : " + (int)hash["Ready_Count"] + ", ≥ª ¡ÿ∫ÒªÛ≈¬ : " + isReady);
    }
}
