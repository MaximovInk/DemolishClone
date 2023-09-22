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

        private void Awake()
        {
            Init();
            LevelManager.Instance.OnLevelComplete += Instance_OnLevelComplete;
        }

        private void Instance_OnLevelComplete()
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

        }

        private void OnImpact()
        {
            LevelManager.Instance.UpdateBuildingState();
        }

        private bool _isDestroyed = false;


        public void ResetDestroyed()
        {
            _isDestroyed = false;
        }

        private void Update  ()
        {
            //if (_isDestroyed) return;

            var chunkRaycast = FracturedChunk.ChunkRaycast(
                transform.position,
                _rigidbody.velocity.normalized,
                out var hitInfo,
                 CannonManager.Instance.GetRaycastLength());


            Debug.DrawRay(transform.position, _rigidbody.velocity.normalized * CannonManager.Instance.GetRaycastLength());

            if (!chunkRaycast) return;

            if(_initData.IsExplode) 
                chunkRaycast.Impact(hitInfo.point, _initData.ExplodeForce, _initData.ExplodeRadius, true);
            _isDestroyed = true; 
            OnImpact();
        }

        private void OnCollisionEnter(Collision other)
        {
            OnImpact();
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