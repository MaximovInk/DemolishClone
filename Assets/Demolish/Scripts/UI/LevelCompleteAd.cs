using UnityEngine;
using GamePush;

namespace MaximovInk
{
    public class LevelCompleteAd : MonoBehaviour
    {
        private void Awake()
        {
            PlayerDataManager.Instance.OnLevelIndexAdd += Instance_OnNextLevelInit;
        }

        private void Instance_OnNextLevelInit(int nextLevel)
        {
            if (PlayerDataManager.Instance.AdsDisabled) return;

            var levelIndex = nextLevel-2;   //+1 offset for buildsettings +1 nextLevel

            var isFullscreen = (levelIndex % 3) == 0;

            //Debug.Log($" {nextLevel - 1} => {levelIndex} | ({levelIndex % 3} == {0}) => {isFullscreen} | {GP_Ads.IsFullscreenAvailable()}");

            if (isFullscreen && GP_Ads.IsFullscreenAvailable())
            {
                GP_Ads.ShowFullscreen();
            }
        }

    }
}
