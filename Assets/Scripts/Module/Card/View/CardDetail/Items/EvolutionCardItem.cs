using System;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class EvolutionCardItem : MonoBehaviour
    {
        private bool _isSelected = false;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                if (_isSelected)
                {
                    _image.color = new Color(_image.color.r,_image.color.g,_image.color.b,1.0f);
                }
                else
                {
                    _image.color = new Color(_image.color.r,_image.color.g,_image.color.b,0.2f);
                }
            }
        }

        private Action<EvolutionCardItem> _clickCallback;
        private RawImage _image;

        private void Awake()
        {
            _image = transform.Find("MaskImage/Image").GetComponent<RawImage>();
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if(_clickCallback != null)
                _clickCallback.Invoke(this);
        }

        public void InitItem(Texture sp, Action<EvolutionCardItem> clickCallback)
        {
            if (sp != null)
            {
                _image.texture = sp;
            }
            else
            {
                FlowText.ShowMessage("NOCARD");
            }
            _clickCallback = clickCallback;
        }
    }
}