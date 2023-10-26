using System.Collections;
using UnityEngine;

namespace MaximovInk
{
    public class StarAddAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private AnimationMove _prefab;
        [SerializeField] private float _delayRepeat;
        [SerializeField] private float _animationTime = 1f;

        public void SpawnStars(int count)
        {
            StartCoroutine(SpawnStarsCoroutine(count));
        }

        private IEnumerator SpawnStarsCoroutine(int count)
        {
            for (int i = 0; i < count; i++)
            {
                SpawnStar();
                yield return new WaitForSeconds(_delayRepeat);
            }
        }

        //THIS OR USE DOTWEEN
        private void SpawnStar()
        {
            var instance = Instantiate(_prefab, UIManager.Instance.MainCanvas);

            instance.GetComponent<RectTransform>().anchoredPosition = new Vector3(0,0,0);

            instance.Initialize(_target, _animationTime);
            instance.OnKill += () =>
            {
                PlayerDataManager.Instance.AddStars(1);
            };

        }
    }
}
