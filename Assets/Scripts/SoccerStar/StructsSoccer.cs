using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diaco.SoccerStar.CustomTypes
{
    [Serializable]
    public struct PlayerInfo
    {
        public string userName;
        public string avatar;
        public string skin;
        public int score;
        public float force;
        public float aim;

        ///   public List<DataPostionsMarbles> plan;
    }
    [Serializable]
    public struct GameData
    {
        public string gid;
        public PlayerInfo playerOne;
        public PlayerInfo playerTwo;
        public MarblesData positions;
        public float totalTime;
        public float turnTime;
        public int ownerTurn;
        public int gameTime;
        public string winner;
        public string loser;
        public int state;
        public int step;
       
        public string ground;
        public string type;

        public int game;
        public int cost;
        public int costType;
        public int award;

    }
    [Serializable]
    public struct UserInfo
    {
        public string userName;
    }
    [Serializable]
    public struct DataPostionsMarbles
    {
        public Vector2 marble_0;
        public float RotateY_0;
        public Vector2 marble_1;
        public float RotateY_1;
        public Vector2 marble_2;
        public float RotateY_2;
        public Vector2 marble_3;
        public float RotateY_3;
        public Vector2 marble_4;
        public float RotateY_4;
        public Vector2 marble_5;
        public float RotateY_5;
        public Vector2 marble_6;
        public float RotateY_6;
        public Vector2 marble_7;
        public float RotateY_7;
        public Vector2 marble_8;
        public float RotateY_8;
        public Vector2 marble_9;
        public float RotateY_9;
        public Vector2 ball;
        public float RotateY_ball;

    }

    [Serializable]
    public struct FORCEDATA
    {
        public short id;
        public Vector3 direction;
        public float power;
        public float aimPower;
    }
    [Serializable]
    public struct MOVE
    {
        public int id;
        public Vector3 force;
    }
    [Serializable]
    public struct MarbleMovementData
    {
        //public int frame;
        public short id;
        public Vector3 position;
        // public float rotate_y;
        //public Vector3 velocity;
        public bool IsRotateBall;
        public bool IsRotateMarble;
       
    }
    public struct MarbleMovementPackets
    {
        public List<MarbleMovementData> marbleMovements;
        public float TimeStepPacket;
        public bool IsLastPacket;
    }
    [Serializable]
    public struct AimData
    {
        public float ID;
        public Vector3 Position;
        public float CricleScale;
        public float CircleRotate_Y;
        public float AimPower;
       // public Vector3 MaskPosition;
       //// public float RotateIndicator_Y;
    }
    [Serializable]
    public struct GetInfoForSpawn
    {
        public List<Vector3> PositionMarbles;


    }
    [Serializable]
    public struct MatchInfo
    {
        public List<string> PlayersName;
        public List<string> PlayersImage;
        public string Coin;
    }
    [Serializable]
    public struct _InfoMarbleInGoalWithPositions
    {
        public List<int> ID;
        public List<Vector3> LastPosition;

    }

    [Serializable]
    public struct MarblesData
    {
        public Vec_Soccer m_p_1; /// Marble 1 Position
        public Vec_Soccer m_v_1;// Marable 1 Velocity

        public Vec_Soccer m_p_2;
        public Vec_Soccer m_v_2;

        public Vec_Soccer m_p_3;
        public Vec_Soccer m_v_3;

        public Vec_Soccer m_p_4;
        public Vec_Soccer m_v_4;

        public Vec_Soccer m_p_5;
        public Vec_Soccer m_v_5;

        public Vec_Soccer m_p_6;
        public Vec_Soccer m_v_6;

        public Vec_Soccer m_p_7;
        public Vec_Soccer m_v_7;

        public Vec_Soccer m_p_8;
        public Vec_Soccer m_v_8;

        public Vec_Soccer m_p_9;
        public Vec_Soccer m_v_9;

        public Vec_Soccer m_p_10;
        public Vec_Soccer m_v_10;

        public Vec_Soccer b_p; // Ball Position
        public Vec_Soccer b_v; // Ball Velocity

        public int Tik;
        public bool LastPacket;
    }
    [Serializable]
    public struct Vec_Soccer
    {
        public float x;
        public float y;
        public float z;

        public Vec_Soccer(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }


    }
    public static class VectorHelper

    {
        public static Vec_Soccer To_Vec_Soccer(Vector3 vec3)
        {
            return new Vec_Soccer(vec3.x, vec3.y, vec3.z);
        }
        public static Vector3 ToVector3(Vec_Soccer vec_Soccer)
        {
            return new Vector3(vec_Soccer.x, vec_Soccer.y, vec_Soccer.z);
        }
        public static Vector3 ToVector3WithReversX(Vec_Soccer vec_Soccer)
        {
            return new Vector3(-1 * vec_Soccer.x, vec_Soccer.y, vec_Soccer.z);
        }
        public static Vector3 ToVector3WithSide(Vec_Soccer vec_Soccer, int side)
        {
            return new Vector3(side * vec_Soccer.x, vec_Soccer.y, vec_Soccer.z);
        }
    }
}
