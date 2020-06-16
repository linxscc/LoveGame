using System;
using Common;
using DG.Tweening;
using UnityEngine;

namespace game.main
{
    public class ProgressBar : MonoBehaviour
    {
        private RectTransform _mask;

        private RectTransform _rectTransform;
        
        private RectTransform _bar;
        
        private int _progress;

        private float _progressFloat;

        private int _progressY;

        private float deltaX = -17;

        private float _recordProgress=0;

        private void Awake()
        {
            _mask = transform.Find("Mask").GetComponent<RectTransform>();
            _rectTransform = transform.GetComponent<RectTransform>();
            _bar = transform.Find("Mask/Bar").GetComponent<RectTransform>();
            _bar.sizeDelta = new Vector2(_rectTransform.sizeDelta.x+deltaX, _bar.sizeDelta.y);

            
        }

        public float DeltaX
        {
            get { return deltaX; }
            set
            {
                deltaX = value;
                _bar.sizeDelta = new Vector2(_rectTransform.sizeDelta.x+deltaX, _bar.sizeDelta.y);
            }
        }
        
        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value>100?100:value;
                if (_rectTransform!=null)
                {
                    float w = _rectTransform.sizeDelta.x / 100 * _progress;
                
                    _mask.sizeDelta = new Vector2(w<0?0:w, _mask.sizeDelta.y);
                }
            }
        }

        public float ProgressFloat
        {
            get { return _progressFloat; }
            set
            {
                _progressFloat = value > 100 ? 100 : value;
                float w = _rectTransform.sizeDelta.x / 100 * _progressFloat;
                _mask.sizeDelta = new Vector2(w<0?0:w, _mask.sizeDelta.y);
            }
        }

        public int ProgressY
        {
            get { return _progressY; }
            set
            {
//                Debug.LogError(value);
                _progressY = value;
                float h = _rectTransform.sizeDelta.y / 100 * _progressY;
                _mask.sizeDelta=new Vector2(_mask.sizeDelta.x,h);
            }
        }

        public void TweenSlider(float curprogress,Action action,int times=0,float duration=6f)
        {

            if (curprogress>=_progress&&_progress>=0)
            {
                var tweener3=_mask.DOSizeDelta(new Vector2(_rectTransform.sizeDelta.x / 100 * curprogress,_mask.sizeDelta.y), duration/(times+1));
                if (times>=1)
                {
                    var tweener1 = _mask.DOSizeDelta(new Vector2(_rectTransform.sizeDelta.x, _mask.sizeDelta.y),
                        duration / (times+1));
                    
                    var tweener2 = _mask.DOSizeDelta(new Vector2(_rectTransform.sizeDelta.x, _mask.sizeDelta.y), duration / (times+1));

                    if (times==1)
                    {
                        DOTween.Sequence().Append(tweener1)
                            .AppendCallback(() => { _mask.sizeDelta = new Vector2(0, _mask.sizeDelta.y); })
                            .Append(tweener3).OnComplete(
                                () => { action();}); 
                    }
                    else
                    {
                        DOTween.Sequence().Append(tweener1)
                            .AppendCallback(() => { _mask.sizeDelta = new Vector2(0, _mask.sizeDelta.y); }).Append(tweener2.SetEase(Ease.Linear).SetLoops(times - 1, LoopType.Restart))
                            .AppendCallback(() => { _mask.sizeDelta = new Vector2(0, _mask.sizeDelta.y); })
                            .Append(tweener3).OnComplete(
                                () => { action();}); 
                    }


                }
                else
                {
                    //这边可能会倒着进度条播放，只能往右播！主要出现在接近100%点时候，
//                    Debug.LogError(curprogress+"compare"+_recordProgress);

                    if (curprogress<_recordProgress)
                    {
                        DOTween.Sequence().AppendCallback(() =>
                        {
                            _mask.sizeDelta = new Vector2(0, _mask.sizeDelta.y); 
                        }).Append(tweener3).OnComplete(
                            () => { action();});    
                    }
                    else
                    {
                        DOTween.Sequence().Append(tweener3).OnComplete(
                            () => { action();});   
                    }

                    _recordProgress = curprogress;
                }
                


            }
            else
            {
                if (times>=1)
                {
                    var tweener3=_mask.DOSizeDelta(new Vector2(_rectTransform.sizeDelta.x / 100 * curprogress,_mask.sizeDelta.y), duration/(times+1));
                    var tweener1 = _mask.DOSizeDelta(new Vector2(_rectTransform.sizeDelta.x, _mask.sizeDelta.y),
                        duration / (times+1));
                    var tweener2 = _mask.DOSizeDelta(new Vector2(_rectTransform.sizeDelta.x, _mask.sizeDelta.y), duration / (times+1));   //从0到1点动画

                    if (times==1)
                    {
                        DOTween.Sequence().Append(tweener1)
                            .AppendCallback(() => { _mask.sizeDelta = new Vector2(0, _mask.sizeDelta.y); })
                            .Append(tweener3).OnComplete(
                                () => { action();});
                    }
                    else
                    {
                        DOTween.Sequence().Append(tweener1)
                            .AppendCallback(() => { _mask.sizeDelta = new Vector2(0, _mask.sizeDelta.y); }).Append(tweener2.SetEase(Ease.Linear).SetLoops(times - 1, LoopType.Restart))
                            .AppendCallback(() => { _mask.sizeDelta = new Vector2(0, _mask.sizeDelta.y); })
                            .Append(tweener3).OnComplete(
                                () => { action();}); 
                    }

                }
                else
                {
                    Tweener tweener1= _mask.DOSizeDelta(new Vector2(_rectTransform.sizeDelta.x,_mask.sizeDelta.y), duration / (times+1));
                    Tweener tweener2=_mask.DOSizeDelta(new Vector2(_rectTransform.sizeDelta.x / 100 * curprogress,_mask.sizeDelta.y),  duration / (times+1));
                    DOTween.Sequence().Append(tweener1).AppendCallback(() =>
                    {
                        _mask.sizeDelta = new Vector2(0, _mask.sizeDelta.y);  
                    }).Append(tweener2).OnComplete(
                        () => { action(); });  
                }
                

            }
            
            
        }
        
        public void ProgressBarAudio(float duration=0.9f,int pitch=2)
        {
            DOTween.Sequence().AppendCallback(() =>
                {
                    AudioManager.Instance.PlayEffect("progress_bar", 1,true,pitch);
                }).AppendInterval(duration)
                .OnComplete(() =>
                {
                    AudioManager.Instance.StopEffect();  
                });

        }

    }
}