using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;
using UnityEngine.UI;

namespace Samples.Purchasing.Core.BuyingSubscription
{
    public class BuyingSubscription : MonoBehaviour, IDetailedStoreListener
    {

        public Image subscriptionButton;
        public Button enterGame;
        public TMPro.TMP_Text subStatus;

        IStoreController m_StoreController;

        // Your subscription ID. It should match the id of your subscription in your store.
        public string subscriptionProductId = "com.mycompany.mygame.my_vip_pass_subscription";

        public Text isSubscribedText;
        [SerializeField]
        bool isTrial;
        void Start()
        {

            if (PlayerPrefs.HasKey("InstallationDate")) {
                string installationDate = PlayerPrefs.GetString("InstallationDate");
                DateTime installDateTime = DateTime.Parse(installationDate);
                DateTime currentDateTime = System.DateTime.Now;
                subscriptionButton.GetComponent<Button>().interactable = false;
                isTrial = true;
                TimeSpan difference = currentDateTime - installDateTime;
                subStatus.text = "Not Subscribed!";
                int daysLeft = 7 - (int)difference.TotalDays;
                isSubscribedText.text = daysLeft + " Days left for Trial!";
                if (difference.TotalDays >= 7) {
                    // Activate the feature or content
                    subscriptionButton.GetComponent<Button>().interactable = true;
                    isTrial = false;
                }
                Debug.Log("is Trial: " + isTrial);
            } else {
                isTrial = true;

                PlayerPrefs.SetString("InstallationDate", System.DateTime.Now.ToString());
                string installationDate = PlayerPrefs.GetString("InstallationDate");
                DateTime installDateTime = DateTime.Parse(installationDate);
                DateTime currentDateTime = System.DateTime.Now;
                subscriptionButton.GetComponent<Button>().interactable = false;


                TimeSpan difference = currentDateTime - installDateTime;
                subStatus.text = "Not Subscribed!";
                int daysLeft = 7 - (int)difference.TotalDays;
                isSubscribedText.text = daysLeft + " Days left for Trial!";
            }
            InitializePurchasing();

        }

        void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            // Add our purchasable product and indicate its type.
            builder.AddProduct(subscriptionProductId, ProductType.Subscription);

            UnityPurchasing.Initialize(this, builder);
        }

        public void BuySubscription()
        {
            m_StoreController.InitiatePurchase(subscriptionProductId);
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            Debug.Log("In-App Purchasing successfully initialized");
            m_StoreController = controller;

            UpdateUI();
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            OnInitializeFailed(error, null);
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            var errorMessage = $"Purchasing failed to initialize. Reason: {error}.";

            if (message != null)
            {
                errorMessage += $" More details: {message}";
            }

            Debug.Log(errorMessage);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
        {
            // Retrieve the purchased product
            var product = args.purchasedProduct;

            Debug.Log($"Purchase Complete - Product: {product.definition.id}");

            UpdateUI();

            // We return Complete, informing IAP that the processing on our side is done and the transaction can be closed.
            return PurchaseProcessingResult.Complete;
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.Log($"Purchase failed - Product: '{product.definition.id}', PurchaseFailureReason: {failureReason}");
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            Debug.Log($"Purchase failed - Product: '{product.definition.id}'," +
                $" Purchase failure reason: {failureDescription.reason}," +
                $" Purchase failure details: {failureDescription.message}");
        }

        bool IsSubscribedTo(Product subscription)
        {
            // If the product doesn't have a receipt, then it wasn't purchased and the user is therefore not subscribed.
            if (subscription.receipt == null)
            {
                return false;
            }

            //The intro_json parameter is optional and is only used for the App Store to get introductory information.
            var subscriptionManager = new SubscriptionManager(subscription, null);

            // The SubscriptionInfo contains all of the information about the subscription.
            // Find out more: https://docs.unity3d.com/Packages/com.unity.purchasing@3.1/manual/UnityIAPSubscriptionProducts.html
            var info = subscriptionManager.getSubscriptionInfo();

            return info.isSubscribed() == Result.True;
        }

        void UpdateUI()
        {
            var subscriptionProduct = m_StoreController.products.WithID(subscriptionProductId);
            Debug.Log("Update and check subscription");
            try
            {
                var isSubscribed = IsSubscribedTo(subscriptionProduct);
                Debug.Log("isSub" + isSubscribed);
                //isSubscribedText.text = isSubscribed ? "You are subscribed" : "You are not subscribed";
                Debug.Log("Is Trial Period: " + isTrial);
                if(isSubscribed || isTrial) {
                    enterGame.enabled = true;
                    subscriptionButton.color = Color.green;
                    subStatus.text = isTrial ? "Trial" : "Subscribed!";
                } else {
                    enterGame.enabled = false;
                    subscriptionButton.color = Color.red;
                    subStatus.text = "Not Subscribed!";

                }
            }
            catch (StoreSubscriptionInfoNotSupportedException)
            {

                var receipt = (Dictionary<string, object>)MiniJson.JsonDecode(subscriptionProduct.receipt);
                Debug.Log("Error" + receipt["Store"]);
                var store = receipt["Store"];
                isSubscribedText.text =
                    "Couldn't retrieve subscription information because your current store is not supported.\n" +
                    $"Your store: \"{store}\"\n\n" +
                    "You must use the App Store, Google Play Store or Amazon Store to be able to retrieve subscription information.\n\n" +
                    "For more information, see README.md";
            }
        }
    }
}
