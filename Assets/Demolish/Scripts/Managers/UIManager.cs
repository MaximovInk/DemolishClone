using UnityEngine;

namespace MaximovInk
{
    public class UIManager : MonoBehaviourSingleton<UIManager>
    {
        [SerializeField] private Sprite _weaponButtonSelectSprite;
        public Sprite WeaponButtonSelectSprite => _weaponButtonSelectSprite;
        [SerializeField] private Sprite _weaponButtonDeselectSprite;
        public Sprite WeaponButtonDeselectSprite => _weaponButtonDeselectSprite;

        public LevelCompleteScreen LevelCompleteScreen;
        public RewardScreen RewardScreen;

        public LayoutScreens Screens { get => _layoutScreens; }

        [SerializeField] private LayoutScreens _layoutScreens;

    }
}