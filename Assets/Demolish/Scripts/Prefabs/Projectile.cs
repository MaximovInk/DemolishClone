using System;
using UnityEngine;

namespace MaximovInk
{
    public class Projectile : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private AmmoData _initData;
        private int _releaseIndex = 0;

        private void Awake()
        {
            Init();
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

        }

        private void OnImpact()
        {
            LevelManager.Instance.UpdateBuildingState();
        }

        private bool _isDestroyed = false;

        private void Update()
        {
            if (_isDestroyed) return;

            var chunkRaycast = FracturedChunk.ChunkRaycast(
                transform.position,
                _rigidbody.velocity.normalized,
                out var hitInfo,
                CannonManager.Instance.GetRaycastLength());

            if (!chunkRaycast) return;

            chunkRaycast.Impact(hitInfo.point, _initData.ExplodeForce, _initData.ExplodeRadius, true);
            _isDestroyed = true; 
            OnImpact();
        }

        private void OnDrawGizmos()
        {
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