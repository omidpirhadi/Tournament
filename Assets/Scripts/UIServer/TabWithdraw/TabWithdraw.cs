using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.UI.WithDrawGem
{
    public class TabWithdraw : MonoBehaviour
    {
        public Text DiamondAmount;
       
        public InputField ShabaNumber;
        public TMPro.TMP_InputField OwnerAccountName;
        public InputField Amount;
        public Button Withdraw;

 
        public void Wihtdrawinit()
        {
            var server = FindObjectOfType<ServerUI>();
            DiamondAmount.text = server.BODY.withdraw;
            
            ShabaNumber.text = "";
            OwnerAccountName.text = "";
            Amount.text = "";
            Withdraw.onClick.AddListener(WithdrawClick);
        }
        public void WithdrawClick()
        {
            var server = FindObjectOfType<ServerUI>();
            WithdrawData data = new WithdrawData() { shabanumber = ShabaNumber.text, accountname = OwnerAccountName.text, amount = Amount.text };
            server.RequestWithdraw(data);
        }
       
   
    }
    [Serializable]
    public struct WithdrawData
    {
        
        public string shabanumber;
        public string accountname;
        public string amount;
    }
}