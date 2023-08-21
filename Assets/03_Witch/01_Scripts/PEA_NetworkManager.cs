using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PEA_NetworkManager : MonoBehaviourPunCallbacks
{
    public static PEA_NetworkManager instance = null;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Destroy(gameObject);
        }

    }

    void Update()
    {
        
    }

    public void OnClickQuickMatch()
    {
        RoomOptions roomOption = new RoomOptions();
        roomOption.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("WitchIt", roomOption, TypedLobby.Default);
    }

    public override void OnConnectedToMaster()
    {
        print("서버에 연결됨");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        print("로비 입장");
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        print("방 생성");

        ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
        hash["Room_State"] = "Waiting";
        hash["Witch_Alive"] = 0;
        hash["Witch_Count"] = 0;
        hash["Hunter_Count"] = 0;
        hash["Ready_Count"] = 0;

        PhotonNetwork.CurrentRoom.SetCustomProperties(hash);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        print("방 입장");
        PhotonNetwork.LoadLevel("PEA_GameRoom");
    }
}
