using System;
using UnityEngine;

namespace MaximovInk
{
    public class LevelManager : MonoBehaviourSingleton<LevelManager>
    {
        public event Action<float> OnStateChangedEvent;

        private float _buildingState = 0f;
        private FracturedObject _fracturedObject;
        private bool _isDirty = false;

        private void Awake()
        {
            _fracturedObject = FindObjectOfType<FracturedObject>();
        }

        public void UpdateBuildingState()
        {
            _isDirty = true;
        }

        private void LateUpdate()
        {
            if (_isDirty)
            {
                _isDirty = false;
                var total = _fracturedObject.ListFracturedChunks.Count;
                var detached = 0;
                for (int i = 0; i < total; i++)
                {
                    if (!_fracturedObject.ListFracturedChunks[i].IsDetachedChunk) continue;

                    detached++;
                }

                _buildingState = 1f - ((float)detached / total);
                OnStateChangedEvent?.Invoke(_buildingState);
            }
        }
    }
}