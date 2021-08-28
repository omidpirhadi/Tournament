using System;
//sing System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using DG.Tweening;
using Diaco.SoccerStar.Server;
using Sirenix.OdinInspector;

namespace Diaco.SoccerStar.Marble
{

    
    public class ForceToBall : MonoBehaviour
    {
        
        public enum Marble_Type { Marble, Ball };
        public Marble_Type MarbleType;
        //public bool InRecordMode = false;
        public int ID;

        public ForceMode forceMode;

        [Range(110, 1000000000)]
        public float PowerForce;
        public GameObject SelectEffect;
        public float SelectEffectRotateSenciviti = 1.5f;
       // p//ublic bool calculate = false;
       //// public float SpeedRotationBall = 20.0f;
       /// public float ForceStopBallInSpeed = 0.02f;
       // public float ForceStopBallInSpeed2 = 0.0001f;

        private new Rigidbody rigidbody;

        private ServerManager server;

        private TempPlayerControll playerControll;

        public float ThresholdSleep = 0.09f;
        private Vector3 VlocityBall;
        
        private Vector3 LastPos;
       [SerializeField] private float Y_Pos_Refrence = 0.0f;
        public bool InMove = false;

        public void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            server = FindObjectOfType<ServerManager>();
            SetYPositionRefrence();
            LastPos = transform.position;
        }
        void Update()
        {
             if(MarbleType == Marble_Type.Ball)
             {
                manageBallFriction();
             }
        }
        void LateUpdate()
        {

            FixOverflowMovment();
        }
        private void FixedUpdate()
        {
            VlocityBall = rigidbody.velocity;

        }

        private void OnEnable()
        {
            rigidbody = GetComponent<Rigidbody>();

            server = FindObjectOfType<ServerManager>();
            if (MarbleType == Marble_Type.Marble)
            {
                server.OnChangeTurn += Server_OnChangeTurn;
                server.EnableRingMarbleForOpponent += Server_EnableRingMarbleForOpponent;
                ///  server.Move += Server_Move;
                playerControll = FindObjectOfType<TempPlayerControll>();
                playerControll.EnableSelectRingEffect += PlayerControll_EnableSelectRingEffect;
                playerControll.OnShoot += PlayerControll_OnShoot;

            }

            SetYPositionRefrence();
        }



        public void OnDestroy()
        {
     
                if (MarbleType == Marble_Type.Marble)
                {

                    playerControll.EnableSelectRingEffect -= PlayerControll_EnableSelectRingEffect;
                    playerControll.OnShoot -= PlayerControll_OnShoot;
                    server.EnableRingMarbleForOpponent -= Server_EnableRingMarbleForOpponent;
                    server.OnChangeTurn -= Server_OnChangeTurn;

                }
            
        }



        private void Server_EnableRingMarbleForOpponent( bool enable)
        {
            SelectEffectEnableForOpponent(enable);
        }

        private void PlayerControll_OnShoot(int marbleID,Vector3 dir, float pow)
        {
            if(marbleID == ID)
            {
                Move(dir, pow);
            }
        }

        private void PlayerControll_EnableSelectRingEffect(bool obj)
        {
            SelectEffectEnable(obj);
        }

      

        private void Server_OnChangeTurn(bool obj)
        {
            SelectEffectEnable(obj);
            ///Debug.Log("TURN" + obj.ToString());
        }



        void OnCollisionEnter(Collision obj)
        {


            if (MarbleType == Marble_Type.Ball)
            {
                if(obj.collider.tag== "marble" ||  obj.collider.tag == "wall")
                StartCoroutine(fakeRotation());
            }
        }

        public void Move(Vector3 dir, float pow)
        {
          

            var d_n = new Vector3(dir.x, 0, dir.z).normalized;
            var P_F = SoftFloat.Soft((PowerForce) * pow);
            //server.SendForceData(new CustomTypes.FORCEDATA { id = (short)ID, direction = d_n, power = PowerForce, aimPower = pow });
           /// ForceStopBallInSpeed2 = 00;
            //Debug.Log(P_F);
          rigidbody.AddForce(d_n * (float)P_F, forceMode);
            //server.SendDirctionForceToServer((short)ID, dir_temp * (PowerForce * pow));

            if (server.InRecordMode == false)

                StartCoroutine(server.SendDataMarblesMovement());
            else
                server.StarCheckMovment();
          //  Debug.Log("ForceTT");


        }
        
        public void MoveFromSever(Diaco.SoccerStar.CustomTypes.MOVE move)
        {
           // Debug.Log($"MOVEFromServer{move.force} ID {move.id}");
            if (move.id == ID)
            {
                rigidbody.AddForce(move.force, forceMode);
               // Debug.Log("ForceRecive");
            }
            
        }

       


