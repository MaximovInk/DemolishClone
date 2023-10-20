using UnityEngine;

namespace MaximovInk
{
    [RequireComponent(typeof(LineRenderer))]
    public class TrajectoryRenderer : MonoBehaviour
    {
        private LineRenderer _lineRenderer;
        private readonly Vector3[] _points = new Vector3[100];
        private int _pointCount = 100;

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }

        public void CalculatePath(Vector3 source, Vector3 force)
        {
            _pointCount = _points.Length;

            var layerMask = CannonManager.Instance.ExplosionLayerMask;

            for (var i = 0; i < _points.Length; i++)
            {
                var time = i * 0.1f;

                _points[i] = source + force * time + Physics.gravity * (time * time) / 2f;

                var hit = Physics.OverlapSphere(_points[i], 0.1f, layerMask);
                if (hit.Length>0)
                {
                    _pointCount = i+1;
                    break;
                }

            }
        }

        public void Draw()
        {
            _lineRenderer.positionCount = _pointCount;
            _lineRenderer.SetPositions(_points);
        }
    }
}