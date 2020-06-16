using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomListManager : MonoBehaviourPunCallbacks
{
    public GameObject roomPrefab;
    public Transform gridLayout;//content; 

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var i in roomList)
        {
            GameObject newRoom = Instantiate(roomPrefab, gridLayout.position, Quaternion.identity);
            newRoom.GetComponentInChildren<Text>().text = i.Name;
            newRoom.transform.SetParent(gridLayout);
            Debug.Log(i.Name);
        }
    }
}
