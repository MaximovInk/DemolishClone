using TMPro;
using UnityEngine;

namespace MaximovInk
{
    public class StarsStat : MonoBehaviour
    {
        private TextMeshProUGUI _textDisplay;

        private void Awake()
        {
            _textDisplay = GetComponent<TextMeshProUGUI>();

            PlayerDataManager.Instance.OnLoadEvent += UpdateUI;
            PlayerDataManager.Instance.OnSaveEvent += UpdateUI;
        }

        private void UpdateUI(PlayerData obj)
        {
            _textDisplay.text = obj.Stars.ToString();
        }
    }
}