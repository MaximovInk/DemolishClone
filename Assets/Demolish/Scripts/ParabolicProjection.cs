
    using UnityEngine;

    [RequireComponent(typeof(LineRenderer))]
    public class ParabolicProjection : MonoBehaviour
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float force = 50;
        [SerializeField] private int maxSegmentCount = 300;

        private int _numSegments = 0;

        private Vector3[] _segments;
        private LineRenderer _lineRenderer;

        private Vector3 _target;
        private Transform _source;


        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
        }


        private void Update()
        {
            var mousePosition = Input.mousePosition;
            var ray = Camera.main.ScreenPointToRay(mousePosition);

            if (!Physics.Raycast(ray, out var hit, 50)) return;

            _source = transform;
            _target = hit.point;

            SimulatePath();
            Draw();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shoot();
            }
        }


        private void SimulatePath()
        {
            if (_segments == null || _segments.Length != maxSegmentCount)
            {
                _segments = new Vector3[maxSegmentCount];
            }

            var distance = Vector3.Distance(_source.position, _target);

            _segments[0] = _source.position;
            _numSegments = 1;

            _segments[1] = _source.position + _source.forward * distance;
            _numSegments++;
        }

        private void Draw()
        {
            _lineRenderer.transform.position = _segments[0];

            _lineRenderer.positionCount = _numSegments;
            for (var i = 0; i < _numSegments; i++)
            {
                _lineRenderer.SetPosition(i, _segments[i]);
            }
        }

        private void Shoot()
        {
            var solutions = new Vector3[2];

            var sourcePosition = _source.position;
            var numSolutions = fts.solve_ballistic_arc(sourcePosition, force, _target, Mathf.Abs(Physics.gravity.y),
                out solutions[0],
                out solutions[1]);

            var targetPos = _target;
            var diff = targetPos - sourcePosition;
            var diffGround = new Vector3(diff.x, 0f, diff.z);

            if (numSolutions > 0)
            {
                var projectile = Instantiate(projectilePrefab);

                var projectileTransform = projectile.transform;
                var gunTransform = transform;

                projectileTransform.SetPositionAndRotation(gunTransform.position, gunTransform.rotation);

                transform.forward = diffGround;

                var motion = projectile.GetComponent<BallisticMotion>();
                motion.Initialize(_source.position, Physics.gravity.y);

                var impulse = solutions[0];

                motion.AddImpulse(impulse);
            }
            else
            {
                Debug.Log("No solutions");
            }


        }
    }