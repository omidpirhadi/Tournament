using System;

using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Diaco.HTTPRequests;
using Diaco.HTTPBody;

public class HTTPTEST : MonoBehaviour
{

    public HTTPRequest HTTP;
    [SerializeField]
    public _Apis APIs;

    
    [Serializable]
    public struct UserData
    {
        public string username;
    }
    private void Start()
    {
        REGISTER register = new REGISTER() { userName = "omid435dsf", email = "atlacvcv@gmail.com", password = "007007", confirmPassword = "007007" };
        var data = JsonUtility.ToJson(register);

        HTTP = new HTTPRequest(APIs.GETAPI("register"), "Content-Type", "application/json", HTTPRequest.Method.POST);

        StartCoroutine(HTTP.POST(data, HTTPRequest.Decoder.Buffer, true));
        HTTP.OnResponse += HTTP_OnResponse;
    }
    
    private void HTTP_OnResponse(string respons)
    {
        // t = json;
        Debug.Log("Response:" + respons);

    }
}
