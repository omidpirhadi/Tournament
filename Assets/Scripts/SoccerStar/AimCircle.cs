using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Diaco.SoccerStar.Server;
public class AimCircle : MonoBehaviour
{
    public ServerManager server;
    public bool RecordMode = false;
    public bool RecordAim = false;
    public float SendRateAimDataToServer = 0.04f;
    public BodyCircle bodyCircle;
    public Transform MaskCircle;
    public GameObject indicator;
    public GameObject indicatorNegativ;

    public float SensivitiScale = 0.3f;

    public float PowerRadius;

    public float CurrentAimPower;

    // private Coroutine AimRecordControll;
    public void Awake()
    {
        BuildCircleAim();
        if (RecordMode == false)
        {
            server.OnAimRecive += Server_OnAimRecive;
            server.ResetAim += Server_ResetAim;
        }

    }
    void Update()
    {
       
        MarbleRingEffect();
    }
    public void MarbleRingEffect()
    {
        if (server.InRecordMode == false)
        {


            if (server.Turn && CurrentAimPower < 3.5f)
            {
                Handler_EnableRingEffectOwner(true);
                Handler_EnableRingEffectOppenent(false);
            }
            if (server.Turn && CurrentAimPower > 3.5f)
            {
                Handler_EnableRingEffectOwner(false);
                Handler_EnableRingEffectOppenent(false);
            }
            if (!server.Turn && CurrentAimPower < 3.5f)
            {
                Handler_EnableRingEffectOppenent(true);
                Handler_EnableRingEffectOwner(false);

                // Debug.Log("eR");
            }
            if (!server.Turn && CurrentAimPower > 3.5f)
            {
                Handler_EnableRingEffectOppenent(false);
                Handler_EnableRingEffectOwner(false);
                //Debug.Log("eRD");
            }
        }
        else
        {
           // Debug.Log("R1");
            if (server.Turn && CurrentAimPower < 3.5f)
            {
                Handler_EnableRingEffectOwner(true);
             //   Debug.Log("R4");
            }
            if (server.Turn && CurrentAimPower > 3.5f)
            {
                Handler_EnableRingEffectOwner(false);
             //   Debug.Log("R3");
            }
        }
        //// Profiler.EndSample();
    }

    public void BuildCircleAim()
    {
        int segment = 50;
        ///float angle = 20;
        var line_c = GetComponent<LineRenderer>();
        line_c.positionCount = segment + 1;
        var points = new Vector3[segment + 1];
        for (int i = 0; i < points.Length; i++)
        {
            var x = Mathf.Sin(Mathf.Deg2Rad * (i * 360 / segment)) * 1;
            var z = Mathf.Cos(Mathf.Deg2Rad * (i * 360 / segment)) * 1;

            points[i] = new Vector3(x, 0, z);
        }
        line_c.SetPositions(points);
    }

    public void ScaleAim(int marbleId , float changescale )
    {
      //  var currentscale = this.transform.localScale.x;
      //  var tempscale = currentscale + changescale ;
        CurrentAimPower = Mathf.Clamp(changescale, 0, PowerRadius);
        this.transform.localScale = new Vector3(CurrentAimPower, CurrentAimPower, CurrentAimPower);

    }
    public void AimCircleAndIndicatorRotate(int marbleId,Vector3 point)
    {

        var dir = point - this.transform.position;
        var angel = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(new Vector3(0.0f, angel, 0.0f));
      /*  SendAimDataToServer(new Diaco.SoccerStar.CustomTypes.AimData
        {
            ID = marbleId,
            Position = this.transform.position,
            CircleRotate_Y = this.transform.eulerAngles.y,
            CricleScale = this.transform.localScale.x,

        });*/
    }
    public void AimCircleAndIndicatorRotate2(int marbleId,Vector3 point)
    {

        var dir = point - this.transform.position;
        var angel = Mathf.Atan2(-dir.x, -dir.z) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(new Vector3(0.0f, angel, 0.0f));

      /*  SendAimDataToServer(new Diaco.SoccerStar.CustomTypes.AimData
        {
            ID = marbleId,
            Position = this.transform.position,
            CircleRotate_Y = this.transform.eulerAngles.y,
            CricleScale = this.transform.localScale.x,

        });*/
    }
    public void AimCircleRotate(int marbleId,float angel)
    {
        ////Debug.Log(this.transform.localEulerAngles.y);
        var dir = DirectionShoot().normalized;
        int side = 0;
        if(dir.x>0)
        {
            Debug.Log("Positiv_X");
            side = -1;
        }
        else
        {
            Debug.Log("Negativ_X");
            side = 1;
        }

        this.transform.rotation = Quaternion.Euler(new Vector3(0.0f, this.transform.localEulerAngles.y + (angel * side), 0.0f));
    }
    public Vector3  DirectionShoot()
    {

        var dir = indicator.transform.position - this.transform.position;
        return dir;
    }

