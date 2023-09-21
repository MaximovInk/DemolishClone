using System;
using UnityEngine;

namespace MaximovInk
{
    public class LevelManager : MonoBehaviourSingleton<LevelManager>
    {
        public event Action<float> OnStateChangedEvent;
        public event Action OnLevelComplete;

        [SerializeField] private float levelCompleteThreshold = 0.1f;

        private float _buildingState = 0f;
        private FracturedObject _fracturedObject;
        private bool _isDirty = false;

        private FracturedObject[] _refObjects;

        [SerializeField] private Transform _refParent;

        private int _currentIndex = 0;

        private void Awake()
        {

            _refObjects = new FracturedObject[_refParent.childCount];

            for (var a = 0; a < _refParent.childCount; a++)
            {
                _refObjects[a] = _refParent.GetChild(a).GetComponent<FracturedObject>();
            }

            NextLevelInit();
            OnStateChangedEvent += LevelManager_OnStateChangedEvent;
        }

        public void UpdateBuildingState()
        {
            _isDirty = true;
        }

        private void LevelManager_OnStateChangedEvent(float obj)
        {
            if (obj < levelCompleteThreshold)
            {
                    LevelComplete();
                    NextLevelInit();
            }
        }

        private void NextLevelInit()
        {
            var objRef = _refObjects[_currentIndex];
            _fracturedObject = Instantiate(objRef);

            _fracturedObject.gameObject.SetActive(true);
            _fracturedObject.transform.SetPositionAndRotation(objRef.transform.position, objRef.transform.rotation);

            _currentIndex++;
            if (_currentIndex >= _refObjects.Length)
            {
                _currentIndex = 0;
            }
        }

        private void LevelComplete()
        {
            if (_fracturedObject != null)
            {
                var chunks = _fracturedObject.ListFracturedChunks;
                foreach (var t in chunks)
                {
                    Destroy(t.gameObject);
                }

                Destroy(_fracturedObject.gameObject);
            }
            OnLevelComplete?.Invoke();
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