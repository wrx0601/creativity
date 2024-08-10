using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{

    [SerializeField] public Image closeimage;
    [SerializeField] public Image openimage;
    [SerializeField] public int imagenum;
    [SerializeField] public GameObject myobject;
    [SerializeField] public PhotonView myview;
    [SerializeField] private Renderer myrenderer;
    public Sprite Sprite1;
    public Sprite Sprite2;
    public Sprite Sprite3;
    public Sprite Sprite4;
    public Sprite Sprite5;
    public Sprite Sprite6;
    public Sprite Sprite7;
    public Sprite Sprite8;
    public Sprite Sprite9;
    public Sprite Sprite10;
    public int open=0;//0なら裏、表なら1として同期用のパラメータとする
    public int clear=0;//0なら残す、1なら消すとして同期用のパラメータとする
    private float alpha_on_invalid = 1.0f;

    public static Card instance;
    void Awake()
    {
        gameObject.transform.parent = FindParent();
        //gameObject.name = "Card"+ Gamesetting.cardcount;
        //Gamesetting.cardcount++;
    }

    private Transform FindParent()
    {
        Transform parent = GameObject.FindGameObjectWithTag("Parent").transform; ;
        //Find object should be parent
        return parent;
    }
    // Start is called before the first frame update
    void Start()
    {
        clear = 0;
        open = 0;
        closeimage.enabled = true;
        openimage.enabled = false;
    }
    private void Update()
    {
       if(clear == 1 && open == 1)
        {
            alpha_on_invalid = 0.5f;
            Button mybutton = myobject.GetComponent<Button>();
            closeimage.color = new Color(
                closeimage.color.r,
                closeimage.color.g,
                closeimage.color.b,
                alpha_on_invalid);
            openimage.color=new Color(
                openimage.color.r,
                openimage.color.g,
                openimage.color.b,
                alpha_on_invalid);
            mybutton.interactable = false;
            if (open == 0)
            {
                closeimage.enabled = true;
                openimage.enabled = false;
            }
        }
    }

    public void OnClickCard()
    {
        if(Gamesetting.mode=="Single")
        {
            closeimage.enabled = false;
            openimage.enabled = true;
            open = 1;
            string name = myobject.name;
            int num = imagenum;
            GameManager.instance.Concentration(num, name);
            Button mybutton = myobject.GetComponent<Button>();
            mybutton.interactable = false;
        }
        if(Gamesetting.mode=="Multi" && myview.isMine)
        {
            closeimage.enabled = false;
            openimage.enabled = true;
            open = 1;
            string name = myobject.name;
            int num = imagenum;
            GameManager.instance.Concentration(num, name);
            Button mybutton = myobject.GetComponent<Button>();
            mybutton.interactable = false;
        }
    }
    public void ChangeImage(int imagenumber)
    {
        if (imagenumber == 1)
        {
            openimage.sprite = Sprite1;
            imagenum = 1;
        }
        else if (imagenumber == 2)
        {
            openimage.sprite = Sprite2;
            imagenum = 2;
        }
        else if (imagenumber == 3)
        {
            openimage.sprite = Sprite3;
            imagenum = 3;
        }
        else if (imagenumber == 4)
        {
            openimage.sprite = Sprite4;
            imagenum = 4;
        }
        else if (imagenumber == 5)
        {
            openimage.sprite = Sprite5;
            imagenum = 5;
        }
        else if (imagenumber == 6)
        {
            openimage.sprite = Sprite6;
            imagenum = 6;
        }
        else if (imagenumber == 7)
        {
            openimage.sprite = Sprite7;
            imagenum = 7;
        }
        else if (imagenumber == 8)
        {
            openimage.sprite = Sprite8;
            imagenum = 8;
        }
        else if (imagenumber == 9)
        {
            openimage.sprite = Sprite9;
            imagenum = 9;
        }
        else if (imagenumber == 10)
        {
            openimage.sprite = Sprite10;
            imagenum = 10;
        }
    }
    public void AllClose()
    {   
            open = 0;
            closeimage.enabled = true;
            openimage.enabled = false;
            Button mybutton = myobject.GetComponent<Button>();
            mybutton.interactable = true;
    }
    public void CardTurn()
    {
        if (open == 0)
        {
            closeimage.enabled = true;
            openimage.enabled = false;
        }
        else if(open==1)
        {
            closeimage.enabled = false;
            openimage.enabled = true;
        }
    }
    public void ChangeClear()
    {
        clear = 1;

    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(imagenum);
            stream.SendNext(myobject.name);
            stream.SendNext(open);
            stream.SendNext(clear);
        }
        else
        {
            imagenum = (int)stream.ReceiveNext();
            ChangeImage(imagenum);
            myobject.name= (string)stream.ReceiveNext();
            open = (int)stream.ReceiveNext();
            CardTurn();
            clear = (int)stream.ReceiveNext();
        }
    }
    void OwnerChange()
    {
        myview.RequestOwnership();
    }
}