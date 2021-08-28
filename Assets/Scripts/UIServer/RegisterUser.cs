using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Diaco.HTTPBody;
using Diaco.HTTPRequests;
public class RegisterUser : MonoBehaviour
{
    public ServerUI Server;
    /// <summary>
    /// IP LOCAL = "http://192.168.1.100:8420/api/user/register"
    /// IP GOLBAL = "http://37.152.185.15:8420/api/user/register"
    /// </summary>
    public string RegisterAPI = "http://37.152.185.15:8420/api/user/register";
    public InputField UserName;
    public InputField Email;
    public InputField Password;
    public InputField PasswordConfirm;
    public Button Submit;

    private void Awake()
    {
        Submit.onClick.AddListener(SendRegisterUser);
    }
    public void OnEnable()
    {
       
    }

    public void SendRegisterUser()
    {
        if (UserName.text != null && Email.text != null && Password.text != null && PasswordConfirm != null)
        {
            if (Password.text == PasswordConfirm.text)
            {
                REGISTER register = new REGISTER() { userName = UserName.text, email = Email.text, password = Password.text, confirmPassword = PasswordConfirm.text };
                // REGISTER register = new REGISTER() { username = "oms3dfsff", email = "atlaspltrtnts@gmail.com", password = "007007", confirmPassword = "007007" };
                var data = JsonUtility.ToJson(register);
                HTTPRequest SendInfo = new HTTPRequest(RegisterAPI, "Content-Type", "application/json", HTTPRequest.Method.POST);
                SendInfo.OnResponse += SendInfo_OnResponseRegisterUser;
                SendInfo.OnRequsetFail += SendInfo_OnRequsetFailRegisterUser;
                StartCoroutine(SendInfo.POST(data, HTTPRequest.Decoder.Buffer, true));
            }
            else
            {
                Debug.Log("Error : CheckPassword");


            }
        }
        else
        {
            Debug.Log("Error : FieldEmpty!");
        }
    }

    private void SendInfo_OnRequsetFailRegisterUser(string error)
    {

    }

    private void SendInfo_OnResponseRegisterUser(string Response)
    {
        Debug.Log(Response);
        Server.ConnectToUIServer();
        
      // this.gameObject.SetActive(false);
    }
}
