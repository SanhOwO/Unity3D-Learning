using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class NetworkLauncher : MonoBehaviourPunCallbacks
{
    static NetworkLauncher instance;

    public GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
        instance = new NetworkLauncher();
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
       
        Debug.Log("Welcome");

        UI.SetActive(true);
        PhotonNetwork.JoinOrCreateRoom("SanhOwO", new Photon.Realtime.RoomOptions() { MaxPlayers = 4 }, default);
        PhotonNetwork.JoinLobby();//加入游戏大厅
        if (PhotonNetwork.InLobby)
            UI.SetActive(true);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PhotonNetwork.Instantiate("player", new Vector3(-6, 0, 0), Quaternion.identity, 0);
    }
    
    public static void PlayButton()
    {
        if (PhotonNetwork.InLobby)
            Debug.Log("1231231231231");
        //UI.SetActive(false);
        PhotonNetwork.NickName = "SanhOwO";

        RoomOptions options = new RoomOptions { MaxPlayers = 4 };
        //PhotonNetwork.JoinOrCreateRoom("S's ROOM", options, default);
        PhotonNetwork.LoadLevel(1);

        //if (PhotonNetwork.InLobby)
        //    return;

    }

    
}
