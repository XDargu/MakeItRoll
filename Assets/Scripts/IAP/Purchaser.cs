using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

// Placing the Purchaser class in the CompleteProject namespace allows it to interact with ScoreManager, 
// one of the existing Survival Shooter scripts.
namespace CompleteProject
{
	// Deriving the Purchaser class from IStoreListener enables it to receive messages from Unity Purchasing.
	public class Purchaser : MonoBehaviour, IStoreListener
	{
		private static IStoreController m_StoreController;          // The Unity Purchasing system.
		private static IExtensionProvider m_StoreExtensionProvider; // The store-specific Purchasing subsystems.

		// Product identifiers for all products capable of being purchased: 
		// "convenience" general identifiers for use with Purchasing, and their store-specific identifier 
		// counterparts for use with and outside of Unity Purchasing. Define store-specific identifiers 
		// also on each platform's publisher dashboard (iTunes Connect, Google Play Developer Console, etc.)

		// General product identifiers for the consumable, non-consumable, and subscription products.
		// Use these handles in the code to reference which product to purchase. Also use these values 
		// when defining the Product Identifiers on the store. Except, for illustration purposes, the 
		// kProductIDSubscription - it has custom Apple and Google identifiers. We declare their store-
		// specific mapping to Unity Purchasing's AddProduct, below.

        public const string DOUBLE_PAPER = "double_paper";
        public const string DOUBLE_METERS = "double_meters";
        public const string x100_PRODUCTION = "x100_production";
        public const string METERS_1000 = "meters_1000";

        public GameObject targetUI;
        public InAppProductItem storeItemPrefab;

		void Start()
		{
			// If we haven't set up the Unity Purchasing reference
			if (m_StoreController == null)
			{
				// Begin to configure our connection to Purchasing
				InitializePurchasing();
			}

            // Load local data regarding purchased products, in case user does not have internet access
            foreach (DataManager.InAppProduct product in DataManager.inAppProducts)
            {
                if (PlayerPrefs.HasKey(product.ID))
                {
                    SetProductPurchased(product.ID, false);
                }
            }
		}

		public void InitializePurchasing()
		{
			// If we have already connected to Purchasing ...
			if (IsInitialized())
			{
				// ... we are done here.
				return;
			}

			// Create a builder, first passing in a suite of Unity provided stores.
			var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

			// Add a product to sell / restore by way of its identifier, associating the general identifier
			// with its store-specific identifiers.
            foreach (DataManager.InAppProduct product in DataManager.inAppProducts)
            {
                builder.AddProduct(product.ID, ProductType.Consumable, new IDs(){
				{ product.ID, GooglePlay.Name },
			});
            }

			// Kick off the remainder of the set-up with an asynchrounous call, passing the configuration 
			// and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
			UnityPurchasing.Initialize(this, builder);
		}


		private bool IsInitialized()
		{
			// Only say we are initialized if both the Purchasing references are set.
			return m_StoreController != null && m_StoreExtensionProvider != null;
		}

		void BuyProductID(string productId)
		{
			// If Purchasing has been initialized ...
			if (IsInitialized())
			{
				// ... look up the Product reference with the general product identifier and the Purchasing 
				// system's products collection.
				Product product = m_StoreController.products.WithID(productId);

				// If the look up found a product for this device's store and that product is ready to be sold ... 
				if (product != null && product.availableToPurchase)
				{
					Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
					// ... buy the product. Expect a response either through ProcessPurchase or OnPurchaseFailed 
					// asynchronously.
					m_StoreController.InitiatePurchase(product);
				}
				// Otherwise ...
				else
				{
					// ... report the product look-up failure situation  
					Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
				}
			}
			// Otherwise ...
			else
			{
				// ... report the fact Purchasing has not succeeded initializing yet. Consider waiting longer or 
				// retrying initiailization.
				Debug.Log("BuyProductID FAIL. Not initialized.");
			}
		}


