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

            FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>().Emit_ShareSticker(Convert.ToInt32(ID));
           
        }
        else if (FindObjectOfType<Diaco.EightBall.Server.BilliardServer>())
        {

        }
        Debug.Log("SendSticker:" + ID);
    }


}
