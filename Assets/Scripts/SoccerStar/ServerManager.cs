using System;
using System.IO;

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using BestHTTP.SocketIO;
using DG.Tweening;
using Diaco.HTTPBody;
using Diaco.SoccerStar.Marble;
using Diaco.SoccerStar.Goals;
using Sirenix.OdinInspector;

using Diaco.SoccerStar.CustomTypes;
namespace Diaco.SoccerStar.Server
{
    public class ServerManager : MonoBehaviour
    {
        public SoundEffectControll soundeffectcontrollLayer1;
        public SoundEffectControll soundeffectcontrollLayer2;
        public Transform ParentForSpawn;
        public bool MarblesInMove = false;
        public bool InRecordMode = false;
        public float TimeStep = 0.0f;
        public bool FreePlay = false;
        #region DataMemberGlobal
        [FoldoutGroup("ServerSettings")]
        public GameObject ResultGamePage;
        /// [FoldoutGroup("ServerSettings")]
        
        /// public string URLLocal;
        // [FoldoutGroup("ServerSettings")]
        // public string URLGlobal;
        [FoldoutGroup("ServerSettings")]
        public string NamespaceServer;
        [FoldoutGroup("ServerSettings")]
        public int SendRate = 100;
        [FoldoutGroup("ServerSettings")]
        public float TimeTurn = 10.0f;
        // [FoldoutGroup("ServerSettings")]
        // private float _temptime = 0.0f;
        [FoldoutGroup("ServerSettings")]
        private bool turn;
        [FoldoutGroup("ServerSettings")]

        public bool Turn
        {
            set
            {
                turn = value;

                handler_OnTurnChange(turn);
                // Debug.Log("GENERAlTURN");
                if (turn == true)
                {
                    /*     DOVirtual.Float(0, 1, 0.3f, x =>
                         {
                         }).OnComplete(() =>
                         {
                             Handler_OnPhysicFreeze(false);
                         });*/
                }


            }
            get
            {
                return turn;
            }
        }
        [FoldoutGroup("ServerSettings")]
        public bool EnablerRingEffect = false;
        [FoldoutGroup("ServerSettings")]
        public UserInfo Info;
        [FoldoutGroup("ServerSettings")]
        public GameData gameData;
        [FoldoutGroup("ServerSettings")]
        public bool SpwanedMarbels = false;

        [FoldoutGroup("ServerSettings")]
        public Diaco.ImageContainerTool.ImageContainer imageContainer;
        [FoldoutGroup("ServerSettings")]
        public Diaco.ImageContainerTool.ImageContainer MarbleSkins;
        //public bool Play { set; get; }
        [FoldoutGroup("ServerSettings")]
        public int MinRangMarblesId = 0;
        [FoldoutGroup("ServerSettings")]
        public int MaxRangMarblesId = 0;
        [FoldoutGroup("ServerSettings")]
        public SocketManager socket_manager;
        [FoldoutGroup("ServerSettings")]
        public Socket socket;
        [FoldoutGroup("ServerSettings")]
        public GameObject[] MarbleRegistered;
        [FoldoutGroup("ServerSettings")]
        public List<ForceToBall> Marbles;
        [FoldoutGroup("ServerSettings")]
        public List<ARENA> Arena;
        [FoldoutGroup("ServerSettings")]
        public List<FLAG> Flags;

        // [FoldoutGroup("ServerSettings")]
        //public List<Goal> Goals;
        [FoldoutGroup("ServerSettings")]
        public int MarbleInGoal = 0;
        [FoldoutGroup("ServerSettings")]
        public int IsGoal = -1;///SendWithDirection
        [FoldoutGroup("ServerSettings")]
        public float FrameRate;





        //[FoldoutGroup("ServerSettingsInRecordMode")]
        //public RecordModeGameData recordmodeGameData;
        [FoldoutGroup("ServerSettingsInRecordMode")]
        public Sibl SiblArea;
        [FoldoutGroup("ServerSettingsInRecordMode")]
        public Diaco.SoccerStar.Marble.ForceToBall BallPrefab;
        [FoldoutGroup("ServerSettingsInRecordMode")]
        public Diaco.SoccerStar.Marble.ForceToBall[] MarblesRegisteredForRecordMode;
        [FoldoutGroup("ServerSettingsInRecordMode")]
        public List<Diaco.SoccerStar.Marble.ForceToBall> MarblesInRecorodMode;
        [SerializeField]
        [FoldoutGroup("UIInRecordMode")]
        public UIElements UIControllInRecordMode;
        [FoldoutGroup("UIInRecordMode")]
        public ResultInRecordMode resualtInRecordMode;
        private float H;
        private float M;
        private float S;

        [FoldoutGroup("NetworkedUI")]
        public Button BlockChat_Button;
        [FoldoutGroup("NetworkedUI")]
        public List<Image> ImagePlayerIndicatorOnBiliboard;
        [FoldoutGroup("NetworkedUI")]
        public List<Text> GoalIndicatorOnBiliboard;
        [FoldoutGroup("NetworkedUI")]
        public List<RTLTMPro.RTLTextMeshPro> NameIndicatorOnBiliboard;
        [FoldoutGroup("NetworkedUI")]
        public Image CostTypeIndicator;
        [FoldoutGroup("NetworkedUI")]
        public Text CoinOrTimeIndicatorOnBiliboard;
        [FoldoutGroup("NetworkedUI")]
        public List<Image> IndicatorTime;
        [FoldoutGroup("NetworkedUI")]
        public StickerShareViwer StickerViwerLeft;
        [FoldoutGroup("NetworkedUI")]
        public StickerShareViwer StickerViwerRight;

        [FoldoutGroup("NetworkedUI")]
        public Sprite Cup_sprite;
        [FoldoutGroup("NetworkedUI")]
        public Sprite Gem_sprite;
        [FoldoutGroup("NetworkedUI")]
        public Sprite Coin_sprite;

        [FoldoutGroup("NetworkedUI")]
        public GameObject im_BadConnection;
        //public Slider slider;
        // [SerializeField]
        // public List<MarbleState> Frames;
        // public Button Interpolate;
        //  public Text marbledebug;
        // public Text balldebug;

