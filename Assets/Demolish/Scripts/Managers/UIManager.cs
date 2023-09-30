using UnityEngine;
using UnityEngine.UI;

namespace MaximovInk
{
    public class UIManager : MonoBehaviourSingleton<UIManager>
    {
        
        [SerializeField] private Sprite _weaponButtonSelectSprite;
        public Sprite WeaponButtonSelectSprite => _weaponButtonSelectSprite;
        [SerializeField] private Sprite _weaponButtonDeselectSprite;
        public Sprite WeaponButtonDeselectSprite => _weaponButtonDeselectSprite;

        public RewardScreen RewardScreen;

    }
}