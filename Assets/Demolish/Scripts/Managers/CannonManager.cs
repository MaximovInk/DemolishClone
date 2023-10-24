using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MaximovInk
{
    
    public class CannonManager : MonoBehaviourSingleton<CannonManager>
    {
        private class CooldownLogic
        {
            private float _delay;
            private float _timer;

            public CooldownLogic(float cooldown)
            {
                _delay = cooldown;
                _timer = _delay;
            }

            public bool Click()
            {
                if (_timer >= _delay)
                {
                    _timer = 0;

                    return true;
                }

                return false;
            }

            public void Update(float cooldown)
            {
                _delay = cooldown;

                if (_timer > _delay) return;

                _timer += Time.deltaTime;
            }
        }


        [Header("Cannon")]
        [SerializeField] private float _rotationSpeed = 10f;
        [SerializeField] private Vector3 _lookOffset = new(0, 2, 0);
        [SerializeField] private float _cooldown = 5f;

        [Header("Projectile")]
        [SerializeField] private float _projectileForce = 50;
        [SerializeField] private float _hideProjectileDelay = 5f;
        [Header("Ammo")]
        [SerializeField] private AmmoDatabase _ammoDatabase;
        [SerializeField] private int _currentAmmoIdx = 0;

        [SerializeField] private TrajectoryRenderer _trajectoryRenderer;
        [SerializeField] private Cannon _currentCannon;
 
        private Transform _source;
        private Camera _camera;

        public AmmoDatabase AmmoDatabase=>_ammoDatabase;
        public float GetCannonSpeedRotation() => _rotationSpeed;
        public Vector3 GetLookCannonOffset() => _lookOffset;
        public float GetProjectileHideDelay() => _hideProjectileDelay;

        public event Action<int> OnWeaponShootEvent;

        [SerializeField] private LayerMask _explosionLayerMask;

        public LayerMask ExplosionLayerMask => _explosionLayerMask;

        private CooldownLogic _cooldownLogic;

        public int CurrentAmmoIndex
        {
            get => _currentAmmoIdx;
            set => _currentAmmoIdx = value;
        }

        private void Awake()
        {
            _camera = Camera.main;
            _cooldownLogic = new CooldownLogic(_cooldown);
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
            if (!Physics.Raycast(ray, out _, Mathf.Infinity)) return;

            _source = _trajectoryRenderer.transform;

            _trajectoryRenderer.CalculatePath(_source.position, _source.forward * _projectileForce);
            _trajectoryRenderer.Draw();
             
            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (Input.GetMouseButtonUp(0) && _cooldownLogic.Click())
            {
                Shoot();
            }

            _cooldownLogic.Update(_cooldown);
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
