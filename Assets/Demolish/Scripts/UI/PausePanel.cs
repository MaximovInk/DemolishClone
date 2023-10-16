using UnityEngine;

namespace MaximovInk
{
    public class PausePanel : MonoBehaviour
    {
        private void OnEnable()
        {
            GamePush.GP_Game.Pause();
        }

        private void OnDisable()
        {
            GamePush.GP_Game.Resume();
        }
    }
}
