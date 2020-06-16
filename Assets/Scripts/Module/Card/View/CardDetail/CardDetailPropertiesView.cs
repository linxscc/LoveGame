using System;
using DataModel;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace game.main
{
    public class CardDetailPropertiesView : MonoBehaviour
    {
        private Text _cardNameText;
        private Transform _heartContainer;
        private Text _singText;
        private Text _danceText;
        private Text _glamourText;
        private Text _popularityText;
        private Text _originalText;
        private Text _willpowerText;
        private Text _totalText;
        private Image _cardCreditImage;
        private HorizontalLayoutGroup _horizontalLayoutGroup;

        private GameObject _upgradeLevelUpEffect;

        private void Awake()
        {
            _cardNameText = transform.Find("Title/CardCreditImage/Text").GetComponent<Text>();
            _cardCreditImage = transform.Find("Title/CardCreditImage").GetComponent<Image>();
            
            _heartContainer = transform.Find("Title/CardCreditImage/Text/HeartContainer");
            
            _singText = transform.Find("Properties/Sing/Value").GetComponent<Text>();
            _danceText = transform.Find("Properties/Dance/Value").GetComponent<Text>();
            _originalText = transform.Find("Properties/Original/Value").GetComponent<Text>();
            _popularityText = transform.Find("Properties/Popularity/Value").GetComponent<Text>();
            _glamourText = transform.Find("Properties/Glamour/Value").GetComponent<Text>();
            _willpowerText = transform.Find("Properties/Willpower/Value").GetComponent<Text>();
            _horizontalLayoutGroup = transform.Find("Title").GetComponent<HorizontalLayoutGroup>();
            //_horizontalLayoutGroup.enabled=false;
            _totalText = transform.Find("Title/CardCreditImage/Total/Value").GetComponent<Text>();

            _upgradeLevelUpEffect = transform.Find("Title/CardCreditImage/Total/Effect").gameObject;
        }


        int _cardId = -1;
        bool _isOther = false;
        public void SetData(UserCardVo userCardVo, AdditionType type)
        {
            Debug.LogError(type);
            if(userCardVo == null)
                return;

            _isOther = _cardId != userCardVo.CardId;
            _cardId = userCardVo.CardId;

            _cardNameText.text = userCardVo.CardVo.CardName;
            var rect = _cardNameText.GetComponent<RectTransform>();
            rect.sizeDelta =new Vector2(_cardNameText.preferredWidth,rect.sizeDelta.y);
            SetCredit(userCardVo.CardVo);
            SetProperties(userCardVo, type);
            SetStars(userCardVo);

            //这个组件必须要在UI全部渲染完后的第二帧后才能激活使用，第一帧就使用的话会不生效。
//            if (_horizontalLayoutGroup.enabled == false)
//            {
//                ClientTimer.Instance.DelayCall(() =>
//                {
//                    _horizontalLayoutGroup.enabled=true;
//                },0.1f);
// 
//            }
        }

        private void SetCredit(CardVo vo)
        {
            string spName = "UIAtlas_Common_newR";
            if (vo.Credit == CreditPB.Ssr)
            {
                spName = "UIAtlas_Common_newSSR";
            }
            else if(vo.Credit == CreditPB.Sr)
            {
                spName = "UIAtlas_Common_newSR";
            }
            _cardCreditImage.sprite = AssetManager.Instance.GetSpriteAtlas(spName);
            //_cardCreditImage.SetNativeSize();
        }

        public void SetProperties(UserCardVo userCardVo, AdditionType type)
        {
            CardVo vo = userCardVo.CardVo;

            CardAdditionVo curAdditionVo = userCardVo.CurLevelInfo;
            CardAdditionVo  additionVo = userCardVo.NextLevelInfo;
            int addsing=0;
            int adddance=0;
            int addoriginal=0;
            int addpopularity=0;
            int addglamour=0;
            int addwillpower=0;
            
            if (type==AdditionType.Level)
            {
                addsing = (additionVo.SingingAdditon - curAdditionVo.SingingAdditon);
                adddance = (additionVo.DancingAdditon - curAdditionVo.DancingAdditon);
                addoriginal = (additionVo.OriginalAdditon - curAdditionVo.OriginalAdditon);
                addpopularity = (additionVo.PopularityAdditon - curAdditionVo.PopularityAdditon);
                addglamour = (additionVo.GlamourAdditon - curAdditionVo.GlamourAdditon);
                addwillpower = (additionVo.WillpowerAdditon - curAdditionVo.WillpowerAdditon);
            }
            
            if (type == AdditionType.Star)
            {           
                addsing = userCardVo.NextStarInfo.SingingAdditon-userCardVo.CurStarInfo.SingingAdditon;
                adddance = userCardVo.NextStarInfo.DancingAdditon-userCardVo.CurStarInfo.DancingAdditon;
                addoriginal = userCardVo.NextStarInfo.OriginalAdditon-userCardVo.CurStarInfo.OriginalAdditon;
                addpopularity =userCardVo.NextStarInfo.PopularityAdditon-userCardVo.CurStarInfo.PopularityAdditon;
                addglamour = userCardVo.NextStarInfo.GlamourAdditon-userCardVo.CurStarInfo.GlamourAdditon;
                addwillpower = userCardVo.NextStarInfo.WillpowerAdditon-userCardVo.CurStarInfo.WillpowerAdditon;
                
            }
            else if (type == AdditionType.Evolution)
            {       
                
                
                addsing = userCardVo.EvoInfoAddition.SingingAdditon-userCardVo.CurEvolutionInfo.SingingAdditon;
                adddance = userCardVo.EvoInfoAddition.DancingAdditon-userCardVo.CurEvolutionInfo.DancingAdditon;
                addoriginal = userCardVo.EvoInfoAddition.OriginalAdditon-userCardVo.CurEvolutionInfo.OriginalAdditon;
                addpopularity =userCardVo.EvoInfoAddition.PopularityAdditon-userCardVo.CurEvolutionInfo.PopularityAdditon;
                addglamour = userCardVo.EvoInfoAddition.GlamourAdditon-userCardVo.CurEvolutionInfo.GlamourAdditon;
                addwillpower = userCardVo.EvoInfoAddition.WillpowerAdditon-userCardVo.CurEvolutionInfo.WillpowerAdditon;
                
            }

            //正确公式应该是vo.sing*groth+pb.power
            //进化3之前都要显示加号！
          
            
            if (addsing>0&&(int)userCardVo.Evolution<vo.GetMaxEvoTimes())//userCardVo.Level<=userCardVo.CardVo.MaxLevel&&userCardVo.Evolution!=EvolutionPB.Evo1&&
            {
                //Debug.LogError(type+" vo.singing "+vo.Singing+" curAdditionVo "+curAdditionVo.SingingAdditon+" additionVo "+additionVo.SingingAdditon);
                _singText.text = userCardVo.Singing+"  +"+ addsing;
                _danceText.text = userCardVo.Dancing + "  +" + adddance;
                _originalText.text = userCardVo.Original + "   +" + addoriginal;//-curAdditionVo.OriginalAdditon);
                _popularityText.text = userCardVo.Popularity + "   +" + addpopularity;//-curAdditionVo.PopularityAdditon);
                _glamourText.text = userCardVo.Glamour + "   +" + addglamour;//-curAdditionVo.GlamourAdditon);
                _willpowerText.text =userCardVo.Willpower + "   +" + addwillpower; //-curAdditionVo.WillpowerAdditon);
            }
            else
            {
                //Debug.LogError(type+" vo.singing "+vo.Singing+" curAdditionVo "+curAdditionVo.SingingAdditon+" additionVo "+additionVo.SingingAdditon);
                _singText.text = userCardVo.Singing.ToString();//+"  +"+additionVo.SingingAdditon;//(vo.Singing+ curAdditionVo.SingingAdditon )+ "   +" + (additionVo.SingingAdditon-curAdditionVo.SingingAdditon);
                _danceText.text = userCardVo.Dancing.ToString();// + "  +" + additionVo.DancingAdditon; //(vo.Dancing + curAdditionVo.DancingAdditon)+"   +" + (additionVo.DancingAdditon-curAdditionVo.DancingAdditon);
                _originalText.text = userCardVo.Original.ToString();//+ "   +" + additionVo.OriginalAdditon;//-curAdditionVo.OriginalAdditon);
                _popularityText.text = userCardVo.Popularity.ToString();// + "   +" + additionVo.PopularityAdditon;//-curAdditionVo.PopularityAdditon);
                _glamourText.text = userCardVo.Glamour.ToString();// + "   +" + additionVo.GlamourAdditon;//-curAdditionVo.GlamourAdditon);
                _willpowerText.text = userCardVo.Willpower.ToString();// + "   +" + additionVo.WillpowerAdditon; //-curAdditionVo.WillpowerAdditon);
            }

            int cur = userCardVo.Singing + userCardVo.Dancing
                + userCardVo.Original + userCardVo.Popularity
                + userCardVo.Glamour + userCardVo.Willpower;
            if(_isOther)
            {
                oldNum =  cur;
                newNum = cur;
                setTotal(cur);
            }

            newNum = cur;
            setTotal(newNum);

            if (newNum > oldNum)
            {
                ShowUpgradeEffect();
            }
        }
        int newNum = 0;
        int oldNum = 0;

        float time = 0.6f;
        float curConsume = 0;
        private void Update()
        {
            //改用动画
            //int offset = newNum - oldNum;
            //if (offset != 0)
            //{
            //    curConsume += Time.deltaTime;
            //    if (curConsume > time)
            //    {
            //        curConsume = time;
            //    }
            //    float cur = offset * (curConsume / time);
            //    int show = (int)cur;
            //    setTotal(oldNum+show);
            //    if (curConsume == time)
            //    {
            //        curConsume = 0;
            //        oldNum = newNum;
            //    }
            //}
        }

        void setTotal(int total)
        {
            _totalText.text = total.ToString();
        }


        private void SetStars(UserCardVo userCardVo)
        {
            //先播放特效后打开试试！
            
            
            for (int i = 0; i < _heartContainer.childCount; i++)
            {
                var heartroot=_heartContainer.GetChild(i);
                heartroot.gameObject.SetActive(i < userCardVo.MaxStars);
                GameObject redheart = heartroot.Find("Light").gameObject;
                redheart.SetActive(i < userCardVo.Star);  


//                Image img = _heartContainer.GetChild(i).GetComponent<Image>();
//                //float alpha = 0.5f;
//                img.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Card_whiteHeart");
//                if (i < userCardVo.Star)
//                {
//                   // alpha = 1.0f;
//                    img.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Card_redHeart");
//                }
//                //img.color = new Color(img.color.r, img.color.g, img.color.b, alpha);
//                img.gameObject.SetActive(i < userCardVo.MaxStars);
            }
            
        }

        public void ShowUpgradeEffect()
        {
            CancelInvoke("HideUpgradeEffect");
            _upgradeLevelUpEffect.gameObject.SetActive(true);
            Invoke("HideUpgradeEffect", 1.667f);

            _totalText.gameObject.SetActive(false);
            //Debug.LogWarning("ShowUpgradeEffect-----:"+newNum + " oldNum:"+oldNum);
            _upgradeLevelUpEffect.transform.Find("Text_01").GetComponent<Text>().text = oldNum.ToString();
            _upgradeLevelUpEffect.transform.Find("Text_02").GetComponent<Text>().text = newNum.ToString();
            oldNum = newNum;
        }

        private void HideUpgradeEffect()
        {
            _totalText.gameObject.SetActive(true);
            _upgradeLevelUpEffect.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            CancelInvoke("HideUpgradeEffect");
            HideUpgradeEffect();
        }
    }

    public enum AdditionType
    {
        Level,
        Star,
        Evolution
    }
}