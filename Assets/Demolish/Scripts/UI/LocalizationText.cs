
using TMPro;
using UnityEngine;

namespace MaximovInk
{
    [RequireComponent (typeof(TextMeshProUGUI))]
    public class LocalizationText : MonoBehaviour
    {
        [SerializeField] private string _key;
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            LocalizationManager.Instance.OnLanguageChange += Instance_OnLanguageChange;
            Instance_OnLanguageChange();
        }

        private void Instance_OnLanguageChange()
        {
            _text.text = LocalizationManager.Instance.Get(_key);
        }
    }
}
