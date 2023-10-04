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
        [SerializeField] private float _sequenceDelay = 0.1f;

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

            this.Invoke(() =>
            {
                if (!enabled) return;

                if (starsCount > 1)
                    _left.Play(_animationStateName);

            }, _sequenceDelay);

            this.Invoke(() =>
            {
                if (!enabled) return;

                if (starsCount > 2)
                    _right.Play(_animationStateName);

            }, _sequenceDelay * 2);


        }
    }
}
