using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownLoad : MonoBehaviour
{

    [SerializeField] public float maxCount;

    double startTime;
    float leftTime;

    void OnEnable()
    {
        startTime = (double)PhotonNetwork.room.CustomProperties["StartTime"];
        leftTime = maxCount;
    }

    void Update()
    {
        if (leftTime <= 0)
        {
            GameManager.instance.Changeturn();
        }
        else
        {
            double elapsedTime = PhotonNetwork.time - startTime;
            leftTime = maxCount - (float)elapsedTime;
        }
    }
}