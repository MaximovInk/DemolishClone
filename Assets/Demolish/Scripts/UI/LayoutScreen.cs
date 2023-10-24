using UnityEngine;

namespace MaximovInk
{
    public class LayoutScreen : MonoBehaviour
    {
        public virtual void Show(bool isShowed) => gameObject.SetActive(true);
        public virtual void Hide() => gameObject.SetActive(false);
        public bool IsActive => gameObject.activeSelf;

        public virtual void Close() {
            LayoutScreens.Instance?.OnScreenClosed();
            Hide();
        }

    }
}
