using UnityEngine;

namespace MaximovInk
{
    public class Helicopter : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            HelicopterEvent.Instance.HelicopterDamaged();
        }
    }
}
