using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = UnityEngine.Random;

namespace MaximovInk
{
    [System.Serializable]
    public struct RewardInfo
    {
        public AmmoType Type;

        [Range(0,100)]
        public int Chance;

        [Header("Min-Max")]
        public Vector2Int Count;

        public Sprite Icon;
        
    }

    [System.Serializable]
    public struct RewardGenerated
    {
        public Sprite Icon;
        public AmmoType Type;
        public int Count;
    }

    public class RewardManager : MonoBehaviourSingleton<RewardManager>
    {
        [SerializeField] private int _chestRewardMinCount = 1;
        [SerializeField] private int _chestRewardMaxCount = 3;
        [SerializeField] private RewardInfo[] _rewards;

        private Dictionary<AmmoType, RewardInfo> _cachedRewardInfos = new Dictionary<AmmoType, RewardInfo>();

        class Items<T>
        {
            public double Probability { get; set; }
            public T Item { get; set; }
        }

        private void Awake()
        {
            if (_cachedRewardInfos.Count != _rewards.Length)
            {
                CacheRewardInfos();
            }
        }

        private void CacheRewardInfos()
        {
            for (int i = 0; i < _rewards.Length; i++)
            {
                _cachedRewardInfos.Add(_rewards[i].Type, _rewards[i]);
            }
        }

        public RewardGenerated[] GenerateOnce(AmmoType type)
        {
            RewardGenerated[] rewards = new RewardGenerated[1];

            var rewardInfo = _cachedRewardInfos[type];

            var count = Random.Range(rewardInfo.Count.x, rewardInfo.Count.y);

            rewards[0] = new RewardGenerated { Count = count, Type = type, Icon = rewardInfo.Icon };

            return rewards;
        }
        private RewardGenerated GenerateChestOnceReward(ref List<AmmoType> availableTypes)
        {
            if(_cachedRewardInfos.Count != _rewards.Length) {
                CacheRewardInfos();
            }

            RewardGenerated reward = default;

            if (availableTypes == null || availableTypes.Count == 0) return reward;

            var converted = new List<Items<AmmoType>>(availableTypes.Count);

            var sum = 0.0;
            foreach (var item in availableTypes.Take(availableTypes.Count - 1))
            {
                sum += _cachedRewardInfos[item].Chance/100f;
                converted.Add(new Items<AmmoType> { Probability = sum, Item = item });
            }

            converted.Add(new Items<AmmoType> { Probability = 1.0, Item = availableTypes.Last() });

            while (true)
            {
                var probability = Random.Range(0, 1f);
                var selected = converted.SkipWhile(i => i.Probability < probability).First().Item;

                var rewardInfo = _cachedRewardInfos[selected];

                var count = Random.Range(rewardInfo.Count.x, rewardInfo.Count.y);

                availableTypes.Remove(selected);

                return new RewardGenerated { Count = count, Type = selected, Icon = rewardInfo.Icon };
            }

        }

        private List<AmmoType> GetAvailableTypes()
        {
            /*
              var typesAvailable = Enum.GetValues(typeof(AmmoType))
    .Cast<AmmoType>()
    .ToList();

            typesAvailable.Remove(AmmoType.Basic); 
            */
            List<AmmoType> ammoTypes = new List<AmmoType>();

            foreach (var rewardInfo in _cachedRewardInfos)
            {
                ammoTypes.Add(rewardInfo.Key);
            }

            return ammoTypes;
        }

        public RewardGenerated[] GenerateChest()
        {
            int count = Random.Range(_chestRewardMinCount, _chestRewardMaxCount);

            RewardGenerated[] rewards = new RewardGenerated[count];

            var typesAvailable = GetAvailableTypes();

            for (int i = 0; i < count; i++)
            {
                rewards[i] = GenerateChestOnceReward(ref typesAvailable);
            }

            return rewards;
        }
    }
}
