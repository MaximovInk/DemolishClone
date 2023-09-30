using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MaximovInk
{
    [System.Serializable]
    public struct ButtonStruct
    {
        public Sprite NotActive;
        public Sprite Active;
    }

    public class SettingsManager : MonoBehaviourSingleton<SettingsManager>
    {
        private const string SETTINGS_KEY = "SETTINGS_";

        [SerializeField] private Button _soundButton;
        [SerializeField] private Button _restartButton;

        [SerializeField] private ButtonStruct _soundButtonStruct;

        private void Awake()
        {
            SoundManager.Instance.OnSoundStateChanged += Instance_OnSoundStateChanged;

            _restartButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            });

            _soundButton.onClick.AddListener(() =>
            {
                var soundActive = SoundManager.Instance.IsSoundActive;
                soundActive = !soundActive;
                SoundManager.Instance.IsSoundActive = soundActive;
            });

            SoundManager.Instance.IsSoundActive = PlayerPrefs.GetInt(SETTINGS_KEY + "SOUND", 0) == 1;
        }

        private void Instance_OnSoundStateChanged(bool obj)
        {
            PlayerPrefs.SetInt(SETTINGS_KEY + "SOUND", obj?1:0);
            _soundButton.GetComponent<Image>().sprite = obj ? _soundButtonStruct.Active : _soundButtonStruct.NotActive;
        }
    }
}