        // public float speed = 1.0f;
        // public bool play = false;
        //  public bool IsLerp = false;
        /// public int Step = 0;

        private bool GameDataRecive = false;
        private int Side;
        private bool intergate = true;
        private Diaco.Setting.GeneralSetting setting;

        private Coroutine ReciveDataMarblesMovment_Coroutine;
        #endregion

        #region StructGame

        //  Queue<MarbleMovementPackets> QueuemovementPackets;
        List<MarblesData> MarblesDataRecived = new List<MarblesData>();
        //  public List<Frame> Frames;

        #endregion
        #region FunctionUnity

        public void Start()
        {

            //  Frames = new List<Frame>();
            // QueuemovementPackets = new Queue<MarbleMovementPackets>();

            OnChangeTurn += ServerManager_OnChangeTurn;
            if (InRecordMode)
                MarblesInRecorodMode = new List<ForceToBall>();
            if (BlockChat_Button)
                BlockChat_Button.onClick.AddListener(() => { Emit_BlockChat(); });
            /// Interpolate.onClick.AddListener(startMovxxx);


            //  Debug.Log(SoftFloat.Soft(12));
        }
        // private float timer;

        void Update()
        {

            /*if (Physics.autoSimulation)
                return; 

            timer += Time.deltaTime;
            while (timer >= Time.fixedDeltaTime)
            {
                timer -= Time.fixedDeltaTime;

                Physics.Simulate(Time.fixedDeltaTime);
            }*/
          /*  marbledebug.text = "";
            for (int i = 0; i < Marbles.Count; i++)
            {
                if (Marbles[i] != null)
                    marbledebug.text += $"ID:{Marbles[i].ID}  X:  {Marbles[i].transform.position.x}  Y:  {Marbles[i].transform.position.y}  Z:  {Marbles[i].transform.position.z}\r\n";
            }*/


        }

        public void OnEnable()
        {

            ConnectToServer();
        }
        private void OnDestroy()
        {
            ClearMemoryTextures();
            if (BlockChat_Button)
                BlockChat_Button.onClick.RemoveAllListeners();
            CloseSocket();
        }
        #endregion


        private Tween canclemove;
        public void ConnectToServer()
        {

            setting = FindObjectOfType<Diaco.Setting.GeneralSetting>();
            var Notification_Dialog = FindObjectOfType<Diaco.Notification.Notification_Dialog_Manager>();
            var namespaceserver = FindObjectOfType<GameLuncher>().NamespaceServer;
            
            string URL = setting.ServerAddress;

            Notification_Dialog.server_soccer = this;
            Notification_Dialog.init_Notification_soccer();
            Debug.Log("before connect ");
            SocketOptions options = new SocketOptions();

            options.AutoConnect = true;
            
            this.NamespaceServer = namespaceserver;
            socket_manager = new SocketManager(new Uri(URL), options);
            socket = socket_manager["/soccer" + namespaceserver];
            socket.On("connect", (s, p, a) =>
            {
                socket.Emit("authToken", ReadToken("token"),setting.Version);
                BadConnectionShow(false);
                Time.timeScale = 1;
                print("Connected");
                // Handler_GameReady();
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
                    //Debug.Log("Notifi" + m[1].ToString());
                    Handler_OnNotification(notif);

                }

            });
            if (InRecordMode == false)
            {
                soundeffectcontrollLayer1.PlaySoundSoccer(0);
                socket.On("userInformation", (s, p, m) =>
                {
                    Info = new UserInfo();
                    Info = JsonUtility.FromJson<UserInfo>(m[0].ToString());
                });
                socket.On("gameData", (s, p, m) =>
                {
                    Debug.Log("gameData");
                    StopCoroutineDataRecive();
                
                    DoResetAim();
                    gameData = JsonUtility.FromJson<GameData>(m[0].ToString());
                    if (!SpwanedMarbels)
                        SelectArena(gameData.ground);
                 //   Debug.Log("GameStep::" + gameData.ground);
                    if(gameData.step  ==0 )
                    {
                        FreePlay = true;
                        
                    }
                    else
                    {
                        FreePlay = false;
                    }
                //    Debug.Log("gameData2");
                    var aim_dot = FindObjectOfType<AimDot>();
                    if (gameData.playerOne.userName == Info.userName)
                    {
                        Side = 1;
                        if (gameData.ownerTurn == 1)
                            aim_dot.DotPower = Mathf.Clamp(gameData.playerOne.aim, 0, 1);
                        else
                            aim_dot.DotPower = Mathf.Clamp(gameData.playerTwo.aim, 0, 1);
                    }
                    else
                    {
                        Side = 2;
                        //SetPlayerTwo(gameData);
                        if (gameData.ownerTurn == 2)
                            aim_dot.DotPower = Mathf.Clamp(gameData.playerTwo.aim, 0, 1);
                        else
                            aim_dot.DotPower = Mathf.Clamp(gameData.playerOne.aim, 0, 1);
                    }
                  //  Debug.Log("gameData3");
                    if (NamespaceServer != "_classic")
                        CalculateGameTime(gameData.gameTime / 1000.0f);
                    else
                        SetTypeCost(gameData.costType);
                        SetUICountCost(gameData.cost.ToString());

                    SetPlayer();
                 Debug.Log("gameData2");
                });
                socket.On("Aim", (s, p, a) =>
                {



                    var aim = JsonUtility.FromJson<AimData>(a[0].ToString());
                    Handler_OnAimrRecive(aim);
                    //  Debug.Log("XXXXXXXx");
                });
                socket.On("Position", (s, p, a) =>
                {

                   // Debug.Log("XXX");
                    var data = JsonUtility.FromJson<MarblesData>(a[0].ToString());
                    MarblesDataRecived.Add(data);
                    if (MarblesDataRecived.Count > 0 && intergate)
                    {
                     //   Debug.Log("DDDD");
                        ReciveDataMarblesMovment_Coroutine = StartCoroutine(ReciveDataMarblesMovment());
                        intergate = false;
                    }



                });
                socket.On("CancelCooldown", (s, p, a) =>
                     {

                         DoResetAim();
                         CancleInvokeTime();
                         Debug.Log("CancelCoolDown");
                     });
                socket.On("game-result", (s, p, a) =>
                {

                    StartCoroutine(ShowResualtPage(a)); 

                    Debug.Log("GameRes Resssss");
                });
                socket.On("formationShop", (s, p, m) => {
                   /// Debug.Log("kjkjkjkjkjkjjjkjkjkjk");
                    var data = JsonUtility.FromJson<Diaco.Store.Soccer.SoccerShopDatas>(m[0].ToString());
                    Handler_InitShop(data);
                    Debug.Log("formationShopInGameRecive"+ m[0].ToString());
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
                        Time.timeScale = 1;


                    Handler_IncomingMessage(message, durtaion);

                    Debug.Log("ReciveMessage:" + message);
                });


            }
            else
            {
                socket.On("gameData", (s, p, a) =>
                {
                    
                    var recordmodeGameData = JsonUtility.FromJson<RecordModeGameData>(a[0].ToString());

                    
                    ClearSceneInRecordMode();
                    SpawnAssetsInRecordMode(recordmodeGameData.ballPos, recordmodeGameData.playerPos, recordmodeGameData.marblePos);
                    SetUIInRecordMode(recordmodeGameData.totalPoint, recordmodeGameData.level, recordmodeGameData.timer, recordmodeGameData.points);
                    Turn = true;
                    SiblArea.Area = 4;
                    Handler_GameReady();
                  //  Debug.Log("READY" + a[0].ToString());
                });
                socket.On("result", (s, p, a) =>
                {

                    var result = JsonUtility.FromJson<ResualtInRecordModeData>(a[0].ToString());
                    resualtInRecordMode.gameObject.SetActive(true);
                    resualtInRecordMode.Set(result);
                    CancelInvoke("RunTimer");

                });
            }

