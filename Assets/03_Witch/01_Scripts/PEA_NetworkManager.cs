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


    public override void OnConnectedToMaster()
    {
        print("서버에 연결됨");
    }

    public void OnClickQuickPlay()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        print("로비 입장");
    }
}
