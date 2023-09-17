using UnityEngine;

namespace MaximovInk
{
    public class Cannon : MonoBehaviour
    {
        private const float DISTANCE = 50f;

        [SerializeField] private Transform _tower;
        [SerializeField] private Transform _gun;
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private Vector3 _lookOffset = new Vector3(0, 2, 0);
        [SerializeField] private Transform _trajectoryParent;

        private Camera _camera;

        public Transform GetTrajectoryParent() => _trajectoryParent;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void RotateTower(Vector3 lookAt)
        {
            var originalRot = _tower.rotation;
            lookAt.y = _tower.position.y;
            _tower.LookAt(lookAt, Vector3.up);
            var newRot = _tower.rotation;

            _tower.rotation = Quaternion.Lerp(originalRot, newRot, _rotationSpeed * Time.deltaTime);
        }

        private void RotateGun(Vector3 lookAt)
        {
            var originalRot = _gun.rotation;
            _gun.LookAt(lookAt+ _lookOffset, Vector3.up);

            var newRot = _gun.rotation;
            var angles = newRot.eulerAngles;
            angles.y = _tower.rotation.eulerAngles.y;
            newRot.eulerAngles = angles;

            _gun.rotation = Quaternion.Lerp(originalRot, newRot, _rotationSpeed * Time.deltaTime);
        }

        private void Update()
        {
            var mousePosition = Input.mousePosition;
            var ray = _camera.ScreenPointToRay(mousePosition);

            if (!Physics.Raycast(ray, out var hit, DISTANCE)) return;

            RotateTower(hit.point);
            RotateGun(hit.point);
        }
    }
}