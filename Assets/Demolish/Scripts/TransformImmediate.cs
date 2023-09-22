
using System;
using UnityEngine;

namespace Assets.Demolish.Scripts
{
    public class TransformImmediate : MonoBehaviour
    {
        private void Update()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}
