using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Diaco.SoccerStar.Server;
using Diaco.SoccerStar.Marble;
namespace Diaco.SoccerStar.Goals
{
    public class Goal : MonoBehaviour
    {
        public int ID = -1;
        public ServerManager server;
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

                    DOVirtual.Float(0, 1, 0.2f, (x) => { }).OnComplete(() =>
                      {
                          rigibody.velocity = Vector3.zero;
                      });
                    server.IsGoal = ID;
                }
                if (other.GetComponent<ForceToBall>().MarbleType == ForceToBall.Marble_Type.Marble)
                {
                   /// other.GetComponent<ForceToBall>().FrontAreaforMoveout = FrontArea; 
                }
            }
        }
    }
}