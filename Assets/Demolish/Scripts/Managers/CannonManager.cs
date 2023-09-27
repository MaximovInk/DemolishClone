using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace MaximovInk
{
    public class CannonManager : MonoBehaviourSingleton<CannonManager>
    {
        [Header("Projectile")]
        [SerializeField] private float _projectileForce = 50;
        [SerializeField] private float _raycastProjectileLength = 1f;
        [SerializeField] private float _hideProjectileDelay = 5f;
        [Header("Ammo")]
        [SerializeField] private AmmoDatabase _ammoDatabase;
        [SerializeField] private int _currentAmmoIdx = 0;

        [SerializeField] private TrajectoryRenderer _trajectoryRenderer;
        [SerializeField] private Cannon _currentCannon;
 
        private Vector3 _target;
        private Transform _source;
        private Camera _camera;

        public float GetProjectileHideDelay() => _hideProjectileDelay;
        public float GetRaycastLength()=> _raycastProjectileLength;

        public event Action<int> OnWeaponShootEvent;

        public int CurrentAmmoIndex
        {
            get => _currentAmmoIdx;
            set => _currentAmmoIdx = value;
        }

        private void Awake()
        {
            _camera = Camera.main;
            InitializePool();
            InitializeTrajectory();
        }

        private void InitializePool()
        {
            for (var i = 0; i < _ammoDatabase.Size; i++)
            {
                var data = _ammoDatabase.GetAmmoData(i);
                ProjectilePool.Instance.AddProjectilePool(i, data);
            }
        }

        private void InitializeTrajectory()
        {
            var trajectoryParent = _currentCannon.GetTrajectoryParent();
            _trajectoryRenderer.transform.SetParent(trajectoryParent);
            _trajectoryRenderer.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        private void Update()
        {
            var mousePosition = Input.mousePosition;
            var ray = _camera.ScreenPointToRay(mousePosition);

            if (!Physics.Raycast(ray, out var hit, 50)) return;

            _source = _trajectoryRenderer.transform;
            _target = hit.point;


            _trajectoryRenderer.CalculatePath(_source.position, _source.forward * _projectileForce, _target);
            _trajectoryRenderer.Draw();
             
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
        }

        public AmmoData GetAmmoByID(int index)
        {
            return _ammoDatabase.GetAmmoData(index);
        }

        private void Shoot()
        {
            var currentAmmo = _ammoDatabase.GetAmmoData(_currentAmmoIdx);
            var projectile = ProjectilePool.Instance.GetProjectile(_currentAmmoIdx);

            projectile.transform.position = _source.position;
            projectile.SetupProjectile(currentAmmo);
            projectile.SetVelocity(_source.forward * _projectileForce);

            OnWeaponShootEvent?.Invoke(_currentAmmoIdx);

            if (!WeaponButton.IsCanShoot(_currentAmmoIdx))
            {
                _currentAmmoIdx = 0;
                WeaponButton.Select(0);
            }
        }

    }
}
