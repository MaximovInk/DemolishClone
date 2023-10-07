using System;
using UnityEngine;

namespace MaximovInk
{
    public class Projectile : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private AmmoData _initData;
        private int _releaseIndex = 0;

        public bool IsReleased { get; set; }

        private bool _isSound = false;

        public event Action OnChunkImpact;
        public event Action OnSetup;

        private Vector3 _target;

        private void Awake()
        {
            Init();
            LevelManager.Instance.OnNextLevelInit += InstanceOnNextLevelInit;
        }

        private void InstanceOnNextLevelInit()
        {
            ProjectilePool.Instance.ReleaseProjectile(_releaseIndex, this);
        }

        public void SetVelocity(Vector3 velocity)
        {
            if (_rigidbody == null) Init();

            _rigidbody.velocity = velocity * _initData.ForceMultiplier;
        }

        private void Init()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void SetupProjectile(AmmoData data)
        {
            transform.localScale = data.Scale;
            _initData = data;

            this.Invoke(() =>
            {
                ProjectilePool.Instance.ReleaseProjectile(_releaseIndex, this);
            }, CannonManager.Instance.GetProjectileHideDelay());

            OnSetup?.Invoke();
        }

        private void OnImpact()
        {
            LevelManager.Instance.UpdateBuildingState();
            OnChunkImpact?.Invoke();

            if (IsReleased) return;

            if ( _initData.CustomExplosionPrefab)
            {
                var explosionPrefab = Instantiate(_initData.CustomExplosionPrefab, CannonManager.Instance.transform);
                explosionPrefab.transform.position = transform.position;
                Destroy(explosionPrefab, 3f);
            }

            if (_initData.IsExplode && _initData.DestroyOnExplode)
            {
                ProjectilePool.Instance.ReleaseProjectile(_releaseIndex, this);
            }

            if (_initData.HitAudioClip != null && !_isSound)
            {
                SoundManager.Instance.PlayOneShot(_initData.HitAudioClip);
                _isSound = true;
            }
        }

        public void ResetSound()
        {
            _isSound = false;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (IsReleased) return;

            var colliders =  Physics.OverlapSphere(transform.position, _initData.ExplodeRadius,
                CannonManager.Instance.ExplosionLayerMask);


            foreach (var coll in colliders)
            {
                if (!coll.gameObject.TryGetComponent<FracturedChunk>(out var chunk))
                    chunk = coll.gameObject.GetComponentInParent<FracturedChunk>();

                if (chunk && _initData.IsExplode)
                {
                    chunk.Impact(transform.position, _initData.ExplodeForce, _initData.ExplodeRadius, true);
                }

                if (chunk)
                    OnImpact();
            }


            {
                if (!other.gameObject.TryGetComponent<FracturedChunk>(out var chunk))
                    chunk = other.gameObject.GetComponentInParent<FracturedChunk>();

                if (chunk && _initData.IsExplode)
                {
                    chunk.Impact(transform.position, _initData.ExplodeForce, _initData.ExplodeRadius, true);
                }

                if (chunk)
                    OnImpact();
            }
        }

        private void OnDrawGizmos()
        {
            if (_initData == null) return;

            if (!_initData.IsExplode) return;

            var position = transform.position;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, _initData.ExplodeRadius);

        }

        public void SetReleaseIndex(int index)
        {
            _releaseIndex = index;
        }
    }

}