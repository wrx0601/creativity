using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;
using HashTable = ExitGames.Client.Photon.Hashtable;
using System;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private Text nametext;
    [SerializeField] private Button createbutton;
    [SerializeField] private Button joinbutton;
    [SerializeField] private GameObject RoomButton;
    [SerializeField] private Button returnbutton;
    [SerializeField] private GameObject Roomlist;
    [SerializeField] private Text roommaster;
    [SerializeField] private Text roommember;
    [SerializeField] private Text roomname;
    [SerializeField] private Text roomtimer;

    public static LobbyManager instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }





    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.JoinLobby();
        nametext.text = nametext.text + Gamesetting.username;
        PhotonNetwork.playerName = Gamesetting.username;
        PhotonNetwork.player.NickName = Gamesetting.username;
        Debug.Log(PhotonNetwork.playerName);
        Debug.Log(PhotonNetwork.player.NickName);

        roomname.text = "";
        roommaster.text = "";
        roommember.text = "";
        roomtimer.text = "";
        joinbutton.interactable =false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Gamesetting.selectroomname!="")
        {
            joinbutton.interactable = true;
        }
    }
    public void OnClickReturnButton()
    {
        PhotonNetwork.LeaveLobby();
        SceneManager.LoadScene("Home");
        Gamesetting.mode = "";
        Debug.Log("Mode:Selecting");
    }

    [System.Obsolete]
    public void OnClickCreateButton()
    {
        //Debug.Log();
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        Debug.Log(string.Format("room count is {0}", rooms.Length));
        int roomnum = rooms.Length;
        int v = roomnum + 1;
        RoomOptions roomOptions = new RoomOptions()
        {
            MaxPlayers = 2,
            publishUserId = true,
            CustomRoomPropertiesForLobby = new string[] { "MasterName", "SraveName", "CountDown" }
        };
        PhotonNetwork.CreateRoom("Room" + v, roomOptions, null);
        SceneManager.LoadScene("Room");

    }

    public void OnClickJoinButton()
    {
        PhotonNetwork.JoinRoom(Gamesetting.selectroomname);
        SceneManager.LoadScene("Room");
    }

    public void OnSelectedRoom()
    {
        if (Gamesetting.selectroomname != null)
        {
            roomname.text = "ルーム名:" + Gamesetting.selectroomname;
            roommaster.text = "メンバー1:" + Gamesetting.selectroommaster;
            roommember.text = "メンバー2:" + Gamesetting.selectroomsrave;
            roomtimer.text = "タイマー:" + Gamesetting.selectroomcountdown + "秒";
        }
    }
}
