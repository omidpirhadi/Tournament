﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Diaco.HTTPBody;
namespace Diaco.Social
{
    public class SelectBadgePopUpController : MonoBehaviour
    {

        public ServerUI Server;
        public NavigationUI navigationUI;
        public Diaco.Social.BadgesElement.BadgeElement  BadgeElement;
        public RectTransform content;
      ///  public CreateTeamTabController TeamTabController;
        private string badgeid;
        public string BadgeID
        {
            set
            {

                badgeid = value;
                Handler_onchangebadgeid(badgeid);

            }
            get { return badgeid; }
        }

        public List<Diaco.Social.BadgesElement.BadgeElement> temp_badgelist;

        private void OnEnable()
        {
            temp_badgelist = new List<Diaco.Social.BadgesElement.BadgeElement>();
            InitializBadgeList(Server.BODY.inventory.badges);
        }
        private void OnDisable()
        {
            ClearBadges();
        }
        public void InitializBadgeList(List<Badge> badgeslist)
        {
           
            for (int i = 0; i < badgeslist.Count; i++)
            {
                var badge = Instantiate(BadgeElement, content);
                badge.SetBadge(Server.BadgesContainer.LoadImage(badgeslist[i].image), badgeslist[i].id, this);
                temp_badgelist.Add(badge);
            }

        }
        public void ClearBadges()
        {
            for (int i = 0; i < temp_badgelist.Count; i++)
            {
                Destroy(temp_badgelist[i].gameObject);
            }
            temp_badgelist.Clear();
        }






        public event Action<string> OnChangeBadgeId;
        protected void Handler_onchangebadgeid(string id)
        {
            if(OnChangeBadgeId != null)
            {
                OnChangeBadgeId(id);
            }
        }
    }
}