using UnityEngine;

namespace MaximovInk
{
    public class Projectile : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        private AmmoData _initData;

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
        }

        //Optimize
        private void UpdateChunksLayer(FracturedObject fracturedObject)
        {
            var chunks = fracturedObject.ListFracturedChunks;

            for (int i = 0; i < chunks.Count; i++)
            {
                if (chunks[i].IsDetachedChunk)
                {
                    chunks[i].gameObject.layer = LayerMask.NameToLayer("BuildingDestructed");
                }
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.transform.TryGetComponent<FracturedChunk>(out var obj)) return;

            if (obj.IsDetachedChunk)
                return;

            if (_initData.IsExplode)
            {
                obj.FracturedObjectSource.Explode(
                    transform.position, 
                    _initData.ExplodeForce,
                    _initData.ExplodeRadius,
                    false,
                    false,
                    false,
                    true
                );
            }

            UpdateChunksLayer(obj.FracturedObjectSource);

            LevelManager.Instance.UpdateBuildingState();
        }

        private void OnDrawGizmos()
        {
            if (!_initData.IsExplode) return;

            var position = transform.position;

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, _initData.ExplodeRadius);

        }

    }

}