		// Restore purchases previously made by this customer. Some platforms automatically restore purchases, like Google. 
		// Apple currently requires explicit purchase restoration for IAP, conditionally displaying a password prompt.
		public void RestorePurchases()
		{
			// If Purchasing has not yet been set up ...
			if (!IsInitialized())
			{
				// ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
				Debug.Log("RestorePurchases FAIL. Not initialized.");
				return;
			}

			
			// We are not running on an Apple device. No work is necessary to restore purchases.
			Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
			
		}


		//  
		// --- IStoreListener
		//

		public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
		{
			// Purchasing has succeeded initializing. Collect our Purchasing references.
			Debug.Log("OnInitialized: PASS");

			// Overall Purchasing system, configured with products for this application.
			m_StoreController = controller;
			// Store specific subsystem, for accessing device-specific store features.
			m_StoreExtensionProvider = extensions;

            PopulateUI(m_StoreController.products.all);
		}


		public void OnInitializeFailed(InitializationFailureReason error)
		{
			// Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
			Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
		}

        void SetProductPurchased(string ID, bool justPurchased)
        {
            if (String.Equals(ID, DOUBLE_METERS, StringComparison.Ordinal))
            {
                DataManager.DoubleMeters();
                PlayerPrefs.SetInt("no_ads", 1);
                PlayerPrefs.SetInt(DOUBLE_METERS, 1);
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", ID));
            }
            else if (String.Equals(ID, x100_PRODUCTION, StringComparison.Ordinal))
            {
                if (justPurchased)
                {
                    DataManager.x100Production();
                }
                PlayerPrefs.SetInt("no_ads", 1);
                PlayerPrefs.SetInt(x100_PRODUCTION, 1);
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", ID));
            }
            else if (String.Equals(ID, METERS_1000, StringComparison.Ordinal))
            {
                if (justPurchased)
                {
                    DataManager.x1000Meters();
                }
                PlayerPrefs.SetInt("no_ads", 1);
                PlayerPrefs.SetInt(METERS_1000, 1);
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", ID));
            }
            else if (String.Equals(ID, DOUBLE_PAPER, StringComparison.Ordinal))
            {
                DataManager.EnableDoublePaper();
                PlayerPrefs.SetInt("no_ads", 1);
                PlayerPrefs.SetInt(DOUBLE_PAPER, 1);
                Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", ID));
            }
            else
            {
                Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", ID));
            }
        }

		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
		{
            SetProductPurchased(args.purchasedProduct.definition.id, true);

			// Return a flag indicating whether this product has completely been received, or if the application needs 
			// to be reminded of this purchase at next app launch. Use PurchaseProcessingResult.Pending when still 
			// saving purchased products to the cloud, and when that save is delayed. 
			return PurchaseProcessingResult.Complete;
		}


		public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
		{
			// A product purchase attempt did not succeed. Check failureReason for more detail. Consider sharing 
			// this reason with the user to guide their troubleshooting actions.
			Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
		}

        void PopulateUI(Product[] products)
        {
            foreach (Product product in products)
            {
                bool itemBought = false;

                // Check if you already have the product
                // We will NOT override local storage data
                // Probably mean that it can be easily hacked, but better that than not giving a user what he paid for!
                if (product.hasReceipt)
                {
                    itemBought = true;
                    SetProductPurchased(product.definition.id, false);
                }

                DataManager.SetProductData(product.definition.id, product.metadata.localizedTitle, product.metadata.localizedDescription, product.metadata.localizedPriceString);

                InAppProductItem item = Instantiate(storeItemPrefab) as InAppProductItem;
                item.transform.SetParent(targetUI.transform, false);
                item.transform.SetSiblingIndex(targetUI.transform.childCount - 3);
                item.SetProduct(product, BuyProductID, itemBought);
            }

            DataManager.SetOnlineProductsLoaded();
        }
	}
}