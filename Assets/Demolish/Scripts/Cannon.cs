using UnityEngine;

namespace MaximovInk
{
    public class Cannon : MonoBehaviour
    {
        [SerializeField] private AmmoDatabase _ammoDatabase;
        [SerializeField] private float force = 50;
        [SerializeField] private TrajectoryRenderer trajectoryRenderer;

        [SerializeField] private int _currentAmmoIdx = 0;

        private Vector3 _target;
        private Transform _source;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            var mousePosition = Input.mousePosition;
            var ray = _camera.ScreenPointToRay(mousePosition);

            if (!Physics.Raycast(ray, out var hit, 50)) return;

            _source = transform;
            _target = hit.point;


            trajectoryRenderer.CalculatePath(_source.position, _source.forward * force, _target);
            trajectoryRenderer.Draw();

            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }

        private void Shoot()
        {
            var currentAmmo = _ammoDatabase.GetAmmoData(_currentAmmoIdx);

            var projectile = Instantiate(currentAmmo.Prefab);

            projectile.transform.position = _source.position;
            projectile.SetupProjectile(currentAmmo);
            projectile.SetVelocity(_source.forward * force);
        }

    }
}