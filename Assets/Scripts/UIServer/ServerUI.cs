using System;
using System.Collections;
using System.IO;

using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
using BestHTTP.SocketIO;
using Diaco.HTTPBody;
public class ServerUI : MonoBehaviour
{
    /// <summary>
    /// SERVER IP LOCAL = "http://192.168.1.100:8420/socket.io/"
    /// SERVER IP GOLBAL = "http://37.152.185.15:8420/socket.io/"
    /// </summary>
   /// public string UIServerURL = "http://192.168.1.109:8420/socket.io/";

    public Diaco.ImageContainerTool.ImageContainer AvatarContainer, LeagueFlagsContainer, ImageGameType, ImageTypeCosts;
    public GameObject MainMenu, Footer, Header, Login, SplashScreen, LoginError, ExitApp;
    [SerializeField]
    public BODY BODY;
    ///public Shop Shop;
    ///  public SoccerShopProducts SoccerShopProducts;
    // public BilliardShopProducts BilliardShopProducts;
    public Socket socket;
    public SocketManager socketmanager;
    public GameLuncher Luncher;
  //  public Diaco.Notification.NotificationPopUp NotificationPopUp;
    public NavigationUI navigationUi;
    public  Diaco.UI.UIRegisterOnServer UIInFooterAndHeader;

    private bool loadedpage = false;
    private int intergation = 0;

