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

        private void Awake()
        {
            LevelManager.Instance.OnStateChangedEvent += Instance_OnStateChangedEvent;
            _resetDataButton.onClick.AddListener(() => { PlayerDataManager.Instance.ClearAndApply();});
            _startHeliEventButton.onClick.AddListener(() => { HelicopterEvent.Instance.StartEvent();});
        }

        private void Instance_OnStateChangedEvent(float obj)
        {
            var clampedValue = Mathf.Clamp(obj + 0.05f, 0f, 1f);
            var intVal = (int)(clampedValue * 100);
            _buildingStateTextInfo.text = $"State:{intVal}";
        }
    }
}
