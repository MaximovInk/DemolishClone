using GamePush;
using UnityEngine;
using UnityEngine.UI;

namespace MaximovInk
{
    [RequireComponent(typeof(Button))]
    public class PurchaseButton : MonoBehaviour
    {
        private void Awake()
        {
            PlayerDataManager.Instance.OnLoadEvent += Instance_OnLoadEvent;
            GP_Payments.OnFetchPlayerPurchases += GP_Payments_OnFetchPlayerPurchases;
            GP_Payments.OnFetchProducts += GP_Payments_OnFetchProducts;
        }

        private void Start()
        {
            GP_Payments.Fetch();
        }

        private void GP_Payments_OnFetchProducts(System.Collections.Generic.List<FetchProducts> arg0)
        {
            Debug.Log($"FETCH available products: {arg0.Count}");
            for (int i = 0; i < arg0.Count; i++)
            {
                Debug.Log($"{i} {arg0[i].name} {arg0[i].price} {arg0[i].id}");
            }

        }

        private void GP_Payments_OnFetchPlayerPurchases(System.Collections.Generic.List<FetchPlayerPurcahses> arg0)
        {
            Debug.Log($"FETCH player purchases: {arg0.Count}");
            for (int i = 0; i < arg0.Count; i++)
            {
                Debug.Log($"Player purchase - {i+1} productID: {arg0[i].productId}");
            }
        }

        private void Instance_OnLoadEvent(PlayerData obj)
        {
            var adsDisabled = PlayerDataManager.Instance.AdsDisabled;

            gameObject.SetActive(!adsDisabled);

            if (adsDisabled) return;

            var button = GetComponent<Button>();

            button.onClick.AddListener(OnClick);

            button.interactable = GP_Payments.IsPaymentsAvailable();
        }

        private void OnClick()
        {
            if (!GP_Payments.IsPaymentsAvailable()) return;

            GP_Payments.Purchase(UIManager.Instance.NoAdPurchaseID, idOrTag =>
            {
                PlayerDataManager.Instance.DisableAds();
                gameObject.SetActive(false);
            }, (() => { print($"PURCHASE ERROR id: {UIManager.Instance.NoAdPurchaseID}");}));
        }
    }
}