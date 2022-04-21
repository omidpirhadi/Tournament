using System;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Diaco.EightBall.Structs
{
    [Serializable]
    public struct PositionAndRotateBalls
    {
        [DataMember] public Vector2 CueBall;
        [DataMember] public Vector3 CueBall_R;
     ////   [DataMember] public bool CueBallInPocket;

        [DataMember] public Vector2 Ball_1;
        [DataMember] public Vector3 Ball_1_R;
       //// [DataMember] public bool Ball_1InPocket;



        [DataMember] public Vector2 Ball_2;
        [DataMember] public Vector3 Ball_2_R;
      ///  [DataMember] public bool Ball_2InPocket;

        [DataMember] public Vector2 Ball_3;
        [DataMember] public Vector3 Ball_3_R;
       /// [DataMember] public bool Ball_3InPocket;

        [DataMember] public Vector2 Ball_4;
        [DataMember] public Vector3 Ball_4_R;
      ////  [DataMember] public bool Ball_4InPocket;


        [DataMember] public Vector2 Ball_5;
        [DataMember] public Vector3 Ball_5_R;
       // [DataMember] public bool Ball_5InPocket;

        [DataMember] public Vector2 Ball_6;
        [DataMember] public Vector3 Ball_6_R;
     ///   [DataMember] public bool Ball_6InPocket;

        [DataMember] public Vector2 Ball_7;
        [DataMember] public Vector3 Ball_7_R;
       /// [DataMember] public bool Ball_7InPocket;

        [DataMember] public Vector2 Ball_8;
        [DataMember] public Vector3 Ball_8_R;
      ///  [DataMember] public bool Ball_8InPocket;

        [DataMember] public Vector2 Ball_9;
        [DataMember] public Vector3 Ball_9_R;
       /// [DataMember] public bool Ball_9InPocket;

        [DataMember] public Vector2 Ball_10;
        [DataMember] public Vector3 Ball_10_R;
       // [DataMember] public bool Ball_10InPocket;


        [DataMember] public Vector2 Ball_11;
        [DataMember] public Vector3 Ball_11_R;
       // [DataMember] public bool Ball_11InPocket;

        [DataMember] public Vector2 Ball_12;
        [DataMember] public Vector3 Ball_12_R;
       /// [DataMember] public bool Ball_12InPocket;

        [DataMember] public Vector2 Ball_13;
        [DataMember] public Vector3 Ball_13_R;
      //  [DataMember] public bool Ball_13InPocket;

        [DataMember] public Vector2 Ball_14;
        [DataMember] public Vector3 Ball_14_R;
       /// [DataMember] public bool Ball_14InPocket;

        [DataMember] public Vector2 Ball_15;
        [DataMember] public Vector3 Ball_15_R;
      //  [DataMember] public bool Ball_15InPocket;

        [DataMember] public float TimeStepPacket;
        [DataMember] public bool isLastPacket;
    }
    [Serializable]
    public  struct AimData
    {
        [DataMember] public float X_position;
        [DataMember] public float Z_position;
        [DataMember] public float YY_rotation;
        [DataMember] public float X_rotation;
        [DataMember] public float Y_rotation;
        [DataMember] public Vector3 PosCueBall;
    }

    [Serializable]
    public struct CueBallData
    {
        [DataMember] public Vector3 position;
        [DataMember] public bool isDrag;

    }
    [Serializable]
    public struct BallIDs
    {
        public List<int> IDBalls;
    }
    public enum Side{ Left = 0 , Right = 1}
    public enum BillboardSection { Image = 0, CoolDown = 1, Coin = 2 }
    public enum Shar {None = -1, Solid = 0, Stripe = 1 }
    [Serializable]
    public struct _Shar
    {
        public Sprite Shar;
        public int ID;
    }
    [Serializable]
    public struct _Shars
    {
        public List<_Shar> Shars;


    }
    [Serializable]
    public struct GameData
    {
        public string gid;
        public PlayerInfo playerOne;
        public PlayerInfo playerTwo;
        public int totalTime;
        public int turnTime;
        public int ownerTurn;
        public int pitok;//0 no pitok
        public bool pitokCueBall;
        public bool sharSeted;
        public List<int> deletedBalls;
        public string winner;
        public string loser;
        public int state;
        public PositionAndRotateBalls positions;
        public Vector3 cueBallLastPosition;
        public string table;
        public string type;
        public int game;
        public int cost;
        public int costType;
        public int award;

        public int selectedPocket;//  -1 = Do notthing, 0 = Call pocket Only Without Turn, >0 = give Turn and Call Pocket 

    }
    [Serializable]
    public struct PlayerInfo
    {
        public string userName;
        public string avatar;
        public string shar;
        public int time;
        public List<int> remaningBalls;
    }
    [Serializable]
    public struct UserInfo
    {
        public string userName;
    }
    [Serializable]
    public struct ResultPlayer
    {
        public string userName;
        public string avatar;
        public List<string> friends;
        public int goalcount;
        
        public string rank;
        public string coin;
        public string cup;
        public string xp;
        
    }
    [Serializable]
    public struct ResultGame
    {
        
        public ResultPlayer winner;
        public ResultPlayer loser;
        

    }
}
