﻿using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MaximovInk
{
    public static class WeaponSerialization
    {
        private const string WEAPONS_KEY = "WEAPON_";

        public static void SetAmmoData(int index, int value)
        {
            if (index == 0) return;
            PlayerPrefs.SetInt(WEAPONS_KEY + index.ToString(), value);
        }

        public static void AddAmmoData(int index, int value)
        {
            if (index == 0) return;

            SetAmmoData(index, GetAmmoData(index)+value);
        }

        public static int GetAmmoData(int index)
        {
            return index == 0 
                ? int.MaxValue 
                : PlayerPrefs.GetInt(WEAPONS_KEY + index.ToString(), 0);
        }
    }


    [RequireComponent(typeof(Button),typeof(Image))]
    public class WeaponButton : MonoBehaviour
    {
        private static List<WeaponButton> _buttons = new();

        [SerializeField] private int _ammoID;
        [SerializeField] private TextMeshProUGUI _textInfo;
        [SerializeField] private Image _iconImage;
        
        private int _count = 0;
        private Button _button;
        private Image _image;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _image = GetComponent<Image>();

            Deserialize();

            _buttons.Add(this);

            CannonManager.Instance.OnWeaponShootEvent += Instance_OnWeaponShootEvent;

            _button.onClick.AddListener(() =>
            {
                DeselectAll();
                Select();
            });

            UpdateUI();
        }

        private void Instance_OnWeaponShootEvent(int ammoIndex)
        {
            if (ammoIndex == 0) return;

            if (ammoIndex == _ammoID)
            {
                _count--;
                Serialize();
                UpdateUI();
            }
        }
        private void Serialize()
        {
            WeaponSerialization.SetAmmoData(_ammoID, _count);
        }


        private void Deserialize()
        {
            _count = WeaponSerialization.GetAmmoData(_ammoID);
        }

        public void Add(int count)
        {
            _count += count;
            WeaponSerialization.SetAmmoData(_ammoID, _count);
            UpdateUI();
        }

        public static void ResetAll()
        {
            foreach (var btn in _buttons)
            {
                WeaponSerialization.SetAmmoData(btn._ammoID, 0);
                btn.Deserialize();
                btn.UpdateUI();
            }
        }

        public static bool IsCanShoot(int index)
        {
            return _buttons.Find(n => n._ammoID == index)._count != 0;
        }

        public static void UpdateAllButtons()
        {
            foreach (var btn in _buttons)
            {
                btn.Deserialize();
                btn.UpdateUI();
            }
        }

        public static void Select(int index)
        {
            DeselectAll();
            var btn = _buttons.Find(n => n._ammoID == index);
            btn.Select();
        }

        private static void DeselectAll()
        {
            foreach (var btn in _buttons)
            {
                btn.Deselect();
            }
        }

        private void Deselect()
        {
            _iconImage.color = Color.white;
            _image.sprite = UIManager.Instance.WeaponButtonDeselectSprite;
        }

        private void Select()
        {
            _iconImage.color = Color.black;
            

            _image.sprite = UIManager.Instance.WeaponButtonSelectSprite;
            CannonManager.Instance.CurrentAmmoIndex = _ammoID;
        }

        private void UpdateUI()
        {
            if (_ammoID == 0) return;

            _button.interactable = _count > 0;

            var ammoText = Mathf.Clamp(_count, 0, 999).ToString(); 

            _textInfo.text = ammoText;
        }

    }
}