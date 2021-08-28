using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.EightBall.CueControllers
{
    public class GhostBallControll : MonoBehaviour
    {

        public SpriteRenderer spriteRenderer;
        public LineRenderer line;
        public Vector3 DefaultPosition = new Vector3(0f, 0.0f, 0.0f);
        public Vector3 DefaultRotation = new Vector3(90.0f, 0.0f, 0.0f);
        public Sprite ImageNoAllow;
        public Sprite ImageAllow;

        public void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            line = GetComponent<LineRenderer>();
        }
        public void ChangeImage(bool state)
        {
            if (state == true)
            {
             spriteRenderer.sprite = ImageAllow;
                line.enabled = true;

            }
            else
            {
               spriteRenderer.sprite = ImageNoAllow;
                line.enabled = false;
            }
        }
        public void SetDefault()
        {
           // transform.localPosition = DefaultPosition;
           /// transform.localEulerAngles = DefaultRotation;
            line.SetPosition(0, Vector3.zero);
            line.SetPosition(1, Vector3.zero);
        }
        public void OnEnable()
        {
        
          //  transform.localPosition = DefaultPosition;
          //  transform.localEulerAngles = DefaultRotation;
            if (line)
            {
                line.SetPosition(0, Vector3.zero);
                line.SetPosition(1, Vector3.zero);
            }

        }
        public void OnDisable()
        {

             ///   transform.localPosition = DefaultPosition;
             ///   transform.localEulerAngles = DefaultRotation;
            if (line)
            {
                line.SetPosition(0, Vector3.zero);
                line.SetPosition(1, Vector3.zero);
            }
        }
    }
}