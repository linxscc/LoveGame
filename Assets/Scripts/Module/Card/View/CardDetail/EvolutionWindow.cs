using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Services;
using Com.Proto;
using Common;
using DataModel;
using DG.Tweening;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace game.main
{
    public class EvolutionWindow : Window
    {
        private Text _title;
        private RawImage _smallCard;
        private Text _ruletext;
        private RawImage _ruleImage;
        private Button _evolutionBtn;
        private Transform _pointGroup;
        private Transform _propGroup;
        private Transform _arrow;
        private int _curPageIndex=1;
        private Text CostNum;
        private UserCardVo _vo;
        private Text _evoeffecttxt;
        private Text _unlockText;
        private int _maxevotimes;
        private CardEvoRulePB _cardEvoRulePb;

//        private Transform _ruleTran;
//        private Transform _tips;
//        private Text _ruleText;
//        private Button _tipsBG;

        private Button _chooseWindowBG;
        private Button _chooseOkBtn;
        private Toggle _PropToggle;
        private Toggle _cardResolveToggle;
        private Text _propChooseText;
        private Text _cardChooseText;
        private RawImage _propToggletex;
        private RawImage _headToggletex;
        

        private Text _btnText;
        
        private Vector2 prePressPos;

        private Text _resolveRule;
        private bool _isTrigger;
        private Coroutine _coroutine;

        private Transform _unlockVoceTran;
//        private Transform _smallImageTran;
        private Transform _unlockClothTran;
        private Text _unlockvoiceTips;

        private Image _reloadingItem;
        private Text _clothName;
        private Text _costGoldNum;
        private int aniIndex = 0;
        private Sequence tween;
        private bool canPlayAni=false;
        private int ChooseCardResolve = 0;
        
        
        
        

        private void Awake()
        {
            _title = transform.Find("Title/Text").GetComponent<Text>();
            _smallCard = transform.Find("SmallImage").GetComponent<RawImage>();
            UIEventListener.Get(_smallCard.gameObject).onDown = OnDown;
            UIEventListener.Get(_smallCard.gameObject).onUp = OnUp;
            
            _resolveRule = transform.Find("RuleText").GetComponent<Text>();
            _ruleImage = transform.Find("RuleText/Image").GetComponent<RawImage>();
            _evolutionBtn = transform.Find("EvoBtn").GetComponent<Button>();
            _btnText = transform.Find("EvoBtn/Btntex").GetComponent<Text>();
            _evolutionBtn.onClick.AddListener(OnEvolutionClick);
            _propGroup = transform.Find("PropBg");
            _pointGroup = transform.Find("TapPoint");
            _arrow = transform.Find("Arrow");
            CostNum = transform.Find("SmallImage/NumBg/Text").GetComponent<Text>();
            _evoeffecttxt = transform.Find("EvoEffect").GetComponent<Text>();
            _unlockText = transform.Find("UnLockText").GetComponent<Text>();
//            _ruletext = transform.Find("EvoEffect/Rule/Tips/Text").GetComponent<Text>();
//            _ruleTran = transform.Find("EvoEffect/Rule");
//            _tips = transform.Find("EvoEffect/Rule/Tips");
            _chooseWindowBG = transform.Find("ChoosePropBG").GetButton();
            _chooseOkBtn = transform.Find("ChoosePropBG/Proptogglepanel/OkBtn").GetButton();
            _PropToggle = transform.Find("ChoosePropBG/Proptogglepanel/PropBg/BG").GetComponent<Toggle>();
            _cardResolveToggle = transform.Find("ChoosePropBG/Proptogglepanel/PropBg/BG1").GetComponent<Toggle>();
            _propChooseText = transform.Find("ChoosePropBG/Proptogglepanel/PropBg/BG/Prop/Text").GetText();
            _cardChooseText = transform.Find("ChoosePropBG/Proptogglepanel/PropBg/BG1/Head/Text").GetText();
            _propToggletex = transform.Find("ChoosePropBG/Proptogglepanel/PropBg/BG/Prop").GetRawImage();
            _headToggletex = transform.Find("ChoosePropBG/Proptogglepanel/PropBg/BG1/Head").GetRawImage();
            
            _chooseWindowBG.onClick.AddListener(() =>
            {
                _chooseWindowBG.gameObject.Hide();
            });
            _chooseWindowBG.gameObject.Hide();
            
            _chooseOkBtn.onClick.AddListener(() =>
            {
               // Debug.LogError("选择道具种类！");
                //是否直接进化？！
                
                _chooseWindowBG.gameObject.Hide(); 
            });
            
            _PropToggle.onValueChanged.AddListener(ChooseEvoProp);
            _cardResolveToggle.onValueChanged.AddListener(ChooseEvoProp);
            
            

            _unlockVoceTran = transform.Find("UnLockVoice");
//            _smallImageTran = transform.Find("");
            _unlockClothTran = transform.Find("UnlockCloth");
            

            _btnText.text = I18NManager.Get("Card_CanEvolution");
            //Get到了新的知识点：父子节点是不存在遮挡前后的，但是同一父级的话存在遮挡关系的
//            _tipsBG = transform.Find("EvoEffect/Rule/Tips/BigBG").GetButton();
            _unlockvoiceTips = transform.Find("UnLockVoice/Text").GetText();
            _reloadingItem = transform.Find("UnlockCloth/Cloth/ReloadingItem/Icon").GetImage();
            _clothName = transform.Find("UnlockCloth/Cloth/ReloadingItem/InfoText").GetText();
            _costGoldNum = transform.Find("CostGold/Num").GetText();

//            _tipsBG.onClick.AddListener(() =>
//            {
//                _tips.gameObject.Hide();
//            });
            
            for (int i = 0; i < _pointGroup.childCount; i++)
            {
                _pointGroup.GetChild(i).gameObject.Hide();
            }
            
            PointerClickListener.Get(_arrow.GetChild(1).gameObject).onClick = go =>
            {
                //3应该也是个变量！
                if (_curPageIndex>=0&&_curPageIndex<_maxevotimes)
                {
                    _curPageIndex++;
                    SetPageData(_vo);
                }
            };
            
            PointerClickListener.Get(_arrow.GetChild(0).gameObject).onClick = go =>
            {

                if (_curPageIndex<=_maxevotimes&&_curPageIndex>0)
                {
                    _curPageIndex--;
                    SetPageData(_vo);
                }
            };
            _btnText.text = I18NManager.Get("Card_CanEvolution");
        }

        private void ChooseEvoProp(bool isOn)
        {
            if (isOn==false)
                return;

            if (EventSystem.current.currentSelectedGameObject==null)
            {
                    return;
            }
            
            string tapname = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log("OnTabChange===>" + tapname);

            //ChangeColor(name);

            switch (tapname)
            {
                case "BG":
                    //回溯
                    ChooseCardResolve = 0;
                    break;
                case "BG1":
                    //选择卡
                    if (_vo.Num==1)
                    {
                        SetToggleState(false);
                        //Debug.LogError("?!");
                        FlowText.ShowMessage(I18NManager.Get("Card_CardNumDontEnough"));
                        return;
                    }
                    
                    ChooseCardResolve = 1;
                    break;

                        
            }
        }

        public void SetData(UserCardVo vo)
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            
            _vo = vo;
            //最多进化几次的限制！圆点的限制
            _maxevotimes = vo.CardVo.GetMaxEvoTimes();
            for (int i = 0; i < _maxevotimes; i++)
            {
                _pointGroup.GetChild(i).gameObject.Show();
            }

            _isTrigger = false;
            SetFitPage(_maxevotimes,vo);
            SetPageData(vo);
                    
        }

        private void SetFitPage(int maxevoTime,UserCardVo vo)
        {
            for (int i = 1; i < maxevoTime+1; i++)
            {
                var cardevorulepb = GlobalData.CardModel.GetCardEvoRule(vo.CardVo.Credit, vo.CardVo.Player,(EvolutionPB)(i));
                if (vo.Evolution==cardevorulepb.Evo-1&&vo.Star>=(int)cardevorulepb.StarNeed)
                {
                    _curPageIndex = i;
                    break;
                }
            }
        }
        
        private void SetPageData(UserCardVo vo)
        {
//            _tips.gameObject.Hide();
            var cardevorulepb = GlobalData.CardModel.GetCardEvoRule(vo.CardVo.Credit, vo.CardVo.Player,(EvolutionPB)(_curPageIndex));
            
//            _ruleTran.gameObject.SetActive(_curPageIndex==1&&vo.CardVo.Credit!=CreditPB.R);
            _cardEvoRulePb = cardevorulepb;
            _title.text = I18NManager.Get("Card_EvolutionTo") + ShowIndex(_curPageIndex);
            SetArrowShow((int)cardevorulepb.Evo);            
            var costReduceNum = GlobalData.CardModel.GetCardResolveRule(vo.CardVo.Credit, vo.CardVo.Player).EvoResolve[10000 + (int) vo.CardVo.Player];
            _resolveRule.text = I18NManager.Get("Card_GetResolveItem",costReduceNum); //$"一张相同星缘可抵扣<color=#9769AC>{costReduceNum}</color>";
            _ruleImage.texture=ResourceManager.Load<Texture>(GlobalData.PropModel.GetPropStrPath(vo.EvolutionRequirePropId().ToString()), ModuleConfig.MODULE_CARD);
            _smallCard.texture = ResourceManager.Load<Texture>(vo.CardVo.MiddleCardPath(true),ModuleConfig.MODULE_CARD);
            CostNum.text = (vo.Num-1)+"/"+cardevorulepb.UseCardNum;//可抵消星缘的数量
            _costGoldNum.text = I18NManager.Get("Card_CostEvoGoldNeed",cardevorulepb.GoldNeed);
            int reduceCardNum = 0;
            if (vo.Num-1>=cardevorulepb.UseCardNum)
            {
                reduceCardNum = cardevorulepb.UseCardNum;
            }
            else
            {
                reduceCardNum = vo.Num - 1;
            }

            SetEffectRule(cardevorulepb);
            //SetRuleInfo();
            SetCardEvoInfoState(_curPageIndex);


            for (int j = 0; j < _propGroup.childCount; j++)
            {
                _propGroup.GetChild(j).gameObject.Hide();
            }

            bool isCrystalSatisfy = false;
            bool isPropSatisfy = false;
            
            int i = 0;
            int propIndex = 0;
            bool enableAni = false;

            foreach (var v in cardevorulepb.Consume)
            {
                var propitem = GlobalData.PropModel.GetUserProp(v.Key);
                if (propitem==null)
                {
                    continue;
                }
                else
                {
                    _propGroup.GetChild(i).GetChild(0).GetComponent<RawImage>().texture = ResourceManager.Load<Texture>(GlobalData.PropModel.GetPropStrPath(v.Key.ToString()),ModuleConfig.MODULE_CARD);
                }

                _propGroup.GetChild(i).gameObject.Show();
                //要扣去可抵消的卡牌数量，要先确定这个是不是卡牌
                if (propitem.ItemId>=10001&&propitem.ItemId<=10004)
                {
                    _propGroup.GetChild(i).GetChild(0).Find("Text").GetComponent<Text>().text = propitem.Num + "/" +v.Value;// (v.Value-costReduceNum*reduceCardNum)
                    isCrystalSatisfy = propitem.Num >= v.Value - costReduceNum * reduceCardNum;
                    _propGroup.GetChild(0).GetChild(1).GetComponent<RawImage>().texture=ResourceManager.Load<Texture>(_vo.CardAppointmentRuleVo.SmallPicPath,ModuleConfig.MODULE_CARD);
                    //开始dotween动画！
                    enableAni = true;
                }
                else
                {
                    propIndex++;
                    _propGroup.GetChild(i).GetChild(0).Find("Text").GetComponent<Text>().text = propitem.Num + "/" + v.Value;  
                    if(propIndex == 1)
                    {
                        isPropSatisfy = propitem.Num >= v.Value;
                    }
                    else if(propIndex == 2)
                    {
                        isPropSatisfy = propitem.Num >= v.Value & isPropSatisfy;
                    }
                }
              

                PointerClickListener.Get(_propGroup.GetChild(i).gameObject).onClick = go =>
                {
                    //道具来源
                    //BUG，道具来源不值一种啊！

                    if (propitem.ItemId>=10001&&propitem.ItemId<=10004)
                    {                                            
                       // FlowText.ShowMessage(I18NManager.Get("Card_GetByResolve"));
                        SetToggleState(vo.Num-1>0);
                        SetTogglePropData(propitem.Num + "/" + v.Value, (vo.Num-1)+"/"+cardevorulepb.UseCardNum,GlobalData.PropModel.GetPropStrPath(v.Key.ToString()),_vo.CardAppointmentRuleVo.SmallPicPath);
                        _chooseWindowBG.gameObject.Show();
                    }
                    else
                    {
                        FlowText.ShowMessage(I18NManager.Get("Card_GetByVisit"));
                        
                    }

                };
                
                i++;
            }
            
            if (enableAni)
            {
                if (!canPlayAni)
                {
                    canPlayAni = true;
                    ChangePropAni();
                }
            }
            else
            {
                tween.Kill();
                ResetPropBg();
            }


            if (_isTrigger == false && vo.Evolution == cardevorulepb.Evo - 1 && vo.Star >= (int) cardevorulepb.StarNeed)
            {
                _isTrigger = true;

                _coroutine = ClientTimer.Instance.DelayCall(() =>
                {
                    if (_curPageIndex == 3)
                    {
                        //结晶满足, 进化道具不满足
                        int id = (7014 + (int) vo.CardVo.Player - 1);
                        if (isCrystalSatisfy && isPropSatisfy == false &&
                            GlobalData.RandomEventModel.CheckTrigger(id))
                        {
                            new TriggerService().Request(id).ShowNewGiftWindow().Execute();
                        }
                    }
                    else if(_curPageIndex == 2)
                    {
                        int id2 = (7010 + (int) vo.CardVo.Player - 1);
                        //道具满足，但是结晶不满足
                        if (isCrystalSatisfy == false && isPropSatisfy && GlobalData.RandomEventModel.CheckTrigger(id2))
                        {
                            new TriggerService().Request(id2).ShowNewGiftWindow().Execute();
                        }
                    }
                    
                }, 3);
            }


            SetUnLockTextInfo(cardevorulepb);

            SetEvoState(cardevorulepb);

            if (vo.Num > 1)
            {
                ChooseCardResolve = 1;
            }
            else
            {
                ChooseCardResolve = 0;
            }

        }

        private void ChangePropAni()
        {
            if (!canPlayAni)
            {
                return;
            }
            
            var curBg=  _propGroup.GetChild(0).GetChild(aniIndex).GetComponent<RawImage>();
            RawImage nextBg;

            //Debug.LogError(aniIndex);
            if (aniIndex==1)
            {
                nextBg =_propGroup.GetChild(0).GetChild(0).GetComponent<RawImage>();
            }
            else
            {
                nextBg =_propGroup.GetChild(0).GetChild(1).GetComponent<RawImage>();
            }
                
            Tween curBgAlpha = curBg.DOColor(new Color(curBg.color.r,curBg.color.g,curBg.color.b,0),1f );
            Tween nextBgAlpha = nextBg.DOColor(new Color(nextBg.color.r,nextBg.color.g,nextBg.color.b,1),1f );
            
//            Tween curBgAlpha1 = curBg.DOColor(new Color(curBg.color.r,curBg.color.g,curBg.color.b,0),1f );
//            Tween nextBgAlpha1 = nextBg.DOColor(new Color(nextBg.color.r,nextBg.color.g,nextBg.color.b,1),1f );


            tween = DOTween.Sequence()
                .Join(curBgAlpha)
                .Join(nextBgAlpha);

//            tween.SetAutoKill(false);
//            tween.SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);

            tween.onComplete = () =>
            {
                if (!canPlayAni)
                {
                    return;
                }
                aniIndex = aniIndex==1 ? 0 : 1;
                ClientTimer.Instance.DelayCall(ChangePropAni, 2f);
            };   
        }

        private void ResetPropBg()
        {
            canPlayAni = false;
            aniIndex = 0;
            var curBg=  _propGroup.GetChild(0).GetChild(0).GetComponent<RawImage>();
            var nextBg =_propGroup.GetChild(0).GetChild(1).GetComponent<RawImage>();
            curBg.color=new Color(curBg.color.r,curBg.color.g,curBg.color.b,1);
            nextBg.color = new Color(nextBg.color.r, nextBg.color.g, nextBg.color.b, 0);
//            Debug.LogError("Reset");
        }

        private void SetCardEvoInfoState(int page)
        {
            switch (page)
            {
                case 1:
                    _smallCard.gameObject.Hide();
                    _unlockClothTran.gameObject.Hide();
                    _unlockVoceTran.gameObject.SetActive(_vo.CardVo.Credit!=CreditPB.R);
                    if (_vo.CardVo.Credit!=CreditPB.R)
                    {
                        _unlockvoiceTips.text = GlobalData.DiaryElementModel.GetDialogByCardId(_vo.CardId).Name; 
                    }

                    break;
                case 2:
                    _smallCard.gameObject.Show();
                    _unlockClothTran.gameObject.Hide();
                    _unlockVoceTran.gameObject.Hide();        
                    break;
                case 3:
                    _smallCard.gameObject.Hide();
                    _unlockClothTran.gameObject.Show();
                    _unlockVoceTran.gameObject.Hide();  
                    var unlockpb = GlobalData.FavorabilityMainModel.IsUnlockCloth(_vo.CardId);
                    //Debug.LogError(unlockpb);
                    if (unlockpb != null)
                    {
                        _reloadingItem.sprite = ResourceManager.Load<Sprite>("Prop/" + unlockpb.ItemId);//AssetManager.Instance.GetSpriteAtlas("UIAtlas_ReloadingClothing_"+unlockpb.ItemId);//vo.IconPath
                        _clothName.text = GlobalData.PropModel.GetPropBase(unlockpb.ItemId).Name;
                    }
                    else
                    {
                        _reloadingItem.sprite = null;
                        _clothName.text = I18NManager.Get("Common_Expect");
                    }
                    break;
                default:
                    break;
            }
            
        }
        
        private void SetRuleInfo()
        {
//            if (_vo.CardVo.Credit!=CreditPB.R)
//            {
//                _ruletext.text = _curPageIndex==1?I18NManager.Get("Card_UnLockTapVoice",GlobalData.DiaryElementModel.GetDialogByCardId(_vo.CardId).Name,GlobalData.FavorabilityMainModel.GetPlayerName(_vo.CardVo.Player)):""; 
//            }

//            PointerClickListener.Get(_ruleTran.gameObject).onClick = go =>
//            {
//                _tips.gameObject.Show();
//            };
        }

        private void SetUnLockTextInfo(CardEvoRulePB cardevorulepb)
        {
            _unlockText.text = I18NManager.Get("Card_UnLockStar",(int) cardevorulepb.StarNeed+1);//"星缘" + (int) cardevorulepb.StarNeed + "心解锁";
            _unlockText.gameObject.SetActive(_vo.Star<(int)cardevorulepb.StarNeed||_vo.Evolution<cardevorulepb.Evo);
        }
        
        private void SetEffectRule(CardEvoRulePB cardevorulepb)
        {
            if (_curPageIndex!=3)
            {
                _evoeffecttxt.text = I18NManager.Get("Card_EvoEffect",GlobalData.CardModel.CardEvoPowerUp(cardevorulepb))+cardevorulepb.Desc;
            }
            else
            {
                var unlockpb = GlobalData.FavorabilityMainModel.IsUnlockCloth(_vo.CardId);
                if (unlockpb!=null)
                {
                    _evoeffecttxt.text = I18NManager.Get("Card_EvoEffect",GlobalData.CardModel.CardEvoPowerUp(cardevorulepb))+cardevorulepb.Desc;  
                }
                else
                {
                    _evoeffecttxt.text = I18NManager.Get("Card_EvoEffectTemp",GlobalData.CardModel.CardEvoPowerUp(cardevorulepb));  
                }
            }
        }

        private void SetEvoState(CardEvoRulePB cardevorulepb)
        {
            //按钮状态分三种：1.粉丝已经进化。2.粉色进化。3.不可进化灰色按钮。
            _evolutionBtn.onClick.RemoveAllListeners();
            if (_vo.Evolution>=cardevorulepb.Evo)
            {
                //已经进化。
                _evolutionBtn.image.color = Color.gray;
                _btnText.text = I18NManager.Get("Card_HasEvolution");
                //_evolutionBtn.onClick.AddListener(HasEvo);

            }
            else if(_vo.Evolution==cardevorulepb.Evo-1&&_vo.Star>=(int)cardevorulepb.StarNeed)
            {
                //可以进化
                _evolutionBtn.image.color = Color.white;
                _btnText.text = I18NManager.Get("Card_CanEvolution");
                _evolutionBtn.onClick.AddListener(OnEvolutionClick);
            }
            else
            {
                //不可进化
                _evolutionBtn.image.color = Color.gray;
                _btnText.text = I18NManager.Get("Card_CanEvolution");
//                _evolutionBtn.onClick.RemoveListener(HasEvo);
//                _evolutionBtn.onClick.RemoveListener(OnEvolutionClick);
                _evolutionBtn.onClick.AddListener(() =>
                {
                    if (_vo.Evolution!=cardevorulepb.Evo-1)
                    {
                        FlowText.ShowMessage(I18NManager.Get("Card_EvoCondiction")); 
                    }
                    else
                    {
                        FlowText.ShowMessage(I18NManager.Get("Card_StarCondiction"));
                    }
                });
            }
        }
        
        private void SetArrowShow(int curPageIndex)
        {
            //Debug.LogError(curPageIndex);
            switch (curPageIndex)
            {
                case 1:
                    _arrow.GetChild(0).gameObject.Hide();
                    _arrow.GetChild(1).gameObject.SetActive(_maxevotimes>1);
                    SetPointActive(0);    
                    break;

                case 2:
                    //如果是SR的话只能单向了。
                    _arrow.GetChild(0).gameObject.Show();
                    _arrow.GetChild(1).gameObject.SetActive(_maxevotimes>2);
                    SetPointActive(1);
                    break;

                case 3:
                    _arrow.GetChild(1).gameObject.Hide();
                    _arrow.GetChild(0).gameObject.Show();
                    SetPointActive(2);
                    break;
            }
        }

        private void SetPointActive(int index)
        {
            for (int i = 0; i < _pointGroup.childCount; i++)
            {
                _pointGroup.GetChild(i).GetChild(0).gameObject.SetActive(index==i);
            }
        }
        
        private void OnEvolutionClick()
        {
            if ((int)_cardEvoRulePb.StarNeed > _vo.Star)
            {
                //Debug.LogError((int)_cardEvoRulePb.StarNeed+" "+_vo.Star);
                FlowText.ShowMessage(I18NManager.Get("Card_StarCondiction"));
                return;
            }

            EventDispatcher.TriggerEvent(EventConst.CardEvoConfirm,_vo,ChooseCardResolve);
            //SendMessage(new Message(MessageConst.CMD_CARD_CONFIRMEVOLUTION,_vo));
        }

        private void SetTogglePropData(string propnum,string cardresolvenum,string proppath,string headpath)
        {
            _propChooseText.text = propnum;
            _cardChooseText.text = cardresolvenum;
            _propToggletex.texture = ResourceManager.Load<Texture>(proppath);
            _headToggletex.texture = ResourceManager.Load<Texture>(headpath);

        }

        private void SetToggleState(bool hasResolve)
        {
            _PropToggle.onValueChanged.RemoveListener(ChooseEvoProp);
            _cardResolveToggle.onValueChanged.RemoveListener(ChooseEvoProp);
            
            _PropToggle.isOn = !hasResolve;
            _cardResolveToggle.isOn = hasResolve;
            ChooseCardResolve = hasResolve ? 1 : 0;
            
            _PropToggle.onValueChanged.AddListener(ChooseEvoProp);
            _cardResolveToggle.onValueChanged.AddListener(ChooseEvoProp);
            
            
        }

        private string ShowIndex(int idx)
        {
            switch (idx)
            {
                    
                case 1:
                    return I18NManager.Get("Card_Evo1");
                case 2:
                    return I18NManager.Get("Card_Evo2");
                case 3:
                    return I18NManager.Get("Card_Evo3");
                default:
                    //Debug.LogError("NoIdx"+idx);
                    return " ";
            }
        }
        
        private void OnDown(PointerEventData data)
        {
            prePressPos = data.pressPosition;
        }

        private void OnUp(PointerEventData data)
        {
            float dis = (data.position - prePressPos).magnitude;
            bool isRight = (prePressPos.x - data.position.x) > 0 ? true : false;
            if (dis>100)
            {
                ScrollingDisplay(isRight);           
            }

        }

        private void ScrollingDisplay(bool isRight)
        {
            if (isRight)
            {
                if (_curPageIndex>=0&&_curPageIndex<_maxevotimes)
                {
                    _curPageIndex++;
                    SetPageData(_vo);
                }
                
            }
            else
            {
                if (_curPageIndex <= _maxevotimes && _curPageIndex > 1)
                {
                    _curPageIndex--;
                    SetPageData(_vo);
                }       
            }
        }

        private void OnDestroy()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }
    }
}