#region 模块信息

// **********************************************************************
// Copyright (C) 2018 The 深圳望尘体育科技
//
// 文件名(File Name):             CardInfoView.cs
// 作者(Author):                  张晓宇
// 创建时间(CreateTime):           #CreateTime#
// 修改者列表(modifier):
// 模块描述(Module description):
// **********************************************************************

#endregion

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Common;
using Componets;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardDetailView : View//,IBeginDragHandler,IDragHandler,IEndDragHandler//,IPointerDownHandler
{

    private Button _loveBtn;

    private Button _evopreToggle;
    private Button _starupBtn;
    private Button _levelOneBtn;
    
    private CardDetailPropertiesView _cardDetailPropertiesView;
    private Text _levelText;
    private Text _expText;
    private ProgressBar _levelProgressBar;
    private Transform _levelBar;
    private Transform _upgradeLevelContainer;
    private Transform _upgradeStarContainer;
    //private Transform _evolutionContainer;
    private Transform _evolutionSelectContainer;
    private UserCardVo _userCardVo;
    //private ToggleGroup _toggleGroup;
    private RawImage _heroCard;
    private RawImage _fadeCard;
    private RawImage _effectCard;
    private RawImage _signature;
    
    private int _selectedEvolutionCards = -1;
    private Transform _upgradeLevelProps;
    private Transform _upgradeOneLevel;
    private Transform _leveluplimittext;
    private Transform _propsTran;
    private Text _levellimittext;
    private Text _maxStar;
    private Transform _arrowBtn;   //箭头要做动画和按钮
    private Transform _titletran;
    private Transform _propertiesTran;
    
    private AdditionType _curType=AdditionType.Level;

    private EvolutionPB _curEvo = EvolutionPB.Evo0;
    private int _cardId = 0;

    private int _smallPropNum = 0;
    private int _bigPropNum = 0;
    private int _largePropNum = 0;

    private int _intsmallPropNum = 0;
    private int _intbigPropNum = 0;
    private int _intlargePropNum = 0;
    private int _callTimes = 0;
    
    private Text _upgradeTips;
    //private GameObject _selectIcon;

    private bool needRepeat = false;
    
    private int _curLevelExp;
    private int _needExp;
    private int oldLevel;
    private string oldNeedExp;
    private int oldneedExpNum;
    
    private Text conditiontext;
    private Text costText;
    
    private bool _propertiesState=false;
    private bool _starUpSuccess = true;
    
    
    private Vector2 _onchoosevec=new Vector2(286.6f,-100f);
    private Vector2 _nochoosevec=new Vector2(100f,-253.2f);

//    private ParticleSystem _particle1;
//    private ParticleSystem _particle2;
//    private ParticleSystem _particle3;

    private Animator _starAnimator;

    Text _numText;
    int _oneExp1 = 0;
    int _oneExp2 = 0;
    int _oneExp3 = 0;

    private Transform levelprop1;
    private Transform levelprop2;
    private Transform levelprop3;

    private Transform starProp1;
    private Transform starProp2;
    private Transform starProp3;
    private GameObject _evoMask;
    private bool hassignature;
    private Button starUpPreviewBtn;

    private Transform _bottomTab;
    private List<Toggle> toggles;
    private Transform _starupRedPoint;
    private Transform _evolutionRedPoint;
    private Transform _loveRedPoint;

    private AppointmentRuleVo _AppointmentVo;
    private Text _cardfaceText;

    //public GameObject _barEffect;

    public bool CanBack
    {
        get { return !_evoMask.activeInHierarchy; }
    }


    private void Awake()
    {
        //_toggleGroup = transform.Find("TabBar").GetComponent<ToggleGroup>();
        RectTransform rect = transform.Find("List").GetComponent<RectTransform>();
        rect.offsetMax = new Vector2(0, ModuleManager.OffY );
        RectTransform Evorect = transform.Find("EvoList").GetComponent<RectTransform>();
        Evorect.offsetMax = new Vector2(0, ModuleManager.OffY );
        _starAnimator = transform.GetComponent<Animator>();
//        _upgradeLevelToggle = transform.Find("TabBar/UpgradeLevelToggle").GetComponent<Toggle>();
//        _upgradeStarToggle = transform.Find("TabBar/UpgradeStarToggle").GetComponent<Toggle>();
        _loveBtn = transform.Find("LoveBtn").GetComponent<Button>();
        _loveBtn.onClick.AddListener(() =>
        {
            //FlowText.ShowMessage("doing!");
            GlobalData.CardModel._curRoleId = _userCardVo.EvolutionRequirePropId();
            SendMessage(new Message(MessageConst.CMD_CARD_LOVE,Message.MessageReciverType.CONTROLLER,_AppointmentVo));
        });
        
        //_evolutionToggle.onValueChanged.AddListener(OnTabBarChange);
        _evoMask = transform.Find("EffectMask").gameObject;

        _levelBar = transform.Find("LevelBar");
        _levelText = transform.Find("LevelBar/LevelText").GetComponent<Text>();
        _expText = transform.Find("LevelBar/LevelText/ExpImage/ExpText").GetComponent<Text>();
        _levelProgressBar = transform.Find("LevelBar/ProgressBar").GetComponent<ProgressBar>();
        _arrowBtn = transform.Find("LevelBar/LevelText/Arrow");
        PointerClickListener.Get(_arrowBtn.gameObject).onClick=go =>
        {
            if (_propertiesState)
            {
                SetPropertiesViewShow(false);
            }
            else
            {
                SetPropertiesViewShow(true);
            }
            
            //这里是测试用的代码
//            StarTween();
            
        };

        _signature=transform.Find("List/HeroCard/SignatureTex").GetRawImage();
        starUpPreviewBtn =transform.GetButton("StarUpPreviewBtn");
        starUpPreviewBtn.onClick.AddListener(() =>
        {
            SendMessage(new Message(MessageConst.CMD_CARD_SHOPSTARUPPREVIEW,Message.MessageReciverType.CONTROLLER,_userCardVo));
        });
            
        _cardDetailPropertiesView = transform.Find("CardDetailPropertiesView").GetComponent<CardDetailPropertiesView>();
        _titletran = transform.Find("CardDetailPropertiesView/Title");
        _propertiesTran = transform.Find("CardDetailPropertiesView/Properties");

        _upgradeLevelContainer = transform.Find("UpgradeLevelContainer");
        _upgradeStarContainer = transform.Find("UpgradeStarContainer");
        _evolutionSelectContainer = transform.Find("ChooseEvolutionContainer");
        _maxStar = transform.Find("UpgradeStarContainer/MaxStar").GetComponent<Text>();
        _evopreToggle = _evolutionSelectContainer.Find("Toggle/SelectEvolutionPre").GetComponent<Button>();
        //_selectIcon = _evolutionSelectContainer.Find("Toggle/SelectEvolutionPre/SelectIcon").gameObject;
        _cardfaceText = _evolutionSelectContainer.Find("Toggle/SelectEvolutionPre/Text").GetText();
        _evopreToggle.onClick.AddListener(OnEvoChange);

        _upgradeTips = transform.Find("UpgradeTips").GetText();
        _propsTran= _upgradeLevelContainer.Find("UpgradeLevelProps/");
        //_barEffect = transform.Find("LevelBar/ProgressBar/Mask/Effect").gameObject;

//        _particle1 = _propsTran.GetChild(0).Find("Particle").GetComponent<ParticleSystem>();
//        _particle2 = _propsTran.GetChild(1).Find("Particle").GetComponent<ParticleSystem>();
//        _particle3 = _propsTran.GetChild(2).Find("Particle").GetComponent<ParticleSystem>();

        _bottomTab = transform.Find("BottomTab");
        toggles=new List<Toggle>();
        for (int i = 0; i < _bottomTab.childCount; i++)
        {
            Toggle toogle = _bottomTab.GetChild(i).GetComponent<Toggle>();
            toggles.Add(toogle);
            toogle.onValueChanged.AddListener(OnBottomTabChange);
        }

        _starupRedPoint = transform.Find("BottomTab/starupToggle/RedPoint");
        _evolutionRedPoint = transform.Find("BottomTab/evolutionToggle/RedPoint");
        _loveRedPoint = transform.Find("LoveBtn/redpoint");
        
        conditiontext= _upgradeStarContainer.Find("NoMaxStar/ConditionText").GetComponent<Text>();
        costText= _upgradeStarContainer.Find("NoMaxStar/CostLayout/CostText").GetComponent<Text>();

        EventCombination();
        
        _levelOneBtn=_upgradeLevelContainer.Find("UpgradeOneLevel").GetComponent<Button>();
        _levelOneBtn.onClick.AddListener(() =>
        {
            //当前等级小于上限才可以发送
            if (_userCardVo.Level < _userCardVo.UpgradeRequireLevel)
            {
                SendMessage(new Message(MessageConst.CMD_CARD_UPGRADE_ONELEVEL, Message.MessageReciverType.CONTROLLER,
                    _userCardVo));
            }
            else
            {
                FlowText.ShowMessage(I18NManager.Get("Card_NeedMoreLevel"));
            }

        });
        
        //升星
        _starupBtn=_upgradeStarContainer.Find("NoMaxStar/UpgradeStarBtn").GetComponent<Button>();
        _starupBtn.onClick.AddListener((() =>
        {
            if (_starUpSuccess)
            {
                //判断是否满足条件！
                if (_userCardVo.Level<_userCardVo.UpgradeRequireLevel)
                {
                    FlowText.ShowMessage(I18NManager.Get("Card_NeedLevelLock",_userCardVo.UpgradeRequireLevel));
                }
                else
                {
                    SendMessage(new Message(MessageConst.CMD_CARD_UPGRADE_STAR, _userCardVo)); 
                }

            }
            else
            {
                FlowText.ShowMessage(I18NManager.Get("LoveAppointment_Hint5"));
            }
            

        }));

        //卡牌可滑动
        _heroCard = transform.Find("List/HeroCard").GetComponent<RawImage>();
        _fadeCard = transform.Find("EvoList/HeroCard").GetRawImage();
        _effectCard = transform.Find("EvoList/FadeShow").GetRawImage();
        
        
        PointerClickListener.Get(_heroCard.gameObject).onClick = go =>
        {
            SendMessage(new Message(MessageConst.MODULE_CARD_SHOW_FULLSCREEN_CARD, Message.MessageReciverType.DEFAULT,_heroCard,hassignature?_signature:null));
        };

        var starprop = _upgradeStarContainer.Find("NoMaxStar/UpgradeStarProps");
        
        for (int i = 0; i < _propsTran.childCount; i++)
        {
            starprop.GetChild(i).Find("Particle").gameObject.Hide();  
        }
        
        _leveluplimittext  = _upgradeLevelContainer.Find("LevelUpLimitText");   
        _upgradeLevelProps = _upgradeLevelContainer.Find("UpgradeLevelProps");
        _upgradeOneLevel   = _upgradeLevelContainer.Find("UpgradeOneLevel");
        //_levellimittext = _upgradeStarContainer.Find("MaxStar").GetComponent<Text>();
        levelprop1= _upgradeStarContainer.Find("NoMaxStar/UpgradeStarProps").GetChild(0)
            .Find("LevelProp");
        levelprop2 = _upgradeStarContainer.Find("NoMaxStar/UpgradeStarProps").GetChild(1)
            .Find("LevelProp");
        levelprop3 = _upgradeStarContainer.Find("NoMaxStar/UpgradeStarProps").GetChild(2)
            .Find("LevelProp");
        starProp1= _upgradeStarContainer.Find("NoMaxStar/UpgradeStarProps").GetChild(0)
            .Find("PropImage");
        starProp2 = _upgradeStarContainer.Find("NoMaxStar/UpgradeStarProps").GetChild(1)
            .Find("PropImage");
        starProp3 = _upgradeStarContainer.Find("NoMaxStar/UpgradeStarProps").GetChild(2)
            .Find("PropImage");

        _oneExp1 = GlobalData.PropModel.GetPropBase(PropConst.CardUpgradePropSmall).Exp;
        _oneExp2 = GlobalData.PropModel.GetPropBase(PropConst.CardUpgradePropBig).Exp;
        _oneExp3 = GlobalData.PropModel.GetPropBase(PropConst.CardUpgradePropLarge).Exp;
    }

    private void OnBottomTabChange(bool isOn)
    {
        if (isOn==false)
            return;

        string name = EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("OnTabChange===>" + name);

        switch (name)
        {
            case "levelupToggle":
                ShowUpgradeLevel();
                break;
            case "starupToggle":
                ShowUpgradeStar();    
                break;
            case "evolutionToggle":
                //打开进化界面
                SendMessage(new Message(MessageConst.CMD_CARD_EVOLUTION,Message.MessageReciverType.CONTROLLER, _userCardVo));
                //ShowEvolution();
                SetToggleShow(2);
                break;
            default:
                Debug.LogError(name);
                break;
                        
        }


    }

    public int ToggleIndex = 0;
    
    public void SetToggleShow(int idx)
    {

        string name = "";


        switch (idx)
        {
            case 0:
                name = "levelupToggle";
                ToggleIndex = 0;
                break;
            case 1:
                name = "starupToggle";
                ToggleIndex = 1;
                break;
            case 2:
                name = "evolutionToggle";
                break;
            default:
                name = "levelupToggle";
                break;   
                        
        }
        
        Debug.Log("OnTabChange===>" + name);
        
        foreach (var v in toggles)
        {
            v.onValueChanged.RemoveAllListeners();
        }
        
        for (int i = 0; i < _bottomTab.childCount; i++)
        {
            _bottomTab.GetChild(i).Find("bglabel").gameObject.SetActive(i != idx);
            toggles[i].isOn = i == idx;
        }
        
        foreach (var v in toggles)
        {
            v.onValueChanged.AddListener(OnBottomTabChange);
        }

//        if (_userCardVo.Star>=_userCardVo.CardVo.MaxStar)
//        {
//            ShowEvolution();
//        }
//        else
//        {
//            ShowUpgradeLevel();
//        }


        

    }


    private void EventCombination()
    {
        //升级
        _upgradeLevelContainer.Find("UpgradeLevelProps/PropItem1").GetComponent<LongPressButton>().OnDown=(() =>
        {
            if (_intsmallPropNum>0)
            {
                _smallPropNum++;
               // _particle1.Play();
                ValidateView(PropConst.CardUpgradePropSmall);
            }
            else
            {
                AudioManager.Instance.StopEffect();  
            }
        });
   
        _upgradeLevelContainer.Find("UpgradeLevelProps/PropItem2").GetComponent<LongPressButton>().OnDown=(() =>
            {
                if (_intbigPropNum>0)
                {
                    _bigPropNum++;
                    //_particle2.Play();
                    ValidateView(PropConst.CardUpgradePropBig); 
                }
                else
                {
                    AudioManager.Instance.StopEffect();  
                }

            });
        
        _upgradeLevelContainer.Find("UpgradeLevelProps/PropItem3").GetComponent<LongPressButton>().OnDown=(() =>
            {
                if (_intlargePropNum>0)
                {
                    _largePropNum++;
                    //_particle3.Play();
                    ValidateView(PropConst.CardUpgradePropLarge);   
                }
                else
                {
                    AudioManager.Instance.StopEffect();  
                }
            });
        
                 
        //可以这样，onUp可以跟长按组件组合起来，长按组件只负责UI更新显示和计数，但是OnUp才是真正给后端发送请求。
        _upgradeLevelContainer.Find("UpgradeLevelProps/PropItem1").GetComponent<LongPressButton>().OnUp =(() =>
        {
            if (_smallPropNum>0)
            {
                SendSmallProp();
            }
            else
            {
                //FlowText.ShowMessage(I18NManager.Get("Card_NumNotEnough"));
                ShowPropLack();
            }

        });

        _upgradeLevelContainer.Find("UpgradeLevelProps/PropItem2").GetComponent<LongPressButton>().OnUp =(() =>
        {
            if (_bigPropNum>0)
            {
                SendBigProp();
            }
            else
            {
                //FlowText.ShowMessage(I18NManager.Get("Card_NumNotEnough"));
                ShowPropLack();
            }
        });
        
        _upgradeLevelContainer.Find("UpgradeLevelProps/PropItem3").GetComponent<LongPressButton>().OnUp= (()=>
        {
            if (_largePropNum>0)
            {
                SendLargerProp();
            }
            else
            {
                //FlowText.ShowMessage(I18NManager.Get("Card_NumNotEnough"));
                ShowPropLack();
            }
        });


    }
    void ShowPropLack()
    {        
        PopupManager.ShowConfirmWindow(
        I18NManager.Get("Card_PropLacking"),
            null,
            I18NManager.Get("Card_GotoShop"),
            I18NManager.Get("Card_GotoMainLine")
            ).WindowActionCallback = evt =>
            {
                if (evt == WindowEvent.Ok)
                {
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_SHOP, false, true,3);
                  
                }
                if (evt == WindowEvent.Cancel)
                {
                    ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_MAIN_LINE, false, true);
                }
            };
    }

    private void OnEvoChange()
    {
        if (_userCardVo.Evolution < EvolutionPB.Evo2)
        {            
            FlowText.ShowMessage(I18NManager.Get("Card_Evo2Condiction"));
            return;
        }

        
        if (_curEvo!=EvolutionPB.Evo0)
        {
            _curEvo = EvolutionPB.Evo0;
            int[] send={0,_cardId};
            SendMessage(new Message(MessageConst.CMD_CARD_CHOOSE_EVO,Message.MessageReciverType.CONTROLLER,send));
        }
        else//(_curEvo!=_userCardVo.CardVo.NewViewEvo)
        {
            _curEvo = _userCardVo.CardVo.NewViewEvo;
            int[] send={(int) (_userCardVo.CardVo.NewViewEvo),_cardId};
            SendMessage(new Message(MessageConst.CMD_CARD_CHOOSE_EVO,Message.MessageReciverType.CONTROLLER,send));  
        } 


    }




    private void ShowEvoSelect()
    {
        
        if (_userCardVo.CardVo.Credit!=CreditPB.R)
        {
            _evolutionSelectContainer.gameObject.Show();

        }
        else
        {
            _evolutionSelectContainer.gameObject.Hide();
        }

//        if (_userCardVo.Level==_userCardVo.CardVo.MaxLevel)
//        {
//            _cardDetailPropertiesView.SetData(_userCardVo, AdditionType.Evolution);
//        }
        
        
    }
    
    public void ShowEvolution()
    {
        _cardDetailPropertiesView.SetData(_userCardVo, AdditionType.Evolution);
        _levelBar.gameObject.Show();
        _upgradeLevelContainer.gameObject.Hide();
        _upgradeStarContainer.gameObject.Hide();
        //starUpPreviewBtn.gameObject.Hide();
        _upgradeTips.gameObject.Hide();
        //_curType = AdditionType.Evolution;
        ShowEvoSelect();
        
        
        MoveView(260);
    }

    public void ShowUpgradeStar()
    {
        SetToggleShow(1);
        _cardDetailPropertiesView.SetData(_userCardVo, AdditionType.Star);
        _curType = AdditionType.Star;
        _upgradeLevelContainer.gameObject.Hide();
        _upgradeStarContainer.gameObject.Show();
        //starUpPreviewBtn.gameObject.Hide();
        _upgradeTips.gameObject.Hide();
        MoveView(120);
    }

    public void ShowUpgradeLevel(bool resetView = false)
    {
        SetToggleShow(0);
        _curType = AdditionType.Level;
        _cardDetailPropertiesView.SetData(_userCardVo, AdditionType.Level);
        _levelBar.gameObject.Show();
        _upgradeLevelContainer.gameObject.Show();
        //starUpPreviewBtn.gameObject.Show();
        //升心预览先根据关卡信息直接隐藏掉！ 
        //starUpPreviewBtn.gameObject.SetActive(GlobalData.LevelModel.FindLevel("1-12").IsPass);
        _upgradeTips.gameObject.Show();
        if (_userCardVo?.Level>=_userCardVo?.CardVo.MaxLevel)
        {
            _upgradeTips.gameObject.Hide(); 
            //starUpPreviewBtn.gameObject.Hide();
        }
        
        _upgradeStarContainer.gameObject.Hide();
        MoveView(260);
    }

    private void MoveView(int y)
    {
        return;
        
        RectTransform rt = _cardDetailPropertiesView.GetComponent<RectTransform>();
        rt.localPosition = new Vector3(rt.localPosition.x, y);
    }

    private void OnEnable()
    {
        _heroCard.transform.position=Vector3.zero;
    }


    private void SendSmallProp()
    {
        if (_smallPropNum>0)
        {
            AudioManager.Instance.StopEffect();  
            SendMessage(new Message(MessageConst.CMD_CARD_UPGRADE_LEVEL, Message.MessageReciverType.CONTROLLER,
                _userCardVo, PropConst.CardUpgradePropSmall, _smallPropNum));
            //Debug.LogError("Sendnum"+_smallPropNum);
            _smallPropNum = 0;
        } 
    }

    private void SendBigProp()
    {
        if (_bigPropNum>0)
        {
            AudioManager.Instance.StopEffect();  
            SendMessage(new Message(MessageConst.CMD_CARD_UPGRADE_LEVEL, Message.MessageReciverType.CONTROLLER,
                _userCardVo, PropConst.CardUpgradePropBig, _bigPropNum));
            //Debug.LogError("Sendnum"+_bigPropNum);
            _bigPropNum = 0;
        }
    }

    private void SendLargerProp()
    {
        if (_largePropNum>0)
        {
            AudioManager.Instance.StopEffect();  
            SendMessage(new Message(MessageConst.CMD_CARD_UPGRADE_LEVEL, Message.MessageReciverType.CONTROLLER,
                _userCardVo, PropConst.CardUpgradePropLarge, _largePropNum));
            //Debug.LogError("Sendnum"+_largePropNum);
            _largePropNum = 0;
        }  
    }
    
    private void ValidateView(int propType)
    {
        //Debug.Log("ValidateView start:");
        //触发这个函数就会减1个类型道具，加一个道具的经验，如果当前的经验超过了等级上限的最大经验则返回。
        //要刷新三处：1.道具ui 2.经验值经验条 3.等级和上限  4.没有道具会弹出警告！
        if (_userCardVo.Level==_userCardVo.UpgradeRequireLevel)
        {     
            ChangeCondiction(_userCardVo);
            //Debug.Log("ValidateView  equal---");
            ChangePropView(_userCardVo,true);
            
            //下列要抽出成一个函数！
            SendSmallProp();
            SendBigProp();
            SendLargerProp();
            
            return;
        }

        int _oneExp = 0;
        switch (propType)
        {
            case PropConst.CardUpgradePropSmall:
                if (_intsmallPropNum<=0)
                {
                    //FlowText.ShowMessage("道具不足");
                    return ;
                }
                _numText = _propsTran.GetChild(0).Find("RedCirclePoint/RedNum").GetComponent<Text>();
                _intsmallPropNum--;
                _numText.text = _intsmallPropNum.ToString();
                _oneExp=_oneExp1;
                break;
            case PropConst.CardUpgradePropBig:
                if (_intbigPropNum<=0)
                {
                    //FlowText.ShowMessage("道具不足");

                    return ;
                }
                _numText = _propsTran.GetChild(1).Find("RedCirclePoint/RedNum").GetComponent<Text>();
                _intbigPropNum--;
                _numText.text = _intbigPropNum.ToString();
                _oneExp=_oneExp2;
                break;
            case PropConst.CardUpgradePropLarge:
                if (_intlargePropNum<=0)
                {
                    //FlowText.ShowMessage("道具不足");
                    return ;
                }
                _numText = _propsTran.GetChild(2).Find("RedCirclePoint/RedNum").GetComponent<Text>();
                _intlargePropNum--;
                _numText.text = _intlargePropNum.ToString();
                _oneExp=_oneExp3;
                break;
        }
          
        _curLevelExp += _oneExp;

        //_levelProgressBar.ProgressBarAudio(0);
        //AudioManager.Instance.PlayEffect("progress_bar", 1,true,1);
        if (_curLevelExp>=_needExp)
        {
            
            _userCardVo.Level++;

            _curLevelExp = _curLevelExp - _needExp;
            _cardDetailPropertiesView.SetData(_userCardVo, AdditionType.Level);
            //需要播放特效！
            
            _needExp=GlobalData.CardModel.GetCardLevelRule(_userCardVo.Level, _userCardVo.CardVo.Credit).Exp-
                GlobalData.CardModel.GetCardLevelRule(_userCardVo.Level-1,_userCardVo.CardVo.Credit).Exp;

            if (_callTimes>1)
            {
                FlowText.ShowMessage(I18NManager.Get("Card_LevelUpSuccess"));
                AudioManager.Instance.PlayEffect("levelup",1);
                _levelText.text =
                    I18NManager.Get("Card_LevelTitleShow", _userCardVo.Level,
                        _userCardVo.UpgradeRequireLevel);
                //string.Format("等级  {0}\n<color='#F5CCFE'>上限  {1}</color>",_userCardVo.Level,_userCardVo.UpgradeRequireLevel) ;
            }
        }

            //长按的就是这样解决！
        _callTimes++;
        if (_callTimes>1)
        {
            UpdateExpInfo();
            //_levelProgressBar.ProgressBarAudio();  
            if (!needRepeat)
            {
                //Debug.LogError("needRepeat");
                AudioManager.Instance.PlayEffect("progress_bar", 1,true,3); 
            }
            
            needRepeat = true;
        }

        if (_callTimes>1000)
        {
            _callTimes = 0;
        }


        //在这里设置状态切换，查看是否升级了！

    }

    
    /// <summary>
    /// 经验增加的动画
    /// </summary>
    /// <param name="addexp"></param>
    private void UpdateExpInfo(bool addexp=false)
    {
        //Debug.LogWarning("----UpdateExpInfo:"+addexp);
        _levelProgressBar.DeltaX = 0;
        var compare = _userCardVo.Level - oldLevel;
        if (_userCardVo.Level>=_userCardVo.CardVo.MaxLevel)
        {
            //满级其实也需要动画的
            _expText.text = I18NManager.Get("Card_MaxLevel");
            _levelProgressBar.Progress = 100;
            _levelText.text = I18NManager.Get("Card_LevelTitleShow", _userCardVo.Level,
                _userCardVo.UpgradeRequireLevel); 
            //"等级  "+_userCardVo.Level + "\n<color='#F5CCFE'>上限  "+_userCardVo.UpgradeRequireLevel+"</color>";
        }
        else
        {
            if (addexp)
            {
                if (needRepeat)
                {
                    //Debug.LogError("??");
                    AudioManager.Instance.StopEffect();  
                    needRepeat = false;
                }
                else
                {
                    _levelProgressBar.TweenSlider((float) _curLevelExp/ _needExp * 100, () =>
                        {
                            //安全起见，最后要强制同步为后端值。
                            //Debug.LogError("Call"+compare);
                        
                            //强制在最后同步一下动画是很致命的
                            if (_userCardVo.Level>=_userCardVo.CardVo.MaxLevel)
                            {
                                //满级其实也需要动画的
                                _expText.text = I18NManager.Get("Card_MaxLevel");
                                _levelProgressBar.Progress = 100;
                                _levelText.text = I18NManager.Get("Card_LevelTitleShow", _userCardVo.Level,
                                    _userCardVo.UpgradeRequireLevel); //"等级  "+_userCardVo.Level + "\n<color='#F5CCFE'>上限  "+_userCardVo.UpgradeRequireLevel+"</color>";
                            }

                        },compare,1);
                    _levelProgressBar.ProgressBarAudio();
                }


                if (_expText.text.Contains(oldNeedExp))
                {
                    Util.TweenTextNum(_expText,1f, _curLevelExp == 0 ? oldneedExpNum : _curLevelExp, "", oldNeedExp, () =>
                    {
                        if (compare > 0)
                        {
                            FlowText.ShowMessage(I18NManager.Get("Card_LevelUpSuccess"));	
                            AudioManager.Instance.PlayEffect("levelup",1);
                            //_cardDetailPropertiesView.ShowUpgradeEffect();
                            ChangePropView(_userCardVo, addexp);
                        }
                        _levelProgressBar.Progress = (int)((float)_curLevelExp / _needExp * 100);
                        _levelText.text = I18NManager.Get("Card_LevelTitleShow", _userCardVo.Level,
                            _userCardVo.UpgradeRequireLevel); //"等级  " + _userCardVo.Level + "\n<color='#F5CCFE'>上限  " +    _userCardVo.UpgradeRequireLevel + "</color>";

                        _expText.text = _curLevelExp + "/" + _needExp;
                        oldNeedExp = "/" + _needExp;
                        
                        //不知道这句代码需不需要，作为保护用的，因为最后升一级的话需要显示满级才对的！

                    }); 
                }

            }
            else
            {
                //Debug.LogError("111");
                _levelText.text =I18NManager.Get("Card_LevelTitleShow", _userCardVo.Level,
                    _userCardVo.UpgradeRequireLevel); 
                    
                    //"等级  "+_userCardVo.Level + "\n<color='#F5CCFE'>上限  "+_userCardVo.UpgradeRequireLevel+"</color>";
                _expText.text = _curLevelExp + "/" + _needExp;
                _levelProgressBar.Progress = (int)((float)_curLevelExp / _needExp * 100);
                oldNeedExp = "/" + _needExp;
            }

        }

        //最后要记录之前的等级,连续点的话会出现问题！
        oldLevel = _userCardVo.Level; 
        oldneedExpNum = _needExp;
    }


    /// <summary>
    /// 判断切换升星界面还是升级界面
    /// </summary>
    /// <param name="userCardVo"></param>
    private void ChangeCondiction(UserCardVo userCardVo)
    {
        _curType = userCardVo.Level/20>userCardVo.Star ? AdditionType.Star : AdditionType.Level;
//        //这里可以判断升心或者升级的按钮是否可以点。
//        switch (_curType)
//        {
//            case AdditionType.Level:
//                break;
//            case AdditionType.Star:
//                break;
//        }
        
        
    }
    
    /// <summary>
    /// 赋值主通道
    /// </summary>
    /// <param name="userCardVo"></param>
    /// <param name="propModel"></param>
    /// <param name="addExp"></param>
    /// <param name="starupsuccess"></param>
    public void SetData(UserCardVo userCardVo, PropModel propModel,bool addExp=false,bool starupsuccess=false)
    {
        GlobalData.CardModel.CurCardVo = userCardVo;
        _callTimes = 0;
        _userCardVo = userCardVo;
        _starUpSuccess = starupsuccess;
        ChangeCondiction(_userCardVo);
        SetCardBg(_userCardVo);


        _curLevelExp = userCardVo.CurrentLevelExp;
        _needExp = userCardVo.NeedExp;

        //Debug.LogWarning("cardDetailView SetData:" + addExp + "oldLevel:"+ oldLevel + " userCard lv:"+ _userCardVo.Level);
        if (!addExp || _userCardVo.Level <= oldLevel)
        {
            ChangePropView(userCardVo, addExp);
        }
        //经验信息
        UpdateExpInfo(addExp);

        //升星道具
        UpdateStarPropNum(userCardVo, addExp);

        //升级道具

        //Debug.LogError(userCardVo.Level+" "+ _userCardVo.UpgradeRequireLevel);
        if (userCardVo.Level < _userCardVo.UpgradeRequireLevel)
        {
            _upgradeLevelProps.gameObject.SetActive(true);
            _upgradeOneLevel.gameObject.SetActive(true);
            _leveluplimittext.gameObject.SetActive(false);
            Text expText;
            for (int i = 0; i < _propsTran.childCount; i++)
            {
                expText = _propsTran.GetChild(i).Find("ExpText").GetComponent<Text>();
                expText.text="+" + propModel.GetPropBase(PropConst.CardUpgradePropSmall+i).Exp;
            }
                       
            _upgradeTips.text=I18NManager.Get("Card_LevelUptoStarUp");
        }
        else
        {
            _upgradeLevelProps.gameObject.SetActive(userCardVo.Level < _userCardVo.CardVo.MaxLevel);
            _upgradeOneLevel.gameObject.SetActive(userCardVo.Level < _userCardVo.CardVo.MaxLevel);
            _leveluplimittext.gameObject.SetActive(userCardVo.Level >= _userCardVo.CardVo.MaxLevel);
            //等级达到上限的Text！只有切换到升星的时候才会显示。
            _leveluplimittext.GetComponent<Text>().text = I18NManager.Get("Card_HasMaxLevel");//"+"userCardVo.Level < _userCardVo.CardVo.MaxLevel ? I18NManager.Get("Card_StarupTips") : 

            _upgradeTips.text=I18NManager.Get("Card_NeedMoreLevel");
        }
        UpdatePropNum(propModel);
        _levelOneBtn.image.color = userCardVo.Level < _userCardVo.UpgradeRequireLevel ? Color.white : Color.gray;

        SetStarEffectIndex(starupsuccess);
        //_evolutionBtn.gameObject.SetActive(userCardVo.Star>=3);
        
        ShowEvoSelect();

        hassignature= _userCardVo.SignatureCard != null;
        _signature.gameObject.SetActive(hassignature);
        if (hassignature)
        {
            _signature.texture = ResourceManager.Load<Texture>("Prop/Signature/sign"+(int)_userCardVo.CardVo.Player);
            _signature.SetNativeSize();
        }
        
        starUpPreviewBtn.gameObject.SetActive( GuideManager.IsPass1_9());
        
        _starupRedPoint.gameObject.SetActive(userCardVo.ShowUpgradeStarRedpoint);
        _evolutionRedPoint.gameObject.SetActive(userCardVo.ShowEvolutionRedPoint);

        
        _loveBtn.gameObject.SetActive(userCardVo.CardVo.Credit!=CreditPB.R);
        _AppointmentVo = userCardVo.CardAppointmentRuleVo;
        _loveRedPoint.gameObject.SetActive(userCardVo.ShowLovePoint);

    }

    /// <summary>
    /// 升星动画
    /// </summary>
    /// <param name="starupsuccess"></param>
    private void SetStarEffectIndex(bool starupsuccess)
    {
        if (starupsuccess)
        {
            //Debug.LogError("show"+starupsuccess);
            _starAnimator.Play("StarUpHeart"+_userCardVo.Star,0,0f);

        }     
//        Debug.LogError("show"+starupsuccess);
    }
    
    /// <summary>
    /// 进化动画
    /// </summary>
    public void SetEvoEffect()
    {
        _effectCard.rectTransform.localPosition = _heroCard.rectTransform.localPosition;
        _fadeCard.rectTransform.localPosition = _heroCard.rectTransform.localPosition;
        _starAnimator.Play("EvoEffectAni",0,0f);        
    }
    
    /// <summary>
    /// 切换底部道具栏
    /// </summary>
    /// <param name="userCardVo"></param>
    /// <param name="addExp"></param>
    private void ChangePropView(UserCardVo userCardVo,bool addExp=false)
    {
        if (_curType!=AdditionType.Level)
        {
            //_cardDetailPropertiesView.SetData(userCardVo, _curType);
            if (addExp)
            {
                SetStarPropShow(false);
            }
            ShowUpgradeStar();
            
        }
        else
        {
            //_cardDetailPropertiesView.SetData(userCardVo, _curType);
            ShowUpgradeLevel();
        }
    }


    /// <summary>
    /// 关卡掉落后需要更新升级道具
    /// </summary>
    /// <param name="propModel"></param>
    public void UpdatePropNum(PropModel propModel)
    {
        for (int i = 0; i < _propsTran.childCount; i++)
        {
            var numText = _propsTran.GetChild(i).Find("RedCirclePoint/RedNum").GetComponent<Text>();
            numText.text=propModel.GetUserProp(PropConst.CardUpgradePropSmall+i).Num.ToString();
            switch (PropConst.CardUpgradePropSmall+i)
            {
                case PropConst.CardUpgradePropSmall:
                    _intsmallPropNum = propModel.GetUserProp(PropConst.CardUpgradePropSmall + i).Num;
                    break;
                case PropConst.CardUpgradePropBig:
                    _intbigPropNum= propModel.GetUserProp(PropConst.CardUpgradePropSmall + i).Num;
                    break;
                case PropConst.CardUpgradePropLarge:
                    _intlargePropNum= propModel.GetUserProp(PropConst.CardUpgradePropSmall + i).Num;
                    break;
            }
        }
    }



    /// <summary>
    /// 关卡掉落后需要更新升星道具
    /// </summary>
    /// <param name="userCardVo"></param>
    public void UpdateStarPropNum(UserCardVo userCardVo,bool addLevelExp=false)
    {
        _starUpSuccess = true;
        if (userCardVo.Star < userCardVo.MaxStars)
        {
            //_upgradeStarContainer.gameObject.Show();
            _upgradeStarContainer.Find("NoMaxStar").gameObject.Show();
            _maxStar.gameObject.Hide();
            
            conditiontext.gameObject.SetActive(userCardVo.Level<userCardVo.UpgradeRequireLevel);
            costText.gameObject.SetActive(userCardVo.Level==userCardVo.UpgradeRequireLevel);

            _starupBtn.image.color = userCardVo.Level == userCardVo.UpgradeRequireLevel ? Color.white : Color.gray;
            
            if (userCardVo.Level<userCardVo.UpgradeRequireLevel)
            {
                conditiontext.text =
                    I18NManager.Get("Card_NeedLevel")+ userCardVo.UpgradeRequireLevel;
            }
            else
            {
                costText.text = I18NManager.Get("Card_NeedCost")+ userCardVo.UpStarCost;
            }



            List<UpgradeStarRequireVo> upgradeStarProps = userCardVo.GetUpgradeStarProps;
            
            //升星道具
            for (int i = 0; i < 3; i++)
            {
                Transform item = _upgradeStarContainer.Find("NoMaxStar/UpgradeStarProps").GetChild(i);
                item.GetComponent<UpgradeStarPropItem>().SetData(upgradeStarProps?[i]);
                item.GetComponent<Button>().onClick.RemoveAllListeners();
                if (upgradeStarProps?[i].CurrentNum<upgradeStarProps?[i].NeedNum)
                {
                    _starUpSuccess = false;
                }
                
                item.GetComponent<Button>().onClick.AddListener(() =>
                {
                    string str = EventSystem.current.currentSelectedGameObject.name;
                    int index = int.Parse(str.Substring(str.Length - 1)) - 1;
                    SendMessage(new Message(MessageConst.CMD_CARD_GET_MORE_PROPS, Message.MessageReciverType.CONTROLLER,
                        userCardVo.GetUpgradeStarProps[index], userCardVo.CardId));
                    
                });
            }

            //从升级切换到升星的画面
            if (addLevelExp)
            {
                StarTween();
            }
            
        }
        else
        {
            if (_curType != AdditionType.Level)
            {
                _cardDetailPropertiesView.SetData(_userCardVo, AdditionType.Evolution);
            }
            _upgradeStarContainer.Find("NoMaxStar").gameObject.Hide();
            _maxStar.gameObject.Show();
        }
    }

    /// <summary>
    /// 切换到道具栏动画
    /// </summary>
    private void StarTween()
    {
        //必须要这样才能重复播放！
        _starAnimator.Play("starUp1",0,0f);
        //SetToggleShow(1);
        

    }
    
    /// <summary>
    /// 切换动画用的图片
    /// </summary>
    /// <param name="enable"></param>
    private void SetStarPropShow(bool enable)
    {
//        Debug.LogError(enable);
        levelprop1.gameObject.SetActive(!enable);
        levelprop2.gameObject.SetActive(!enable);
        levelprop3.gameObject.SetActive(!enable);
        starProp1.gameObject.SetActive(enable);
        starProp2.gameObject.SetActive(enable);
        starProp3.gameObject.SetActive(enable);
    }
    
    /// <summary>
    /// 大图背景设置
    /// </summary>
    /// <param name="userCardVo"></param>
    public void SetCardBg(UserCardVo userCardVo)
    {
        _curEvo = userCardVo.UseEvo;
        _cardId = userCardVo.CardId;
        //_selectIcon.SetActive(userCardVo.UseEvo != EvolutionPB.Evo0);
        //_evolaterToggle.isOn = userCardVo.UseEvo >= (userCardVo.CardVo.NewViewEvo);      
        _cardfaceText.text = I18NManager.Get("Card_CardFace");//userCardVo.UseEvo != EvolutionPB.Evo0 ?  I18NManager.Get("Card_CardFace"):I18NManager.Get("Card_CardBack") ;

        Texture texture = null;
        
        //这个可能导致BUG？！
        if (userCardVo.UserNeedShowEvoCard())
        {
            texture = ResourceManager.Load<Texture>(userCardVo.CardVo.BigCardPath(userCardVo.UserNeedShowEvoCard()),ModuleName);
        }
        else
        {
            texture = ResourceManager.Load<Texture>(userCardVo.CardVo.BigCardPath(),ModuleName);
        }


        if (userCardVo.CardVo.Credit!=CreditPB.R)
        {
            SetFadeCardBg(userCardVo.CardVo.BigCardPath(!userCardVo.UserNeedShowEvoCard()));
        }

 
        _heroCard.texture = texture;
    }

    private void SetFadeCardBg(string path)
    {
        _fadeCard.texture =ResourceManager.Load<Texture>(path,ModuleName);
            _effectCard.texture = _fadeCard.texture;

    }
    
    
    private void SetPropertiesViewShow(bool isShow)
    {
        if (isShow)
        {   
            Tweener move1 = _levelBar.GetComponent<RectTransform>().DOAnchorPos(new Vector3(540, 1257), 0.5f);//transform.DOLocalMoveY(1064, 0.5f);
            Tweener move2 = _titletran.GetComponent<RectTransform>().DOAnchorPos(new Vector3(528,306), 0.5f);//transform.DOLocalMoveY(269, 0.5f);

            DOTween.Sequence().Append(move1).Join(move2).AppendCallback(() =>
            {
                _arrowBtn.transform.localEulerAngles=new Vector3(0,0,0);
                _propertiesTran.gameObject.Show();
                _propertiesState = true; 
            });
            
        }
        else
        {
            Tweener move1 = _levelBar.GetComponent<RectTransform>().DOAnchorPos(new Vector3(540, 803), 0.5f);
            Tweener move2 = _titletran.GetComponent<RectTransform>().DOAnchorPos(new Vector3(528, -153), 0.5f);
            DOTween.Sequence().AppendCallback(() =>
            {
                _arrowBtn.transform.localEulerAngles=new Vector3(0,0,180);
                _propertiesTran.gameObject.Hide(); 
                _propertiesState = false;
            }).Append(move1).Join(move2);
            
        }
    }

}