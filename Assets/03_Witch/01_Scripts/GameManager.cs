using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
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

    // ∞‘¿” Ω√¿€ ¿¸ ¡ÿ∫Ò¥‹∞Ëø° « ø‰«— ∫ØºˆµÈ
    public GameObject readyUI;
    public Transform spawnPoint;
    public GameObject selectTeam;
    public GameObject witchTeam;
    public GameObject hunterTeam;

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
        PhotonNetwork.SerializationRate = 30;
        PEA_GameSceneUI.instance.SetTeamCountText(PhotonNetwork.CurrentRoom.CustomProperties);
    }

    void Update()
    {
        
    }

    public void TeamToWitch()
    {
        if (team != Team.Witch)
        {
            ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;

            hash["Witch_Count"] = (int)hash["Witch_Count"] + 1;
            //witchCountText.text = "Player : " + hash["Witch_Count"];

            if (team == Team.Hunter)
            {
                hash["Hunter_Count"] = (int)hash["Hunter_Count"] - 1;
                //hunterCountText.text = "Player : " + hash["Hunter_Count"];
            }

            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
            team = Team.Witch;
            selectTeam.SetActive(false);
            witchTeam.SetActive(true);
            print("∏∂≥‡∆¿ : " + (int)hash["Witch_Count"] + ", «Â≈Õ∆¿ : " + (int)hash["Hunter_Count"]);

            //return hash;
        }
        else
        {
            print("¿ÃπÃ ∏∂≥‡∆¿");
            //return null;
        }
    }

    public void  TeamToHunter()
    {
        if(team != Team.Hunter)
        {
            ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;

            hash["Hunter_Count"] = (int)hash["Hunter_Count"] + 1;
            //hunterCountText.text = "Player : " + (int)hash["Hunter_Count"];

            if (team == Team.Witch)
            {
                hash["Witch_Count"] = (int)hash["Witch_Count"] - 1;
                //witchCountText.text = "Player : " + (int)hash["Witch_Count"];
            }

            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
            team = Team.Hunter;
            selectTeam.SetActive(false);
            hunterTeam.SetActive(true);
            print("∏∂≥‡∆¿ : " + (int)hash["Witch_Count"] + ", «Â≈Õ∆¿ : " + (int)hash["Hunter_Count"]);
            //return hash;
        }
        else
        {
            print("¿ÃπÃ «Â≈Õ∆¿");
            //return null;
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

    public ExitGames.Client.Photon.Hashtable OnClickChangeTeam()
    {
        if(isReady)
        {
            OnClickReady();
        }

        ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;

        switch (team)
        {
            case Team.Witch:
                hash["Witch_Count"] = (int)hash["Witch_Count"] - 1;
                break;
            case Team.Hunter:
                hash["Hunter_Count"] = (int)hash["Hunter_Count"] - 1;
                break;
        }

        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        print("∏∂≥‡∆¿ : " + (int)hash["Witch_Count"] + ", «Â≈Õ∆¿ : " + (int)hash["Hunter_Count"]);
        team = Team.None;

        return hash;
    }

    public void LeaveRoom()
    {
        if(team != Team.None)
        {
            ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;

            if(team == Team.Witch)
            {
                hash["Witch_Count"] = (int)hash["Witch_Count"] - 1;
            }
            else
            {
                hash["Hunter_Count"] = (int)hash["Hunter_Count"] - 1;
            }

            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        }

        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("PEA_Main");
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);

        PEA_GameSceneUI.instance.SetTeamCountText(PhotonNetwork.CurrentRoom.CustomProperties);

        if (((string)propertiesThatChanged["Room_State"]).Equals("Waiting") && (int)propertiesThatChanged["Ready_Count"] == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            readyUI.SetActive(false);
            ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;
            hash["Room_State"] = "Playing";
            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
            switch (team)
            {
                case Team.Witch:
                    PhotonNetwork.Instantiate("Witch", spawnPoint.position, spawnPoint.rotation);
                    break;
                case Team.Hunter:
                    PhotonNetwork.Instantiate("Hunter", spawnPoint.position, spawnPoint.rotation);
                    break;
            }
        }
    }
}
