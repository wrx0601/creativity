using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class Frag : Photon.MonoBehaviour
{
    [SerializeField] public int tfrag=0;
    [SerializeField] private PhotonView _PhotonView;

    public static Frag instance;
    void Awake()
    {
        this.gameObject.name = "MyFlag";
    }
    // Start is called before the first frame update

    private void Update()
    {
        if (tfrag==1)
        {
            GameManager.instance.TurnFrag();
            tfrag = 0;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(this.gameObject.name);
            stream.SendNext(tfrag);
        }
        else
        {
            this.gameObject.name=(string)stream.ReceiveNext();
            tfrag = (int)stream.ReceiveNext();
        }
    }
    public void FragChange()
    {
            tfrag = 1;
    }

}