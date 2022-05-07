

    // Start is called before the first frame update

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Diaco.UI.PopupRecordModeResult
{
    public class PopupAwardRecordMode : MonoBehaviour
    {
        public Diaco.ImageContainerTool.ImageContainer AvatarsImage;
        private GameLuncher Luncher;
        public Image Avatar;
        public RTLTMPro.RTLTextMeshPro UserName;
        public Text Rank;
        public Text BestRecord;
        public RTLTMPro.RTLTextMeshPro AwardCount;
        public Button Btn_Accept;
        public void Set(ResualtRecordModeData data)
        {
            Luncher = FindObjectOfType<GameLuncher>();
            Avatar.sprite = AvatarsImage.LoadImage(data.avatar);
            UserName.text = data.userName;
            Rank.text = data.rank.ToString();
            BestRecord.text = data.bestPoint.ToString();
            AwardCount.text = data.award;
            Btn_Accept.onClick.AddListener(AcceptClick);
        }
        private void AcceptClick()
        {

            this.gameObject.SetActive(false);
            Debug.Log("Accept");
        }
        private void OnDisable()
        {
            Btn_Accept.onClick.RemoveAllListeners();
        }
    }
    public struct ResualtRecordModeData
    {
        public string avatar;
        public string userName;
        public int rank;
        public int bestPoint;
        public string award;
    }
}