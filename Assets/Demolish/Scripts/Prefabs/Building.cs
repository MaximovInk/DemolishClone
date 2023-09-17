using UnityEngine;

namespace MaximovInk
{
    public class Building : MonoBehaviour
    {
        [Header("Сила разрушения от 1 удара \n0 - практически без эффекта \n1 - полностью объект")]
        [SerializeField] [Range(0,1)] private float _damageStrength = 0.2f;

        private FracturedObject _fracturedObject;

        private void Awake()
        {
            _fracturedObject = GetComponent<FracturedObject>();

            ApplyParameters();
        }

        private void ApplyParameters()
        {
            _fracturedObject.ChunkConnectionStrength = 1f - _damageStrength;
        }

        private void OnValidate()
        {
            if(_fracturedObject == null)
                _fracturedObject = GetComponent<FracturedObject>();

            if (_fracturedObject == null)
                return;

            ApplyParameters();
        }
    }
}
