using System;
using UnityEngine;
using UnityEngine.UI;

using GamePush;

namespace MaximovInk
{
    [Serializable]
    public struct StageSetStruct
    {
        public Sprite Point;
    }

    public class RewardScreen : MonoBehaviour
    {
        private const string REWARD_ID = "STARS_MULTIPLY";

        [SerializeField] private StageSetStruct _completedStruct;
        [SerializeField] private StageSetStruct _normalStruct;

        [SerializeField] private Image[] _points;
        [SerializeField] private Image[] _lines;
       // [SerializeField] private GameObject[] _stars;
        [SerializeField] private StarsAnimation _starAnimation;

        public int Stars = 0;

        public event Action OnRewardEvent;

        public void Show()
        {
            gameObject.SetActive(true);

            /*
              for (var i = 0; i < _stars.Length; i++)
             {
                 _stars[i].SetActive((i + 1) <= Stars);
             }
             */

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

        }

        private int _multipliedStars = 0;

        private void NextLevel()
        {
            PlayerDataManager.Instance.AddStars(_multipliedStars);
            LevelManager.Instance.NextLevel();
            OnRewardEvent?.Invoke();
            gameObject.SetActive(false);
        }

        public void GetMultiplied()
        {
            _multipliedStars = Stars;
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
