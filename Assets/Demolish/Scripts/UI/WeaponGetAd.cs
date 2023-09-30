using System;
using UnityEngine;
using UnityEngine.UI;
using GamePush;

namespace MaximovInk
{
    [RequireComponent(typeof(Button))]
    public class WeaponGetAd : MonoBehaviour
    {
        [SerializeField] private int _ammoID = 0;
        [SerializeField] private int _ammoAmount = 1;

        private void Awake()
        {
            var button = GetComponent<Button>();

            button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            GP_Ads.ShowFullscreen(null, isFailed =>
            {
                if (!isFailed)
                {
                    WeaponSerialization.AddAmmoData(_ammoID, _ammoAmount);
                }

            });
        }
    }
}