            socket.On("BackToMenu", (s, p, m) => {


                //// CloseSocket();
                var Luncher = FindObjectOfType<GameLuncher>();
                Luncher.BackToMenu();

            });
            socket.On("disconnect", (s, p, a) =>
            {
                BadConnectionShow(true);
                
                if(FindObjectOfType<GameLuncher>().InBackToMenu  == false)
                {
                    Time.timeScale = 0;
                }
                else
                {
                    FindObjectOfType<GameLuncher>().InBackToMenu = false;
                    Time.timeScale = 1;
                }
                print("disConnected");
            });
        }
        public void CloseSocket()
        {
            socket.Off();
            socket.Manager.Close();
            socket.Disconnect();
            Debug.Log("SoccerCloseConnection");
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

        
        #region FunctionServerInNormalMode
        public void IntiGameData()
        {

            gameData = new GameData();
        }
        /// <summary>
        /// ///////////////////////////////////////
        /// </summary>
        /// <param name="data"></param>

        public IEnumerator ResetDataInNormalMode()
        {
            // gameData = new GameData();
            //  Info = new UserInfo();
            MarbleInGoal = 0;
            IsGoal = -1;
            SpwanedMarbels = false;
            MinRangMarblesId = 0;
            MaxRangMarblesId = 0;
            SetGoalUIText("", "");
            SetGoalUIName("", "");
            SetImageUI(null, null);
            CancleInvokeTime();
            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < Marbles.Count; i++)
            {
                if (Marbles[i] != null)
                {
                    if (Marbles[i].ID == 10)
                    {
                        Marbles[i].transform.position = new Vector3(0.0f, 2.0f, 0.0f);

                    }
                    else
                    {
                        Destroy(Marbles[i].gameObject);
                    }
                }
            }
            yield return new WaitForSeconds(0.1f);
            //  SendPositionMarblesToServer = new DataPostionsMarbles();
            //InfoMarbleInGoalWithPositionsForSendToServer = new _InfoMarbleInGoalWithPositions();
            /// GetComponent<PlayerController>().enabled = false;//**/////***///

            GetComponent<TempPlayerControll>().enabled = false;

            // Marbles = new List<ForceToBall>();
            yield return new WaitForSeconds(0.1f);

        }
        public void SetPlayer()
        {
            
            GameDataRecive = false;
            Turn = false;
            // Debug.Log("TRUN5");
          
            if (ResultGamePage.activeSelf && gameData.state != -1)
            {
                //ResultGamePage.SetActive(false);
                var luncher = FindObjectOfType<GameLuncher>();
                luncher.PlayAgainGame(0);
                return;
            }

            if (Side == 1)
            {
                MinRangMarblesId = 0;
                MaxRangMarblesId = 4;

                SetGoalUIText(gameData.playerOne.score.ToString(), gameData.playerTwo.score.ToString());
                SetGoalUIName(gameData.playerOne.userName, gameData.playerTwo.userName);
                SetImageUI(imageContainer.LoadImage(gameData.playerOne.avatar), imageContainer.LoadImage(gameData.playerTwo.avatar));
                SetTime(gameData.turnTime, gameData.totalTime);
            }
            else if (Side == 2)
            {
                MinRangMarblesId = 5;
                MaxRangMarblesId = 9;
                SetGoalUIText(gameData.playerTwo.score.ToString(), gameData.playerOne.score.ToString());
                SetGoalUIName(gameData.playerTwo.userName, gameData.playerOne.userName);
                SetImageUI(imageContainer.LoadImage(gameData.playerTwo.avatar), imageContainer.LoadImage(gameData.playerOne.avatar));
                SetTime(gameData.turnTime, gameData.totalTime);
            }

            if (Convert.ToInt16(gameData.state) == -1)
            {
                if (gameData.winner == Info.userName)
                {
                    Debug.Log("Winner");

                }
                if (gameData.loser == Info.userName)
                {
                    Debug.Log("Loser");

                }
                Turn = false;
                
            }

            StartCoroutine(InitializTurn_new());
            DOVirtual.DelayedCall(0.2f,()=>{Handler_GameReady(); });
            
            IsGoal = -1;
        }
        public IEnumerator InitializTurn_new()
        {
            Handler_OnPhysicFreeze(true);
            if (Side == 1)
            {
                if (SpwanedMarbels == false)

                {

                    if (gameData.playerOne.skin == gameData.playerTwo.skin)
                    {
                        RunSpawnMarble_New(gameData, 1, gameData.playerOne.skin, "D2");
                    }
                    else
                    {
                        RunSpawnMarble_New(gameData, 1, gameData.playerOne.skin, gameData.playerTwo.skin);
                    }

                }
                else
                {
                    yield return  StartCoroutine(MoveMarbelsToPositionFromServer(gameData, 1, FrameRate));
                   /// yield return new WaitForSeconds(0.4f);
                }

                if (gameData.state == 1)
                {
                    if (gameData.ownerTurn == Side)
                    {
                        Turn = true;
                        EnablerRingEffect = true;
                        soundeffectcontrollLayer2.PlaySoundSoccer(0);/////  play turn sound
                        if (setting.Setting.vibration)
                            Handheld.Vibrate();                              
                    }
                    else
                    {
                        EnablerRingEffect = true;
                        ///Handler_EnableRingMarbleForOpponent(true);
                    }
                }
            }
            else if (Side == 2)
            {
                if (SpwanedMarbels == false)
                {

                    if (gameData.playerOne.skin == gameData.playerTwo.skin)
                    {
                        RunSpawnMarble_New(gameData, -1, "D2", gameData.playerTwo.skin);
                    }
                    else
                    {
                        RunSpawnMarble_New(gameData, -1, gameData.playerOne.skin, gameData.playerTwo.skin);
                    }

                }
                else
                {
                 yield return StartCoroutine(MoveMarbelsToPositionFromServer(gameData, -1, FrameRate));
                  //  yield return new WaitForSeconds(0.4f);
                }
                if (gameData.state == 1)
                {
                    if (gameData.ownerTurn == Side)
                    {
                        Turn = true;

                        EnablerRingEffect = true;
                        soundeffectcontrollLayer2.PlaySoundSoccer(0);/////  play turn sound
                        if (setting.Setting.vibration)
                            Handheld.Vibrate();
                        ///  Debug.Log("TRUN2");
                        //  KinimaticMarblesAndBall(false);
                    }
                    else
                    {
                        EnablerRingEffect = true;
                        //Handler_EnableRingMarbleForOpponent(true);
                    }
                }

            }

            Handler_OnPhysicFreeze(false);
        }

