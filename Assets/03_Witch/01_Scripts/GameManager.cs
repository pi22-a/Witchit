using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance = null;

    private enum Team
    {
        None,
        Witch,
        Hunter
    }

    public enum Room_State
    {
        Waiting,
        Playing,
        Over
    }

    private Team team = Team.None;
    private Room_State room_State = Room_State.Waiting;

    private bool isReady = false;

    public bool IsReady
    {
        get { return isReady; }
    }

    public Room_State RoomState
    {
        get { return room_State; }
    }

    // ∞‘¿” Ω√¿€ ¿¸ ¡ÿ∫Ò¥‹∞Ëø° « ø‰«— ∫ØºˆµÈ
    public GameObject readyUI;
    public Transform spawnPoint;
    //public GameObject selectTeam;
    //public GameObject witchTeam;
    //public GameObject hunterTeam;

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
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            WitchDie();
        }
    }


    // ∞‘¿” Ω√¿€ ¿¸ ¡ÿ∫Ò¥‹∞Ë «‘ºˆµÈ
    public void TeamToWitch()
    {
        if (team != Team.Witch)
        {
            ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;

            hash["Witch_Count"] = (int)hash["Witch_Count"] + 1;

            if (team == Team.Hunter)
            {
                hash["Hunter_Count"] = (int)hash["Hunter_Count"] - 1;
            }

            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
            team = Team.Witch;
            //selectTeam.SetActive(false);
            //witchTeam.SetActive(true);
            print("∏∂≥‡∆¿ : " + (int)hash["Witch_Count"] + ", «Â≈Õ∆¿ : " + (int)hash["Hunter_Count"]);

        }
        else
        {
            print("¿ÃπÃ ∏∂≥‡∆¿");
        }
    }

    public void  TeamToHunter()
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
            //selectTeam.SetActive(false);
            //hunterTeam.SetActive(true);
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
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("PEA_Main");
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnRoomPropertiesUpdate(propertiesThatChanged);

        PEA_GameSceneUI.instance.SetTeamCountText(PhotonNetwork.CurrentRoom.CustomProperties);

        if (((string)propertiesThatChanged["Room_State"]).Equals("Waiting") && (int)propertiesThatChanged["Ready_Count"] == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            GameStart();
        }
        else if(((string)propertiesThatChanged["Room_State"]).Equals("Playing") && (int)propertiesThatChanged["Witch_Alive"] == 0)
        {
            HunterWin();
        }
    }

    private void GameStart()
    {
        ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;
        hash["Room_State"] = "Playing";
        hash["Witch_Alive"] = (int)hash["Witch_Count"];
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

        PEA_GameSceneUI.instance.GameStart();
        room_State = Room_State.Playing;
        SoundManager.instance.StopBGM();
    }

    public void WitchDie()
    {
        print("pppppp");
        ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;
        hash["Witch_Alive"] = (int)hash["Witch_Alive"] - 1;
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
        print((int)hash["Witch_Alive"]);
    }

    public void WitchWin()
    {
        GameOver();
    }

    private void HunterWin()
    {
        PEA_GameSceneUI.instance.HunterWin();
    }

    public void GameOver()
    {
        ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;
        hash["Room_State"] = "Over";
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);

        room_State = Room_State.Over;
        PEA_GameSceneUI.instance.GameOver();
        SoundManager.instance.PlayBGM(SoundManager.BGM.Lobby);
    }

    public void Restart()
    {
        PhotonNetwork.LoadLevel("PEA_GameRoom");

        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["Room_State"] = "Waiting";
        hash["Witch_Alive"] = 0;
        hash["Witch_Count"] = 0;
        hash["Hunter_Count"] = 0;
        hash["Ready_Count"] = 0;

        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }

    //private void OnApplicationQuit()
    //{
    //    print("1111");
    //    if (PhotonNetwork.IsConnected)
    //    {
    //        print("22222");
    //        if (team != Team.None)
    //        {
    //            ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;

    //            if (team == Team.Witch)
    //            {
    //                hash["Witch_Count"] = (int)hash["Witch_Count"] - 1;
    //            }
    //            else
    //            {
    //                hash["Hunter_Count"] = (int)hash["Hunter_Count"] - 1;
    //            }

    //            if (isReady)
    //            {
    //                hash["Ready_Cout"] = (int)hash["Ready_Count"] - 1;
    //            }

    //            PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    //            print((int)PhotonNetwork.CurrentRoom.CustomProperties["Witch_Count"]);
    //        }
    //    }
    //}
}
