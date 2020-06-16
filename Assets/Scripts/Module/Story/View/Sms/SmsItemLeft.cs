using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class SmsItemLeft : SmsItem
    {
        private Image _catImage;
        private Image _catHead;
        private Image _catTail;
        private float _tailY;

        protected override void AwakeProxy()
        {
            _catImage = transform.Find("Dialog/CatBg").GetComponent<Image>();
            _catHead = transform.Find("Dialog/CatBg/Cat").GetComponent<Image>();
            _catTail = transform.Find("Dialog/Tail").GetComponent<Image>();
            _tailY = _catTail.rectTransform.anchoredPosition.y;
        }

        public override void SetData(SmsDialogVo vo)
        {
            _data = vo;

            LayoutRebuilder.MarkLayoutForRebuild(transform.parent as RectTransform);
            RollToEnd();
            
            StartCoroutine(ShowWaitForInput());
        }

        private IEnumerator ShowWaitForInput()
        {
            string str = I18NManager.Get("Story_Input");

            int count = 0;
            while (count < 10)
            {
                _contextText.text = str.Substring(0, 3 + count % 6 + 1);
                count++;
                yield return new WaitForSeconds(0.25f);
            }
            
            SetText(_data.ContextText);
            LayoutRebuilder.MarkLayoutForRebuild(transform.parent as RectTransform);
            
            yield return new WaitForSeconds(0.5f);
            IsEnd = true;
        }
        
        public override void SetText(string text)
        {
            _contextText.text = text;

            string[] arr = text.Split('\n');

            float height = arr.Length * _lineHeight;
            
            RectTransform contextRect = _contextText.GetComponent<RectTransform>();
            Vector2 size = new Vector2(_contextText.preferredWidth, height);

            float duration = 0.2f;
            contextRect.DOSizeDelta(size, duration);
            _bgImage.rectTransform.DOSizeDelta(new Vector2(size.x + 80, size.y + 60), duration);
            _catImage.rectTransform.DOAnchorPos(new Vector2(size.x + 50, _catImage.rectTransform.anchoredPosition.y),
                duration);
            _catImage.rectTransform.DOSizeDelta(
                new Vector2(_catImage.rectTransform.sizeDelta.x, size.y + 60), duration);
            RectTransform rect = GetComponent<RectTransform>();
            rect.DOSizeDelta(new Vector2(rect.sizeDelta.x, _originalHeight + height - _lineHeight), duration).onUpdate =
                () =>
                {
                    LayoutRebuilder.MarkLayoutForRebuild(transform.parent as RectTransform);
                };
            
            RollToEnd();

            Sprite sp;
            if (arr.Length == 1)
            {
                sp = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Story_cat1Line");
            }
            else if (arr.Length == 2)
            {
                sp = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Story_cat2Line");
            }
            else
            {
                sp = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Story_cat3Line");
            }
            _catHead.sprite = sp;
            _catHead.SetNativeSize();
        }
    }
}