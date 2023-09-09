using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;

namespace MaximovInk
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float radius = 1.74f;
        [SerializeField] private float explosionForce = 1f;

        private void Start()
        {
            GetComponent<Rigidbody>().solverIterations = 255;
        }

        private void OnCollisionEnter(Collision other)
        {
            var obj = other.transform.GetComponent<FracturedChunk>();

            if(obj != null){

                if (obj.IsDetachedChunk)
                    return;

                obj.FracturedObjectSource.Explode(transform.position, explosionForce, radius,false,false,false,true);

                var chunks =obj.FracturedObjectSource.ListFracturedChunks;

                for (int i = 0; i < chunks.Count; i++)
                {
                    if (chunks[i].IsDetachedChunk)
                    {
                        chunks[i].gameObject.layer = LayerMask.NameToLayer("BuildingDestructed");
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            var position = transform.position;

            Gizmos.DrawWireSphere(position, radius);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(position, radius);

        }

    }

}