using System;
using UnityEngine;
using UnityEngine.UI;

using GamePush;

namespace MaximovInk
{

    [System.Serializable]
    public struct StageSetStruct
    {
        public Sprite Point;
    }

    public class RewardScreen : MonoBehaviour
    {
        [SerializeField] private StageSetStruct _completedStruct;
        [SerializeField] private StageSetStruct _normalStruct;

        [SerializeField] private Image[] _points;
        [SerializeField] private Image[] _lines;
        [SerializeField] private GameObject[] _stars;

        public int Stars = 0;

        public event Action OnRewardEvent;

        public void Show()
        {
            gameObject.SetActive(true);

            for (var i = 0; i < _stars.Length; i++)
            {
                _stars[i].SetActive((i + 1) <= Stars);
            }

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

        public void GetMultiplied()
        {
            GP_Ads.ShowFullscreen(null, isFailed =>
            {
                if (!isFailed)
                {
                    PlayerDataManager.Instance.AddStars(Stars * 3);
                    LevelManager.Instance.NextLevel();
                }

                OnRewardEvent?.Invoke();
                gameObject.SetActive(false);
            });

        }

        public void GetNormal()
        {
            gameObject.SetActive(false);
            PlayerDataManager.Instance.AddStars(Stars);
            LevelManager.Instance.NextLevel();
            OnRewardEvent?.Invoke();
        }
    }
}
