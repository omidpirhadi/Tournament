using System;

using UnityEngine;
using UnityEngine.UI;
public class ElementSticker : MonoBehaviour
{

    public string ID;
    public Image ImageSticker;
    public Text TitleSticker;
    public Button Submit;

    public  void Set(string id,Sprite image, string title)
    {
        ID = id;
        ImageSticker.sprite = image;
        TitleSticker.text = title;
        Submit.onClick.AddListener(onclick);

    }
    private  void onclick()
    {
        if (FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>())
        {

            FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>().Emit_ShareSticker(ID);
            Debug.Log("SendSticker In Game:" + ID);
        }
        else if (FindObjectOfType<Diaco.EightBall.Server.BilliardServer>())
        {
            FindObjectOfType<Diaco.EightBall.Server.BilliardServer>().Emit_ShareSticker(ID);
            Debug.Log("SendSticker In Game:" + ID);

        }
        else if (FindObjectOfType<ServerUI>())
        {
            if(FindObjectOfType<Diaco.UI.Chatbox.ChatBoxController>())
            {
                FindObjectOfType<Diaco.UI.Chatbox.ChatBoxController>().SendSticekr(ID);
                Debug.Log("SendSticker  In Chat:" + "##" + ID);
            }
            else if (FindObjectOfType<Diaco.Chat.TeamChatsController>())
            {
                FindObjectOfType<Diaco.Chat.TeamChatsController>().SendSticekr(ID);
                Debug.Log("SendSticker  In Chat Team:" + "##" + ID);
            }

             
        }
        
    }


}
