using UnityEditor.Experimental.GraphView;
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
                var part = other.gameObject.GetComponent<BuildingPart>();
                part.AddExplosion(explosionForce, GetExplosionRadius());
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
            var part = other.GetComponent<BuildingPart>();
            part.DestroyBuildingPart();
            part.AddExplosion(explosionForce, GetExplosionRadius());
        }
    }

}