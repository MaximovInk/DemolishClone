using System;
using System.Collections.Generic;
using UnityEngine;

namespace MaximovInk
{
    public class LevelManager : MonoBehaviourSingleton<LevelManager>
    {
        public event Action<float> OnStateChangedEvent;
        public event Action OnNextLevelInit;
        public event Action OnLevelComplete;

        [SerializeField] private float levelCompleteThreshold = 0.1f;
        [SerializeField] private Transform _refParent;
        [SerializeField] private GameObject ExplosivePrefab;

        private float _buildingState = 0f;
        private FracturedObject _fracturedObject;
        private bool _isDirty = false;

        private FracturedObject[] _refObjects;

        private int CurrentRefIndex => (Mathf.Clamp(PlayerDataManager.Instance.GetLevel() - 1,0, int.MaxValue)) % _refObjects.Length;

        private const int MAX_EXPLOSIONS = 5;

        private readonly List<GameObject> _explosionsList = new();

        private const float STATE_OFFSET = 0.5f;

        public bool IsCompleted { get; private set; } = false;

        public void UpdateBuildingState()
        {
            _isDirty = true;
        }

        private void Awake()
        {
            _refObjects = new FracturedObject[_refParent.childCount];

            for (var a = 0; a < _refParent.childCount; a++)
            {
                _refObjects[a] = _refParent.GetChild(a).GetComponent<FracturedObject>();
            }

            OnStateChangedEvent += LevelManager_OnStateChangedEvent;

            PlayerDataManager.Instance.OnLoadEvent += Instance_OnLoadEvent;
        }

        private void Instance_OnLoadEvent(PlayerData obj)
        {
            
            NextLevelInit();

        }

        private void NextLevelInit()
        {
            DestroyCurrentBuilding();

            IsCompleted = false;

            var objRef = _refObjects[CurrentRefIndex];
            _fracturedObject = Instantiate(objRef);

            _fracturedObject.gameObject.SetActive(true);
            _fracturedObject.transform.SetPositionAndRotation(objRef.transform.position, objRef.transform.rotation);

            _fracturedObject.OnChunkDetachEvent += _fracturedObject_OnChunkDetachEvent;

        }

        private void LevelManager_OnStateChangedEvent(float obj)
        {
            if (obj < levelCompleteThreshold && !IsCompleted)
            {
                IsCompleted = true;
                OnLevelComplete?.Invoke();
                this.Invoke(() =>
                {
                    OnNextLevelInit?.Invoke();
                    NextLevelInit();
                }, 2f);

                OnStateChangedEvent?.Invoke(0f);
            }
        }

        private void DestroyCurrentBuilding()
        {
            if (_fracturedObject == null) return;

            var chunks = _fracturedObject.ListFracturedChunks;
            foreach (var t in chunks)
            {
                Destroy(t.gameObject);
            }

            Destroy(_fracturedObject.gameObject);
        }

        private void _fracturedObject_OnChunkDetachEvent(FracturedChunk obj)
        {
            _explosionsList.RemoveAll(item => item == null);

            while (_explosionsList.Count > MAX_EXPLOSIONS)
            {
                Destroy(_explosionsList[0]);
                _explosionsList.RemoveAt(0);
            }

            var explosionInstance = Instantiate(ExplosivePrefab, transform, true);
            explosionInstance.transform.SetPositionAndRotation(obj.transform.position, obj.transform.rotation);

            _explosionsList.Add(explosionInstance);

            Destroy(explosionInstance, 5f);
        }

        private void LateUpdate()
        {
            if (IsCompleted) return;

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

                //_buildingState = 1f - ((float)detached / total);
                //_buildingState = Mathf.Clamp01(_buildingState / (STATE_OFFSET+1f));
                _buildingState = ((float)detached / total);
                _buildingState *= (1f + STATE_OFFSET);
                _buildingState = 1f - _buildingState;

            
                OnStateChangedEvent?.Invoke(_buildingState);
            }
        }
    }
}