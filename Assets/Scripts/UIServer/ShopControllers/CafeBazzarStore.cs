using System;
using System.IO;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Diaco.Store.CafeBazzar
{
    public class CafeBazzarStore : MonoBehaviour, IStoreListener

    {


        [SerializeField] private CafeBazzarData cafeBazzarData;
        

        private IStoreController storeController;

        private bool Cafe_initialized = false;
        public static CafeBazzarStore instance;
        private void Start()
        {
            if (instance == null)
                instance = this;
            if (Cafe_initialized == false)
                InitializeCafebazzarShop();
        }

        public void InitializeCafebazzarShop()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            for (int i = 0; i < cafeBazzarData.cafebazzarItems.Count; i++)
            {
                var name = cafeBazzarData.cafebazzarItems[i].name;
                var typeitem = cafeBazzarData.cafebazzarItems[i].productType;
                builder.AddProduct(name, typeitem);
               // Debug.Log($"AddProduct{name}{typeitem}");
                
            }
            Cafe_initialized = true;
            UnityPurchasing.Initialize(this, builder);
        }





        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {

            this.storeController = controller;

            Debug.Log("Cafebazzar Store Initialized");

        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            //_Debug.text = "Store Failed" + error.ToString();
            Debug.Log("Store Failed" + error.ToString());
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            //_Debug.text = "Store buy Failed" + product.definition.id;
            Debug.Log("Store buy Failed" + product.definition.id);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {

            var product_id = purchaseEvent.purchasedProduct.definition.id;
            var transaction_id = purchaseEvent.purchasedProduct.transactionID;
            var package_name = Application.identifier;

            TransactionData t = new TransactionData() { packageName = package_name, productId = product_id, transactionId = transaction_id };
            var json = JsonUtility.ToJson(t);
            SaveTransaction("translog", json);

            //TransactionIDSendToServer(json);
            Debug.Log($"PurchaseProcessingResult:{product_id}.............{transaction_id}");
            return PurchaseProcessingResult.Complete;
        }





        public void DoTransaction(string product, string payload)
        {
            storeController.InitiatePurchase(product, payload);
        }

        /*private void TransactionIDSendToServer(string tranlog)
        {
            FindObjectOfType<ServerUI>().Emit_Transaction(tranlog);
        }*/



        private void SaveTransaction(string FileName, string Token)
        {
            if (File.Exists(Application.persistentDataPath + "//" + FileName + ".json"))
            {
                File.Delete(Application.persistentDataPath + "//" + FileName + ".json");
            }
            File.WriteAllText(Application.persistentDataPath + "//" + FileName + ".json", Token);
            Debug.Log("translog Saved");
        }
        public string ReadTransaction(string FileName)
        {
            //  TransactionData data = new TransactionData();
            string J_token = "";
            if (File.Exists(Application.persistentDataPath + "//" + FileName + ".json"))
            {
                J_token = File.ReadAllText(Application.persistentDataPath + "//" + FileName + ".json");
               // data = JsonUtility.FromJson<TransactionData>(j_token);

            }
            Debug.Log("translog Readed");
            return J_token;
        }
        public bool ExistTransactionFile(string FileName)
        {
            bool find = false;
            if (File.Exists(Application.persistentDataPath + "//" + FileName + ".json"))
            {
                find = true;
            }
            Debug.Log("translog Exist");
            return find;
        }
        public void DeleteTransaction(string FileName)
        {

            if (File.Exists(Application.persistentDataPath + "//" + FileName + ".json"))
            {
                File.Delete(Application.persistentDataPath + "//" + FileName + ".json");
            }
            Debug.Log("TranlogDeleted");
        }
    }


    [Serializable]
    public struct CafebazzarItem
    {
        public string name;
        public ProductType productType;
    }
    [Serializable]
    public struct CafeBazzarData
    {
        public List<CafebazzarItem> cafebazzarItems;
    }
    [Serializable]
    public struct TransactionData
    {
        public string productId;
        public string transactionId;
        public string packageName;
    }
}