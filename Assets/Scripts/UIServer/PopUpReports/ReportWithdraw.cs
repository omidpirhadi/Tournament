using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.Reports
{
    public class ReportWithdraw : MonoBehaviour
    {

       
        public Text TotalWithdraw;

        public ReportWithdrawCard ReportWithdrawCardElement;
        public RectTransform Content;
        private List<ReportWithdrawCard> listReportWithdrawCard = new List<ReportWithdrawCard>();

        public void InitializReportWithdraw(WithdrawData mywithdraw)
        {
            // listReportWithdrawCard = new List<ReportWithdrawCard>();
            ClearReportWithdrawCard();
            TotalWithdraw.text = mywithdraw.totalwithdraw;
            for (int i = 0; i < mywithdraw.withdraws.Count; i++)
            {
                var card = Instantiate(ReportWithdrawCardElement, Content);
                card.initReportWithdrawCard(mywithdraw.withdraws[i].BankCardNumber, mywithdraw.withdraws[i].Date, mywithdraw.withdraws[i].Cash);
                listReportWithdrawCard.Add(card);
            }
        }
        public void ClearReportWithdrawCard()
        {
            for (int i = 0; i < listReportWithdrawCard.Count; i++)
            {
                Destroy(listReportWithdrawCard[i].gameObject);
            }
            listReportWithdrawCard.Clear();
        }
    }
    [Serializable]
    public struct Withdraw

    {
        public string BankCardNumber;
        public string Date;
        public string Cash;
    }
    [Serializable]
    public struct WithdrawData
    {
        public string totalwithdraw;
        public List<Withdraw> withdraws;

    }
}