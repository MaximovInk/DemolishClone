using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MaximovInk
{
    public class DebugUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _buildingStateTextInfo;
        [SerializeField] private Button _resetDataButton;
        [SerializeField] private Button _startHeliEventButton;
        [SerializeField] private Button _addAllWeapons1;

        private void Awake()
        {
            LevelManager.Instance.OnStateChangedEvent += Instance_OnStateChangedEvent;
            _resetDataButton.onClick.AddListener(() =>
            {
                PlayerDataManager.Instance.ClearAndApply();
                WeaponButton.ResetAll();
            });
            _startHeliEventButton.onClick.AddListener(() => { HelicopterEvent.Instance.StartEvent();});
            _addAllWeapons1.onClick.AddListener(() =>
            {
                WeaponSerialization.AddAmmoData(1, 1);
                WeaponSerialization.AddAmmoData(2, 1);
                WeaponSerialization.AddAmmoData(3, 1);
                WeaponSerialization.AddAmmoData(4, 1);
                WeaponButton.UpdateAllButtons();
            });
        }

        private void Instance_OnStateChangedEvent(float obj)
        {
            var clampedValue = Mathf.Clamp(obj + 0.05f, 0f, 1f);
            var intVal = (int)(clampedValue * 100);
            _buildingStateTextInfo.text = $"State:{intVal}";
        }
    }
}
