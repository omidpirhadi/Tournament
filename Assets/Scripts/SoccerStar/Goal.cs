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
        [SerializeField] private int once = 0;
        public void Start()
        {
            server = FindObjectOfType<ServerManager>();
            /// Tweener tweener;

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "ball" && once == 0)
            {
                if (other.GetComponent<ForceToBall>().MarbleType == ForceToBall.Marble_Type.Ball)
                {
                    once = 1;
                   /// var rigibody = other.GetComponent<Rigidbody>();

                    if (server.FreePlay == false)
                    {
                        server.soundeffectcontrollLayer2.PlaySoundSoccer(2);
                        ShowAnnounceImage(true);
                       // Debug.Log("goal");
                    }
                    else if (server.FreePlay == true)
                    {
                        server.soundeffectcontrollLayer2.PlaySoundSoccer(1);
                        ShowAnnounceImage(false);
                       // Debug.Log("foul");
                    }
                       
                    server.IsGoal = ID;
                }

            }
        }

        private void ShowAnnounceImage(bool goal)
        {
            if(goal)
            {

                GoalAnnounceImage.DOColor(new Color(1,1,1,1),SpeedFillImage);
                
            }
            else
            {
                FoulAnnounceImage.DOColor(new Color(1, 1, 1, 1), SpeedFillImage);
            }
            AnnounceDisable();
        }
        private void AnnounceDisable()
        {
            DOVirtual.Float(0, 1, DurationShow, (x) =>
            {

            }).OnComplete(() => {
                GoalAnnounceImage.DOColor(new Color(1, 1, 1, 0), SpeedFillImage);
                FoulAnnounceImage.DOColor(new Color(1, 1, 1, 0), SpeedFillImage);
                once = 0;
            });
        }
    }
}