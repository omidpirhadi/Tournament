using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diaco.UI.Reports
{
    public class ReportManager : MonoBehaviour
    {
        public ServerUI server;
        public ReportMyNetwork MyNetwork;
        public ReportMyTeams MyTeams;
        public ReportWithdraw ReportWithdraw;

        [SerializeField]
        public ReportsData reports;
        public void SetMyteams()
        {
            MyTeams.InitializeMyTeams(reports.reportsTeams);
        }
        public void SetMynework()
        {
            MyNetwork.InitializeMyTeams(reports.reportNetwork);
        }

        public void SetReportWithdarw()
        {
            ReportWithdraw.InitializReportWithdraw(reports.reportsWithdraw);
        }

        public void RequestReportsData()
        {
            server.RequestReports();
        }

        private void OnDestroy()
        {
            MyTeams.ClearCardTeamCreated();
            MyNetwork.ClearCardTeamCreated();
            ReportWithdraw.ClearReportWithdrawCard();
        }

        private void OnDisable()
        {
            MyTeams.ClearCardTeamCreated();
            MyNetwork.ClearCardTeamCreated();
            ReportWithdraw.ClearReportWithdrawCard();
        }
    }
    [Serializable]
    public struct ReportsData
    {
        public MyNetworkData reportNetwork;
        public MyTeamsData reportsTeams;
        public WithdrawData reportsWithdraw;
    }
}