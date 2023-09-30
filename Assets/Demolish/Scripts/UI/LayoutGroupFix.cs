using UnityEngine;
using UnityEngine.UI;

namespace MaximovInk
{
    public class LayoutGroupFix : MonoBehaviour
    {


        void Start()
        {
            var rectTransform = GetComponent<RectTransform>();

            this.Invoke(() =>
            {
                LayoutRebuilder.MarkLayoutForRebuild(rectTransform);
            },0.1f);

            
        }
    }
}