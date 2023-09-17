using UnityEngine;

namespace MaximovInk
{
    [CreateAssetMenu(fileName = "AmmoDatabase", menuName = "Game/AmmoDatabase", order = 1)]
    public class AmmoDatabase : ScriptableObject
    {
        [SerializeField]
        private AmmoData[] AmmoTypes;

        public AmmoData GetAmmoData(int index)
        {
            return AmmoTypes[index];
        }

        public int Size => AmmoTypes.Length;
    }
}