        bool frFlag = false;
        public int rotationSpeed = 15;
        IEnumerator fakeRotation()
        {

            if (frFlag)
                yield break;

            frFlag = true;

            float t = 0;
            while (t < 1)
            {

 
                //print ("fake rotation...");

                t += Time.deltaTime * 0.4f;
                float rot = rotationSpeed - (t * rotationSpeed);
                transform.Rotate(new Vector3(rot / 3, rot, rot / 3));
                yield return 0;
            }

            if (t >= 1)
            {
                frFlag = false;
            }
        }
        private void manageBallFriction()
        {


           var ballSpeed = GetComponent<Rigidbody>().velocity.magnitude;
            //print("Ball Speed: " + rigidbody.velocity.magnitude);
            if (ballSpeed < 0.5f)
            {
              
                GetComponent<Rigidbody>().drag = 2;
                //GetComponent<Rigidbody>().velocity = Vector3.zero;
            }
            else
            {
                //let it slide
                GetComponent<Rigidbody>().drag = 0.9f;
            }
        }

        public void SetYPositionRefrence()
        {
            DOVirtual.Float(0, 1, 2.0f, (x) => { }).OnComplete(() =>
            {

            }).OnComplete(() => {
                
                
                if (MarbleType == Marble_Type.Marble)
                {
                    
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                }
                else
                {
                    rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
                }
                Y_Pos_Refrence = transform.position.y;
            });
        }
        private void FixOverflowMovment()
        {
            if (Y_Pos_Refrence > 0.0f)
            {
                var conflict_Y = Mathf.Abs(transform.position.y - Y_Pos_Refrence);
                if (conflict_Y > Physics.defaultContactOffset)
                {

                    transform.position = new Vector3(transform.position.x, Y_Pos_Refrence, transform.position.z);

                    Debug.Log("Fix Y Ball");
                }


               
            }
            if (CheckBallMove() == true && InMove == false)
            {
                DOVirtual.Float(0, 1, 1.0f, (x) => { }).OnComplete(() =>
                {

                }).OnComplete(() =>
                {

                    InMove = true;
                });
            }
            if (VlocityBall.magnitude < ThresholdSleep && InMove == true)
            {

                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
                InMove = false;
                Debug.Log("Fix Move Ball");

            }
        }
        private bool CheckBallMove()
        {
            var move = false;

            if (rigidbody.velocity.magnitude >ThresholdSleep || rigidbody.angularVelocity.magnitude >ThresholdSleep)
            {

                move = true;
            }




            return move;
        }

        public bool CheckOwnerMarble()
        {
            bool c = false;
            if (ID >= server.MinRangMarblesId && ID <= server.MaxRangMarblesId)
            {
                c = true;
            }
            return c;
        }
        public void SelectEffectEnable(bool Active)
        {
           // Debug.Log("aaaszx");
            if (MarbleType == Marble_Type.Marble)
            {
                //Debug.Log("34343");
                if (CheckOwnerMarble())
                {
                    SelectEffect.SetActive(Active); //Debug.Log("sdasd");
                }
                else
                {
                    SelectEffect.SetActive(false);
                }
               
            }
        }
        public void SelectEffectEnableForOpponent(bool Active)
        {
            // Debug.Log("aaaszx");
            if (MarbleType == Marble_Type.Marble)
            {
                //Debug.Log("34343");
                if (!CheckOwnerMarble())
                {
                    SelectEffect.SetActive(Active); //Debug.Log("sdasd");
                }

            }
        }
        public void SetSkinMarble(Texture2D  skin)
        {
            var mat = GetComponent<MeshRenderer>().material;
            mat.mainTexture = skin;
        }
        private void SoftPositionAndRotation()
        {
            DOVirtual.Float(0, 1, 0.5f, x =>
            {
              
            }).OnComplete(() => {
                var x_p = (float)SoftFloat.Soft(this.transform.position.x);
                var y_p = this.transform.position.y;
                var z_p = (float)SoftFloat.Soft(this.transform.position.z);

                var x_r = (float)SoftFloat.Soft(this.transform.eulerAngles.x);
                var y_r = (float)SoftFloat.Soft(this.transform.eulerAngles.y);
                var z_r = (float)SoftFloat.Soft(this.transform.eulerAngles.z);
              //  Debug.Log(new Vector3(x_p, y_p, z_p));
               // Debug.Log(new Vector3(x_r, y_r, z_r));
                this.transform.position = new Vector3(x_p, y_p, z_p);
                this.transform.eulerAngles = new Vector3(x_r, y_r, z_r);
            });

        }
    }
}