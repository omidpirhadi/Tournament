using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.EightBall.GameManager
{

    public class GameManager : MonoBehaviour
    {
        #region Property
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
        [SerializeField] private bool record;
        public bool Record
        {
            set
            {
                record = value;
                Handler_OnPlay(record);
               // print("record");
            }
            get
            {
                return record;
            }
        }
        [SerializeField] private int firstballimpact;
        public int FirstBallImpact
        {
            set
            {

                firstballimpact = value;

            }
            get { return firstballimpact; }
        }
        [SerializeField] private int pitok;
        public int Pitok
        {
            set
            {
                pitok = value;
                if(pitok > 0)
                {
                    Handler_OnPitok(pitok);
                }
            }
            get { return pitok; }
        }

        #endregion
        #region GlobalField
        public Diaco.EightBall.Structs.Shar PlayerShar;
        public bool EightBallEnable = false;
        public List<int> PocketedBallsID = new List<int>();
        public List<Pockets.Pockets> pockets;
        public List<int> IDImpactToWall;
        //public Slider SendRate;
        //public Text SendRateCounter;
        #endregion
        #region LocalField

        #endregion
        #region Unity_Function
        public void Start()
        {
            pockets[0].OnPocket += GameManager_OnPocket0;
            pockets[1].OnPocket += GameManager_OnPocket1;
            pockets[2].OnPocket += GameManager_OnPocket2;
            pockets[3].OnPocket += GameManager_OnPocket3;
            pockets[4].OnPocket += GameManager_OnPocket4;
            pockets[5].OnPocket += GameManager_OnPocket5;
            IDImpactToWall = new List<int>();
           // SendRate.onValueChanged.AddListener((X) => { SendRateCounter.text = X.ToString(); });
        }
        public void Update()
        {
        }
        #endregion
        #region Custom_Function

        public void ClearPocketedBallList()
        {
            PocketedBallsID.Clear();
        }
        public bool CheckPitok()

        {
            bool find = false;
            if(PocketedBallsID.Contains(0))
            {
                find = true;

            }
            return find;
        }
        public void FillImpactList(int Id)
        {

            if (!IDImpactToWall.Contains(Id))
            {
                IDImpactToWall.Add(Id);
            }

        }
        public bool CheckBallForAllowHit(int id)
        {
            bool Allow = false;
            if (EightBallEnable == false )
            {
                
                var type = -1;
                if (id > 0 && id < 8)
                {
                    type = 0;
                }
                else if(id > 8 )
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
            else if(EightBallEnable == true && id == 8)
            {
                Allow = true;
            }
            return Allow;
        }
        
        #endregion
        #region TriggersForEvents
        private void GameManager_OnPocket5(int ID)
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
        }

        private void GameManager_OnPocket0(int ID)
        {
            PocketedBallsID.Add(ID);
          ///  Debug.Log("Pocket1");
        }

        #endregion

        #region Events

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
        #endregion
    }
}
