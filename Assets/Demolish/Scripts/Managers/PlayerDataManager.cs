using System;
using UnityEngine;

namespace MaximovInk
{
    [System.Serializable]
    public struct PlayerData
    {
        public int Stars;
        public int CurrentLevel;
        public int Stage;
    }

    public static class SerializationPlayerData
    {
        private const string STARS_KEY = "PS_STARS";
        private const string LEVEL_KEY = "PS_CURRENT_LEVEL";
        private const string STAGE_KEY = "PS_STAGE";

        public static void SetData(PlayerData data)
        {
            PlayerPrefs.SetInt(STARS_KEY, data.Stars);
            PlayerPrefs.SetInt(LEVEL_KEY, data.CurrentLevel);
            PlayerPrefs.SetInt(STAGE_KEY, data.Stage);
            PlayerPrefs.Save();
        }

        public static PlayerData GetData()
        {
            var data = new PlayerData
            {
                Stars = PlayerPrefs.GetInt(STARS_KEY, 0),
                CurrentLevel = PlayerPrefs.GetInt(LEVEL_KEY, 1),
                Stage = PlayerPrefs.GetInt(STAGE_KEY, 0)
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

        public int GetStars() => _playerData.Stars;
        public int GetLevel() => _playerData.CurrentLevel;
        public int GetStage() => _playerData.Stage;

        public event Action<PlayerData> OnLoadEvent;
        public event Action<PlayerData> OnSaveEvent;
        
        private void Awake()
        {
            LevelManager.Instance.OnNextLevelInit += InstanceOnNextLevelInit;
        }

        private void Start()
        {
            Load();
        }

        private const int MAX_STAGE = 4;

        private void InstanceOnNextLevelInit()
        {
            _playerData.CurrentLevel++;
            _playerData.Stage++;

            if (_playerData.Stage > MAX_STAGE)
                _playerData.Stage = 0;

            Save();
        }

        private void Save()
        {
            SerializationPlayerData.SetData(_playerData);
            OnSaveEvent?.Invoke(_playerData);
        }

        private void Load()
        {
            _playerData = SerializationPlayerData.GetData();
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
    }
}
