using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
namespace Diaco.Lobby
{
    public class FindPlayerLobby : MonoBehaviour
    {

        //private NavigationUI._GameLobby GameLobby;

        public ServerUI Server;
        public GameLuncher Luncher;
        public NavigationUI NavigationUi;

        public Image PlayerProfile;
        public Text PlayerName;

        public Image OpponentProfile;
        public Text OpponentName;

        public Button CancelLobbyButton;
        public Text Note;
        private Coroutine lobbyCoroutine;
        private AsyncOperation operationLoadScene;
        [SerializeField] private Diaco.HTTPBody.Opponent opponentData;
        //  [SerializeField] private bool SceneLoaded = false;
        [SerializeField] private bool FindOpponent = false;
        [SerializeField] private bool RepeatAnimation = true;
        [SerializeField] private float DurationSequence = 0.01f;
        public void Awake()
        {
            Luncher = FindObjectOfType<GameLuncher>();
            Server.OnOpponentFind += Server_OnOpponentFind;
            CancelLobbyButton.onClick.AddListener(() => { CancelLobby(); });


        }
        public void OnEnable()
        {
            ///GameLobby = NavigationUi.GameLobbyType;
            RunLobby();

            Debug.Log("LobbyRun");
        }
        private void Server_OnOpponentFind(string user, string avatar)
        {
            FindOpponent = true;
            opponentData.userName = user;
            opponentData.avatar = avatar;
        }



        private IEnumerator LobbyController()
        {
            /// var ingergate = 0;
            SetDataPlayer();
            if (NavigationUi.GameLobby == _GameLobby.Soccer)
            {
                Server.RequestFindOpponent(0, (short)NavigationUi.SubGame);
            }
            else
            {
                Server.RequestFindOpponent(1, (short)NavigationUi.SubGame);
            }
            var animation_Coroutine = StartCoroutine(AnimationProfileOpponent());
            yield return new WaitForSecondsRealtime(3.00f);
            ///   GameLobby = NavigationUi.GameLobbyType;
            //CancelLobbyButton.interactable = false;
            yield return new WaitUntil(() => FindOpponent);
            if (FindOpponent)
            {
                RepeatAnimation = false;
                StopCoroutine(animation_Coroutine);
                yield return new WaitForSecondsRealtime(0.5f);
                SetDataOpponent(opponentData.userName, opponentData.avatar);
                yield return new WaitForSecondsRealtime(2.00f);
                if (NavigationUi.GameLobby == _GameLobby.Soccer)

                    Luncher.SwitchScene(0);
                else
                    Luncher.SwitchScene(1);
                //Debug.Log("Scene Active");
            }

            RepeatAnimation = true;
            FindOpponent = false;
            yield return null;
        }

        /* private IEnumerator SceneLoad(string name)
         {

             operationLoadScene = SceneManager.LoadSceneAsync(name, LoadSceneMode.Single);
             operationLoadScene.allowSceneActivation = false;

             while (operationLoadScene.isDone == false)
             {
                 //  Debug.Log(operationLoadScene.progress * 100.0f);
                 if (operationLoadScene.progress >= 0.9f)
                 {
                     SceneLoaded = true;
                     yield break;
                 }



                 yield return null;
             }

         }*/
        private IEnumerator AnimationProfileOpponent()
        {

            // var repeated = 0;
            var ingergate = 0;
            do
            {
                for (int i = 0; i < Server.AvatarContainer.imageContainers.Count; i++)
                {
                    var profile_image = Server.AvatarContainer.LoadImage(Server.AvatarContainer.imageContainers[i].name);
                    OpponentProfile.sprite = profile_image;
                    yield return new WaitForSecondsRealtime(DurationSequence);
                }
                ingergate++;
            } while (RepeatAnimation && ingergate < 100);


        }

        private void SetDataPlayer()
        {
            PlayerProfile.sprite = Server.AvatarContainer.LoadImage(Server.BODY.profile.avatar);
            PlayerName.text = Server.BODY.userName;
        }
        private void SetDataOpponent(string user, string avatar)
        {
            OpponentProfile.sprite = Server.AvatarContainer.LoadImage(avatar);
            OpponentName.text = user;
        }
        public void RunLobby()
        {

            lobbyCoroutine = StartCoroutine(LobbyController());
        }
        public void CancelLobby()
        {
            StopCoroutine(lobbyCoroutine);
            Server.RequestLeaveLobby();
            NavigationUi.CloseAllPopUp();
            NavigationUi.SwitchUI("selectgame");
        }
    }
}