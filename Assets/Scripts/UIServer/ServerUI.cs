using System;

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
    public string UIServerURL = "http://192.168.1.109:8420/socket.io/";

    public Diaco.ImageContainerTool.ImageContainer AvatarContainer, BadgesContainer, ImageGameType, ImageTypeCosts;
    public GameObject MainMenu, Footer, Header, Login, Register, SplashScreen;
    [SerializeField]
    public BODY BODY;
    public Shop Shop;
    public SoccerShopProducts SoccerShopProducts;
    public BilliardShopProducts BilliardShopProducts;
    public Socket socket;
    public SocketManager socketmanager;
    public GameLuncher Luncher;
    public Diaco.Notification.NotificationPopUp NotificationPopUp;
    public NavigationUI navigationUi;
    public UIRegisterOnServer UIInFooterAndHeader;

    private bool loadedpage = false;
    private int  intergation = 0;
    public void ConnectToUIServer()
    {
        Luncher = FindObjectOfType<GameLuncher>();
        SocketOptions options = new SocketOptions();
        options.AutoConnect = true;
        socketmanager = new SocketManager(new Uri(UIServerURL), options);
        socket = socketmanager.Socket;

        socket.On("connect", (s, p, m) =>
        {

            intergation = 0;
            socket.Emit("authToken", ReadToken("token"));
            navigationUi.StopLoadingPage();

            Debug.Log($"<color=blue><b>Connection And ReadToken</b></color>");
        });
        socket.On("wrong-token", (s, p, m) =>
        {
            Register.SetActive(true);
            Debug.Log($"<color=red><b>Wrong Token</b></color>");
        });
        socket.On("main-menu", (s, p, m) =>
        {


            BODY = new BODY();
            var byte_data = p.Attachments[0];
            var json = System.Text.UTF8Encoding.UTF8.GetString(byte_data);
            BODY = JsonUtility.FromJson<BODY>(json);

            if (BODY.inGame.id != "" && intergation == 0)
            {
                Debug.Log("InGame");
                if (BODY.inGame.gameType == "soccer")
                {
                    //navigationUi.GetComponent<SceneManagers>().loadlevel("SoccerGame");

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

                    if (BODY.inGame.namespaceServer == "_record")
                    {
                        Luncher.SetNameSpaceServer(3, BODY.inGame.namespaceServer);
                        Luncher.SwitchScene(3);
                        Debug.Log("InGame!!!!!");
                    }
                    else
                    {
                        Luncher.SetNameSpaceServer(1, BODY.inGame.namespaceServer);
                        Luncher.SwitchScene(1);
                        Debug.Log("InGame@@@@@");
                    }
                }
                intergation = 1;
                return;
            }


            UIInFooterAndHeader.initTournmentCard(BODY.profile.tournaments);
            SetElementInHeaderAndFooter();
            Handler_OnCreateTeamCompeleted();

            if (loadedpage == false)
            {
                Login.SetActive(false);
                Register.SetActive(false);
                SplashScreen.SetActive(false);

                MainMenu.SetActive(true);
                Footer.SetActive(true);
                Header.SetActive(true);
                loadedpage = true;
            }
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
            }
            navigationUi.StopLoadingPage();
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
        socket.On("playWithFriend", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error:" + m[1].ToString());
            }
            else
            {
                var playwithfriend = JsonUtility.FromJson<PlayWithFriend>(m[1].ToString());

                NotificationPopUp.NotificationInviteMatchShow(
               (_GameLobby)playwithfriend.game,
               (_SubGame)playwithfriend.subgame,
               PersianFix.Persian.Fix("شمارا دعوت کرده" + (_SubGame)playwithfriend.game, 255),
               playwithfriend.friend);

                Debug.Log(playwithfriend.friend + "r/n/" + playwithfriend.game + "/r/n" + playwithfriend.subgame);
            }
        });
        socket.On("get-team-time", (s, p, m) =>
         {
             Handler_OnGetTimeTeam(Convert.ToSingle(m[1]));
             navigationUi.StopLoadingPage();
             Debug.Log("GetTime" + Convert.ToSingle(m[1]));
         });
        socket.On("get-teams", (s, p, m) =>
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
                Debug.Log("TeamsLoaded");
            }
            navigationUi.StopLoadingPage();
        });
        socket.On("team-info", (s, p, m) =>
        {

            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error:" + m[1].ToString());

            }
            else
            {

                var teams_info = JsonUtility.FromJson<TeamInfo>(m[1].ToString());

                Handler_OnGetTeamInfo(teams_info);

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

                Handler_OnGetAward(awrad);

                Debug.Log("ReciveTeamAwardAndLoadedInPopUp.");
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
                Debug.Log("GetMessages");
            }
            navigationUi.StopLoadingPage();
        });
        socket.On("create-team", (s, p, m) =>
        {

            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                OnErrorCreateTeam(m[1].ToString());
                Debug.Log("Error:" + m[1].ToString());

            }
            else
            {
                var byte_data = p.Attachments[0];
                var json = System.Text.UTF8Encoding.UTF8.GetString(byte_data);

                BODY = JsonUtility.FromJson<BODY>(json);
                SetElementInHeaderAndFooter();
                Handler_OnCreateTeamCompeleted();



            }
            navigationUi.StopLoadingPage();
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
                Debug.Log("Error: Get Profile " + m[1].ToString());

            }
            else
            {
                var profile = JsonUtility.FromJson<ProfileOtherPerson>(m[1].ToString());
                Handler_OnGetProfileOtherPerson(profile);
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
                Shop = new Shop();
                Shop = JsonUtility.FromJson<Shop>(m[1].ToString());
                Handler_OnShopLoaded(Shop);
                Debug.Log("Shop Loaded" + m[1].ToString());
            }
            navigationUi.StopLoadingPage();
        });
        socket.On("shop-soccer", (s, p, m) =>
        {
            if (Convert.ToBoolean(m[0]) == true)///Error
            {
                Debug.Log("Error: ShopSoccer: " + m[1].ToString());

            }
            else
            {
                SoccerShopProducts = new SoccerShopProducts();
                SoccerShopProducts = JsonUtility.FromJson<SoccerShopProducts>(m[1].ToString());
                Handler_OnSoccerShopLoaded(SoccerShopProducts);
                Debug.Log("Shop Soccer Loaded" + m[1].ToString());
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
                BilliardShopProducts = new BilliardShopProducts();
                BilliardShopProducts = JsonUtility.FromJson<BilliardShopProducts>(m[1].ToString());
                Handler_OnBilliardShopLoaded(BilliardShopProducts);
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
        socket.On("disconnect", (s, p, m) =>
        {
            Debug.Log("disConnection");
        });

    }
    #region EmitServer
    public void SendRequestGetFriends()
    {
        socket.Emit("get-friends");
        navigationUi.StartLoadingPageShow();
    }

    public void SendRequestChatWithUser(string userName)
    {

        socket.Emit("chat", userName);
        navigationUi.StartLoadingPageShow();
        Debug.Log("GetChat..." + userName);
    }
    public void SendChatToUser(string username, string Message)
    {
        socket.Emit("chat", username, Message);
        navigationUi.StartLoadingPageShow();
        Debug.Log("SendChat...");
    }
    public void SendChatToTeam(string message)
    {
        socket.Emit("team-chat", message);
        navigationUi.StartLoadingPageShow();
        Debug.Log("ChatSendToTeam");
    }
    public void SendCurrentPage(string Page, string Targetuser)
    {
        var info = new InfoPage();

        info.targetUser = Targetuser;
        var info_j = JsonUtility.ToJson(info);
        socket.Emit("current-page", info_j);
        ///  navigationUi.StartLoadingPageShow();
        Debug.Log("SendedCurrentPage");
    }
    public void GetTeams()
    {
        socket.Emit("get-teams");
        navigationUi.StartLoadingPageShow();
        Debug.Log("SendGetTeamRequest");
    }
    public void SearchTeam(string tagId)
    {
        socket.Emit("get-teams", tagId);
        navigationUi.StartLoadingPageShow();
        Debug.Log("SearchTeam :" + tagId);
    }
    public void GetTeamInfo(string idteam)
    {
        socket.Emit("team-info", idteam);
        navigationUi.StartLoadingPageShow();
        Debug.Log("Get Team Info...");
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
    public void GetProfilePerson(string PersonUserName)
    {
        socket.Emit("get-profile", PersonUserName);
        navigationUi.StartLoadingPageShow();
        Debug.Log("GetProfile");

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
        socket.Emit("get-team-time");
        navigationUi.StartLoadingPageShow();
    }
    public void JoinToTeam(string teamid)
    {
        socket.Emit("join-team", teamid);
        navigationUi.StartLoadingPageShow();
        Debug.Log("JoinToTeam");
    }
    public void LeaveTheTeam()
    {
        socket.Emit("leave-team");
        navigationUi.StartLoadingPageShow();
        Debug.Log("leaveTheTeam");
    }
    public void GetAwardsTeam(string teamid)
    {
        socket.Emit("awards", teamid);
        navigationUi.StartLoadingPageShow();
        Debug.Log("GET AWARD");
    }
    //USingInSearchButtonIn UI Friend
    public void SearchFriendRequest(InputField input)
    {

        if (SearchCheckCharacter.Check(input.text))
        {
            socket.Emit("search-friend", input.text);
            Debug.Log(input.text);
        }
        else
        {
            Debug.Log("CharacterValidate");
        }
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
        socket.Emit("create-team", json);
        navigationUi.StartLoadingPageShow();
        Debug.Log("TeamCreated");
    }
    public void RequestItemShop()
    {
        socket.Emit("shop");
        navigationUi.StartLoadingPageShow();
        Debug.Log("Shop Requested");
    }
    public void RequestItemShopSoccer()
    {
        socket.Emit("shop-soccer");
        navigationUi.StartLoadingPageShow();
        Debug.Log("Shop Soccer Requested");
    }
    public void RequestItemShopBiliard()
    {
        socket.Emit("shop-billiard");
        navigationUi.StartLoadingPageShow();
        Debug.Log("Shop billiard Requested");
    }
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
    public void RequestGetMatch( string Type)
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
    public void RequestGoToTableRanking()
    {
        socket.Emit("gotoranking");
        navigationUi.StartLoadingPageShow();
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
        UIInFooterAndHeader.Gem_inPageSelectGame.text = BODY.profile.gem.ToString();
    }

    public void CloseConnectionUIToServer()
    {

        socket.Off();
        socket.Manager.Close();
        socket.Disconnect();
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
    public Sprite ConvertImageToSprite(string image)
    {

        var image_byte = Convert.FromBase64String(image);
        Texture2D texture = new Texture2D(512, 512, TextureFormat.DXT5, false);
        texture.LoadImage(image_byte);
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
    #endregion
    #region Events
    public event Action OnCreateTeamCompeleted;
    protected void Handler_OnCreateTeamCompeleted()
    {
        if (OnCreateTeamCompeleted != null)
        {
            OnCreateTeamCompeleted();
        }
    }

    public event Action<string> OnErrorCreateTeam;
    protected void Handler_ErrorCreateTeam(string error)
    {
        if (OnErrorCreateTeam != null)
        {
            OnErrorCreateTeam(error);
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

    public event Action<Diaco.HTTPBody.Chats> OnChatsRecive;
    protected void Handler_onchatreciver(Chats chats)
    {
        if (OnChatsRecive != null)
        {
            OnChatsRecive(chats);
        }
    }

    public event Action<InRequsets> OnGetMessages;
    protected void Handler_OnGetMessages(InRequsets inRequsets)
    {
        if (OnGetMessages != null)
        {
            OnGetMessages(inRequsets);
        }
    }

    public event Action<Teams> OnGetTeams;
    protected void Handler_OnGetTeams(Teams teams)
    {
        if (OnGetTeams != null)
        {
            OnGetTeams(teams);
        }
    }

    public event Action<TeamInfo> OnGetTeamInfo;
    protected void Handler_OnGetTeamInfo(TeamInfo teamInfos)
    {
        if (OnGetTeamInfo != null)
        {
            OnGetTeamInfo(teamInfos);
        }
    }

    public event Action OnUpdateChatTeam;
    protected void handler_OnUpdateChatTeam()
    {
        if (OnUpdateChatTeam != null)
        {
            OnUpdateChatTeam();
        }
    }

    public event Action<AwardsName> OnGetAward;
    protected void Handler_OnGetAward(AwardsName awards)
    {
        if (OnGetAward != null)
        {
            OnGetAward(awards);
        }
    }

    public event Action<float> OnGetTimeTeam;
    protected void Handler_OnGetTimeTeam(float time)
    {
        if (OnGetTimeTeam != null)
        {

            OnGetTimeTeam(time);
        }
    }

    public event Action<ProfileOtherPerson> OnGetProfileOtherPerson;
    protected void Handler_OnGetProfileOtherPerson(ProfileOtherPerson profile)
    {
        if (OnGetProfileOtherPerson != null)
        {
            OnGetProfileOtherPerson(profile);
        }
    }

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

    public event Action<BilliardShopProducts> OnBilliardShopLoaded;
    protected void Handler_OnBilliardShopLoaded(BilliardShopProducts billiardshop)
    {
        if (OnBilliardShopLoaded != null)
        {
            OnBilliardShopLoaded(billiardshop);
        }
    }

    public event Action<SoccerShopProducts> OnSoccerShopLoaded;
    protected void Handler_OnSoccerShopLoaded(SoccerShopProducts soccershop)
    {
        if (OnSoccerShopLoaded != null)
        {
            OnSoccerShopLoaded(soccershop);
        }
    }

    public event Action<string, string> OnOpponentFind;
    protected void Handler_OnOpponentFind(string username, string avatar)
    {
        if (OnOpponentFind != null)
        {

            OnOpponentFind(username, avatar);
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

    #endregion

}
