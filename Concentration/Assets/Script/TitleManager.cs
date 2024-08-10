using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


// ★どのシーンからでもアクセスできるクラス★
public static class Gamesetting
{
    // どのシーンからでもアクセスできる変数
    public static string username;
    public static string difficulty;
    public static string mode;
    public static float score=0;
    public static string scorestr="";
    public static string easytopstr = "";
    public static string normaltopstr = "";
    public static string hardtopstr = "";
    public static string myeasy = "";
    public static string mynormal = "";
    public static string myhard = "";
    public static string mastername = "";
    public static string sravename = "";
    public static string selectroomname = "";
    public static string selectroommaster = "";
    public static string selectroomsrave = "";
    public static string selectroomcountdown = "";
    public static int cardcount = 0;
}




public class TitleManager : MonoBehaviour
{
    [SerializeField] private Button startbutton;
    [SerializeField] private InputField namefield;
    
    // Start is called before the first frame update
    void Start()
    {
        startbutton.interactable = false;
        namefield.text = "";
        Gamesetting.username = "";
        Gamesetting.difficulty = "";
        Gamesetting.mode = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(namefield.text != Gamesetting.username)
        {
            startbutton.interactable = true;
            Gamesetting.username = namefield.text;
            Debug.Log(Gamesetting.username);
            if(Gamesetting.username == "")
            {
                startbutton.interactable=false;
            }
        }
    }
    public void OnClickStart()
    {
        SceneManager.LoadScene("Home");
    }
}
