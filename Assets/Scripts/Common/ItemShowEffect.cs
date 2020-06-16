using UnityEngine;
using DG.Tweening;

namespace Common
{
    public class ItemShowEffect:MonoBehaviour
    {
//        public void OnShowEffectTween(float delay)
//        {
//            transform.localScale=new Vector3(1.4f,1.4f);
//            var canvas = gameObject.GetComponent<CanvasGroup>();
//            if ( canvas == null)
//            {
//                canvas = gameObject.AddComponent<CanvasGroup>();
//            }
//            canvas.alpha = 0f;
//            // gameObject.Show();
//            Go.to(transform, 0.3f,
//                new GoTweenConfig().scale(new Vector3(1f, 1f)).setDelay(delay).setEaseType(GoEaseType.ExpoOut));
//            Go.to(canvas, 0.3f,
//                new GoTweenConfig().floatProp("alpha",1f).setDelay(delay).setEaseType(GoEaseType.ExpoOut));
//        }

        public void OnShowEffect(float delay)
        {
            transform.localScale=new Vector3(1.4f,1.4f);
            var canvas = gameObject.GetComponent<CanvasGroup>();
            if ( canvas == null)
            {
                canvas = gameObject.AddComponent<CanvasGroup>();
            }
            canvas.alpha = 0f;
            transform.DOScale(new Vector3(1f, 1f), 0.3f).SetEase(Ease.OutExpo).SetDelay(delay);
            canvas.DOFade(1f, 0.3f).SetEase(Ease.OutExpo).SetDelay(delay);;
        }
    }
}