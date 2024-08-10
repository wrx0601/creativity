using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using HashTable = ExitGames.Client.Photon.Hashtable;
using System.Runtime.CompilerServices;
using System.IO;
using static UnityEngine.Networking.UnityWebRequest;

public class GameManager : Photon.MonoBehaviour
{

    [SerializeField] private Canvas easycanvas;
    [SerializeField] private Canvas normalcanvas;
    [SerializeField] private Canvas hardcanvas;
    [SerializeField] private Canvas multicanvas;
    [SerializeField] private GameObject mainpanel;
    [SerializeField] private GameObject easyfield;
    [SerializeField] private GameObject normalfield;
    [SerializeField] private GameObject hardfield;
    [SerializeField] private GameObject multifield;
    [SerializeField] private GridLayoutGroup easygrid;
    [SerializeField] private GridLayoutGroup normalgrid;
    [SerializeField] private GridLayoutGroup hardgrid;
    [SerializeField] private GridLayoutGroup multigrid;
    [SerializeField] private GameObject cardobj;
    [SerializeField] private GameObject myfield;
    [SerializeField] private GameObject otherfield;
    [SerializeField] private Text mastername;
    [SerializeField] private Text sravename;
    [SerializeField] private Text timertext;
    [SerializeField] private GameObject resultpanel;
    [SerializeField] private Text winname;
    [SerializeField] private Text winscore;
    [SerializeField] private Text wintext;
    [SerializeField] private UnityEngine.UI.Button rebutton;
    [SerializeField] private Text turntext;
    [SerializeField] private PhotonView _PhotonView;
    [SerializeField] public GameObject countdown;
    [SerializeField] public GameObject countdownload;
    [SerializeField] public GameObject countdownchange;
    [SerializeField] private GameObject loadpanel;
    [SerializeField] private Text loadtext;
    [SerializeField] private GameObject multiresultpanel;
    [SerializeField] private Text masterscoretext;
    [SerializeField] private Text sravescoretext;
    [SerializeField] private Text winnertext;
    [SerializeField] private GameObject multirebutton;

    public List<Card> cardList;
    private string sb;

    private int bfnum = 0;
    private string bfname = "";
    private int opn = 0;

    public float timer;
    public bool clear;

    public static GameManager instance;
    public string turn="";
    PhotonView _photonView;

    public object[] myCustomInitData;
    public int cardid;

    public int tfrag = 0;
    public bool loadinfo;
    public int masterscore = 0;
    public int sravescore = 0;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        Gamesetting.score=0;
        easycanvas.enabled = false;
        normalcanvas.enabled = false;
        hardcanvas.enabled = false;
        multicanvas.enabled = false;
        easygrid.enabled = true;
        normalgrid.enabled = true;
        hardgrid.enabled = true;
        multigrid.enabled = true;
        mainpanel.SetActive(true);
        loadpanel.SetActive(false);
        multirebutton.SetActive(false);
        multiresultpanel.SetActive(false);

        if (Gamesetting.mode=="Single")
        {
            mastername.text = Gamesetting.username;
        }
        clear=false;
        resultpanel.SetActive(false);

