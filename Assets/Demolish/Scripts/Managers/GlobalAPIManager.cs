using GamePush;

namespace MaximovInk
{
    internal class GlobalAPIManager : MonoBehaviourSingleton<GlobalAPIManager>
    {
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(this);
            GP_Game.GameplayStart();
        }

        private void OnDestroy()
        {
            if (Instance != null && Instance == this)
                GP_Game.GameplayStop();
        }
    }
}
