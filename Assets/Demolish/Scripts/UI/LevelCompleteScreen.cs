using System;
using UnityEngine;
using UnityEngine.UI;

using GamePush;
using TMPro;

namespace MaximovInk
{
    [Serializable]
    public struct StageSetStruct
    {
        public Sprite Point;
    }

    public class LevelCompleteScreen : LayoutScreen
    {
        private const string REWARD_ID = "STARS_MULTIPLY";

        [SerializeField] private StageSetStruct _completedStruct;
        [SerializeField] private StageSetStruct _normalStruct;

        [SerializeField] private Image[] _points;
        [SerializeField] private Image[] _lines;
       // [SerializeField] private GameObject[] _stars;
        [SerializeField] private StarsAnimation _starAnimation;
        [SerializeField] private TextMeshProUGUI _text;

        public int Stars = 0;

        public event Action OnRewardEvent;

        public override void Show(bool isShowed)
        {
            base.Show(isShowed);

           

            _starAnimation.Show(Stars);

            var stage = PlayerDataManager.Instance.GetStage();

            for (var i = 0; i < _points.Length; i++)
            {
                _points[i].sprite = i <= stage ? _completedStruct.Point : _normalStruct.Point;
            }
            for (var i = 0; i < _lines.Length; i++)
            {
                _lines[i].gameObject.SetActive(i < stage);
            }

            var message = string.Empty;

            switch (Stars)
            {
                case 1:
                    message = LocalizationManager.Instance.Get("GREATHER_X_SHOTS", 20);
                    break;
                case 2:
                    message = LocalizationManager.Instance.Get("LESS_X_SHOTS", 20);
                    break;
                case 3:
                    message = LocalizationManager.Instance.Get("LESS_X_SHOTS", 10);
                    break;
            }

            _text.text = message;

            if (isShowed) return;

            if (PlayerDataManager.Instance.IsReward)
            {
                UIManager.Instance.RewardScreen.GenerateChestRewards();
                UIManager.Instance.Screens.ShowScreen("Reward");
            }

        }

        private int _multipliedStars = 0;

        private void NextLevel()
        {
            PlayerDataManager.Instance.AddStars(_multipliedStars);
            LevelManager.Instance.NextLevel();
            OnRewardEvent?.Invoke();
            Close();
        }

        public void GetMultiplied()
        {
            _multipliedStars = Stars;

            if (PlayerDataManager.Instance.AdsDisabled)
            {
                _multipliedStars *= 3;
                NextLevel();

                return;
            }

            if (!GP_Ads.IsRewardedAvailable())
            {
                NextLevel();
                return;
            }

            GP_Ads.ShowRewarded(REWARD_ID, 
                idOrTag =>
                {
                    if (idOrTag != REWARD_ID) return;

                    _multipliedStars *= 3;

                    NextLevel();
                }, 
                null, 
                success =>
                {
                    if (success) return;

                    NextLevel();
                });


        }

        public void GetNormal()
        {
            _multipliedStars = Stars;

            NextLevel();
        }
    }
}
