using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;
namespace Diaco.GameAnalytic
{
    public class DiacoAnalytic : MonoBehaviour, IGameAnalyticsATTListener
    {
        void Start()
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                GameAnalytics.RequestTrackingAuthorization(this);
            }
            else
            {
                GameAnalytics.Initialize();
            }
            EventSend();
        }
        public void GameAnalyticsATTListenerAuthorized()
        {
            GameAnalytics.Initialize();
        }

        public void GameAnalyticsATTListenerDenied()
        {
            GameAnalytics.Initialize();
        }

        public void GameAnalyticsATTListenerNotDetermined()
        {
            GameAnalytics.Initialize();
        }

        public void GameAnalyticsATTListenerRestricted()
        {
            GameAnalytics.Initialize();
        }
        public void EventSend()
        {
            GameAnalytics.NewDesignEvent("Hello World", 0);
            GameAnalytics.NewDesignEvent("Hello World", 1);
            GameAnalytics.NewDesignEvent("Hello World", 3);
            GameAnalytics.NewDesignEvent("omid:level", 3);
            GameAnalytics.NewDesignEvent("omid:World", 3);
            GameAnalytics.NewDesignEvent("omid:prihadi", 3);
        }


    }
}