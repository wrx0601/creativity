using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class RoomButton : MonoBehaviour
{
    [SerializeField] private GameObject roombutton;
    public void OnClickJoinButton()
    {
        Gamesetting.selectroomname = roombutton.name;
        Debug.Log(Gamesetting.selectroomname);
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        Debug.Log(string.Format("room count is {0}", rooms.Length));
        foreach (RoomInfo info in rooms)
        {
            if (info.Name == roombutton.name)
            {
                Debug.Log(info.Name);
                Debug.Log("PlayerCount:" + info.PlayerCount);
                Debug.Log("MaxPlayer:" + info.MaxPlayers);
                Debug.Log(info.Name + ":Master:" + info.CustomProperties["MasterName"]);
                Debug.Log(info.Name + ":Membar:" + info.CustomProperties["SraveName"]);
                Debug.Log(info.Name + ":CountDown:" + info.CustomProperties["CountDown"]);
                Gamesetting.selectroomname = info.Name;
                Gamesetting.selectroommaster = (string)info.CustomProperties["MasterName"];
                Gamesetting.selectroomsrave = (string)info.CustomProperties["SraveName"];
                Gamesetting.selectroomcountdown = (string)info.CustomProperties["CountDown"];
                LobbyManager.instance.OnSelectedRoom();
            }

        }

        //PhotonNetwork.JoinRoom(roombutton.name);
        //SceneManager.LoadScene("Room");
    }
    
}
