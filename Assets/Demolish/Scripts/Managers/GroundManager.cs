﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MaximovInk
{
    public class GroundManager : MonoBehaviourSingleton<GroundManager>
    {
        [SerializeField] private Material[] _materials;

        [SerializeField] private int _currentMaterialIndex = 0;

        [SerializeField] private GameObject _ground;

        private MeshRenderer _groundRenderer;

        private void Awake()
        {
            _groundRenderer = _ground.GetComponent<MeshRenderer>();

            ApplyCurrentMaterial();
        }

        private void ApplyCurrentMaterial()
        {
            if (_materials.Length == 0) return;

            _currentMaterialIndex = Mathf.Clamp(_currentMaterialIndex, 0, _materials.Length - 1);

            _groundRenderer.sharedMaterial = _materials[_currentMaterialIndex];
        }
    }
}
