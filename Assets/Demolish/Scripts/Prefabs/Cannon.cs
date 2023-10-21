using UnityEngine;

namespace MaximovInk
{
    public class Cannon : MonoBehaviour
    {
        private const float DISTANCE = 50f;

        [SerializeField] private Transform _tower;
        [SerializeField] private Transform _gun;

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

            _tower.rotation = Quaternion.Lerp(originalRot, newRot, 1);
        }

        private void RotateGun(Vector3 lookAt)
        {
            var originalRot = _gun.rotation;
            _gun.LookAt(lookAt+ _calculatedLookOffset, Vector3.up);

            var newRot = _gun.rotation;
            var angles = newRot.eulerAngles;
            angles.y = _tower.rotation.eulerAngles.y;
            newRot.eulerAngles = angles;

            _gun.rotation = Quaternion.Lerp(originalRot, newRot, 1);
        }

        private Vector3 _calculatedLookOffset;

        private void Update()
        {
            var mousePosition = Input.mousePosition;
            var ray = _camera.ScreenPointToRay(mousePosition);

            

            if (!Physics.Raycast(ray, out var hit, DISTANCE)) return;

            //Debug.Log(hit.point);

            // var offset = CannonManager.Instance.GetLookCannonOffset();
            var offset = CannonManager.Instance.GetLookCannonOffset();

             var offset1 = offset;
             offset1.y = 0f;

            // _calculatedLookOffset = Vector3.Lerp(offset1, offset, Mathf.Abs(hit.point));

            _calculatedLookOffset = offset1;

            _calculatedLookOffset.y = hit.point.y/offset.y;

            RotateTower(hit.point);
            RotateGun(hit.point);
        }
    }
}