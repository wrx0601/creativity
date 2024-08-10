using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
//using UnityEditor.Experimental.GraphView;

public class GamePhotonManager : MonoBehaviour
{
    [SerializeField] private PhotonView _PhotonView;
    private string sb;

    void Start()
    {
        if (Gamesetting.mode == "Multi")
        {

        
        //itemObject = PhotonNetwork.Instantiate("CardButton", multifield.transform, Quaternion.identity,0);
        //itemObject.transform.SetParent(multifield.transform);
        _PhotonView.RPC("multicardnumber", PhotonTargets.AllBuffered);

        /*
        Card item_content = itemObject.GetComponent<Card>();
        item_content.ChangeImage(cards[i]);
        itemObject.name = "Card" + i;
        sb = sb + cards[i] + ",";
        */
        Debug.Log(sb);
        }
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
        for (int i = 1; i <= max * 2; i++)
        {
            j = i;
            if (i > max)
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
    }

    [PunRPC]
    public void CardNumber()
    {
        CardShuffle(10);
        for (int i = 1; i <= 20; i++)
        {
            GameObject cardtemp = GameObject.Find("Card" + i);
            Card item_content = cardtemp.GetComponent<Card>();
            item_content.ChangeImage(cards[i]);
            sb = sb + cards[i] + ",";
        }
    }

}
