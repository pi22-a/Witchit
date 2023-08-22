using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PEA_NetworkManager : MonoBehaviourPunCallbacks
{
    public static PEA_NetworkManager instance = null;

    public GameObject signInUI;
    public GameObject lobbyUI;
    public GameObject loadingUI;
    public TMP_InputField inputNicaname;
    public TMP_Text nicknameText;
    public Button enterButton;

    void Start()
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

    void Update()
    {
        
    }

    public void OnNicknameValueChanged()
    {
        if(nicknameText.text.Length > 0)
        {
            enterButton.interactable = true;
        }
        else
        {
            enterButton.interactable = false;
        }
    }

    public void OnClickEnter()
    {
        PhotonNetwork.NickName = nicknameText.text;
        loadingUI.SetActive(true);
        PhotonNetwork.ConnectUsingSettings();
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
        signInUI.SetActive(false);
        lobbyUI.SetActive(true);
        loadingUI.SetActive(false);
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();

        SoundManager.instance.PlayBGM(SoundManager.BGM.Lobby);

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
