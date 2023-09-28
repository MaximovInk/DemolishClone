using UnityEngine;

namespace MaximovInk
{
    public class LookAtVelocity : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private Projectile _projectile;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            if (!TryGetComponent(out _projectile)) return;

            _projectile.OnChunkImpact += Projectile_OnChunkImpact;
            _projectile.OnSetup += Projectile_OnSetup;
        }

        private void Projectile_OnSetup()
        {
            enabled = true;
        }

        private void Projectile_OnChunkImpact()
        {
            enabled = false;
        }

        private void Update()
        {
            transform.LookAt(transform.position + _rigidbody.velocity);
        }
    }
}