        public void RunSpawnMarble_New(GameData data, int Side, string selfskin, string enemyskin)
        {

            Vector3 pos = new Vector3();
           // Debug.Log("Side:" + Side);
            for (int i = 0; i < 10; i++)
            {



                if (i == 0)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_1, Side);
                }
                else if (i == 1)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_2, Side);
                }
                else if (i == 2)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_3, Side);
                }
                else if (i == 3)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_4, Side);
                }
                else if (i == 4)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_5, Side);
                }
                else if (i == 5)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_6, Side);
                }
                else if (i == 6)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_7, Side);
                }
                else if (i == 7)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_8, Side);
                }
                else if (i == 8)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_9, Side);
                }
                else if (i == 9)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_10, Side);
                }

                if (i >= 0 && i < 5)
                {
                    var type = SelectBottomFlag(selfskin);
                    var marble = Instantiate(MarbleRegistered[type], pos, Quaternion.identity, ParentForSpawn);
                    marble.GetComponent<ForceToBall>().ID = i;

                    marble.GetComponent<ForceToBall>().SetSkinMarble(SelectFlag(selfskin));
                    //  Debug.Log("POS:" + pos);
                }

                else if (i > 4 && i < 10)
                {
                    var type = SelectBottomFlag(enemyskin);
                    var marble = Instantiate(MarbleRegistered[type], pos, Quaternion.identity, ParentForSpawn);
                    marble.GetComponent<ForceToBall>().ID = i;
                    marble.GetComponent<ForceToBall>().SetSkinMarble(SelectFlag(enemyskin));
                    ///  Debug.Log("POS:" + pos);
                }
            }
            SpwanedMarbels = true;
            UpdateListMarbles();


        }
        public IEnumerator MoveMarbelsToPositionFromServer(GameData data, int side, float speed)
        {
            EnablerRingEffect = false;
            Vector3 pos = new Vector3();
            for (int i = 0; i < Marbles.Count; i++)
            {
                if (i == 0)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_1, side);
                }
                else if (i == 1)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_2, side);
                }
                else if (i == 2)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_3, side);
                }
                else if (i == 3)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_4, side);
                }
                else if (i == 4)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_5, side);
                }
                else if (i == 5)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_6, side);
                }
                else if (i == 6)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_7, side);
                }
                else if (i == 7)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_8, side);
                }
                else if (i == 8)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_9, side);
                }
                else if (i == 9)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.m_p_10, side);
                }
                else if (i == 10)
                {
                    pos = VectorHelper.ToVector3WithSide(data.positions.b_p, side);
                }
                Marbles[i].transform.position = pos;


                yield return null;
            }
            

        }



        /// <summary>
        /// use in Invoke_CheckMovemenInSecond()
        /// </summary>

        private void CheckMovment()
        {
            /// var move = false;
            int count_stoped = 0;
            for (int i = 0; i < Marbles.Count; i++)
            {
                if (Marbles[i])
                {


                    if (!Marbles[i].OnlyCheckMove())
                    {
                        count_stoped++;

                    }

                    if (count_stoped == Marbles.Count)
                    {

                        CancelInvoke("CheckMovment");
                        MarblesInMove = false;
                        Debug.Log("All Marbles Stoped");
                    }
                }
            }

        }
        public Text DebugEndTurn;
        public void Invoke_CheckMovemenInSecond()
        {
            InvokeRepeating("CheckMovment", 1f, 0.1f);
            Debug.Log("CheckMovmentFromServer");
        
        }

        public IEnumerator SendDataMarblesMovement()
        {
            DebugEndTurn.text = "EndTurn : Send Data Wait";
            MarblesInMove = true;
            Turn = false;
            Invoke_CheckMovemenInSecond();
            MarblesData Data = new MarblesData();
            canclemove = DOVirtual.DelayedCall(20, () =>
            {
                MarblesInMove = false;
                for (int i = 0; i < Marbles.Count; i++)
                {
                    Marbles[i].StopMovment();
                }
                //Debug.Log("Force Stop");

            }, false);
            do
            {
                Data.m_p_1 = VectorHelper.To_Vec_Soccer(Marbles[0].transform.position);
                Data.m_v_1 = VectorHelper.To_Vec_Soccer(Marbles[0].rigidbody.velocity);

                Data.m_p_2 = VectorHelper.To_Vec_Soccer(Marbles[1].transform.position);
                Data.m_v_2 = VectorHelper.To_Vec_Soccer(Marbles[1].rigidbody.velocity);

                Data.m_p_3 = VectorHelper.To_Vec_Soccer(Marbles[2].transform.position);
                Data.m_v_3 = VectorHelper.To_Vec_Soccer(Marbles[2].rigidbody.velocity);

                Data.m_p_4 = VectorHelper.To_Vec_Soccer(Marbles[3].transform.position);
                Data.m_v_4 = VectorHelper.To_Vec_Soccer(Marbles[3].rigidbody.velocity);

                Data.m_p_5 = VectorHelper.To_Vec_Soccer(Marbles[4].transform.position);
                Data.m_v_5 = VectorHelper.To_Vec_Soccer(Marbles[4].rigidbody.velocity);

                Data.m_p_6 = VectorHelper.To_Vec_Soccer(Marbles[5].transform.position);
                Data.m_v_6 = VectorHelper.To_Vec_Soccer(Marbles[5].rigidbody.velocity);

                Data.m_p_7 = VectorHelper.To_Vec_Soccer(Marbles[6].transform.position);
                Data.m_v_7 = VectorHelper.To_Vec_Soccer(Marbles[6].rigidbody.velocity);

                Data.m_p_8 = VectorHelper.To_Vec_Soccer(Marbles[7].transform.position);
                Data.m_v_8 = VectorHelper.To_Vec_Soccer(Marbles[7].rigidbody.velocity);

                Data.m_p_9 = VectorHelper.To_Vec_Soccer(Marbles[8].transform.position);
                Data.m_v_9 = VectorHelper.To_Vec_Soccer(Marbles[8].rigidbody.velocity);

                Data.m_p_10 = VectorHelper.To_Vec_Soccer(Marbles[9].transform.position);
                Data.m_v_10 = VectorHelper.To_Vec_Soccer(Marbles[9].rigidbody.velocity);

                Data.b_p = VectorHelper.To_Vec_Soccer(Marbles[10].transform.position);
                Data.b_v = VectorHelper.To_Vec_Soccer(Marbles[10].rigidbody.velocity);

                Data.Tik += 1;
                Data.LastPacket = false;



                Emit_DataMarblesToServer(Data);
                //Debug.Log("SendData++");
                yield return new WaitForSecondsRealtime(FrameRate);
            }
            while (MarblesInMove);

            canclemove.Kill(false);
            Data.LastPacket = true;
            Emit_DataMarblesToServer(Data);
            Debug.Log("LastPacket:" + Data.Tik);

            yield return new WaitForSecondsRealtime(FrameRate);
            socket.Emit("EndTurn", IsGoal);
            Debug.Log("ISGoal::" + IsGoal);
            IsGoal = -1;
            DebugEndTurn.text = "EndTurn : True";
            CancelInvoke("CheckMovment");
        }
        public void Emit_DataMarblesToServer(MarblesData data)
        {
            string json = JsonUtility.ToJson(data);
            socket.Emit("Position", json);
        }

        public void StopCoroutineDataRecive()
        {
            MarblesDataRecived = new List<MarblesData>();
            if (ReciveDataMarblesMovment_Coroutine != null)
                StopCoroutine(ReciveDataMarblesMovment_Coroutine);
            intergate = true;
        }
        public IEnumerator ReciveDataMarblesMovment()
        {
            DoResetAim();
            EnablerRingEffect = false;
            bool LoopCancle = false;
            int tik = 0;
            yield return new WaitForSecondsRealtime(2.0f);
            while (LoopCancle == false)
            {

                int t = Mathf.Clamp(tik, 0, MarblesDataRecived.Count - 1);
                Marbles[0].SetMovmentData(MarblesDataRecived[t].m_p_1, MarblesDataRecived[t].m_v_1, t);
                Marbles[1].SetMovmentData(MarblesDataRecived[t].m_p_2, MarblesDataRecived[t].m_v_2, t);
                Marbles[2].SetMovmentData(MarblesDataRecived[t].m_p_3, MarblesDataRecived[t].m_v_3, t);
                Marbles[3].SetMovmentData(MarblesDataRecived[t].m_p_4, MarblesDataRecived[t].m_v_4, t);
                Marbles[4].SetMovmentData(MarblesDataRecived[t].m_p_5, MarblesDataRecived[t].m_v_5, t);
                Marbles[5].SetMovmentData(MarblesDataRecived[t].m_p_6, MarblesDataRecived[t].m_v_6, t);
                Marbles[6].SetMovmentData(MarblesDataRecived[t].m_p_7, MarblesDataRecived[t].m_v_7, t);
                Marbles[7].SetMovmentData(MarblesDataRecived[t].m_p_8, MarblesDataRecived[t].m_v_8, t);
                Marbles[8].SetMovmentData(MarblesDataRecived[t].m_p_9, MarblesDataRecived[t].m_v_9, t);
                Marbles[9].SetMovmentData(MarblesDataRecived[t].m_p_10, MarblesDataRecived[t].m_v_10, t);
                Marbles[10].SetMovmentData(MarblesDataRecived[t].b_p, MarblesDataRecived[t].b_v, t);
                tik++;

                //  Debug.Log("ReciveData" + MarblesDataRecived[t].LastPacket);
                yield return new WaitForSecondsRealtime(FrameRate);


                if (MarblesDataRecived[t].LastPacket)
                {
                    LoopCancle = true;
                    socket.Emit("EndTurn");
                    for (int i = 0; i < Marbles.Count; i++)
                    {
                        if (Marbles[i] != null)
                            Marbles[i].StopMovment();
                    }


                    Debug.Log("LastRecive:" + MarblesDataRecived[t].Tik);

                }
            }
            MarblesDataRecived.Clear();

            intergate = true;

            yield return null;

        }

        public IEnumerator DelayAim(string json)//ReciveFromServer
        {
            var aim = JsonUtility.FromJson<AimData>(json);
            //Debug.Log("2");
            yield return new WaitForSeconds(0.001f);
            /* for (int i = 0; i < Marbles.Length; i++)
             {
                 var marble = Marbles[i].GetComponent<ForceToBall>();
                 if (marble.ID == aim.ID)
                 {
                     marble.ReciveAimdataFromSever(aim);

                 }

             }*/
            Handler_OnAimrRecive(aim);
        }

        public void SendForceData(FORCEDATA forcedata)
        {
            var force = JsonUtility.ToJson(forcedata);
            socket.Emit("Force", force);
            Debug.Log(force);
        }
        public void SendAimData(AimData aimRecord)
        {
            var j = JsonUtility.ToJson(aimRecord);
            socket.Emit("Aim", j);

        }

        public void SelectArena(string name)
        {

            for (int i = 0; i < Arena.Count; i++)
            {
                if (Arena[i].name == name)
                {
                    Arena[i].arena.SetActive(true);

                }
                else if (Arena[i].name != name)
                {
                    Arena[i].arena.SetActive(false);
                }
                else if (Arena[i].name == "")
                {
                    Arena[0].arena.SetActive(true);
                }
            }
        }




        public Sprite SelectFlag(string name)
        {

            Texture2D texture = new Texture2D(512, 512);

            //var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            Flags.ForEach(e => {

                if (e.name == name)
                {
                    texture = e.flag;

                }

                else if (e.name == "")
                {
                    texture = Flags[0].flag;

                }

            });
            var s = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 512);
            //temp_texture.Add(texture);
            //temp_sprite.Add(s);
            return s;
        }

        //private List<Texture2D> temp_texture = new List<Texture2D>();
        //private List<Sprite> temp_sprite = new List<Sprite>();
        private void ClearMemoryTextures()
        {
           /* if (temp_texture.Count > 0)
            {
                for (int i = 0; i < temp_texture.Count; i++)
                {
                    Destroy(temp_texture[i]);
                    Destroy(temp_sprite[i]);
                }
                temp_texture.Clear();
                temp_sprite.Clear();
            }*/
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">NameOfMarble</param>
        /// <returns>TypeOfBottomMarble : 0 = gold, 1 = silver</returns>
        public int SelectBottomFlag(string name)
        {
            int type = 0;
            Flags.ForEach(e => {

                if (e.name == name)
                {
                    if (e.BottomGold)
                        type = 0;
                    else
                        type = 1;

                }

            });
            return type;
        }
        public void StickerViwer(object namesticker, object side)
        {
            string name = Convert.ToString(namesticker);
            int sideshow = Convert.ToInt32(side);///0 left, 1 right
            if (sideshow == 0)
            {
                StickerViwerLeft.SelectSticker(name);
                StickerViwerLeft.gameObject.SetActive(true);
                Debug.Log("StickerEnable Left");
            }
            else
            {
                StickerViwerRight.SelectSticker(name);
                StickerViwerRight.gameObject.SetActive(true);
                Debug.Log("StickerEnable Right");
            }
        }
        public void BadConnectionShow(bool show)
        {

            im_BadConnection.SetActive(show);


        }
       
        public IEnumerator ShowResualtPage(object []a)
        {
            Time.timeScale = 1;

            soundeffectcontrollLayer1.PlaySoundSoccer(1);
            yield return new WaitForSeconds(2);
           /// Debug.Log("ssssesddssddss");
            ResultGamePage.SetActive(true);

            var result = JsonUtility.FromJson<Diaco.EightBall.Structs.ResultGame>(a[0].ToString());

            bool PlayAgianActive = Convert.ToBoolean(a[1]);/// Enable Button Play Again
            Handler_OnGameResult(result, PlayAgianActive);

            soundeffectcontrollLayer2.PlaySoundSoccer(3);
            yield return new WaitForSeconds(3);
            if (Info.userName == result.winner.userName)
            {
                soundeffectcontrollLayer1.PlaySoundSoccer(2);
            }
                Debug.Log("GameResult");
        }
        public void SetGoalUIText(string left, string right)
        {
            GoalIndicatorOnBiliboard[0].text = left;
            GoalIndicatorOnBiliboard[1].text = right;
            //    Debug.Log("UIChanged");
        }
        public void SetGoalUIName(string left, string right)
        {
            NameIndicatorOnBiliboard[0].text = left;
            NameIndicatorOnBiliboard[1].text = right;
            //    Debug.Log("UIChanged");
        }
        public void SetUICountCost(string Count)
        {
            CoinOrTimeIndicatorOnBiliboard.text = Count;

            //    Debug.Log("UIChanged");
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
        public void SetUITimer(string Count)
        {
            CoinOrTimeIndicatorOnBiliboard.text = Count;

            //    Debug.Log("UIChanged");
        }
        public void SetImageUI(Sprite left, Sprite right)
        {
            ImagePlayerIndicatorOnBiliboard[0].sprite = left;
            ImagePlayerIndicatorOnBiliboard[1].sprite = right;
        }


        private void Timerleft()
        {

            var unit_time = 1.0f / TimeTurn;
            var fill = IndicatorTime[0].fillAmount - unit_time / 100;////smooth fill
            IndicatorTime[0].fillAmount = fill;
            if (IndicatorTime[0].fillAmount < 0.01f)
            {
                Turn = false;
                IndicatorTime[0].fillAmount = 1;
                DoResetAim();
                CancelInvoke("Timerleft");
                //  Debug.Log("TRUN8");
                //Pitok = 0;
                //   Debug.Log("CancleCooldownLeft...");
            }


        }
        private void Timerright()
        {


            var unit_time = 1.0f / TimeTurn;
            var fill = IndicatorTime[1].fillAmount - unit_time / 100;////smooth fill
            IndicatorTime[1].fillAmount = fill;
            if (IndicatorTime[1].fillAmount < 0.01f)
            {
                Turn = false;
                IndicatorTime[1].fillAmount = 1;
                DoResetAim();
                CancelInvoke("Timerright");
                ///  Debug.Log("TRUN9");
                //Pitok = 0;
                //   Debug.Log("CancleCooldownLeft...");
            }


        }
        private void StartInvokeTimerLeft()
        {
            InvokeRepeating("Timerleft", 0.0f, 0.01f * UnityEngine.Time.timeScale);
            ///      Debug.Log("TimerLeftStart");
        }
        private void StartInvokeTimerRight()
        {
            InvokeRepeating("Timerright", 0.0f, 0.01f* UnityEngine.Time.timeScale);
            //Debug.Log("TimerRightStart");
        }
        public void CancleInvokeTime()
        {

            CancelInvoke("Timerleft");
            CancelInvoke("Timerright");
            IndicatorTime[0].fillAmount = 1.0f;
            IndicatorTime[1].fillAmount = 1.0f;
            
         //   Debug.Log("TimeCancelAndReset");
        }
        public void SetTime(float t, float totaltime)
        {
            CancleInvokeTime();
            if (gameData.state == 1)
            {
                TimeTurn = t / 1000;////convert to  sec
                float fill = t / totaltime;

                if (gameData.ownerTurn == Side)
                {
                     IndicatorTime[0].fillAmount = fill; 
                    StartInvokeTimerLeft();
                }
                else
                {
                     IndicatorTime[1].fillAmount = fill;
                    StartInvokeTimerRight();
                }

                //   Debug.Log("TimerSet:" + t);
            }
        }

        private void CalculateGameTime(float time)
        {

            H = 0;
            M = 0;
            S = 0;
            CancelInvoke("RunGameTimerForNormalMode");


            H = (float)Math.Floor(time / 3600);
            M = (float)Math.Floor(time / 60 % 60);
            S = (float)Math.Floor(time % 60);
            if (gameData.state == 1)
                InvokeRepeating("RunGameTimerForNormalMode", 0, 1.0f);
        }
        /// <summary>
        /// INVOKE IN Calculate
        /// </summary>
        private void RunGameTimerForNormalMode()
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

            string ss = S < 10 ? "0" + S.ToString() : S.ToString();
            string mm = M < 10 ? "0" + M.ToString() : M.ToString();
            CoinOrTimeIndicatorOnBiliboard.text = mm + ":" + ss;
            if (S == 0 && M == 0 && H == 0)
            {
                CancelInvoke("RunGameTimerForNormalMode");


            }
        }


        public void UpdateListMarbles()
        {
            Marbles = FindObjectsOfType<ForceToBall>().ToList();

            Marbles = Marbles.OrderBy(m => m.ID).ToList();
            Debug.Log("update");
        }
        //public float ThresholdSleep = 0.09f;
        
       /* public bool EqeulPosition(Vector3 a, Vector3 b)
        {
            bool eqeul = false;
            var x = a.x - b.x;
            var y = a.z - b.z;
            if (x == 0.0f && y == 0.0f)
            {
                eqeul = true;
            }
            return eqeul;
        }*/
        public void Emit_LeftGame()
        {
            socket.Emit("left-game");
           // CloseSocket();

        }
        public void Emit_PlayAgain()
        {
          
            socket.Emit("play-again");


        }
        public void Emit_Shopformation()
        {
            socket.Emit("formationShop");
            Debug.Log("Emit_Shopformation");
        }
        public void Emit_UseFormation(string id)
        {
            socket.Emit("useFormation", id);
        }
        public void Emit_RentFormation( string rentId)
        {
            socket.Emit("rentFormation",  rentId);
            Debug.Log("Emit_ShopformationRent="+"::"+rentId);
        }

        public void Emit_GetSticker()
        {
            socket.Emit("getSticker");
            Debug.Log("Emit_getSticker");
        }
        public void  Emit_ShareSticker(string name)
        {
            socket.Emit("shareSticker" , name);
            Debug.Log("Emit_shareSticker");
        }
        public void  Emit_Message(string message)
        {
            socket.Emit("message", message);
            Debug.Log("Emit_Message");

        }
        public  void Emit_BlockChat()
        {
            socket.Emit("blockChat");
            Debug.Log("ChatBloked!");
        }
        public void Emit_AddFriend()
        {
            socket.Emit("add-friend");
            Debug.Log("ChatBloked!");
        }
        public void Emit_DialogAndNotification(string eventName, string data)
        {
            socket.Emit(eventName, data);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="active"></param>
        public void KinimaticMarblesAndBall(bool active)
        {
            /*  if (active)
              {
                  for (int i = 0; i < Marbles.Length; i++)
                  {
                      if (Marbles[i] != null)
                      {
                          Marbles[i].GetComponent<Rigidbody>().isKinematic = true;
                          Marbles[i].GetComponent<Rigidbody>().useGravity = false;
                          Marbles[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                          Marbles[i].GetComponent<Collider>().enabled = false;
                      }

                  }
              }
              else
              {
                  for (int i = 0; i < Marbles.Length; i++)
                  {
                      if (Marbles[i] != null)
                      {
                          Marbles[i].GetComponent<Collider>().enabled = true;
                          Marbles[i].GetComponent<Rigidbody>().useGravity = true;
                          Marbles[i].GetComponent<Rigidbody>().isKinematic = false;

                          Marbles[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

                      }
                  }
              }*/
        }
        public void DoResetAim()
        {
            Handler_ResetAim();
        }
        /// <summary>
        /// /////////////////////////////////////////////
        /// </summary>
        /// <returns></returns>



      


        #endregion
        #region FunctionServerInRecordMode
        private void SpawnAssetsInRecordMode(Vector3 ballPos, Vector3 playerPos, Vector3[] marblePos)
        {
            var ball = Instantiate(BallPrefab, ballPos, Quaternion.identity, ParentForSpawn);
            MarblesInRecorodMode.Add(ball);
            var player = Instantiate(MarblesRegisteredForRecordMode[0], playerPos, Quaternion.identity, ParentForSpawn);
            MarblesInRecorodMode.Add(player);
            //player.InRecordMode = true;
            player.ID = 0;
            MinRangMarblesId = 0;
            MaxRangMarblesId = 4;
            for (int i = 0; i < marblePos.Length; i++)
            {
                var marble = Instantiate(MarblesRegisteredForRecordMode[1], marblePos[i], Quaternion.identity, ParentForSpawn);
              //  marble.InRecordMode = true;
                marble.ID = 5;
                MarblesInRecorodMode.Add(marble);
                //marble.gameObject.SetActive(true);
            }

        }
        public void ClearSceneInRecordMode()
        {

            for (int i = 0; i < MarblesInRecorodMode.Count; i++)
            {
                Destroy(MarblesInRecorodMode[i].gameObject);

            }
            MarblesInRecorodMode.Clear();
        }
        public bool CheckMarbleMoveInRecordMode()
        {
            var move = false;

                
                for (int i = 0; i < MarblesInRecorodMode.Count; i++)
                {
                   
                    if (MarblesInRecorodMode[i])
                    {
                       
                        if (MarblesInRecorodMode[i].GetComponent<Rigidbody>().velocity != Vector3.zero || MarblesInRecorodMode[i].GetComponent<Rigidbody>().angularVelocity != Vector3.zero)
                        {

                            move = true;
                        }
                    }
                }

            if (move == false)
            {
                Emit_EndTurnInRecordMode();
            }
            return move;
        }
        /// <summary>
        /// invoke CheckMarbleMoveInRecordMode
        /// </summary>
        public void StartCheckMovmentInRecordMode()
        {
            Turn = false;
            InvokeRepeating("CheckMarbleMoveInRecordMode", 1f, 1f * Time.timeScale);
        }
       /* private void CheckMOVEInRecordMode()
        {
            if(CheckMarbleMoveInRecordMode() == false)
            {
                Emit_EndTurnInRecordMode();
            }
        }*/
        private void Emit_EndTurnInRecordMode()
        {
            if (SiblArea.Area != 4)
                soundeffectcontrollLayer2.PlaySoundSoccer(0);///sib sound
            socket.Emit("EndTurn", SiblArea.Area);
            CancelInvoke("CheckMarbleMoveInRecordMode");
            Debug.Log($"<color=blue><b>EndTurn</b>{SiblArea.Area}</color>");
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
            InvokeRepeating("RunTimer", 0, 1.0f);
        }
        /// <summary>
        /// INVOKE IN Calculate
        /// </summary>
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
        #endregion
        #region Trigger
        private void ServerManager_OnChangeTurn(bool turn)
        {
            // GetComponent<PlayerController>().enabled = turn;/**//*///*/
            GetComponent<TempPlayerControll>().enabled = turn;
        }

        #endregion
        #region Events
        private Action<bool> onturn;
        public event Action<bool> OnChangeTurn
        {
            add
            {
                onturn += value;
            }
            remove
            {
                onturn -= value;
            }
        }
        protected void handler_OnTurnChange(bool turn)
        {
            if (onturn != null)
                onturn(turn);
        }
        public event Action OnMoveOutInGoal;
        protected void Handler_OnMoveOutInGoal()
        {
            if (OnMoveOutInGoal != null)
            {
                OnMoveOutInGoal();
            }
        }

        public event Action<Diaco.EightBall.Structs.ResultGame, bool> OnGameResult;
        protected void Handler_OnGameResult(Diaco.EightBall.Structs.ResultGame result, bool playagin)
        {
            if (OnGameResult != null)
            {
                OnGameResult(result,playagin);
            }
        }

        private Action<AimData> aimRecive;
        public event Action<AimData> OnAimRecive
        {
            add
            {
                aimRecive += value;
            }
            remove
            {
                aimRecive -= value;
            }
        }
        protected void Handler_OnAimrRecive(AimData aimData)
        {
            if (aimRecive != null)
            {
                aimRecive(aimData);
            }
        }

        private Action resetaim;
        public event Action ResetAim
        {

            add
            {
                resetaim += value;

            }
            remove
            {
                resetaim -= value;
            }
        }
        protected void Handler_ResetAim()
        {
            if (resetaim != null)
            {
                resetaim();

            }
        }
        private Action<MOVE> move;
        public event Action<MOVE> Move
        {
            add
            {
                move += value;
            }
            remove
            {
                move -= value;
            }
        }
        protected void Handler_Move(MOVE m)
        {
            if (move != null)
            {
                move(m);
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

        /*private Action<bool> enableringmarblforopponent;
        public event Action<bool> EnableRingMarbleForOpponent
        {

            add
            {
                enableringmarblforopponent += value;
            }
            remove
            {
                enableringmarblforopponent -= value;
            }
        }
        public void Handler_EnableRingMarbleForOpponent(bool enable)
        {
            if (enableringmarblforopponent != null)
            {

                enableringmarblforopponent(enable);
            }
        }*/

        private Action softpositionandrotation;
        public event Action SoftPositionAndRotation
        {
            add
            {
                softpositionandrotation += value;
            }
            remove
            {
                softpositionandrotation -= value;
            }
        }
        protected void Handler_SoftPositionAndRotation()
        {
            if (softpositionandrotation != null)
            {
                softpositionandrotation();
            }
        }


        private Action<bool> physicfreeze;
        public event Action<bool> OnPhysicFreeze
        {

            add
            {
                physicfreeze += value;
            }
            remove
            {
                physicfreeze -= value;
            }
        }
        public void Handler_OnPhysicFreeze(bool enable)
        {
            if (physicfreeze != null)
            {

                physicfreeze(enable);
            }
        }
        private Action<Diaco.Store.Soccer.SoccerShopDatas> initshop;
        public event Action<Diaco.Store.Soccer.SoccerShopDatas> InitShop
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
        protected void Handler_InitShop(Diaco.Store.Soccer.SoccerShopDatas data)
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

        private Action<string,float> incomingmessage;
        public event Action<string,float>InComingMessage
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
        protected void Handler_IncomingMessage(string mess,float d)
        {
            if (incomingmessage != null)
            {
                incomingmessage(mess,d);
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
    public Vector3 playerPos;
    public Vector3[] marblePos;
    public Vector3 ballPos;
    public int timer;
    public int[] points;
    public string level;
    public int totalPoint;
}
public class SoftFloat
{
    public static double Soft(double digit )
    {
        
        return Math.Round(digit, 2);
    }

}


[Serializable]
public struct ARENA
{
    public string name;
    public GameObject arena;
}
[Serializable]
public struct FLAG
{
    public string name;
    public Texture2D flag;
    public bool BottomGold;
}
