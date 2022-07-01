using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Diaco.HTTPBody
{
    [Serializable]
    public struct REGISTER
    {
        public string userName;
        public string email;
        public string password;
        public string confirmPassword;
    }
    [Serializable]
    public struct LOGIN
    {
        public string phone;

        public string code;
    }
    [Serializable]
    public struct TOKEN
    {
        public string token;
    }
    
    [Serializable]
    public struct TOURNOMENTS
    {
        public string id;
        public string name;
        public string type; // competition , league
        public int time;
       
    }
    [Serializable]
    public struct InfoPage
    {
        public string page;
        public string targetUser;

    }
    [Serializable]
    public struct PROFILE
    {
        public int soccer_cup;
        public int billiard_cup;
        public int soccer_level;
        public int soccer_currentxp;
        public int soccer_totalxp;
        public int billiard_level;
        public int billiard_currentxp;
        public int billiard_totalxp;
        public int coin;
        public string gem;

        [Multiline]
        public string avatar;
        public string description;
        
        public PersonInformation information;
        public BiliardProfile billiard;
        public SoccerProfile soccer;

        public List<TOURNOMENTS> tournaments;
        public List<Achievement> achievements;
    }
    [Serializable]
    public struct TopPlayer
    {
        public string userName;
        public string avatar;
        public int rank;
        public int cup;
        public int gem;
        public int star;
    }
    [Serializable]
    public struct TopPlayers
    {
        public List<TopPlayer> players;
        public string time;
    }
    [Serializable]
    public struct ProfileOtherPerson
    {
        public string id;
        public string userName;
        public bool isBlock;
        public bool isFriend;
        public PROFILE profile;

    }
    [Serializable]
    public struct SOCIAL
    {
        public int inRequests;
        public List<OutRequset> outRequest;
        public List<FriendBody> friends;
        public MyTeam team;
       // public string team
  
    }
    [Serializable]
    public struct InRequset
    {
        public string id;
        public string from;
        public string type; //chat , team, friend
        public int game;//0 biliard , 1 soccer
        public string desc;
        public string capacity;
        public string remainingTime;
        public int cost;
        public int cup;
        public string messageCount;
        public int costType; //coin , gem, cup
        public bool isOnline;
        public string avatar;
        public string teamId;
    }
    [Serializable]
    public struct InRequsets
    {
        public List<InRequset> inRequests;
    }
    [Serializable]
    public struct Teams
    {
        public List<InRequset> teams;
    }
    [Serializable]
    public struct TeamInfo
    {
        public string teamId;
        public string name;
        
        public string description;
        public int game; //0 biliard 1 socor
        public int mode; //0 privte 1 public
        public int costType;//0 cup 1 coin 2gem
        public int cost;
        public string avatar;
        public string capacity;
        public string from;
        public List<FriendBody> members;
       // public List<AwardName> awardsname;
        public int remainingTime;
        public int join;//  0 ready to join, 1 ready to leave;  2 only watch
    }
    [Serializable]
    public struct OutRequset
    {
        public string to;
        public string type;
        public string desc;
    }
   /* [Serializable]
    public struct Message
    {
        public int type;
        public string userName;
      
    }*/
    [Serializable]
    public struct ChatBody
    {
        public int type;
        public bool isSticker;
        public string text;
        public string time;
        public string date;
        public bool read;
    }
    [Serializable]
    public struct Chats
    {
        public string chatId;
        public List<ChatBody> chats;
    }
    [Serializable]
    public struct TeamChats
    {
        public List<ChatBodyTeam> chats;
    }
    [Serializable]
    public struct ChatBodyTeam
    {
        public string userName;
        public bool isSticker;
        public string avatar;
        public string text;
        public string time;
        public string date;
    }
    [Serializable]
    public struct MyTeam
    {
        public List<ChatBodyTeam> chats;
        //public List<FriendBody> members;

        public string teamId;
        public string name;
       // public string description;
        public int game; //0 biliard 1 socor
      //  public int mode; //0 privte 1 public
      //  public int costType;//0 cup 1 coin 2gem
      //  public int cost;
        public string avatar;//1
      //  public int capacity;
       // public string from;
      //  public string status;
       // public string remainingTime;
        
    }
    
    
    [Serializable]
    public struct FriendBody
    {
        public string userName;
        public string id;
        public int cup;
        public bool isOnline;
        public bool isBlock;
        [Multiline]
        public string avatar;
    }
    [Serializable]
    public struct Friends
    {
      public List<FriendBody> friends;
    }
    [Serializable]
    public struct SearchUser
    {
        public string id;
        public int friend; ///state:" 0 = nonfriend, 1 = req sended, 2 = Isfriend"; 
        public bool isOnline;    
        public string userName;
       
        public int level;
        public int cup;
        [Multiline]
        public string avatar;
    }
    [Serializable]
    public struct BODY
    {
        // public string email;
        //public bool isDeleted;
        //  public string tokenCode;
        //public string socketId;
        // public string id;
        public string invitationCode;
        public string withdraw;
        public string userName;
        public string phone;
        public string guidContext;
        public GameLock soccerGameLock;
        public GameLock billiardGameLock;
        public InGame inGame;
        public PROFILE profile;
        public SOCIAL social;
        public Inventory inventory;
    }

    [Serializable]
    public struct GameLock
    {
        public bool classic;
        public bool quick;
        public bool big;
    }
    [Serializable]
    public struct Inventory
    {
        public List<string> avatars;
        public  List<LeagueFlage> leagueFlags;
        public List<TicketData> tickets;
    }
    [Serializable]
    public struct TicketData
    {
        public string id;
        public short gameType;//0 soccer  1 billiard
        public short costType; // 0 cup 1 coin 2 gem
        public string cost;
        public string capacity;
        public int time;
    }
    [Serializable]
    public struct Award
    {
        public int gem;
        public int coin;
        public int card;
        public string cardName;
        public int ticket;
        public int xp;
        public int cpu;
    }
    public struct AwardsName
    {
        public int capacity;
        public int active;
        public int costType;
        public int cost;
        public Award awards1;
        public Award awards2;
        public Award awards3;


    }
    [Serializable]
    public struct LeagueFlage
    {
        public string id;
        public string name;
    }
    [Serializable]
    public struct Achievement
    {
        public string name;
        public bool active;
        //public string require;
        //public string image;
        public string description;
    }
    [Serializable]
    public struct BiliardProfile
    {
        public float total;
        public float win;
        public float lose;
        public int purple;
        public int blue;
        public int green;
        public int yellow;
    }
    [Serializable]
    public struct SoccerProfile
    {
        public float total;
        public float win;
        public float lose;
        public int purple;
        public int blue;
        public int green;
        public int yellow;
    }
    [Serializable]
    public struct PersonInformation
    {
        public string firstName;
        public string lastName;
        public string email;
        public int gender;
        public int birthyear;
        public string phone;
    }
    [Serializable]
    public struct ProfileEdited
    {
        public string firstName;
        public string lastName;
        public string email;
        public int gender;
        public string password;
        public string confirmPassword;
        public string birthyear;
        public string phone;
    }
    [Serializable]
    public struct Shop
    {
        public Box specialleftBox;
        public Box specialrightBox;

        public AwardBox awardleftBox;
        public AwardBox awardrightBox;

        public List<Box> coinsProducts;

        public List<Box> gemsProducts;
    }
    [Serializable]
    public struct AwardBox
    {
        public string id;
        public string image;
        public string remainderTime;
        public int current;
        public int max;
    }
    [Serializable]
    public struct Box
    {
        public string id;

        public string image;

    }


   /* [Serializable]
    public struct SoccerShopProducts
    {

        public List<SoccerShopItem> soccerProducts;
    }
    [Serializable]
    public struct SoccerShopItem
    {
        public string _id;
        public string name;
        public string title;
        public string description;
        public string image;
        public int price;
        public string priceType;//gem , coin , cup;
        public int time;
        public string timeString;
        public string type;///marble , plane
        public int aim;
        public int force;
    }*/
   
   /* [Serializable]
    public struct BilliardShopProducts
    {
        public List<BilliardShopItem> billiardProducts;

    }
    [Serializable]
    public struct BilliardShopItem
    {
        public string _id;
        public string name;
        public string title;
        public string description;
        public string image;
        public int price;
        public string priceType;//gem , coin , cup;
        public int time;
        public string timeString;
        public string type;///marble , plane
        public int aim;
        public int force;
        public int spin;
    }
    */
    [Serializable]
    public struct InGame
    {
        public string id;
        public string gameType;/// soccer , billiard
        public string namespaceServer;
    }
    [Serializable]
    public struct API
    {
        public string name;
        public string URL;
    }
    [Serializable]
    public struct _Apis
    {
       public List<API> apis;
        public string GETAPI(string name)
        {
            var url = "";
            apis.ForEach((e) => {
                if(e.name == name)
                {
                    url = e.URL;
                }
            });
            Debug.Log("URL:" + url);
            return url;
        }
    }
    [SerializeField]
    public struct CreateTeam
    {
        public string name;
        public string description;
        public int game;//0 biliard 1 scooer;
        public int mode;//0 private 1 public
        public int typeCost; //0 cup 1 coin 2 gem
        public int cost;
        public int capacity;
        public int hour;
        public int min;
        public List<string> invitation;
        public string leagueFlag;
    }
    [Serializable]
    public struct Opponent
    {
        public string userName;
        public string avatar;
        public int game;//0 soccer, 1 billiard
        public string namespaceServer;
    }
    [Serializable]
    public struct PlayWithFriend
    {
        public string friend;
        public int game;
        public int subgame;
    }
    public class SearchCheckCharacter
    {

        public static bool Check(string input)
        {
            char[] CharacterValidation = new char[5] { '+', '-', '*', '/', 'ƒ' };
            bool check = true;
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < CharacterValidation.Length; j++)
                {
                    if (input[i] == CharacterValidation[j])
                    {
                        check = false;
                    }
                }

            }

            return check;
        }
    }
}