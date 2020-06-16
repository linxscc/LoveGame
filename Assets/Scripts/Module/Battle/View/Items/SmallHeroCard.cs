using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Common;
using game.tools;
using Module.Battle.Data;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class SmallHeroCard : MonoBehaviour
    {
        private RawImage _cardImage;
        private Image _cardQualityImage;
        private Text _nameText;
        private Text _levelText;
        private Transform _heartBar;
        private CanvasGroup _cg;
        private BattleUserCardVo _battleUserCardVo;

        private void Awake()
        {
            _cardImage = transform.Find("Mask/CardImage").GetComponent<RawImage>();
            _cardQualityImage = transform.Find("CardQualityImage").GetComponent<Image>();
            _heartBar = transform.Find("HeartBar/herat");
            _nameText = transform.Find("NameText").GetComponent<Text>();
            _levelText = transform.Find("LevelText").GetComponent<Text>();
            _cg = GetComponent<CanvasGroup>();

            PointerClickListener.Get(gameObject).onClick = OnClick;
        }

        private void OnClick(GameObject go)
        {
            EventDispatcher.TriggerEvent(EventConst.SmallHeroCardClick, _battleUserCardVo);
        }

        public void SetData(BattleUserCardVo battleUserCardVo)
        {
            _battleUserCardVo = battleUserCardVo;
            UserCardVo vo = battleUserCardVo.UserCardVo;

            Texture texture = ResourceManager.Load<Texture>(vo.CardVo.SmallCardPath(vo.UserNeedShowEvoCard()), ModuleConfig.MODULE_BATTLE);
            if (texture == null)
            {
                //Debug.LogError(vo.CardVo.SmallCardPath(vo.UseEvo==EvolutionPB.Evo1));
                texture = ResourceManager.Load<Texture>(vo.CardVo.SmallCardPath(), ModuleConfig.MODULE_BATTLE);
            }

            _cardImage.texture = texture;
            
            _cardQualityImage.sprite = HeroCardUtil.GetQualityImage(vo.CardVo.Credit);
            //_cardQualityImage.SetNativeSize();

            _nameText.text = vo.CardVo.CardName;
            _levelText.text = "Lv." + vo.Level;
            
            for (int i = 0; i < _heartBar.childCount; i++)
            {
//                RawImage heart = _heartBar.GetChild(i).GetComponent<RawImage>();
                   //                if (vo.Star > i)
                   //                {
                   //                    heart.color = new Color(heart.color.r, heart.color.g, heart.color.b, 1.0f);
                   //                }
                   //                else
                   //                {
                   //                    heart.color = new Color(heart.color.r, heart.color.g, heart.color.b, 0.5f);
                   //                }
                var heartroot=_heartBar.GetChild(i);
                heartroot.gameObject.SetActive(i < vo.MaxStars);
                GameObject redheart = heartroot.Find("redHeart").gameObject;
                redheart.SetActive(i < vo.Star);  
            }

            _cg.alpha = battleUserCardVo.IsUsed ? 0.6f : 1.0f;
        }
    }
}