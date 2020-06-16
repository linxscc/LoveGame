using DG.Tweening;
using game.tools;
using Spine.Unity;
using UnityEngine;

namespace game.main
{
    public class FansSpineItem : MonoBehaviour
    {
        private RectTransform _rect;
        private Vector2 _startPos;
        private Tweener _tweener;
        private Vector2 _endPos;
        private float _time;
        private SkeletonGraphic _skg;
        private string _animationName;
        private bool _reversal;

        public void Init(string spineId, SkeletonGraphic skg, string animationName)
        {
            _skg = skg;
            _animationName = animationName;

            SkeletonDataAsset skData = SpineUtil.BuildSkeletonDataAsset(spineId, skg);
            skg.skeletonDataAsset = skData;
            skg.Initialize(true);
            skg.AnimationState.SetAnimation(0, animationName, true);

            _rect = skg.GetComponent<RectTransform>();
            _rect.pivot = new Vector2(0.5f, 0);
            _rect.anchorMin = new Vector2(0.5f, 0);
            _rect.anchorMax = new Vector2(0.5f, 0);
            _rect.sizeDelta = new Vector2(300, 500);
            _rect.localScale = new Vector3(0.5f, 0.5f, 1);


             moveTowardPosition = transform.localPosition;
        }


        private Vector3 moveDirection;
        float moveSpeed = 1f;
        Transform tf;
        public void Move(Vector3[] path, float deltaTime)
        {
            tf = transform;
            Vector3 currentPosition = tf.localPosition;
            Vector3 moveDirection = path[0] - tf.localPosition;
            moveDirection.z = 0;
            moveDirection.Normalize();

            Vector3 target = moveDirection * moveSpeed + currentPosition;
            transform.position = Vector3.Lerp(currentPosition, target, Time.deltaTime);
        //    Vector3.MoveTowards()

        }

        Vector3 moveTowardPosition;
        private void Update()
        {

            //1、获得当前位置
            Vector3 curenPosition = transform.localPosition;
            //2、获得方向
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (_tweener != null)
                {
                    _tweener.Kill();
                    _tweener = null;
                }
                // moveTowardPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Debug.Log("Fire1");
                moveTowardPosition = new Vector3(1080, 1080, 0);
                moveTowardPosition.z = 0;
            }
            if (Vector3.Distance(curenPosition, moveTowardPosition) < 0.01f)
            {
                transform.localPosition = moveTowardPosition;
            }
            else
            {
                //3、插值移动
                //距离就等于 间隔时间乘以速度即可
                float maxDistanceDelta = Time.deltaTime * moveSpeed;
                transform.localPosition = Vector3.MoveTowards(curenPosition, moveTowardPosition, maxDistanceDelta);
            }
        }
        //private void Update()
        //{
        //    //1
        //    Vector3 currentPosition = transform.position;
        //    if (Input.GetKeyDown(KeyCode.M))
        //    {
        //        // 3
        //        Vector3 moveToward = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        // 4
        //        moveDirection = moveToward - currentPosition;
        //        moveDirection.z = 0;
        //        moveDirection.Normalize();
        //    }

        //    Vector3 target = moveDirection * moveSpeed + currentPosition;
        //    transform.position = Vector3.Lerp(currentPosition, target, Time.deltaTime);
        //}

        public void Reversal(bool isFront = false)
        {
            if (isFront)
            {
                _rect.Rotate(0, 0, 0);
            }
            else
            {
                _rect.Rotate(0, 180, 0);
            }
        }

        public void ReversalInSeconds(float seconds)
        {
            ClientTimer.Instance.AddCountDown("ReversalInSeconds", -1, seconds,
                s =>
                {
                    Reversal(Random.Range(0,100) > 50);
                }, null);
        }

        public void DoPatrol(Vector2 endPos, float time, float delay, bool reversal = false)
        {
            _reversal = reversal;
            
            KillTween();
            
            _skg.AnimationState.SetAnimation(0, "01_dai_ji", true);
            
            Reversal(!reversal);
            
            _time = time;
            _endPos = endPos;
            _startPos = new Vector2(_rect.anchoredPosition.x, _rect.anchoredPosition.y);
            _tweener = _rect.DOAnchorPos(endPos, time)
                .OnStart(() => { _skg.AnimationState.SetAnimation(0, _animationName, true); })
                .SetDelay(delay)
                .SetAutoKill(false)
                .SetEase(Ease.Linear);
            _tweener.onComplete = PatrolEnd;
        }

        private void PatrolEnd()
        {
            Reversal(!_reversal);
            _rect.DOPlayBackwards();

            ClientTimer.Instance.DelayCall(() =>
                {
                    DoPatrol(_endPos, _time, Random.Range(0.8f, 2.2f), true);
                },
                _tweener.Duration(false));
        }

        public void RandonAnimation()
        {
            KillTween();
            
            Reversal(Random.Range(0,100) > 50);

            string animationName = "03_hui_shou";;
            if (Random.Range(0, 100) > 65)
            {
                animationName = "01_dai_ji";
            }
            _skg.AnimationState.SetAnimation(0, animationName, true);
        }

        public void Stand()
        {
            KillTween();

            Reversal(Random.Range(0,100) > 50);
            _skg.AnimationState.SetAnimation(0, "01_dai_ji", true);
        }
        
        public void WaveHand()
        {
            if (_tweener != null)
            {
                _tweener.Kill();
                _tweener = null;
            }
            
            Reversal(Random.Range(0,100) > 50);
            _skg.AnimationState.SetAnimation(0, "03_hui_shou", true);
        }
        
        private void KillTween()
        {
            if (_tweener != null)
            {
                _tweener.Kill();
                _tweener = null;
            }
        }

        private void OnDestroy()
        {
            KillTween();
        }
    }
}