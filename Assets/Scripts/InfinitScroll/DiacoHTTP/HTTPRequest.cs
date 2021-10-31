using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Diaco.HTTPRequests
{
    public class HTTPRequest
    {

        public enum Method { POST = 0, GET = 1 }
        public enum Decoder { Buffer = 0, File = 1, Texture = 2, Sprite = 3 }
        public UnityWebRequest ClientHTTP { set; get; }
        public string Response { set; get; }
        public Texture2D TextureResponse { set; get; }
        public Sprite SpriteResponse { set; get; }
        public HTTPRequest(string Api, string Header, string Value, Method Method)
        {
            ClientHTTP = new UnityWebRequest();
            ClientHTTP.url = Api;
            ClientHTTP.SetRequestHeader(Header, Value);
            ClientHTTP.method = Method.ToString();


        }
        /* public HTTPRequest(string Api, Method Method)
         {
             ClientHTTP = new UnityWebRequest();
             ClientHTTP.url = Api;
             ///ClientHTTP.SetRequestHeader(Header, Value);
             ClientHTTP.method = Method.ToString();


         }*/

        public IEnumerator POST(string Data, Decoder decoder , bool SaveHeader)
        {
            var buffer_data = ASCIIEncoding.UTF8.GetBytes(Data);
            UploadHandler upload = new UploadHandlerRaw(buffer_data);
            ClientHTTP.uploadHandler = upload;
            if (decoder == Decoder.Buffer)
            {
                ClientHTTP.downloadHandler = new DownloadHandlerBuffer();
            }
            else if (decoder == Decoder.Texture || decoder == Decoder.Sprite)
            {
                ClientHTTP.downloadHandler = new DownloadHandlerTexture(true);
            }

            yield return ClientHTTP.SendWebRequest();
            if (ClientHTTP.isNetworkError || ClientHTTP.isHttpError)
            {
                // Debug.Log("Error:" + ClientHTTP.error);
                var Error = ClientHTTP.error;
                Handler_OnRequsetFail(Error);
            }
            else
            {

                if (decoder == Decoder.Buffer)
                {
                    Response = Encoding.UTF8.GetString(ClientHTTP.downloadHandler.data);
                    
                    if (SaveHeader)
                    {
                        var token = ClientHTTP.GetResponseHeader("auth_token");
                        SaveToken("token", token);
                        yield return new WaitForSeconds(0.2f);
                        Debug.Log(token);
                    }
                    Handler_OnResponse(Response);

                }
                else if (decoder == Decoder.Texture)
                {
                    TextureResponse = DownloadHandlerTexture.GetContent(ClientHTTP);
                    Handler_OnResponse(TextureResponse);

                }
                else
                {
                    TextureResponse = DownloadHandlerTexture.GetContent(ClientHTTP);
                    SpriteResponse = Sprite.Create(TextureResponse, new Rect(0.0f, 00.0f, TextureResponse.width, TextureResponse.height), new Vector2(0.5f, 0.5f), 100);
                    Handler_OnResponse(TextureResponse);
                }
            }
        }
        public IEnumerator GET(Decoder decoder)
        {

            if (decoder == Decoder.Buffer)
            {
                DownloadHandlerBuffer downloadbuffer = new DownloadHandlerBuffer();
                ClientHTTP.downloadHandler = downloadbuffer;
            }
            else if (decoder == Decoder.Texture || decoder == Decoder.Sprite)
            {
                ClientHTTP.downloadHandler = new DownloadHandlerTexture();
            }

            yield return ClientHTTP.SendWebRequest();

            if (ClientHTTP.isNetworkError || ClientHTTP.isHttpError)
            {
                // Debug.Log("Error:" + ClientHTTP.error);
                var Error = ClientHTTP.error;
                Handler_OnRequsetFail(Error);
            }
            else
            {
                if (decoder == Decoder.Buffer)
                {
                    Response = Encoding.UTF8.GetString(ClientHTTP.downloadHandler.data);
                    Handler_OnResponse(Response);

                }
                else if (decoder == Decoder.Texture)
                {
                    TextureResponse = DownloadHandlerTexture.GetContent(ClientHTTP);
                    Handler_OnResponse(TextureResponse);

                }
                else
                {
                    TextureResponse = DownloadHandlerTexture.GetContent(ClientHTTP);
                    SpriteResponse = Sprite.Create(TextureResponse, new Rect(0.0f, 00.0f, TextureResponse.width, TextureResponse.height), new Vector2(0.5f, 0.5f), 100);
                    Handler_OnResponse(SpriteResponse);

                }
            }
        }

        private void SaveToken(string FileName, string Token)
        {
            if (File.Exists(Application.persistentDataPath + "//" + FileName + ".json"))
            {
                File.Delete(Application.persistentDataPath + "//" + FileName + ".json");
            }
            File.WriteAllText(Application.persistentDataPath + "//" + FileName + ".json", Token);
        }
        public Diaco.HTTPBody.TOKEN LoadToken(string FileName)
        {
            Diaco.HTTPBody.TOKEN token = new HTTPBody.TOKEN();
            if (File.Exists(Application.persistentDataPath + "//" + FileName + ".json"))
            {
                var j_token = File.ReadAllText(Application.persistentDataPath + "//" + FileName + ".json");
                token = JsonUtility.FromJson<Diaco.HTTPBody.TOKEN>(j_token);
               
            }
            return token;
        }
        public event Action<string> OnResponse;
        public event Action<string> OnRequsetFail;
        public event Action<Texture> OnResponseTexture;
        public event Action<Sprite> OnResponseSprite;
        protected void Handler_OnResponse(string response)
        {
            if (OnResponse != null)
            {
                OnResponse(response);
            }
        }
        protected void Handler_OnRequsetFail(string error)
        {
            if (OnResponse != null)
            {
                OnRequsetFail(error);
            }
        }

        protected void Handler_OnResponse(Texture response)
        {
            Debug.Log("asdasdasd" + response.width);
            if (OnResponseTexture != null)
            {
                Debug.Log("asdasdasd" + response.width);
                OnResponseTexture(response);

            }
        }

        protected void Handler_OnResponse(Sprite response)
        {
            if (OnResponseSprite != null)
            {
                OnResponseSprite(response);
            }
        }

    }

}