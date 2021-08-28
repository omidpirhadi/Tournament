using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.Reports
{
    public class ReportWithdrawCard : MonoBehaviour
    {

        public Text BankCardNumber;
        public Text Date;
        public Text Cash;

        public void initReportWithdrawCard(string bankcardnumber, string data, string cash)
        {
            BankCardNumber.text = bankcardnumber;
            Date.text = data;
            Cash.text = cash;
        }
    }
}