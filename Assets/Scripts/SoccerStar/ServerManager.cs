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
        public bool InRecordMode = false;
        public float TimeStep = 0.0f;
        public bool FreePlay = false;
        #region DataMemberGlobal
        [FoldoutGroup("ServerSettings")]
        public GameObject ResultGamePage;
        [FoldoutGroup("ServerSettings")]

        public string URLLocal;
        [FoldoutGroup("ServerSettings")]
        public string URLGlobal;
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
        public float smooth;


      
       
       
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
        public List<Image> ImagePlayerIndicatorOnBiliboard;
        [FoldoutGroup("NetworkedUI")]
        public List<Text> GoalIndicatorOnBiliboard;
        [FoldoutGroup("NetworkedUI")]
        public List<Text> NameIndicatorOnBiliboard;
        [FoldoutGroup("NetworkedUI")]
        public Text CoinOrTimeIndicatorOnBiliboard;
        [FoldoutGroup("NetworkedUI")]
        public List<Image> IndicatorTime;
        [FoldoutGroup("NetworkedUI")]
        public StickerShareViwer StickerViwerLeft;
        [FoldoutGroup("NetworkedUI")]
        public StickerShareViwer StickerViwerRight;
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
        #endregion

        #region StructGame

        Queue<MarbleMovementPackets> QueuemovementPackets;
        //  public List<Frame> Frames;

        #endregion
        #region FunctionUnity

        public void Start()
        {
          
            //  Frames = new List<Frame>();
            QueuemovementPackets = new Queue<MarbleMovementPackets>();
  
            OnChangeTurn += ServerManager_OnChangeTurn;
            if (InRecordMode)
                MarblesInRecorodMode = new List<ForceToBall>();
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
            ConnectToServer(URLGlobal);
        }
        private void OnDestroy()
        {
            CloseSocket();
        }
        #endregion


        public void ConnectToServer(string URL)
        {
            
            SocketOptions options = new SocketOptions();

            options.AutoConnect = true;
            var namespaceserver = FindObjectOfType<GameLuncher>().NamespaceServer;
            this.NamespaceServer = namespaceserver;
            socket_manager = new SocketManager(new Uri(URL), options);
            socket = socket_manager["/soccer" + namespaceserver];
            socket.On("connect", (s, p, a) =>
            {
                socket.Emit("authToken", ReadToken("token"));
                BadConnectionShow(false);
                Time.timeScale = 1;
                print("Connected");
                // Handler_GameReady();
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
                    
                    DoResetAim();
                    gameData = JsonUtility.FromJson<GameData>(m[0].ToString());
                   // Debug.Log("GameStep::" + gameData.step);
                    if(gameData.step  ==0 )
                    {
                        FreePlay = true;
                        
                    }
                    else
                    {
                        FreePlay = false;
                    }
                    if (gameData.playerOne.userName == Info.userName)
                    {
                        Side = 1;
                        //SetPlayerOne(gameData);

                    }
                    else
                    {
                        Side = 2;
                        //SetPlayerTwo(gameData);

                    }

                    if (!SpwanedMarbels)
                        SelectArena(gameData.ground);

                    SetPlayer();


                    if (NamespaceServer != "_classic")
                        CalculateGameTime(gameData.gameTime / 1000.0f);
                    else
                        SetUICoin(gameData.cost.ToString());

                    Handler_GameReady();
                   // Debug.Log("gameData");
                });
                socket.On("Aim", (s, p, a) =>
                {



                    var aim = JsonUtility.FromJson<AimData>(a[0].ToString());
                    Handler_OnAimrRecive(aim);
                    //  Debug.Log("XXXXXXXx");
                });
                socket.On("Position", (s, p, a) =>
                {


                    var packets = JsonUtility.FromJson<MarbleMovementPackets>(a[0].ToString());
                    QueuemovementPackets.Enqueue(packets);
                    if (QueuemovementPackets.Count > 0 && intergate)
                    {

                        StartCoroutine(ReciveDataMarblesMovment());
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
                    Debug.Log("formationShopInGameRecive");
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
                socket.On("BackToMenu", (s, p, m) => {

                    
                   //// CloseSocket();
                    var Luncher = FindObjectOfType<GameLuncher>();
                    Luncher.BackToMenu();
                    
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
                    Debug.Log("READY" + a[0].ToString());
                });
                socket.On("result", (s, p, a) =>
                {

                    var result = JsonUtility.FromJson<ResualtInRecordModeData>(a[0].ToString());
                    resualtInRecordMode.gameObject.SetActive(true);
                    resualtInRecordMode.Set(result);
                    CancelInvoke("RunTimer");

                });
            }
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
                    //  GameResult.enabled = true;
                    //  GameResult.text = "Winner";
                }
                if (gameData.loser == Info.userName)
                {
                    Debug.Log("Loser");
                    //GameResult.enabled = true;
                    // GameResult.text = "Loser";
                }
                Turn = false;
                // Debug.Log("TRUN3");
            }
            //*********///****///***///
            if (QueuemovementPackets.Count > 0)
            {
                GameDataRecive = true;
            }
            else
            {
               
                    InitializTurn_new();

            }
            IsGoal = -1;
        }
        public void InitializTurn_new()
        {
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
                    StartCoroutine(MoveMarbelsToPositionFromServer(gameData, 1, smooth));
                }

                if (gameData.state == 1)
                {
                    if (gameData.ownerTurn == Side)
                    {
                        Turn = true;
                        EnablerRingEffect = true;
                        soundeffectcontrollLayer2.PlaySoundSoccer(0);/////  play turn sound
                        Handheld.Vibrate();                                  //  Debug.Log("TRUN1");
                                                                             /// KinimaticMarblesAndBall(false);
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
                    StartCoroutine(MoveMarbelsToPositionFromServer(gameData, -1, smooth));
                }
                if (gameData.state == 1)
                {
                    if (gameData.ownerTurn == Side)
                    {
                        Turn = true;

                        EnablerRingEffect = true;
                        soundeffectcontrollLayer2.PlaySoundSoccer(0);/////  play turn sound
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


        }

        public void RunSpawnMarble_New(GameData data, int Side, string selfskin, string enemyskin)
        {
            var count_movement = data.positions.Count;
            // Debug.Log(count_movement);

            for (int i = 0; i < count_movement; i++)
            {
                if (i < 5)
                {
                    var type = SelectBottomFlag(selfskin);
                    var marble = Instantiate(MarbleRegistered[type], new Vector3(data.positions[i].position.x * Side, 2.0f, data.positions[i].position.z), Quaternion.identity, ParentForSpawn);
                    marble.GetComponent<ForceToBall>().ID = i;
                    
                     marble.GetComponent<ForceToBall>().SetSkinMarble(SelectFlag(selfskin));
                }
                else if (i < 10)
                {
                    var type = SelectBottomFlag(enemyskin);
                    var marble = Instantiate(MarbleRegistered[type], new Vector3(data.positions[i].position.x * Side, 2.0f, data.positions[i].position.z), Quaternion.identity, ParentForSpawn);
                    marble.GetComponent<ForceToBall>().ID = i;
                    marble.GetComponent<ForceToBall>().SetSkinMarble(SelectFlag(enemyskin));
                }
            }
            SpwanedMarbels = true;
            UpdateListMarbles();


        }
        public IEnumerator MoveMarbelsToPositionFromServer(GameData data, int side, float speed)
        {
            EnablerRingEffect = false;
            Handler_OnPhysicFreeze(true);
            var count_movement = data.positions.Count;
            for (int i = 0; i < count_movement; i++)
            {
                var index_marble = data.positions[i].id;

                var temp_pos = data.positions[i].position;
                var pos = new Vector3(side * temp_pos.x, Marbles[index_marble].transform.position.y, temp_pos.z);

                //  var rotate = new Vector3(0.0f, 0.0f, 0.0f);


                ///Marbles[index_marble].transform.eulerAngles = rotate;
                Marbles[index_marble].transform.position = pos;
                //Marbles[index_marble].transform.DOMove(pos, 0.01f);
                
            }
            //Debug.Log("MOVVVVEEEEE1212212E:::"+Marbles[10].transform.position); 
            Handler_OnPhysicFreeze(false);
            yield return null;

        }

        public void SelectArena(string name)
        {
            Arena.ForEach(e => {

                if(e.name == name)
                {
                    e.arena.SetActive(true);

                }
                else if(e.name != name)
                {
                    e.arena.SetActive(false);
                }
                else if (e.name =="")
                {
                    Arena[0].arena.SetActive(true);
                }

            });
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

            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 512);
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
            int name = Convert.ToInt32(namesticker);
            int sideshow = Convert.ToInt32(side);///0 left, 1 right
            if (sideshow == 0)
            {
                StickerViwerLeft.StickerSelected = name - 1;
                StickerViwerLeft.gameObject.SetActive(true);
            }
            else
            {
                StickerViwerRight.StickerSelected = name - 1;
                StickerViwerRight.gameObject.SetActive(true);
            }
        }
        public void BadConnectionShow(bool show)
        {

            im_BadConnection.SetActive(show);
           

        }
        public IEnumerator SendDataMarblesMovement()
        {
          //  Debug.Log("ForceT2222T");
            Turn = false;
            List<MarbleMovementData> marbleMovments;
            do
            {
                marbleMovments = new List<MarbleMovementData>();
                Vector3 CurrentPositionMarbles;
                Vector3 velocity;
                for (int i = 0; i < Marbles.Count; i++)
                {
                    CurrentPositionMarbles = Marbles[i].transform.position;
                    velocity = Marbles[i].GetVlocity;
                    marbleMovments.Add(new MarbleMovementData
                    {
                        id = (short)Marbles[i].ID,
                        position = CurrentPositionMarbles,
                       // velocity = velocity,
                        IsRotateBall = Marbles[i].IsRotateBall,
                        IsRotateMarble = Marbles[i].IsRotatingMarble,


                    });
                }
                MarbleMovementPackets movementPackets = new MarbleMovementPackets();
                movementPackets.marbleMovements = marbleMovments;
                

                if (TimeStep == 0.0f)
                {
                    movementPackets.TimeStepPacket = smooth;
                }
                else
                {
                    movementPackets.TimeStepPacket = Mathf.Abs(Time.realtimeSinceStartup - TimeStep);
                   // Debug.Log($"<color=green>TimeStepPacket{movementPackets.TimeStepPacket}</color>");
                }

                var json_packet = JsonUtility.ToJson(movementPackets);
                socket.Emit("Position", json_packet);

                this.TimeStep = TimeStep = Time.realtimeSinceStartup;
                yield return new WaitForSecondsRealtime(smooth);
                   //Debug.Log("SendPositions");
            }
            while (CheckMarbleMove());
            ///Handler_OnPhysicFreeze(true);
            socket.Emit("EndTurn", IsGoal);
            Debug.Log("ISGoal::"+ IsGoal);
            IsGoal = -1;
            
            // Handler_SoftPositionAndRotation();
            this.TimeStep = 0.0f;
            
            yield return null;
        }

        public IEnumerator ReciveDataMarblesMovment()
        {

            DoResetAim();
            EnablerRingEffect = false;
            // Handler_EnableRingMarbleForOpponent(false);
            //yield return new WaitForSecondsRealtime(slider.value);
            while (QueuemovementPackets.Count > 0)
            {
                var movement_packet = QueuemovementPackets.Dequeue();
                var count_movement_in_packet = movement_packet.marbleMovements.Count;
                for (int i = 0; i < count_movement_in_packet; i++)
                {
                    var data = movement_packet.marbleMovements[i];
                    var index_marble = data.id;

                    var temp_pos = data.position;
                    var pos = new Vector3(-1 * temp_pos.x, Marbles[index_marble].transform.position.y, temp_pos.z);

                    /// var rotate = new Vector3(0.0f, movement_packet.marbleMovements[i].rotate_y, 0.0f);
                    Marbles[index_marble].transform.DOMove(pos, movement_packet.TimeStepPacket);
                    if (index_marble < 10)
                    {
                        
                        Marbles[index_marble].RotateMarbleFromServer(data.IsRotateMarble);
                    }
                    else
                    {
                        Marbles[index_marble].RotateBallFromServer(data.IsRotateBall);
                    }
                  /////  Marbles[index_marble].transform.DORotate(rotate, movement_packet.TimeStepPacket);

                }
                yield return new WaitForSecondsRealtime(movement_packet.TimeStepPacket);
                //   Debug.Log("C");
            }
            intergate = true;
            if (GameDataRecive)
            {
                InitializTurn_new();
            }
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
        public void SetUICoin(string Count)
        {
            CoinOrTimeIndicatorOnBiliboard.text = Count;

            //    Debug.Log("UIChanged");
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
            InvokeRepeating("Timerleft", 0.0f, 0.01f* UnityEngine.Time.timeScale);
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
                    // IndicatorTime[0].fillAmount = fill; 
                    StartInvokeTimerLeft();
                }
                else
                {
                    // IndicatorTime[1].fillAmount = fill;
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
        public bool CheckMarbleMove()
        {
            var move = false;
            if (InRecordMode == false)
            {
                for (int i = 0; i < Marbles.Count; i++)
                {
                    if (Marbles[i])
                    {
                        if (Marbles[i].GetComponent<Rigidbody>().velocity != Vector3.zero || Marbles[i].GetComponent<Rigidbody>().angularVelocity != Vector3.zero)
                        {
                            /* if (Marbles[i].GetComponent<Rigidbody>().velocity.magnitude < ThresholdSleep)
                             {
                                 Marbles[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                                 Marbles[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                             }
                             {


                             }*/
                            move = true;
                        }
                    }
                }
            }
            else
            {
             //   Debug.Log("MOvE1");
                for (int i = 0; i < MarblesInRecorodMode.Count; i++)
                {
                 //   Debug.Log("MOvE2");
                    if (MarblesInRecorodMode[i])
                    {
                     //   Debug.Log("MOvE3");
                        if (MarblesInRecorodMode[i].GetComponent<Rigidbody>().velocity != Vector3.zero || MarblesInRecorodMode[i].GetComponent<Rigidbody>().angularVelocity != Vector3.zero)
                        {
                            /*if (MarblesInRecorodMode[i].GetComponent<Rigidbody>().velocity.magnitude < ThresholdSleep)
                            {
                                MarblesInRecorodMode[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                                MarblesInRecorodMode[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                            }
                            {

                                
                            }*/
                           /// Debug.Log("MOvE4");
                            move = true;
                        }
                    }
                }
            }
         //   Debug.Log("MOvE5");
            return move;
        }
        public bool EqeulPosition(Vector3 a, Vector3 b)
        {
            bool eqeul = false;
            var x = a.x - b.x;
            var y = a.z - b.z;
            if (x == 0.0f && y == 0.0f)
            {
                eqeul = true;
            }
            return eqeul;
        }
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
        public void  Emit_ShareSticker(int name)
        {
            socket.Emit("shareSticker" , name + 1);
            Debug.Log("Emit_shareSticker");
        }
        public void  Emit_Message(string message)
        {
            socket.Emit("message", message);
            Debug.Log("Emit_Message");

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

        public void StarCheckMovment()
        {
            Turn = false;
            InvokeRepeating("CheckMOVE", 1f, 1f * Time.timeScale);
        }
        private void CheckMOVE()
        {
            if(CheckMarbleMove() == false)
            {
                Emit_EndTurnInRecordMode();
            }
        }
        private void Emit_EndTurnInRecordMode()
        {
            socket.Emit("EndTurn", SiblArea.Area);
            CancelInvoke("CheckMOVE");
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
