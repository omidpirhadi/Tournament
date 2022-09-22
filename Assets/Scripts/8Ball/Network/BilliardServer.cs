using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using BestHTTP.SocketIO;
using Sirenix.OdinInspector;
using DG.Tweening;
using Diaco.EightBall.Structs;
using Diaco.HTTPBody;


namespace Diaco.EightBall.Server
{
    public enum _GamePlayRule { classic = 0, quick = 1, big = 2 };
    public class BilliardServer : MonoBehaviour
    {
        public SoundEffectControll soundeffectcontroll;
        public bool InRecordMode = false;
        public Transform ParentForspwan;
        public bool SpwnedBall = false;
        public float TimeStep = 0.0f;
        public _GamePlayRule GamePlayRule;

        /*  [SerializeField] private int pocketselected;
           public int PocketSelected {
               set {
                   pocketselected = value;
                   if(pocketselected !=0)
                   {
                       Handler_EnableBoarderPocket(false,);
                   }

               }
               get { return pocketselected; }
           }*/
        // public float ThresholdSleep = 0.09f;
        #region ServerSettings
        public Socket socket;
        public SocketManager socketmanager;
        ////  [FoldoutGroup("ServerSettings")]
        /// public string GlobalURL;
        /// [FoldoutGroup("ServerSettings")]
        ///  public string LocalURL;
        [FoldoutGroup("ServerSettings")]
        public string Namespaceserver;
        [SerializeField]
        [FoldoutGroup("ServerSettings")]
        public Diaco.EightBall.Structs.UserInfo UserName;

        [SerializeField]
        [FoldoutGroup("ServerSettings")]
        public Diaco.EightBall.Structs.GameData gameData;
        public GameObject ResultGamePage;

        [FoldoutGroup("ServerSettings")]
        public float Framerate = 0.02f;
        [FoldoutGroup("ServerSettings")]
        // public bool TurnRecived = false;
        [FoldoutGroup("ServerSettings")]
        public List<AddressBall> AddressBalls;
        [FoldoutGroup("ServerSettings")]
        public List<TABLE> Tables;
        [FoldoutGroup("ServerSettings")]
        public MeshRenderer TableRenderer;
        [FoldoutGroup("ServerSettings")]
        private int intergateplayposition;
        [SerializeField]
        [FoldoutGroup("ServerSettings")]
        private float Timer { set; get; }
        [SerializeField]
        [FoldoutGroup("ServerSettings")]
        public int DeletedBallCount = 0;

        [FoldoutGroup("ServerSettingsInRecordMode")]
        public GameObject SiblPrefab;
        [FoldoutGroup("ServerSettingsInRecordMode")]
        public Sibl Sibl;
        [FoldoutGroup("UIInRecordMode")]
        public ResultInRecordMode resualtInRecordMode;
        [SerializeField]
        [FoldoutGroup("UIInRecordMode")]
        public UIElements UIControllInRecordMode;

        private float H;
        private float M;
        private float S;
        private Coroutine CoroutineSendPositionToServer;
        private Coroutine CoroutineRecivePositionFromServer;
        private List<PositionAndRotateBalls> QueuePositionsBallFromServer;
        private Queue<CueBallData> QueueCueBallPositionFromServer;
        private Queue<AimData> QueueAimFromServer;
        #endregion

        #region Property GameData
        [FoldoutGroup("GameData")]
        public List<GameObject> BallsPrefabs;
        [FoldoutGroup("GameData")]
        public Diaco.EightBall.Structs.Shar PlayerShar;
        [FoldoutGroup("GameData")]
        [SerializeField] private bool turn;
        public bool Turn
        {
            set
            {
                turn = value;

                Handler_OnTurn(turn);
                //  print("trun");
            }
            get
            {
                return turn;
            }
        }

        [FoldoutGroup("GameData")]
        private int firstballimpact;
        public int FirstBallImpact
        {
            set
            {

                firstballimpact = value;

            }
            get { return firstballimpact; }
        }
        [FoldoutGroup("GameData")]

        [FoldoutGroup("GameData")]
        public SpriteRenderer AllowAreaForMoveCueBallRenderer;

        // [SerializeField] private int pitok;
        public int Pitok = 0;
        /* {
             set
             {
                 pitok = value;
                 if (pitok > 0)
                 {
                     Handler_OnPitok(pitok);
                 }
             }
             get { return pitok; }
         }*/
        [FoldoutGroup("GameData")]
        public bool EightBallEnableLeftShar = false;
        [FoldoutGroup("GameData")]
        public bool EightBallEnableRightShar = false;
        [FoldoutGroup("GameData")]
        public Basket Basket;
        [FoldoutGroup("GameData")]
        private List<int> PocketedBallsID = new List<int>();
        [FoldoutGroup("GameData")]
        public List<int> BallInBasket = new List<int>();
        [FoldoutGroup("GameData")]
        public int FirstPocketCall = 0;
        // [FoldoutGroup("GameData")]
        // public List<Pockets.Pockets> pockets;
        // [FoldoutGroup("GameData")]
        // public List<Diaco.EightBall.Band.BandsController> BandsControllers;
        [FoldoutGroup("GameData")]
        private List<int> IDImpactToWall;

        #endregion


        #region UI Settings


        [FoldoutGroup("BillboardAssets")]
        public Diaco.EightBall.Structs._Shars AssetsUI_Shar_Solid;
        [FoldoutGroup("BillboardAssets")]
        public Diaco.EightBall.Structs._Shars AssetsUI_Shar_Stripe;
        [FoldoutGroup("BillboardAssets")]
        public Sprite DisableShar_sprite;
        [FoldoutGroup("BillboardAssets")]
        public Sprite EightBall;
        [FoldoutGroup("BillboardAssets")]
        public ImageContainerTool.ImageContainer Avatars;

        [FoldoutGroup("BillboardAssets")]
        public Sprite Cup_sprite;
        [FoldoutGroup("BillboardAssets")]
        public Sprite Gem_sprite;
        [FoldoutGroup("BillboardAssets")]
        public Sprite Coin_sprite;

        [FoldoutGroup("BillboardUI")]
        public List<RTLTMPro.RTLTextMeshPro> UserNameIndicator;
        [FoldoutGroup("BillboardUI")]
        public Image CostTypeIndicator;
        [FoldoutGroup("BillboardUI")]
        public List<Button> UI_Biliboard_SharLeft, UI_Biliboard_SharRight;
        [FoldoutGroup("BillboardUI")]
        public List<Image> ProfileImage;
        [FoldoutGroup("BillboardUI")]
        public List<Image> PlayerCoolDowns;
        [FoldoutGroup("BillboardUI")]
        public Text TotalCoin;

        [FoldoutGroup("BillboardUI")]
        public Text PlayerTimeLeft;
        [FoldoutGroup("BillboardUI")]
        public Text PlayerTimeRight;

        [FoldoutGroup("BillboardUI")]
        public Image WoodInhHud;
        [FoldoutGroup("BillboardUI")]
        public ImageContainerTool.ImageContainer WoodImages;
        [FoldoutGroup("BillboardUI")]
        public StickerShareViwer StickerViwerLeft;
        [FoldoutGroup("BillboardUI")]
        public StickerShareViwer StickerViwerRight;
        [FoldoutGroup("BillboardUI")]
        public GameObject im_BadConnection;
        [FoldoutGroup("BillboardUI")]
        public Button BlockChat_Button;
        #endregion

        #region UnityFunctions
        public void Start()
        {
            ///  Debug.Log("tiiiim" + Time.fixedDeltaTime);

            if (InRecordMode == false)
            {
                var PocketsInScene = FindObjectsOfType<Pockets.Pockets>().ToList();
                AllowAreaForMoveCueBallRenderer = GameObject.Find("AreaForMoveCueBall").GetComponent<SpriteRenderer>();
                this.Basket = FindObjectOfType<Basket>();
                QueuePositionsBallFromServer = new List<PositionAndRotateBalls>();
                QueueCueBallPositionFromServer = new Queue<CueBallData>();
                QueueAimFromServer = new Queue<AimData>();
                IDImpactToWall = new List<int>();
                PocketedBallsID = new List<int>();
                BallInBasket = new List<int>();

                PocketsInScene.ForEach(pocket =>
                {
                    pocket.OnPocket += GameManager_OnPocket0;
                    //   Debug.Log("pocket find");
                    // pockets[1].OnPocket += GameManager_OnPocket1;
                    // pockets[2].OnPocket += GameManager_OnPocket2;
                    // pockets[3].OnPocket += GameManager_OnPocket3;
                    //pockets[4].OnPocket += GameManager_OnPocket4;
                    // pockets[5].OnPocket += GameManager_OnPocket5;
                });

                var namespaceserver = FindObjectOfType<GameLuncher>().NamespaceServer;
                this.Namespaceserver = namespaceserver;
                var tableName = (namespaceserver == "_competition") ? "_quick" : namespaceserver;

                SelectTable(tableName.Substring(1));
            }
            if (BlockChat_Button)
                BlockChat_Button.onClick.AddListener(() => { Emit_BlockChat(); });
        }
        public void OnEnable()
        {
            OnInitializeServer();
        }
        public void OnDisable()
        {
            CloseConnection();
            if (BlockChat_Button)
                BlockChat_Button.onClick.RemoveAllListeners();
        }
        public void Destroy()
        {
            CloseConnection();
            if (BlockChat_Button)
                BlockChat_Button.onClick.RemoveAllListeners();
        }
        #endregion

