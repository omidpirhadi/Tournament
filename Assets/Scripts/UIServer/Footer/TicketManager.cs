using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Diaco.UI.TicketManagers
{
    public class TicketManager : MonoBehaviour
    {
        public GameObject IdleTicket;
        public Ticket ticket;

        public void Show(List<Diaco.HTTPBody.TicketData> data)
        {
            if(data.Count >0)
            {
                IdleTicket.SetActive(false);
                ticket.gameObject.SetActive(true);
                ticket.Set(data[0]);
            }
            else
            {
                IdleTicket.SetActive(true);
                ticket.gameObject.SetActive(false);
            }
        }
        
    }

   
}