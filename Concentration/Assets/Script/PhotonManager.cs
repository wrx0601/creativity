using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class PhotonManager : Photon.MonoBehaviour
{
    //private const string	PHOTON_GAME_VER		= "v1.0";		//バージョンコード
    //private const string	GAMEROOM_NAME		= "myroom01";	//部屋のデフォルト名
    //private const int		GAMEROOM_LIMIT		= 2;			//部屋の制限人数

    public bool isInitPhoton = false;   //photon初期化済み？
    public bool isJoinedLobby = false;  //ロビー入場済み？
    public bool isJoinedRoom = false;   //ルーム入室済み？
    public bool isRoomMake = false; //自分でルームを作成した？

    //public	List<photonRoomInfo>	roomList	= new List<photonRoomInfo> ();

    //public Action<string> debug.Logage	= null;	//デバッグメッセージ表示用コールバック

    // Start is called before the first frame update

    [SerializeField] private GameObject RoomButton;
    [SerializeField] private GameObject Roomlist;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("1.0");

        PhotonNetwork.sendRate = 60;
        PhotonNetwork.sendRateOnSerialize = 60;

        PhotonNetwork.automaticallySyncScene = true;


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnClickGameStartButton()
    {
        PhotonNetwork.LoadLevel("GameScene");
    }

    //---------------------------------------------
    // photon callback

    /// <summary>
    /// event:photonに接続した
    /// </summary>
    public void OnConnectedToPhoton()
    {
        Debug.Log("OnConnectedToPhoton");
        isInitPhoton = true;    //photon初期化済み？
    }

    /// <summary>
    /// event:photonが切断した
    /// </summary>
    public void OnDisconnectedFromPhoton()
    {
        Debug.Log("OnDisconnectedFromPhoton");
    }

    /// <summary>
    /// event:接続失敗
    /// </summary>
    public void OnConnectionFail()
    {
        Debug.Log("OnConnectionFail");
    }

    /// <summary>
    /// event:photon接続失敗
    /// </summary>
    /// <param name="parameters">Parameters.</param>
    public void OnFailedToConnectToPhoton(object parameters)
    {
        Debug.Log("OnFailedToConnectToPhoton");
    }

    /// <summary>
    /// event:ロビー入室
    /// </summary>
    public void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        isJoinedLobby = true;
    }

    /// <summary>
    /// event:ロビー退室
    /// </summary>
    public void OnLeftLobby()
    {
        Debug.Log("OnLeftLobby");
    }

    /// <summary>
    /// Raises the connected to master event.
    /// autoJoinLobby が true 時には OnJoinedLobby が代わりに呼ばれる。
    /// </summary>
    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
    }

    /// <summary>
    /// event:ルームリストが更新された
    /// </summary>
    public void OnReceivedRoomListUpdate()
    {
        Debug.Log("OnReceivedRoomListUpdate");

        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        Debug.Log(string.Format("room count is {0}", rooms.Length));
        foreach (RoomInfo info in rooms)
        {
            Debug.Log(info.Name);
            Debug.Log("PlayerCount:"+info.PlayerCount);
            Debug.Log("MaxPlayer:"+info.MaxPlayers);
            Debug.Log(info.Name + ":RoomMaster:" + info.CustomProperties["RoomMaster"]);
        }


        //LobbyManager lobbyManager = GetComponent<LobbyManager>();
        foreach (RoomInfo r in rooms)
        {
            //プレイヤーが存在しているルーム
            if (r.PlayerCount > 0)
            {
                RoomButtonCreate(r);
            }
            else
            {
                RoomButtonDelete(r);
            }
        }
        //lobbyManager.OnRoomListUpdate();
    //ルーム一覧を取得
    //CheckRoomList();
}

    /// <summary>
    /// event:ルーム作成
    /// </summary>
    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        Debug.Log(string.Format("Name:{0}", PhotonNetwork.room.Name));
    }

    /// <summary>
    /// event:ルーム作成失敗
    /// </summary>
    public void OnPhotonCreateRoomFailed()
    {
        Debug.Log("OnPhotonCreateRoomFailed");
    }

    /// <summary>
    /// event:ルーム入室
    /// </summary>
    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        Debug.Log(string.Format("Name:{0}", PhotonNetwork.room.Name));

        isJoinedRoom = true;    //ルーム入室済み？
    }

    /// <summary>
    /// event:ルーム入室失敗
    /// </summary>
    /// <param name="cause">Cause.</param>
    public void OnPhotonJoinRoomFailed(object[] cause)
    {
        Debug.Log("OnPhotonJoinRoomFailed");
    }

    /// <summary>
    /// event:ランダム入室失敗
    /// </summary>
    public void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnPhotonRandomJoinFailed");
    }

    /// <summary>
    /// event:ルーム退室コールバック
    /// </summary>
    public void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
    }

    /// <summary>
    /// event:誰かプレイヤーが接続された
    /// </summary>
    /// <param name="player">Player.</param>
    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("OnPhotonPlayerConnected");
    }

    /// <summary>
    /// event:誰かプレイヤーの接続が切れた
    /// </summary>
    /// <param name="player">Player.</param>
    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log("OnPhotonPlayerDisconnected");
    }

    /// <summary>
    /// event:マスタークライアントが切り替わった
    /// </summary>
    /// <param name="player">Player.</param>
    public void OnMasterClientSwitched(PhotonPlayer player)
    {
        Debug.Log("OnMasterClientSwitched");
    }

    //ルームボタンの作成
    public void RoomButtonCreate(RoomInfo r)
    {

        //すでに存在していたのなら情報の更新
        if (Roomlist.transform.Find(r.Name))
        {
            RoomInfoUpdate(Roomlist.transform.Find(r.Name).gameObject, r);
        }
        //新しく作られたルームならばボタンの作成
        else
        {
            var roomButton = (GameObject)Instantiate(RoomButton);
            roomButton.transform.SetParent(Roomlist.transform, false);
            RoomInfoUpdate(roomButton, r);
            //生成したボタンの名前を作成するルームの名前にする
            roomButton.name = r.Name;
        }
    }

    //ルームボタンの削除
    public void RoomButtonDelete(RoomInfo r)
    {
        //ボタンが存在すれば削除
        if (Roomlist.transform.Find(r.Name))
        {
            GameObject.Destroy(gameObject.transform.Find(r.Name).gameObject);
        }
    }

    //ルームボタンのInfoの更新
    public void RoomInfoUpdate(GameObject button, RoomInfo info)
    {
        foreach (Text t in button.GetComponentsInChildren<Text>())
        {
            if (t.name == "RoomName")
            {
                t.text = info.Name;
            }
            else if (t.name == "PlayerCount")
            {
                t.text = info.PlayerCount.ToString() + "/" + info.MaxPlayers.ToString();
            }
        }
    }

}
