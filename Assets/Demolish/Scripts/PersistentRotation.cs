using UnityEngine;

namespace MaximovInk
{
    public class PersistentRotation : MonoBehaviour
    {
        [SerializeField] private Vector3 RotationPerSec;

        private void Update()
        {
            transform.rotation *= Quaternion.Euler(RotationPerSec * Time.deltaTime);
        }
    }
}

