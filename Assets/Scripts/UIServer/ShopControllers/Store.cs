
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Diaco.Store
{
    public class Store : MonoBehaviour
    {

        public ServerUI Server;
        public NavigationUI navigationUI;
        public Button SoccerShopButton;
        public Button BilliardShopButton;
        public SpecialBox SpecialBoxleft;
        public SpecialBox SpecialBoxright;
        public AwardElement AwardBoxLeft;
        public AwardElement AwardBoxRight;
        [SerializeField]
        public List<BuyBoxElement> BuyBoxCoins;
        [SerializeField]
        public List<BuyBoxElement> BuyBoxGems;
       
        private void OnEnable()
        {
            Server.OnShopLoaded += Server_OnShopLoaded;
        }
        private void OnDisable()
        {
            Server.OnShopLoaded -= Server_OnShopLoaded;
            RemoveAllListeners();

        }
        private void  OnDestory()

        {
            RemoveAllListeners();
            Server.OnShopLoaded -= Server_OnShopLoaded;
        }
        private void Server_OnShopLoaded(HTTPBody.Shop shopdata)
        {
            StartCoroutine(InitializeShopPage(shopdata));
        }

        public IEnumerator InitializeShopPage(HTTPBody.Shop data)
        {
            RemoveAllListeners();
            yield return new WaitForSeconds(0.1f);
            var imagespecialleftBox = ConvertImageToSprite(data.specialleftBox.image);
            SpecialBoxleft.Set(data.specialleftBox.id, imagespecialleftBox);
            yield return new WaitForSeconds(0.1f);

            var imagespecialrightBox = ConvertImageToSprite(data.specialrightBox.image);
            SpecialBoxright.Set(data.specialrightBox.id, imagespecialrightBox);
            yield return new WaitForSeconds(0.1f);


            var imageawardleft = ConvertImageToSprite(data.awardleftBox.image);
            AwardBoxLeft.Set(data.awardleftBox.id, imageawardleft, data.awardleftBox.remainderTime);
            yield return new WaitForSeconds(0.1f);

            var imageawardright = ConvertImageToSprite(data.awardrightBox.image);
            AwardBoxRight.Set2(data.awardrightBox.id, imageawardright,data.awardrightBox.current,data.awardrightBox.max);
            yield return new WaitForSeconds(0.1f);

            for (int i = 0; i < data.coinsProducts.Count; i++)
            {
                var id = data.coinsProducts[i].id;
                var image = ConvertImageToSprite(data.coinsProducts[i].image);
                BuyBoxCoins[i].set(id,image);
            }
            yield return new WaitForSeconds(0.1f);

            for (int i = 0; i<data.gemsProducts.Count; i++)
            {
                var id = data.gemsProducts[i].id;
                var image = ConvertImageToSprite(data.gemsProducts[i].image);
                BuyBoxGems[i].set(id, image);
            }
            yield return new WaitForSeconds(0.1f);

        }

        private Sprite ConvertImageToSprite(string image)
        {
            
            var image_byte = Convert.FromBase64String(image);
            Texture2D texture = new Texture2D(512, 512, TextureFormat.DXT5, false);
            texture.LoadImage(image_byte);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }

        private void RemoveAllListeners()
        {
            SpecialBoxleft.RemoveListener();
            SpecialBoxright.RemoveListener();

            AwardBoxLeft.RemoveListener();
            AwardBoxRight.RemoveListener();

            for (int i = 0; i < BuyBoxCoins.Count; i++)
            {

                BuyBoxCoins[i].RemoveListener();
            }
            for (int i = 0; i < BuyBoxGems.Count; i++)
            {

                BuyBoxGems[i].RemoveListener();
            }
        }
    }
    [Serializable]
    public struct SpecialBox
    {
        public string id;
        public Image imageLeft;
        
       // public Image imageRight;
       // public Text countLeft;
       /// public Text countRight;
        ///public Text priceBox;
        public Button btnSubmit;
        public void Set(string id , Sprite imageleft)
        {
            this.id = id;
            imageLeft.sprite = imageleft;
            btnSubmit.onClick.AddListener(onclick);
        }
        public void onclick()
        {
            var server = UnityEngine.GameObject.FindObjectOfType<ServerUI>();

            server.Emit_Shop(id);
            Debug.Log(id);
        }


        public void RemoveListener()
        {
            btnSubmit.onClick.RemoveAllListeners();
        }
    }
    [Serializable]
    public struct AwardElement
    {
        public string Id;
        public Image backGround;
        public Text RemainderTime;

        public Text IndicatorValue;

        public Image Prograssbar;
        public float CurrentValue;
        public float MaxValue;


        public Button btnSubmit;
        public void Set(string id,   Sprite backimage, string remainderTime)
        {
            Id = id; 
            backGround.sprite = backimage;
            RemainderTime.text = remainderTime;
            btnSubmit.onClick.AddListener(onclick);
        }
        public void Set2(string id, Sprite backimage, float current , float max)
        {
            Id = id;
            backGround.sprite = backimage;
            var value = current / max;
            Prograssbar.fillAmount = value;
            IndicatorValue.text = current + "/" + max;
            btnSubmit.onClick.AddListener(onclick);
        }
        public void onclick()
        {

            var server = UnityEngine.GameObject.FindObjectOfType<ServerUI>();

            server.Emit_Shop(Id);
            Debug.Log(Id);
        }
        public void RemoveListener()
        {
            btnSubmit.onClick.RemoveAllListeners();
        }
    }
    [Serializable]
    public struct BuyBoxElement
    {
        public string Id;
        public Button Gems_btn;
        public Image backGround;
        public void set(string id, Sprite back_image)
        {
            Id = id;
            backGround.sprite = back_image;
            Gems_btn.onClick.AddListener(onclick);
        }
        public void onclick()
        {
            var server = UnityEngine.GameObject.FindObjectOfType<ServerUI>();

            server.Emit_Shop(Id);
            Debug.Log(Id);
        }
        public void RemoveListener()
        {
            Gems_btn.onClick.RemoveAllListeners();
        }
    }
}