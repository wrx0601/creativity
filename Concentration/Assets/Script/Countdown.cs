using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp.Net;


public class Countdown : MonoBehaviour
{

    [SerializeField] public float maxCount;
    [SerializeField] public Text timertext;
    [SerializeField] public GameObject myobject;


    double startTime;
    float leftTime;


    public static Countdown instance;
    private void Awake()
    {
       // myobject.name = "CountDown";
    }

    void OnEnable()
    {
        startTime = (double)PhotonNetwork.room.CustomProperties["StartTime"];
        leftTime = maxCount;
    }

    void Update()
    {
        if (leftTime <= 0)
        {
            GameManager.instance.TurnFrag();
            //myobject.SetActive(false);
        }
        else
        {
            double elapsedTime = PhotonNetwork.time - startTime;
            leftTime = maxCount - (float)elapsedTime;
            timertext.text = leftTime.ToString("0.00") + "•b";
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            //stream.SendNext(myobject.name);
            stream.SendNext(startTime);
            //stream.SendNext(leftTime);
        }
        else
        {
            //myobject.name=(string)stream.ReceiveNext();
            startTime = (double)stream.ReceiveNext();
            //leftTime=(float)stream.ReceiveNext();
        }
    }
    public void ZeroLeft()
    {
        leftTime = 0;
    }
}