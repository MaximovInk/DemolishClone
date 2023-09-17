using System;
using UnityEngine;

namespace MaximovInk
{
    [Serializable]
    public class AmmoData
    {
        public string NameID = string.Empty;
        public float ForceMultiplier = 1f;
        public bool IsExplode = false;
        public float ExplodeForce;
        public float ExplodeRadius;
        public Vector3 Scale = Vector3.one;
        public Projectile Prefab;
    }
}
