using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Recollection.Data;
using Common;
using DataModel;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Module.Recollection.View
{
    public class CardPropItem : MonoBehaviour
    {
        private Image _cardQualityImage;
        
        private RecollectionCardDropVo _data;
        private RawImage _cardImage;

        private GameObject _recollectionCountImage;


        private void Awake()
        {
            _recollectionCountImage = transform.Find("RecollectionCountImage").gameObject;
            _recollectionCountImage.SetActive(false);
        }
        public void SetData(RecollectionCardDropVo vo)
        {
            _data = vo;
            transform.Find("NameText").GetComponent<Text>().text = vo.CardName;
           
            _cardQualityImage = transform.Find("CardQualityImage").GetComponent<Image>();
            _cardQualityImage.sprite = AssetManager.Instance.GetSpriteAtlas(CardUtil.GetCreditSpritePath(vo.Credit));
            //_cardQualityImage.SetNativeSize();
           
            _cardImage = transform.Find("Mask/CardImage").GetComponent<RawImage>();
            var _vo = vo.UserCardVo;
            bool _evo = false;
            if (_vo != null)
            {
                _evo = _vo.UserNeedShowEvoCard();
            }
            Texture texture = ResourceManager.Load<Texture>(vo.MiddleCardPath(_evo));
          
            if (texture == null)
            {
                texture = ResourceManager.Load<Texture>(vo.MiddleCardPath());
            }

            _cardImage.texture = texture;

            _recollectionCountImage.SetActive(vo.HasCard);
            var num = _recollectionCountImage.transform.Find("Text").GetComponent<Text>();

            var userCardVo = GlobalData.CardModel.GetUserCardById(vo.CardId);
            if (vo.UserCardVo!=null)
            {

                // num.text = (3 - userCardVo.RecollectionCount) + "/3";
                num.text = I18NManager.Get("Recollection_CardPropItemNum",userCardVo.RecollectionCount);


            }
           
           

            if (vo.HasCard)
            {
              
                PointerClickListener.Get(gameObject).onClick = go =>
                {
                    EventDispatcher.TriggerEvent(EventConst.RecollectionCardClick, userCardVo);
                };
            }
            else
            {

                PointerClickListener.Get(gameObject).onClick = go =>
                {
                    FlowText.ShowMessage(I18NManager.Get("Recollection_Hint13"));
                };
                _cardImage.color = Color.gray;
                _cardQualityImage.color = Color.gray;
            }
        }
    }
}