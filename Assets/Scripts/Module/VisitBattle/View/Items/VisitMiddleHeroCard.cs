using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using game.tools;
using Module.VisitBattle.Data;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{

    public class VisitMiddleHeroCard : MonoBehaviour
    {

        private RawImage _cardImage;
        private VisitBattleUserCardVo _data;
        private Image _cardQualityImage;
        private Transform _container;
        private Transform _emptyBg;

        private void Awake()
        {
            _cardImage = transform.Find("Container/Mask/CardImage").GetComponent<RawImage>();
            _cardQualityImage = transform.Find("Container/CardQualityImage").GetComponent<Image>();
            var _cardQualityRect = _cardQualityImage.GetComponent<RectTransform>();
            _cardQualityRect.SetWidth(149.6f);
            _cardQualityRect.SetHeight(80.0f);
            _emptyBg = transform.Find("EmptyImage");

            _container = transform.Find("Container");
            _container.gameObject.Hide();
        }


        public void InitCard(int index, bool isOpen, List<ChallengeCardNumRulePB> cardNumRules)
        {
            Text text = _emptyBg.Find("Bg/Text").GetComponent<Text>();
            Transform bg = _emptyBg.Find("Bg");
            if (isOpen)
            {
                bg.gameObject.Hide();
            }
            else
            {
                bg.gameObject.Show();
                text.text = I18NManager.Get("Battle_LevelOpen", cardNumRules[index].LevelMin);
            }
        }

        public VisitBattleUserCardVo GetData()
        {
            return _data;
        }

        public void SetData(VisitBattleUserCardVo battleUserCardVo)
        {
            if (battleUserCardVo == null)
            {
                _data = null;
                _container.gameObject.Hide();
                _emptyBg.gameObject.Show();
                return;
            }

            UserCardVo vo = battleUserCardVo.UserCardVo;

            _emptyBg.gameObject.Hide();
            _container.gameObject.Show();

            _data = battleUserCardVo;

            transform.Find("Container/QualityBg/LevelText").GetComponent<Text>().text = "Lv." + vo.Level;
            Transform heartBar = transform.Find("Container/QualityBg/HeartBar");

            _cardQualityImage.sprite =
                AssetManager.Instance.GetSpriteAtlas(CardUtil.GetNewCreditSpritePath(vo.CardVo.Credit));
            //_cardQualityImage.SetNativeSize();

            for (int i = 1; i < 6; i++)
            {
//                RawImage item = heartBar.GetChild(4 - i).GetComponent<RawImage>();
//                float alpha = 0.5f;
//                if (vo.Star > i)
//                {
//                    alpha = 1.0f;
//                }
//
//                item.color = new Color(item.color.r, item.color.g, item.color.b, alpha);
//                item.gameObject.SetActive(i < vo.MaxStars);
                var heartroot=heartBar.GetChild(i);
                heartroot.gameObject.SetActive(i-1 < vo.MaxStars);
                GameObject redheart = heartroot.Find("redHeart").gameObject;
                redheart.SetActive(i-1 < vo.Star);  
            }

            Texture texture = ResourceManager.Load<Texture>(vo.CardVo.MiddleCardPath(vo.UserNeedShowEvoCard()), ModuleConfig.MODULE_VISITBATTLE);

            if (texture == null)
            {
                Debug.LogError(vo.CardVo.MiddleCardPath(vo.UseEvo == EvolutionPB.Evo1));
                texture = ResourceManager.Load<Texture>(vo.CardVo.MiddleCardPath(), ModuleConfig.MODULE_VISITBATTLE);
            }

            _cardImage.texture = texture;
        }
    }
}