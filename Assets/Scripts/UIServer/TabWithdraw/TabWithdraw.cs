using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.UI.WithDrawGem
{
    public class TabWithdraw : MonoBehaviour
    {

        public Text AmountGemCash;
        public Text ShabaNumber;
        public Text OwnerAccountName;
        public Text MaxCash;
        public Button Withdraw;

        public void initWithdraw(WITHDRAWGEM withdrawgem)
        {
            Withdraw.onClick.AddListener(WithdrawClick);
            AmountGemCash.text = withdrawgem.AmountGemCash;
            ShabaNumber.text = withdrawgem.ShabaNumber;

            OwnerAccountName.text = withdrawgem.OwnerAccountName;
            MaxCash.text = withdrawgem.MaxCash;
        }

        public void WithdrawClick()
        {

        }

        private void clearTab()
        {
            AmountGemCash.text = "";
            ShabaNumber.text = "";


            OwnerAccountName.text = "";
            MaxCash.text = "";
        }
    }
    [Serializable]
    public struct WITHDRAWGEM
    {
        public string AmountGemCash;
        public string ShabaNumber;
        public string OwnerAccountName;
        public string MaxCash;
    }
}