        if(Gamesetting.mode== "Single")
        {
            if(Gamesetting.difficulty=="Easy")
            {
                easycanvas.enabled=true;
                CardShuffle(6);
                for (int i = 0; i < 12; i++)
                {
                    GameObject itemObject = Instantiate(cardobj, Vector3.zero, Quaternion.identity);
                    itemObject.transform.SetParent(easyfield.transform);
                    Card item_content = itemObject.GetComponent<Card>();
                    item_content.ChangeImage(cards[i]);
                    itemObject.name = "Card" + i;
                    sb = sb + cards[i] + ",";
                }
                Debug.Log(sb);
            }
            else if(Gamesetting.difficulty=="Normal")
            {
                normalcanvas.enabled=true;
                CardShuffle(8);
                for (int i = 0; i < 16; i++)
                {
                    GameObject itemObject = Instantiate(cardobj, Vector3.zero, Quaternion.identity);
                    itemObject.transform.SetParent(normalfield.transform);
                    Card item_content = itemObject.GetComponent<Card>();
                    item_content.ChangeImage(cards[i]);
                    itemObject.name = "Card" + i;
                    sb = sb + cards[i] + ",";
                }
                Debug.Log(sb);
            }
            else if(Gamesetting.difficulty=="Hard")
            {
                hardcanvas.enabled=true;
                CardShuffle(10);
                for(int i = 0; i < 20; i++)
                {
                    GameObject itemObject = Instantiate(cardobj, Vector3.zero, Quaternion.identity);
                    itemObject.transform.SetParent(hardfield.transform);
                    Card item_content = itemObject.GetComponent<Card>();
                    item_content.ChangeImage(cards[i]);
                    itemObject.name = "Card" + i;
                    sb = sb + cards[i] + ",";
                }
                Debug.Log(sb);
            }
            timer = 0.0f;
            sravename.enabled = false;
            turntext.enabled = false;
        }
        else if(Gamesetting.mode=="Multi" && PhotonNetwork.player.IsMasterClient)
        {
            CardShuffle(10);
            for (int i = 0; i < 20; i++)
                {
                    GameObject itemObject = PhotonNetwork.InstantiateSceneObject("CardButton", Vector3.zero, Quaternion.identity, 0, null);
                    itemObject.name = "Card" + i;
                    itemObject.GetComponent<Card>().ChangeImage(cards[i]);
                itemObject.GetPhotonView().viewID=i+1;
                }
            for (int i = 1; i <= 10; i++)
            {
                int j = 100 + i;
                int k = 200 + i;
                PhotonView photonview = GameObject.Find("Image" + j).GetPhotonView();
                photonview.TransferOwnership(1);
                photonview = GameObject.Find("Image" + k).GetPhotonView();
                photonview.TransferOwnership(2);
            }
            var properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add("StartTime", PhotonNetwork.time);
            PhotonNetwork.room.SetCustomProperties(properties);
            Debug.Log(sb);
            multirebutton.SetActive(true);
        }
        if(Gamesetting.mode=="Multi")
        {
            Gamesetting.score = 0;
            PhotonView photonview = this.GetComponent<PhotonView>();
            photonview.RPC("LoadPanelLoad", PhotonTargets.AllBuffered, true);
            loadpanel.transform.SetAsLastSibling();
            photonView.RPC("TurnInfo", PhotonTargets.AllBuffered, "Srave", "同期中");
            multicanvas.enabled = true;
            
            mastername.text = PhotonNetwork.masterClient.NickName;
            sravename.text = PhotonNetwork.player.NickName;
            if(PhotonNetwork.isMasterClient)
            {
                sravename.text = Gamesetting.sravename;
            }
            turntext.enabled = true;
        }
        StartCoroutine(sleep(1.0f, () => {
            if(Gamesetting.mode=="Multi")
            {
                countdownload.SetActive(true);
            }
            easygrid.enabled = false;
            normalgrid.enabled = false;
            hardgrid.enabled = false;
            multigrid.enabled = false;
        }));
    }

    List<int> cards; //リストの宣言。

    public void CardShuffle(int max)　//shuffleメソッド本体。
    {
        if (cards == null)　//cardsが空であれば。
        {
            cards = new List<int>();　//cardの初期化。
        }
        else　//その他の場合。
        {
            cards.Clear();　//cardsを空にする。
        }
        int j;
        for (int i = 1; i <= max*2; i++)
        {
            j = i;
            if(i>max)
            {
                j = i - max;
            }
            cards.Add(j);　//リストにiを加える。
        }
        int n = cards.Count; //整数ｎの初期値はカードの枚数とする。
        while (n > 1)　//nが１より大きい場合下記を繰り返す。
        {
            n--;　//nをディクリメント
            int k = Random.Range(0, n + 1);//kは０～n+1の間のランダムな数とする
            int temp = cards[k];　//k番目のカードをtempに代入
            cards[k] = cards[n];　//k番目のインデックスにn番目のインデックスを代入
            cards[n] = temp;　//n番目のインデックスにtemp(k番目のインデックス）を代入
        }
        if(Gamesetting.mode=="Multi")
        {
            HashTable hs = new HashTable();
            for (int i = 0; i < 20; i++)
            {
                hs["Multi"+i] = cards[i];
            }
            PhotonNetwork.room.SetCustomProperties(hs);
        }



    }
    public void Concentration(int number,string name)
    {
        easygrid.enabled = false;
        normalgrid.enabled = false;
        hardgrid.enabled = false;
        multigrid.enabled = false;

        if(opn == 1)
        {
            if (number == bfnum)
            {
                StartCoroutine(sleep(1.0f, () => {
                    GameObject bfcard;
                    bfcard = GameObject.Find(bfname);
                    bfcard.GetComponent<Card>().ChangeClear();
                    GameObject afcard;
                    afcard = GameObject.Find(name);
                    afcard.GetComponent<Card>().ChangeClear();
                    if(Gamesetting.mode=="Single")
                    {
                        int t=100+number;
                        GameObject tempobject = GameObject.Find("Image"+t);
                        Image temp = tempobject.GetComponent<Image>();
                        temp.enabled = true;
                        Gamesetting.score++;
                    }
                    else if(Gamesetting.mode=="Multi")
                    {
                        if(turn=="Master")
                        {
                            PhotonView photonview = this.GetComponent<PhotonView>();
                            photonview.RPC("CardGet", PhotonTargets.AllBuffered, 100,number);
                        }
                        else if(turn=="Srave")
                        {
                            PhotonView photonview = this.GetComponent<PhotonView>();
                            photonview.RPC("CardGet", PhotonTargets.AllBuffered, 200, number);
                        }
                    }

                    opn = 0;
                bfnum = 0;
                bfname = "";

                }));
                if (Gamesetting.mode == "Multi")
                {
                    {
                        var properties = new ExitGames.Client.Photon.Hashtable();
                        properties.Add("StartTime", PhotonNetwork.time);
                        PhotonNetwork.room.SetCustomProperties(properties);
                    }
                }

            }
            else if (number != bfnum)
            {
                StartCoroutine(sleep(1.0f, () => {
                    GameObject bfcard = GameObject.Find(bfname);
                    bfcard.GetComponent<Card>().AllClose();
                    GameObject afcard;
                    afcard = GameObject.Find(name);
                    afcard.GetComponent<Card>().AllClose();
                    opn = 0;
                    bfnum = 0;
                    bfname = "";
                    StartCoroutine(sleep(0.3f, () => {
                        if (Gamesetting.mode == "Multi")
                        {
                            TurnFrag();
                        }
                    }));

                }));

            }
        }
        else if (opn==0)
        {
            bfnum=number;
            bfname = name;
            opn = 1;
        }
    }
    IEnumerator sleep(float seconds, UnityAction callback)
    {
        mainpanel.SetActive(true);
        yield return new WaitForSeconds(seconds);  //5秒待つ
        callback?.Invoke();
        mainpanel.SetActive(false);
    }
    private void Update()
    {
        if(clear==true)
        {
        }
        else if(Gamesetting.score>=6 && Gamesetting.difficulty=="Easy")
        {
            resultpanel.SetActive(true);
            winname.text =mastername.text;
            winscore.text = timer.ToString("f1") + "秒";
            clear =true;
            Debug.Log(timer);
        }
        else if(Gamesetting.score>=8 &&  Gamesetting.difficulty=="Normal")
        {
            clear = true;
            resultpanel.SetActive(true);
            winname.text = mastername.text;
            winscore.text = timer.ToString("f1") + "秒";
            Debug.Log(timer);
        }
        else if(Gamesetting.score>=10 &&  Gamesetting.difficulty=="Hard")
        {
            clear = true;
            resultpanel.SetActive(true);
            winname.text = mastername.text;
            winscore.text = timer.ToString("f1") + "秒";
            Debug.Log(timer);
        }
        else if (Gamesetting.mode == "Single" && clear==false)
        {
            timer += Time.deltaTime;
            timertext.text = timer.ToString("f1") + "秒";
        }

        if (Gamesetting.mode == "Multi")
        {
            tfrag = (int)PhotonNetwork.room.CustomProperties["Tfrag"];
            if (Gamesetting.score == 10)
            {
                PhotonView photonview = this.GetComponent<PhotonView>();
                photonview.RPC("MultiResult", PhotonTargets.AllBuffered);
            }
            else if (tfrag==1)
            {
                Changeturn();
            }
        }
    }
    public void OnClickreturnButton()
    {
        Gamesetting.scorestr = timer.ToString("f1");
        SceneManager.LoadScene("Home");
    }

    public void MasterTurn()
    {
        mainpanel.SetActive(true);
        if (mastername.text== PhotonNetwork.player.NickName)
        {
            mainpanel.SetActive(false);
        }
        PhotonView[] photonViews = GameObject.FindObjectsOfType<PhotonView>();
        for (int i = 0; i <20; i++)
        {
            photonViews[i].TransferOwnership(1);
        }
        string turnstr = mastername.text + "のターン";
        PhotonView photonview = this.GetComponent<PhotonView>();
        photonview.RPC("LoadPanelLoad", PhotonTargets.AllBuffered, true);
        photonView.RPC("TurnInfo", PhotonTargets.AllBuffered,"Master",turnstr);
        tfrag = 0;
    }
    public void SraveTurn()
    {
        mainpanel.SetActive(true);
        if (sravename.text== PhotonNetwork.player.NickName)
        {
            mainpanel.SetActive(false);
        }
        PhotonView[] photonViews = GameObject.FindObjectsOfType<PhotonView>();

        for(int i = 0;i<20;i++)
        {
            photonViews[i].TransferOwnership(2);
        }
        string turnstr = sravename.text + "のターン";
        PhotonView photonview = this.GetComponent<PhotonView>();
        photonview.RPC("LoadPanelLoad", PhotonTargets.AllBuffered, true);
        photonView.RPC("TurnInfo", PhotonTargets.AllBuffered, "Srave", turnstr);
        tfrag = 2;
    }

    public void Changeturn()
    {
        tfrag = 0;
        ExitGames.Client.Photon.Hashtable cp = PhotonNetwork.room.CustomProperties;
        cp["Tfrag"] = tfrag;
        PhotonNetwork.room.SetCustomProperties(cp);
        countdown.SetActive(false);
        countdownload.SetActive(false);
        if (turn=="Master")
            {
                SraveTurn();
            }
            else if (turn=="Srave")
            {
                MasterTurn();
            }
        if (PhotonNetwork.isMasterClient)
        {
            cp["StartTime"] = PhotonNetwork.time;
            PhotonNetwork.room.SetCustomProperties(cp);
        }
        StartCoroutine(sleep(1.0f, () => {
            countdownchange.SetActive(true);
        }));
    }
    [PunRPC]
    public void TurnFrag()
    {
        tfrag=1;
        ExitGames.Client.Photon.Hashtable cp = PhotonNetwork.room.CustomProperties;
        cp["Tfrag"] = tfrag;
        PhotonNetwork.room.SetCustomProperties(cp);
        countdown.SetActive(false);
    }
    public void TurnStart()
    {
        ExitGames.Client.Photon.Hashtable cp = PhotonNetwork.room.CustomProperties;
        cp["StartTime"] = PhotonNetwork.time;
        PhotonNetwork.room.SetCustomProperties(cp);
        PhotonView photonview = this.GetComponent<PhotonView>();
        photonview.RPC("LoadPanelLoad", PhotonTargets.AllBuffered, false);

        countdownchange.SetActive(false);
        photonview.RPC("CountDowninit", PhotonTargets.AllBuffered);
    }

    [PunRPC]
    void LoadPanelLoad(bool load)
    {
        loadpanel.SetActive(load);
    }
    [PunRPC]
    void TurnInfo(string user, string turntxt)
    {
        turn = user;
        turntext.text = turntxt;
        loadtext.text = turntxt;
    }
    [PunRPC]
    void CardGet(int user,int num)
    {
        int t = user + num;
        GameObject tempobject = GameObject.Find("Image" + t);
        Image temp = tempobject.GetComponent<Image>();
        temp.enabled = true;
        Gamesetting.score++;
        if(user==100)
        {
            masterscore++;
        }
        else if (user == 200)
        {
            sravescore++;
        }
    }
    [PunRPC]
    void CountDowninit()
    {
        countdown.SetActive(false);
        countdown.SetActive(true);
    }
    [PunRPC]
    void MultiResult()
    {
        countdown.SetActive(false);
        multiresultpanel.SetActive(true);
        masterscoretext.text = mastername.text + "：" + masterscore + "組";
        sravescoretext.text = sravename.text + "：" + sravescore + "組";
        if (masterscore>sravescore)
        {
            winnertext.text=mastername.text+"の勝利！";
        }
        else if(masterscore<sravescore)
        {
            winnertext.text = sravename.text + "の勝利！";
        }
        else if(masterscore==sravescore)
        {
            winnertext.text = "引き分け！";
        }
        Gamesetting.score = 0;
    }
    public void MultiReturn()
    {
        /*PhotonView[] photonViews = GameObject.FindObjectsOfType<PhotonView>();
        for (int i = 0; i < 20; i++)
        {
            photonViews[i].TransferOwnership(1);
            PhotonNetwork.Destroy(photonViews[i]);
        }*/
        PhotonNetwork.LoadLevel("Home");
    }
}