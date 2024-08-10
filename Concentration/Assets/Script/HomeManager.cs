using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Collections;
using UnityEngine.SceneManagement;
using MiniJSON; // Json
using System;
using UnityEngine.Networking;
//using UnityEditor.VersionControl;
using ExitGames.Client.Photon.StructWrapping;
using System.Net;
//using UnityEditor.PackageManager.Requests;

public class HomeManager : MonoBehaviour
{
    [SerializeField] private Text nametext;
    [SerializeField] private Button singlebutton;
    [SerializeField] private Button multibutton;
    [SerializeField] private Button endbutton;
    [SerializeField] private Button easybutton;
    [SerializeField] private Button normalbutton;
    [SerializeField] private Button hardbutton;
    [SerializeField] private Button returnbutton;
    [SerializeField] private Canvas homecanvas;
    [SerializeField] private Canvas singlecanvas;
    [SerializeField] private Canvas scorecanvas;
    [SerializeField] private Text myeasyscore;
    [SerializeField] private Text mynormalscore;
    [SerializeField] private Text myhardscore;
    [SerializeField] private Text topeasyscore;
    [SerializeField] private Text topnormalscore;
    [SerializeField] private Text tophardscore;



    // Start is called before the first frame update
    void Start()
    {
        nametext.text =nametext.text + Gamesetting.username;

        homecanvas.enabled =true;
        singlecanvas.enabled = false;
        scorecanvas.enabled =false;
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        //UnityWebRequest.chunkedTransfer = true;

        Debug.Log(Gamesetting.mode);
        Debug.Log(Gamesetting.difficulty);
        if (Gamesetting.scorestr != "")
        {
            SetJsonFromWWW();

            Gamesetting.difficulty = "";
            Gamesetting.score = 0;
            Gamesetting.mode = "";
            Gamesetting.scorestr = "";
        }
        GetJsonFromWebRequest();
        OnClickShowMemberList();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnClickSingleButton()
    {
        homecanvas.enabled = false;
        singlecanvas.enabled = true;
        scorecanvas.enabled = true;
        Gamesetting.mode = "Single";
        Debug.Log("Mode:"+Gamesetting.mode);
    }
    public void OnClickEasyButton()
    {
        //�Q�[���ւ̑J��
        Gamesetting.difficulty = "Easy";
        SceneManager.LoadScene("Game");
        Debug.Log("��Փx:"+Gamesetting.difficulty);
    }
    public void OnClickNomalButton()
    {
        //�Q�[���ւ̑J��
        Gamesetting.difficulty = "Normal";
        SceneManager.LoadScene("Game");
        Debug.Log("��Փx:" + Gamesetting.difficulty);
    }
    public void OnClickHardButton()
    {
        //�Q�[���ւ̑J��
        Gamesetting.difficulty = "Hard";
        SceneManager.LoadScene("Game");
        Debug.Log("��Փx:" + Gamesetting.difficulty);
    }
    public void OnClickReturnButton()
    {
        homecanvas.enabled = true;
        singlecanvas.enabled = false;
        scorecanvas.enabled = false;
        Gamesetting.mode = "";
        Debug.Log("Mode:Selecting");
    }
    public void OnClickEndButton()
    {
        //�Q�[���I��
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        Debug.Log("END");
    }
    public void OnClickMultiButton()
    {
        //���r�[�ւ̑J��
        Gamesetting.mode = "Multi";
        SceneManager.LoadScene("Lobby");
        Debug.Log("Mode:"+Gamesetting.mode);
    }


    //�ȉ��X�R�ADB�֌W

    private List<RankData> _RankList;

    /// <summary>
    /// Raises the click get json from www event.
    /// </summary>
    public void OnClickGetJsonFromWebRequest()
    {
        GetJsonFromWebRequest();
    }

    /// <summary>
    /// Raises the click show member list
    /// </summary>
    public void OnClickShowMemberList()
    {
        string sStrOutput = "";

        if (null == _RankList)
        {
            sStrOutput = "no list !";
        }
        else
        {
            //���X�g�̓��e��\��
            foreach (RankData memberOne in _RankList)
            {
                sStrOutput += $"Name:{memberOne.Name} Difficulty:{memberOne.Difficulty} Score:{memberOne.Score} \n";
            }
        }
        Debug.Log(sStrOutput);
        
    }


    /// <summary>
    /// Gets the json from www.
    /// </summary>
    private void GetJsonFromWebRequest()
    {
        // Www�𗘗p���� json �f�[�^�擾�����N�G�X�g����
        StartCoroutine(
            DownloadJson(
                CallbackWebRequestSuccess, // API�R�[�������������ۂɌĂ΂��֐����w��
                CallbackWebRequestFailed // API�R�[�������s�����ۂɌĂ΂��֐����w��
            )
        );
    }

    /// <summary>
    /// Callbacks the www success.
    /// </summary>
    /// <param name="response">Response.</param>
    private void CallbackWebRequestSuccess(string response)
    {
        //Json �̓��e�� MemberData�^�̃��X�g�Ƃ��ăf�R�[�h����B
        _RankList = DeserializeFromJson(response);

        //memberList �����Ƀf�R�[�h���ꂽ�����o�[���X�g���i�[�����B

        myeasyscore.text = Gamesetting.myeasy;
        mynormalscore.text=Gamesetting.mynormal;
        myhardscore.text=Gamesetting.myhard;
        topeasyscore.text=Gamesetting.easytopstr;
        topnormalscore.text=Gamesetting.normaltopstr;
        tophardscore.text = Gamesetting.hardtopstr;

        Debug.Log("RequestSuccess!");
    }

    /// <summary>
    /// Callbacks the www failed.
    /// </summary>
    private void CallbackWebRequestFailed()
    {
        // json�f�[�^�擾�Ɏ��s����
        Debug.Log("WebRequest Failed");
    }

    /// <summary>
    /// Downloads the json.
    /// </summary>
    /// <returns>The json.</returns>
    /// <param name="cbkSuccess">Cbk success.</param>
    /// <param name="cbkFailed">Cbk failed.</param>
    private IEnumerator DownloadJson(Action<string> cbkSuccess = null, Action cbkFailed = null)
    {
        UnityWebRequest www = UnityWebRequest.Get("http://localhost/concentration/singlerank/getMessages");
        yield return www.SendWebRequest();
        if (www.error != null)
        {
            //���X�|���X�G���[�̏ꍇ
            Debug.LogError(www.error);
            if (null != cbkFailed)
            {
                cbkFailed();
            }
        }
        else if (www.isDone)
        {
            // ���N�G�X�g�����̏ꍇ
            Debug.Log($"Success:{www.downloadHandler.text}");
            if (null != cbkSuccess)
            {
                cbkSuccess(www.downloadHandler.text);
            }
        }
    }

    private void SetJsonFromWWW()
    {
        string sTgtURL = "http://localhost/concentration/singlerank/setMessage";

        string Name = Gamesetting.username;
        string Difficulty = Gamesetting.difficulty;
        string Score = Gamesetting.scorestr;

        StartCoroutine(SetMessage(sTgtURL, Name, Difficulty, Score, WebRequestSuccess, CallbackWebRequestFailed));

    }
    private IEnumerator SetMessage(string url, string Name, string Difficulty, string Score, Action<string> cbkSuccess = null, Action cbkFailed = null)
    {
        WWWForm form = new WWWForm();
        form.AddField("Name", Name);
        form.AddField("Difficulty", Difficulty);
        form.AddField("Score", Score);


        var webRequest = UnityWebRequest.Post(url, form);

        webRequest.timeout = 5;

        yield return webRequest.SendWebRequest();

        if (webRequest.error != null)
        {
            if (cbkFailed != null)
                cbkFailed();
            cbkFailed?.Invoke();
        }
        else if (webRequest.isDone)
        {
            cbkSuccess(webRequest.downloadHandler.text);
        }
}
    private void WebRequestSuccess(string response)
    {
        Debug.Log(response);
    }

    /// <summary>
    /// Deserialize from json.
    /// MemberData�^�̃��X�g��json�ɓ����Ă���Ɖ��肵��
    /// </summary>
    /// <returns>The from json.</returns>
    /// <param name="sStrJson">S string json.</param>
    public static List<RankData> DeserializeFromJson(string sStrJson)
    {
        var ret = new List<RankData>();
        int count = 0;
        // JSON�f�[�^�͍ŏ��͔z�񂩂�n�܂�̂ŁADeserialize�i�f�R�[�h�j��������Ƀ��X�g�փL���X�g      
        IList jsonList = (IList)Json.Deserialize(sStrJson);

        // ���X�g�̓��e�̓I�u�W�F�N�g�Ȃ̂ŁA�����^�̕ϐ��Ɉ�������Ȃ���A����
        foreach (IDictionary jsonOne in jsonList)
        {
            //�V���R�[�h��͊J�n

            var tmp = new RankData();

            //�Y������L�[���� jsonOne �ɑ��݂��邩���ׁA���݂�����擾���ĕϐ��Ɋi�[����B
            if (jsonOne.Contains("Name"))
            {
                tmp.Name = (string)jsonOne["Name"];
            }
            if (jsonOne.Contains("Difficulty"))
            {
                tmp.Difficulty = (string)jsonOne["Difficulty"];
            }
            if(jsonOne.Contains("Score"))
            {
                tmp.Score = (string)jsonOne["Score"];
            }

            //�����R�[�h��͏I��
            ret.Add(tmp);
            count++;
        }

        //Ranking��


        ret.Sort(delegate (RankData x, RankData y)
        {
            return String.Compare(x.Score, y.Score, StringComparison.Ordinal); //Score�v���p�e�B�Ń\�[�g����
        });

        int easytop = 0;
        int normaltop = 0;
        int hardtop = 0;
        int easymy = 0;
        int normalmy = 0;
        int hardmy = 0;


        foreach (var ranktop in ret)
        {
            if (ranktop.Difficulty == "Easy" && easytop == 0)
            {
                Debug.Log("EasyTOP" + ranktop.Name);
                Debug.Log("EasyTOP" + ranktop.Score);
                Gamesetting.easytopstr = ranktop.Name + ":" + ranktop.Score;
                easytop++;
            }
            if (ranktop.Difficulty == "Normal" && normaltop == 0)
            {
                Debug.Log("NormalTOP" + ranktop.Name);
                Debug.Log("NormalTOP" + ranktop.Score);
                Gamesetting.normaltopstr = ranktop.Name + ":" + ranktop.Score;
                normaltop++;
            }
            if (ranktop.Difficulty == "Hard" && hardtop == 0)
            {
                Debug.Log("HardTOP" + ranktop.Name);
                Debug.Log("HardTOP" + ranktop.Score);
                Gamesetting.hardtopstr = ranktop.Name + ":" + ranktop.Score;
                hardtop++;
            }
            if (ranktop.Difficulty == "Easy" && easymy == 0 && ranktop.Name == Gamesetting.username)
            {
                Debug.Log("EasyMy" + ranktop.Name);
                Debug.Log("EasyMy" + ranktop.Score);
                Gamesetting.myeasy = ranktop.Score + "�b";
                easymy++;
            }
            if (ranktop.Difficulty == "Normal" && normalmy == 0 && ranktop.Name == Gamesetting.username)
            {
                Debug.Log("NormalMy" + ranktop.Name);
                Debug.Log("NormalMy" + ranktop.Score);
                Gamesetting.mynormal = ranktop.Score + "�b";
                normalmy++;
            }
            if (ranktop.Difficulty == "Hard" && hardmy == 0 && ranktop.Name == Gamesetting.username)
            {
                Debug.Log("HardMy" + ranktop.Name);
                Debug.Log("HardMy" + ranktop.Score);
                Gamesetting.myhard = ranktop.Score + "�b";
                hardmy++;
            }
        }


        return ret;
    }
}
