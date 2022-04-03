using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResultInRecordMode : MonoBehaviour
{
    public Diaco.ImageContainerTool.ImageContainer AvatarsImage;
    private GameLuncher Luncher;
    public Image Avatar;
    public Text UserName;
    public Text Rank;
    public Text BestRecord;
    public  RTLTMPro.RTLTextMeshPro Context;
    public Button Btn_Accept;
    public void Set(ResualtInRecordModeData data)
    {
        Luncher = FindObjectOfType<GameLuncher>();
        Avatar.sprite = AvatarsImage.LoadImage(data.avatar);
        UserName.text = data.userName;
        Rank.text = data.rank.ToString();
        BestRecord.text = data.bestPoint.ToString();
        Context.text = data.context;
        Btn_Accept.onClick.AddListener(AcceptClick);
    }
    private void AcceptClick()
    {
        Luncher.BackToMenu();
        this.gameObject.SetActive(false);
        Debug.Log("Accept");
    }
}
public struct ResualtInRecordModeData
{
    public string avatar;
    public string userName;
    public int rank;
    public int bestPoint;
    public string context;
}
