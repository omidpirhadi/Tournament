using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Diaco;
public class SplashScreen : MonoBehaviour
{

    public ServerUI Server;
    public GameObject loginPage;
    public Image Prograssbar;
    public Text CounterPrograssbar;
    public Text VersionClient;
    private SoundEffectControll soundEffect;
    private IEnumerator Start()
    {
        soundEffect = GetComponent<SoundEffectControll>();
        VersionClient.text = FindObjectOfType<Diaco.Setting.GeneralSetting>().Version;
        soundEffect.PlaySound(0);
        // CheckInternetConnection();
        if (Server.ExistTokenFile("token"))
        {

            Prograssbar.DOFillAmount(1f, 3f);
           
            yield return new WaitForSeconds(3.0f);
            Server.ConnectToUIServer();
            Debug.Log("Token Send To Server");

        }
        else
        {
            yield return new WaitForSeconds(3.0f);
            this.gameObject.SetActive(false);
            loginPage.SetActive(true);
            Debug.Log("Go To LoginPage");
        }
    }
    public void Update()
    {
        var fill = Mathf.RoundToInt(Prograssbar.fillAmount * 100f);
        CounterPrograssbar.text = fill.ToString()+"%";
    }
    public void CheckInternetConnection()
    {
        Diaco.HTTPRequests.HTTPRequest check_net = new Diaco.HTTPRequests.HTTPRequest("http://37.152.185.15:8420/api/user/checknet", "Content-Type", "application/json", Diaco.HTTPRequests.HTTPRequest.Method.GET);
        check_net.OnResponse += Check_net_OnResponse;
        StartCoroutine(check_net.GET(Diaco.HTTPRequests.HTTPRequest.Decoder.Buffer));
    }

    private void Check_net_OnResponse(string response)
    {
        Debug.Log("OMIDPIRHADI");
    }
}
