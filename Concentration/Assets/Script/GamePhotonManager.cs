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

        



    List<int> cards; //���X�g�̐錾�B

    public void CardShuffle(int max)�@//shuffle���\�b�h�{�́B
    {
        if (cards == null)�@//cards����ł���΁B
        {
            cards = new List<int>();�@//card�̏������B
        }
        else�@//���̑��̏ꍇ�B
        {
            cards.Clear();�@//cards����ɂ���B
        }
        int j;
        for (int i = 1; i <= max * 2; i++)
        {
            j = i;
            if (i > max)
            {
                j = i - max;
            }
            cards.Add(j);�@//���X�g��i��������B
        }
        int n = cards.Count; //�������̏����l�̓J�[�h�̖����Ƃ���B
        while (n > 1)�@//n���P���傫���ꍇ���L���J��Ԃ��B
        {
            n--;�@//n���f�B�N�������g
            int k = Random.Range(0, n + 1);//k�͂O�`n+1�̊Ԃ̃����_���Ȑ��Ƃ���
            int temp = cards[k];�@//k�Ԗڂ̃J�[�h��temp�ɑ��
            cards[k] = cards[n];�@//k�Ԗڂ̃C���f�b�N�X��n�Ԗڂ̃C���f�b�N�X����
            cards[n] = temp;�@//n�Ԗڂ̃C���f�b�N�X��temp(k�Ԗڂ̃C���f�b�N�X�j����
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
