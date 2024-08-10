using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Photon.Realtime;
using ExitGames.Client.Photon.StructWrapping;
using HashTable = ExitGames.Client.Photon.Hashtable;
using System.Data.SqlTypes;

public class RoomManager : Photon.MonoBehaviour
{


    [SerializeField] private GameObject returnbutton;
    [SerializeField] private GameObject readybutton;
    [SerializeField] private Text mastername;
    [SerializeField] private Text sravename;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("1.0");

        PhotonNetwork.sendRate = 60;
        PhotonNetwork.sendRateOnSerialize = 60;

        PhotonNetwork.automaticallySyncScene = true;
        Gamesetting.score = 0;

        if (PhotonNetwork.player.IsMasterClient)
        {
            mastername.text=PhotonNetwork.player.NickName;
            Debug.Log("master");
        }
        else
        {
            sravename.text=PhotonNetwork.player.NickName;
            foreach (var player in PhotonNetwork.otherPlayers)
            {
                mastername.text=player.NickName;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.isMasterClient)
        {
            readybutton.SetActive(true);
            sravename.text = "空き";
            mastername.text = PhotonNetwork.player.NickName;
            HashTable hs = new HashTable();
            hs["CountDown"] = "30";
            hs["MasterName"] = mastername.text;
            hs.Add("Tfrag", 0);
            //hs.Add("Loadinfo", true);
            foreach (var player in PhotonNetwork.otherPlayers)
            {
                sravename.text = player.NickName;
            }
            if(sravename.text=="")
            {
                sravename.text = "空き";
            }
            hs["SraveName"] = sravename.text;
            PhotonNetwork.room.SetCustomProperties(hs);
        }
        else
        {
            readybutton.SetActive(false);
            foreach (var player in PhotonNetwork.otherPlayers)
            {
                mastername.text = player.NickName;
            }
        }
        Button mybutton = readybutton.GetComponent<Button>();
        if (sravename.text == "空き")
        {
            mybutton.interactable = false;
        }
        else if(sravename.text != "空き")
        {
            mybutton.interactable = true;
        }
    }

    public void OnClickReturnButton()
    {
        PhotonNetwork.LeaveRoom(this);
        SceneManager.LoadScene("Lobby");
    }

    public void OnClickReadyButton()
    {
        //ゲームへ遷移
        Gamesetting.mastername = mastername.text;
        Gamesetting.sravename = sravename.text;
        PhotonNetwork.LoadLevel("Game");
    }

    /// <summary>
    /// event:誰かプレイヤーが接続された
    /// </summary>
    /// <param name="player">Player.</param>
    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("OnPhotonPlayerConnected");
        sravename.text=player.NickName;
        HashTable hs = new HashTable();
        hs["SraveName"] = sravename.text;
        PhotonNetwork.room.SetCustomProperties(hs);
    }

    /// <summary>
    /// event:誰かプレイヤーの接続が切れた
    /// </summary>
    /// <param name="player">Player.</param>
    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log("OnPhotonPlayerDisconnected");
        sravename.text = "空き";
    }

    /// <summary>
    /// event:マスタークライアントが切り替わった
    /// </summary>
    /// <param name="player">Player.</param>
    public void OnMasterClientSwitched(PhotonPlayer player)
    {

        Debug.Log("OnMasterClientSwitched");
        mastername.text = player.NickName;
        sravename.text = "空き";

    }
}