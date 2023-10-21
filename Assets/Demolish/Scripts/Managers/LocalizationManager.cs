using GamePush;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MaximovInk
{
    [System.Serializable]
    public struct LocalizationWord
    {
        public string Key;
        public string RusValue;
        public string EngValue;
        public string TurValue;
    }

    public enum LanguageType
    {
        Russian,
        English,
        Turkish
    }

    public class LocalizationManager : MonoBehaviourSingleton<LocalizationManager>
    {
        [SerializeField] private LocalizationWord[] _localizationWords;

        private Dictionary<string, LocalizationWord> _localizationDictionary;

        private LanguageType _languageType;

        public event Action OnLanguageChange;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(this);

            GP_Language.OnChangeLanguage += GP_Language_OnChangeLanguage;

            CacheWords();

            DetectLanguage(GP_Language.Current());
        }

        private void CacheWords()
        {
            _localizationDictionary = new Dictionary<string, LocalizationWord>();
            for (int i = 0; i < _localizationWords.Length; i++)
            {
                _localizationDictionary.Add(_localizationWords[i].Key, _localizationWords[i]);
            }
        }

        private void OnDestroy()
        {
            GP_Language.OnChangeLanguage -= GP_Language_OnChangeLanguage;
        }

        private void GP_Language_OnChangeLanguage(Language arg0)
        {
            DetectLanguage(arg0);
        }

        private void DetectLanguage(Language source)
        {
            switch (source)
            {
                case Language.English:
                    _languageType = LanguageType.English;
                    break;
                case Language.Russian:
                    _languageType = LanguageType.Russian;
                    break;
                case Language.Turkish:
                    _languageType = LanguageType.Turkish;
                    break;
                default:
                    _languageType = LanguageType.English;
                    break;
            }

            OnLanguageChange?.Invoke();
        }

        public string Get(string key, object value = null)
        {
            if (!_localizationDictionary.TryGetValue(key, out var wordStruct))
                return key;

            var word = ParseToLanguage(wordStruct);

            word = word.Replace("{n}", "\n");

            if (value == null)
                return word;

            return word.Replace("{x}", value.ToString());
        }

        private string ParseToLanguage(LocalizationWord word)
        {
            switch (_languageType)
            {
                case LanguageType.Russian:
                    return word.RusValue;
                case LanguageType.English:
                    return word.EngValue;
                case LanguageType.Turkish:
                    return word.TurValue;
            }

            return word.EngValue;
        }
    }
}