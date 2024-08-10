using ExitGames.Client.Photon.StructWrapping;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using HashTable = ExitGames.Client.Photon.Hashtable;

public class PhotonManager : Photon.MonoBehaviour
{
    //private const string	PHOTON_GAME_VER		= "v1.0";		//�o�[�W�����R�[�h
    //private const string	GAMEROOM_NAME		= "myroom01";	//�����̃f�t�H���g��
    //private const int		GAMEROOM_LIMIT		= 2;			//�����̐����l��

    public bool isInitPhoton = false;   //photon�������ς݁H
    public bool isJoinedLobby = false;  //���r�[����ς݁H
    public bool isJoinedRoom = false;   //���[�������ς݁H
    public bool isRoomMake = false; //�����Ń��[�����쐬�����H

    //public	List<photonRoomInfo>	roomList	= new List<photonRoomInfo> ();

    //public Action<string> debug.Logage	= null;	//�f�o�b�O���b�Z�[�W�\���p�R�[���o�b�N

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
    /// event:photon�ɐڑ�����
    /// </summary>
    public void OnConnectedToPhoton()
    {
        Debug.Log("OnConnectedToPhoton");
        isInitPhoton = true;    //photon�������ς݁H
    }

    /// <summary>
    /// event:photon���ؒf����
    /// </summary>
    public void OnDisconnectedFromPhoton()
    {
        Debug.Log("OnDisconnectedFromPhoton");
    }

    /// <summary>
    /// event:�ڑ����s
    /// </summary>
    public void OnConnectionFail()
    {
        Debug.Log("OnConnectionFail");
    }

    /// <summary>
    /// event:photon�ڑ����s
    /// </summary>
    /// <param name="parameters">Parameters.</param>
    public void OnFailedToConnectToPhoton(object parameters)
    {
        Debug.Log("OnFailedToConnectToPhoton");
    }

    /// <summary>
    /// event:���r�[����
    /// </summary>
    public void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
        isJoinedLobby = true;
    }

    /// <summary>
    /// event:���r�[�ގ�
    /// </summary>
    public void OnLeftLobby()
    {
        Debug.Log("OnLeftLobby");
    }

    /// <summary>
    /// Raises the connected to master event.
    /// autoJoinLobby �� true ���ɂ� OnJoinedLobby ������ɌĂ΂��B
    /// </summary>
    public void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
    }

    /// <summary>
    /// event:���[�����X�g���X�V���ꂽ
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
            //�v���C���[�����݂��Ă��郋�[��
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
    //���[���ꗗ���擾
    //CheckRoomList();
}

    /// <summary>
    /// event:���[���쐬
    /// </summary>
    public void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        Debug.Log(string.Format("Name:{0}", PhotonNetwork.room.Name));
    }

    /// <summary>
    /// event:���[���쐬���s
    /// </summary>
    public void OnPhotonCreateRoomFailed()
    {
        Debug.Log("OnPhotonCreateRoomFailed");
    }

    /// <summary>
    /// event:���[������
    /// </summary>
    public void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");
        Debug.Log(string.Format("Name:{0}", PhotonNetwork.room.Name));

        isJoinedRoom = true;    //���[�������ς݁H
    }

    /// <summary>
    /// event:���[���������s
    /// </summary>
    /// <param name="cause">Cause.</param>
    public void OnPhotonJoinRoomFailed(object[] cause)
    {
        Debug.Log("OnPhotonJoinRoomFailed");
    }

    /// <summary>
    /// event:�����_���������s
    /// </summary>
    public void OnPhotonRandomJoinFailed()
    {
        Debug.Log("OnPhotonRandomJoinFailed");
    }

    /// <summary>
    /// event:���[���ގ��R�[���o�b�N
    /// </summary>
    public void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom");
    }

    /// <summary>
    /// event:�N���v���C���[���ڑ����ꂽ
    /// </summary>
    /// <param name="player">Player.</param>
    public void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        Debug.Log("OnPhotonPlayerConnected");
    }

    /// <summary>
    /// event:�N���v���C���[�̐ڑ����؂ꂽ
    /// </summary>
    /// <param name="player">Player.</param>
    public void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        Debug.Log("OnPhotonPlayerDisconnected");
    }

    /// <summary>
    /// event:�}�X�^�[�N���C�A���g���؂�ւ����
    /// </summary>
    /// <param name="player">Player.</param>
    public void OnMasterClientSwitched(PhotonPlayer player)
    {
        Debug.Log("OnMasterClientSwitched");
    }

    //���[���{�^���̍쐬
    public void RoomButtonCreate(RoomInfo r)
    {

        //���łɑ��݂��Ă����̂Ȃ���̍X�V
        if (Roomlist.transform.Find(r.Name))
        {
            RoomInfoUpdate(Roomlist.transform.Find(r.Name).gameObject, r);
        }
        //�V�������ꂽ���[���Ȃ�΃{�^���̍쐬
        else
        {
            var roomButton = (GameObject)Instantiate(RoomButton);
            roomButton.transform.SetParent(Roomlist.transform, false);
            RoomInfoUpdate(roomButton, r);
            //���������{�^���̖��O���쐬���郋�[���̖��O�ɂ���
            roomButton.name = r.Name;
        }
    }

    //���[���{�^���̍폜
    public void RoomButtonDelete(RoomInfo r)
    {
        //�{�^�������݂���΍폜
        if (Roomlist.transform.Find(r.Name))
        {
            GameObject.Destroy(gameObject.transform.Find(r.Name).gameObject);
        }
    }

    //���[���{�^����Info�̍X�V
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
