using System;

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
            TransactionIDSendToServer(transaction_id, product_id, package_name);
           // Debug.Log($"PurchaseProcessingResult:{product_id}.............{transaction_id}");
            return PurchaseProcessingResult.Complete;
        }

        public void DoTransaction(string product, string payload)
        {
            storeController.InitiatePurchase(product, payload);
        }

        private void TransactionIDSendToServer(string tranaction_id, string product_id, string package_name)
        {
            FindObjectOfType<ServerUI>().Emit_Transaction(tranaction_id, product_id, package_name);
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
}