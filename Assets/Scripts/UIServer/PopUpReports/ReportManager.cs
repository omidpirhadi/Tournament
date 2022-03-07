using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diaco.UI.Reports
{
    public class ReportManager : MonoBehaviour
    {
        public ReportMyNetwork MyNetwork;
        public ReportMyTeams MyTeams;
        public ReportWithdraw ReportWithdraw;

       
        public void SetMynework(MyNetworkData data)
        {
            MyNetwork.InitializeMyTeams(data);
        }
        public void SetMyteams(MyTeamsData  data)
        {
            MyTeams.InitializeMyTeams( data); 
        }
        public void SetReportWithdarw(WithdrawData data)
        {
            ReportWithdraw.InitializReportWithdraw(data);
        }
    }
}