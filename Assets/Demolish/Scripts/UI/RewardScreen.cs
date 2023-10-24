using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MaximovInk
{
    public class RewardScreen : LayoutScreen
    {
        [SerializeField] private Transform _rewardsParent;

        [SerializeField] private Image _rewardPrefab;

        private RewardGenerated[] _currentRewards;

        public void GenerateChestRewards()
        {
            var rewards = RewardManager.Instance.GenerateChest();

            MKUtils.DestroyAllChildren(_rewardsParent);

            for (int i = 0; i < rewards.Length; i++)
            {
                var instance = Instantiate(_rewardPrefab, _rewardsParent);

                instance.sprite = rewards[i].Icon;

                Debug.Log(instance.name);
                //Not good for perfomance!:
                instance.GetComponentInChildren<TextMeshProUGUI>().text = rewards[i].Count.ToString();
            }

            _currentRewards = rewards;
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

        public void GetMultiplied()
        {
            for (int i = 0; i < _currentRewards.Length; i++)
            {
                var reward = _currentRewards[i];
                reward.Count *= 3;
                _currentRewards[i] = reward;
            }

            Get();
        }

    }

}
