using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace MaximovInk
{
    public class BuildingPart : MonoBehaviour
    {

        private int layerIndex = -1;

        public event Action<int> OnDestroyThis;

        public void SetLayerIndex(int layerIndex)
        {
            this.layerIndex = layerIndex;
        }

        public bool IsInited() => layerIndex != -1;

        public void DestroyBuildingPart(bool invokeAction = true)
        {
            gameObject.layer = LayerMask.NameToLayer("BuildingDestructed");
            gameObject.tag = "BuildingDestructed";

            if (!gameObject.TryGetComponent<Rigidbody>(out var rb))
                gameObject.AddComponent<Rigidbody>();

            if(invokeAction)
                OnDestroyThis?.Invoke(layerIndex);
        }

        public void AddExplosion(float force, float radius)
        {
            var buildingPosition = transform.parent.position;

            var explosionPosition = transform.position;
            explosionPosition.x = buildingPosition.x;
            explosionPosition.z = buildingPosition.z;

            
            if (!gameObject.TryGetComponent<Rigidbody>(out var rb))
                rb = gameObject.AddComponent<Rigidbody>();

            rb.AddExplosionForce(
                force,
                explosionPosition,
                radius,
                0f,
                ForceMode.Impulse
            );
        }

        
    }
}
