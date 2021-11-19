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
     ///   public List<DataPostionsMarbles> plan;
    }
    [Serializable]
    public struct GameData
    {
        public string gid;
        public PlayerInfo playerOne;
        public PlayerInfo playerTwo;
        public List<MarbleMovementData> positions;
        public int totalTime;
        public int turnTime;
        public int ownerTurn;

        public string winner;
        public string loser;
        public int state;
       
       
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
        public Vector3 velocity;
        public bool IsRotateBall;
        public bool IsRotateMarble;
       
    }
    public struct MarbleMovementPackets
    {
        public List<MarbleMovementData> marbleMovements;
        public float TimeStepPacket;
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
}
