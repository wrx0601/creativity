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
        //ゲームへの遷移
        Gamesetting.difficulty = "Easy";
        SceneManager.LoadScene("Game");
        Debug.Log("難易度:"+Gamesetting.difficulty);
    }
    public void OnClickNomalButton()
    {
        //ゲームへの遷移
        Gamesetting.difficulty = "Normal";
        SceneManager.LoadScene("Game");
        Debug.Log("難易度:" + Gamesetting.difficulty);
    }
    public void OnClickHardButton()
    {
        //ゲームへの遷移
        Gamesetting.difficulty = "Hard";
        SceneManager.LoadScene("Game");
        Debug.Log("難易度:" + Gamesetting.difficulty);
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
        //ゲーム終了
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
        Debug.Log("END");
    }
    public void OnClickMultiButton()
    {
        //ロビーへの遷移
        Gamesetting.mode = "Multi";
        SceneManager.LoadScene("Lobby");
        Debug.Log("Mode:"+Gamesetting.mode);
    }


    //以下スコアDB関係

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
            //リストの内容を表示
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
        // Wwwを利用して json データ取得をリクエストする
        StartCoroutine(
            DownloadJson(
                CallbackWebRequestSuccess, // APIコールが成功した際に呼ばれる関数を指定
                CallbackWebRequestFailed // APIコールが失敗した際に呼ばれる関数を指定
            )
        );
    }

    /// <summary>
    /// Callbacks the www success.
    /// </summary>
    /// <param name="response">Response.</param>
    private void CallbackWebRequestSuccess(string response)
    {
        //Json の内容を MemberData型のリストとしてデコードする。
        _RankList = DeserializeFromJson(response);

        //memberList ここにデコードされたメンバーリストが格納される。

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
        // jsonデータ取得に失敗した
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
            //レスポンスエラーの場合
            Debug.LogError(www.error);
            if (null != cbkFailed)
            {
                cbkFailed();
            }
        }
        else if (www.isDone)
        {
            // リクエスト成功の場合
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
    /// MemberData型のリストがjsonに入っていると仮定して
    /// </summary>
    /// <returns>The from json.</returns>
    /// <param name="sStrJson">S string json.</param>
    public static List<RankData> DeserializeFromJson(string sStrJson)
    {
        var ret = new List<RankData>();
        int count = 0;
        // JSONデータは最初は配列から始まるので、Deserialize（デコード）した直後にリストへキャスト      
        IList jsonList = (IList)Json.Deserialize(sStrJson);

        // リストの内容はオブジェクトなので、辞書型の変数に一つ一つ代入しながら、処理
        foreach (IDictionary jsonOne in jsonList)
        {
            //新レコード解析開始

            var tmp = new RankData();

            //該当するキー名が jsonOne に存在するか調べ、存在したら取得して変数に格納する。
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

            //現レコード解析終了
            ret.Add(tmp);
            count++;
        }

        //Ranking化


        ret.Sort(delegate (RankData x, RankData y)
        {
            return String.Compare(x.Score, y.Score, StringComparison.Ordinal); //Scoreプロパティでソートする
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
                Gamesetting.myeasy = ranktop.Score + "秒";
                easymy++;
            }
            if (ranktop.Difficulty == "Normal" && normalmy == 0 && ranktop.Name == Gamesetting.username)
            {
                Debug.Log("NormalMy" + ranktop.Name);
                Debug.Log("NormalMy" + ranktop.Score);
                Gamesetting.mynormal = ranktop.Score + "秒";
                normalmy++;
            }
            if (ranktop.Difficulty == "Hard" && hardmy == 0 && ranktop.Name == Gamesetting.username)
            {
                Debug.Log("HardMy" + ranktop.Name);
                Debug.Log("HardMy" + ranktop.Score);
                Gamesetting.myhard = ranktop.Score + "秒";
                hardmy++;
            }
        }


        return ret;
    }
}
