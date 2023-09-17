using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MaximovInk
{
    public class DebugUI : MonoBehaviour
    {
        [SerializeField] private Slider _buildingStateSlider;
        [SerializeField] private TextMeshProUGUI _buildingStateTextInfo;

        private void Awake()
        {
            LevelManager.Instance.OnStateChangedEvent += Instance_OnStateChangedEvent;
        }

        private void Instance_OnStateChangedEvent(float obj)
        {
            var clampedValue = Mathf.Clamp(obj + 0.05f, 0f, 1f);

            _buildingStateSlider.value = clampedValue;

            var intVal = (int)(clampedValue * 100);

            _buildingStateTextInfo.text = $"State:{intVal}";
        }
    }
}
