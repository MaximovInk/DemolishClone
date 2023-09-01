using UnityEngine;

namespace MaximovInk
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float radius = 1.74f;
        [SerializeField] private LayerMask destroyLayer;
        [SerializeField] private float explosionRadiusMultiplier = 2f;
        [SerializeField] private float explosionForce = 1f;
        [SerializeField] private float destroyedForceMultiplier = 2f;

        private float GetExplosionRadius() => radius * explosionRadiusMultiplier;

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("BuildingDestructed"))
            {
                var rb = other.gameObject.GetComponent<Rigidbody>();
                ApplyExplosionForce(rb, destroyedForceMultiplier);
            }

            if (!other.gameObject.CompareTag("Building")) return;

            StructureDestroy(other.gameObject);

            CastStructures(other.gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            var position = transform.position;

            Gizmos.DrawWireSphere(position, radius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, GetExplosionRadius());

        }

        private void CastStructures(GameObject ignoreObject)
        {
            // Opt: Use NonAlloc version
            var hit = Physics.OverlapSphere(transform.position, radius, destroyLayer);

            foreach (var structure in hit)
            {
                if (structure.gameObject == ignoreObject) continue;

                StructureDestroy(structure.gameObject);
            }
        }

        private void StructureDestroy(GameObject other)
        {
            other.layer = LayerMask.NameToLayer("BuildingDestructed");
            other.tag = "BuildingDestructed";
            var rb = other.AddComponent<Rigidbody>();
            ApplyExplosionForce(rb);
        }

        private void ApplyExplosionForce(Rigidbody rigidbody, float forceMultiplier = 1f)
        {
            rigidbody.AddExplosionForce(
                explosionForce,
                transform.position,
                GetExplosionRadius(),
                0f,
                ForceMode.Impulse
                );
        }
    }

}