using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class SmsItem : MonoBehaviour
    {
        protected Image _headImage;
        protected Image _bgImage;
        protected Text _contextText;
        protected float _lineHeight;
        protected float _lineSpacing;
        protected float _originalHeight;
        protected SmsDialogVo _data;

        protected Coroutine _coroutine;
        
        public Action OnStepEnd;
        public bool IsEnd;

        private void Awake()
        {
            _headImage = transform.Find("HeadBg/HeadMask/Head").GetComponent<Image>();
            _bgImage = transform.Find("Dialog/BgImage").GetComponent<Image>();

            _contextText = transform.Find("Dialog/BgImage/Text").GetComponent<Text>();

            _contextText.text = "";
            _lineHeight = _contextText.preferredHeight + 3;
            _lineSpacing = (_contextText.lineSpacing - 1) * _contextText.fontSize;

            RectTransform rect = GetComponent<RectTransform>();
            _originalHeight = rect.sizeDelta.y;

            float w = Screen.width / Main.CanvasScaleFactor;
            
            rect.sizeDelta = new Vector2(w, _originalHeight);

            IsEnd = false;
            
            AwakeProxy();

            CanvasGroup cg = transform.GetComponent<CanvasGroup>();
            cg.alpha = 0.3f;
            cg.DOFade(1, 0.3f);
        }
        
        public virtual void Stop()
        {
            if (_coroutine != null)
            {
                ClientTimer.Instance.CancelDelayCall(_coroutine);
            }
        }

        public virtual void SetHead(SmsVo.SmsRole role)
        {
            _headImage.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Story_Role" + (int) (role)); 
        }

        public virtual void SetData(SmsDialogVo smsVo)
        {
            _data = smsVo;

            SetText(_data.ContextText);
        }
        
        public virtual void SetText(string text)
        {
            _coroutine = ClientTimer.Instance.DelayCall(() =>
            {
                IsEnd = true;
            }, 0.5f);
        }
        
        protected virtual void AwakeProxy()
        {
            
        }
        
        protected void RollToEnd()
        {
            RectTransform content = transform.parent.GetRectTransform();
            RectTransform viewport = content.parent.GetRectTransform();

            content.DOAnchorPos(new Vector2(content.anchoredPosition.x, content.sizeDelta.y - viewport.sizeDelta.y), 0.5f);
        }
    }
}