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
    public class BilliardServer : MonoBehaviour
    {
        public bool InRecordMode = false;
        public Transform ParentForspwan;
        public bool SpwnedBall = false;
        public float TimeStep = 0.0f;
        // public float ThresholdSleep = 0.09f;
        #region ServerSettings
        public Socket socket;
        public SocketManager socketmanager;
        [FoldoutGroup("ServerSettings")]
        public string GlobalURL;
        [FoldoutGroup("ServerSettings")]
        public string LocalURL;
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
        public float SpeedPlayRecord = 0.02f;
        [FoldoutGroup("ServerSettings")]
        // public bool TurnRecived = false;
        [FoldoutGroup("ServerSettings")]
        public List<AddressBall> AddressBalls;


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
        [FoldoutGroup("UIInRecordMode")]
        private float H;
        [FoldoutGroup("UIInRecordMode")]
        private float M;
        [FoldoutGroup("UIInRecordMode")]
        private float S;

        private Queue<PositionAndRotateBalls> QueuePositionsBallFromServer;
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
        public bool EightBallEnable = false;
        [FoldoutGroup("GameData")]
        public Basket Basket;
        [FoldoutGroup("GameData")]
        private List<int> PocketedBallsID = new List<int>();
        [FoldoutGroup("GameData")]
        public List<int> BallInBasket = new List<int>();
        // [FoldoutGroup("GameData")]
        // public List<Pockets.Pockets> pockets;
        // [FoldoutGroup("GameData")]
        // public List<Diaco.EightBall.Band.BandsController> BandsControllers;
        [FoldoutGroup("GameData")]
        private List<int> IDImpactToWall;
        [FoldoutGroup("GameData")]
        //private int temp_pitok = -1;
        [FoldoutGroup("GameData")]
        // private Vector3 temp_lastpos;

        //public Slider SendRate;
        //public Text SendRateCounter;
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
        public List<Text> UserNameIndicator;
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
        public Text GameResult;
        [FoldoutGroup("BillboardUI")]
        public Text VisualLogOnScreen;

        #endregion

        #region UnityFunctions
        public void Start()
        {
            if (InRecordMode == false)
            {
                var PocketsInScene = FindObjectsOfType<Pockets.Pockets>().ToList();
                AllowAreaForMoveCueBallRenderer = GameObject.Find("AreaForMoveCueBall").GetComponent<SpriteRenderer>();
                this.Basket = FindObjectOfType<Basket>();
                QueuePositionsBallFromServer = new Queue<PositionAndRotateBalls>();
                QueueCueBallPositionFromServer = new Queue<CueBallData>();
                QueueAimFromServer = new Queue<AimData>();
                IDImpactToWall = new List<int>();
                PocketedBallsID = new List<int>();
                BallInBasket = new List<int>();

                PocketsInScene.ForEach(pocket =>
                {
                    pocket.OnPocket += GameManager_OnPocket0;
                    Debug.Log("pocket find");
                    // pockets[1].OnPocket += GameManager_OnPocket1;
                    // pockets[2].OnPocket += GameManager_OnPocket2;
                    // pockets[3].OnPocket += GameManager_OnPocket3;
                    //pockets[4].OnPocket += GameManager_OnPocket4;
                    // pockets[5].OnPocket += GameManager_OnPocket5;
                });
            }

        }
        public void OnEnable()
        {
            OnInitializeServer();
        }
        #endregion

        #region Server_On
        public void OnInitializeServer()
        {
            SocketOptions options = new SocketOptions();
            options.AutoConnect = true;

            var namespaceserver = FindObjectOfType<GameLuncher>().NamespaceServer;
            this.Namespaceserver = namespaceserver;
            socketmanager = new SocketManager(new Uri(GlobalURL), options);
            socket = socketmanager["/billiard" + namespaceserver];

            socket.On("connect", (s, p, m) =>
            {
                socket.Emit("authToken", ReadToken("token"));
                Debug.Log("Connection");
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

                    gameData = new Structs.GameData();
                    gameData = JsonUtility.FromJson<Diaco.EightBall.Structs.GameData>(m[0].ToString());
                    Turn = false;
                    if (gameData.playerOne.userName == UserName.userName)
                    {
                        SetPlayerOne(gameData);
                    }
                    else
                    {
                        SetPlayerTwo(gameData);
                    }
                    // Handler_GameReady();
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
                    QueuePositionsBallFromServer.Enqueue(positions);

                    if (intergateplayposition == 0)
                    {

                        StartCoroutine(PositionsBallsRecivedFromServer());
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
                    ResultGamePage.SetActive(true);

                    var result = JsonUtility.FromJson<Diaco.EightBall.Structs.ResultGame>(m[0].ToString());
                    Handler_OnGameResult(result);
                    Debug.Log("GameResult");
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
                    Turn = true;
                    Sibl.Area = 4;
                    Handler_GameReady();
                });
                socket.On("result", (s, p, a) =>
                {

                    var result = JsonUtility.FromJson<ResualtInRecordModeData>(a[0].ToString());
                    resualtInRecordMode.gameObject.SetActive(true);
                    resualtInRecordMode.Set(result);
                    CancelInvoke("RunTimer");

                });
            }
            socket.On("disconnect", (s, p, m) =>
            {
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

            socket.Emit("EndRecord", PocketedBallsID, FirstBallImpact, IDImpactToWall, LastPosition);
            //   Debug.Log("End Record And SendData Of Turn");
        }
        public void Emit_LeftGame()
        {
            socket.Emit("left-game");
            CloseConnection();

        }
        public void Emit_PlayAgain()
        {
            socket.Emit("play-again");
        }
        #endregion

        #region BilliardGameFunction

        #region IN NORMAL MODE
        public void SetPlayerOne(Diaco.EightBall.Structs.GameData data)
        {

            if (SpwnedBall == false)
            {
                SpwnBalls(data);
            }
            if (ResultGamePage.activeSelf && data.state != -1)
            {
                ResultGamePage.SetActive(false);
                ResetSharBillboard();
                EnableSharInBiliboard();
            }
            DeletedBallCount = 0;
            CancelCoolDownTimer();
            KinimaticBalls(true);
            SetUserNameInBillboard(data.playerOne.userName, data.playerTwo.userName);
            UpdateAvatarProfile(Avatars.LoadImage(data.playerOne.avatar), Avatars.LoadImage(data.playerTwo.avatar));
            SetTypeCost(Convert.ToInt16(data.costType));
            SetCountCostBillboard(data.cost.ToString());

            ///   SetPositionsBalls(data.positions);
            StartCoroutine(SpwanBallInBasketAndDestroyBallInTable(data));
            QueuePositionsBallFromServer.Enqueue(data.positions);
            StartCoroutine(FASTPlayRecordPositionsBallsAndRecivedFromServer());


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
            if (data.ownerTurn == 1)
            {
                initializTurn(data);

                EnableCoolDown(Side.Left, data.turnTime);
                //  Debug.Log("TimeAndTurnOnwer");
            }
            else
            {
                EnableCoolDown(Side.Right, data.turnTime);
                CheckEnable8BallRightInOtherClient();

                AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>().ActiveAimSystemForShowInOtherClient(true);
                Debug.Log("HE::::::");
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
            /*AddressBalls[0].transform.DOScale(0.33f, 0.1f);
            AddressBalls[0].GetComponent<Rigidbody>().isKinematic = false;
            AddressBalls[0].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            AddressBalls[0].GetComponent<Rigidbody>().WakeUp();
            AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>().resetpos();*/
            Handler_GameReady();
        }
        public void SetPlayerTwo(Diaco.EightBall.Structs.GameData data)
        {

            if (SpwnedBall == false)
            {
                SpwnBalls(data);
            }
            if (ResultGamePage.activeSelf && data.state != -1)
            {
                ResultGamePage.SetActive(false);
                ResetSharBillboard();
                EnableSharInBiliboard();
            }
            DeletedBallCount = 0;
            CancelCoolDownTimer();
            KinimaticBalls(true);
            SetUserNameInBillboard(data.playerTwo.userName, data.playerOne.userName);
            UpdateAvatarProfile(Avatars.LoadImage(data.playerTwo.avatar), Avatars.LoadImage(data.playerOne.avatar));
            SetTypeCost(Convert.ToInt16(data.costType));
            SetCountCostBillboard(data.cost.ToString());


            StartCoroutine(SpwanBallInBasketAndDestroyBallInTable(data));
            QueuePositionsBallFromServer.Enqueue(data.positions);
            StartCoroutine(FASTPlayRecordPositionsBallsAndRecivedFromServer());




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
            if (data.ownerTurn == 2)
            {
                initializTurn(data);
                /// CheckPitok(data.pitok, data.positions.CueBall);
                EnableCoolDown(Side.Left, data.turnTime);
                //  Debug.Log("TimeAndTurnOnwer");
            }
            else
            {
                EnableCoolDown(Side.Right, data.turnTime);
                CheckEnable8BallRightInOtherClient();
                AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>().ActiveAimSystemForShowInOtherClient(true);

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

            Handler_GameReady();
        }

        public void initializTurn(Diaco.EightBall.Structs.GameData data)
        {
            DOVirtual.Float(0f, 0.1f, 1, (x) => { }).OnComplete(() =>
            {
                Turn = true;


                CheckEnable8Ball();

                FirstBallImpact = 0;
                ClearPocketedBallList();
                IDImpactToWall.Clear();
                //  SendEndLimit = 0;


                Debug.Log("Turn");
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
                        /* if(AddressBalls[i].GetComponent<Rigidbody>().velocity.magnitude<ThresholdSleep)
                         {
                             AddressBalls[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                             AddressBalls[i].GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                             Debug.Log("0.5f");

                         }
                         {


                         }*/

                        move = true;
                    }
                }
            }

            return move;
        }

        public IEnumerator PositionsBallsSendToServer()
        {
            PositionAndRotateBalls PositionBalls = new PositionAndRotateBalls();
            do
            {
                if (AddressBalls[0] != null)
                {
                    PositionBalls.CueBall = new Vector2(AddressBalls[0].transform.position.x, AddressBalls[0].transform.position.z);
                    PositionBalls.CueBall_R = AddressBalls[0].transform.eulerAngles;
                }
                if (AddressBalls[1] != null)
                {
                    PositionBalls.Ball_1 = new Vector2(AddressBalls[1].transform.position.x, AddressBalls[1].transform.position.z);
                    PositionBalls.Ball_1_R = AddressBalls[1].transform.eulerAngles;
                }
                if (AddressBalls[2] != null)
                {
                    PositionBalls.Ball_2 = new Vector2(AddressBalls[2].transform.position.x, AddressBalls[2].transform.position.z);
                    PositionBalls.Ball_2_R = AddressBalls[2].transform.eulerAngles;
                }
                if (AddressBalls[3] != null)
                {
                    PositionBalls.Ball_3 = new Vector2(AddressBalls[3].transform.position.x, AddressBalls[3].transform.position.z);
                    PositionBalls.Ball_3_R = AddressBalls[3].transform.eulerAngles;
                }
                if (AddressBalls[4] != null)
                {
                    PositionBalls.Ball_4 = new Vector2(AddressBalls[4].transform.position.x, AddressBalls[4].transform.position.z);
                    PositionBalls.Ball_4_R = AddressBalls[4].transform.eulerAngles;
                }
                if (AddressBalls[5] != null)
                {
                    PositionBalls.Ball_5 = new Vector2(AddressBalls[5].transform.position.x, AddressBalls[5].transform.position.z);
                    PositionBalls.Ball_5_R = AddressBalls[5].transform.eulerAngles;
                }
                if (AddressBalls[6] != null)
                {
                    PositionBalls.Ball_6 = new Vector2(AddressBalls[6].transform.position.x, AddressBalls[6].transform.position.z);
                    PositionBalls.Ball_6_R = AddressBalls[6].transform.eulerAngles;
                }
                if (AddressBalls[7] != null)
                {
                    PositionBalls.Ball_7 = new Vector2(AddressBalls[7].transform.position.x, AddressBalls[7].transform.position.z);
                    PositionBalls.Ball_7_R = AddressBalls[7].transform.eulerAngles;
                }
                if (AddressBalls[8] != null)
                {
                    PositionBalls.Ball_8 = new Vector2(AddressBalls[8].transform.position.x, AddressBalls[8].transform.position.z);
                    PositionBalls.Ball_8_R = AddressBalls[8].transform.eulerAngles;
                }
                if (AddressBalls[9] != null)
                {
                    PositionBalls.Ball_9 = new Vector2(AddressBalls[9].transform.position.x, AddressBalls[9].transform.position.z);
                    PositionBalls.Ball_9_R = AddressBalls[9].transform.eulerAngles;
                }
                if (AddressBalls[10] != null)
                {
                    PositionBalls.Ball_10 = new Vector2(AddressBalls[10].transform.position.x, AddressBalls[10].transform.position.z);
                    PositionBalls.Ball_10_R = AddressBalls[10].transform.eulerAngles;
                }
                if (AddressBalls[11] != null)
                {
                    PositionBalls.Ball_11 = new Vector2(AddressBalls[11].transform.position.x, AddressBalls[11].transform.position.z);
                    PositionBalls.Ball_11_R = AddressBalls[11].transform.eulerAngles;
                }
                if (AddressBalls[12] != null)
                {
                    PositionBalls.Ball_12 = new Vector2(AddressBalls[12].transform.position.x, AddressBalls[12].transform.position.z);
                    PositionBalls.Ball_12_R = AddressBalls[12].transform.eulerAngles;
                }
                if (AddressBalls[13] != null)
                {
                    PositionBalls.Ball_13 = new Vector2(AddressBalls[13].transform.position.x, AddressBalls[13].transform.position.z);
                    PositionBalls.Ball_13_R = AddressBalls[13].transform.eulerAngles;
                }
                if (AddressBalls[14] != null)
                {
                    PositionBalls.Ball_14 = new Vector2(AddressBalls[14].transform.position.x, AddressBalls[14].transform.position.z);
                    PositionBalls.Ball_14_R = AddressBalls[14].transform.eulerAngles;
                }
                if (AddressBalls[15] != null)
                {
                    PositionBalls.Ball_15 = new Vector2(AddressBalls[15].transform.position.x, AddressBalls[15].transform.position.z);
                    PositionBalls.Ball_15_R = AddressBalls[15].transform.eulerAngles;
                }
                if (TimeStep == 0.0f)
                {
                    PositionBalls.TimeStepPacket = SpeedPlayRecord;
                }
                else
                {
                    PositionBalls.TimeStepPacket = Mathf.Abs(Time.realtimeSinceStartup - TimeStep);
                    //  Debug.Log($"<color=green>TimeStepPacket{PositionBalls.TimeStepPacket}</color>");
                }


                Emit_PositionsBalls(PositionBalls);
                TimeStep = Time.realtimeSinceStartup;
                yield return new WaitForSecondsRealtime(SpeedPlayRecord);
            } while (CheckBallsMove());



            //Emit_PositionsBalls(PositionBalls);
            //TimeStep = Time.realtimeSinceStartup;
            Emit_SendDataOfGameOnEndTurn(AddressBalls[0].transform.position);
            TimeStep = 0.0f;

            // AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>().ActiveAimSystemOnPlayRecord(true);

            yield return null;
        }
        public IEnumerator PositionsBallsRecivedFromServer()
        {
            //  yield return new WaitForSecondsRealtime(0.5f);
            var cueball = AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>();

            cueball.ActiveAimSystem(false);
            //ActiveAimSystemForShowInOtherClient(true);
            cueball. Handler_OnHitBall(-1, Vector3.zero);
            do
            {
                
                var PositionBalls = QueuePositionsBallFromServer.Dequeue();

                if (AddressBalls[0] != null)
                {
                    AddressBalls[0].transform.DOMove(new Vector3(PositionBalls.CueBall.x, AddressBalls[0].transform.position.y, PositionBalls.CueBall.y), PositionBalls.TimeStepPacket);
                    AddressBalls[0].transform.DORotate(PositionBalls.CueBall_R, PositionBalls.TimeStepPacket);
                }
                if (AddressBalls[1] != null)
                {
                    AddressBalls[1].transform.DOMove(new Vector3(PositionBalls.Ball_1.x, AddressBalls[1].transform.position.y, PositionBalls.Ball_1.y), PositionBalls.TimeStepPacket);
                    AddressBalls[1].transform.DORotate(PositionBalls.Ball_1_R, PositionBalls.TimeStepPacket);
                }
                if (AddressBalls[2] != null)
                {
                    AddressBalls[2].transform.DOMove(new Vector3(PositionBalls.Ball_2.x, AddressBalls[2].transform.position.y, PositionBalls.Ball_2.y), PositionBalls.TimeStepPacket);
                    AddressBalls[2].transform.DORotate(PositionBalls.Ball_2_R, PositionBalls.TimeStepPacket);
                }
                if (AddressBalls[3] != null)
                {
                    AddressBalls[3].transform.DOMove(new Vector3(PositionBalls.Ball_3.x, AddressBalls[3].transform.position.y, PositionBalls.Ball_3.y), PositionBalls.TimeStepPacket);
                    AddressBalls[3].transform.DORotate(PositionBalls.Ball_3_R, PositionBalls.TimeStepPacket);
                }
                if (AddressBalls[4] != null)
                {
                    AddressBalls[4].transform.DOMove(new Vector3(PositionBalls.Ball_4.x, AddressBalls[4].transform.position.y, PositionBalls.Ball_4.y), PositionBalls.TimeStepPacket);
                    AddressBalls[4].transform.DORotate(PositionBalls.Ball_4_R, PositionBalls.TimeStepPacket);
                }
                if (AddressBalls[5] != null)
                {
                    AddressBalls[5].transform.DOMove(new Vector3(PositionBalls.Ball_5.x, AddressBalls[5].transform.position.y, PositionBalls.Ball_5.y), PositionBalls.TimeStepPacket);
                    AddressBalls[5].transform.DORotate(PositionBalls.Ball_5_R, PositionBalls.TimeStepPacket);
                }
                if (AddressBalls[6] != null)
                {
                    AddressBalls[6].transform.DOMove(new Vector3(PositionBalls.Ball_6.x, AddressBalls[6].transform.position.y, PositionBalls.Ball_6.y), PositionBalls.TimeStepPacket);
                    AddressBalls[6].transform.DORotate(PositionBalls.Ball_6_R, PositionBalls.TimeStepPacket);
                }
                if (AddressBalls[7] != null)
                {
                    AddressBalls[7].transform.DOMove(new Vector3(PositionBalls.Ball_7.x, AddressBalls[7].transform.position.y, PositionBalls.Ball_7.y), PositionBalls.TimeStepPacket);
                    AddressBalls[7].transform.DORotate(PositionBalls.Ball_7_R, PositionBalls.TimeStepPacket);
                }
                if (AddressBalls[8] != null)
                {
                    AddressBalls[8].transform.DOMove(new Vector3(PositionBalls.Ball_8.x, AddressBalls[8].transform.position.y, PositionBalls.Ball_8.y), PositionBalls.TimeStepPacket);
                    AddressBalls[8].transform.DORotate(PositionBalls.Ball_8_R, PositionBalls.TimeStepPacket);
                }
                if (AddressBalls[9] != null)
                {
                    AddressBalls[9].transform.DOMove(new Vector3(PositionBalls.Ball_9.x, AddressBalls[9].transform.position.y, PositionBalls.Ball_9.y), PositionBalls.TimeStepPacket);
                    AddressBalls[9].transform.DORotate(PositionBalls.Ball_9_R, PositionBalls.TimeStepPacket);
                }
                if (AddressBalls[10] != null)
                {
                    AddressBalls[10].transform.DOMove(new Vector3(PositionBalls.Ball_10.x, AddressBalls[10].transform.position.y, PositionBalls.Ball_10.y), PositionBalls.TimeStepPacket);
                    AddressBalls[10].transform.DORotate(PositionBalls.Ball_10_R, PositionBalls.TimeStepPacket);
                }
                if (AddressBalls[11] != null)
                {
                    AddressBalls[11].transform.DOMove(new Vector3(PositionBalls.Ball_11.x, AddressBalls[11].transform.position.y, PositionBalls.Ball_11.y), PositionBalls.TimeStepPacket);
                    AddressBalls[11].transform.DORotate(PositionBalls.Ball_11_R, PositionBalls.TimeStepPacket);
                }
                if (AddressBalls[12] != null)
                {
                    AddressBalls[12].transform.DOMove(new Vector3(PositionBalls.Ball_12.x, AddressBalls[12].transform.position.y, PositionBalls.Ball_12.y), PositionBalls.TimeStepPacket);
                    AddressBalls[12].transform.DORotate(PositionBalls.Ball_12_R, PositionBalls.TimeStepPacket);
                }
                if (AddressBalls[13] != null)
                {
                    AddressBalls[13].transform.DOMove(new Vector3(PositionBalls.Ball_13.x, AddressBalls[13].transform.position.y, PositionBalls.Ball_13.y), PositionBalls.TimeStepPacket);
                    AddressBalls[13].transform.DORotate(PositionBalls.Ball_13_R, PositionBalls.TimeStepPacket);
                }
                if (AddressBalls[14] != null)
                {
                    AddressBalls[14].transform.DOMove(new Vector3(PositionBalls.Ball_14.x, AddressBalls[14].transform.position.y, PositionBalls.Ball_14.y), PositionBalls.TimeStepPacket);
                    AddressBalls[14].transform.DORotate(PositionBalls.Ball_14_R, PositionBalls.TimeStepPacket);
                }
                if (AddressBalls[15] != null)
                {
                    AddressBalls[15].transform.DOMove(new Vector3(PositionBalls.Ball_15.x, AddressBalls[15].transform.position.y, PositionBalls.Ball_15.y), PositionBalls.TimeStepPacket);
                    AddressBalls[15].transform.DORotate(PositionBalls.Ball_15_R, PositionBalls.TimeStepPacket);
                }
                yield return new WaitForSecondsRealtime(PositionBalls.TimeStepPacket);
            } while (QueuePositionsBallFromServer.Count > 0);


            // cueball.resetpos();
            intergateplayposition = 0;



        }
        public IEnumerator FASTPlayRecordPositionsBallsAndRecivedFromServer()
        {

            var cueball = AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>();
            cueball.ActiveAimSystem(false);
            do
            {
                var PositionBalls = QueuePositionsBallFromServer.Dequeue();

                if (AddressBalls[0] != null)
                {
                    AddressBalls[0].transform.DOMove(new Vector3(PositionBalls.CueBall.x, AddressBalls[0].transform.position.y, PositionBalls.CueBall.y), 0.001f);
                    AddressBalls[0].transform.DORotate(PositionBalls.CueBall_R, 0.001f);
                }
                if (AddressBalls[1] != null)
                {
                    AddressBalls[1].transform.DOMove(new Vector3(PositionBalls.Ball_1.x, AddressBalls[1].transform.position.y, PositionBalls.Ball_1.y), 0.001f);
                    AddressBalls[1].transform.DORotate(PositionBalls.Ball_1_R, 0.001f);
                }
                if (AddressBalls[2] != null)
                {
                    AddressBalls[2].transform.DOMove(new Vector3(PositionBalls.Ball_2.x, AddressBalls[2].transform.position.y, PositionBalls.Ball_2.y), 0.001f);
                    AddressBalls[2].transform.DORotate(PositionBalls.Ball_2_R, 0.001f);
                }
                if (AddressBalls[3] != null)
                {
                    AddressBalls[3].transform.DOMove(new Vector3(PositionBalls.Ball_3.x, AddressBalls[3].transform.position.y, PositionBalls.Ball_3.y), 0.001f);
                    AddressBalls[3].transform.DORotate(PositionBalls.Ball_3_R, 0.001f);
                }
                if (AddressBalls[4] != null)
                {
                    AddressBalls[4].transform.DOMove(new Vector3(PositionBalls.Ball_4.x, AddressBalls[4].transform.position.y, PositionBalls.Ball_4.y), 0.001f);
                    AddressBalls[4].transform.DORotate(PositionBalls.Ball_4_R, 0.001f);
                }
                if (AddressBalls[5] != null)
                {
                    AddressBalls[5].transform.DOMove(new Vector3(PositionBalls.Ball_5.x, AddressBalls[5].transform.position.y, PositionBalls.Ball_5.y), 0.001f);
                    AddressBalls[5].transform.DORotate(PositionBalls.Ball_5_R, 0.001f);
                }
                if (AddressBalls[6] != null)
                {
                    AddressBalls[6].transform.DOMove(new Vector3(PositionBalls.Ball_6.x, AddressBalls[6].transform.position.y, PositionBalls.Ball_6.y), 0.001f);
                    AddressBalls[6].transform.DORotate(PositionBalls.Ball_6_R, 0.001f);
                }
                if (AddressBalls[7] != null)
                {
                    AddressBalls[7].transform.DOMove(new Vector3(PositionBalls.Ball_7.x, AddressBalls[7].transform.position.y, PositionBalls.Ball_7.y), 0.001f);
                    AddressBalls[7].transform.DORotate(PositionBalls.Ball_7_R, 0.001f);
                }
                if (AddressBalls[8] != null)
                {
                    AddressBalls[8].transform.DOMove(new Vector3(PositionBalls.Ball_8.x, AddressBalls[8].transform.position.y, PositionBalls.Ball_8.y), 0.001f);
                    AddressBalls[8].transform.DORotate(PositionBalls.Ball_8_R, 0.001f);
                }
                if (AddressBalls[9] != null)
                {
                    AddressBalls[9].transform.DOMove(new Vector3(PositionBalls.Ball_9.x, AddressBalls[9].transform.position.y, PositionBalls.Ball_9.y), 0.001f);
                    AddressBalls[9].transform.DORotate(PositionBalls.Ball_9_R, 0.001f);
                }
                if (AddressBalls[10] != null)
                {
                    AddressBalls[10].transform.DOMove(new Vector3(PositionBalls.Ball_10.x, AddressBalls[10].transform.position.y, PositionBalls.Ball_10.y), 0.001f);
                    AddressBalls[10].transform.DORotate(PositionBalls.Ball_10_R, 0.001f);
                }
                if (AddressBalls[11] != null)
                {
                    AddressBalls[11].transform.DOMove(new Vector3(PositionBalls.Ball_11.x, AddressBalls[11].transform.position.y, PositionBalls.Ball_11.y), 0.001f);
                    AddressBalls[11].transform.DORotate(PositionBalls.Ball_11_R, 0.001f);
                }
                if (AddressBalls[12] != null)
                {
                    AddressBalls[12].transform.DOMove(new Vector3(PositionBalls.Ball_12.x, AddressBalls[12].transform.position.y, PositionBalls.Ball_12.y), 0.001f);
                    AddressBalls[12].transform.DORotate(PositionBalls.Ball_12_R, 0.001f);
                }
                if (AddressBalls[13] != null)
                {
                    AddressBalls[13].transform.DOMove(new Vector3(PositionBalls.Ball_13.x, AddressBalls[13].transform.position.y, PositionBalls.Ball_13.y), 0.001f);
                    AddressBalls[13].transform.DORotate(PositionBalls.Ball_13_R, 0.001f);
                }
                if (AddressBalls[14] != null)
                {
                    AddressBalls[14].transform.DOMove(new Vector3(PositionBalls.Ball_14.x, AddressBalls[14].transform.position.y, PositionBalls.Ball_14.y), 0.001f);
                    AddressBalls[14].transform.DORotate(PositionBalls.Ball_14_R, 0.001f);
                }
                if (AddressBalls[15] != null)
                {
                    AddressBalls[15].transform.DOMove(new Vector3(PositionBalls.Ball_15.x, AddressBalls[15].transform.position.y, PositionBalls.Ball_15.y), 0.001f);
                    AddressBalls[15].transform.DORotate(PositionBalls.Ball_15_R, 0.001f);
                }
                yield return new WaitForSecondsRealtime(0.001f);
            } while (QueuePositionsBallFromServer.Count > 0);

            CheckPitok(gameData.pitok, gameData.positions.CueBall);
            KinimaticBalls(false);
            intergateplayposition = 0;
           
            //cueball.resetpos();
        }
        /*public void SetPositionsBalls(PositionAndRotateBalls PositionBalls)
        {
            if (AddressBalls[0] != null)
            {
                AddressBalls[0].transform.DOMove(new Vector3(PositionBalls.CueBall.x, AddressBalls[0].transform.position.y, PositionBalls.CueBall.y), 0);
                AddressBalls[0].transform.DORotate(PositionBalls.CueBall_R, 0);
            }
            if (AddressBalls[1] != null)
            {
                AddressBalls[1].transform.DOMove(new Vector3(PositionBalls.Ball_1.x, AddressBalls[1].transform.position.y, PositionBalls.Ball_1.y), 0);
                AddressBalls[1].transform.DORotate(PositionBalls.Ball_1_R, 0);
            }
            if (AddressBalls[2] != null)
            {
                AddressBalls[2].transform.DOMove(new Vector3(PositionBalls.Ball_2.x, AddressBalls[2].transform.position.y, PositionBalls.Ball_2.y), 0);
                AddressBalls[2].transform.DORotate(PositionBalls.Ball_2_R, 0);
            }
            if (AddressBalls[3] != null)
            {
                AddressBalls[3].transform.DOMove(new Vector3(PositionBalls.Ball_3.x, AddressBalls[3].transform.position.y, PositionBalls.Ball_3.y), 0);
                AddressBalls[3].transform.DORotate(PositionBalls.Ball_3_R, 0);
            }
            if (AddressBalls[4] != null)
            {
                AddressBalls[4].transform.DOMove(new Vector3(PositionBalls.Ball_4.x, AddressBalls[4].transform.position.y, PositionBalls.Ball_4.y), 0);
                AddressBalls[4].transform.DORotate(PositionBalls.Ball_4_R, 0);
            }
            if (AddressBalls[5] != null)
            {
                AddressBalls[5].transform.DOMove(new Vector3(PositionBalls.Ball_5.x, AddressBalls[5].transform.position.y, PositionBalls.Ball_5.y), 0);
                AddressBalls[5].transform.DORotate(PositionBalls.Ball_5_R, 0);
            }
            if (AddressBalls[6] != null)
            {
                AddressBalls[6].transform.DOMove(new Vector3(PositionBalls.Ball_6.x, AddressBalls[6].transform.position.y, PositionBalls.Ball_6.y), 0);
                AddressBalls[6].transform.DORotate(PositionBalls.Ball_6_R, 0);
            }
            if (AddressBalls[7] != null)
            {
                AddressBalls[7].transform.DOMove(new Vector3(PositionBalls.Ball_7.x, AddressBalls[7].transform.position.y, PositionBalls.Ball_7.y), 0);
                AddressBalls[7].transform.DORotate(PositionBalls.Ball_7_R, 0);
            }
            if (AddressBalls[8] != null)
            {
                AddressBalls[8].transform.DOMove(new Vector3(PositionBalls.Ball_8.x, AddressBalls[8].transform.position.y, PositionBalls.Ball_8.y), 0);
                AddressBalls[8].transform.DORotate(PositionBalls.Ball_8_R, 0);
            }
            if (AddressBalls[9] != null)
            {
                AddressBalls[9].transform.DOMove(new Vector3(PositionBalls.Ball_9.x, AddressBalls[9].transform.position.y, PositionBalls.Ball_9.y), 0);
                AddressBalls[9].transform.DORotate(PositionBalls.Ball_9_R, 0);
            }
            if (AddressBalls[10] != null)
            {
                AddressBalls[10].transform.DOMove(new Vector3(PositionBalls.Ball_10.x, AddressBalls[10].transform.position.y, PositionBalls.Ball_10.y), 0);
                AddressBalls[10].transform.DORotate(PositionBalls.Ball_10_R, 0);
            }
            if (AddressBalls[11] != null)
            {
                AddressBalls[11].transform.DOMove(new Vector3(PositionBalls.Ball_11.x, AddressBalls[11].transform.position.y, PositionBalls.Ball_11.y), 0);
                AddressBalls[11].transform.DORotate(PositionBalls.Ball_11_R, 0);
            }
            if (AddressBalls[12] != null)
            {
                AddressBalls[12].transform.DOMove(new Vector3(PositionBalls.Ball_12.x, AddressBalls[12].transform.position.y, PositionBalls.Ball_12.y), 0);
                AddressBalls[12].transform.DORotate(PositionBalls.Ball_12_R, 0);
            }
            if (AddressBalls[13] != null)
            {
                AddressBalls[13].transform.DOMove(new Vector3(PositionBalls.Ball_13.x, AddressBalls[13].transform.position.y, PositionBalls.Ball_13.y), 0);
                AddressBalls[13].transform.DORotate(PositionBalls.Ball_13_R, 0);
            }
            if (AddressBalls[14] != null)
            {
                AddressBalls[14].transform.DOMove(new Vector3(PositionBalls.Ball_14.x, AddressBalls[14].transform.position.y, PositionBalls.Ball_14.y), 0);
                AddressBalls[14].transform.DORotate(PositionBalls.Ball_14_R, 0);
            }
            if (AddressBalls[15] != null)
            {
                AddressBalls[15].transform.DOMove(new Vector3(PositionBalls.Ball_15.x, AddressBalls[15].transform.position.y, PositionBalls.Ball_15.y), 0);
                AddressBalls[15].transform.DORotate(PositionBalls.Ball_15_R, 0);
            }
        }*/
        public IEnumerator CueBallPositionRecivedFromServer()
        {
            // AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>().ActiveAimSystemOnPlayRecord(false);
            var PositionBall = QueueCueBallPositionFromServer.Dequeue();
            AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>().ActiveAimSystemForShowInOtherClient(false);
            AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>().CueBallMoveFromServer(PositionBall, 0.02f);
           /// Debug.Log("Recive2");
            yield return null;
        }

        public IEnumerator AimRecivedFromServer()
        {
            if (AddressBalls[0] != null)
            {
                var cueball = AddressBalls[0].GetComponent<Diaco.EightBall.CueControllers.HitBallController>();
                cueball.ActiveAimSystemForShowInOtherClient(true);
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
                        AddressBalls[i].GetComponent<Rigidbody>().isKinematic = true;
                        AddressBalls[i].GetComponent<Rigidbody>().useGravity = false;
                        AddressBalls[i].GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
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

        public IEnumerator SpwanBallInBasketAndDestroyBallInTable(Diaco.EightBall.Structs.GameData data)
        {
            for (int i = 0; i < data.deletedBalls.Count; i++)
            {

                if (!BallInBasket.Contains(data.deletedBalls[i]))
                {
                    Basket.AddToQueue(data.deletedBalls[i]);
                    BallInBasket.Add(data.deletedBalls[i]);
                    Destroy(AddressBalls[data.deletedBalls[i]].gameObject);
                    DeletedBallCount++;

                }
            }
           /// Debug.Log("basket length : " + BallInBasket.Count);
            yield return new WaitForSecondsRealtime(1f);
            
            for (int i = 0; i < BallInBasket.Count; i++)///check for Wrong Ball In Basket
            {
                var id = BallInBasket[i];
                if (!data.deletedBalls.Contains(id))
                {
                    var ball = Instantiate(BallsPrefabs[id], new Vector3(0.0f, 0.08885605f, 0.0f), Quaternion.identity, ParentForspwan);
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
                            BallInBasket.Remove(id);
                            i = -1;
                           
                        }
                    }

                }
  // Debug.Log("VBBBVBVBVBVBVBVBVBVBV"+id);
            }
            yield return new WaitForSecondsRealtime(1f);
            StartCoroutine(Basket.ExtractBallFast());
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
            EightBallEnable = false;

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
        public void SpwnBalls(GameData gameData)
        {
            Vector3[] positions = new Vector3[16] {
                new Vector3(gameData.positions.CueBall.x,0.08885605f,gameData.positions.CueBall.y),
                new Vector3(gameData.positions.Ball_1.x,0.08885605f,gameData.positions.Ball_1.x),
                new Vector3(gameData.positions.Ball_2.x,0.08885605f,gameData.positions.Ball_2.y),
                new Vector3(gameData.positions.Ball_3.x,0.08885605f,gameData.positions.Ball_3.y),
                new Vector3(gameData.positions.Ball_4.x,0.08885605f,gameData.positions.Ball_4.y),
                new Vector3(gameData.positions.Ball_5.x,0.08885605f,gameData.positions.Ball_5.y),
                new Vector3(gameData.positions.Ball_6.x,0.08885605f,gameData.positions.Ball_6.y),
                new Vector3(gameData.positions.Ball_7.x,0.08885605f,gameData.positions.Ball_7.y),
                new Vector3(gameData.positions.Ball_8.x,0.08885605f,gameData.positions.Ball_8.y),
                new Vector3(gameData.positions.Ball_9.x,0.08885605f,gameData.positions.Ball_9.y),
                new Vector3(gameData.positions.Ball_10.x,0.08885605f,gameData.positions.Ball_10.y),
                new Vector3(gameData.positions.Ball_11.x,0.08885605f,gameData.positions.Ball_1.y),
                new Vector3(gameData.positions.Ball_12.x,0.08885605f,gameData.positions.Ball_2.y),
                new Vector3(gameData.positions.Ball_13.x,0.08885605f,gameData.positions.Ball_3.y),
                new Vector3(gameData.positions.Ball_14.x,0.08885605f,gameData.positions.Ball_4.y),
                new Vector3(gameData.positions.Ball_15.x,0.08885605f,gameData.positions.Ball_5.y),

            };
            for (int i = 0; i < BallsPrefabs.Count; i++)
            {

                if (i == 0)
                {
                    var cueball = FindObjectOfType<Diaco.EightBall.CueControllers.HitBallController>();
                    cueball.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);

                    cueball.transform.position = positions[i];
                    AddressBalls.Add(cueball.GetComponent<AddressBall>());
                }
                else
                {
                    var ball = Instantiate(BallsPrefabs[i], positions[i], Quaternion.identity, ParentForspwan);
                    ball.transform.eulerAngles = new Vector3(0.0f, 90f, 180f);
                    ball.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
                    AddressBalls.Add(ball.GetComponent<AddressBall>());
                }


            }
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
        #endregion
        #region IN RECORD MODE
        /// <summary>
        /// /Y = 0.1060765
        /// </summary>
        /// <param name="whiteball"></param>
        /// <param name="colorball"></param>
        /// <param name="siblpos"></param>
        private void SpawnAssetInRecordMode(Vector3 whiteball,  Vector3 colorball, Vector3 siblpos)
        {
            var w_ball = FindObjectOfType<Diaco.EightBall.CueControllers.HitBallController>();
            w_ball.transform.localScale = new Vector3(0.33f, 0.33f, 0.33f);
            w_ball.transform.position = whiteball;
            
            //var rand = UnityEngine.Random.Range(1, BallsPrefabs.Count];
            var c_ball = Instantiate(BallsPrefabs[1], colorball ,Quaternion.identity, ParentForspwan);
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
                yield return new WaitForSeconds(0.1f);
            }
            while (CheckBallsMove());
            Emit_EndTurnInRecordMode();
            yield return null;
        }
        private void Emit_EndTurnInRecordMode()
        {
            socket.Emit("EndTurn", Sibl.Area);
            Debug.Log($"<color=blue><b>EndTurn</b>{Sibl.Area}</color>");
        }
        public void ClearSceneInRecordMode()
        {
            if (AddressBalls.Count>0)
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
        private void SetUserNameInBillboard(string userleft , string userright)
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
        private void EnableCoolDown(Diaco.EightBall.Structs.Side side, int Time)
        {
            PlayerCoolDowns[0].fillAmount = 1;
            PlayerCoolDowns[1].fillAmount = 1;
            Timer = Time/1000;
            ////  Debug.Log("Time" + Timer);

            if (side == 0)
            {
                InvokeRepeating("SetLeftCoolDown", 0.0f, 0.01f * UnityEngine.Time.timeScale);
              
                 //Debug.Log("EnableCooldownLeft");
            }
            else
            {
                InvokeRepeating("SetRightCoolDown", 0.0f, 0.01f * UnityEngine.Time.timeScale);
                  /// Debug.Log("EnableCooldownRight");
            }
           // Debug.Log("CoolDown");
        }
        private void SetLeftCoolDown()
        {

            var unit_time = 1.0f / Timer;
            var fill = PlayerCoolDowns[0].fillAmount - unit_time/100;
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
            var fill = PlayerCoolDowns[1].fillAmount - unit_time/100;
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
            if(cost == 0)///cup
            {
                CostTypeIndicator.sprite = Cup_sprite;
              ///  Debug.Log("Cup");
            }
            else if( cost == 1)//coin
            {

                CostTypeIndicator.sprite = Coin_sprite;
              //  Debug.Log("Coin");
            }

            else if(cost ==2)//gem
            {
                CostTypeIndicator.sprite = Gem_sprite;
                //Debug.Log("Gem");
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
                EightBallEnable = true;
                UI_Biliboard_SharLeft[0].interactable = true;
                UI_Biliboard_SharLeft[0].image.sprite = EightBall;
                //  Debug.Log("EightBallEnable");
            }


        }
        public void CheckEnable8BallRightInOtherClient()
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
                EightBallEnable = true;
                UI_Biliboard_SharLeft[0].interactable = true;
                UI_Biliboard_SharLeft[0].image.sprite = EightBall;
                //  Debug.Log("EightBallEnable");
            }


        }
        public void CheckPitok(int pitok , Vector2 pos)
        {
            if (pitok == 1)
            {
                var lastpos = new Vector3(pos.x, 0.08885605f, pos.y);
            
                NormalPitok(lastpos);
            }
            else if (pitok == 2)
            {
                var lastpos = new Vector3(pos.x, 0.08885605f, pos.y);
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
            //cueball.gameObject.SetActive(true); 
            var ooo = new Vector3(lastpos.x, lastpos.y, lastpos.z);
            cueball.transform.position = ooo;
            //   cueball.GetComponent<Diaco.EightBall.CueControllers.HitBallController>().CUEWoodSetPosition(Vector3.zero);
            cueball.transform.DOScale(0.33f, 0.1f);
            cueball.GetComponent<Rigidbody>().isKinematic = false;
            cueball.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            cueball.GetComponent<Rigidbody>().WakeUp();
            cueball.GetComponent<Diaco.EightBall.CueControllers.HitBallController>().EnableYFix = true;
           // Debug.Log("Pitok1" + ooo);

            //  });

        }
        public void Pitok1_3(Vector3 lastpos)
        {
            // DOVirtual.Float(0, 1, 0.2f, (x) => { }).OnComplete(() =>
            //  {

            Pitok = 2;
            var cueball = AddressBalls[0];
            // cueball.gameObject.SetActive(true);
            var ooo = new Vector3(lastpos.x, lastpos.y, lastpos.z);
            cueball.transform.position = ooo;
            /// cueball.GetComponent<Diaco.EightBall.CueControllers.HitBallController>().CUEWoodSetPosition(Vector3.zero);
            cueball.transform.DOScale(0.33f, 0.1f);
            cueball.GetComponent<Rigidbody>().isKinematic = false;
            cueball.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            cueball.GetComponent<Rigidbody>().WakeUp();
            cueball.GetComponent<Diaco.EightBall.CueControllers.HitBallController>().EnableYFix = true;
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
            if (EightBallEnable == false)
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
            else if (EightBallEnable == true && id == 8)
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

        public event Action<Diaco.EightBall.Structs.ResultGame > OnGameResult;
        protected void Handler_OnGameResult( Diaco.EightBall.Structs.ResultGame result)
        {
            if(OnGameResult != null)
            {
                OnGameResult(result);
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

}
