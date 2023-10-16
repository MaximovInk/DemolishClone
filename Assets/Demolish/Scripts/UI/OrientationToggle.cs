using UnityEngine;

namespace MaximovInk
{
    public class OrientationToggle : MonoBehaviour
    {
        [SerializeField] private GameObject _landscape;
        [SerializeField] private GameObject _portrait;

        private void Awake()
        {
            var isPortrait = Screen.width < Screen.height;

            _landscape.SetActive(!isPortrait);
            _portrait.SetActive(isPortrait);
        }
    }
}
