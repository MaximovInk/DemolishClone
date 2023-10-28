using GamePush;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MaximovInk
{
    public class RewardScreen : LayoutScreen
    {
        private const string REWARD_ID = "REWARD_MUTLIPLY";

        [SerializeField] private Transform _rewardsParent;

        [SerializeField] private Image _rewardPrefab;

        [SerializeField] private GameObject _multiplyButton;

        private RewardGenerated[] _currentRewards;

        private Button _backgroundButton;

        private void Awake()
        {
            _backgroundButton = GetComponent<Button>();
            _backgroundButton.onClick.AddListener(() => {
                if (_multiplyButton.activeSelf) return;

                Get();
            });
        }

        public void GenerateChestRewards()
        {
            _currentRewards = RewardManager.Instance.GenerateChest();

            InitRewards(_currentRewards);

            _multiplyButton.SetActive(true);
        }


        public void GenerateOnce(AmmoType type)
        {
            _currentRewards = RewardManager.Instance.GenerateOnce(type);

            InitRewards(_currentRewards);

            _multiplyButton.SetActive(false);

            MKUtils.Invoke(this, () => {
                Get();
            }, 2f);
        }

        private void InitRewards(RewardGenerated[] rewards)
        {
            MKUtils.DestroyAllChildren(_rewardsParent);

            for (int i = 0; i < rewards.Length; i++)
            {
                var instance = Instantiate(_rewardPrefab, _rewardsParent);

                instance.sprite = rewards[i].Icon;

                //Not good for perfomance!:
                instance.GetComponentInChildren<TextMeshProUGUI>().text = rewards[i].Count.ToString();
            }
        }


        public void Get()
        {
            for (int i = 0; i < _currentRewards.Length; i++)
            {
                var reward = _currentRewards[i];

                WeaponSerialization.AddAmmoData(CannonManager.Instance.AmmoDatabase.GetIndex(reward.Type), reward.Count);
            }

            WeaponButton.UpdateAllButtons();

            Close();
        }

        private void Multiply()
        {
            for (int i = 0; i < _currentRewards.Length; i++)
            {
                var reward = _currentRewards[i];
                reward.Count *= 3;
                _currentRewards[i] = reward;
            }
        }

        public void GetMultiplied()
        {
            if (PlayerDataManager.Instance.AdsDisabled)
            {
                Multiply();
                Get();

                return;
            }

            if (!GP_Ads.IsRewardedAvailable())
            {
                Get();
                return;
            }


            GP_Ads.ShowRewarded(REWARD_ID,
                idOrTag =>
                {
                    if (idOrTag != REWARD_ID) return;

                    Multiply();

                    Get();
                },
                null,
                success =>
                {
                    if (success) return;

                    Get();
                });
        }

    }

}
