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
            if (index < AmmoTypes.Length) return AmmoTypes[index];

            Debug.LogError($"Index({index}) > AmmoTypes.Length{AmmoTypes.Length}");
            return null;

        }
    }
}
