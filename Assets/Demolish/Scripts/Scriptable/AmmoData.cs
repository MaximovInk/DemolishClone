using System;
using UnityEngine;

namespace MaximovInk
{
    public enum AmmoType
    {
        Basic,
        Bomb,
        Fire,
        Missle,
        Nuclear
    }

    [Serializable]
    public class AmmoData
    {
        [Header("Main")]
        public AmmoType NameID = AmmoType.Basic;
        public float ForceMultiplier = 1f;

        public Vector3 Scale = Vector3.one;
        public Projectile Prefab;

        [Header("Audio")]
        public AudioClip HitAudioClip;
        public AudioClip ShootAudioClip;

        [Header("Explosion parameters")]
        public bool IsExplode = false;
        public float ExplodeForce;
        public float ExplodeRadius;
        public GameObject CustomExplosionPrefab;
        public bool DestroyOnExplode;
    }
}
