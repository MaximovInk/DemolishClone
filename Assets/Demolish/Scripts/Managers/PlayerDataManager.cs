using System;
using UnityEngine;

namespace MaximovInk
{
    [Serializable]
    public struct PlayerData
    {
        public int Stars;
        public int CurrentLevel;
        public int Stage;
        public bool AdsDisabled;
    }

    public static class SerializationPlayerData
    {
        private const string STARS_KEY = "PS_STARS";
        private const string LEVEL_KEY = "PS_CURRENT_LEVEL";
        private const string STAGE_KEY = "PS_STAGE";
        private const string ADS_KEY = "PS_ADS";

        public static void SetData(PlayerData data)
        {
            PlayerPrefs.SetInt(STARS_KEY, data.Stars);
            PlayerPrefs.SetInt(LEVEL_KEY, data.CurrentLevel);
            PlayerPrefs.SetInt(STAGE_KEY, data.Stage);
            PlayerPrefs.SetInt(ADS_KEY, data.AdsDisabled? 1 : 0);
            PlayerPrefs.Save();
        }

        public static PlayerData GetData()
        {
            var data = new PlayerData
            {
                Stars = PlayerPrefs.GetInt(STARS_KEY, 0),
                CurrentLevel = PlayerPrefs.GetInt(LEVEL_KEY, 1),
                Stage = PlayerPrefs.GetInt(STAGE_KEY, 0),
                AdsDisabled = PlayerPrefs.GetInt(ADS_KEY, 0) == 1
            };

            return data;
        }

        public static void Clear()
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public class PlayerDataManager : MonoBehaviourSingletonAuto<PlayerDataManager>
    {

        private PlayerData _playerData = new();

        public event Action<int> OnLevelIndexAdd;

        public int GetStars() => _playerData.Stars;
        public int GetLevel() => _playerData.CurrentLevel;
        public int GetStage() => _playerData.Stage;
        public bool AdsDisabled => _playerData.AdsDisabled;

        public PlayerData GetPlayerData() => _playerData;

        public event Action<PlayerData> OnLoadEvent;
        public event Action<PlayerData> OnSaveEvent;
        
        private void Awake()
        {
            _playerData.CurrentLevel = -1;
            LevelManager.Instance.OnNextLevelInit += InstanceOnNextLevelInit;
        }

        private void Start()
        {
            Load();
        }

        private const int MAX_STAGE = 4;

        public bool IsReward => MAX_STAGE == _playerData.Stage;

        private void InstanceOnNextLevelInit()
        {
            _playerData.CurrentLevel++;
            _playerData.Stage++;

            if (_playerData.Stage > MAX_STAGE)
            {
                _playerData.Stage = 0;
            }

            Save(); 
            
            OnLevelIndexAdd?.Invoke(_playerData.CurrentLevel);
        }

        private void Save()
        {
            SerializationPlayerData.SetData(_playerData);
            OnSaveEvent?.Invoke(_playerData);
        }

        private void Load()
        {
            var prevLevel = _playerData.CurrentLevel;

            _playerData = SerializationPlayerData.GetData();

            if (prevLevel != _playerData.CurrentLevel)
            {
                OnLevelIndexAdd?.Invoke(_playerData.CurrentLevel);
            }

            OnLoadEvent?.Invoke(_playerData);
        }

        public void ClearAndApply()
        {
            SerializationPlayerData.Clear();
            Load();
        }

        public void AddStars(int value)
        {
            _playerData.Stars += value;
            Save();
        }

        public void DisableAds()
        {
            _playerData.AdsDisabled = true;
            Save();
        }
    }
}
