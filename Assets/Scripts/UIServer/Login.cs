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

    public GameObject registerpage;
    public InputField UserName;
    public InputField Password;
    public Button EnterButton;

    public Button Forgetpassword;
    private void Awake()
    {
        EnterButton.onClick.AddListener(() =>
        {
            LoginSend();


        });

    }
    private void OnEnable()
    {
        
  

       
    }
    private void OnDisable()
    {
       // EnterButton.onClick.RemoveListener(() => { });
    }
    public void LoginSend()
    {

        if (UserName.text != null && Password.text != null)
        {
            LOGIN login = new LOGIN() { userName = UserName.text, password = Password.text };

            var data = JsonUtility.ToJson(login);
            HTTPRequest SendInfo = new HTTPRequest(LoginAPI, "Content-Type", "application/json", HTTPRequest.Method.POST);
            SendInfo.OnResponse += SendInfo_OnResponse;
            SendInfo.OnRequsetFail += SendInfo_OnRequsetFail;
            StartCoroutine(SendInfo.POST(data, HTTPRequest.Decoder.Buffer, true));
            Debug.Log("LoginSended");
            EnterButton.interactable = false;
            Password.interactable = false;
        }
    }



    private void SendInfo_OnResponse(string Response)
    {
       
        Server.ConnectToUIServer();
        Debug.Log("Login Reponse:"+Response);
    }
    private void SendInfo_OnRequsetFail(string obj)
    {
        EnterButton.interactable = true;
        Password.interactable = true;
    }

}
