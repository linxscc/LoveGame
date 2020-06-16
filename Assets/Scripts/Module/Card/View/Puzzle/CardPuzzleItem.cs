using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using Common;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace game.main
{
    public class CardPuzzleItem : MonoBehaviour, IPointerClickHandler
    {
        public int Index;
        private Image _cardQualityImage;
        private CardPuzzleVo _data;
        private GameObject _redPoint;

        private void Awake()
        {
            _cardQualityImage = transform.Find("CardQualityImage").GetComponent<Image>();
            _redPoint = transform.Find("RedPoint").gameObject;
        }

        public void SetData(CardPuzzleVo vo)
        {
            _data = vo;

            transform.Find("NameText").GetComponent<Text>().text = $"{vo.Name}({I18NManager.Get("Card_PuzzleTap")})";
            transform.Find("Shape/NumText").GetComponent<Text>().text = vo.Num + "/" + vo.RequireNum;
            
            _cardQualityImage.sprite = AssetManager.Instance.GetSpriteAtlas(CardUtil.GetNewCreditSpritePath(vo.Credit));
            
            //_cardQualityImage.SetNativeSize();
            RawImage cardImage = transform.Find("Mask/CardImage").GetComponent<RawImage>();
            Texture texture = ResourceManager.Load<Texture>(vo.CardPath,ModuleConfig.MODULE_CARD);

            
            //假如没有图的话，默认不去读图！
//            if (texture == null)
//                texture = ResourceManager.Load<Texture>("Card/Image/SmallCard/1000");
            cardImage.texture = texture;
            _redPoint.SetActive(vo.Num>=vo.RequireNum);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            EventDispatcher.TriggerEvent<CardPuzzleVo>(EventConst.CardPuzzleClick, _data);
        }
    }
}