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


        public Diaco_Billiard_Vec CueBall;
        public Diaco_Billiard_Vec CueBall_R;
        public Diaco_Billiard_Vec CueBall_velocity;

        public Diaco_Billiard_Vec Ball_1;
        public Diaco_Billiard_Vec Ball_1_R;
        public Diaco_Billiard_Vec Ball_1_velocity;



        public Diaco_Billiard_Vec Ball_2;
        public Diaco_Billiard_Vec Ball_2_R;
        public Diaco_Billiard_Vec Ball_2_velocity;

        public Diaco_Billiard_Vec Ball_3;
        public Diaco_Billiard_Vec Ball_3_R;
        public Diaco_Billiard_Vec Ball_3_velocity;

        public Diaco_Billiard_Vec Ball_4;
        public Diaco_Billiard_Vec Ball_4_R;
        public Diaco_Billiard_Vec Ball_4_velocity;


        public Diaco_Billiard_Vec Ball_5;
        public Diaco_Billiard_Vec Ball_5_R;
        public Diaco_Billiard_Vec Ball_5_velocity;

        public Diaco_Billiard_Vec Ball_6;
        public Diaco_Billiard_Vec Ball_6_R;
        public Diaco_Billiard_Vec Ball_6_velocity;

        public Diaco_Billiard_Vec Ball_7;
        public Diaco_Billiard_Vec Ball_7_R;
        public Diaco_Billiard_Vec Ball_7_velocity;

        public Diaco_Billiard_Vec Ball_8;
        public Diaco_Billiard_Vec Ball_8_R;
        public Diaco_Billiard_Vec Ball_8_velocity;

        public Diaco_Billiard_Vec Ball_9;
        public Diaco_Billiard_Vec Ball_9_R;
        public Diaco_Billiard_Vec Ball_9_velocity;

        public Diaco_Billiard_Vec Ball_10;
        public Diaco_Billiard_Vec Ball_10_R;
        public Diaco_Billiard_Vec Ball_10_velocity;


        public Diaco_Billiard_Vec Ball_11;
        public Diaco_Billiard_Vec Ball_11_R;
        public Diaco_Billiard_Vec Ball_11_velocity;

        public Diaco_Billiard_Vec Ball_12;
        public Diaco_Billiard_Vec Ball_12_R;
        public Diaco_Billiard_Vec Ball_12_velocity;

        public Diaco_Billiard_Vec Ball_13;
        public Diaco_Billiard_Vec Ball_13_R;
        public Diaco_Billiard_Vec Ball_13_velocity;

        public Diaco_Billiard_Vec Ball_14;
        public Diaco_Billiard_Vec Ball_14_R;
        public Diaco_Billiard_Vec Ball_14_velocity;

        public Diaco_Billiard_Vec Ball_15;
        public Diaco_Billiard_Vec Ball_15_R;
        public Diaco_Billiard_Vec Ball_15_velocity;

        public int Tik;

        public bool isLastPacket;
    }
    [Serializable]
    public struct AimData
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
    public enum Side { Left = 0, Right = 1 }
    public enum BillboardSection { Image = 0, CoolDown = 1, Coin = 2 }
    public enum Shar { None = -1, Solid = 0, Stripe = 1 }
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
        public float totalTime;
        public float turnTime;
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
        public int goalCount;

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
        public bool isFriend;

    }


    public static class Vec3Helper
    {
        public static Vector3 ToVector3(Diaco_Billiard_Vec billiard_Vec)
        {
            var vec = new Vector3(billiard_Vec.x, billiard_Vec.y, billiard_Vec.z);
            return vec;
        }
        public static Diaco_Billiard_Vec ToBilliardVec(Vector3 vector)
        {
            return new Diaco_Billiard_Vec(vector.x, vector.y, vector.z);
        }
    }
    [Serializable]
    public struct Diaco_Billiard_Vec
    {
        public float x;
        public float y;
        public float z;
        public Diaco_Billiard_Vec(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

    }
    [Serializable]
    public struct Diaco_Soccer_Vec
    {
        public float x;
        public float y;
        public float z;
        public float r;
        public Diaco_Soccer_Vec(float x, float y, float z, float r)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.r = r;
        }

    }
    [Serializable]
    public struct BallData
    {
        public List<Diaco_Billiard_Vec> Position;
        public List<Diaco_Billiard_Vec> Velocity;
        public List<Diaco_Billiard_Vec> Rotation;

        /* public BallData(BallData data)
         {
             this.Position = data.Position;
             this.Velocity = data.Velocity;
             this.Rotation = data.Rotation;
         }*/
    }
}
