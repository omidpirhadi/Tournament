using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Store.Soccer
{
    
    public class SoccerShopOwnerElement : MonoBehaviour
    {
        public TypeElement typeElement;
        public string ID;
        public Image TeamImage;
        public Image PrograssBarForce;
        public Image PrograssBarAim;
        public Text RemainderTime;
        public Button Btn_Use;

        public void SetForTeamElement(string id, Sprite teamImage, float force, float aim, string time)
        {
            ID = id;
            TeamImage.sprite = teamImage;
            PrograssBarForce.fillAmount = force;
            PrograssBarAim.fillAmount = aim;
            RemainderTime.text = time;
            Btn_Use.onClick.AddListener(UseButtonClick);
            typeElement = TypeElement.Team;
        }
        public void SetForFormationElement(string id, Sprite teamImage, string time)
        {
            ID = id;
            TeamImage.sprite = teamImage;
            RemainderTime.text = time;
            typeElement = TypeElement.Formation;
            Btn_Use.onClick.AddListener(UseButtonClick);
        }
        private void UseButtonClick()
        {
            if(typeElement  ==  TypeElement.Team)
            {
                if(FindObjectOfType<ServerUI>())//in ui
                {
                    FindObjectOfType<ServerUI>().Emit_UseTeam(ID);
                    Debug.Log("Use This Team:" + ID);
                  
                }
                
            }
            else if(typeElement == TypeElement.Formation)
            {
                if (FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>())///in game
                {
                    FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>().Emit_UseFormation(ID);
                    Debug.Log("Use This Formation:" + ID);
                }
                else//in ui
                {
                    FindObjectOfType<ServerUI>().Emit_UseFormation(ID);
                    Debug.Log("Use This Formation:" + ID);
                }
            }
           

        }
    }
}