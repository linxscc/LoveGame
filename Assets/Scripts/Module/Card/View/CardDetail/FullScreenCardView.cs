using Assets.Scripts.Framework.GalaSports.Core;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class FullScreenCardView : View
    {
        private RawImage _card;
        private RawImage _signaturetex;
        
        private void Awake()
        {
            RectTransform rect = transform.Find("Scroll").GetComponent<RectTransform>();
            rect.offsetMax = new Vector2(0, ModuleManager.OffY );
            _card = transform.Find("Scroll/Card").GetComponent<RawImage>();
            _signaturetex = transform.Find("Scroll/Card/SignatureTex").GetRawImage();
            PointerClickListener.Get(gameObject).onClick = go => { SendMessage(new Message(MessageConst.MODULE_CARD_CLOSE_FULLSCREEN));};
        }

        public void SetTexture(RawImage texture,RawImage signature=null)
        {
            _card.texture = texture.texture;
            _card.transform.position = texture.transform.position;
            var hassignature = signature != null;
            _signaturetex.gameObject.SetActive(hassignature);
            if (hassignature)
            {
                _signaturetex.texture = signature.texture;  
                _signaturetex.SetNativeSize();
            }

        }
    }
}