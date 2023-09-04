using TMPro;
using UnityEngine;

namespace MaximovInk
{
    public class UIManager : MonoBehaviourSingleton<UIManager>
    {
        [SerializeField] private TextMeshProUGUI buildingConditionText;

        public void SetBuildingCondition(BuildingState state)
        {
            buildingConditionText.text = $"Building Condition: {(int)(state.total * 100)}\n";

            foreach (var layerState in state.layers)
            {
                buildingConditionText.text += $"LayerState: {(int)(layerState * 100)}\n";
            }
        }

    }
}