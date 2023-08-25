using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

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
    public PEA_Door hunterDoor;
    public Transform witchSpawnPoint;
    public Transform hunterSpawnPoint;
    //public GameObject selectTeam;
    //public GameObject witchTeam;
    //public GameObject hunterTeam;

    public GameObject message;
    public Transform messageWindow;

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
        Cursor.lockState = CursorLockMode.Confined;
        PhotonNetwork.SerializationRate = 30;
        PEA_GameSceneUI.instance.SetTeamCountText(PhotonNetwork.CurrentRoom.CustomProperties);
        //SoundManager.instance.PlayBGM(SoundManager.BGM.Ready);

        ExitGames.Client.Photon.Hashtable playerProperty = new ExitGames.Client.Photon.Hashtable();
        playerProperty["Team"] = "None";
        PhotonNetwork.SetPlayerCustomProperties(playerProperty);
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
            ExitGames.Client.Photon.Hashtable playerProperty =  PhotonNetwork.LocalPlayer.CustomProperties;

            playerProperty["Team"] = "Witch";

            PhotonNetwork.SetPlayerCustomProperties(playerProperty);

            ExitGames.Client.Photon.Hashtable roomProperty = PhotonNetwork.CurrentRoom.CustomProperties;

            roomProperty["Witch_Count"] = (int)roomProperty["Witch_Count"] + 1;

            if (team == Team.Hunter)
            {
                roomProperty["Hunter_Count"] = (int)roomProperty["Hunter_Count"] - 1;
            }

            PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperty);
            team = Team.Witch;
            //selectTeam.SetActive(false);
            //witchTeam.SetActive(true);
            print("∏∂≥‡∆¿ : " + (int)roomProperty["Witch_Count"] + ", «Â≈Õ∆¿ : " + (int)roomProperty["Hunter_Count"]);
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
            ExitGames.Client.Photon.Hashtable playerProperty = PhotonNetwork.LocalPlayer.CustomProperties;

            playerProperty["Team"] = "Hunter";

            PhotonNetwork.SetPlayerCustomProperties(playerProperty);

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

        ExitGames.Client.Photon.Hashtable playerProperty = PhotonNetwork.LocalPlayer.CustomProperties;

        playerProperty["Team"] = "Watch";

        PhotonNetwork.SetPlayerCustomProperties(playerProperty);

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
        else if(((string)propertiesThatChanged["Room_State"]).Equals("Playing"))
        {
            PEA_GameSceneUI.instance.SetAliveWitchCountText((int)propertiesThatChanged["Witch_Alive"]);
            if ((int)propertiesThatChanged["Witch_Alive"] == 0)
            {
                HunterWin();
            }
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
                PhotonNetwork.Instantiate("Witch", witchSpawnPoint.position, witchSpawnPoint.rotation);
                Camera.main.cullingMask = ~(1 << LayerMask.NameToLayer("HunterNickname"));
                break;
            case Team.Hunter:
                PhotonNetwork.Instantiate("Hunter", hunterSpawnPoint.position, hunterSpawnPoint.rotation);
                Destroy(Camera.main);
                //Camera.main.cullingMask = ~(1 << LayerMask.NameToLayer("WitchNickname"));
                break;
        }

        PEA_GameSceneUI.instance.GameStart();
        room_State = Room_State.Playing;
        SoundManager.instance.PlayBGM(SoundManager.BGM.Ready);
        print("playing");

        Cursor.visible = false;
        //SoundManager.instance.StopBGM();
    }

    public void ShowDieMessage(string hunterNickname, string witchNickname)
    {
        GameObject msg = Instantiate(message, messageWindow);
        msg.GetComponent<TMP_Text>().text = hunterNickname + " killed " + witchNickname;
    }

    public void HunterGo()
    {
        hunterDoor.enabled = true;
    }

    public void WitchDie()
    {
        ExitGames.Client.Photon.Hashtable playerProperty = PhotonNetwork.LocalPlayer.CustomProperties;
        playerProperty["Team"] = "Watch";
        PhotonNetwork.SetPlayerCustomProperties(playerProperty);

        ExitGames.Client.Photon.Hashtable hash = PhotonNetwork.CurrentRoom.CustomProperties;
        hash["Witch_Alive"] = (int)hash["Witch_Alive"] - 1;
        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }

    public void WitchWin()
    {
        GameOver();
    }

    private void HunterWin()
    {
        GameOver();
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
        Cursor.visible = true;
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

    private void SetPlayerList()
    {
        string myTeam = (string)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
        List<string> players = new List<string>();
        List<string> teams = new List<string>();
        List<string> myTeams = new List<string>();

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            players.Add(player.NickName);
            teams.Add((string)player.CustomProperties["Team"]);
            if (((string)player.CustomProperties["Team"]).Equals(myTeam))
            {
                myTeams.Add(player.NickName);
            }
        }

        if (room_State == Room_State.Waiting)
        {
            PEA_GameSceneUI.instance.SetMyTeamList(myTeam.Equals("Witch") ? true : false, myTeams.ToArray());
        }

        PEA_GameSceneUI.instance.SetPlayerList(players.ToArray(), teams.ToArray());
    }

    private void ShowPlayerInOutMessage(string playerNickname, bool isPlayerIn)
    {
        GameObject msg = Instantiate(message, messageWindow);
        msg.GetComponent<TMP_Text>().text = playerNickname + (isPlayerIn ? " entered Room" : " left Room");
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        SetPlayerList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        ShowPlayerInOutMessage(newPlayer.NickName, true);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        ShowPlayerInOutMessage(otherPlayer.NickName, false);

        SetPlayerList();
    }
}
