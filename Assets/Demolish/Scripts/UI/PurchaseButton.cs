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
                if (idOrTag != UIManager.Instance.NoAdPurchaseID) return;

                PlayerDataManager.Instance.DisableAds();
                gameObject.SetActive(false);
            }, null);
        }
    }
}