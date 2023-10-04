using UnityEngine;

namespace MaximovInk
{
    public class StarsAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _center;
        [SerializeField] private Animator _left;
        [SerializeField] private Animator _right;
        [SerializeField] private string _animationStateName = "StarAnimation";
        [SerializeField] private string _emptyStateName = "Empty";

        private void OnEnable()
        {
            _center.Play(_emptyStateName);
            _left.Play(_emptyStateName);
            _right.Play(_emptyStateName);
        }

        public void Show(int starsCount)
        {
            if (starsCount > 0)
                _center.Play(_animationStateName);

            if (starsCount > 1)
                _left.Play(_animationStateName);

            if (starsCount > 2)
                _right.Play(_animationStateName);
        }
    }
}
