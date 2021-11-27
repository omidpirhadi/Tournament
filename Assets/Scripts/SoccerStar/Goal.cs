using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Diaco.SoccerStar.Server;
using Diaco.SoccerStar.Marble;
namespace Diaco.SoccerStar.Goals
{
    public class Goal : MonoBehaviour
    {
        public int ID = -1;
        public ServerManager server;
        public Image FoulAnnounceImage;
        public Image GoalAnnounceImage;
        public float SpeedFillImage = 0.5f;
        public float DurationShow = 3f;
        //public Collider FrontArea;
        public void Start()
        {
            server = FindObjectOfType<ServerManager>();
            /// Tweener tweener;

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "marble" || other.tag == "ball")
            {
                if (other.GetComponent<ForceToBall>().MarbleType == ForceToBall.Marble_Type.Ball)
                {
                    //   Debug.Log("GOAL");
                    var rigibody = other.GetComponent<Rigidbody>();

                    if (server.FreePlay == false)
                    {
                        server.soundeffectcontrollLayer2.PlaySoundSoccer(2);
                        ShowAnnounceImage(true);
                    }
                    else
                    {
                        server.soundeffectcontrollLayer2.PlaySoundSoccer(1);
                        ShowAnnounceImage(false);
                    }
                       
                    server.IsGoal = ID;
                }
                if (other.GetComponent<ForceToBall>().MarbleType == ForceToBall.Marble_Type.Marble)
                {
                   /// other.GetComponent<ForceToBall>().FrontAreaforMoveout = FrontArea; 
                }
            }
        }

        private void ShowAnnounceImage(bool goal)
        {
            if(goal)
            {
                GoalAnnounceImage.DOFade(255, SpeedFillImage);
               
            }
            else
            {
                FoulAnnounceImage.DOFade(255, SpeedFillImage);
            }
        }
    }
}