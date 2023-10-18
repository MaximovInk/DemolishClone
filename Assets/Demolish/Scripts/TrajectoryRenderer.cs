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

        public void CalculatePath(Vector3 source, Vector3 force, Vector3 target)
        {
            _pointCount = _points.Length;
            var lastDistance = 0f;
            for (var i = 0; i < _points.Length; i++)
            {
                var time = i * 0.1f;

                _points[i] = source + force * time + Physics.gravity * (time * time) / 2f;
                var distance = Vector3.Distance(_points[i], target);
                if (i == 0)
                {
                    lastDistance = distance;
                }

                if (_points[i].y < 0 || distance > lastDistance)
                {
                    _pointCount = i + 1;
                    break;
                }

                lastDistance = Vector3.Distance(_points[i], target);
            }
        }

        public void Draw()
        {
            _lineRenderer.positionCount = _pointCount;
            _lineRenderer.SetPositions(_points);
        }
    }
}