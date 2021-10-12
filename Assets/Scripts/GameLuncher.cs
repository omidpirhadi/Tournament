using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class GameLuncher : MonoBehaviour
{
    public NavigationUI navigationUi;

    public Camera mainCam;
    public string NamespaceServer;


    public GameObject MainMenuPrefab;
    private GameObject MainMenu;
  
    public GameObject SoccerGamePrefab;
    private GameObject SoccerGame;
    // public Diaco.SoccerStar.Server.ServerManager ServerSoccerController;

    public GameObject SoccerRecordGamePrefab;
    private GameObject SoccerRecordGame;
    // public Diaco.SoccerStar.Server.ServerManager ServerSoccerRecordGameController;

    public GameObject BilliardGamePrefab;
    private GameObject BilliardGame;
    /// public Diaco.EightBall.Server.BilliardServer ServerBilliardController;

    public GameObject BilliardRecordGamePrefabs;
    private GameObject BilliardRecordGame;
    //public Diaco.EightBall.Server.BilliardServer ServerBilliardRecordGameController;

   
   

    public CanvasGroup FadeOutFadeInGroup;
    public float FadeInDuration = 0.2f;
    [SerializeField] private bool FadeInCompelete = false;
    public float FadeOutDuration = 0.2f;
    [SerializeField] private bool FadeOutCompelete = false;
    [SerializeField] private bool LoadGameCompeleted = false;
    void Awake()
    {
        //if (Application.isMobilePlatform)
         //   QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;
        Diaco.ImageContainerTool.ImageContainer.InitializeTexture();
        MainMenu = Instantiate(MainMenuPrefab);
        navigationUi = MainMenu.GetComponentInChildren<ServerUI>().navigationUi;

    }



    private void Start()
    {
        mainCam.gameObject.SetActive(false);
    }

    private float timer;

   

    private void ServerGamesController_GameReady()
    {
        LoadGameCompeleted = true;
       // Debug.Log("GameREADY");
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index">0 = Soccer 2 = SoccerRecordGame , 1 = Billiard , 3 = BilliardRecordGame </param>
    /// <param name="Namespace">The NameSpace of Server</param>
    public void SetNameSpaceServer(int index  , string Namespace)
    {

        NamespaceServer = Namespace;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="index">0 = Soccer 2 = SoccerRecordGame , 1 = Billiard , 3 = BilliardRecordGame</param>
    public void SwitchScene(int index)
    {
        StartCoroutine(SwitchSceneRun(index));
    }

  /*  public void GotoSceneGameWithFriend(int index)
    {
        
        if (index == 0)
        {

           SwitchScene(1);
            Debug.Log("SoccerGamelunch");
          
        }
        else if(index == 1)
        {
            SwitchScene(2);
            Debug.Log("BilliardGamelunch");
        
        }
    }*/
    public void BackToMenu()
    {
        StartCoroutine(BackToMenuWithClearDataScene());
    }
    private IEnumerator SwitchSceneRun(int index)
    {
        navigationUi.CloseAllPopUp();
        FadeIn();
        yield return new WaitUntil(() => FadeInCompelete);
        if (index == 0)
        {
            Time.timeScale = 1;
          ///  Physics.sleepThreshold = 0.01f;
            if (MainMenu)
            {
                MainMenu.GetComponentInChildren<ServerUI>().CloseConnectionUIToServer(); 
                Destroy(MainMenu);
                MainMenu = null;
            }
            if (SoccerRecordGame)
            {
                Destroy(SoccerRecordGame);
                SoccerRecordGame = null;
            }
            if (BilliardGame)
            {
                Destroy(BilliardGame);
                BilliardGame = null;
            }
            if (BilliardRecordGame)
            {
                Destroy(BilliardRecordGame);
                BilliardRecordGame = null;
            }


            if (SoccerGame == null)
            {
                SoccerGame = Instantiate(SoccerGamePrefab);

                SoccerGame.GetComponentInChildren<Diaco.SoccerStar.Server.ServerManager>().GameReady += ServerGamesController_GameReady;
            }
          
        }
        else if (index == 1)
        {
            Time.timeScale = 2;
            //Physics.sleepThreshold = 0.000001f;
            if (MainMenu)
            {
                MainMenu.GetComponentInChildren<ServerUI>().CloseConnectionUIToServer();
                Destroy(MainMenu);
                MainMenu = null;
            }
            if (SoccerRecordGame)
            {
                Destroy(SoccerRecordGame);
                SoccerRecordGame = null;
            }

            if (SoccerGame)
            {
                Destroy(SoccerGame);
                SoccerGame = null;
            }
            if (BilliardRecordGame)
            {
                Destroy(BilliardRecordGame);
                BilliardRecordGame = null;
            }
            if (BilliardGame == null)
            {
                BilliardGame = Instantiate(BilliardGamePrefab);
                BilliardGame.GetComponentInChildren<Diaco.EightBall.Server.BilliardServer>().GameReady += ServerGamesController_GameReady;
            }

        }
        else if(index == 2)
        {
            Time.timeScale = 1;
           // Physics.sleepThreshold = 0.01f;
            if (MainMenu)
            {
                MainMenu.GetComponentInChildren<ServerUI>().CloseConnectionUIToServer();
                Destroy(MainMenu);
                MainMenu = null;
            }
            if (SoccerGame)
            {
                Destroy(SoccerGame);
                SoccerGame = null;
            }
            if (BilliardGame)
            {
                Destroy(BilliardGame);
                BilliardGame = null;
            }
            if (BilliardRecordGame)
            {
                Destroy(BilliardRecordGame);
                BilliardRecordGame = null;
            }
            if (SoccerRecordGame == null)
            {
                SoccerRecordGame = Instantiate(SoccerRecordGamePrefab);
                SoccerRecordGame.GetComponentInChildren<Diaco.SoccerStar.Server.ServerManager>().GameReady += ServerGamesController_GameReady;
            }
          
        }
        else if (index == 3)
        {
            Time.timeScale = 2;
           // Physics.sleepThreshold = 0.001f;
            if (MainMenu)
            {
                MainMenu.GetComponentInChildren<ServerUI>().CloseConnectionUIToServer();
                Destroy(MainMenu);
            }
            if (SoccerRecordGame)
            {
                Destroy(SoccerRecordGame);
                SoccerRecordGame = null;
            }
            if (SoccerGame)
            {
                Destroy(SoccerGame);
                SoccerGame = null;
            }
            if (BilliardGame)
            {
                Destroy(BilliardGame);
                BilliardGame = null;
            }
            if (BilliardRecordGame == null)
            {
                BilliardRecordGame = Instantiate(BilliardRecordGamePrefabs);
                BilliardRecordGame.GetComponentInChildren<Diaco.EightBall.Server.BilliardServer>().GameReady += ServerGamesController_GameReady;
                Debug.Log("BilliardRecordGameLoadedGame");
            }
           //
           // Debug.Log(1);
        }
        yield return new WaitUntil(() => LoadGameCompeleted);
        FadeOut();
        FadeInCompelete = false;
        FadeOutCompelete = false;
        LoadGameCompeleted = false;
    }
    private IEnumerator BackToMenuWithClearDataScene()
    {



        FadeIn();
        //Debug.Log("WaitForBackToMenu");
        yield return new WaitUntil(() => FadeInCompelete);
        if (SoccerGame)
        {
          //  StartCoroutine(ServerSoccerController.ResetDataInNormalMode());
          ////  ServerSoccerController.IntiGameData();
            yield return new WaitForSeconds(1.0f);
            ////  ServerSoccerController.CloseSocket();
            StartCoroutine(ShowMainMenu());
            
            Debug.Log("ClearSoccerData");
        }
        else if(BilliardGame)
        {
            ///StartCoroutine(ServerBilliardController.ResetData());
           // ServerBilliardController.IntiGameData();
            yield return new WaitForSeconds(1.0f);
           /// ServerBilliardController.CloseConnection();
          StartCoroutine(  ShowMainMenu());
            Debug.Log("ClearBilliardData");

        }
        else if(SoccerRecordGame)
        {
          //  ServerSoccerRecordGameController.ClearSceneInRecordMode();
          //  ServerSoccerRecordGameController.CloseSocket();
           StartCoroutine( ShowMainMenu());
            Debug.Log("ClearBilliardRecordModeData");
        }
        else if(BilliardRecordGame)
        {
            // ServerBilliardRecordGameController.ClearSceneInRecordMode();
            /// ServerBilliardRecordGameController.CloseConnection();
            StartCoroutine(ShowMainMenu());
            Debug.Log("ClearBilliardData");
        }
        yield return new WaitForSeconds(1.1f);


        if(MainMenu == null)
        {
            MainMenu = Instantiate(MainMenuPrefab);
            navigationUi = MainMenu.GetComponentInChildren<ServerUI>().navigationUi;
        }
       
        yield return new WaitForSeconds(0.1f);
        navigationUi.CloseAllPopUp();
        navigationUi.SwitchUI("selectgame");
       // MainMenu.GetComponentInChildren<ServerUI>().ConnectToUIServer();
        FadeOut();
        yield return new WaitUntil(() => FadeOutCompelete);
        FadeInCompelete = false;
        FadeOutCompelete = false;
    }
   
    private IEnumerator ShowMainMenu()
    {

        if (SoccerGame)
            SoccerGame.GetComponentInChildren<Diaco.SoccerStar.Server.ServerManager>().GameReady -= ServerGamesController_GameReady;
        if (BilliardGame)
            BilliardGame.GetComponentInChildren<Diaco.EightBall.Server.BilliardServer>().GameReady -= ServerGamesController_GameReady;
        if (SoccerRecordGame)
            SoccerRecordGame.GetComponentInChildren<Diaco.SoccerStar.Server.ServerManager>().GameReady -= ServerGamesController_GameReady;
        if (BilliardRecordGame)
            BilliardRecordGame.GetComponentInChildren<Diaco.EightBall.Server.BilliardServer>().GameReady -= ServerGamesController_GameReady;
        Destroy(SoccerGame);
        Destroy(BilliardGame );
        Destroy(SoccerRecordGame);
        Destroy(BilliardRecordGame);
       
        BilliardGame = null;
        SoccerGame = null;
        BilliardRecordGame = null;
        SoccerRecordGame = null;
        yield return new WaitForSeconds(1.0f);
    }
    private void FadeIn()
    {
        FadeOutFadeInGroup.blocksRaycasts = true;
        FadeOutFadeInGroup.DOFade(1.0f, FadeInDuration).OnComplete(() =>
        {
            FadeInCompelete = true;

        });
    }
    private void FadeOut()
    {
        FadeOutFadeInGroup.DOFade(0.0f, FadeOutDuration).OnComplete(() =>
        {
            FadeOutCompelete = true;
            FadeOutFadeInGroup.blocksRaycasts = false;
        });
    }
    public void PhysicOn()
    {
        if (!Physics.autoSimulation)
            Physics.autoSimulation = true;
        else
            Physics.autoSimulation = false;
    }
}