        #region Server_On
        public void OnInitializeServer()
        {

            var setting = FindObjectOfType<Diaco.Setting.GeneralSetting>();
            var namespaceserver = FindObjectOfType<GameLuncher>().NamespaceServer;
            var Notification_Dialog = FindObjectOfType<Diaco.Notification.Notification_Dialog_Manager>();
            string URL = setting.ServerAddress;

            Notification_Dialog.server_billiard = this;
            Notification_Dialog.init_Notification_billiard();


            SocketOptions options = new SocketOptions();
            options.AutoConnect = true;


            this.Namespaceserver = namespaceserver;
            socketmanager = new SocketManager(new Uri(URL), options);
            socket = socketmanager["/billiard" + namespaceserver];

            socket.On("connect", (s, p, m) =>
            {
                socket.Emit("authToken", ReadToken("token"),setting.Version);
                BadConnectionShow(false);
                Time.timeScale = 2;
                Debug.Log("Connection");
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
            if (InRecordMode == false)
            {
                socket.On("userInformation", (s, p, m) =>
                {
                    UserName = new Structs.UserInfo();
                    UserName = JsonUtility.FromJson<Diaco.EightBall.Structs.UserInfo>(m[0].ToString());
                });
                socket.On("gameData", (s, p, m) =>
                {
                    FirstPocketCall = 0;
                    Pitok = 0;
                    if (CoroutineSendPositionToServer != null)
                        StopCoroutine(CoroutineSendPositionToServer);
                    if (CoroutineRecivePositionFromServer != null)
                        StopCoroutine(CoroutineRecivePositionFromServer);

                    // PocketSelected = 0;
                    Turn = false;
                    gameData = new Structs.GameData();
                    gameData = JsonUtility.FromJson<Diaco.EightBall.Structs.GameData>(m[0].ToString());

                    /*if (!SpwnedBall)
                        SelectTable(gameData.table);*/

                    if (gameData.playerOne.userName == UserName.userName)
                    {
                        StartCoroutine(SetPlayerOne(gameData));

                    }
                    else
                    {
                        StartCoroutine(SetPlayerTwo(gameData));

                    }




                    Debug.Log("GAME DATA");
                    //Handler_GameReady();
                    //  Debug.Log(m[0].ToString());
                });
                socket.On("RoomId", (s, p, m) =>
                {

                    Emit_ReadyForPlayGame(m[0].ToString());
                    //  Debug.Log(m[0].ToString());
                });
                socket.On("Position", (s, p, m) =>
                {
                    var positions = JsonUtility.FromJson<Diaco.EightBall.Structs.PositionAndRotateBalls>(m[0].ToString());
                    QueuePositionsBallFromServer.Add(positions);
                    //  Debug.Log("tik" + positions.Tik + "S" + positions.CueBall.x);
                    if (intergateplayposition == 0)
                    {

                        CoroutineRecivePositionFromServer = StartCoroutine(PositionsBallsRecivedFromServer());
                        intergateplayposition = 1;

                    }
                    // Debug.Log("Reciveeeeee");
                });
                socket.On("PositionCueBall", (s, p, m) =>
                {
                    var positionCueball = JsonUtility.FromJson<CueBallData>(m[0].ToString());
                    QueueCueBallPositionFromServer.Enqueue(positionCueball);
                    if (QueueCueBallPositionFromServer.Count > 0)
                    {
                        StartCoroutine(CueBallPositionRecivedFromServer());
                        //  Debug.Log("Recive1" );
                    }

                    //Debug.Log("Recive3");
                });
                socket.On("Aim", (s, p, m) =>
                {

                    var aimdata = JsonUtility.FromJson<Diaco.EightBall.Structs.AimData>(m[0].ToString());
                    QueueAimFromServer.Enqueue(aimdata);
                    if (QueueAimFromServer.Count > 0)
                    {
                        StartCoroutine(AimRecivedFromServer());
                    }
                });
                socket.On("CancelCooldown", (s, p, m) =>
                {

                    CancelCoolDownTimer();
                    // Pitok = 0;

                });
                socket.On("game-result", (s, p, m) =>
                {
                    StartCoroutine(ShowResualtPage(m));
                });
                socket.On("shop", (s, p, m) => {

                    var data = JsonUtility.FromJson<Diaco.Store.Billiard.BilliardShopDatas>(m[0].ToString());
                    Handler_InitShop(data);
                    Debug.Log("ShopInGameRecive");
                });
                socket.On("cueState", (s, p, m) => {
                    var data = JsonUtility.FromJson<CueStateData>(m[0].ToString());
                    SetWoodState(data);
                    Debug.Log("ChangeCueSate");
                });
                socket.On("getSticker", (s, p, m) => {

                    var data = JsonUtility.FromJson<StickerData>(m[0].ToString());
                    Handler_GetStickers(data);
                    Debug.Log("StickerRecived");
                });
                socket.On("shareSticker", (s, p, m) => {

                    StickerViwer(m[0], m[1]);
                    Debug.Log("ShareStickerRecived");
                });
                socket.On("message", (s, p, m) => {

                    var message = Convert.ToString(m[0]);
                    var durtaion = Convert.ToSingle(m[1]);
                    if (Convert.ToInt16(m[2]) == 0)
                        Time.timeScale = 0;
                    else
                        Time.timeScale = 2;
                    Handler_IncomingMessage(message, durtaion);
                    Debug.Log("ReciveMessage:" + message);
                });


                socket.On("gameTime", (s, p, m) => {


                    SetTimePlayerInUI(Convert.ToSingle(m[0]) / 1000, Convert.ToSingle(m[1]) / 1000);

                });


            }
            else
            {
                socket.On("gameData", (s, p, a) =>
                {

                    var recordmodeGameData = JsonUtility.FromJson<RecordModeGameData>(a[0].ToString());

                    ClearSceneInRecordMode();
                    SpawnAssetInRecordMode(recordmodeGameData.whiteballPos, recordmodeGameData.colorballPos, recordmodeGameData.siblPos);
                    SetUIInRecordMode(recordmodeGameData.totalPoint, recordmodeGameData.level, recordmodeGameData.timer, recordmodeGameData.points);

                    Sibl.Area = 4;
                    Turn = true;
                    Handler_GameReady();
                    Debug.Log("ReadyRecordMode");
                });
                socket.On("result", (s, p, a) =>
                {

                    var result = JsonUtility.FromJson<ResualtInRecordModeData>(a[0].ToString());
                    resualtInRecordMode.gameObject.SetActive(true);
                    resualtInRecordMode.Set(result);
                    CancelInvoke("RunTimer");
                    Debug.Log("ReadyRecordModeResualt");
                });
            }
            socket.On("BackToMenu", (s, p, m) => {


                //// CloseSocket();
                var Luncher = FindObjectOfType<GameLuncher>();
                Luncher.BackToMenu();
                Debug.Log("back To menu from Server");


            });
            socket.On("disconnect", (s, p, m) =>
            {
                if (FindObjectOfType<GameLuncher>().InBackToMenu == false)
                {
                    Time.timeScale = 0;
                }
                else
                {

                    FindObjectOfType<GameLuncher>().InBackToMenu = false;
                    Time.timeScale = 1;
                }
                BadConnectionShow(true);
                // Debug.Log("disConnection");
            });
        }



        #endregion

        #region Server_Emit

        public void Emit_ReadyForPlayGame(string RoomID)
        {
            socket.Emit("ImReady", RoomID);
            //   Debug.Log("Ready For Play Game");
        }
        public void Emit_AimCueBall(AimData aimdata)
        {
            var json = JsonUtility.ToJson(aimdata);
            if (socket != null)
                socket.Emit("Aim", json);
            //   Debug.Log("Sending Aim To Server");
        }
        public void Emit_PositionCueBallInPitoks(CueBallData data)
        {
            var cue_data = JsonUtility.ToJson(data);
            socket.Emit("PositionCueBall", cue_data);
            // Debug.Log("Sending Cue Ball Position To Server"+data.position);
        }


        public void Emit_PositionsBalls(PositionAndRotateBalls positionAndRotate)
        {
            var data = JsonUtility.ToJson(positionAndRotate);
            socket.Emit("Position", data);
            //  Debug.Log("Sending  Balls Position To Server");
        }


        public void Emit_SendDataOfGameOnEndTurn(Vector3 LastPosCueBall)
        {
            var LastPosition = JsonUtility.ToJson(LastPosCueBall);
            if (GamePlayRule == _GamePlayRule.classic)
                socket.Emit("EndRecord", PocketedBallsID, FirstBallImpact, IDImpactToWall, LastPosition);
            else
                socket.Emit("EndRecord", PocketedBallsID, FirstBallImpact, IDImpactToWall, LastPosition, FirstPocketCall);
            Debug.Log("End Record And SendData Of Turn");
        }
        public void Emit_EndPlayRecord()
        {


            socket.Emit("EndRecord");
        }
        public void Emit_LeftGame()
        {
           socket.Emit("left-game");
            Debug.Log("Left game");
            //CloseConnection();

        }
        public void Emit_PlayAgain()
        {
            //var luncher = FindObjectOfType<GameLuncher>();
            socket.Emit("play-again");

            //  luncher.PlayAgainGame(1);
        }
        public void Emit_Shop()
        {
            socket.Emit("shop");
        }
        public void Emit_UseCue(string id)
        {
            socket.Emit("useCue", id);

            Debug.Log("USSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE");
        }
        public void Emit_RentCue(string rentId)
        {
            socket.Emit("rentCue", rentId);
            Debug.Log("Emit_ShopformationRent=" + "::" + rentId);
        }


        public void Emit_GetSticker()
        {
            socket.Emit("getSticker");
            Debug.Log("Emit_getSticker");
        }
        public void Emit_ShareSticker(string name)
        {
            socket.Emit("shareSticker", name);
            Debug.Log("Emit_shareSticker");
        }
        public void Emit_Message(string message)
        {
            socket.Emit("message", message);
            Debug.Log("Emit_Message");

        }
        public void Emit_BlockChat()
        {
            socket.Emit("blockChat");
            Debug.Log("ChatBloked!");
        }
        public void Emit_AddFriend()
        {
            socket.Emit("add-friend");
            Debug.Log("ChatBloked!");
        }
        public void Emit_CallPocket(int id)
        {
            socket.Emit("selectPocket", id);
            Debug.Log("selectPocket:" + id);
        }

        public void Emit_DialogAndNotification(string eventName, string data)
        {
            socket.Emit(eventName, data);
        }
        #endregion

        #region BilliardGameFunction

        #region IN NORMAL MODE
        public IEnumerator SetPlayerOne(Diaco.EightBall.Structs.GameData data)
        {

            if (SpwnedBall == false)
            {
                SpawnBalls(data);
                Debug.Log("AAAAAAAAAAAA");
            }

            if (ResultGamePage.activeSelf && data.state != -1)
            {
                // ResultGamePage.SetActive(false);
                // ResetSharBillboard();
                // EnableSharInBiliboard();
                var luncher = FindObjectOfType<GameLuncher>();
                luncher.PlayAgainGame(1);

            }
            DeletedBallCount = 0;

            CancelCoolDownTimer();
            KinimaticBalls(true);
            SetUserNameInBillboard(data.playerOne.userName, data.playerTwo.userName);
            UpdateAvatarProfile(Avatars.LoadImage(data.playerOne.avatar), Avatars.LoadImage(data.playerTwo.avatar));
            SetTypeCost(Convert.ToInt16(data.costType));
            SetCountCostBillboard(data.cost.ToString());

            yield return new WaitForSeconds(0.01f);

            ///   SetPositionsBalls(data.positions);
            /*  if (!Infunc)
                  yield return StartCoroutine(SpwanBallInBasketAndDestroyBallInTable(data));*/
            /// StartCoroutine(SpwanBallInBasketAndDestroyBallInTable(data));

            StartCoroutine(DeleteAdditionsBallInTable(data));
            StartCoroutine(CheckBallInBasket(data));
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(FASTPlayRecordPositionsBallsAndRecivedFromServer(data));

            ResetSharBillboard();
            if (data.sharSeted)
            {
                if (data.playerOne.shar == "solid")
                {
                    SetSharForPlayers(Shar.Solid, Shar.Stripe, 0);

                }
                else
                {
                    SetSharForPlayers(Shar.Stripe, Shar.Solid, 1);
                }
                SetDisableSharInBiliboard(data.deletedBalls);


            }

            SetTimePlayerInUI(data.playerOne.time / 1000, data.playerTwo.time / 1000);

            Handler_EnableBoarderPocket(false, 0);

            if (data.ownerTurn == 1)
            {
                if (gameData.selectedPocket == -1)
                {
                    Handler_EnableBoarderPocket(false, 0);
                    initializTurn(data);
                }
                else if (gameData.selectedPocket == 0)
                {
                    Handler_EnableBoarderPocket(true, 0);
                }
                else if (gameData.selectedPocket > 0)
                {
                    initializTurn(data);
                    Handler_EnableBoarderPocket(true, gameData.selectedPocket);
                }
                EnableCoolDown(Side.Left, data.turnTime,  data.totalTime);

                //  Debug.Log("TimeAndTurnOnwer");
            }
            else
            {
                if (gameData.selectedPocket == 0)
                {
                    Handler_EnableBoarderPocket(false, 0);
                }
                else if (gameData.selectedPocket > 0)
                {
                    Handler_EnableBoarderPocket(true, gameData.selectedPocket);
                }


                EnableCoolDown(Side.Right, data.turnTime, data.totalTime);

                CheckEnable8BallRightInOtherClient();

                // AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>().ActiveAimSystemForShowInOtherClient(true);
                //Debug.Log("HE::::::");
            }



            if (Convert.ToInt16(data.state) == -1)
            {
                if (data.winner == UserName.userName)
                {
                    Debug.Log("Winner");
                    //  GameResult.enabled = true;
                    //  GameResult.text = "Winner";
                }
                if (data.loser == UserName.userName)
                {
                    Debug.Log("Loser");
                    //GameResult.enabled = true;
                    // GameResult.text = "Loser";
                }
                Turn = false;
            }

            AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>().inPlayPos = false;
            yield return new WaitForSeconds(0.1f);
            Handler_GameReady();
        }
        public IEnumerator SetPlayerTwo(Diaco.EightBall.Structs.GameData data)
        {

            if (SpwnedBall == false)
            {
                SpawnBalls(data);
                
            }

            if (ResultGamePage.activeSelf && data.state != -1)
            {
                //  ResultGamePage.SetActive(false);
                // ResetSharBillboard();
                // EnableSharInBiliboard();
                var luncher = FindObjectOfType<GameLuncher>();
                luncher.PlayAgainGame(1);
                //return;
            }
            DeletedBallCount = 0;

            CancelCoolDownTimer();
            KinimaticBalls(true);
            SetUserNameInBillboard(data.playerTwo.userName, data.playerOne.userName);
            UpdateAvatarProfile(Avatars.LoadImage(data.playerTwo.avatar), Avatars.LoadImage(data.playerOne.avatar));
            SetTypeCost(Convert.ToInt16(data.costType));
            SetCountCostBillboard(data.cost.ToString());
            yield return new WaitForSeconds(0.01f);
            /*  if (!Infunc)
                   yield return StartCoroutine(SpwanBallInBasketAndDestroyBallInTable(data));*/
            /// StartCoroutine(SpwanBallInBasketAndDestroyBallInTable(data));

            StartCoroutine(DeleteAdditionsBallInTable(data));
            StartCoroutine(CheckBallInBasket(data));
            yield return new WaitForSeconds(0.2f);
            StartCoroutine(FASTPlayRecordPositionsBallsAndRecivedFromServer(data));

            ResetSharBillboard();
            if (data.sharSeted)
            {
                if (data.playerTwo.shar == "solid")
                {
                    SetSharForPlayers(Shar.Solid, Shar.Stripe, 0);

                }
                else
                {
                    SetSharForPlayers(Shar.Stripe, Shar.Solid, 1);
                }
                SetDisableSharInBiliboard(data.deletedBalls);
            }

            /// SetTimePlayerInUI(data.playerTwo.time / 1000, data.playerOne.time / 1000);
            /// 
            Handler_EnableBoarderPocket(false, 0);
            if (data.ownerTurn == 2)
            {
                if (gameData.selectedPocket == -1)
                {
                    Handler_EnableBoarderPocket(false, 0);
                    initializTurn(data);
                }
                else if (gameData.selectedPocket == 0)
                {
                    Handler_EnableBoarderPocket(true, 0);
                }
                else if (gameData.selectedPocket > 0)
                {
                    initializTurn(data);
                    Handler_EnableBoarderPocket(true, gameData.selectedPocket);
                }

                EnableCoolDown(Side.Left, data.turnTime, data.totalTime);

                //  Debug.Log("TimeAndTurnOnwer");
            }
            else
            {
                if (gameData.selectedPocket == 0)
                {
                    Handler_EnableBoarderPocket(false, 0);
                }
                else if (gameData.selectedPocket > 0)
                {
                    Handler_EnableBoarderPocket(true, gameData.selectedPocket);
                }
                EnableCoolDown(Side.Right, data.turnTime, data.totalTime);


                CheckEnable8BallRightInOtherClient();
                //   AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>().ActiveAimSystemForShowInOtherClient(true);

                // Debug.Log("TimeAndTurn");
            }




            if (Convert.ToInt16(data.state) == -1)
            {
                if (data.winner == UserName.userName)
                {
                    Debug.Log("Winner");
                    //GameResult.enabled = true;
                    //GameResult.text = "Winner";
                }
                if (data.loser == UserName.userName)
                {
                    Debug.Log("Loser");
                    //  GameResult.enabled = true;
                    //  GameResult.text = "Loser";
                }
            }
            AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>().inPlayPos = false;
            yield return new WaitForSeconds(0.01f);
            Handler_GameReady();
        }
        public void SetWoodState(CueStateData state)
        {
            var cueball = FindObjectOfType<Diaco.EightBall.CueControllers.HitBallController>();
            WoodInhHud.sprite = WoodImages.LoadImage(state.name);

            cueball.PowerCUE = Mathf.Clamp(state.force, 1.34f, 1.74f);
            cueball.PowerSpin = Mathf.Clamp(state.spin, 3, 4);
            Debug.Log("WoodUpdate::" + state.name + ";;" + state.spin);
        }
        public void initializTurn(Diaco.EightBall.Structs.GameData data)
        {
            DOVirtual.Float(0f, 0.1f, 1, (x) => { }).OnComplete(() =>
            {

                CheckEnable8Ball();
                ClearPocketedBallList();
                IDImpactToWall.Clear();


                FirstBallImpact = 0;

                Turn = true;
                // CallPacketEnable();
                // Debug.Log("Turn");
            });

            /*   TurnRecived = false;
               temp_lastpos = Vector3.zero;
               temp_pitok = -1;*/
        }

        private bool CheckBallsMove()
        {
            var move = false;
            //var count_sleep = 0;
            for (int i = 0; i < AddressBalls.Count; i++)
            {
                if (AddressBalls[i] != null)
                {

                    if (AddressBalls[i].GetComponent<Rigidbody>().velocity != Vector3.zero || AddressBalls[i].GetComponent<Rigidbody>().angularVelocity != Vector3.zero)
                    {

                        move = true;
                    }
                }
            }

            return move;
        }

        private Tween canclemove;
        private IEnumerator PositionsBallsSendToServer()
        {
            PositionAndRotateBalls PositionBalls = new PositionAndRotateBalls();
            bool integateAllow = true;
            canclemove = DOVirtual.DelayedCall(14, () =>
            {
                integateAllow = false;
                for (int i = 0; i < AddressBalls.Count; i++)
                {
                    AddressBalls[i].StopMoving();
                }
                Debug.Log("Force Stop");

            }, false);
            do
            {

                #region T3
                if (AddressBalls[0] != null)
                {
                    PositionBalls.CueBall = Vec3Helper.ToBilliardVec(AddressBalls[0].transform.position);
                    PositionBalls.CueBall_velocity = Vec3Helper.ToBilliardVec(AddressBalls[0].rb.velocity);
                    PositionBalls.CueBall_R = Vec3Helper.ToBilliardVec(AddressBalls[0].rb.angularVelocity);

                    // PositionBalls.CueBallInPocket = false;
                }
                if (AddressBalls[1] != null)
                {
                    PositionBalls.Ball_1 = Vec3Helper.ToBilliardVec(AddressBalls[1].transform.position);
                    PositionBalls.Ball_1_velocity = Vec3Helper.ToBilliardVec(AddressBalls[1].rb.velocity);
                    PositionBalls.Ball_1_R = Vec3Helper.ToBilliardVec(AddressBalls[1].rb.angularVelocity);
                }
                if (AddressBalls[2] != null)
                {
                    PositionBalls.Ball_2 = Vec3Helper.ToBilliardVec(AddressBalls[2].transform.position);
                    PositionBalls.Ball_2_velocity = Vec3Helper.ToBilliardVec(AddressBalls[2].rb.velocity);
                    PositionBalls.Ball_2_R = Vec3Helper.ToBilliardVec(AddressBalls[2].rb.angularVelocity);
                }
                if (AddressBalls[3] != null)
                {
                    PositionBalls.Ball_3 = Vec3Helper.ToBilliardVec(AddressBalls[3].transform.position);
                    PositionBalls.Ball_3_velocity = Vec3Helper.ToBilliardVec(AddressBalls[3].rb.velocity);
                    PositionBalls.Ball_3_R = Vec3Helper.ToBilliardVec(AddressBalls[3].rb.angularVelocity);
                }
                if (AddressBalls[4] != null)
                {
                    PositionBalls.Ball_4 = Vec3Helper.ToBilliardVec(AddressBalls[4].transform.position);
                    PositionBalls.Ball_4_velocity = Vec3Helper.ToBilliardVec(AddressBalls[4].rb.velocity);
                    PositionBalls.Ball_4_R = Vec3Helper.ToBilliardVec(AddressBalls[4].rb.angularVelocity);
                    ///  PositionBalls.Ball_4InPocket = AddressBalls[4].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix;
                }
                if (AddressBalls[5] != null)
                {
                    PositionBalls.Ball_5 = Vec3Helper.ToBilliardVec(AddressBalls[5].transform.position);
                    PositionBalls.Ball_5_velocity = Vec3Helper.ToBilliardVec(AddressBalls[5].rb.velocity);
                    PositionBalls.Ball_5_R = Vec3Helper.ToBilliardVec(AddressBalls[5].rb.angularVelocity);
                    ///  PositionBalls.Ball_5InPocket = AddressBalls[5].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix;
                }
                if (AddressBalls[6] != null)
                {
                    PositionBalls.Ball_6 = Vec3Helper.ToBilliardVec(AddressBalls[6].transform.position);
                    PositionBalls.Ball_6_velocity = Vec3Helper.ToBilliardVec(AddressBalls[6].rb.velocity);
                    PositionBalls.Ball_6_R = Vec3Helper.ToBilliardVec(AddressBalls[6].rb.angularVelocity);
                    ///  PositionBalls.Ball_6InPocket = AddressBalls[6].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix;
                }
                if (AddressBalls[7] != null)
                {
                    PositionBalls.Ball_7 = Vec3Helper.ToBilliardVec(AddressBalls[7].transform.position);
                    PositionBalls.Ball_7_velocity = Vec3Helper.ToBilliardVec(AddressBalls[7].rb.velocity);
                    PositionBalls.Ball_7_R = Vec3Helper.ToBilliardVec(AddressBalls[7].rb.angularVelocity);
                    //   PositionBalls.Ball_7InPocket = AddressBalls[7].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix;
                }
                if (AddressBalls[8] != null)
                {
                    PositionBalls.Ball_8 = Vec3Helper.ToBilliardVec(AddressBalls[8].transform.position);
                    PositionBalls.Ball_8_velocity = Vec3Helper.ToBilliardVec(AddressBalls[8].rb.velocity);
                    PositionBalls.Ball_8_R = Vec3Helper.ToBilliardVec(AddressBalls[8].rb.angularVelocity);
                    /////  PositionBalls.Ball_8InPocket = AddressBalls[8].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix;
                }
                if (AddressBalls[9] != null)
                {
                    PositionBalls.Ball_9 = Vec3Helper.ToBilliardVec(AddressBalls[9].transform.position);
                    PositionBalls.Ball_9_velocity = Vec3Helper.ToBilliardVec(AddressBalls[9].rb.velocity);
                    PositionBalls.Ball_9_R = Vec3Helper.ToBilliardVec(AddressBalls[9].rb.angularVelocity);
                    //// PositionBalls.Ball_9InPocket = AddressBalls[99].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix;
                }
                if (AddressBalls[10] != null)
                {
                    PositionBalls.Ball_10 = Vec3Helper.ToBilliardVec(AddressBalls[10].transform.position);
                    PositionBalls.Ball_10_velocity = Vec3Helper.ToBilliardVec(AddressBalls[10].rb.velocity);
                    PositionBalls.Ball_10_R = Vec3Helper.ToBilliardVec(AddressBalls[10].rb.angularVelocity);
                    ////   PositionBalls.Ball_10InPocket = AddressBalls[10].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix;
                }
                if (AddressBalls[11] != null)
                {
                    PositionBalls.Ball_11 = Vec3Helper.ToBilliardVec(AddressBalls[11].transform.position);
                    PositionBalls.Ball_11_velocity = Vec3Helper.ToBilliardVec(AddressBalls[11].rb.velocity);
                    PositionBalls.Ball_11_R = Vec3Helper.ToBilliardVec(AddressBalls[11].rb.angularVelocity);
                    ///   PositionBalls.Ball_11InPocket = AddressBalls[11].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix;
                }
                if (AddressBalls[12] != null)
                {
                    PositionBalls.Ball_12 = Vec3Helper.ToBilliardVec(AddressBalls[12].transform.position);
                    PositionBalls.Ball_12_velocity = Vec3Helper.ToBilliardVec(AddressBalls[12].rb.velocity);
                    PositionBalls.Ball_12_R = Vec3Helper.ToBilliardVec(AddressBalls[12].rb.angularVelocity);
                    ////   PositionBalls.Ball_12InPocket = AddressBalls[12].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix;
                }
                if (AddressBalls[13] != null)
                {
                    PositionBalls.Ball_13 = Vec3Helper.ToBilliardVec(AddressBalls[13].transform.position);
                    PositionBalls.Ball_13_velocity = Vec3Helper.ToBilliardVec(AddressBalls[13].rb.velocity);
                    PositionBalls.Ball_13_R = Vec3Helper.ToBilliardVec(AddressBalls[13].rb.angularVelocity);
                    ////   PositionBalls.Ball_13InPocket = AddressBalls[13].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix;
                }
                if (AddressBalls[14] != null)
                {
                    PositionBalls.Ball_14 = Vec3Helper.ToBilliardVec(AddressBalls[14].transform.position);
                    PositionBalls.Ball_14_velocity = Vec3Helper.ToBilliardVec(AddressBalls[14].rb.velocity);
                    PositionBalls.Ball_14_R = Vec3Helper.ToBilliardVec(AddressBalls[14].rb.angularVelocity);
                    ///    PositionBalls.Ball_14InPocket = AddressBalls[14].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix;
                }
                if (AddressBalls[15] != null)
                {
                    PositionBalls.Ball_15 = Vec3Helper.ToBilliardVec(AddressBalls[15].transform.position);
                    PositionBalls.Ball_15_velocity = Vec3Helper.ToBilliardVec(AddressBalls[15].rb.velocity);
                    PositionBalls.Ball_15_R = Vec3Helper.ToBilliardVec(AddressBalls[15].rb.angularVelocity);
                    /////PositionBalls.Ball_15InPocket = AddressBalls[15].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix;
                }
                #endregion
                PositionBalls.isLastPacket = false;
                Emit_PositionsBalls(PositionBalls);

                //Debug.Log(PositionBalls.Tik);
                yield return new WaitForSecondsRealtime(Framerate);
                PositionBalls.Tik++;

            } while (CheckBallsMove() && integateAllow);

            canclemove.Kill(false);
            PositionBalls.isLastPacket = true;
            Debug.Log("LastSend:" + PositionBalls.Tik);
            Emit_PositionsBalls(PositionBalls);
            //TimeStep = Time.realtimeSinceStartup;
            Emit_SendDataOfGameOnEndTurn(AddressBalls[0].transform.position);
            TimeStep = 0.0f;

            // AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>().ActiveAimSystemOnPlayRecord(true);

            yield return new WaitForSecondsRealtime(Framerate);
        }
        public void StartSendPositionToServer()
        {
            CoroutineSendPositionToServer = StartCoroutine(PositionsBallsSendToServer());
        }
        public IEnumerator PositionsBallsRecivedFromServer()
        {

            var cueball = AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>();
            PositionAndRotateBalls PositionBalls = new PositionAndRotateBalls();
            cueball.Handler_OnHitBall(-1, Vector3.zero);
            cueball.DragIsBusy = true;
            cueball.inPlayPos = true;
            int tik = 0;
            bool loopCancle = false;

            yield return new WaitForSecondsRealtime(2.0f);
            while (loopCancle == false)
            {


                try
                {
                    // Debug.Log(tik + "   ::::::::::   " + QueuePositionsBallFromServer.Count);
                    tik = Mathf.Clamp(tik, 0, QueuePositionsBallFromServer.Count - 1);
                    PositionBalls = QueuePositionsBallFromServer[tik];

                    //  if (tik > QueuePositionsBallFromServer.Count)
                    //      tik = QueuePositionsBallFromServer.Count;
                    //  Debug.Log(tik +"   ::::::::::   "+ QueuePositionsBallFromServer.Count);

                    if (AddressBalls[0] != null)
                    {

                        AddressBalls[0].MoveBall(PositionBalls.CueBall,
                            PositionBalls.CueBall_R,
                            PositionBalls.CueBall_velocity,

                            Framerate,
                            PositionBalls.Tik, tik);
                        // AddressBalls[0].Rotate(PositionBalls.CueBall_R, Framerate);
                        /// PositionBalls.Ball_1InPocket = AddressBalls[1].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix;
                    }
                    if (AddressBalls[1] != null)
                    {
                        AddressBalls[1].MoveBall(PositionBalls.Ball_1,
                            PositionBalls.Ball_1_R,
                            PositionBalls.Ball_1_velocity,
                            Framerate, PositionBalls.Tik, tik);
                        //     AddressBalls[1].Rotate(PositionBalls.Ball_1_R, Framerate);
                        //// AddressBalls[1].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix = PositionBalls.Ball_1InPocket;
                    }
                    if (AddressBalls[2] != null)
                    {
                        AddressBalls[2].MoveBall(PositionBalls.Ball_2,
                            PositionBalls.Ball_2_R,
                            PositionBalls.Ball_2_velocity,
                              Framerate, PositionBalls.Tik, tik);
                        ///   AddressBalls[2].Rotate(PositionBalls.Ball_2_R, Framerate);
                        ///    AddressBalls[2].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix = PositionBalls.Ball_2InPocket;
                    }
                    if (AddressBalls[3] != null)
                    {
                        AddressBalls[3].MoveBall(PositionBalls.Ball_3,
                            PositionBalls.Ball_3_R,
                            PositionBalls.Ball_3_velocity,
                             Framerate, PositionBalls.Tik, tik);
                        //  AddressBalls[3].Rotate(PositionBalls.Ball_3_R, Framerate);
                        ////   AddressBalls[3].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix = PositionBalls.Ball_3InPocket;
                    }
                    if (AddressBalls[4] != null)
                    {
                        AddressBalls[4].MoveBall(PositionBalls.Ball_4,
                            PositionBalls.Ball_4_R,
                            PositionBalls.Ball_4_velocity,
                             Framerate, PositionBalls.Tik, tik);
                        //     AddressBalls[4].Rotate(PositionBalls.Ball_4_R, Framerate);
                        //////  AddressBalls[4].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix = PositionBalls.Ball_4InPocket;
                    }
                    if (AddressBalls[5] != null)
                    {
                        AddressBalls[5].MoveBall(PositionBalls.Ball_5,
                            PositionBalls.Ball_5_R,
                            PositionBalls.Ball_5_velocity,
                             Framerate, PositionBalls.Tik, tik);
                        //  AddressBalls[5].Rotate(PositionBalls.Ball_5_R, Framerate);
                        //////   AddressBalls[5].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix = PositionBalls.Ball_5InPocket;
                    }
                    if (AddressBalls[6] != null)
                    {
                        AddressBalls[6].MoveBall(PositionBalls.Ball_6,
                            PositionBalls.Ball_6_R,
                            PositionBalls.Ball_6_velocity,
                             Framerate, PositionBalls.Tik, tik);
                        /// AddressBalls[6].Rotate(PositionBalls.Ball_6_R, Framerate);
                        ////  AddressBalls[6].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix = PositionBalls.Ball_6InPocket;
                    }
                    if (AddressBalls[7] != null)
                    {
                        AddressBalls[7].MoveBall(PositionBalls.Ball_7,
                            PositionBalls.Ball_7_R,
                            PositionBalls.Ball_7_velocity,
                             Framerate, PositionBalls.Tik, tik);
                        // AddressBalls[7].Rotate(PositionBalls.Ball_7_R, Framerate);
                        //   AddressBalls[7].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix = PositionBalls.Ball_7InPocket;
                    }
                    if (AddressBalls[8] != null)
                    {
                        AddressBalls[8].MoveBall(PositionBalls.Ball_8,
                            PositionBalls.Ball_8_R,
                            PositionBalls.Ball_8_velocity,
                             Framerate, PositionBalls.Tik, tik);
                        ///    AddressBalls[8].Rotate(PositionBalls.Ball_8_R, Framerate);
                        ///   AddressBalls[8].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix = PositionBalls.Ball_8InPocket;
                    }
                    if (AddressBalls[9] != null)
                    {
                        AddressBalls[9].MoveBall(PositionBalls.Ball_9,
                            PositionBalls.Ball_9_R,
                            PositionBalls.Ball_9_velocity,
                             Framerate, PositionBalls.Tik, tik);
                        // AddressBalls[9].Rotate(PositionBalls.Ball_9_R, Framerate);
                        //  AddressBalls[9].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix = PositionBalls.Ball_9InPocket;
                    }
                    if (AddressBalls[10] != null)
                    {
                        AddressBalls[10].MoveBall(PositionBalls.Ball_10,
                            PositionBalls.Ball_10_R,
                            PositionBalls.Ball_10_velocity,
                             Framerate, PositionBalls.Tik, tik);
                        //// AddressBalls[10].Rotate(PositionBalls.Ball_10_R, Framerate);
                        //   AddressBalls[10].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix = PositionBalls.Ball_10InPocket;
                    }
                    if (AddressBalls[11] != null)
                    {
                        AddressBalls[11].MoveBall(PositionBalls.Ball_11,
                            PositionBalls.Ball_11_R,
                            PositionBalls.Ball_11_velocity,
                             Framerate, PositionBalls.Tik, tik);
                        ///  AddressBalls[11].Rotate(PositionBalls.Ball_11_R, Framerate);
                        //  AddressBalls[11].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix = PositionBalls.Ball_11InPocket;
                    }
                    if (AddressBalls[12] != null)
                    {
                        AddressBalls[12].MoveBall(PositionBalls.Ball_12,
                            PositionBalls.Ball_12_R,
                            PositionBalls.Ball_12_velocity,
                             Framerate, PositionBalls.Tik, tik);
                        ///      AddressBalls[12].Rotate(PositionBalls.Ball_12_R, Framerate);
                        //  AddressBalls[12].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix = PositionBalls.Ball_12InPocket;
                    }
                    if (AddressBalls[13] != null)
                    {
                        AddressBalls[13].MoveBall(PositionBalls.Ball_13,
                            PositionBalls.Ball_13_R,
                            PositionBalls.Ball_13_velocity,
                             Framerate, PositionBalls.Tik, tik);
                        // AddressBalls[13].Rotate(PositionBalls.Ball_13_R, Framerate);
                        //   AddressBalls[13].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix = PositionBalls.Ball_13InPocket;
                    }
                    if (AddressBalls[14] != null)
                    {
                        AddressBalls[14].MoveBall(PositionBalls.Ball_14,
                            PositionBalls.Ball_14_R,
                            PositionBalls.Ball_14_velocity,
                             Framerate, PositionBalls.Tik, tik);
                        ////   AddressBalls[14].Rotate(PositionBalls.Ball_14_R, Framerate);
                        /// AddressBalls[14].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix = PositionBalls.Ball_14InPocket;
                    }
                    if (AddressBalls[15] != null)
                    {
                        AddressBalls[15].MoveBall(PositionBalls.Ball_15,
                            PositionBalls.Ball_15_R,
                            PositionBalls.Ball_15_velocity,
                             Framerate, PositionBalls.Tik, tik);
                        //  AddressBalls[15].Rotate(PositionBalls.Ball_15_R, Framerate);
                        ///  AddressBalls[15].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix = PositionBalls.Ball_15InPocket;
                    }
                    if (PositionBalls.isLastPacket)
                    {

                        for (int i = 0; i < AddressBalls.Count; i++)
                        {
                            if (AddressBalls[i] != null)
                                AddressBalls[i].StopMoving();
                        }
                        Emit_EndPlayRecord();
                        loopCancle = true;
                        // Debug.Log("LastRecive:" + PositionBalls.Tik);

                    }

                }
                catch(Exception r)
                {

                }
                //  Physics.Simulate(Time.fixedDeltaTime);
                yield return new WaitForSecondsRealtime(Framerate);

               //  Debug.Log("................Count List:"+QueuePositionsBallFromServer.Count+"...............TIk PACKET:"+PositionBalls.Tik+"................TIkLOOP:"+tik);
                tik++;
            }
            //Debug.Log("xXXXXXXXXXxXXXXXXXXXXXXXXX");
            QueuePositionsBallFromServer.Clear();
            // cueball.resetpos();
            intergateplayposition = 0;



        }
        public IEnumerator FASTPlayRecordPositionsBallsAndRecivedFromServer(Diaco.EightBall.Structs.GameData data)
        {
            PositionAndRotateBalls PositionBalls = new PositionAndRotateBalls();
            PositionBalls = data.positions;

            var cueball = AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>();
            // cueball.ActiveAimSystem(false);



            #region T1
            if (AddressBalls[0] != null)
            {

                AddressBalls[0].MoveBall(PositionBalls.CueBall,
                    PositionBalls.CueBall_R,
                    PositionBalls.CueBall_velocity,
                     0, 0, 0);
                // AddressBalls[0].Rotate(PositionBalls.CueBall_R, Framerate);
                /// PositionBalls.Ball_1InPocket = AddressBalls[1].GetComponent<Diaco.EightBall.CueControllers.Ball>().EnableYFix;
            }
            if (AddressBalls[1] != null)
            {
                AddressBalls[1].MoveBall(PositionBalls.Ball_1,
                    PositionBalls.Ball_1_R,
                    PositionBalls.Ball_1_velocity,
                     0, 0, 0);
               
            }
            if (AddressBalls[2] != null)
            {
                AddressBalls[2].MoveBall(PositionBalls.Ball_2,
                    PositionBalls.Ball_2_R,
                    PositionBalls.Ball_2_velocity,
                     0, 0, 0);
              
            }
            if (AddressBalls[3] != null)
            {
                AddressBalls[3].MoveBall(PositionBalls.Ball_3,
                    PositionBalls.Ball_3_R,
                    PositionBalls.Ball_3_velocity,
                     0, 0, 0);
              
            }
            if (AddressBalls[4] != null)
            {
                AddressBalls[4].MoveBall(PositionBalls.Ball_4,
                    PositionBalls.Ball_4_R,
                    PositionBalls.Ball_4_velocity,
                     0, 0, 0);

            }
            if (AddressBalls[5] != null)
            {
                AddressBalls[5].MoveBall(PositionBalls.Ball_5,
                    PositionBalls.Ball_5_R,
                    PositionBalls.Ball_5_velocity,
                     0, 0, 0);

            }
            if (AddressBalls[6] != null)
            {
                AddressBalls[6].MoveBall(PositionBalls.Ball_6,
                    PositionBalls.Ball_6_R,
                    PositionBalls.Ball_6_velocity,
                     0, 0, 0);
 
            }
            if (AddressBalls[7] != null)
            {
                AddressBalls[7].MoveBall(PositionBalls.Ball_7,
                    PositionBalls.Ball_7_R,
                    PositionBalls.Ball_7_velocity,
                     0, 0, 0);

            }
            if (AddressBalls[8] != null)
            {
                AddressBalls[8].MoveBall(PositionBalls.Ball_8,
                    PositionBalls.Ball_8_R,
                    PositionBalls.Ball_8_velocity,
                     0, 0, 0);
 
            }
            if (AddressBalls[9] != null)
            {
                AddressBalls[9].MoveBall(PositionBalls.Ball_9,
                    PositionBalls.Ball_9_R,
                    PositionBalls.Ball_9_velocity,
                     0, 0, 0);

            }
            if (AddressBalls[10] != null)
            {
                AddressBalls[10].MoveBall(PositionBalls.Ball_10,
                    PositionBalls.Ball_10_R,
                    PositionBalls.Ball_10_velocity,
                     0, 0, 0);

            }
            if (AddressBalls[11] != null)
            {
                AddressBalls[11].MoveBall(PositionBalls.Ball_11,
                    PositionBalls.Ball_11_R,
                    PositionBalls.Ball_11_velocity,
                     0, 0, 0);

            }
            if (AddressBalls[12] != null)
            {
                AddressBalls[12].MoveBall(PositionBalls.Ball_12,
                    PositionBalls.Ball_12_R,
                    PositionBalls.Ball_12_velocity,
                     0, 0, 0);

            }
            if (AddressBalls[13] != null)
            {
                AddressBalls[13].MoveBall(PositionBalls.Ball_13,
                    PositionBalls.Ball_13_R,
                    PositionBalls.Ball_13_velocity,
                     0, 0, 0);

            }
            if (AddressBalls[14] != null)
            {
                AddressBalls[14].MoveBall(PositionBalls.Ball_14,
                    PositionBalls.Ball_14_R,
                    PositionBalls.Ball_14_velocity,
                     0, 0, 0);

            }
            if (AddressBalls[15] != null)
            {
                AddressBalls[15].MoveBall(PositionBalls.Ball_15,
                    PositionBalls.Ball_15_R,
                    PositionBalls.Ball_15_velocity,
                     0, 0, 0);

            }
            #endregion
            yield return new WaitForSecondsRealtime(0.001f);


            for (int i = 0; i < AddressBalls.Count; i++)
            {
                if (AddressBalls[i] != null)
                    AddressBalls[i].StopMoving();
            }
            yield return new WaitForSecondsRealtime(0.001f);
            CheckPitok(gameData.pitok, Vec3Helper.ToVector3(gameData.positions.CueBall));
            KinimaticBalls(false);
            intergateplayposition = 0;
            QueuePositionsBallFromServer.Clear();
            cueball.DragIsBusy = false;
            yield return null;
        }

        public IEnumerator CueBallPositionRecivedFromServer()
        {
            // AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>().ActiveAimSystemOnPlayRecord(false);
            var PositionBall = QueueCueBallPositionFromServer.Dequeue();
            // AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>().ActiveAimSystemForShowInOtherClient(false);
            AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>().CueBallMoveFromServer(PositionBall, 0.02f);
            /// Debug.Log("Recive2");
            yield return null;
        }

        public IEnumerator AimRecivedFromServer()
        {
            if (AddressBalls[0] != null)
            {
                var cueball = AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>();
                //cueball.ActiveAimSystemForShowInOtherClient(true);
                var aimdata = QueueAimFromServer.Dequeue();

                cueball.SetCueWoodPositionAndRotationFromServer(aimdata);
                // cueball.transform.position = aimdata.PosCueBall;
                /// Debug.Log(aimdata);
                yield return null;

            }
        }
        public void KinimaticBalls(bool active)
        {
            if (active)
            {
                for (int i = 0; i < AddressBalls.Count; i++)
                {
                    if (AddressBalls[i] != null)
                    {

                        AddressBalls[i].GetComponent<Rigidbody>().useGravity = false;
                        AddressBalls[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                        AddressBalls[i].GetComponent<Rigidbody>().isKinematic = true;
                        AddressBalls[i].GetComponent<SphereCollider>().enabled = false;
                    }

                }
            }
            else
            {
                for (int i = 0; i < AddressBalls.Count; i++)
                {
                    if (AddressBalls[i] != null)
                    {
                        AddressBalls[i].GetComponent<SphereCollider>().enabled = true;
                        AddressBalls[i].GetComponent<Rigidbody>().useGravity = true;
                        AddressBalls[i].GetComponent<Rigidbody>().isKinematic = false;

                        AddressBalls[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

                    }
                }
            }
        }
        private bool Infunc = false;
        // private GameObject finded_ball;
        public IEnumerator SpwanBallInBasketAndDestroyBallInTable(Diaco.EightBall.Structs.GameData data)
        {
            var list_object_in_basket = FindObjectsOfType<ballinbasket>().ToList();
            yield return new WaitForSecondsRealtime(0.1f);

            for (int i = 0; i < Basket.ballinbasket.Count; i++)
            {
                bool find = false;
                var ballID = Basket.ballinbasket[i];
                for (int j = 0; j < list_object_in_basket.Count; j++)
                {
                    if (list_object_in_basket[j].BallID == ballID)
                    {
                        Debug.Log("AAAAAA" + list_object_in_basket[j].BallID);
                        find = true;

                    }

                }
                if (!find)
                {
                    Basket.ballinbasket.Remove(ballID);
                }

            }

            yield return new WaitForSecondsRealtime(0.1f);
            for (int i = 0; i < data.deletedBalls.Count; i++)
            {

                if (!Basket.ballinbasket.Contains(data.deletedBalls[i]))
                {
                    try
                    {
                        Basket.AddToQueue(data.deletedBalls[i]);
                        BallInBasket.Add(data.deletedBalls[i]);

                        Destroy(AddressBalls[data.deletedBalls[i]].gameObject);
                        DeletedBallCount++;
                    }
                    catch (Exception e)
                    {

                    }
                }
            }
            /// Debug.Log("basket length : " + BallInBasket.Count);
            yield return new WaitForSecondsRealtime(0.3f);

            for (int i = 0; i < Basket.ballinbasket.Count; i++)///check for Wrong Ball In Basket
            {
                var id = Basket.ballinbasket[i];
                if (!data.deletedBalls.Contains(id))
                {
                    var ball = Instantiate(BallsPrefabs[id - 1], new Vector3(0.0f, 0.08885605f, 0.0f), Quaternion.identity, ParentForspwan);
                    AddressBalls[id] = ball.GetComponent<AddressBall>();
                    AddressBalls[id].GetComponent<Rigidbody>().isKinematic = true;
                    AddressBalls[id].GetComponent<Rigidbody>().useGravity = false;
                    AddressBalls[id].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                    AddressBalls[id].GetComponent<SphereCollider>().enabled = false;
                    var ball_in_basket = FindObjectsOfType<ballinbasket>();
                    for (int j = 0; j < ball_in_basket.Length; j++)
                    {
                        //   Debug.Log("bas__:" + id);
                        if (ball_in_basket[j].BallID == id)
                        {
                            Destroy(ball_in_basket[j].gameObject);
                            Basket.ballinbasket.Remove(id);
                            i = -1;

                        }
                    }

                }
                // Debug.Log("VBBBVBVBVBVBVBVBVBVBV"+id);
            }
            yield return new WaitForSecondsRealtime(0.5f);
            StartCoroutine(Basket.ExtractBallFast());
        }

        public bool SpawnBallWorng(GameData gameData)
        {
            bool findworng = false;
            for (int i = 0; i < AddressBalls.Count; i++)
            {
                if (AddressBalls[i] != null)
                {
                    var idball = AddressBalls[i].IDPost;
                    if (gameData.deletedBalls.Contains(idball))
                    {
                        Debug.Log("............" + idball);
                        findworng = true;
                    }
                }
            }

            return findworng;
        }
        public IEnumerator DeleteAdditionsBallInTable(GameData data)
        {
            //List<AddressBall> mustbedelete = new List<AddressBall>();
            var BallInTable = FindObjectsOfType<AddressBall>();
            //List<int> mustbedelete_ids = new List<int>();
            bool need_reset = false;
            for (int i = 0; i < BallInTable.Length; i++)
            {
                if (!need_reset)
                {
                    var ID = BallInTable[i].IDPost;
                    for (int j = 0; j < BallInTable.Length; j++)
                    {
                        var ID2 = BallInTable[j].IDPost;
                        if (i != j && ID == ID2)
                        {
                            need_reset = true;
                            Debug.Log("..............ResetBallAdd33333");
                        }
                    }
                }
            }
           
            if (!need_reset)
            {
                if(SpawnBallWorng(data))
                {
                    need_reset = true;
                    Debug.Log("..............ResetBallAdd2222");
                }
            }

            if (!need_reset)
            {
                if((16-data.deletedBalls.Count) != BallInTable.Length)
                {
                    need_reset = true;
                }
            }
            if (need_reset)
            {
                for (int i = 0; i < BallInTable.Length; i++)
                {
                    if (BallInTable[i].tag != "whiteball")
                    {
                        Debug.Log("..............count ball in table"+BallInTable.Length);
                        Destroy(BallInTable[i].gameObject);
                    }

                }
                AddressBalls.Clear();
                Debug.Log("..............ResetBallAdd");
                SpawnBalls2(data);
            }
            yield return null;
        }
      
        public IEnumerator CheckBallInBasket(Diaco.EightBall.Structs.GameData data)

        {
            //Check For Worng 
            //  bool findworng = false;
            var temp_deletedball = data.deletedBalls;
            var list_object_in_basket = FindObjectsOfType<ballinbasket>().ToList();

            bool need_reset = false;
            for (int i = 0; i < list_object_in_basket.Count; i++)
            {
                if (!need_reset)
                {
                    var ID = list_object_in_basket[i].BallID;
                    for (int j = 0; j < list_object_in_basket.Count; j++)
                    {
                        var ID2 = list_object_in_basket[j].BallID;
                        if (i != j && ID == ID2)
                        {
                            need_reset = true;
                        }
                    }
                }
            }

            if (!need_reset)
            {
                for (int i = 0; i < list_object_in_basket.Count; i++)
                {
                    var ballID = list_object_in_basket[i].BallID;

                    if (!temp_deletedball.Contains(ballID))
                    {
                        need_reset = true;
                        Debug.Log("Find Worng this id: " + ballID);

                    }
                }

            }

            if (!need_reset)
            {
                if(data.deletedBalls.Count != list_object_in_basket.Count)
                {
                    need_reset = true;
                }
            }
            //yield return new WaitForSecondsRealtime(0.1f);
            if (need_reset)
            {
                Debug.Log("Start solving the problem in basket");
                for (int i = 0; i < list_object_in_basket.Count; i++)
                {
                    Destroy(list_object_in_basket[i].gameObject);
                    this.Basket.QueueBasket.Clear();
                    this.Basket.ballinbasket.Clear();
                    Debug.Log("DESTROY BALL IN BASKET");
                }
               // yield return new WaitForSecondsRealtime(0.1f);
                for (int i = 0; i < data.deletedBalls.Count; i++)
                {

                    var deletedballID = data.deletedBalls[i];
                    Debug.Log("ADD BALL IN BASKET:::   " + deletedballID);
                    this.Basket.AddToQueue(deletedballID);


                }
                Debug.Log("The problem was solved in basket.");
                ///yield return new WaitForSecondsRealtime(0.1f);
                StartCoroutine(this.Basket.ExtractBallFast());
                
            }
            
            yield return null;
        }

        public void CloseConnection()
        {
            socket.Off();
            socket.Manager.Close();
            socket.Disconnect();
            Debug.Log("BilliardCloseConnection");
        }
        public IEnumerator ResetData()
        {
            SpwnedBall = false;
            // UserName = new UserInfo();
            // gameData = new GameData();
            PlayerShar = Shar.None;
            Turn = false;
            // Record = false;
            FirstBallImpact = 0;
            Pitok = 0;
            EightBallEnableLeftShar = false;

            // SetUserNameInBillboard("", "");
            // ResetSharBillboard();
            yield return new WaitForSeconds(00.1f);
            var ballinbasket = FindObjectsOfType<ballinbasket>();
            for (int i = 0; i < ballinbasket.Length; i++)
            {
                if (ballinbasket[i] != null)
                {
                    Destroy(ballinbasket[i].gameObject);
                }
            }
            yield return new WaitForSeconds(00.1f);
            for (int i = 0; i < AddressBalls.Count; i++)
            {
                if (AddressBalls[i] != null && i > 0)
                {
                    Destroy(AddressBalls[i].gameObject);
                    //  Debug.Log(AddressBalls[i].gameObject.name);
                }

            }
            yield return new WaitForSeconds(00.1f);

            PocketedBallsID.Clear();
            BallInBasket.Clear();
            IDImpactToWall.Clear();
            AddressBalls.Clear();
            Basket.clearbasket();
            yield return new WaitForSeconds(00.1f);
        }

        public void IntiGameData()
        {

            gameData = new GameData();
        }
        public void SpawnBalls(GameData gameData)
        {
            Vector3[] positions = new Vector3[16] {

                new Vector3(gameData.positions.CueBall.x,0.08885605f,gameData.positions.CueBall.z),
                new Vector3(gameData.positions.Ball_1.x,0.08885605f,gameData.positions.Ball_1.z),
                new Vector3(gameData.positions.Ball_2.x,0.08885605f,gameData.positions.Ball_2.z),
                new Vector3(gameData.positions.Ball_3.x,0.08885605f,gameData.positions.Ball_3.z),
                new Vector3(gameData.positions.Ball_4.x,0.08885605f,gameData.positions.Ball_4.z),
                new Vector3(gameData.positions.Ball_5.x,0.08885605f,gameData.positions.Ball_5.z),
                new Vector3(gameData.positions.Ball_6.x,0.08885605f,gameData.positions.Ball_6.z),
                new Vector3(gameData.positions.Ball_7.x,0.08885605f,gameData.positions.Ball_7.z),
                new Vector3(gameData.positions.Ball_8.x,0.08885605f,gameData.positions.Ball_8.z),
                new Vector3(gameData.positions.Ball_9.x,0.08885605f,gameData.positions.Ball_9.z),
                new Vector3(gameData.positions.Ball_10.x,0.08885605f,gameData.positions.Ball_10.z),
                new Vector3(gameData.positions.Ball_11.x,0.08885605f,gameData.positions.Ball_11.z),
                new Vector3(gameData.positions.Ball_12.x,0.08885605f,gameData.positions.Ball_12.z),
                new Vector3(gameData.positions.Ball_13.x,0.08885605f,gameData.positions.Ball_13.z),
                new Vector3(gameData.positions.Ball_14.x,0.08885605f,gameData.positions.Ball_14.z),
                new Vector3(gameData.positions.Ball_15.x,0.08885605f,gameData.positions.Ball_15.z),

            };
            var cueball = FindObjectOfType<Diaco.EightBall.CueControllers.HitBallController>();
            cueball.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);

            cueball.transform.position = positions[0];
            AddressBalls.Add(cueball.GetComponent<AddressBall>());


            for (int i = 0; i < BallsPrefabs.Count; i++)
            {
                if (!gameData.deletedBalls.Contains(i + 1))
                {
                    var ball = Instantiate(BallsPrefabs[i], positions[i + 1], Quaternion.identity, ParentForspwan);
                    //ball.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
                    ball.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
                    AddressBalls.Add(ball.GetComponent<AddressBall>());
                    // Debug.Log("BALLLLLttttt::::::::::::");

                }
                else
                {
                    AddressBalls.Add(null);
                }
            }
            

            soundeffectcontroll.PlaySound(2);////  play arrange  ball;
            SpwnedBall = true;
        }
        public void SpawnBalls2(GameData gameData)
        {
            Vector3[] positions = new Vector3[16] {

                new Vector3(gameData.positions.CueBall.x,0.08885605f,gameData.positions.CueBall.z),
                new Vector3(gameData.positions.Ball_1.x,0.08885605f,gameData.positions.Ball_1.z),
                new Vector3(gameData.positions.Ball_2.x,0.08885605f,gameData.positions.Ball_2.z),
                new Vector3(gameData.positions.Ball_3.x,0.08885605f,gameData.positions.Ball_3.z),
                new Vector3(gameData.positions.Ball_4.x,0.08885605f,gameData.positions.Ball_4.z),
                new Vector3(gameData.positions.Ball_5.x,0.08885605f,gameData.positions.Ball_5.z),
                new Vector3(gameData.positions.Ball_6.x,0.08885605f,gameData.positions.Ball_6.z),
                new Vector3(gameData.positions.Ball_7.x,0.08885605f,gameData.positions.Ball_7.z),
                new Vector3(gameData.positions.Ball_8.x,0.08885605f,gameData.positions.Ball_8.z),
                new Vector3(gameData.positions.Ball_9.x,0.08885605f,gameData.positions.Ball_9.z),
                new Vector3(gameData.positions.Ball_10.x,0.08885605f,gameData.positions.Ball_10.z),
                new Vector3(gameData.positions.Ball_11.x,0.08885605f,gameData.positions.Ball_11.z),
                new Vector3(gameData.positions.Ball_12.x,0.08885605f,gameData.positions.Ball_12.z),
                new Vector3(gameData.positions.Ball_13.x,0.08885605f,gameData.positions.Ball_13.z),
                new Vector3(gameData.positions.Ball_14.x,0.08885605f,gameData.positions.Ball_14.z),
                new Vector3(gameData.positions.Ball_15.x,0.08885605f,gameData.positions.Ball_15.z),

            };
            var cueball = FindObjectOfType<Diaco.EightBall.CueControllers.HitBallController>();
            cueball.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);

            cueball.transform.position = positions[0];
            AddressBalls.Add(cueball.GetComponent<AddressBall>());

            for (int i = 0; i < BallsPrefabs.Count; i++)
            {
                if (!gameData.deletedBalls.Contains(i + 1))
                {
                    var ball = Instantiate(BallsPrefabs[i], positions[i + 1], Quaternion.identity, ParentForspwan);
                    //ball.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
                    ball.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
                    AddressBalls.Add(ball.GetComponent<AddressBall>());
                    // Debug.Log("BALLLLLttttt::::::::::::");

                }
                else
                {
                    AddressBalls.Add(null);
                }
            }


            // soundeffectcontroll.PlaySound(2);////  play arrange  ball;
            SpwnedBall = true;
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

        public void AddBallToBasket(int id)
        {
            if (!BallInBasket.Contains(id))
                BallInBasket.Add(id);
        }
        public void SelectTable(string name)
        {

            // Debug.Log("e.nsddsasdame"+name);
            Tables.ForEach(e => {

                if (e.name == name)
                {
                    TableRenderer.material = e.table;
                    //Debug.Log(e.name);
                    if (name == "classic")
                        GamePlayRule = _GamePlayRule.classic;
                    else if (name == "quick" || name == "competition")
                        GamePlayRule = _GamePlayRule.quick;
                    else if (name == "big")
                        GamePlayRule = _GamePlayRule.big;
                }


                else if (e.name == "")
                {
                    TableRenderer.material = Tables[0].table;
                    //  Debug.Log(e.name);
                }

            });

        }
        public IEnumerator ShowResualtPage(object[] m)
        {
            Time.timeScale = 2;
            ResultGamePage.SetActive(true);


            var result = JsonUtility.FromJson<Diaco.EightBall.Structs.ResultGame>(m[0].ToString());
            bool PlayAgianActive = Convert.ToBoolean(m[1]); ;/// Enable Button Play Again
            Handler_OnGameResult(result, PlayAgianActive);
            soundeffectcontroll.PlaySound(3);////  play resault sound;
            yield return new WaitForSeconds(3);
            soundeffectcontroll.PlaySound(4);////  play resault sound;
            Debug.Log("GameResult");
        }




        private void SetTimePlayerInUI(float playerOneTime, float playerTwoTime)
        {

            float H1 = 0;
            float M1 = 0;
            float S1 = 0;

            float H2 = 0;
            float M2 = 0;
            float S2 = 0;

            H1 = (float)Math.Floor(playerOneTime / 3600);
            M1 = (float)Math.Floor(playerOneTime / 60 % 60);
            S1 = (float)Math.Floor(playerOneTime % 60);

            H2 = (float)Math.Floor(playerTwoTime / 3600);
            M2 = (float)Math.Floor(playerTwoTime / 60 % 60);
            S2 = (float)Math.Floor(playerTwoTime % 60);



            if (gameData.playerOne.userName == UserName.userName)
            {
                PlayerTimeLeft.text = M1 + ":" + S1;
                PlayerTimeRight.text = M2 + ":" + S2;
            }
            else
            {
                PlayerTimeLeft.text = M2 + ":" + S2;
                PlayerTimeRight.text = M1 + ":" + S1;
            }
        }
        #endregion
        #region IN RECORD MODE
        /// <summary>
        /// /Y = 0.1060765
        /// </summary>
        /// <param name="whiteball"></param>
        /// <param name="colorball"></param>
        /// <param name="siblpos"></param>
        private void SpawnAssetInRecordMode(Vector3 whiteball, Vector3 colorball, Vector3 siblpos)
        {
            var w_ball = FindObjectOfType<Diaco.EightBall.CueControllers.HitBallController>();
            w_ball.GetComponent<Collider>().enabled = true;

            w_ball.GetComponent<Rigidbody>().isKinematic = false;
            w_ball.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

            w_ball.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
            w_ball.transform.position = whiteball;
            w_ball.DragIsBusy = false;
            w_ball.InMove = false;

            //var rand = UnityEngine.Random.Range(1, BallsPrefabs.Count];
            var c_ball = Instantiate(BallsPrefabs[1], colorball, Quaternion.identity, ParentForspwan);
            var sibl = Instantiate(SiblPrefab, siblpos, Quaternion.identity, ParentForspwan);
            AddressBalls.Add(w_ball.GetComponent<AddressBall>());
            AddressBalls.Add(c_ball.GetComponent<AddressBall>());
            Sibl = sibl.GetComponent<Sibl>();

        }
        public IEnumerator CheckMovementAndSendDataInRecordMode()
        {
            Turn = false;

            do
            {
                /// Debug.Log("MoveInRecordMode");
                yield return new WaitForSeconds(0.2f);
            }
            while (CheckBallsMove());

            Emit_EndTurnInRecordMode();
            yield return null;
        }
        private void Emit_EndTurnInRecordMode()
        {
            if (Sibl.Area != 4)
                soundeffectcontroll.PlaySound(5);///sib sound
            socket.Emit("EndTurn", Sibl.Area);
            Debug.Log($"<color=blue><b>EndTurn</b>{Sibl.Area}</color>");
        }
        public void ClearSceneInRecordMode()
        {
            if (AddressBalls.Count > 0)
            {
                ///  Destroy(AddressBalls[0].gameObject);
                if (AddressBalls[1] != null)
                    Destroy(AddressBalls[1].gameObject);
                Destroy(Sibl.gameObject);
                this.Sibl = null;
                AddressBalls.Clear();
            }
        }
        #endregion
        #endregion

        #region UIFunction
        private void SetUserNameInBillboard(string userleft, string userright)
        {
            UserNameIndicator[0].text = userleft;
            UserNameIndicator[1].text = userright;
        }
        private void SetSharForPlayers(Diaco.EightBall.Structs.Shar typeofleftshar, Diaco.EightBall.Structs.Shar typeofrightshar, int player_shar)
        {
            for (int i = 0; i < 7; i++)
            {
                if (typeofleftshar == 0)
                {
                    UI_Biliboard_SharLeft[i].image.sprite = AssetsUI_Shar_Solid.Shars[i].Shar;
                    UI_Biliboard_SharLeft[i].GetComponent<ui_shar>().IDShar = AssetsUI_Shar_Solid.Shars[i].ID;

                    //   gamemanager.PlayerShar = CustomType.Shar.Solid;
                }
                else
                {
                    UI_Biliboard_SharLeft[i].image.sprite = AssetsUI_Shar_Stripe.Shars[i].Shar;
                    UI_Biliboard_SharLeft[i].GetComponent<ui_shar>().IDShar = AssetsUI_Shar_Stripe.Shars[i].ID;
                    //   gamemanager.PlayerShar = CustomType.Shar.Stripe;
                }
                if (typeofrightshar == 0)
                {
                    UI_Biliboard_SharRight[i].image.sprite = AssetsUI_Shar_Solid.Shars[i].Shar;
                    UI_Biliboard_SharRight[i].GetComponent<ui_shar>().IDShar = AssetsUI_Shar_Solid.Shars[i].ID;
                    //  gamemanager.PlayerShar = CustomType.Shar.Solid;
                }
                else
                {
                    UI_Biliboard_SharRight[i].image.sprite = AssetsUI_Shar_Stripe.Shars[i].Shar;
                    UI_Biliboard_SharRight[i].GetComponent<ui_shar>().IDShar = AssetsUI_Shar_Stripe.Shars[i].ID;
                    //  gamemanager.PlayerShar = CustomType.Shar.Stripe;
                }
            }
            if (player_shar == 0)
            {
                PlayerShar = Diaco.EightBall.Structs.Shar.Solid;
            }
            else if (player_shar == 1)
            {
                PlayerShar = Diaco.EightBall.Structs.Shar.Stripe;
            }

        }
        private void ResetSharBillboard()
        {
            for (int i = 0; i < 7; i++)
            {
                UI_Biliboard_SharLeft[i].image.sprite = DisableShar_sprite;
                UI_Biliboard_SharLeft[i].GetComponent<ui_shar>().IDShar = -1;
                UI_Biliboard_SharLeft[i].interactable = true;

                UI_Biliboard_SharRight[i].image.sprite = DisableShar_sprite;
                UI_Biliboard_SharRight[i].GetComponent<ui_shar>().IDShar = -1;
                UI_Biliboard_SharRight[i].interactable = true;
            }
            PlayerShar = Shar.None;
        }
        public void SetDisableSharInBiliboard(List<int> Sharid)
        {
            var shars = FindObjectsOfType<ui_shar>();

            for (int i = 0; i < shars.Length; i++)
            {
                for (int j = 0; j < Sharid.Count; j++)
                {
                    if (shars[i].IDShar == Sharid[j])
                    {
                        shars[i].GetComponent<Button>().interactable = false;
                        // Debug.Log("Shar:::"+ shars[i].IDShar);
                    }
                }

            }
        }

        public void DisableAllSharInBiliboard(int Sharid)
        {
            var shars = FindObjectsOfType<ui_shar>();
            foreach (ui_shar shar in shars)
            {

                if (shar.IDShar == Sharid)
                {
                    shar.GetComponent<Button>().interactable = false;
                }

            }
        }
        public void EnableSharInBiliboard()
        {
            var shars = FindObjectsOfType<ui_shar>();
            foreach (ui_shar shar in shars)
            {

                shar.GetComponent<Button>().interactable = true;


            }
        }
        private void UpdateAvatarProfile(Sprite pic1, Sprite pic2)
        {


            ProfileImage[0].sprite = pic1;


            ProfileImage[1].sprite = pic2;

        }


        private void EnableCoolDown(Diaco.EightBall.Structs.Side side, float Time, float totaltime)
        {

            float t = Time / totaltime;
            if (side == Side.Left)
            {
               
                PlayerCoolDowns[0].fillAmount = t;

                PlayerCoolDowns[1].fillAmount = 1.0f;
               Debug.Log(Time+"....."+totaltime+"L_R : , R_S"+t);
            }
            else if (side == Side.Right)
            {
                PlayerCoolDowns[0].fillAmount = 1.0f;
                PlayerCoolDowns[1].fillAmount = t;
                Debug.Log(Time + "....." + totaltime + "R_R, L_S" +t);
            }
            
            
            Timer = Time / 1000;

            // float fill = Time / totaltime;
            ////  Debug.Log("Time" + Timer);

            if (side == 0)
            {
                // PlayerCoolDowns[0].fillAmount = fill;
                InvokeRepeating("SetLeftCoolDown", 0.0f, 0.01f * UnityEngine.Time.timeScale);

                //Debug.Log("EnableCooldownLeft");
            }
            else
            {
                //  PlayerCoolDowns[0].fillAmount = fill;
                InvokeRepeating("SetRightCoolDown", 0.0f, 0.01f * UnityEngine.Time.timeScale);
                /// Debug.Log("EnableCooldownRight");
            }
            // Debug.Log("CoolDown");
        }
        private void SetLeftCoolDown()
        {

            var unit_time = 1.0f / Timer;
            var fill = PlayerCoolDowns[0].fillAmount - unit_time / 100;
            PlayerCoolDowns[0].fillAmount = fill;
            // Debug.Log("EnableCooldownLeft...");
            if (PlayerCoolDowns[0].fillAmount < 0.01f)
            {
                PlayerCoolDowns[0].fillAmount = 1;
                CancelInvoke("SetLeftCoolDown");

                Turn = false;
                //Pitok = 0;
                //   Debug.Log("CancleCooldownLeft...");
            }
        }
        private void SetRightCoolDown()
        {

            var unit_time = 1.0f / Timer;
            var fill = PlayerCoolDowns[1].fillAmount - unit_time / 100;
            PlayerCoolDowns[1].fillAmount = fill;
            ///  Debug.Log("EnableCooldownRight...");
            if (PlayerCoolDowns[1].fillAmount < 0.01f)
            {
                PlayerCoolDowns[1].fillAmount = 1;
                CancelInvoke("SetRightCoolDown");

                //   Debug.Log("CancleCooldownLeft...");
            }
        }
        private void CancelCoolDownTimer()
        {
            CancelInvoke("SetLeftCoolDown");
            CancelInvoke("SetRightCoolDown");

        }
        private void SetCountCostBillboard(string coin)
        {
            TotalCoin.text = coin;
        }
        private void SetTypeCost(int cost)
        {
            if (cost == 0)///cup
            {
                CostTypeIndicator.sprite = Cup_sprite;
                ///  Debug.Log("Cup");
            }
            else if (cost == 1)//coin
            {

                CostTypeIndicator.sprite = Coin_sprite;
                //  Debug.Log("Coin");
            }

            else if (cost == 2)//gem
            {
                CostTypeIndicator.sprite = Gem_sprite;
                //Debug.Log("Gem");
            }

        }
        public void StickerViwer(object namesticker, object side)
        {
            string name = Convert.ToString(namesticker);
            int sideshow = Convert.ToInt32(side);///0 left, 1 right
            if (sideshow == 0)
            {
                StickerViwerLeft.SelectSticker(name);
                StickerViwerLeft.gameObject.SetActive(true);
            }
            else
            {
                StickerViwerRight.SelectSticker(name);
                StickerViwerRight.gameObject.SetActive(true);
            }
        }
        private void SetUIInRecordMode(int totalpoint, string level, int time, int[] pointsibl)
        {
            UIControllInRecordMode.SetTotalPoint(totalpoint);
            UIControllInRecordMode.SetLevel(level);
            UIControllInRecordMode.SetPointSibl(pointsibl);
            CalculateTime(time);

        }
        private void CalculateTime(float time)
        {
            H = 0;
            M = 0;
            S = 0;
            CancelInvoke("RunTimer");


            H = (float)Math.Floor(time / 3600);
            M = (float)Math.Floor(time / 60 % 60);
            S = (float)Math.Floor(time % 60);
            InvokeRepeating("RunTimer", 0, 2.0f);
        }

        private void RunTimer()
        {
            S--;
            if (S < 0)
            {
                if (M > 0 || H > 0)
                {
                    S = 59;
                    M--;
                    if (M < 0)
                    {
                        if (H > 0)
                        {
                            M = 59;
                            H--;
                        }
                        else
                        {
                            M = 0;
                        }
                    }

                }
                else
                {
                    S = 0;
                }
            }


            UIControllInRecordMode.SetTimer(M + ":" + S);
            if (S == 0 && M == 0 && H == 0)
            {
                CancelInvoke("RunTimer");


            }
        }

        public void BadConnectionShow(bool show)
        {

            im_BadConnection.SetActive(show);


        }
        #endregion

        #region BilliardGameRulesFunction
        public void CheckEnable8Ball()
        {

            int count = 0;
            for (int i = 0; i < UI_Biliboard_SharLeft.Count; i++)
            {

                if (!UI_Biliboard_SharLeft[i].interactable)
                {
                    count++;
                }
            }
            if (count == 7)
            {
                EightBallEnableLeftShar = true;
                UI_Biliboard_SharLeft[0].interactable = true;
                UI_Biliboard_SharLeft[0].image.sprite = EightBall;
                //  Debug.Log("EightBallEnable");
            }


        }
        public void CheckEnable8BallRightInOtherClient()
        {

            int count = 0;
            for (int i = 0; i < UI_Biliboard_SharRight.Count; i++)
            {

                if (!UI_Biliboard_SharRight[i].interactable)
                {
                    count++;
                }
            }
            if (count == 7)
            {
                EightBallEnableRightShar = true;
                UI_Biliboard_SharRight[0].interactable = true;
                UI_Biliboard_SharRight[0].image.sprite = EightBall;
                //  Debug.Log("EightBallEnable");
            }


        }
        public void CheckPitok(int pitok, Vector3 pos)
        {
            if (pitok == 1)
            {
                var lastpos = new Vector3(pos.x, 0.08885605f, pos.z);

                NormalPitok(lastpos);
            }
            else if (pitok == 2)
            {
                var lastpos = new Vector3(pos.x, 0.08885605f, pos.z);
                Pitok1_3(lastpos);
            }
            else if (pitok == 0)
            {
                PitokOff();

            }
        }
        public void NormalPitok(Vector3 lastpos)
        {
            // DOVirtual.Float(0, 1, 0.2f, (x) => { }).OnComplete(() =>
            /// {
            Pitok = 1;
            var cueball = AddressBalls[0];

            cueball.GetComponent<Collider>().enabled = true;
            //cueball.gameObject.SetActive(true); 
            var ooo = new Vector3(lastpos.x, lastpos.y, lastpos.z);

            cueball.transform.position = ooo;

            cueball.transform.DOScale(0.33f, 0.1f);
            cueball.GetComponent<Rigidbody>().isKinematic = false;
            cueball.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            cueball.GetComponent<Rigidbody>().WakeUp();
            cueball.GetComponent<Diaco.EightBall.CueControllers.HitBallController>().EnableYFix = true;
           cueball.GetComponent<ShodowFake>().shadow.gameObject.SetActive(true);
            // Debug.Log("Pitok1" + ooo);

            //  });

        }
        public void Pitok1_3(Vector3 lastpos)
        {
            // DOVirtual.Float(0, 1, 0.2f, (x) => { }).OnComplete(() =>
            //  {

            Pitok = 2;
            var cueball = AddressBalls[0];
            cueball.GetComponent<Collider>().enabled = true;
            // cueball.gameObject.SetActive(true);
            var ooo = new Vector3(lastpos.x, lastpos.y, lastpos.z);
            cueball.transform.position = ooo;
            /// cueball.GetComponent<Diaco.EightBall.CueControllers.HitBallController>().CUEWoodSetPosition(Vector3.zero);
            cueball.transform.DOScale(0.33f, 0.1f);
            cueball.GetComponent<Rigidbody>().isKinematic = false;
            cueball.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            cueball.GetComponent<Rigidbody>().WakeUp();
            cueball.GetComponent<Diaco.EightBall.CueControllers.HitBallController>().EnableYFix = true;
           cueball.GetComponent<ShodowFake>().shadow.gameObject.SetActive(true);
            LimitPitokEffect();
            //  Debug.Log("Pitok2:1/3" + ooo);
            //// });

        }
        public void LimitPitokEffect()
        {
            var color = AllowAreaForMoveCueBallRenderer.color;
            DOVirtual.Float(0.0f, 1.0f, 2f, alpha =>
            {
                Color c = new Color(color.r, color.g, color.b, alpha);
                AllowAreaForMoveCueBallRenderer.color = c;
            }).OnComplete(() =>
            {
                DOVirtual.Float(1.0f, 0.0f, 2f, alpha =>
                {
                    var color2 = AllowAreaForMoveCueBallRenderer.color;
                    Color c = new Color(color2.r, color2.g, color2.b, alpha);
                    AllowAreaForMoveCueBallRenderer.color = c;
                });
            });
        }
        public void PitokOff()
        {
            // DOVirtual.Float(0, 1, 0.2f, (x) => { }).OnComplete(() =>
            //{

            Pitok = 0;
            var cueball = AddressBalls[0];
            cueball.GetComponent<Diaco.EightBall.CueControllers.HitBallController>().LimitedMovePitok = false;
            ///  Debug.Log("PitokOff");
            //  });

        }
        public void ClearPocketedBallList()
        {
            PocketedBallsID.Clear();
        }
        public bool CheckPitok()

        {
            bool find = false;
            if (PocketedBallsID.Contains(0))
            {
                find = true;

            }
            return find;
        }
        public void FillImpactToWallList(int Id)
        {

            if (!IDImpactToWall.Contains(Id))
            {
                IDImpactToWall.Add(Id);
            }

        }
        public bool CheckBallForAllowHit(int id)
        {
            bool Allow = false;
            if (EightBallEnableLeftShar == false)
            {

                var type = -1;
                if (id > 0 && id < 8)
                {
                    type = 0;
                }
                else if (id > 8)
                {
                    type = 1;
                }
                if (PlayerShar != Diaco.EightBall.Structs.Shar.None)
                {
                    if (type == (int)PlayerShar)
                    {
                        Allow = true;
                        //   Debug.Log("Allow");
                    }
                    else
                    {
                        Allow = false;
                        //Debug.Log("No Allow");
                    }

                }
                else
                {
                    Allow = true;
                }
            }
            else if (EightBallEnableLeftShar == true && id == 8)
            {
                Allow = true;
            }
            return Allow;
        }

        #endregion

        #region Triggers
        /* private void GameManager_OnPocket5(int ID)
         {
             PocketedBallsID.Add(ID);
             //  Debug.Log("Pocket6");
         }

         private void GameManager_OnPocket4(int ID)
         {
             PocketedBallsID.Add(ID);
             //   Debug.Log("Pocket5");
         }

         private void GameManager_OnPocket3(int ID)
         {
             PocketedBallsID.Add(ID);
             /// Debug.Log("Pocket4");
         }

         private void GameManager_OnPocket2(int ID)
         {
             PocketedBallsID.Add(ID);
             ///  Debug.Log("Pocket3");
         }

         private void GameManager_OnPocket1(int ID)
         {
             PocketedBallsID.Add(ID);
             ///  Debug.Log("Pocket2");
         }*/

        private void GameManager_OnPocket0(int ID)
        {
            if (!PocketedBallsID.Contains(ID))
                PocketedBallsID.Add(ID);
            ///  Debug.Log("Pocket1");
        }

        #endregion

        #region Events ServerEvents

        public event Action<bool> OnTurn;

        protected virtual void Handler_OnTurn(bool t)
        {
            if (OnTurn != null)
            {
                OnTurn(t);

            }
        }
        public event Action<bool> OnPlay;
        protected virtual void Handler_OnPlay(bool t)
        {
            if (OnPlay != null)
            {
                OnPlay(t);
            }
        }
        public event Action<int> OnPitok;
        protected virtual void Handler_OnPitok(int value)
        {
            if (OnPitok != null)
            {
                OnPitok(value);
            }
        }

        public event Action<Diaco.EightBall.Structs.ResultGame, bool> OnGameResult;
        protected void Handler_OnGameResult(Diaco.EightBall.Structs.ResultGame result, bool playagin)
        {
            if (OnGameResult != null)
            {
                OnGameResult(result, playagin);
            }
        }

        private Action gameReady;
        public event Action GameReady
        {
            add
            {
                gameReady += value;
            }
            remove
            {
                gameReady -= value;
            }
        }
        protected void Handler_GameReady()
        {
            if (gameReady != null)
            {
                gameReady();
            }

        }

        private Action<Diaco.Store.Billiard.BilliardShopDatas> initshop;
        public event Action<Diaco.Store.Billiard.BilliardShopDatas> InitShop
        {
            add
            {
                initshop += value;
            }
            remove
            {
                initshop -= value;
            }
        }
        protected void Handler_InitShop(Diaco.Store.Billiard.BilliardShopDatas data)
        {
            if (initshop != null)
            {
                initshop(data);
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
        private Action<string, float> incomingmessage;
        public event Action<string, float> InComingMessage
        {
            add
            {
                incomingmessage += value;
            }
            remove
            {
                incomingmessage -= value;
            }
        }
        protected void Handler_IncomingMessage(string mess, float d)
        {
            if (incomingmessage != null)
            {
                incomingmessage(mess, d);
            }

        }

        private Action<bool, int> enableboarderpocket;
        public event Action<bool, int> EnableBoarderPocket
        {
            add
            {
                enableboarderpocket += value;
            }
            remove
            {
                enableboarderpocket -= value;
            }
        }
        protected void Handler_EnableBoarderPocket(bool show, int id)
        {
            if (enableboarderpocket != null)
            {
                enableboarderpocket(show, id);
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
        #endregion
    }
    [Serializable]
    public struct UIElements
    {
        public Text txt_TotalPoint;
        public Text txt_Level;
        public Text txt_Timer;
        public Text[] txt_SiblPoint;

        public void SetTotalPoint(int point)
        {


            txt_TotalPoint.text = point.ToString();
        }
        public void SetLevel(string level)
        {
            txt_Level.text = level;
        }
        public void SetTimer(string timer)
        {
            txt_Timer.text = timer;
        }
        public void SetPointSibl(int[] points)
        {
            for (int i = 0; i < points.Length; i++)
            {
                txt_SiblPoint[i].text = points[i].ToString();
            }
        }
    }
    [Serializable]
    public struct RecordModeGameData
    {
        public Vector3 siblPos;
        public Vector3 whiteballPos;
        public Vector3 colorballPos;
        public int timer;
        public int[] points;
        public string level;
        public int totalPoint;
    }
    [Serializable]

    public struct CueStateData
    {
        public string name;
        public float force;
        public float spin;
        public float aim;
    }
    [Serializable]
    public struct TABLE
    {
        public string name;
        public Material table;
    }

}