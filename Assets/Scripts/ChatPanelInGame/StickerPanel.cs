
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
    private Sprite sprite;

    private void Start()
    {
        
    }
    private void OnEnable()
    {
        texture = new Texture2D(512, 512);
        sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        //  liststicker = new List<GameObject>();
        if (Game == _GameLobby.Soccer)
        {

            FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>().Emit_GetSticker();
            FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>().GetStickers += ChatInGame_GetStickers;
        }
        else if (Game == _GameLobby.Billiard)
        {
            FindObjectOfType<Diaco.EightBall.Server.BilliardServer>().Emit_GetSticker();
            FindObjectOfType<Diaco.EightBall.Server.BilliardServer>().GetStickers += ChatInGame_GetStickers;
        }
        else if (Game == _GameLobby.MainMenu)
        {
            //  FindObjectOfType<ServerUI>().Emit_GetSticker();
            // FindObjectOfType<ServerUI>().GetStickers += ChatInGame_GetStickers; 
            InitChatInGame(new StickerData { stickers = new int[] { 1, 2, 3, 5, 8, 9, 10 } });
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
        else if (Game == _GameLobby.MainMenu)
        {
            FindObjectOfType<ServerUI>().GetStickers -= ChatInGame_GetStickers;
        }
        texture = null;

        sprite = null;
        Clear();

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

            for (int j = 0; j < Stickers.Count; j++)
            {
                if(data.stickers[i].ToString() == Stickers[j].stickerName)
                {
                    var element = Instantiate(Elementsticker, Grid);

                    var image = LoadImage(Stickers[j].Titel);
                    element.Set(Stickers[j].stickerName, image, "");
                    liststicker.Add(element.gameObject);
                }
            }
               
            
        }
    }
    private Sprite LoadImage(Texture2D image)
    {

       
        sprite = Sprite.Create(image, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        return sprite;
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