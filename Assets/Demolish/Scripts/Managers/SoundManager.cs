using UnityEngine;

namespace MaximovInk
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private AudioClip _backgroundMusic;
        [SerializeField] private AudioClip _buttonClickSound;
        [SerializeField] private AudioClip _winSound;
        [SerializeField] private AudioClip _rewardSound;

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
        }

        private void WeaponButton_OnClickEvent()
        {
            if (_buttonClickSound == null) return;

            _source.PlayOneShot(_buttonClickSound);
        }

        private void Instance_OnWeaponShootEvent(int ammoIndex)
        {
            var clip = CannonManager.Instance.GetAmmoByID(ammoIndex).ShootAudioClip;

            if (clip == null) return;

            _source.PlayOneShot(clip);
        }

        private void Instance_OnLevelComplete()
        {
            if (_winSound == null) return;

            _source.PlayOneShot(_winSound);
        }

    }
}