using System;
using UnityEngine;

namespace MaximovInk
{
    public class AnimationMove : MonoBehaviour
    {
        private Transform _target;
        private float _time;

        private Vector3 _initPos;

        private float _timeCounter;

        public event Action OnKill;

        private void Awake()
        {
            _initPos = transform.position;
        }

        public void Initialize(Transform target, float time)
        {
            _target = target;
            _time = time;
        }

        private void OnDestroy()
        {
            OnKill?.Invoke();
        }

        private void Move()
        {
            transform.position = Vector3.Lerp(_initPos, _target.transform.position, _timeCounter/_time);
        }

        private void Update()
        {
            if (_target == null) return;

            _timeCounter += Time.deltaTime;

            if (_timeCounter <= _time)
                Move();
            else
                Destroy(gameObject);
        }
    }
}
