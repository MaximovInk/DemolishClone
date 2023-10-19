using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MaximovInk
{
    public class BuildingSlider : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nextLevelText;
        [SerializeField] private TextMeshProUGUI _currentLevelText;
        [SerializeField] private Slider _slider;

        private void Awake()
        {
            LevelManager.Instance.OnStateChangedEvent += Instance_OnStateChangedEvent;

            PlayerDataManager.Instance.OnLoadEvent += UpdateInfo;
            PlayerDataManager.Instance.OnSaveEvent += UpdateInfo;
        }

        private void OnEnable()
        {
            Instance_OnStateChangedEvent(LevelManager.Instance.BuildingState);
            UpdateInfo(PlayerDataManager.Instance.GetPlayerData());
        }

        private void Instance_OnStateChangedEvent(float obj)
        {
            if(LevelManager.Instance.IsCompleted)
            {
                _slider.value = 1f;
                return;
            }

            var clampedValue = Mathf.Clamp(obj + 0.05f, 0f, 1f);

            _slider.value = 1f - clampedValue;
        }

        private void UpdateInfo(PlayerData data)
        {
            var level = data.CurrentLevel;

            _currentLevelText.text = level.ToString();
            _nextLevelText.text = (level+1).ToString();
        }
    }
}
