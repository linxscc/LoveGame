using DG.Tweening;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Assets.Scripts.Module.Guide
{
    public class GuideArrow
    {
        public static void DoAnimation(Transform parent, float time = 0.6f,float rang = 96)
        {
           
            DoAnimation(
                parent.Find("Arrow/Image"),
                parent.Find("Particle").GetComponent<ParticleSystem>(),
                time,
                rang);
        }

        public static void DoAnimation(Transform arrow, ParticleSystem particleSystem, float time = 0.6f, float rang = 96)
        {
            Vector2 startPos = arrow.GetRectTransform().anchoredPosition;

            Vector3 angle = arrow.localEulerAngles;
            float sin = -Mathf.Sin(angle.z / 180 * Mathf.PI);
            float cosin = Mathf.Cos(angle.z / 180 * Mathf.PI);

            Vector3 pos = new Vector3(startPos.x + rang * sin, startPos.y + rang * cosin);

            var lowerTweener = arrow.DOBlendableLocalMoveBy(pos, time);
            int count = 0;
            lowerTweener.SetLoops(-1, LoopType.Yoyo).onStepComplete = () =>
            {
                count++;
                if(count%2 == 0)
                {
                    particleSystem.Play(true);
                }
            };
            lowerTweener.Play();
        }
    }
}