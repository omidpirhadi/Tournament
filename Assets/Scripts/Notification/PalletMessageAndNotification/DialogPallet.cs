
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Diaco.Notification
{
    public class DialogPallet : MonoBehaviour
    {
        [SerializeField] private string CloseEvent = "";
        [SerializeField] private string DialogEvent = "";
        [SerializeField] private string EventData = "";
        [SerializeField] private int TypeAction;
        public RTLTMPro.RTLTextMeshPro Title_text;
        public RTLTMPro.RTLTextMeshPro Context_text;
        public TMPro.TMP_InputField EditInput_Inputfield;
        public Image Context_Image;

        public List<Button> DialogButtons;// 0  = ok btn , 1 = yes btn  ,  2  = no btn
        public Button CloseButton;

        public CanvasGroup PalletRectTransform;

        public void SetDialog(Notification_Dialog_Body body)
        {
            ClearPallet();

            this.TypeAction = body.actionButton;
            this.DialogEvent = body.eventName;
            this.EventData = body.eventData;
            this.CloseEvent = body.closeEvent;
            SetTypeDialog(body.dialogType, body.greenButtonText,body.redButtonText);
            if (body.image.Length > 0)
            {
                var image = ConvertImageToSprite(body.image);
                SetContextDialog(body.title, "", image);
             //   Debug.Log("sSSs");
            }
            if (body.context.Length > 0)
            {
                SetContextDialog(body.title, body.context, null);
             //   Debug.Log("sSssdd2223233333Ss");
            }
            SetActionButtons();
            ShowDialog(true);
        }


        private void SetTypeDialog(int type ,  string greenbuttontext , string redbuttontext )
        {
            if (type == 0)
            {

                DialogButtons[0].gameObject.SetActive(true);
                DialogButtons[0].GetComponentInChildren<RTLTMPro.RTLTextMeshPro>().text =greenbuttontext;
               // Debug.Log("0");
            }
            else if (type == 1)
            {
                DialogButtons[1].gameObject.SetActive(true);
                DialogButtons[1].GetComponentInChildren<RTLTMPro.RTLTextMeshPro>().text = greenbuttontext;
                DialogButtons[2].gameObject.SetActive(true);
                DialogButtons[2].GetComponentInChildren<RTLTMPro.RTLTextMeshPro>().text = redbuttontext;
              //  Debug.Log("1");
            }
            else if (type == 2)
            {
                DialogButtons[0].gameObject.SetActive(true);
                DialogButtons[0].GetComponentInChildren<RTLTMPro.RTLTextMeshPro>().text = greenbuttontext;

                EditInput_Inputfield.gameObject.SetActive(true);
                EditInput_Inputfield.text = "";
        //        Debug.Log("2");
            }
        }
        private void  SetContextDialog(string title , string context_text  ,Sprite contex_image )
        {
            Title_text.text = title;
            if(context_text.Length>0)
            {
                
                this.Context_text.text = context_text;
            //    Debug.Log("qq"+ context_text);
            }
            if(contex_image !=null)
            {
                Context_Image.gameObject.SetActive(true);
                this.Context_Image.sprite = contex_image;
               // Debug.Log("xxx");
            }
           // Debug.Log("ddd"+title);
        }

        private void SetActionButtons()
        {
            //// ok button
            DialogButtons[0].onClick.AddListener(() => {
                if(TypeAction == 0)
                {
                    if (DialogEvent == "store")
                    {
                        
                        var ui = FindObjectOfType<NavigationUI>();
                        ui.CloseAllPopUp();
                        ui.SwitchUI("shop");
                        FindObjectOfType<ServerUI>().RequestItemShop();
                        ShowDialog(false);
                    }
                }
                else
                {
                    SetServerForEmitData();
                    ShowDialog(false);
                }


            });
            //// yes button
            DialogButtons[1].onClick.AddListener(() => {
                if (TypeAction == 0)
                {
                    if (DialogEvent == "store")
                    {
                        var ui = FindObjectOfType<NavigationUI>();
                        ui.CloseAllPopUp();
                        ui.SwitchUI("shop");
                        FindObjectOfType<ServerUI>().RequestItemShop();
                        ShowDialog(false);
                    }
                }
                else
                {
                    
                    SetServerForEmitData();
                    ShowDialog(false);
                }

            });
            //// no btn  
            DialogButtons[2].onClick.AddListener(() => {

                ShowDialog(false);
                ClearPallet();
            });
            /// close bttn
            CloseButton.onClick.AddListener(() => {

                FindObjectOfType<ServerUI>().Emit_CloseDialog(CloseEvent);
                ShowDialog(false);
                
                ClearPallet();
            });
        }

        private  void  SetServerForEmitData()
        {
            var server_ui = FindObjectOfType<ServerUI>();
            var server_billiard = FindObjectOfType<Diaco.EightBall.Server.BilliardServer>();
            var server_soccer = FindObjectOfType<Diaco.SoccerStar.Server.ServerManager>();
            if (server_ui)
            {
                if (EditInput_Inputfield.text.Length > 1)
                    server_ui.Emit_DialogAndNotification(DialogEvent, EditInput_Inputfield.text);
                else
                    server_ui.Emit_DialogAndNotification(DialogEvent, EventData);
            }

            else if (server_billiard)
            {
                if (EditInput_Inputfield.text.Length > 1)
                    server_billiard.Emit_DialogAndNotification(DialogEvent, EditInput_Inputfield.text);
                else
                    server_billiard.Emit_DialogAndNotification(DialogEvent, EventData);
            }
            else if (server_soccer)
            {
                if (EditInput_Inputfield.text.Length > 1)
                    server_soccer.Emit_DialogAndNotification(DialogEvent, EditInput_Inputfield.text);
                else
                    server_soccer.Emit_DialogAndNotification(DialogEvent, EventData);
            }
        }
        //[Button("ShowTest", ButtonSizes.Medium, ButtonStyle.Box)]
        private void ShowDialog(bool show)
        {

            this.gameObject.SetActive(show);

           // ClearPallet();
          /*  if (show)
            {
                PalletRectTransform.alpha = 1;
                PalletRectTransform.interactable = true;
                PalletRectTransform.blocksRaycasts = true;
            }
            else
            {
                PalletRectTransform.alpha = 0;
                PalletRectTransform.interactable = false;
                PalletRectTransform.blocksRaycasts = false;
            }*/
        }

        private void ClearPallet()
        {
            DialogButtons[0].gameObject.SetActive(false);
            DialogButtons[0].GetComponentInChildren<RTLTMPro.RTLTextMeshPro>().text = "";
            DialogButtons[1].gameObject.SetActive(false);
            DialogButtons[1].GetComponentInChildren<RTLTMPro.RTLTextMeshPro>().text = "";
            DialogButtons[2].gameObject.SetActive(false);
            DialogButtons[2].GetComponentInChildren<RTLTMPro.RTLTextMeshPro>().text = "";

            EditInput_Inputfield.gameObject.SetActive(false);
            EditInput_Inputfield.text = "";

            Context_Image.gameObject.SetActive(false);
            Context_Image.sprite = null;
            Context_text.text = "";
            Title_text.text = "";
            DialogEvent = ""; 
            DialogButtons[0].onClick.RemoveAllListeners();
            DialogButtons[1].onClick.RemoveAllListeners();
            DialogButtons[2].onClick.RemoveAllListeners();
            CloseButton.onClick.RemoveAllListeners();
        }

        private Sprite ConvertImageToSprite(string image)
        {

            var image_byte = System.Convert.FromBase64String(image);
            Texture2D texture = new Texture2D(512, 512, TextureFormat.DXT5, false);
            texture.LoadImage(image_byte);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
    }
}