    public float NormalPing = 2.5f;
    private float Ping = 0.0f;
    private Diaco.Notification.Notification_Dialog_Manager Notification_Dialog;
    public void Update()
    {

        if(Input.GetKey(KeyCode.Escape) &&  navigationUi.CurrentPage == "selectgame")
        {
            ExitApp.SetActive(true);
        }

    }
    #region Server_ON
    public void ConnectToUIServer()
    {
        Luncher = FindObjectOfType<GameLuncher>();
        var setting = FindObjectOfType<Diaco.Setting.GeneralSetting>();
        Notification_Dialog = FindObjectOfType<Diaco.Notification.Notification_Dialog_Manager>();
        var PushNotification = FindObjectOfType<Diaco.Notification.PushNotification>();
        navigationUi = FindObjectOfType<NavigationUI>();

        

        string URL = setting.ServerAddress;

        Notification_Dialog.server = this;
        Notification_Dialog.init_Notification_menu();

        
        PushNotification.server = this;
        PushNotification.InstantiateEvent();

       
        navigationUi.OnChangePage += ServerUI_OnChangePage;

        SocketOptions options = new SocketOptions();
        options.AutoConnect = true;
        socketmanager = new SocketManager(new Uri(URL), options);
        socket = socketmanager.Socket;

        socket.On("connect", (s, p, m) =>
        {

            intergation = 0;
            socket.Emit("authToken", ReadToken("token"), setting.Version, setting.Store);
            Emit_Setting();
            CancelInvoke("Emit_Ping");
            Ping = 0.0f;
            Notification_Dialog.InternetPingDialog(new Diaco.Notification.Notification_Dialog_Body { alartType = 3, context = Ping.ToString() }, false);
            InvokeRepeating("Emit_Ping", 3.0f, 1.0f);////For Get ping in sec 
            navigationUi.StopLoadingPage();
            FindObjectOfType<Diaco.Store.CafeBazzar.CafeBazzarStore>().InitializeCafebazzarShop();

            Debug.Log($"<color=blue><b>Connection And ReadToken</b></color>");
        });
        socket.On("pong2", (s, p, m) => {

            Ping = Time.realtimeSinceStartup - Ping;
            if (Ping > NormalPing)
                Notification_Dialog.InternetPingDialog(new Diaco.Notification.Notification_Dialog_Body { alartType = 3, context ="اتصال اینترنت شما ضعیف است." }, true);
            else
                Notification_Dialog.InternetPingDialog(new Diaco.Notification.Notification_Dialog_Body { alartType = 3, context = Ping.ToString() }, false);
          //  Debug.Log("Ping is :" + Ping);
        });
        socket.On("reconnect", (s, p, m) => {
            Ping = 0;
            CancelInvoke("Emit_Ping");

            Notification_Dialog.InternetPingDialog(new Diaco.Notification.Notification_Dialog_Body { alartType = 3, context = "اتصال اینترنت خود را بررسی کنید." }, true);
            Debug.Log("reconnect");

        });
       // socket.On("reconnecting", (s, p, m) => { Debug.Log("reconnecting"); });
        socket.On("wrong-token", (s, p, m) =>
        {
            Login.SetActive(true);
            LoginError.SetActive(false);
            SplashScreen.SetActive(false);
            /*DeleteToken("token");
            StartCoroutine(Luncher.RestartGame());*/
            CloseConnectionUIToServer(); 

            Debug.Log($"<color=red><b>Wrong Token</b></color>");
        });
        socket.On("loginError", (S, p, m) =>
        {

            LoginError.SetActive(true);

            Login.SetActive(false);

            SplashScreen.SetActive(false);
        });
        socket.On("changePhoneError", (S, p, m) =>
        {
            var message = Convert.ToBoolean(m[0]);
            if (!message)///changed number ;
            {
                navigationUi.StopLoadingPage();
                navigationUi.ClosePopUp("changenumber");
                Debug.Log("Number Changed");
            }
            else

            {
                Debug.Log("Number Not Changed");
            }

        });
        socket.On("main-menu", (s, p, m) =>
        {

            

            if(Diaco.Store.CafeBazzar.CafeBazzarStore.instance.ExistTransactionFile("translog"))
            {
                var dt = Diaco.Store.CafeBazzar.CafeBazzarStore.instance.ReadTransaction("translog");
                Emit_Transaction(dt);
            }
            BODY.inventory.tickets = new System.Collections.Generic.List<TicketData>();
            var ticket = FindObjectOfType<Diaco.UI.TicketManagers.TicketManager>();

            var byte_data = p.Attachments[0];
            var json = System.Text.UTF8Encoding.UTF8.GetString(byte_data);
            BODY = JsonUtility.FromJson<BODY>(json);


            if (navigationUi.CurrentPage != "findplayer")
            {
                if (BODY.inGame.id != "" && intergation == 0)
                {
                    /// Debug.Log("InGame");
                    if (BODY.inGame.gameType == "soccer")
                    {
                        //navigationUi.GetComponent<SceneManagers>().loadlevel("SoccerGame");
                        Debug.Log("Soccer In Game ");
                        if (BODY.inGame.namespaceServer == "_record")
                        {
                            Luncher.SetNameSpaceServer(2, BODY.inGame.namespaceServer);
                            Luncher.SwitchScene(2);

                        }
                        else
                        {
                            Luncher.SetNameSpaceServer(0, BODY.inGame.namespaceServer);
                            Luncher.SwitchScene(0);

                        }

                    }
                    else if (BODY.inGame.gameType == "billiard")
                    {
                        // navigationUi.GetComponent<SceneManagers>().loadlevel("8ballgame");
                        Debug.Log("Billiard In Game");
                        if (BODY.inGame.namespaceServer == "_record")
                        {
                            Luncher.SetNameSpaceServer(3, BODY.inGame.namespaceServer);
                            Luncher.SwitchScene(3);
                            // Debug.Log("InGame!!!!!");
                        }
                        else
                        {
                            Luncher.SetNameSpaceServer(1, BODY.inGame.namespaceServer);
                            Luncher.SwitchScene(1);
                            //  Debug.Log("InGame@@@@@");
                        }
                    }
                    intergation = 1;
                    return;
                }
            }



            if (loadedpage == false)
            {
                Login.SetActive(false);
                //  Register.SetActive(false);
                SplashScreen.SetActive(false);

                MainMenu.SetActive(true);
                Footer.SetActive(true);
                Header.SetActive(true);
                loadedpage = true;
            }
          

            UIInFooterAndHeader.initTournmentCard(BODY.profile.tournaments);
            SetElementInHeaderAndFooter();
            if (ticket != null)
                ticket.Show(BODY.inventory.tickets);
            Handler_OnCreateTeamCompeleted();

            Handler_OnGameBodyUpdate();
            navigationUi.StopLoadingPage();
              Debug.Log("main-menu called");
        });

        socket.On("search-friend", (s, p, m) =>
        {

            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error:" + m[1].ToString());
            }
            else
            {
                var resault_search = JsonUtility.FromJson<SearchUser>(m[1].ToString());
                Handler_onResualtSearchFriend(resault_search);

                Debug.Log("SEARCHFRIEND::::" + m[1].ToString());
            }
            navigationUi.StopLoadingPage();
        });
        socket.On("get-friends", (s, p, m) =>
        {

            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error:" + m[1].ToString());
            }
            else
            {

                var friends = JsonUtility.FromJson<Friends>(m[1].ToString());
                BODY.social.friends = friends.friends;
                Handler_oncomingfriends(friends);
                navigationUi.StopLoadingPage();
                Debug.Log("GETFRIEND");
            }
        });
        socket.On("add-friend", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error:" + m[1].ToString());
            }
            else
            {
                Debug.Log("ADDED" + m[1].ToString());
                navigationUi.StopLoadingPage();
            }
            navigationUi.StopLoadingPage();
        }); //
        socket.On("accept-request", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error:" + m[1].ToString());
            }
            else
            {
                Debug.Log("AcceptRequest" + m[1].ToString());
                navigationUi.StopLoadingPage();
            }
            navigationUi.StopLoadingPage();
        });

        socket.On("open-lobby", (s, p, m) =>
        {
            BODY.guidContext = m[0].ToString();
            navigationUi.CloseAllPopUp();
            navigationUi.SwitchUI("findplayer");
            Debug.Log("Open Lobby Game");

        });
        socket.On("join-lobby", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error:" + m[1].ToString());
            }
            else
            {
                var opponent = JsonUtility.FromJson<Opponent>(m[1].ToString());
                if (opponent.game == 0)
                    navigationUi.GameLobby = _GameLobby.Soccer;
                else
                    navigationUi.GameLobby = _GameLobby.Billiard;
                // Luncher.SwitchScene(Convert.ToInt16(m[2]) + 1);
                Luncher.SetNameSpaceServer(opponent.game, opponent.namespaceServer);

                Handler_OnOpponentFind(opponent.userName, opponent.avatar);
            }
            
        });
        socket.On("join-game", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error:" + m[1].ToString());
            }
            else
            {
                // Luncher.GotoSceneGameWithFriend();
                Luncher.SwitchScene(Convert.ToInt16(m[1]));

                Luncher.SetNameSpaceServer(Convert.ToInt16(m[1]), Convert.ToString(m[2]));
                Debug.Log("JoinGame");
            }
        });
       
        
        socket.On("update-league", (s, p, m) =>
        {

            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Handler_ErrorCreateTeam(m[1].ToString());
                Debug.Log("Error:" + m[1].ToString());

            }
            else
            {
                var byte_data = p.Attachments[0];
                var json = System.Text.UTF8Encoding.UTF8.GetString(byte_data);

                BODY = JsonUtility.FromJson<BODY>(json);
                SetElementInHeaderAndFooter();
                Handler_OnCreateTeamCompeleted();


                Debug.Log("update ggggg");
            }
            navigationUi.StopLoadingPage();
        });
        socket.On("league-rules", (s, p, m) =>
        {

            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Handler_ErrorCreateTeam(m[1].ToString());
                Debug.Log("Error:" + m[1].ToString());

            }
            else
            {

                var data = JsonUtility.FromJson<Diaco.Social.RulesData>(m[1].ToString());
                var tab = FindObjectOfType<Diaco.Social.CreateTeamTabController>();
                tab.Rules(data);
                navigationUi.StopLoadingPage();
            }
            navigationUi.StopLoadingPage();
        });
        socket.On("get-league-time", (s, p, m) =>
        {
            Handler_OnGetTimeTeam(Convert.ToSingle(m[1]));
            navigationUi.StopLoadingPage();
            Debug.Log("GetTime" + Convert.ToSingle(m[1]));
        });
        socket.On("get-league", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error:" + m[1].ToString());

            }
            else
            {
                Debug.Log(m[1].ToString());
                var teams = JsonUtility.FromJson<Teams>(m[1].ToString());
                Handler_OnGetTeams(teams);
                Debug.Log("LeagueLoaded");
            }
            navigationUi.StopLoadingPage();
        });
        socket.On("league-info", (s, p, m) =>
        {

            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error:" + m[1].ToString());

            }
            else
            {

                var teams_info = JsonUtility.FromJson<TeamInfo>(m[1].ToString());
                navigationUi.ShowPopUp("teaminfo");
                var popup = FindObjectOfType<Diaco.UI.TeamInfo.TeamInfoPopUpController>();
                popup.InitializeTeamInfo(teams_info);
                //// Handler_OnGetTeamInfo(teams_info);
                navigationUi.StopLoadingPage();
                Debug.Log("ReciveTeamInfoAndLoadedInPopUp." + teams_info.game);
            }
            navigationUi.StopLoadingPage();
        });
        socket.On("change-team-mode", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error: Team Change Mode" + m[1].ToString());

            }
            else
            {

            }
        });
        socket.On("awards", (s, p, m) =>
        {

            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error:" + m[1].ToString());

            }
            else
            {
                var awrad = JsonUtility.FromJson<AwardsName>(m[1].ToString());

                navigationUi.ShowPopUp("teamaward");
                var popup = FindObjectOfType<Diaco.UI.TeamInfo.PopupAwardConrtoller>();
                popup.initAward(awrad);


                Debug.Log("Recive League Award");
                navigationUi.StopLoadingPage();
            }

        });


        socket.On("open-chatbox", (s, p, m) =>
        {

            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("ChatBoxOpenedError:" + m[1].ToString());

            }
            else
            {
                navigationUi.ShowPopUp("chat");
                var data = JsonUtility.FromJson<Diaco.UI.Chatbox.ChatBoxData>(m[1].ToString());




                var chatbox_popup = FindObjectOfType<Diaco.UI.Chatbox.ChatBoxController>();

                chatbox_popup.SetElementPage(data);
                chatbox_popup.init_chatbox();
                Debug.Log("ChatBoxOpened");
            }
            navigationUi.StopLoadingPage();
        });
        socket.On("chat", (s, p, m) =>
        {

            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("ChatsUpdatedError:" + m[1].ToString());

            }
            else
            {

                var Chats = JsonUtility.FromJson<Chats>(m[1].ToString());
                Handler_onchatreciver(Chats);
                Debug.Log("ChatsUpdated");
            }
            navigationUi.StopLoadingPage();
        });
        socket.On("team-chat", (s, p, m) =>
        {

            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("TeamChatsUpdated Error:" + m[1].ToString());
            }
            else
            {
                var chats = JsonUtility.FromJson<TeamChats>(m[1].ToString());
                BODY.social.team.chats = chats.chats;
                handler_OnUpdateChatTeam();
                Debug.Log("TeamChatsUpdated");

            }
            navigationUi.StopLoadingPage();
        });
        socket.On("messages", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error:" + m[1].ToString());

            }
            else
            {
                Debug.Log(m[1].ToString());
                var reqs = JsonUtility.FromJson<InRequsets>(m[1].ToString());
                BODY.social.inRequests = reqs.inRequests.Count;
                Handler_OnGetMessages(reqs);
                Debug.Log("Update Messages On Social Tab");
            }
            navigationUi.StopLoadingPage();
        });
        ////////
        //// ربطی  ب من ننداره فرشاد زده
       socket.On("update-event", (s, p, m) =>
        {
            socket.Emit(m[0].ToString());
        });


        socket.On("information", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error: Update Information Body " + m[1].ToString());

            }
            else
            {
                Debug.Log("Update Information Body");
            }
            navigationUi.StopLoadingPage();
        });
        socket.On("get-profile", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error: Get Profile Person " + m[1].ToString());

            }
            else
            {
                var profile = JsonUtility.FromJson<ProfileOtherPerson>(m[1].ToString());
                navigationUi.ShowPopUp("profilefromteam");
                var popup = FindObjectOfType<Diaco.UI.Profile.ProfileOtherPersonPopup>();
                popup.InitializeProfile(profile);
                navigationUi.StopLoadingPage();
                Debug.Log("Get Profile Person");
            }
            navigationUi.StopLoadingPage();
        });
        socket.On("top-players", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error: Get TopPlayers " + m[1].ToString());

            }
            else
            {
                var topplayer = JsonUtility.FromJson<TopPlayers>(m[1].ToString());
                Handler_OnGetTopPlayers(topplayer);
                Debug.Log("Get TopPlayers");
            }
            navigationUi.StopLoadingPage();
        });

        socket.On("shop", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error: Shop: " + m[1].ToString());

            }
            else
            {
                ///Shop = new Shop();
                var Shop = JsonUtility.FromJson<Shop>(m[1].ToString());
                Handler_OnShopLoaded(Shop);
                Debug.Log("Shop Loaded" + m[1].ToString());
            }
            navigationUi.StopLoadingPage();
        });
        socket.On("transaction-bazzar", (s, p, m) =>
        {
            var product_id = m[0].ToString();
            var payload = m[1].ToString();
            Diaco.Store.CafeBazzar.CafeBazzarStore.instance.DoTransaction(product_id, payload);
        });
        socket.On("log-transaction-bazzar-delete", (s, p, m) =>
        {
            Diaco.Store.CafeBazzar.CafeBazzarStore.instance.DeleteTransaction("translog");
        });
        socket.On("shop-soccer-plan", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error: FormationShopSoccer: " + m[1].ToString());

            }
            else
            {

                var data = JsonUtility.FromJson<Diaco.Store.Soccer.SoccerShopDatas>(m[1].ToString());
                Handler_InitSoccerFormationShop(data);
                Debug.Log("Formation Shop Soccer Loaded" + m[1].ToString());
            }
            navigationUi.StopLoadingPage();
        });

        socket.On("shop-soccer-marble", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error: ShopSoccerTeam: " + m[1].ToString());

            }
            else
            {

                var data = JsonUtility.FromJson<Diaco.Store.Soccer.SoccerShopDatas>(m[1].ToString());
                Handler_InitSoccerTeamShop(data);
                Debug.Log("Shop Soccer Team Loaded" + m[1].ToString());
            }
            navigationUi.StopLoadingPage();
        });

        socket.On("shop-billiard", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error: ShopBilliard: " + m[1].ToString());

            }
            else
            {

                var data = JsonUtility.FromJson<Diaco.Store.Billiard.BilliardShopDatas>(m[1].ToString());
                Handler_InitshopBilliard(data);
                navigationUi.StopLoadingPage();
                Debug.Log("ShopBilliard Loaded");
            }
            navigationUi.StopLoadingPage();
        });

        socket.On("competitions", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error: competitions: " + m[1].ToString());

            }
            else
            {
                navigationUi.StopLoadingPage();
                var competitions = JsonUtility.FromJson<Diaco.UI.RoyalTournument.CompetitionsData>(m[1].ToString());
                Handler_ReciveMatchs(competitions);
            }
        });
        socket.On("competition-info", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error: competition-info: " + m[1].ToString());

            }
            else
            {
                navigationUi.CloseAllPopUp();
                navigationUi.ShowPopUp("royalteaminfo");
                var competitioninfo = JsonUtility.FromJson<Diaco.UI.RoyalTournument.CompetitionInfoData>(m[1].ToString());
                Handler_OnCompettionInfo(competitioninfo);
                navigationUi.StopLoadingPage();
            }
        });
        socket.On("competition-command", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error: competition-command: " + m[1].ToString());

            }
            navigationUi.StopLoadingPage();

        });
        socket.On("competition-award", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error: competition-award: " + m[1].ToString());

            }
            else
            {
                var awardcompetition = JsonUtility.FromJson<Diaco.UI.RoyalTournument.AwardsData>(m[1].ToString());
                Handler_OnCompetitionAward(awardcompetition);
                Debug.Log("competition-award");
            }
            navigationUi.StopLoadingPage();

        });
        socket.On("start-tournament", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error: star-tournament: " + m[1].ToString());

            }
            else
            {
                var tournumentdata = JsonUtility.FromJson<Diaco.UI.PopupTournumentRanking.TournomentData>(m[1].ToString());
                if (tournumentdata.capacity == 4)
                {
                    navigationUi.ShowPopUp("4playermatch");
                }
                else if (tournumentdata.capacity == 8)
                {
                    navigationUi.ShowPopUp("8playermatch");
                }
                else if (tournumentdata.capacity == 16)
                {
                    navigationUi.ShowPopUp("16playermatch");
                }
                Handler_OnStartTournument(tournumentdata);
                Debug.Log("star-tournamen :" + tournumentdata.capacity + "Player");
            }
            navigationUi.StopLoadingPage();

        });

        socket.On("get-record", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("<color=red>Error: get-record: </color>" + m[1].ToString());
            }
            else
            {
                var recorddata = JsonUtility.FromJson<Diaco.UI.MatchRecord.MatchRecordModeData>(m[1].ToString());
                Handler_OnMatchRecord(recorddata);
                Debug.Log("get-record ");
            }
            navigationUi.StopLoadingPage();

        });
        socket.On("edit-avatar", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("<color=red>Error: get-record: </color>" + m[1].ToString());

            }
            else
            {
                BODY.profile.avatar = m[1].ToString();
                UIInFooterAndHeader.ImageUser_inPageSelectGame.sprite = AvatarContainer.LoadImage(m[1].ToString());
                FindObjectOfType<Diaco.PopupAvatar.PopUpAvatarChange>().initPopupAvatar(BODY.inventory.avatars);
                FindObjectOfType<Diaco.UI.Profile.ProfilePopup>().InitializeProfile();
                navigationUi.StopLoadingPage();
                Debug.Log("EditedAvatar");
            }
            navigationUi.StopLoadingPage();

        });
        socket.On("edit-description", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("<color=red>Error: get-record: </color>" + m[1].ToString());

            }
            else
            {
                BODY.profile.description = m[1].ToString();

                FindObjectOfType<Diaco.UI.Profile.ProfilePopup>().InitializeProfile();
                navigationUi.StopLoadingPage();
                Debug.Log("EditedDescription");
            }
            navigationUi.StopLoadingPage();

        });
        socket.On("reports", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("<color=red>Error: get-record: </color>" + m[1].ToString());

            }
            else
            {
                var data = JsonUtility.FromJson<Diaco.UI.Reports.ReportsData>(m[1].ToString());

                var popup_reports = FindObjectOfType<Diaco.UI.Reports.ReportManager>();
                popup_reports.reports = new Diaco.UI.Reports.ReportsData();
                popup_reports.reports = data;
                popup_reports.SetMyteams();
                popup_reports.SetMynework();
                popup_reports.SetReportWithdarw();
                navigationUi.StopLoadingPage();
                Debug.Log("ReportsLoaded" + m[1].ToString());
            }
            navigationUi.StopLoadingPage();

        });
        socket.On("withdraw", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("<color=red>Error: get-record: </color>" + m[1].ToString());

            }
            else
            {

                BODY.withdraw = m[1].ToString();
                var tabWithdraw = FindObjectOfType<Diaco.UI.WithDrawGem.TabWithdraw>();
                tabWithdraw.Wihtdrawinit();

                Debug.Log("withdraw updated" + m[1].ToString());
            }
            navigationUi.StopLoadingPage();

        });

        socket.On("edit-username", (s, p, m) =>
        {

            if (Convert.ToBoolean(m[0]) == true)///Error
            {

                // popup.AllowUsername = false;
                Debug.Log("<color=red>Error: get-record: </color>" + m[1].ToString());
                Handler_OnChangeUsername(m[1].ToString());

            }
            else
            {




                BODY.userName = m[1].ToString();
                FindObjectOfType<Diaco.UI.Profile.ProfilePopup>().InitializeProfile();
                UIInFooterAndHeader.UserName_inPageSelectGame.text = m[1].ToString();
                navigationUi.StopLoadingPage();
                navigationUi.ClosePopUp("changeusername");
                Debug.Log("ReportsLoaded" + m[1].ToString());
            }

        });
        socket.On("webview", (s, p, m) =>
        {

            if (Convert.ToBoolean(m[0]) == true)///Error
            {

                // popup.AllowUsername = false;
                Debug.Log("<color=red>Error: Webviwe cant Loaded: </color>" + m[1].ToString());
                ///Handler_OnChangeUsername(m[1].ToString());

            }
            else
            {


                navigationUi.ShowPopUpOnPopup("webviwe");
                var webview = FindObjectOfType<UniWebView>();
                webview.CleanCache();
                webview.urlOnStart = m[1].ToString();

                webview.Load(m[1].ToString());
                webview.Show();
                webview.UpdateFrame();
                Debug.Log("WebViweLoaded" + m[1].ToString());
            }

        });
        socket.On("close-webview", (s, p, m) =>
        {

            if (Convert.ToBoolean(m[0]) == true)///Error
            {

                // popup.AllowUsername = false;
                Debug.Log("<color=red>Error: Webviwe cant Closed: </color>" + m[1].ToString());
                ///Handler_OnChangeUsername(m[1].ToString());

            }
            else
            {


                navigationUi.ClosePopUp("webviwe");

                Debug.Log("Web Viwe Closed" + m[1].ToString());
            }

        });
        socket.On("notifications", (s, p, m) =>
        {

            if (Convert.ToBoolean(m[0]) == true)///Error
            {

                // popup.AllowUsername = false;
                Debug.Log("<color=red>Error: Notif cant Loaded: </color>" + m[1].ToString());
                ///Handler_OnChangeUsername(m[1].ToString());

            }
            else
            {
                var notif = JsonUtility.FromJson<Diaco.Notification.Notification_Dialog_Body>(m[1].ToString());
                Debug.Log("Notifi" + m[1].ToString());
                Handler_OnNotification(notif);

            }

        });
        socket.On("push-notifications", (s, p, m) =>
        {

            if (Convert.ToBoolean(m[0]) == true)///Error
            {

                // popup.AllowUsername = false;
                Debug.Log("<color=red>Error: Notif cant Loaded: </color>" + m[1].ToString());
                ///Handler_OnChangeUsername(m[1].ToString());

            }
            else
            {
                //FindObjectOfType<Diaco.Notification.PushNotification>().InstantiateEvent();
                var data = JsonUtility.FromJson<Diaco.Notification.PushNotifcationsData>(m[1].ToString());
                
                Handler_OnPushNotification(data);
                Debug.Log("Notifi" + m[1].ToString());
            }

        });
        socket.On("push-notification-cancel", (s, p, m) =>
        {

            if (Convert.ToBoolean(m[0]) == true)///Error
            {

                // popup.AllowUsername = false;
                Debug.Log("<color=red>Error: Notif cant Loaded: </color>" + m[1].ToString());
                ///Handler_OnChangeUsername(m[1].ToString());

            }
            else
            {


                Handler_OnPushNotificationCancel(Convert.ToInt32(m[1]));
                Debug.Log("CancelNotification" + m[1].ToString());
            }

        });
        socket.On("recordmode_result", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {

                // popup.AllowUsername = false;
                Debug.Log("<color=red>Error: Cant Load RecordModePopup </color>" + m[1].ToString());
                ///Handler_OnChangeUsername(m[1].ToString());

            }
            else
            {
                navigationUi.ShowPopUp("awardrecordmode");
                var data = JsonUtility.FromJson<Diaco.UI.PopupRecordModeResult.ResualtRecordModeData>(m[1].ToString());
                var popup = FindObjectOfType<Diaco.UI.PopupRecordModeResult.PopupAwardRecordMode>();
                popup.Set(data);
                Debug.Log("<color=green Load RecordModePopup : </color>");

            }
        });
        socket.On("league_result", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {

                // popup.AllowUsername = false;
                Debug.Log("<color=red>Error: Cant Load Award League Popup </color>" + m[1].ToString());
                ///Handler_OnChangeUsername(m[1].ToString());

            }
            else
            {
                navigationUi.ShowPopUp("awardleague");
                var data = JsonUtility.FromJson<Diaco.UI.PopupAwardLeague.AwardData>(m[1].ToString());
                var popup = FindObjectOfType<Diaco.UI.PopupAwardLeague.PopUpAwardLeague>();
                popup.Set(data);
                Debug.Log("<color=green Load Award League Popup : </color>");

            }
        });
        socket.On("close-popup", (s, p, m) =>
        {

            navigationUi.CloseAllPopUp();
            Debug.Log("CloseAllPopup");
            
        });
        socket.On("after_DC_result", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {

                // popup.AllowUsername = false;
                Debug.Log("<color=red>Error: Cant Load Award League Popup </color>" + m[1].ToString());
                ///Handler_OnChangeUsername(m[1].ToString());

            }
            else
            {
                navigationUi.ShowPopUp("resultDC");
                var data = JsonUtility.FromJson<Diaco.UI.PopupResualtAfterDC.ResultAfterDCData>(m[1].ToString());
                var popup = FindObjectOfType< Diaco.UI.PopupResualtAfterDC.ResultAfterDC>();
                popup.Set(data);
                Debug.Log("<color=green Load Award League Popup : </color>");

            }
        });

        socket.On("getSticker", (s, p, m) => {

            var data = JsonUtility.FromJson<StickerData>(m[0].ToString());
            Handler_GetStickers(data);
            Debug.Log("StickerRecived");
        });
        socket.On("open-hint", (s, p, m) => {
            navigationUi.ShowPopUpOnPopup("hint");

            var pop = FindObjectOfType<PopupHint>();
            pop.Init_Hint(Convert.ToInt32(m[0].ToString()), m[1].ToString(), m[2].ToString(), m[3].ToString());
            //pop.SetHint(m[0].ToString());
            Debug.Log("Hint Opned");
        });

        socket.On("static-shop-price", (s, p, m) => {
           
            
            var static_shop = FindObjectOfType<StaticShop>();
            var data = JsonUtility.FromJson<StaticShop.ItemPriceData>(m[0].ToString());
            static_shop.Set(data);
            Debug.Log("Static Shop Updated");
        });

        socket.On("disconnect", (s, p, m) =>
        {

            Ping = 0.0f;
            CancelInvoke("Emit_Ping");

            Notification_Dialog.InternetPingDialog(new Diaco.Notification.Notification_Dialog_Body { alartType = 3, context = "اتصال اینترنت خود را بررسی کنید." }, true);

            Debug.Log("disConnection");
        });

    }
    private void ServerUI_OnChangePage(string pagename)
    {
        if (pagename == "modesoccer")
        {

            //UIInFooterAndHeader.Xp_inPageSelectGame.text = BODY.profile.soccer_level.ToString() ;
            ///BODY.profile.soccer
            ///soccer_level , soccer_currentxp  ,soccer_totalxp

            UIInFooterAndHeader.SetXpPrograssBar(BODY.profile.soccer_level, BODY.profile.soccer_currentxp, BODY.profile.soccer_totalxp);
        }
        else if (pagename == "modepool")
        {
            //  UIInFooterAndHeader.Xp_inPageSelectGame.text = BODY.profile.billiard_level.ToString();
            UIInFooterAndHeader.SetXpPrograssBar(BODY.profile.billiard_level, BODY.profile.billiard_currentxp, BODY.profile.billiard_totalxp);
        }
     
        else
        {
            UIInFooterAndHeader.ResetXpPrograssbar();
            // UIInFooterAndHeader.Xp_inPageSelectGame.text = "";
        }
    }

    #endregion
    #region EmitServer

    /// <summary>
    /// Run in ON Connet invoke
    /// </summary>
    public void Emit_Ping()
    {
        Ping = Time.realtimeSinceStartup;
        socket.Emit("ping2");
        //Debug.Log("Ping");
    }
    public void  Emit_DialogAndNotification(string eventName ="shop")
    {
        socket.Emit(eventName);
    }
    public void Emit_DialogAndNotification(string eventName , string data)
    {
        socket.Emit(eventName,data);
    }

    public void SendRequestForEditPhone(string phonenumber)
    {
        socket.Emit("changePhone", phonenumber);
    }
    public void SendPhonAndConfrimCode(string phonenumber, string confrimcode)
    {
        socket.Emit("changePhone", phonenumber, confrimcode);
    }
    
    public void SendRequestGetFriends()
    {
        socket.Emit("get-friends");
        navigationUi.StartLoadingPageShow();
    }

    public void SendRequestGetAllChat(string userName)
    {

        socket.Emit("chat", userName);
        navigationUi.StartLoadingPageShow();
        Debug.Log("GetChat..." + userName);
    }
    public void SendChatToUser(string username, string Message,bool issticker)
    {
        socket.Emit("chat", username, Message,issticker);
        navigationUi.StartLoadingPageShow();
        Debug.Log("SendChat...");
    }
    public void SendReadChat(string userid)
    {
        socket.Emit("read-chat", userid);
        Debug.Log("Im a reading chat");
    }
    public void SendChatToTeam(string message,bool issticker)
    {
        socket.Emit("team-chat", message,issticker);
        navigationUi.StartLoadingPageShow();
        Debug.Log("ChatSendToTeam");
    }
    public void SendRequestOpenChatBox(string userid)
    {
        socket.Emit("open-chatbox", userid);
        navigationUi.StartLoadingPageShow();
        Debug.Log("SendOpenChatBox:" + userid);
    }
    [Obsolete]
    public void SendCurrentPage(string Page, string Targetuser)
    {
        var info = new InfoPage();

        info.targetUser = Targetuser;
        var info_j = JsonUtility.ToJson(info);
        socket.Emit("current-page", info_j);
        ///  navigationUi.StartLoadingPageShow();
        Debug.Log("SendedCurrentPage");
    }
    public void GetLeague()
    {
        socket.Emit("get-league");
        navigationUi.StartLoadingPageShow();
        Debug.Log("Send Get league Request");
    }
    public void SearchLeague(string tagId)
    {
        socket.Emit("get-league", tagId);
        navigationUi.StartLoadingPageShow();
        Debug.Log("SearchLeague :" + tagId);
    }
    public void GetLeagueInfo(string idteam)
    {
        socket.Emit("league-info", idteam);
        navigationUi.StartLoadingPageShow();
        Debug.Log("Get League Info...");
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mode">0 = "Private" , 1 = "Public"</param>
    public void RequestChangeTeamMode(int mode)
    {
        socket.Emit("change-team-mode", mode);
        Debug.Log("Team Mode Change");
    }
    public void SendEditedProfile(ProfileEdited profile)
    {
        var data = JsonUtility.ToJson(profile);
        socket.Emit("information", data);
        navigationUi.StartLoadingPageShow();
        Debug.Log("Change Profile");
    }
    public void GetProfilePerson(string userid)
    {
        socket.Emit("get-profile", userid);
        navigationUi.StartLoadingPageShow();
        Debug.Log("SendRequestShow Profile:" + userid);

    }
    public void GetTopPlayers(string sortBy)
    {
        socket.Emit("top-players", sortBy);
        navigationUi.StartLoadingPageShow();
        Debug.Log("SendRequest TopPlayer By :" + sortBy);
    }
    public void RequestBlockUser(string username)
    {
        socket.Emit("block-user", username);
        navigationUi.StartLoadingPageShow();
        Debug.Log("Blocked:" + username);
    }
    public void SendRequestMatchTimeRemaining()
    {
        socket.Emit("get-league-time");
        navigationUi.StartLoadingPageShow();
    }
    public void JoinToTeam(string teamid)
    {
        socket.Emit("join-league", teamid);
        navigationUi.StartLoadingPageShow();
        Debug.Log("Join To League"  + teamid);
    }
    public void LeaveTheTeam()
    {
        socket.Emit("leave-league");
        navigationUi.StartLoadingPageShow();
        Debug.Log("leaveTheleague");
    }
    public void Emit_DescriptionEdit()
    {
        socket.Emit("edit_description_league");
    }
    public void GetAwardsLeague(string teamid)
    {
        socket.Emit("awards", teamid);
        navigationUi.StartLoadingPageShow();
        Debug.Log("GET AWARD League");
    }
    //USingInSearchButtonIn UI Friend
    public void SearchFriendRequest(InputField input)
    {
        socket.Emit("search-friend", input.text);

        navigationUi.StartLoadingPageShow();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="lobby">0 = soccer , 1 = billiard</param>
    /// <param name="subgame">0 = classic , 1 = quick , 2 = challenge</param>
    public void RequestFindOpponent(short lobby, short subgame)
    {
        socket.Emit("join-lobby", lobby, subgame);
        Debug.Log(string.Format("RequestFindOpponent{0}:::{1}", lobby, subgame));
    }
    public void RequestLeaveLobby()
    {
        socket.Emit("leave-lobby");
        Debug.Log("RequestLeaveLobby");
    }
    public void GetMessages()
    {
        socket.Emit("messages");
        navigationUi.StartLoadingPageShow();
        Debug.Log("GETMESSAGE...");
    }
    public void AcceptRequest(string type, string username)
    {
        socket.Emit("accept-request", type, username);
        navigationUi.StartLoadingPageShow();
        Debug.Log("Accept:" + type + ":" + username);
    }
    public void RejectRequest(string type, string username)
    {
        socket.Emit("reject-request", type, username);
        navigationUi.StartLoadingPageShow();
        Debug.Log("Reject:" + type + ":" + username);
    }
    public void RequsetAddFriend(string input)
    {
        socket.Emit("add-friend", input);
        navigationUi.StartLoadingPageShow();
        Debug.Log("FriendAdded : " + input);
    }
    public void RequestCreateTeam(Diaco.HTTPBody.CreateTeam team)
    {
        var json = JsonUtility.ToJson(team);
        socket.Emit("create-league", json);
        navigationUi.StartLoadingPageShow();
        Debug.Log("TeamCreated");
    }
    public void RequestCreateTeam_NULLfeid()
    {
        
        socket.Emit("create-league-error");
        
        Debug.Log("Field Null");
    }
    public void RequestLeagueRules()
    {
        socket.Emit("league-rules");
        navigationUi.StartLoadingPageShow();
        Debug.Log("LeagueRulesRequested");
    }
    public void RequestItemShop()
    {
        socket.Emit("shop");
        navigationUi.StartLoadingPageShow();
        Debug.Log("Shop Requested");
    }
    public void RequestEditAvatar(string name)
    {
        socket.Emit("edit-avatar",name);
        navigationUi.StartLoadingPageShow();
        Debug.Log("Edit Avatars Requested");
    }
    public void RequestEditDescription(string des)
    {
        socket.Emit("edit-description", des);
        navigationUi.StartLoadingPageShow();
        Debug.Log("Request Edit Description ");
    }
    public void RequestWithdraw(Diaco.UI.WithDrawGem.WithdrawData data)
    {
        var d = JsonUtility.ToJson(data);
        socket.Emit("withdraw", d);
        navigationUi.StartLoadingPageShow();
        Debug.Log("Request withdraw ");
    }
    public void RequestEditUserName(string username)
    {
        socket.Emit("edit-username", username);
      //  navigationUi.StartLoadingPageShow();
        Debug.Log("CheckUserName " + username); 
    }
    #region Emits_Shop
    public void Emit_Transaction(string tranlog)
    {
        socket.Emit("transaction-bazzar", tranlog);
        navigationUi.StartLoadingPageShow();
        Debug.Log($"EmitTransaction:{tranlog}");
    }



    public void Emit_Shop(string id)
    {
        socket.Emit("shopBuy", id);
        navigationUi.StartLoadingPageShow();
        Debug.Log("Emit_Shop");
    }
    public void Emit_SoccerShopTeam()
    {
        socket.Emit("shop-soccer-marble");
        navigationUi.StartLoadingPageShow();
        Debug.Log("Emit_ShopTeam");
    }
    public void Emit_UseTeam(string id)
    {
        socket.Emit("useTeam", id);
        Debug.Log("USE Team:" + id);
    }
    public void Emit_RentTeam(string rentId)
    {
        socket.Emit("rentTeam", rentId);
        Debug.Log("Emit_ShopTeamRent=" + "::" + rentId);
    }


    public void Emit_SoccerShopformation()
    {
        socket.Emit("shop-soccer-plan");
        navigationUi.StartLoadingPageShow();
        Debug.Log("Emit_Shopformation");
    }
    public void Emit_UseFormation(string id)
    {
        socket.Emit("useFormation", id);
        Debug.Log("USE Formation:" + id);
    }
    public void Emit_RentFormation(string rentId)
    {
        socket.Emit("rentFormation", rentId);
        Debug.Log("Emit_ShopformationRent=" + "::" + rentId);
    }


    public void Emit_BilliardShop()
    {
        socket.Emit("shop-billiard");
        navigationUi.StartLoadingPageShow();
        Debug.Log("billiardShop");
    }
    public void Emit_UseCue(string id)
    {
        socket.Emit("useCue", id);
        Debug.Log("Use Cue :" + id);
    }
    public void Emit_RentCue(string rentId)
    {
        socket.Emit("rentCue", rentId);
        Debug.Log("Emit_ShopformationRent=" + "::" + rentId);
    }
    public void Emit_Setting()
    {

        var obj_setting = FindObjectOfType<Diaco.Setting.GeneralSetting>();
        Diaco.Setting.GameSettingDataServer data = new Diaco.Setting.GameSettingDataServer
        {
            status = obj_setting.Setting.status,
            reciveFriendRequest = obj_setting.Setting.reciveFriendRequest,
            reciveLeagueRequest = obj_setting.Setting.reciveLeagueRequest,
            reciveMatchFriendRequest = obj_setting.Setting.reciveMatchFriendRequest

        };
        
        var json_setting = JsonUtility.ToJson(data);
        socket.Emit("setting", json_setting);
        Debug.Log("Setting  Send To Server");

    }
    public  void Emit_WithdrawAwardLeague()
    {
        socket.Emit("league-withdraw");
        Debug.Log("WithdrawAwardLeague");
    }
    public void Emit_WithdrawAwardNetwork()
    {
        socket.Emit("network-withdraw");
        Debug.Log("WithdrawAwardNetwork");
    }
    public void Emit_AchivementDescription(string achivename)
    {
        socket.Emit("achievement-dialog",achivename);
        Debug.Log("Achivment Description"+achivename);
    }
    public void Emit_Ticket(string id)
    {
        socket.Emit("use-ticket", id);
        Debug.Log("Use Ticket :" + id);
    }
    public void Emit_Card(string id)
    {
        socket.Emit("card-dialog", id);
        Debug.Log("Card Dialog :" + id);
    }
    public void Emit_ShopT2Button(string name, int price )
    {
        socket.Emit("shop-t2", name, price);
        Debug.Log("Price Of Prodoct :" + name +":"+ price);
     
    }
    public void Emit_StaticShopUpdate()
    {
        socket.Emit("static-shop-price");
        Debug.Log("StaticShop Requested");
      
    }
    public void Emit_CloseDialog(string closeEvent)
    {
        socket.Emit("close-dialog", closeEvent);
        Debug.Log("CloseDialog");
    }
    #endregion
    /// <summary>
    /// 
    /// </summary>
    /// <param name="Friend">Friend Name = "Omid" Or "Farshad"</param>
    /// <param name="Game">Soccer Or Biliiard</param>
    /// <param name="TypeGame"> Classic ,  Big Match,  ... </param>
    public void RequestPlayGameWithFriend(string Friend, short Game, short TypeGame)
    {
        socket.Emit("playWithFriend", Friend, Game, TypeGame);
        Debug.LogFormat("RequestPlayGameWithFriend:{0}:{1}:{2}", Friend, Game, TypeGame);
    }
    public void AcceptRequsetPlayGameWithFriend(string friend, short game, short subGame)
    {
        socket.Emit("acceptPlayWithFriend", friend, game, subGame);
        Debug.Log("acceptPlayWithFriend:::" + friend + ":::::" + game + ":::::" + subGame);
    }
    public void RequestGetMatch(string Type)
    {
        socket.Emit("competitions", (int)navigationUi.GameLobby, Type);
        navigationUi.StartLoadingPageShow();
        Debug.Log("competitions Requested");
    }
    public void RequestCompetitionInfo(string id)
    {
        socket.Emit("competition-info", id);
        navigationUi.StartLoadingPageShow();
        Debug.Log(" competition-info Requested");
    }


    /// <summary>
    /// 1  = join ,0  = leave , 2  = award
    /// </summary>
    /// <param name="commandindex"> 0  = leave 1  = join 2  = award 3 = cancle</param>
    public void RequestCompetitionCommand(string id, short commandindex)//
    {
        socket.Emit("competition-command", id, commandindex);
        navigationUi.StartLoadingPageShow();
        Debug.Log("CompetitionCommand::" + commandindex);
    }
    public void RequestLeaveCompetition(string id)//
    {
        socket.Emit("competition-leave", id);
        navigationUi.StartLoadingPageShow();
        Debug.Log("Request Competition Leave" +id);
    }
    public void RequestGoToTableRanking()
    {
        socket.Emit("gotoranking");
        navigationUi.StartLoadingPageShow();
        Debug.Log("Timer Zero Ranking Table Requested");
    }

    public void RequestGetRecordMode()
    {
        socket.Emit("get-record", (int)navigationUi.GameLobby);
        navigationUi.StartLoadingPageShow();
        Debug.Log("Send Request DataForRecorodMode");
    }
    public void RequestJoinRecordMode()
    {
        socket.Emit("join-record", (int)navigationUi.GameLobby);
        Debug.Log("JoinRecord");
    }
    public void RequestReports()
    {
        socket.Emit("reports");
        navigationUi.StartLoadingPageShow();
        Debug.Log("RequestReports");

    }
    public void RequestShowTutorialInWebview()
    {

        socket.Emit("tutorial");
        navigationUi.StartLoadingPageShow();
        Debug.Log("Send Request For Show Tutorial");
    }
    public void RequestAds()
    {

        socket.Emit("ads");
        navigationUi.StartLoadingPageShow();
        Debug.Log("Send Request For Show ads");
    }
    public void Emit_GetSticker()
    {
        socket.Emit("getSticker");
        Debug.Log("Emit_getSticker");
    }
    public void  Emit_CurrncyButton(string c)
    {
        socket.Emit("currency-dialog", c);
        Debug.Log("currency-dialog:"+c);
    }
    #endregion
    #region Function

    public void SetElementInHeaderAndFooter()
    {
        // ui.ImageUser_inPageSelectGame.sprite = ConvertImageToSprite(BODY.profile.avatar);
        UIInFooterAndHeader.ImageUser_inPageSelectGame.sprite = AvatarContainer.LoadImage(BODY.profile.avatar);
        UIInFooterAndHeader.UserName_inPageSelectGame.text = BODY.userName;
        UIInFooterAndHeader.Coin_inPageSelectGame.text = BODY.profile.coin.ToString();
        UIInFooterAndHeader.Cupbilliard_inPageSelectGame.text = BODY.profile.billiard_cup.ToString();
        UIInFooterAndHeader.Cupsoccer_inPageSelectGame.text = BODY.profile.soccer_cup.ToString();
        UIInFooterAndHeader.Gem_inPageSelectGame.text = BODY.profile.gem;
    }

    public void CloseConnectionUIToServer()
    {

        socket.Off();
        socket.Manager.Close();
        socket.Disconnect();
        navigationUi.OnChangePage -= ServerUI_OnChangePage;
        CancelInvoke("Emit_Ping");
        Notification_Dialog.InternetPingDialog(new Diaco.Notification.Notification_Dialog_Body { alartType = 3, context = "" }, false);
        Debug.Log("MainMenuCloseConnection");

    }
    public bool ExistTokenFile(string FileName)
    {
        bool find = false;
        if (File.Exists(Application.persistentDataPath + "//" + FileName + ".json"))
        {
            find = true;
        }
        return find;
    }
    public string ReadToken(string FileName)
    {
        TOKEN token = new TOKEN();
        if (File.Exists(Application.persistentDataPath + "//" + FileName + ".json"))
        {
            var j_token = File.ReadAllText(Application.persistentDataPath + "//" + FileName + ".json");
            token = JsonUtility.FromJson<Diaco.HTTPBody.TOKEN>(j_token);

        }
        return token.token;
    }
    public void  DeleteToken(string FileName)
    {
        
        if (File.Exists(Application.persistentDataPath + "//" + FileName + ".json"))
        {
            File.Delete(Application.persistentDataPath + "//" + FileName + ".json");
        }
        
    }
    public Sprite ConvertImageToSprite(string image)
    {

        var image_byte = Convert.FromBase64String(image);
        Texture2D texture = new Texture2D(512, 512, TextureFormat.RGBA32, false);
        texture.LoadImage(image_byte);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
    #endregion
    #region Events
    private Action createteam;
    public event Action OnCreateTeamCompeleted
    {
        add { createteam+=value; }
        remove { createteam -= value; }
    }
    protected void Handler_OnCreateTeamCompeleted()
    {
        if (createteam != null)
        {
            createteam();
        }
    }
    private Action gamebody;
    public event Action OnGameBodyUpdate
    {
        add { gamebody += value; }
        remove { gamebody-= value; }
    }
    protected void Handler_OnGameBodyUpdate()
    {
        if (gamebody != null)
        {
            gamebody();
        }
    }
    private Action<string> errorcreateteam;
    public event Action<string> OnErrorCreateTeam
    {
        add { errorcreateteam += value; }
        remove { errorcreateteam -= value; }
    }
    protected void Handler_ErrorCreateTeam(string error)
    {
        if (errorcreateteam != null)
        {
            errorcreateteam(error);
        }
    }
    public event Action<Friends> OnComingFriends;
    protected void Handler_oncomingfriends(Friends friends)
    {
        if (OnComingFriends != null)
        {
            OnComingFriends(friends);
        }
    }

    public event Action<SearchUser> OnResualtSearchFriend;
    protected void Handler_onResualtSearchFriend(SearchUser friends)
    {
        if (OnComingFriends != null)
        {
            OnResualtSearchFriend(friends);
        }
    }
    private Action<Diaco.HTTPBody.Chats> chatsrecive;
    public event Action<Diaco.HTTPBody.Chats> OnChatsRecive
    {
        add { chatsrecive += value; }
        remove { chatsrecive -= value; }
    }
    protected void Handler_onchatreciver(Chats chats)
    {
        if (chatsrecive != null)
        {
            chatsrecive(chats);
        }
    }
    private Action<InRequsets> getmessage;
    public event Action<InRequsets> OnGetMessages
    {
        add { getmessage += value; }
        remove { getmessage -= value; }
    }
    protected void Handler_OnGetMessages(InRequsets inRequsets)
    {
        if (getmessage != null)
        {
            getmessage(inRequsets);
        }
    }
    private Action<Teams> getteam;
    public event Action<Teams> OnGetTeams
    {
        add { getteam += value; }
        remove { getteam -= value; }
    }
    protected void Handler_OnGetTeams(Teams teams)
    {
        if (getteam != null)
        {
            getteam(teams);
        }
    }
    private Action<TeamInfo> getteaminfo;
    public event Action<TeamInfo> OnGetTeamInfo
    {
        add { getteaminfo += value; }
        remove { getteaminfo -= value; }
    }
    protected void Handler_OnGetTeamInfo(TeamInfo teamInfos)
    {
        if (getteaminfo  != null)
        {
            getteaminfo(teamInfos);
        }
    }
    private Action chatteamupdate;
    public event Action OnUpdateChatTeam
    {
        add { chatteamupdate += value; }
        remove { chatteamupdate -= value; }
    }

    protected void handler_OnUpdateChatTeam()
    {
        if (chatteamupdate != null)
        {
            chatteamupdate();
        }
    }

    /* public event Action<AwardsName> OnGetAward;
     protected void Handler_OnGetAward(AwardsName awards)
     {
         if (OnGetAward != null)
         {
             OnGetAward(awards);
         }
     }*/
    private Action<float> ongetime;
    public event Action<float> OnGetTimeTeam
    {
        add { ongetime += value; }
        remove { ongetime -= value; }
    }
    protected void Handler_OnGetTimeTeam(float time)
    {
        if (ongetime != null)
        {

            ongetime(time);
        }
    }
   /* private Action<ProfileOtherPerson> getprofileperson;
    public event Action<ProfileOtherPerson> OnGetProfileOtherPerson
    {
        add { getprofileperson += value; }
        remove { getprofileperson -= value; }
    }
    protected void Handler_OnGetProfileOtherPerson(ProfileOtherPerson profile)
    {
        if (getprofileperson != null)
        {
            getprofileperson(profile);
        }
    }*/

    public event Action<TopPlayers> OnGetTopPlayers;
    protected void Handler_OnGetTopPlayers(TopPlayers players)
    {
        if (OnGetTopPlayers != null)
        {
            OnGetTopPlayers(players);
        }
    }







    public event Action<Shop> OnShopLoaded;
    protected void Handler_OnShopLoaded(Shop shop)
    {
        if (OnShopLoaded != null)
        {
            OnShopLoaded(shop);
        }
    }


    private Action<Diaco.Store.Billiard.BilliardShopDatas> initshopBilliard;
    public event Action<Diaco.Store.Billiard.BilliardShopDatas> InitshopBilliard
    {
        add
        {
            initshopBilliard += value;
        }
        remove
        {
            initshopBilliard -= value;
        }
    }
    protected void Handler_InitshopBilliard(Diaco.Store.Billiard.BilliardShopDatas data)
    {
        if (initshopBilliard != null)
        {
            initshopBilliard(data);
        }

    }



    private Action<Diaco.Store.Soccer.SoccerShopDatas> initsoccerformationshop;
    public event Action<Diaco.Store.Soccer.SoccerShopDatas> InitSoccerFormationShop
    {
        add
        {
            initsoccerformationshop += value;
        }
        remove
        {
            initsoccerformationshop -= value;
        }
    }
    protected void Handler_InitSoccerFormationShop(Diaco.Store.Soccer.SoccerShopDatas data)
    {
        if (initsoccerformationshop != null)
        {
            initsoccerformationshop(data);
        }

    }



    private Action<Diaco.Store.Soccer.SoccerShopDatas> initsoccerteamshop;
    public event Action<Diaco.Store.Soccer.SoccerShopDatas> InitSoccerTeamShop
    {
        add
        {
            initsoccerteamshop += value;
        }
        remove
        {
            initsoccerteamshop -= value;
        }
    }
    protected void Handler_InitSoccerTeamShop(Diaco.Store.Soccer.SoccerShopDatas data)
    {
        if (initsoccerteamshop != null)
        {
            initsoccerteamshop(data);
        }

    }


    private Action<string, string> onopponetfind;
    public event Action<string, string> OnOpponentFind
    {
        add
        {
            onopponetfind += value;
        }
        remove
        {
            onopponetfind -= value;
        }
    }
    protected void Handler_OnOpponentFind(string username, string avatar)
    {
        if (onopponetfind != null)
        {

            onopponetfind(username, avatar);
        }
    }

    private Action<Diaco.UI.RoyalTournument.CompetitionsData> recivematchs;
    public event Action<Diaco.UI.RoyalTournument.CompetitionsData> OnReciveMatchs
    {
        add
        {
            recivematchs += value;

        }
        remove
        {
            recivematchs -= value;
        }
    }
    protected void Handler_ReciveMatchs(Diaco.UI.RoyalTournument.CompetitionsData data)
    {
        if (recivematchs != null)
        {
            recivematchs(data);
        }
    }

    private Action<Diaco.UI.RoyalTournument.CompetitionInfoData> competitioninfo;
    public event Action<Diaco.UI.RoyalTournument.CompetitionInfoData> OnCompetitionInfo
    {
        add
        {
            competitioninfo += value;
        }
        remove
        {
            competitioninfo -= value;
        }
    }
    protected void Handler_OnCompettionInfo(Diaco.UI.RoyalTournument.CompetitionInfoData data)
    {
        if (competitioninfo != null)
        {
            competitioninfo(data);

        }
    }

    private Action<Diaco.UI.RoyalTournument.AwardsData> competitionaward;
    public event Action<Diaco.UI.RoyalTournument.AwardsData> OnCompetitionAward
    {
        add
        {
            competitionaward += value;
        }
        remove
        {
            competitionaward -= value;
        }
    }
    protected void Handler_OnCompetitionAward(Diaco.UI.RoyalTournument.AwardsData data)
    {
        if (competitionaward != null)
        {
            competitionaward(data);

        }
    }

    private Action<Diaco.UI.PopupTournumentRanking.TournomentData> onstarttournument;
    public event Action<Diaco.UI.PopupTournumentRanking.TournomentData> OnStartTournument
    {
        add
        {
            onstarttournument += value;

        }
        remove
        {
            onstarttournument -= value;
        }
    }
    protected void Handler_OnStartTournument(Diaco.UI.PopupTournumentRanking.TournomentData data)
    {
        if (onstarttournument != null)
        {
            onstarttournument(data);
        }
    }


    private Action<Diaco.UI.MatchRecord.MatchRecordModeData> onmatchrecord;
    public event Action<Diaco.UI.MatchRecord.MatchRecordModeData> OnMatchRecord
    {
        add
        {
            onmatchrecord += value;

        }
        remove
        {
            onmatchrecord -= value;
        }
    }
    protected void Handler_OnMatchRecord(Diaco.UI.MatchRecord.MatchRecordModeData data)
    {
        if (onmatchrecord != null)
        {
            onmatchrecord(data);
        }
    }

    private Action <string> onchangeusername;
    public event Action<string> OnchangeUsername
    {
        add
        {
            onchangeusername += value;

        }
        remove
        {
            onchangeusername-= value;
        }
    }
    protected void Handler_OnChangeUsername(string user)
    {
        if (onchangeusername!= null)
        {
            onchangeusername(user);
        }
    }
    private Action<Diaco.Notification.Notification_Dialog_Body> onnotification;
    public event Action<Diaco.Notification.Notification_Dialog_Body> OnNotification
    {
        add
        {
            onnotification += value;

        }
        remove
        {
            onnotification -= value;
        }
    }
    protected void Handler_OnNotification(Diaco.Notification.Notification_Dialog_Body body)
    {
        if (onnotification != null)
        {
            onnotification(body);
        }
    }



    private Action<Diaco.Notification.PushNotifcationsData> onpushnotification;
    public event Action<Diaco.Notification.PushNotifcationsData> OnPushNotification
    {
        add
        {
            onpushnotification += value;

        }
        remove
        {
            onpushnotification -= value;
        }
    }
    protected void Handler_OnPushNotification(Diaco.Notification.PushNotifcationsData body)
    {
        if (onpushnotification != null)
        {
            onpushnotification(body);
        }
    }





    private Action<int> onpushnotificationcancle;
    public event Action<int> OnPushNotificationCancle
    {
        add
        {
            onpushnotificationcancle += value;

        }
        remove
        {
            onpushnotificationcancle -= value;
        }
    }
    protected void Handler_OnPushNotificationCancel(int id)
    {
        if (onpushnotificationcancle != null)
        {
            onpushnotificationcancle(id);
        }
    }

    private Action<StickerData> getsticker;
    public event Action<StickerData> GetStickers
    {
        add
        {
            getsticker += value;
        }
        remove
        {
            getsticker -= value;
        }
    }
    protected void Handler_GetStickers(StickerData data)
    {
        if (getsticker != null)
        {
            getsticker(data);
        }

    }
    #endregion

}
