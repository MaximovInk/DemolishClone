using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace MaximovInk
{
    public class ProjectilePool : MonoBehaviourSingletonAuto<ProjectilePool>
    {
        private Dictionary<int, ObjectPool<Projectile>> _pool;

        private void Awake()
        {
            name = "ProjectilePool[Instance]";
            _pool = new Dictionary<int, ObjectPool<Projectile>>();
        }

        public void AddProjectilePool(int index, AmmoData data)
        {

            _pool.Add(index, new ObjectPool<Projectile>(
                (() => CreatePooledItem(index, data)),
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject
                ));
        }

        public Projectile GetProjectile(int index)
        {
            var projectile = _pool[index].Get();
            projectile.IsReleased = false;

            return projectile;
        }


        public void ReleaseProjectile(int index, Projectile projectile)
        {
            if (projectile.IsReleased) return;

            projectile.IsReleased = true;
            _pool[index].Release(projectile);
        }

        private Projectile CreatePooledItem(int index, AmmoData data)
        {
            var prefab = data.Prefab;
            var instance = Instantiate(prefab, transform);
            instance.SetReleaseIndex(index);
            instance.gameObject.name = $"Projectile_{data.NameID}";
            return instance;
        }

        private static void OnReturnedToPool(Projectile projectile)
        {
            projectile.gameObject.SetActive(false);
            projectile.ResetDestroyed();
        }


        private static void OnTakeFromPool(Projectile projectile)
        {
            projectile.gameObject.SetActive(true);
        }

        private static void OnDestroyPoolObject(Projectile projectile)
        {
            Destroy(projectile.gameObject);
        }


    }
}