
using System;
using System.Collections.Generic;
using UnityEngine;

public class StickerPanel : MonoBehaviour
{
    public _GameLobby Game;
    public List<Sticker> Stickers;
    public ElementSticker Elementsticker;
    public Transform Grid;

    private List<GameObject> liststicker = new List<GameObject>();


    private Texture2D texture;


    private void Start()
    {
        texture = new Texture2D(512, 512);
    }          
    private void OnEnable()
    {
      //  liststicker = new List<GameObject>();
        if (Game ==  _GameLobby.Soccer)
        {

            FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>().Emit_GetSticker();
            FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>().GetStickers += ChatInGame_GetStickers;
        }
        else if (Game == _GameLobby.Billiard)
        {
            FindObjectOfType<Diaco.EightBall.Server.BilliardServer>().Emit_GetSticker();
            FindObjectOfType<Diaco.EightBall.Server.BilliardServer>().GetStickers += ChatInGame_GetStickers;
        }


    }

    

    private void OnDisable()
    {
        if (Game == _GameLobby.Soccer)
        {
            FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>().GetStickers -= ChatInGame_GetStickers;
        }
        else if (Game == _GameLobby.Billiard)
        {
           
            FindObjectOfType<Diaco.EightBall.Server.BilliardServer>().GetStickers -= ChatInGame_GetStickers;
        }

    }

    private void ChatInGame_GetStickers(StickerData data)
    {
        InitChatInGame(data);
    }
    public void InitChatInGame(StickerData data)
    {
        
            Clear();
        

        for (int i = 0; i < data.stickers.Length; i++)
        {
            var element = Instantiate(Elementsticker, Grid);
            element.Set((data.stickers[i] - 1).ToString(), LoadImage(Stickers[data.stickers[i] - 1].Titel), "");
            liststicker.Add(element.gameObject);
        }
    }
    private Sprite LoadImage(Texture2D image)
    {
        return Sprite.Create(image, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
    private void Clear()
    {
        for (int i = 0; i < liststicker.Count; i++)
        {
            Destroy(liststicker[i]);
        }
        liststicker.Clear();
        Debug.Log("Clear");
    }
}
[Serializable]
public struct StickerData
{

    public int[] stickers;
}