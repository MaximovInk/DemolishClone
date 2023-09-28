using System;
using UnityEngine;

namespace MaximovInk
{
    [System.Serializable]
    public struct PlayerData
    {
        public int Stars;
        public int CurrentLevel;
    }

    public static class SerializationPlayerData
    {
        private const string STARS_KEY = "PS_STARS";
        private const string LEVEL_KEY = "PS_CURRENT_LEVEL";

        public static void SetData(PlayerData data)
        {
            PlayerPrefs.SetInt(STARS_KEY, data.Stars);
            PlayerPrefs.SetInt(LEVEL_KEY, data.CurrentLevel);
            PlayerPrefs.Save();
        }

        public static PlayerData GetData()
        {
            var data = new PlayerData
            {
                Stars = PlayerPrefs.GetInt(STARS_KEY, 0),
                CurrentLevel = PlayerPrefs.GetInt(LEVEL_KEY, 1)
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

        private void InstanceOnNextLevelInit()
        {
            _playerData.CurrentLevel++;
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
