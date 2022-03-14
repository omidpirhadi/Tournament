using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diaco.UI.GameMode
{
    public class PopupGameModeLock : MonoBehaviour
    {

        ServerUI server;
        public bool GameIsSoccer = false;
        public List<GameObject> LockViwer;

        private void OnEnable()
        {
            server = FindObjectOfType<ServerUI>();
            server.OnGameBodyUpdate += Server_OnGameBodyUpdate;
            initGameLock();
        }
        private void OnDisable()
        {
            server.OnGameBodyUpdate -= Server_OnGameBodyUpdate;
           
        }
        private void OnDestroy()
        {
            server.OnGameBodyUpdate-= Server_OnGameBodyUpdate;
            
        }
        private void Server_OnGameBodyUpdate()
        {
            initGameLock();
        }

        public void initGameLock()
        {
            if (GameIsSoccer)
            {
                if (server.BODY.soccerGameLock.classic)
                    LockViwer[0].SetActive(true);
                else
                    LockViwer[0].SetActive(false);

                if (server.BODY.soccerGameLock.quick)
                    LockViwer[1].SetActive(true);
                else
                    LockViwer[1].SetActive(false);

                if (server.BODY.soccerGameLock.big)
                    LockViwer[2].SetActive(true);
                else
                    LockViwer[2].SetActive(false);
            }
            else
            {
                if (server.BODY.billiardGameLock.classic)
                    LockViwer[0].SetActive(true);
                else
                    LockViwer[0].SetActive(false);
                if (server.BODY.billiardGameLock.quick)
                    LockViwer[1].SetActive(true);
                else
                    LockViwer[1].SetActive(false);
                if (server.BODY.billiardGameLock.big)
                    LockViwer[2].SetActive(true);
                else
                    LockViwer[2].SetActive(false);
            }
        }

    }


}