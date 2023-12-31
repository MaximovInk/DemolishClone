using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace MaximovInk
{
    public class LevelManager : MonoBehaviourSingleton<LevelManager>
    {
        public event Action<float> OnStateChangedEvent;
        public event Action OnNextLevelInit;
        public event Action OnLevelComplete;

        [FormerlySerializedAs("levelCompleteThreshold")] [SerializeField]
        private float _levelCompleteThreshold = 0.1f;
        [SerializeField] private GameObject ExplosivePrefab;

        public float BuildingState => _buildingState;
        private float _buildingState = 0f;
        private FracturedObject _fracturedObject;
        private bool _isDirty;

        //private int CurrentRefIndex => ((Mathf.Clamp(PlayerDataManager.Instance.GetLevel() - 1, 0, int.MaxValue)) % _fracturedObjectsLenght) + 1;

        private int CurrentRefIndex => ((Mathf.Clamp(PlayerDataManager.Instance.GetLevel(), 0, int.MaxValue)) % _fracturedObjects.Length);

        private const int MAX_EXPLOSIONS = 5;

        private readonly List<GameObject> _explosionsList = new();

        private int _fracturedObjectsLenght;

        private const float STATE_OFFSET = 0.5f;

        public bool IsCompleted { get; private set; }

        public bool CanShoot => !IsCompleted && !LayoutScreens.Instance.HasActiveScreens();

        [SerializeField] private FracturedObject[] _fracturedObjects;

        public void UpdateBuildingState()
        {
            _isDirty = true;
        }

        private void Awake()
        {
            _fracturedObjectsLenght = SceneManager.sceneCountInBuildSettings - 1;
             OnStateChangedEvent += LevelManager_OnStateChangedEvent;
            CannonManager.Instance.OnWeaponShootEvent += _ => { _shootCount++; };
        }

        public void LoadNewLevel()
        {
            NextLevelInit();
        }

        private void NextLevelInit()
        {
            _shootCount = 0;
            DestroyCurrentBuilding();

            IsCompleted = false;

            if (_fracturedObject != null)
            {
                _fracturedObject.OnChunkDetachEvent -= _fracturedObject_OnChunkDetachEvent;
               Destroy(_fracturedObject.gameObject);
               _fracturedObject = null;
            }

            _fracturedObject = Instantiate(_fracturedObjects[CurrentRefIndex]);

            _buildingState = 1f;
            OnStateChangedEvent?.Invoke(_buildingState);
        }

        public void NextLevel()
        {
            OnNextLevelInit?.Invoke();
        }

        private int _shootCount;

        private int CalculateStars(int shootCount)
        {
            var stars = 1;

            if (shootCount < 20)
            {
                stars = 2;
            }
            if (shootCount < 10)
            {
                stars = 3;
            }

            return stars;
        }

        private void LevelManager_OnStateChangedEvent(float obj)
        {
            if (obj < _levelCompleteThreshold && !IsCompleted)
            {
                IsCompleted = true;

                OnStateChangedEvent?.Invoke(0f);

                OnLevelComplete?.Invoke();

                this.Invoke(() =>
                {
                    if (PlayerDataManager.Instance.IsReward)
                    {
                        UIManager.Instance.RewardScreen.GenerateChestRewards();
                        UIManager.Instance.Screens.ShowScreen("Reward");
                    }

                    UIManager.Instance.LevelCompleteScreen.Stars = CalculateStars(_shootCount);
                    UIManager.Instance.Screens.ShowScreen("LevelComplete");

                }, 2f);
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

            if (ExplosivePrefab == null) return;

            var explosionInstance = Instantiate(ExplosivePrefab, transform, true);
            explosionInstance.transform.SetPositionAndRotation(obj.transform.position, obj.transform.rotation);

            _explosionsList.Add(explosionInstance);

            Destroy(explosionInstance, 5f);
        }

        private void LateUpdate()
        {
            if (_fracturedObject == null) return; 

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

                _buildingState = ((float)detached / total);
                _buildingState *= (1f + STATE_OFFSET);
                _buildingState = 1f - _buildingState;

                OnStateChangedEvent?.Invoke(_buildingState);
            }
        }
    }
}