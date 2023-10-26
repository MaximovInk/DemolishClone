using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MaximovInk
{
    

    public class HelicopterEvent : MonoBehaviourSingleton<HelicopterEvent>
    {
        private struct HeliEventData
        {
            public Vector3 BeginPos;
            public Vector3 EndPos;
            public Helicopter Instance;
            public float CurrentTime;
        }

        [SerializeField] private Helicopter _prefab;
        [SerializeField] private Transform _pointA;
        [SerializeField] private Transform _pointB;
        [SerializeField] private float _randomizeRadius = 1;
        [SerializeField] private float _eventDurationSec = 1f;
        [SerializeField] private AnimationCurve _moveCurve;

        [SerializeField] private int _minStars= 1;
        [SerializeField] private int _maxStars = 10;
        [SerializeField] private GameObject _explosionPrefab;


        private bool _isStarted;

        private HeliEventData _data;

        private void Awake()
        {
            LevelManager.Instance.OnLevelComplete += Instance_OnLevelComplete;
        }

        private void Instance_OnLevelComplete()
        {
            if (_isStarted)
            {
                EndEvent();
            }
        }

        public void StartEvent()
        {
            if (_isStarted) return;

            _isStarted = true;
            _data = new HeliEventData
            {
                BeginPos = _pointA.transform.position + new Vector3(
                    Random.Range(-_randomizeRadius/2, _randomizeRadius),
                    Random.Range(-_randomizeRadius/2, _randomizeRadius),
                    Random.Range(-_randomizeRadius/2, _randomizeRadius)
                ),
                EndPos = _pointB.transform.position + new Vector3(
                    Random.Range(-_randomizeRadius / 2, _randomizeRadius),
                    Random.Range(-_randomizeRadius / 2, _randomizeRadius),
                    Random.Range(-_randomizeRadius / 2, _randomizeRadius)
                ),
                Instance = Instantiate(_prefab, transform),
                CurrentTime = 0f
            };

            _data.Instance.transform.LookAt(_data.EndPos);
        }

        private void EndEvent()
        {
            _isStarted = false;
            Destroy(_data.Instance.gameObject);
        }

        private void Update()
        {
            if (!_isStarted) return;

            _data.CurrentTime += Time.deltaTime;

            var t = _data.CurrentTime / _eventDurationSec;

            _data.Instance.transform.position = Vector3.Lerp( _data.BeginPos,_data.EndPos,_moveCurve.Evaluate(t));

            if (_data.CurrentTime > _eventDurationSec)
            {
                EndEvent();
            }
        }

        public void HelicopterDamaged()
        {
            PlayerDataManager.Instance.AddStars(Random.Range(_minStars,_maxStars));
            var explosionInstance = Instantiate(_explosionPrefab);
            explosionInstance.transform.position = _data.Instance.transform.GetChild(0).position;
            Destroy(explosionInstance,10);


            EndEvent();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            if (_pointA != null)
            {
                Gizmos.DrawWireSphere(_pointA.transform.position, _randomizeRadius);
            }

            if (_pointB != null)
            {
                Gizmos.DrawWireSphere(_pointB.transform.position, _randomizeRadius);
            }

            if (_pointA != null && _pointB != null)
            {
                Gizmos.DrawLine(_pointA.transform.position, _pointB.transform.position);
            }
        }
    }
}