    public void ResetAimCircle()
    {

        this.transform.localScale = new Vector3(0, -10, 0);
        this.transform.position = new Vector3(0, -10, 0);
        this.transform.eulerAngles = new Vector3(0, -10, 0);
        this.MaskCircle.position = new Vector3(0, -10, 0); 
        CurrentAimPower = 0.0f;
        //Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAA");

    }
    public void HideAimCricle(bool hide)
    {
        if(hide)
        {
            bodyCircle.Arrowtexture.enabled = false;
            bodyCircle.Cricletexture.enabled = false;
            bodyCircle.lineRendererArrowBackward.enabled = false;
            bodyCircle.lineRendererArrowForward.enabled = false;
          ///  bodyCircle.lineRendererCircle.enabled = false;
        }
        else if(!hide)
        {
            bodyCircle.Arrowtexture.enabled = true;
            bodyCircle.Cricletexture.enabled = true;
            bodyCircle.lineRendererArrowBackward.enabled = true;
            bodyCircle.lineRendererArrowForward.enabled = true;
           /// bodyCircle.lineRendererCircle.enabled = true;
        }
    }

    public void StartRecordAim(int marbleId)
    {
        RecordAim = true;
       StartCoroutine(SendAimDataToServer(marbleId));
    }
    public void StopRecordAim()
    {
        RecordAim = false;

       //StopCoroutine(AimRecordControll);
    }
    private IEnumerator SendAimDataToServer(int marbleId)
    {
        while (RecordAim)
        {
            server.SendAimData(new Diaco.SoccerStar.CustomTypes.AimData
            {
                ID = marbleId,
                Position = this.transform.position,
                CircleRotate_Y = this.transform.eulerAngles.y,
                CricleScale = this.transform.localScale.x,
                AimPower = CurrentAimPower,
                ///MaskPosition = MaskCircle.position
                

            });
            yield return new WaitForSeconds(SendRateAimDataToServer);
           // Debug.Log("Aim");
        }
        
    }

    private void Server_ResetAim()
    {
        ResetAimCircle();
    }

    private void Server_OnAimRecive(Diaco.SoccerStar.CustomTypes.AimData aimdata)
    {
        this.MaskCircle.position = new Vector3(-1 * aimdata.Position.x, aimdata.Position.y, aimdata.Position.z);
        this.transform.position = new Vector3(-1 * aimdata.Position.x, aimdata.Position.y, aimdata.Position.z);
        this.transform.DOScale(aimdata.CricleScale, SendRateAimDataToServer);
        this.transform.DORotate(new Vector3(this.transform.eulerAngles.x, -aimdata.CircleRotate_Y, this.transform.eulerAngles.z), SendRateAimDataToServer);
        this.CurrentAimPower = aimdata.AimPower; 
       /* if (aimdata.AimPower < 3.5f)
        {
            server.Handler_EnableRingMarbleForOpponent(true);
            HideAimCricle(true);

        }
        else
        {
            server.Handler_EnableRingMarbleForOpponent(false );
            HideAimCricle(false);
        }*/
    }
    private Action<bool> enableRingEffectForOwner;
    public event Action<bool> EnableRingEffectOwner
    {
        add
        {
            enableRingEffectForOwner += value;
        }
        remove
        {
            enableRingEffectForOwner -= value;
        }
    }
    protected void Handler_EnableRingEffectOwner(bool enable)
    {
        if (enableRingEffectForOwner != null)
        {
            enableRingEffectForOwner(enable);
        }
    }

    private Action<bool> enableRingEffectForOpponent;
    public event Action<bool> EnableRingEffectOpponent
    {
        add
        {
            enableRingEffectForOpponent += value;
        }
        remove
        {
            enableRingEffectForOpponent -= value;
        }
    }
    protected void Handler_EnableRingEffectOppenent(bool enable)
    {
        if (enableRingEffectForOpponent != null)
        {
            enableRingEffectForOpponent(enable);
        }
    }
}
[Serializable]
public struct BodyCircle
{
    ///public LineRenderer lineRendererCircle;
    public SpriteRenderer lineRendererArrowForward;
    public SpriteRenderer lineRendererArrowBackward;
    public SpriteRenderer Cricletexture;
    public SpriteRenderer Arrowtexture;
}