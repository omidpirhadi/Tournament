using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BestHTTP.SocketIO;
using DG.Tweening;
using Diaco.HTTPBody;
public class ServerManagerRecordMode : MonoBehaviour
{
    public SocketManager socket_manager;
    public Socket socket;
   public string namespaceServer;
  

    public void ConnectToServer(string URL)
    {
        var namespaceserver = FindObjectOfType<GameLuncher>().NamespaceServer;
        this.namespaceServer = namespaceserver;
        SocketOptions options = new SocketOptions();
        options.AutoConnect = true;
        socket_manager = new SocketManager(new Uri(URL), options);
        socket = socket_manager["/soccer" + namespaceserver];
        socket.On("connect", (s, p, a) =>
        {
            socket.Emit("authToken", ReadToken("token"));
            print("Connected RecodeMode");

        });


        socket.On("disconnect", (s, p, a) =>
        {
            print("disConnected RecordMode");
        });
    }

    


    public void CloseSocket()
    {
        socket.Off();
        socket.Manager.Close();
        socket.Disconnect();
        Debug.Log("SoccerCloseConnection");
    }
    public string ReadToken(string FileName)
    {
        TOKEN token = new TOKEN();
        if (File.Exists(Application.persistentDataPath + "//" + FileName + ".json"))
        {
            var j_token = File.ReadAllText(Application.persistentDataPath + "//" + FileName + ".json");
            token = JsonUtility.FromJson<Diaco.HTTPBody.TOKEN>(j_token);

        }
        return token.token;
    }

    public IEnumerator ResetData()
    {
        yield return new WaitForSeconds(0.1f);

    }

}
