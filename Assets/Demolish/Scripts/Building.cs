using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.VolumeComponent;

namespace MaximovInk
{

    [System.Serializable]
    public class Layer
    {
        public List<Transform> transforms;
    }

    public class Building : MonoBehaviour
    {
        [SerializeField] public Vector3 max;
        [SerializeField] public Vector3 min;

        public int HeightCheckCount = 5;
        public float HeightCheckSize = 1f;

        public LayerMask LayersCheckMask;

        public Layer[] layers;

        private bool layersIsDirty;

        private void Start()
        {
            layers = new Layer[HeightCheckCount];

            var position = transform.position;
            var size = max - min;
            var yMin = position.y + min.y;
            var yMax = position.y + max.y;


            var stepInit =
                position
                + min
                + new Vector3(size.x, 0, size.z) / 2
               ;
            var stepSize = new Vector3(size.x, HeightCheckSize, size.z)/2f;

            for (var i = 0; i < layers.Length; i++)
            {
                var stepCenter =
                    stepInit + new Vector3(0, Mathf.Lerp(yMin, yMax, (float)i / (HeightCheckCount - 1)), 0);

                var colliders = Physics.OverlapBox(stepCenter, stepSize, Quaternion.identity, LayersCheckMask);

                layers[i] = new Layer
                {
                    transforms = new List<Transform>(colliders.Length)
                };

                foreach (var coll in colliders)
                {
                    var part = coll.GetComponent<BuildingPart>();
                    if(part.IsInited()) continue;

                    part.name = $"init_cell[{i}]";

                    layers[i].transforms.Add(coll.transform);
                    part.SetLayerIndex(i);
                    part.OnDestroyThis += index =>
                    {
                        Part_OnDestroyThis(index, part.transform);
                    };
                }
            }
        }

        private void Update()
        {
            if (layersIsDirty)
            {
                layersIsDirty = false;

                var destroyBelow = false;
                var maxIndex = layers.Length-1;

                for (int i = 0; i < layers.Length; i++)
                {
                    if (destroyBelow)
                    {
                        DestroyLayer(layers[i], false);
                    }

                    if (layers[i].transforms.Count == 0)
                    {
                        destroyBelow = true;
                        maxIndex = i;
                    }
                }
            }
        }

        private static void DestroyLayer(Layer layer, bool invoke = true)
        {
            var buildingParts = layer.transforms.Select(t => t.GetComponent<BuildingPart>());

            foreach (var part in buildingParts)
            {
                part.DestroyBuildingPart(invoke);
            }

            layer.transforms.Clear();
        }

        private void Part_OnDestroyThis(int index, Transform part)
        {
            layers[index].transforms.Remove(part);
            if (layers[index].transforms.Count < 3)
                DestroyLayer(layers[index], false);

            layersIsDirty = true;
        }

        private void OnDrawGizmos()
        {
            var position = transform.position;

            Gizmos.color = Color.blue;

            var size = max - min;
            var center = position + min + size / 2;

            Gizmos.DrawWireCube(center,size);

            if (HeightCheckCount <= 0) return;

            Gizmos.color = Color.white;
            var yMin = position.y + min.y;
            var yMax = position.y + max.y;

            var stepInit =
                    position
                    + min
                    + new Vector3(size.x, 0, size.z) / 2
                ;

            var stepSize = new Vector3(size.x, HeightCheckSize, size.z);

            for (var i = 0; i < HeightCheckCount; i++)
            {
                var stepCenter =
                    stepInit + new Vector3(0, Mathf.Lerp(yMin, yMax, (float)i / (HeightCheckCount - 1)), 0);

                Gizmos.DrawWireCube(stepCenter, stepSize);
            }
        }
    }
}