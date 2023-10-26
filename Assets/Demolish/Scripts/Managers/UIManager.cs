using UnityEngine;

namespace MaximovInk
{
    public class UIManager : MonoBehaviourSingleton<UIManager>
    {
        [SerializeField] private Sprite _weaponButtonSelectSprite;
        public Sprite WeaponButtonSelectSprite => _weaponButtonSelectSprite;

        [SerializeField] private Sprite _weaponButtonDeselectSprite;
        public Sprite WeaponButtonDeselectSprite => _weaponButtonDeselectSprite;

        public LayoutScreens Screens { get => _layoutScreens; }
        [SerializeField] private LayoutScreens _layoutScreens;

        public StarAddAnimation StarAnimation => _starAddAnimation;
        [SerializeField] private StarAddAnimation _starAddAnimation;

        public string NoAdPurchaseID => _noAdPurchaseID;
        [SerializeField] private string _noAdPurchaseID = "noads_demolish";

        public RectTransform MainCanvas => _mainCanvas;
        [SerializeField] private RectTransform _mainCanvas;

        public LevelCompleteScreen LevelCompleteScreen;
        public RewardScreen RewardScreen;

        public Color DisabledWeaponColor = Color.gray;
        public Color NormalWeaponColor = Color.white;
    }
}