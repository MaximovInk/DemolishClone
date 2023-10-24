using System;
using UnityEngine;

namespace MaximovInk
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviourSingleton<SoundManager>
    {
        [SerializeField] private AudioClip _backgroundMusic;
        [SerializeField] private AudioClip _buttonClickSound;
        [SerializeField] private AudioClip _winSound;
        [SerializeField] private AudioClip _rewardSound;

        public bool IsSoundActive
        {
            get => _isSoundActive;
            set
            {
                var temp = _isSoundActive;

                _isSoundActive = value;

                if (temp != value)
                {
                    OnSoundStateChanged?.Invoke(value);
                }
            }
        }

        private bool _isSoundActive = true;

        public event Action<bool> OnSoundStateChanged;


        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();

            _source.clip = _backgroundMusic;
            if (_buttonClickSound != null)
                _source.Play();

            LevelManager.Instance.OnLevelComplete += Instance_OnLevelComplete;
            CannonManager.Instance.OnWeaponShootEvent += Instance_OnWeaponShootEvent;
            WeaponButton.OnClickEvent += WeaponButton_OnClickEvent;
            UIManager.Instance.LevelCompleteScreen.OnRewardEvent += RewardScreen_OnRewardEvent;
        }

        private void RewardScreen_OnRewardEvent()
        {
            if (_rewardSound == null) return;

            PlayOneShot(_rewardSound);
        }

        private void WeaponButton_OnClickEvent()
        {
            if (_buttonClickSound == null) return;

            PlayOneShot(_buttonClickSound);
        }

        private void Instance_OnWeaponShootEvent(int ammoIndex)
        {
            var clip = CannonManager.Instance.GetAmmoByID(ammoIndex).ShootAudioClip;

            if (clip == null) return;

            PlayOneShot(clip);
        }

        private void Instance_OnLevelComplete()
        {
            if (_winSound == null) return;

            PlayOneShot(_winSound);
        }

        public void PlayOneShot(AudioClip clip)
        {
            if (!IsSoundActive) return;
            
            _source.PlayOneShot(clip);
        }

    }
}