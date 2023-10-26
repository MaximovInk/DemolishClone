using UnityEngine;
using UnityEngine.UI;
using GamePush;

namespace MaximovInk
{
    [RequireComponent(typeof(Button))]
    public class WeaponGetAd : MonoBehaviour
    {
        private const string REWARD_ID = "AMMO_REWARD";

        [SerializeField] private int _ammoID = -1;

        private void Awake()
        {
            var button = GetComponent<Button>();

            var buttonInfo = GetComponentInParent<WeaponButton>();
            _ammoID = buttonInfo.AmmoID;

            button.onClick.AddListener(OnClick);
        }

        public void OnClick()
        {
            if (!GP_Ads.IsRewardedAvailable()) return;

            GP_Ads.ShowRewarded(REWARD_ID, _ =>
            {
                OnReward();
            });
            
        }

        private void OnReward()
        {
            UIManager.Instance.Screens.ShowScreen("Reward");
            UIManager.Instance.RewardScreen.GenerateOnce(CannonManager.Instance.AmmoDatabase.GetAmmoType(_ammoID));
        }
    }
}
