using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Diaco.HTTPBody;
using Diaco.HTTPRequests;
public class Login : MonoBehaviour
{
    public string LoginAPI = "http://37.152.185.15:8420/api/user/login";

    public ServerUI Server;

    //public GameObject registerpage;

    public InputField phoneNumber;
    public InputField code;
    public Button EnterButton;
    public GameObject Timer_parent;
    public Text Timer_text;
    private bool needcode = true;

    private float H = 0;
    private float M = 0;
    private float S = 0;
    ///  public Button Forgetpassword;
    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        phoneNumber.onValueChanged.AddListener((context) => {
            needcode = true;
            code.interactable = false;
            code.text = "";
            Timer_parent.SetActive(false);

            
            if (context.Length == 11 && (context.StartsWith("09") || context.StartsWith("۰۹"))
            )
            {
                EnterButton.interactable = true;
                EnterButton.GetComponentInChildren<Text>().text = PersianFix.Persian.Fix("ارسال کد", 255);

            }
            else
            {
                EnterButton.interactable = false;
                EnterButton.GetComponentInChildren<Text>().text = PersianFix.Persian.Fix("ارسال کد", 255);

            }
        });

        if (EnterButton)
        {
            EnterButton.onClick.AddListener(() =>
            {
                if(needcode)
                {
                    RequestConfrimCode();
                    EnterButton.GetComponentInChildren<Text>().text = PersianFix.Persian.Fix("ورود", 255);
                    needcode = false;
                    code.interactable = true;
                    Timer_parent.SetActive(true);
                    CalculateTime(130);
                }
                else
                {
                    LoginData();
                    Timer_parent.SetActive(false);
                    CancelInvoke("RunTimer");
                }
                


            });
        }


    }
    private void OnDisable()
    {
        EnterButton.onClick.RemoveListener(() => { });
    }
    public void RequestConfrimCode()
    {

        LOGIN login = new LOGIN() { phone = phoneNumber.text, code = "" };

        var data = JsonUtility.ToJson(login);
        HTTPRequest SendInfo = new HTTPRequest(LoginAPI, "Content-Type", "application/json", HTTPRequest.Method.POST);

        StartCoroutine(SendInfo.POST(data, HTTPRequest.Decoder.Buffer, false));
        Debug.Log("RequestConfrimCode");
      

    }
    public void LoginData()
    {


        LOGIN login = new LOGIN() { phone = phoneNumber.text, code = code.text };

        var data = JsonUtility.ToJson(login);
        HTTPRequest SendInfo = new HTTPRequest(LoginAPI, "Content-Type", "application/json", HTTPRequest.Method.POST);
        SendInfo.OnResponse += SendInfo_OnResponse;
        SendInfo.OnRequsetFail += SendInfo_OnRequsetFail;
        StartCoroutine(SendInfo.POST(data, HTTPRequest.Decoder.Buffer, true));
        Debug.Log("login");
      

    }


    private void CalculateTime(int time)
    {
        H = 0;
        M = 0;
        S = 0;
        CancelInvoke("RunTimer");


        H = (float)Mathf.Floor(time / 3600);
        M = (float)Mathf.Floor(time / 60 % 60);
        S = (float)Mathf.Floor(time % 60);
        InvokeRepeating("RunTimer", 0, 1.0f);
    }
    /// <summary>
    /// INVOKE IN Calculate
    /// </summary>
    private void RunTimer()
    {
        S--;
        if (S < 0)
        {
            if (M > 0 || H > 0)
            {
                S = 59;
                M--;
                if (M < 0)
                {
                    if (H > 0)
                    {
                        M = 59;
                        H--;
                    }
                    else
                    {
                        M = 0;
                    }
                }

            }
            else
            {
                S = 0;
            }
        }


        Timer_text.text = (M + ":" + S);
        if (S == 0 && M == 0 && H == 0)
        {
            CancelInvoke("RunTimer");


        }
    }
    private void SendInfo_OnResponse(string Response)
    {
  
            Server.ConnectToUIServer();
            Debug.Log("Login Reponse:" + Response);
        
      
    }
    private void SendInfo_OnRequsetFail(string obj)
    {
        Debug.Log("Login Error:" + "ERROR 404!");
    }

}
