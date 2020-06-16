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
    public class CollectedCardItem : MonoBehaviour, IPointerClickHandler
    {
        private Image _cardQualityImage;
        private UserCardVo _data;
        private Text _num;
        private Text _name;
        private Text _level;

        private void Awake()
        {
            _num = transform.Find("CardNum").GetText();
            _name = transform.Find("NameText").GetComponent<Text>();
            _level = transform.Find("QualityBg/LevelText").GetComponent<Text>();
        }

        public void SetData(UserCardVo vo)
        {
            _data = vo;
            
            _name.text = vo.CardVo.CardName;
            _level.text = "Lv." + vo.Level;
            _num.text = I18NManager.Get("Card_HasCardNum") + vo.Num;
            Transform heartBar = transform.Find("QualityBg/HeartBar");
           
            _cardQualityImage = transform.Find("CardQualityImage").GetComponent<Image>();
            _cardQualityImage.sprite = AssetManager.Instance.GetSpriteAtlas(CardUtil.GetNewCreditSpritePath(vo.CardVo.Credit));
            //_cardQualityImage.SetNativeSize();

            for (int i = 1; i < 6; i++)
            {
                Transform item = heartBar.GetChild(i);
                var redheart = item.Find("RedHeart");
                redheart.gameObject.SetActive(vo.Star > i-1);

//                float alpha = 0.5f;
//                if (vo.Star > i)
//                {
//                    alpha = 1.0f;
//                }
//                item.color = new Color(item.color.r,item.color.g,item.color.b,alpha);
                item.gameObject.SetActive(i-1 < vo.MaxStars);
            }

            RawImage cardImage = transform.Find("Mask/CardImage").GetComponent<RawImage>();
            Texture texture = ResourceManager.Load<Texture>(vo.CardVo.MiddleCardPath(vo.UserNeedShowEvoCard()), ModuleConfig.MODULE_CARD);

            if (texture == null)
            {
                //Debug.LogError(vo.CardVo.MiddleCardPath(vo.UseEvo==EvolutionPB.Evo1)+" "+vo.UseEvo);
                texture = ResourceManager.Load<Texture>(vo.CardVo.MiddleCardPath(),ModuleConfig.MODULE_CARD);
            }

            cardImage.texture = texture;
            
            transform.Find("RedPoint").gameObject.SetActive(vo.ShowCardDetailRedPoint);

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            EventDispatcher.TriggerEvent<UserCardVo>(EventConst.CollectedCardClick, _data);
        }
    }
}