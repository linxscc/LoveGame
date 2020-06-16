using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Common;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Module.Recollection.View
{
    public class RecollectionCardItem : MonoBehaviour, IPointerClickHandler
    {
        private Image _cardQualityImage;
        private UserCardVo _data;

        public void SetData(UserCardVo vo)
        {
            _data = vo;

            transform.Find("NameText").GetComponent<Text>().text = vo.CardVo.CardName;
            //transform.Find("QualityBg/LevelText").GetComponent<Text>().text = "Lv." + vo.Level;
            transform.Find("QualityBg/LevelText").GetComponent<Text>().text = I18NManager.Get("Recollection_Level", vo.Level);
            
            Transform heartBar = transform.Find("QualityBg/HeartBar");
           
            _cardQualityImage = transform.Find("CardQualityImage").GetComponent<Image>();
            _cardQualityImage.sprite = AssetManager.Instance.GetSpriteAtlas(CardUtil.GetNewCreditSpritePath(vo.CardVo.Credit));
            _cardQualityImage.SetNativeSize();
            _cardQualityImage.GetComponent<RectTransform>().SetWidth(149.6f);
            _cardQualityImage.GetComponent<RectTransform>().SetHeight(80.0f);

            transform.Find("Times/Text").GetComponent<Text>().text = I18NManager.Get("Recollection_CardPropItemNum",  vo.RecollectionCount);
            for (int i = 1; i < 6; i++)
            {
//                RawImage item = heartBar.GetChild(4-i).GetComponent<RawImage>();
//                float alpha = 0.5f;
//                if (vo.Star > i)
//                {
//                    alpha = 1.0f;
//                }
//                item.color = new Color(item.color.r,item.color.g,item.color.b,alpha);
//                item.gameObject.SetActive(i < vo.MaxStars);
                
                var heartroot=heartBar.GetChild(i);
                heartroot.gameObject.SetActive(i-1 < vo.MaxStars);
                GameObject redheart = heartroot.Find("RedHeart").gameObject;
                redheart.SetActive(i-1 < vo.Star);  
            }

            RawImage cardImage = transform.Find("Mask/CardImage").GetComponent<RawImage>();
            Texture texture = ResourceManager.Load<Texture>(vo.CardVo.MiddleCardPath(vo.UserNeedShowEvoCard()));

            if (texture == null)
            {
                texture = ResourceManager.Load<Texture>(vo.CardVo.MiddleCardPath());
            }

            cardImage.texture = texture;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            EventDispatcher.TriggerEvent(EventConst.RecollectionCardClick, _data);
        }
    }
}