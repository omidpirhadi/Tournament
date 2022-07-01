using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Diaco.EightBall.Pockets
{
    public class PocketOnClick : MonoBehaviour
    {
        private Pockets pocket;
        private void Start()
        {
            pocket = GetComponentInParent<Pockets>();
        }
        private void OnMouseDown()
        {
            if (pocket.Selectable)
            {
                pocket.PocketClick();
            }
        }
    }
}