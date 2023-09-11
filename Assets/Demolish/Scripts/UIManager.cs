using TMPro;
using UnityEngine;

namespace MaximovInk
{
    public class UIManager : MonoBehaviourSingleton<UIManager>
    {
        [SerializeField] private TextMeshProUGUI buildingConditionText;

        public void SetBuildingCondition(float state)
        {
            buildingConditionText.text = $"Building Condition: {(int)(state * 100)}\n";
        }

    }
}