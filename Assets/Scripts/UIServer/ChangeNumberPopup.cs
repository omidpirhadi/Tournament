using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeNumberPopup : MonoBehaviour
{
    public ServerUI server;
    
    public InputField OldNumber;
    public InputField NewNumber;
    public InputField ConfirmCode;
    public Button AcceptButton;

    private bool needcode = true;
    private void OnEnable()
    {
        server = FindObjectOfType<ServerUI>();

        OldNumber.text = server.BODY.phone;
        NewNumber.onValueChanged.AddListener((context) => {
            needcode = true;
            ConfirmCode.interactable = false;
            ConfirmCode.text = "";
            if (context.Length == 11 && (context.StartsWith("09") || context.StartsWith("۰۹")))
            {
                AcceptButton.interactable = true;
                AcceptButton.GetComponentInChildren<Text>().text = PersianFix.Persian.Fix("ارسال کد", 255);

            }
            else
            {
                AcceptButton.interactable = false;
                AcceptButton.GetComponentInChildren<Text>().text = PersianFix.Persian.Fix("ارسال کد", 255);

            }
        });

        if (AcceptButton)
        {
            AcceptButton.onClick.AddListener(() =>
            {
                if (needcode)
                {
                    server.SendRequestForEditPhone(NewNumber.text);
                    AcceptButton.GetComponentInChildren<Text>().text = PersianFix.Persian.Fix("تایید", 255);
                    needcode = false;
                    ConfirmCode.interactable = true;

                }
                else
                {
                    server.SendPhonAndConfrimCode(NewNumber.text, ConfirmCode.text);
                }



            });
        }


    }
    private void OnDisable()
    {
        AcceptButton.onClick.RemoveAllListeners();
        NewNumber.onValueChanged.RemoveAllListeners();

    }
}
