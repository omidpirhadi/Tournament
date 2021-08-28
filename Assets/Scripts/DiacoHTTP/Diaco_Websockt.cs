using System;

using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


namespace DiacoWebsocket
{
    public class Diaco_Websockt : MonoBehaviour
    {
        public UnityWebRequest webRequest;
        public InputField in_username;
        public Button Submit;
        [Serializable]
        public struct EndPoint
        {
            public string Name;
            public string Url;
        }
        public List<EndPoint> EndPoints;

        [Serializable]
        public struct UserData
        {
            public string username;
        }
        [Serializable]
        public struct Token
        {
            public string token;
        }
        public void Start()
        {
            /* StartCoroutine(PostRequest("http://37.152.185.15:8400/api/user/login", in_username.text,()=> {

                 Debug.Log("ActionRun");
             }));*/
            /*  StartCoroutine(GetRequest("http://37.152.185.15:8400/api/user/get/request", (x) => {

                  Debug.Log(x);
              }));*/
            
            Submit.onClick.AddListener(click);
          
            if(CheckExistToken())
            {

                StartCoroutine(GetRequest(GetApi("Login"), LoadToken(), (x) =>
                 {
                     Debug.Log(x);
                 }
                ));
            }

          
            

            
        }
        public void click()
        {
             var data = JsonUtility.ToJson(new UserData { username = in_username.text });
                 StartCoroutine(PostRequest("http://37.152.185.15:8400/api/user/login", data, () => {

                     Debug.Log("ActionRun");
                 }));
        }
        public IEnumerator PostRequest(string url,string data, Action EventRequest)
        {

            webRequest = new UnityWebRequest();

            webRequest.method = UnityWebRequest.kHttpVerbPOST;
            webRequest.url = url;
        
            webRequest.SetRequestHeader("content-type", "application/json");

           
            var bytedata = ASCIIEncoding.ASCII.GetBytes(data);
            UploadHandler datasend = new UploadHandlerRaw(bytedata);
            webRequest.uploadHandler = datasend;

            webRequest.downloadHandler = new DownloadHandlerBuffer();
            
           // webRequest.uploadHandler.contentType = "application/json";
            yield return webRequest.SendWebRequest();
            if(webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log("Error" + webRequest.error );
               
            }
            else
            {
                var token = webRequest.GetResponseHeader("auth_token");
                
                SaveToken(token);
                Debug.Log("Recive" + Encoding.UTF8.GetString(webRequest.downloadHandler.data) + "\r\n\t" + token);
                EventRequest();
            }

        }
        public IEnumerator GetRequest(string url, string token,Action<string> Response)
        {
            webRequest = new UnityWebRequest();
            DownloadHandlerBuffer downloadHandler = new DownloadHandlerBuffer();
            webRequest.downloadHandler = downloadHandler;
            webRequest.method = UnityWebRequest.kHttpVerbGET;
            webRequest.url = url;
            webRequest.SetRequestHeader("auth_token", token);

            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log("Error" + webRequest.error);

            }
            else
            {
                 
                Response(webRequest.downloadHandler.text);
                //var r = webRequest.downloadHandler.text;
                
            }
            
        }
        public void SaveToken(string token)
        {
            var path = Application.persistentDataPath;
           
            if (File.Exists(path+"//"+"token.diaco"))
            {
                File.Delete(path + "//" + "token.diaco");
                SaveToken(token);
            }
            else
            {
                File.WriteAllText(path + "//" + "token.diaco", token);
                
            }
        }
        public string LoadToken()
        {
            string token = null;
            var path = Application.persistentDataPath + "//" + "token.diaco";
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var token_get = JsonUtility.FromJson<Token>(json);
                token = token_get.token;

            }
            Debug.Log("Token:" + token);
            return token;
        }
        public bool CheckExistToken()
        {
            bool exist = false;
            var path = Application.persistentDataPath + "//" + "token.diaco";
            if (File.Exists(path))
            {
                exist = true;

            }
            return exist;
        }
        public string GetApi(string order)
        {
            string url = null;
            EndPoints.ForEach((e) => {
                if(order == e.Name)
                {
                    url = e.Url;
                }
            });
            return url;
        }
        public AsyncOperation GetAsyncOmid()
        {
            AsyncOperation o = new AsyncOperation();
            for (int i = 0; i < 2000; i++)
            {
                i++;
                Debug.Log(i);
            }
            return o;
        }
    }
}
