using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using DataModel;
using game.main;
using System.Collections.Generic;
using Assets.Scripts.Module;
using Com.Proto;
using Common;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FavorabilityPhoneEventView : View
{
    private RawImage _roleImage;
    private Text _rolename;
    private Text _level;
    private Text _title;

    private Text _addFavorability;
    private Text _gotoCardtxt;

    private Button _levelupDisposition;
    private Button _gotocard;
    private Transform _heartContainer;
    private Transform _heartBGContainer;

    private Transform _TriggerState;
    private Transform _finisheAll;
    private Transform _noCard;
    private Text _dispositionTo;
    private Text _startxt;

    private Transform _goleft;
    private Transform _goRight;

    private int curIndex = 0;
    private int jumpIndex = 0;
    
    private List<UserCardVo> _cardlist;

    private Image _credit;

    private UserFavorabilityVo _userFavorabilityVo;
    private UserCardVo _curCardVo;

    private Vector2 prePressPos;

    private void Awake()
    {
        _roleImage = transform.Find("Photo/Mask/StoryImage").GetComponent<RawImage>();
        UIEventListener.Get(_roleImage.gameObject).onDown = OnDown;
        UIEventListener.Get(_roleImage.gameObject).onUp = OnUp;
        
        _rolename = transform.Find("Name").GetComponent<Text>();
        _level = transform.Find("Name/Heart/Text").GetComponent<Text>();
        _title = transform.Find("Title").GetComponent<Text>();
        _TriggerState = transform.Find("State/TriggerEvent");
        _finisheAll = transform.Find("State/UnLock");
        _noCard = transform.Find("State/NOCard");
        _dispositionTo = transform.Find("State/TriggerEvent/Disposition").GetComponent<Text>();
        _startxt = transform.Find("State/TriggerEvent/Star").GetComponent<Text>();
        
        _heartContainer = transform.Find("Photo/HeartContainer");
        _heartBGContainer = transform.Find("Photo/HeartContainerBG");
        
        _addFavorability=transform.Find("State/TriggerEvent/Disposition/Button/text").GetComponent<Text>();
        _gotoCardtxt=transform.Find("State/TriggerEvent/Star/Button/text").GetComponent<Text>();
        
        _levelupDisposition = transform.Find("State/TriggerEvent/Disposition/Button").GetComponent<Button>();
        _gotocard = transform.Find("State/TriggerEvent/Star/Button").GetComponent<Button>();
        _credit = transform.Find("Photo/Credit").GetComponent<Image>();


        _goleft = transform.Find("Arrow/Left");
        _goRight = transform.Find("Arrow/Right");

        _levelupDisposition.onClick.AddListener(LevelUpDisiposition);
        _gotocard.onClick.AddListener(GotoCard);

        PointerClickListener.Get(_goleft.gameObject).onClick = go =>
        {
            curIndex--;
            SetIndexState(curIndex);
            SetCardData(_cardlist[curIndex]);
        };
        PointerClickListener.Get(_goRight.gameObject).onClick = go =>
        {
            curIndex++;
            SetIndexState(curIndex);
            SetCardData(_cardlist[curIndex]);
        };
    }

    private void GotoCard()
    {
        if (_curCardVo==null)
        {
            FlowText.ShowMessage(I18NManager.Get("FavorabilityMain_GUESSStaredge")); //暂无星缘
        }
        else
        {
            jumpIndex = curIndex;
            SendMessage(new Message(MessageConst.CMD_FACORABLILITY_JUMPTOCRADS,_curCardVo));    
        } 
    }

    
    /// <summary>
    /// 跳转到送礼的窗口
    /// </summary>
    private void LevelUpDisiposition()
    {
        if (!GuideManager.IsOpen(ModulePB.Favorability, FunctionIDPB.FavorabilityGifts))
        {       
            string desc = GuideManager.GetOpenConditionDesc(ModulePB.Favorability, FunctionIDPB.FavorabilityGifts);
            FlowText.ShowMessage(desc);
            return;
        }
        jumpIndex = curIndex;
        SendMessage(new Message(MessageConst.MODULE_DISIPOSITION_SHOW_GIVEGIFTS,false));
    }

    public void SetData(UserFavorabilityVo vo)
    {
        if (jumpIndex!=0)
        {
            curIndex = jumpIndex;
        }
        else
        {
            curIndex = 0;  
        }

        jumpIndex = 0;
        
        _userFavorabilityVo = vo;
        _rolename.text = GlobalData.FavorabilityMainModel.GetPlayerName(vo.Player);
        _level.text = vo.Level.ToString();

        _cardlist = GlobalData.CardModel.UserCardList.FindAll(x => { return x.CardVo.Player == vo.Player; });

        _heartContainer.gameObject.SetActive(_cardlist.Count > 0);
        _heartBGContainer.gameObject.SetActive(_cardlist.Count > 0);
        

        //bug
        //没开放的卡牌或者那些没有配好感度数据的卡牌，出现异常的时候先直接给隐藏掉，换下一张！

        if (_cardlist.Count > 0)
        {
            //必须要一开始就设置好状态！
            SetIndexState(curIndex);
            SetCardData(_cardlist[curIndex]);
            _credit.gameObject.SetActive(true);
        }
        else
        {
            //空白的时候，什么都系都要默认为空白
            
            _credit.gameObject.SetActive(false);
            _title.text = "";
            _roleImage.texture = null;
            SetStateTran(false, false, true);
            SetIndexState(-1);
        }
    }


    private void SetCardData(UserCardVo vo)
    {
        SetStateTran(true,false,false);
        _curCardVo = vo;
        _title.text = vo.CardVo.TitleName;
        _roleImage.texture = ResourceManager.Load<Texture>(vo.CardVo.BigCardPath(vo.UserNeedShowEvoCard()),ModuleName);
        SetStars(vo);
        string spName = "UIAtlas_Common_R";
        if (vo.CardVo.Credit == CreditPB.Ssr)
        {
            spName = "UIAtlas_Common_SSR";
        }
        else if (vo.CardVo.Credit == CreditPB.Sr)
        {
            spName = "UIAtlas_Common_SR";
        }

        _credit.sprite = AssetManager.Instance.GetSpriteAtlas(spName);
        _credit.SetNativeSize();

        int sceneId = 0;
        bool normalstate = false;
        bool finishedState = false;
        bool finishedCurStep = false;
        _levelupDisposition.image.color=Color.white;
        _gotocard.image.color=Color.white;
        _gotocard.enabled = true;
        _levelupDisposition.enabled = true;
        _gotoCardtxt.text =I18NManager.Get("FavorabilityMain_GoRoseheart"); //前往升心

        for (int i = 0; i < vo.MaxStars; i++)
        {
            //这个表示从0到1的数据！
            var targetStarUpPb = GlobalData.CardModel.GetCardStarUpRule(vo.CardId, (StarPB) i);

            if (targetStarUpPb==null)
            {
             Debug.LogError("no i"+i);   
            }
            
            if (targetStarUpPb != null)
            {
                //Debug.LogError(targetStarUpPb.SceneIds.Count);
                for (int j = 0; j < targetStarUpPb.SceneIds.Count; j++)
                {
                    if (targetStarUpPb.SceneIds[j] == 0) continue;
                    sceneId = targetStarUpPb.SceneIds[j];
                    SceneUnlockRulePB curscenePb = GlobalData.FavorabilityMainModel.GetUnlockRulePb(sceneId);
                    
                    //万一CursemePb为空我直接隐藏吧。
                    if (curscenePb==null)
                    {
                        SetStateTran(false, false, true);
                        FlowText.ShowMessage(I18NManager.Get("FavorabilityMain_IDAndFavorabilityLevelInconformity")); //找不到场景ID对应的好感度等级
                        return;
                    }
                    
//                    Debug.LogError("i"+i+"j"+sceneId);
                    //_dispositionTo.text =
                    //    GlobalData.FavorabilityMainModel.GetPlayerName(_userFavorabilityVo.Player) + "好感度达到" +
                    //    curscenePb.FavorabilityLevel;
                    _dispositionTo.text = I18NManager.Get("FavorabilityMain_DispositionTo", GlobalData.FavorabilityMainModel.GetPlayerName(_userFavorabilityVo.Player), curscenePb.FavorabilityLevel);
                    // _startxt.text = "星缘达到" + (int)targetStarUpPb.Star + "心";    
                    _startxt.text = I18NManager.Get("FavorabilityMain_Startext", (int)targetStarUpPb.Star);
                    if (curscenePb.FavorabilityLevel > _userFavorabilityVo.Level)
                    {
                        _addFavorability.text = I18NManager.Get("FavorabilityMain_PromoteFavorability"); //提升好感度
                        _levelupDisposition.image.color=Color.white;
                        _levelupDisposition.enabled = true;
                        finishedCurStep = false;
                        if ((int) targetStarUpPb.Star > vo.Star)
                        {
                            _gotoCardtxt.text = I18NManager.Get("FavorabilityMain_GoRoseheart"); //前往升心
                            _gotocard.image.color=Color.white;
                            _gotocard.enabled = true;
                        }
                        else
                        {
                            _gotoCardtxt.text = I18NManager.Get("FavorabilityMain_Arriveat"); //已达成
                            _gotocard.image.color=Color.gray;
                            _gotocard.enabled = false;
                        }

                        normalstate = true;
                        break;
                    }
                    else
                    {
                        _addFavorability.text = I18NManager.Get("FavorabilityMain_Arriveat");//已达成
                        _levelupDisposition.image.color=Color.gray;
                        _levelupDisposition.enabled = false;
                        if ((int) targetStarUpPb.Star <= vo.Star)
                        {
//                            Debug.LogError((int) targetStarUpPb.Star +"已达成"+ vo.Star);
                            _gotoCardtxt.text = I18NManager.Get("FavorabilityMain_Arriveat");//已达成
                            _gotocard.image.color=Color.gray;
                            _gotocard.enabled = false;
                            finishedCurStep = true;
                            if ((int) targetStarUpPb.Star==vo.MaxStars)
                            {
                                finishedState = true;
                                break;
                            }
                        }
                        else
                        {
                            _gotoCardtxt.text = I18NManager.Get("FavorabilityMain_GoRoseheart");  //前往升心
                            _gotocard.image.color=Color.white;
                            _gotocard.enabled = true;
                            finishedCurStep = false;
                            normalstate = true;
                            break;
                        }
                    }
                }
                
                //这里需要做一个判断，当解锁完所有关卡了，并且也已经遍历了所有心级，那么久可以当做是finished了！
                if ((int) targetStarUpPb.Star==vo.MaxStars&&finishedCurStep)
                {
                    finishedState = true; 
                }
                
            }

            if (normalstate)
            {
                SetStateTran(true, false, false);
                break;
            }

            if (finishedState)
            {
                SetStateTran(false, true, false); 
                break;
            }
            
            if (targetStarUpPb!=null&&sceneId == 0&&vo.Star==vo.MaxStars)
            {
                _startxt.text = I18NManager.Get("FavorabilityMain_GUESSTargetFavorability");       // 暂无目标好感度
                _dispositionTo.text = I18NManager.Get("FavorabilityMain_GUESSHeartLevelFavorability"); //"暂无心级好感度";
            }
           
        }


    }


    /// <summary>
    /// 要重新检查一下进化窗口的逻辑是否正确！
    /// </summary>
    /// <param name="index"></param>
    private void SetIndexState(int index)
    {
        if (index == 0 && _cardlist.Count > 1)
        {
            SetArrowState(false, true);
        }
        else if (index > 0 && index < _cardlist.Count - 1)
        {
            SetArrowState(true, true);
        }
        else if (index == _cardlist.Count - 1 && _cardlist.Count > 1)
        {
            SetArrowState(true, false);
        }
        else
        {
            SetArrowState(false, false);
        }
    }

    /// <summary>
    /// 箭头状态
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    private void SetArrowState(bool left, bool right)
    {
        _goleft.gameObject.SetActive(left);
        _goRight.gameObject.SetActive(right);
    }

    private void SetStateTran(bool trigger, bool finish, bool nocard)
    {
//        Debug.LogError(trigger+" "+finish+" "+nocard);
        _TriggerState.gameObject.SetActive(trigger);
        _finisheAll.gameObject.SetActive(finish);
        _noCard.gameObject.SetActive(nocard);
    }
    
    private void SetStars(UserCardVo userCardVo)
    {
        for (int i = 0; i <_heartContainer.childCount; i++)
        {
            RawImage img = _heartContainer.GetChild(i).GetComponent<RawImage>();
            img.gameObject.SetActive(i >4-userCardVo.Star);
        }

        for (int i = 0; i <_heartBGContainer.childCount; i++)
        {
            RawImage img = _heartBGContainer.GetChild(i).GetComponent<RawImage>();
            img.gameObject.SetActive(i>4-userCardVo.MaxStars);
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
           //首先要判断有没有下一个卡牌没有的话就return
            if (curIndex<_cardlist.Count-1&&_cardlist.Count>1)
            {             
                curIndex++;
                SetIndexState(curIndex);
                SetCardData(_cardlist[curIndex]);            
            }             
        }
        else
        {
            //首先要判断有没有上一个卡牌，没有就return
            if (curIndex>0&&_cardlist.Count>0)
            {
                curIndex--;
                SetIndexState(curIndex);
                SetCardData(_cardlist[curIndex]);              
            }           
        }
                
    }
}