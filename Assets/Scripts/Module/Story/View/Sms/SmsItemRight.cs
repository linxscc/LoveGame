using UnityEngine;

namespace game.main
{
    public class SmsItemRight : SmsItem
    {
        public override void SetText(string text)
        {
            base.SetText(text);
            
            _contextText.text = text;

            string[] arr = text.Split('\n');

            float height = arr.Length * _lineHeight;
            
            RectTransform contextRect = _contextText.GetComponent<RectTransform>();
            Vector2 size = new Vector2(_contextText.preferredWidth, height);

            contextRect.sizeDelta = size;
            
            _bgImage.rectTransform.sizeDelta = new Vector2(size.x + 100, size.y + 60);
            
            RectTransform rect = GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, _originalHeight + height - _lineHeight);
            
            RollToEnd();
        }
    }
}