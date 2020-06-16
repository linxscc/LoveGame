using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using DG.Tweening;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class RewardHeroCard : MonoBehaviour
    {
        private RawImage _cardImage;
        private Image _cardQualityImage;
        private Transform _heartBar;
        private Text _levelText;

        private void Awake()
        {
            _cardImage = transform.Find("Mask/CardImage").GetComponent<RawImage>();
            _cardQualityImage = transform.Find("CardQualityImage").GetComponent<Image>();
            _heartBar = transform.Find("HeartBar");
            _levelText = transform.Find("LevelText").GetComponent<Text>();
        }

        public void SetData(UserCardVo vo, int expAdd)
        {
            ProgressBar progressBar = transform.Find("ProgressBar").GetComponent<ProgressBar>();
            progressBar.Progress = (int)(((float)vo.CurrentLevelExp / vo.NeedExp) * 100);
            
            _cardImage.texture = ResourceManager.Load<Texture>(vo.CardVo.SmallCardPath(vo.UserNeedShowEvoCard()), ModuleConfig.MODULE_BATTLE);
            if (_cardImage.texture==null)
            {
                Debug.LogError(vo.CardVo.SmallCardPath(vo.UseEvo==EvolutionPB.Evo1));
                _cardImage.texture = ResourceManager.Load<Texture>(vo.CardVo.SmallCardPath(), ModuleConfig.MODULE_BATTLE);
            }
            _levelText.text = "Lv." + vo.Level;

            if (vo.Level >= 100)
            {
                transform.Find("ExpAddText").GetComponent<Text>().text = "MAX";
            }
            else
            {
                transform.Find("ExpAddText").GetComponent<Text>().text = "+" + expAdd + " exp";
            }

            for (int i = 1; i < _heartBar.childCount; i++)
            {
//                RawImage star = _heartBar.GetChild(i).GetComponent<RawImage>();
//
//                star.gameObject.SetActive(i < vo.MaxStars);
//                
//                if (vo.Star > i)
//                {
//                    star.color = new Color(star.color.r, star.color.g, star.color.b, 1.0f);
//                }
//                else
//                {
//                    star.color = new Color(star.color.r, star.color.g, star.color.b, 0.5f);
//                }
                var heartroot=_heartBar.GetChild(i);
                heartroot.gameObject.SetActive(i-1 < vo.MaxStars);
                GameObject redheart = heartroot.Find("redHeart").gameObject;
                redheart.SetActive(i -1< vo.Star);  
            }

            _cardQualityImage.sprite = HeroCardUtil.GetQualityImage(vo.CardVo.Credit);
            //_cardQualityImage.SetNativeSize();
        }
    }
}