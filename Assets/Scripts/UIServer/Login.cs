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

    private bool needcode = true;
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
            if(context.Length == 11 && context.StartsWith("09"))
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
                    GetCode();
                    EnterButton.GetComponentInChildren<Text>().text = PersianFix.Persian.Fix("ورود", 255);
                    needcode = false;
                    code.interactable = true;

                }
                else
                {
                    LoginSend();
                }
                


            });
        }


    }
    private void OnDisable()
    {
        EnterButton.onClick.RemoveListener(() => { });
    }
    public void GetCode()
    {

        LOGIN login = new LOGIN() { phone = phoneNumber.text, code = "" };

        var data = JsonUtility.ToJson(login);
        HTTPRequest SendInfo = new HTTPRequest(LoginAPI, "Content-Type", "application/json", HTTPRequest.Method.POST);

        StartCoroutine(SendInfo.POST(data, HTTPRequest.Decoder.Buffer, true));
        Debug.Log("GetCode");
      

    }
    public void LoginSend()
    {


        LOGIN login = new LOGIN() { phone = phoneNumber.text, code = code.text };

        var data = JsonUtility.ToJson(login);
        HTTPRequest SendInfo = new HTTPRequest(LoginAPI, "Content-Type", "application/json", HTTPRequest.Method.POST);
        SendInfo.OnResponse += SendInfo_OnResponse;
        SendInfo.OnRequsetFail += SendInfo_OnRequsetFail;
        StartCoroutine(SendInfo.POST(data, HTTPRequest.Decoder.Buffer, true));
        Debug.Log("SendData");
      

    }



    private void SendInfo_OnResponse(string Response)
    {
       
        Server.ConnectToUIServer();
        Debug.Log("Login Reponse:"+Response);
    }
    private void SendInfo_OnRequsetFail(string obj)
    {
       
    }

}
