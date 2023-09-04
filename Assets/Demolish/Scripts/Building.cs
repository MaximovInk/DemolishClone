using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MaximovInk
{
    [System.Serializable]
    public class Layer
    {
        public List<Transform> transforms;
        public int count;
        public int initCount;
    }

    [System.Serializable]
    public struct BuildingState
    {
        public float total;
        public float[] layers;
    }

    public class Building : MonoBehaviour
    {
        [SerializeField] private Vector3 max;
        [SerializeField] private Vector3 min;

        [SerializeField] private int heightCheckCount = 5;
        [SerializeField] private float heightCheckSize = 1.8f;

        [SerializeField] private LayerMask layersCheckMask;

        [SerializeField] private float thresholdDestroy = 0.1f;

        private BuildingState _state = new();

        private Layer[] _layers;

        private bool _layersIsDirty;

        private void Start()
        {
            _layers = new Layer[heightCheckCount];

            var position = transform.position;
            var size = max - min;
            var yMin = position.y + min.y;
            var yMax = position.y + max.y;


            var stepInit =
                position
                + min
                + new Vector3(size.x, 0, size.z) / 2
               ;
            var stepSize = new Vector3(size.x, heightCheckSize, size.z)/2f;

            for (var i = 0; i < _layers.Length; i++)
            {
                var stepCenter =
                    stepInit + new Vector3(0, Mathf.Lerp(yMin, yMax, (float)i / (heightCheckCount - 1)), 0);

                var colliders = Physics.OverlapBox(stepCenter, stepSize, Quaternion.identity, layersCheckMask);

                _layers[i] = new Layer
                {
                    transforms = new List<Transform>(colliders.Length),
                    count = colliders.Length,
                   initCount = colliders.Length
                };

                foreach (var coll in colliders)
                {
                    var part = coll.GetComponent<BuildingPart>();
                    if(part.IsInited()) continue;

                    part.name = $"init_cell[{i}]";

                    _layers[i].transforms.Add(coll.transform);
                    part.SetLayerIndex(i);
                    part.OnDestroyThis += index =>
                    {
                        Part_OnDestroyThis(index);
                    };
                }
            }

            UpdateBuildingCondition();
        }

        private void Update()
        {
            if (!_layersIsDirty) return;

            _layersIsDirty = false;

            var destroyBelow = false;

            foreach (var layer in _layers)
            {
                if (destroyBelow)
                    DestroyLayer(layer, false);

                //if (layer.transforms.Count != 0) continue;
                if (layer.count > 0) continue;

                destroyBelow = true;
            }

            UpdateBuildingCondition();
        }

        private static void DestroyLayer(Layer layer, bool invoke = true)
        {
            var buildingParts = layer.transforms.Select(t => t.GetComponent<BuildingPart>());

            foreach (var part in buildingParts)
            {
                if(part.IsDestroyed()) continue;

                part.DestroyBuildingPart(invoke);
            }

            layer.transforms.Clear();
            layer.count = 0;
        }

        private void Part_OnDestroyThis(int index)
        {
            _layers[index].count--;

            if (_state.layers[index] < thresholdDestroy)
                DestroyLayer(_layers[index], false);

            _layersIsDirty = true;
        }

        private void UpdateBuildingCondition()
        {
            var initParts = 0;
            var currentParts = 0;

            _state.layers = new float[_layers.Length];

            for (var i = 0; i < _layers.Length; i++)
            {
                currentParts += _layers[i].count;
                initParts += _layers[i].initCount;

                _state.layers[i] = (float)_layers[i].count / _layers[i].initCount;
            }

            if(initParts <= 0) return;

            _state.total = (float)currentParts / initParts;

            UIManager.Instance.SetBuildingCondition(_state);
        }

        private void OnDrawGizmos()
        {
            var position = transform.position;

            Gizmos.color = Color.blue;

            var size = max - min;
            var center = position + min + size / 2;

            Gizmos.DrawWireCube(center,size);

            if (heightCheckCount <= 0) return;

            Gizmos.color = Color.white;
            var yMin = position.y + min.y;
            var yMax = position.y + max.y;

            var stepInit =
                    position
                    + min
                    + new Vector3(size.x, 0, size.z) / 2
                ;

            var stepSize = new Vector3(size.x, heightCheckSize, size.z);

            for (var i = 0; i < heightCheckCount; i++)
            {
                var stepCenter =
                    stepInit + new Vector3(0, Mathf.Lerp(yMin, yMax, (float)i / (heightCheckCount - 1)), 0);

                Gizmos.DrawWireCube(stepCenter, stepSize);
            }
        }
    }
}