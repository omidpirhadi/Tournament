using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum _GameLobby { Soccer = 0, Billiard = 1 }
public enum _SubGame { Classic = 0, Quick = 1, Challenge = 2 }
public class NavigationUI : MonoBehaviour
{
    public ServerUI Server;
    public string CurrentPage = "";
    public string LastProfileChecked = "";
    public string LastTeamInfoChecked = "";
    public GameObject LoadingPage;

    public _GameLobby GameLobby;
    public _SubGame SubGame;

    [Serializable]
    public struct UIInfo
    {
        public string Name;
        public GameObject UIObject;
        public bool HeaderShow;
        public bool FooterShow;
        public bool ShowBackButtonInFooter;
    }
    public List<UIInfo> CanvansManager;
    [Serializable]
    public struct PopUp
    {
        public string Name;
        public GameObject PopUpObject;
    }
    public List<PopUp> UIPopUps;

    public GameObject Header;
    public Color SelectedHeaderButtonsColor;
    public Color ReadyForSelectButtonsColor;
    public List<Button> ListButtonOfHeader;

    public GameObject Footer;
    public GameObject BackButton;

    public float TimeTooltipShow = 5.0f;
    public float TimeFadeInTooltip = 1.0f;
    public float TimeFadeOutTooltip = 1.0f;
    private Coroutine LoadingPageshow;
    [SerializeField] private RectTransform content;

    public SoundEffectControll soundEffectControll;
    public void Start()
    {
        if(Server)
        {
            soundEffectControll = GameObject.Find("SoundEffectlLayer2").GetComponent<SoundEffectControll>();
        }
    }
    public void SwitchUI(string UIName)
    {
        
        CanvansManager.ForEach((ui) =>
        {
            if(ui.Name == UIName)
            {
                
                ui.UIObject.SetActive(true);
                if (soundEffectControll)
                {
                    soundEffectControll.PlaySoundMenu(1);
                }
                CurrentPage = ui.Name;
                if(ui.HeaderShow)
                {
                    if (Header)
                    {
                        Header.SetActive(true);
                        
                    }
                }
                else
                {
                    if (Header)
                    {
                        Header.SetActive(false);
                    }
                }
                if (ui.FooterShow)
                {
                    if (Footer)
                    {
                        Footer.SetActive(true);
                        if(ui.ShowBackButtonInFooter)
                        {
                            BackButton.SetActive(true);
                        }
                        else
                        {
                            BackButton.SetActive(false);
                        }
                    }
                }
                else
                {
                    if (Footer)
                    {

                    
                        Footer.SetActive(false);
                    }
                }

            }
            else
            {
                if (ui.UIObject)
                    ui.UIObject.SetActive(false);
            }
            Handler_OnUIActive(true);
        });
    }
    public void ColorButtonFix(Button button)
    {
       var block =  button.colors;
        block.normalColor = SelectedHeaderButtonsColor;
        button.colors = block;
        ListButtonOfHeader.ForEach((btn) => {
            if (btn != button)
            {
                var block_btn = btn.colors;
                block_btn.normalColor = ReadyForSelectButtonsColor;
                btn.colors = block_btn;
            }
        });
    }
    public void ShowPopUp(string Popup)
    {
        UIPopUps.ForEach((pop) =>
        {
            
            if (pop.Name == Popup)
            {
                if (pop.PopUpObject)
                {
                    //Debug.Log("AAA");
                    pop.PopUpObject.SetActive(true);
                    CurrentPage = pop.Name;
                  //  Debug.Log("ShowPopup"+pop.Name);
                    if (soundEffectControll)
                    {
                        soundEffectControll.PlaySoundMenu(1);
                    }
                }
            }
            else
            {
                if (pop.PopUpObject)
                    pop.PopUpObject.SetActive(false);
            }
        });
        Handler_OnUIActive(true);
        
    }
    
    public void ShowTooltip(GameObject Tooltip)
    {
        /// Tooltip.SetActive(true);
        var tooltip_canvas = Tooltip.GetComponent<CanvasGroup>();
        tooltip_canvas.DOFade(1.0f, TimeFadeInTooltip).OnComplete(() =>
        {
            DOVirtual.Float(0, 1, TimeTooltipShow, (TimeTooltip) =>
            {
            }).OnComplete(() =>
            {
                tooltip_canvas.DOFade(0.0f, TimeFadeOutTooltip);
            });
            //Tooltip.SetActive(false);
        });
    }
    public void ClosePopUp(string Popup)
    {
        UIPopUps.ForEach((pop) =>
        {
            if (pop.Name == Popup)
            {
                if (pop.PopUpObject)
                {
                    pop.PopUpObject.SetActive(false);
                    
                }
            }
        });
        Handler_OnUIActive(false);
    }
    public void CloseAllPopUp()
    {
        UIPopUps.ForEach((pop) =>
        {

            if (pop.PopUpObject)
            {
                pop.PopUpObject.SetActive(false);

            }


        });
        Handler_OnUIActive(false);
    }

    private IEnumerator LoadingPageShow()
    {
         
         LoadingPage.SetActive(true);
         yield return new WaitForSecondsRealtime(5.0f);
         LoadingPage.SetActive(false);
         Debug.Log("Close Self loading");
        
    }
    public void StartLoadingPageShow()
    {
        LoadingPageshow = StartCoroutine(LoadingPageShow());
    }
    public void StopLoadingPage()
    {
        LoadingPage.SetActive(false);

        if (LoadingPageshow != null)
            StopCoroutine(LoadingPageshow);
     //   Debug.Log("Closeloading");
    }
    public void loadTeaminfoWithLastTeamID()
    {
        Server.GetTeamInfo(LastTeamInfoChecked);
       
    }
    public void LoadProfileWithLastProfile()
    {
        Server.GetProfilePerson(LastProfileChecked);
    }
    public void SetLobby( string lobby)
    {
        if (lobby == "soccer")
            GameLobby = _GameLobby.Soccer;
        else
            GameLobby = _GameLobby.Billiard;

    }

    public void SetSubGame(string SubGame)
    {
        if(SubGame == "classic")
        {
            this.SubGame = _SubGame.Classic;
        }
        else if( SubGame == "quick")
        {
            this.SubGame = _SubGame.Quick;
        }
        else if(SubGame == "challenge")
        {
            this.SubGame = _SubGame.Challenge;
        }

    }



    public event Action<bool> OnUIActive;
    protected void Handler_OnUIActive(bool active)
    {
        if (OnUIActive != null)
        {
            OnUIActive(active);
        }
    }
}
