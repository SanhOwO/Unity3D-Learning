using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject button;
    public GameObject gridLayout;
    
    public void Read2Play()
    {
        button.SetActive(false);
        gridLayout.SetActive(false);
        PhotonNetwork.Instantiate("player", new Vector3(-6, 0, 0), Quaternion.identity, 0);
    }
}
