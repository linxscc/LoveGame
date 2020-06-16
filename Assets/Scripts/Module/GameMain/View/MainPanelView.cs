using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Download;
using Assets.Scripts.Module.Framework.Utils;
using Assets.Scripts.Module.Guide;
using Com.Proto;
using Common;
using Componets;
using DataModel;
using game.main;
using game.main.Live2d;
using game.tools;
using live2d.framework;
using Module.Login;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public partial class MainPanelView : View
{
    private Button _startButton;
    private Button _drawCardBtn;
    private Button _supporterBtn;
    private Button _cardBtn;
    private Button _mailBtn;
    private Button _phoneBtn;
    private Button _missionBtn;
    private Button _achievementBtn;
    private Button _appointmentBtn;
    private Button _changeRoleBtn;
    private Button _fullviewBtn;
    private Button _stagingPostBtn;
    private Button _firstRechargeBtn;
    private Button _totalRechargeBtn;
    private Button _starActivityBtn;
    private Button _birthdayBtn;
    private Button _activityTemplateBtn;
    private Button _activityCapsuleTemplateBtn;
    private Button _activityMusicTemplateBtn;
    
    private Transform _buttons;
    private Transform _topBar;
    private Transform _startPathBtnContainer;
    private Transform _activityBar;
    private Transform _activityPanel;
    private Transform _activityPage;
    private Transform _userInfoView;
    private Transform _powerBar;
    private Transform _powerIcon;
    private Transform _powerAddIcon;
    private Transform _powerAddImg;
    private Transform _goldBar;
    private Transform _goldAddIcon;
    private Transform _gemBar;
    private Transform _backGround;
    private Transform _characterContainer;
    private Transform _recolletionBar;
    private Transform _visitPower;
    private Transform _exchangeIntegralBar;


    private Text _powerTxt;
    private Text _goldTxt;
    private Text _gemTxt;
    private Text _visitPowerTxt;

    private RawImage _bgRawImage;
    private Image _powerIconImage;

    private bool _isfullview = false;


    private UserInfoView _uiv;
    public UserInfoView UserInfoView => _uiv;

    private Animator _animator;
    private AnimationState _aniState;

    private Live2dGraphic _live2dGraphic;

    private Transform _starLineBack;
    private Transform _starLineFront;

    bool isClick = false;
    private bool _isShowLive2d = false;

    private Button _triggerGiftBtn;
    private Text _triggerGiftText;
    private CacheVo _extendCacheVo;
    private Dictionary<string, CacheVo> _backCacheVo;
    private bool _backDownLoading = false;

    private bool _isStartCheckTouch = false;
    private float _checkTime = 0f;
    private Transform _touchArrow;

    private bool _isArrowGoto;

    private Transform _countDownTra;
    
    private void Awake()
    {
        _isArrowGoto = false;
        _touchArrow = transform.Find("Buttons/StartBtn/TouchArrow");
        GuideArrow.DoAnimation(_touchArrow);
        
        _exchangeIntegralBar = transform.Find("TopBar/ExchangeIntegralBar");
        _visitPower = transform.Find("TopBar/VisitPowerBar");
        _recolletionBar = transform.Find("TopBar/RecolletionBar");

        _bgRawImage = transform.Find("BackGround/Bg").GetComponent<RawImage>();
        _startButton = transform.Find("Buttons/StartBtn").GetComponent<Button>();
        _drawCardBtn = transform.Find("Buttons/RightBtnContent/DrawCardBtn").GetComponent<Button>();

        _supporterBtn = transform.Find("StartPathBtnContainer/SupporterBtn").GetComponent<Button>();
        _missionBtn = transform.Find("Buttons/RightBtnContent/MissionBtn").GetComponent<Button>();
        _achievementBtn = transform.Find("Buttons/RightBtnContent/AchievementBtn").GetComponent<Button>();
        _changeRoleBtn = transform.Find("Buttons/LeftBtnContent/ChangeRoleBtn").GetComponent<Button>();

        _userInfoView = transform.Find("UserInfoView");
        _fullviewBtn = transform.Find("Buttons/LeftBtnContent/FullViewBtn").GetComponent<Button>();
        _stagingPostBtn = transform.Find("Buttons/LeftBtnContent/StagingPostBtn").GetComponent<Button>();

        _starActivityBtn = transform.GetButton("Buttons/RightBtnContent/StarActivityBtn");

        _birthdayBtn = transform.GetButton("Buttons/RightBtnContent/BirthdayBtn");

        _activityTemplateBtn = transform.GetButton("Buttons/RightBtnContent/ActivityTemplateBtn");
        _activityCapsuleTemplateBtn = transform.GetButton("Buttons/RightBtnContent/ActivityCapsuleTemplateBtn");

        _activityMusicTemplateBtn = transform.GetButton("Buttons/RightBtnContent/ActivityMusicTemplateBtn");
        
        _animator = this.GetComponent<Animator>();
        _uiv = _userInfoView.GetComponent<UserInfoView>();

        _backGround = transform.Find("BackGround");
        _characterContainer = transform.Find("CharacterContainer");
        _live2dGraphic = _characterContainer.Find("Live2dGraphic").GetComponent<Live2dGraphic>();
        if ((float) Screen.height / Screen.width > 1.8f)
        {
            _live2dGraphic.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        }

        _starLineBack = _characterContainer.Find("StarLineBack");
        _starLineFront = _characterContainer.Find("StarLineFront");

        _phoneBtn = transform.Find("StartPathBtnContainer/PhoneBtn").GetComponent<Button>();
        _mailBtn = transform.Find("StartPathBtnContainer/MailBtn").GetComponent<Button>();

        _cardBtn = transform.Find("Buttons/CardBtn").GetComponent<Button>();
        _appointmentBtn = transform.Find("StartPathBtnContainer/AppointmentBtn").GetComponent<Button>();

        _firstRechargeBtn = transform.GetButton("Buttons/LeftBtnContent/FirstRecharge");
        _totalRechargeBtn = transform.GetButton("Buttons/LeftBtnContent/11");

        _topBar = transform.Find("TopBar");

        _powerBar = _topBar.Find("PowerBar");

        _powerIconImage = _powerBar.Find("powerIcon").GetComponent<Image>();
        _powerIcon = _powerBar.Find("powerIcon/OnClick");
        _powerAddIcon = _powerBar.Find("addIcon/OnClick");
        _powerAddImg = _powerBar.Find("addIcon");

        _goldBar = _topBar.Find("GoldBar");
        _goldAddIcon = _goldBar.Find("addIcon/OnClick");

        _gemBar = _topBar.Find("GemBar");

        _powerTxt = _topBar.Find("PowerBar/Text").GetComponent<Text>();
        _goldTxt = _topBar.Find("GoldBar/Text").GetComponent<Text>();
        _gemTxt = _topBar.Find("GemBar/Text").GetComponent<Text>();
        _visitPowerTxt = _topBar.Find("VisitPowerBar/Text").GetComponent<Text>();

        _buttons = transform.Find("Buttons");

        _startPathBtnContainer = transform.Find("StartPathBtnContainer");

        
        _triggerGiftBtn = transform.GetButton("Buttons/TriggerGiftBtn");
        _triggerGiftText = transform.GetText("Buttons/TriggerGiftBtn/TextBg/Text");

        _activityBar = transform.Find("ActivityBar");
        _activityPanel = _activityBar.Find("Panel");
        _activityPage = _activityBar.Find("PageNums");
        _drawCardBtn.onClick.AddListener(OnDrawCardBtn);
        _startButton.onClick.AddListener(OnStartBtn);
        _supporterBtn.onClick.AddListener(OnSupporterBtn);
        _missionBtn.onClick.AddListener(OnMissionBtn);
        _fullviewBtn.onClick.AddListener(OnFullViewBtn);
        _stagingPostBtn.onClick.AddListener(OnStagingPostBtn);
        _appointmentBtn.onClick.AddListener(OnAppointmentBtn);


        _phoneBtn.onClick.AddListener(OnPhoneBtn);

        _cardBtn.onClick.AddListener(OnCardBtn);
        _mailBtn.onClick.AddListener(OnMailBtn);
        _changeRoleBtn.onClick.AddListener(OnChangeRoleBtn);
        _achievementBtn.onClick.AddListener(OnAchievementBtn);

        _firstRechargeBtn.onClick.AddListener(OnFirstRechargeBtn);
        _totalRechargeBtn.onClick.AddListener(OntotalRechargeBtn);

        _starActivityBtn.onClick.AddListener(OnStarActivityBtn);

        _birthdayBtn.onClick.AddListener(OnBirthdayBtn);
        
        _activityTemplateBtn.onClick.AddListener(ActivityTemplateBtn);
        _activityCapsuleTemplateBtn.onClick.AddListener(ActivityCapsuleTemplateBtn);
        
        _activityMusicTemplateBtn.onClick.AddListener(ActivityMusicTemplateBtn);

        _triggerGiftBtn.onClick.AddListener(ShowTriggerGift);
        _triggerGiftBtn.gameObject.Hide();

        PointerClickListener.Get(_powerIcon.gameObject).onClick = PowerHint; //点击体力图标
        PointerClickListener.Get(_powerAddIcon.gameObject).onClick = BuyPower;

        PointerClickListener.Get(_goldAddIcon.gameObject).onClick = BuyGold;

        PointerClickListener.Get(_gemBar.gameObject).onClick = BuyGem;


        PointerClickListener.Get(_userInfoView.gameObject).onClick = PopupUserInfo;
        //  PointerClickListener.Get(_live2dGraphic.gameObject).onClick = PlayDialog;
        // UIEventListener.Get(_live2dGraphic.gameObject).onUp = Live2dTigger;


        PointerClickListener.Get(_recolletionBar.Find("addIcon/OnClick").gameObject).onClick = BuyRecolletionPower;
        PointerClickListener.Get(_recolletionBar.Find("powerIcon/OnClick").gameObject).onClick =
            BuyRecolletionPowerHint;
                                                                                          
        
                                                                     
        PointerClickListener.Get(_visitPower.Find("addIcon/OnClick").gameObject).onClick = BuySupporterPower;
        PointerClickListener.Get(_visitPower.Find("powerIcon/OnClick").gameObject).onClick = BuySupporterPowerHint;


        Transform headClick = transform.Find("CharacterContainer/Live2dGraphic/Head");
        Transform bodyClick = transform.Find("CharacterContainer/Live2dGraphic/Body");
        PointerClickListener.Get(headClick.gameObject).onClick = Live2dClickTigger;
        PointerClickListener.Get(bodyClick.gameObject).onClick = Live2dClickTigger;


        SetLive2dClickSize();

        HandleFunctionOpen();


        _countDownTra = transform.Find("UserInfoView/CountDown");
        PointerClickListener.Get(_countDownTra.gameObject).onClick = go =>
            {
                GalaAccountManager.Instance.ShowWallVisitorView();
            };

        //CheckDownLoadQueue();
    }

    public void SetCountDownTime(bool isShow,string time)
    {
        _countDownTra.gameObject.SetActive(isShow);
        _countDownTra.GetText("Txt").text = time;
    }
    
    private void OntotalRechargeBtn()
    {
        SendMessage(new Message(MessageConst.CMD_MAIN_ON_TOTALRECHARGE_BTN));
    }


    private void Start()
    {
        InitOpenArrowCondition();
        SetHeadImg();
    }

    public void SetHeadImg()
    {
        var userOther = GlobalData.PlayerModel.PlayerVo.UserOther;
        transform.GetRawImage("UserInfoView/HeadIconMask/HeadIcon").texture = 
            ResourceManager.Load<Texture>(GlobalData.DiaryElementModel.GetHeadPath(userOther.Avatar, ElementTypePB.Avatar));
        transform.GetRawImage("UserInfoView/HeadIconMask/HeadFrame").texture = 
            ResourceManager.Load<Texture>(GlobalData.DiaryElementModel.GetHeadPath(userOther.AvatarBox,ElementTypePB.AvatarBox));
    }
    
    private void Update()
    {
        CheckIsShowArrow();
    }


    public void InitOpenArrowCondition()
    {
        var curGuide = GuideManager.CurStage();
        if (curGuide== GuideStage.MainLine2_12_Over_Stage &&!GlobalData.LevelModel.FindLevel("2-4").IsPass && !PopupManager.IsOnBack())
        {
           _isStartCheckTouch = true; 
        }
        else
        {  
            _isStartCheckTouch = false;
        }


    }

    private void CheckIsShowArrow()
    {
        if (!_isStartCheckTouch)       
          return;  
      
         _checkTime += Time.deltaTime;          
         if ( _checkTime>=5.0f&&!_touchArrow.gameObject.activeSelf)
         {
           _touchArrow.gameObject.Show();               
         }  
         
         if (Input.GetMouseButtonDown(0))
         {
             if (EventSystem.current.currentSelectedGameObject != null)
             {

                 if (_checkTime>=5.0f&&EventSystem.current.currentSelectedGameObject.name=="StartBtn")
                 {
                     _isArrowGoto = true;
                 }

                 if (EventSystem.current.currentSelectedGameObject.name!="StartBtn")
                 {
                     _isArrowGoto = false;
                 }
                 
             }
            _checkTime = 0;            
             _touchArrow.gameObject.Hide();
             
         }
      
       
    }
   
    
    public void CheckDownLoadQueue()
    {
        CheckNeedToDownLoadBacnend();
        CheckNeedToDownLoadExtend();
    }


    private void ShowTriggerGift()
    {
        RandomEventManager.ShowGiftWindow();
    }


    public void IsShowFirstRechargeBtn(bool isShow)
    {
        isShow &= GuideManager.IsOpen(ModulePB.Activity, FunctionIDPB.FirstRush);
        _firstRechargeBtn.gameObject.SetActive(isShow);
        
        //todo 首冲结束后就显示累充 _accumulativeRechargeBtn.gameObject.SetActive(isShow);
        _totalRechargeBtn.gameObject.SetActive(!isShow);
        
    }

    private void IsShowActivityCapsuleTemplateBtn(bool isShow)
    {
     
        _activityCapsuleTemplateBtn.gameObject.SetActive(isShow);
    }

    private void IsShowActivityMusicTemplateBtn()
    {
        var isShow = GlobalData.ActivityModel.IsShowActivityTemplateBtn(ActivityTypePB.ActivityMusicGame);
        _activityMusicTemplateBtn.gameObject.SetActive(isShow);
    }
    
    

    private void IsShowActivityTemplateBtn(bool isShow)
    {
       _activityTemplateBtn.gameObject.SetActive(isShow);  
    }

    private void IsShowPlayerBirthdayBtn(bool isShow)
    {
        _birthdayBtn.gameObject.SetActive(isShow);
    }
    
    private void IsShowStarActivityBtn(bool isShow)
    {
        _starActivityBtn.gameObject.SetActive(isShow);
    }


    public void HandleFunctionOpen()
    {
      

//        _startButton.gameObject.SetActive(GuideManager.IsOpen(ModulePB.Start, FunctionIDPB.StartEntry));
//        _supporterBtn.gameObject.SetActive(GuideManager.IsOpen(ModulePB.Department, FunctionIDPB.DepartmentEntry));
//
//        _missionBtn.gameObject.SetActive(GuideManager.IsOpen(ModulePB.Mission, FunctionIDPB.MissionEntry));
//        _achievementBtn.gameObject.SetActive(GuideManager.IsOpen(ModulePB.Mission, FunctionIDPB.MissionStar));
//
//        _changeRoleBtn.gameObject.SetActive(GuideManager.IsOpen(ModulePB.Favorability, FunctionIDPB.FavorabilityView));
//        _fullviewBtn.gameObject.SetActive(GuideManager.IsOpen(ModulePB.FullView, FunctionIDPB.FullViewEntry));
//
//
//        _stagingPostBtn.gameObject.SetActive(GuideManager.IsOpen(ModulePB.Mall, FunctionIDPB.MallEntry));
//
//        _drawCardBtn.gameObject.SetActive(GuideManager.IsOpen(ModulePB.Draw, FunctionIDPB.DrawEntry));
//        _phoneBtn.gameObject.SetActive(GuideManager.IsOpen(ModulePB.Phone, FunctionIDPB.PhoneEntrance));
//
//        _mailBtn.gameObject.SetActive(GuideManager.IsOpen(ModulePB.Mail, FunctionIDPB.MailEntry));
//
//        _cardBtn.gameObject.SetActive(GuideManager.IsOpen(ModulePB.Card, FunctionIDPB.CardEntry));
//        _appointmentBtn.gameObject.SetActive(GuideManager.IsOpen(ModulePB.Love, FunctionIDPB.LoveEntry));
//
//        _fullviewBtn.gameObject.SetActive(GuideManager.IsOpen(ModulePB.FullView, FunctionIDPB.FullViewEntry));

        if (_backGround.gameObject.activeSelf && GuideManager.IsOpen(ModulePB.Activity, FunctionIDPB.ActivityEntry))
            _activityBar.gameObject.SetActive(true);


        IsShowFirstRechargeBtn(GlobalData.ActivityModel.IsShowFirstRechargeBtn());

      
        IsShowStarActivityBtn(GlobalData.MissionModel.IsShowStarActivity());
        IsShowPlayerBirthdayBtn(GlobalData.MissionModel.IsShowPlayerBirthday());
//        IsShowActivityTemplateBtn(GlobalData.ActivityModel.IsShowActivityTemplate());
//        IsShowActivityCapsuleTemplateBtn(GlobalData.ActivityModel.IsShowActivityCapsuleTemplate());
        IsShowActivityTemplateBtn(GlobalData.ActivityModel.IsShowActivityTemplateBtn(ActivityTypePB.ActivityDrawTemplate));
        IsShowActivityCapsuleTemplateBtn(GlobalData.ActivityModel.IsShowActivityTemplateBtn(ActivityTypePB.ActivityCapsuleTemplate));
        IsShowActivityMusicTemplateBtn();
    }


    public void ChangeBackground()
    {       
        var backgroundItemId = GlobalData.PlayerModel.PlayerVo.Apparel[1];
        var imageName = GlobalData.FavorabilityMainModel.GetBgImageName(backgroundItemId);
        _bgRawImage.texture = ResourceManager.Load<Texture>(AssetLoader.GetStoryBgImage(imageName));
        if (_bgRawImage.texture == null)
        {
            _bgRawImage.texture = ResourceManager.Load<Texture>(AssetLoader.GetStoryBgImage("mtl1"));
        }
    }


    private void SetLive2dClickSize()
    {
        Transform headClick = transform.Find("CharacterContainer/Live2dGraphic/Head");
        Transform bodyClick = transform.Find("CharacterContainer/Live2dGraphic/Body");
        int npcId = GlobalData.PlayerModel.PlayerVo.NpcId;
        if (npcId == 0) npcId = 1;
        headClick.GetComponent<RectTransform>().SetSize(NpcSize[(npcId - 1) * 2]);
        bodyClick.GetComponent<RectTransform>().SetSize(NpcSize[(npcId - 1) * 2 + 1]);
    }

    List<Vector2> NpcSize = new List<Vector2>
    {
        new Vector2(300, 400), new Vector2(485, 1412.4f),
        new Vector2(300, 400), new Vector2(534, 1457),
        new Vector2(300, 400), new Vector2(650, 1457),
        new Vector2(300, 400), new Vector2(503, 1474),
    };

    List<Vector2> NpcPos = new List<Vector2>
    {
        new Vector2(-9, 694), new Vector2(-13.3f, -253.8f), //tang
        new Vector2(5, 730), new Vector2(-34, -231.5f), //qin
        new Vector2(44, 730), new Vector2(20, -231.5f), //yan
        new Vector2(41, 739), new Vector2(-9.6f, -222.6f), //chi
    };

    private const string _mainTirggerGiftCountDown = "mainTirggerGiftCountDown";

    private void Live2dClickTigger(GameObject obj)
    {
        EXPRESSIONTRIGERTYPE eType = EXPRESSIONTRIGERTYPE.NO;
        if (obj.name == "Head")
        {
            eType = EXPRESSIONTRIGERTYPE.HEAD;
        }
        else if (obj.name == "Body")
        {
            eType = EXPRESSIONTRIGERTYPE.BODY;
        }
        else
        {
            eType = EXPRESSIONTRIGERTYPE.NORMAL;
        }

        if (isClick)
            return;
        if (AudioManager.Instance.IsPlayingDubbing)
            return;
        isClick = true;
        //ExpressionInfo expressionInfo = ClientData.GetRandomExpression(GlobalData.PlayerModel.PlayerVo.NpcId, eType);
        Live2dTigger(eType);
    }

    public void Live2dTigger(EXPRESSIONTRIGERTYPE eType, int labelId = -1, bool isSendClick = true)
    {
        //eType = EXPRESSIONTRIGERTYPE.LOVEDIARY;
        //labelId = 8004;
        L2DModel model = _live2dGraphic.GetMainLive2DView.Model;
        ExpressionInfo expressionInfo =
            ClientData.GetRandomExpression(GlobalData.PlayerModel.PlayerVo.NpcId, eType, labelId);
        if (expressionInfo == null)
        {
            Debug.Log("expressionInfo == null");
            isClick = false;
            return;
        }

        if (!model.IsIdle)
        {
            isClick = false;
            Debug.LogError("model  is busy  ");
            return;
        }

        if (isSendClick)
            SendMessage(new Message(MessageConst.CMD_MAIN_ON_LIVE2DCLICK));

        _live2dGraphic.GetMainLive2DView.LipSync = true;


        if (expressionInfo.Dialog == "")
        {
            Debug.Log("expressionInfo.Dialog == null");
            model.SetExpression(model.ExpressionList[expressionInfo.Id], 2);
            isClick = false;
            return;
        }

        string musicId = expressionInfo.Dialog;
        if (labelId != -1)
        {
            if (CacheManager.IsLoveDiaryNeedDown(musicId))
            {
                CacheManager.DownloadLoveDiaryCache(musicId, str =>
                {
                    Debug.LogError("DownloadLoveDiaryCache finish");
                    PlayDialog(musicId, expressionInfo.Id, model);
                }, () =>
                {
                    Debug.LogError("DownloadLoveDiaryCache error");
                    PlayDialog(musicId, expressionInfo.Id, model);
                });
            }
            else
            {
                PlayDialog(musicId, expressionInfo.Id, model);
            }

            //CacheVo cacheVo= CacheManager.CheckLoveDiaryCache(musicId);
            //if (cacheVo != null && cacheVo.needDownload)
            //{
            //    CacheManager.DownloadLoveDiaryCache(musicId, str =>
            //    {
            //        Debug.LogError("DownloadLoveDiaryCache finish");
            //        PlayDialog(musicId, expressionInfo.Id, model);
            //    }, ()=>{
            //        Debug.LogError("DownloadLoveDiaryCache error");
            //        PlayDialog(musicId, expressionInfo.Id, model);
            //    });
            //}
        }
        else
        {
            //new AssetLoader().LoadAudio(AssetLoader.GetMainPanleDialogById(expressionInfo.Dialog), //expressionInfo.Dialog),
            //(clip, loader) =>
            // {
            //     AudioManager.Instance.PlayDubbing(clip);
            //     Debug.Log("AudioManager.Instance.PlayDubbing");
            //    model.SetExpression(model.ExpressionList[expressionInfo.Id], clip.length + 1);
            // isClick = false;
            //});
            PlayDialog(musicId, expressionInfo.Id, model);
        }
    }


    void PlayDialog(string musicId, int expressionId, L2DModel model)
    {
        new AssetLoader().LoadAudio(AssetLoader.GetMainPanleDialogById(musicId), //expressionInfo.Dialog),
            (clip, loader) =>
            {
                AudioManager.Instance.PlayDubbing(clip);
                Debug.Log("AudioManager.Instance.PlayDubbing");
                model.SetExpression(model.ExpressionList[expressionId], clip.length + 1);
                isClick = false;
            });
    }


    public void ShowNumFavorability(int num = -1)
    {
        Debug.Log("GameConfigKey.ShowNumFavorability     " + num);
        Transform tf = transform.Find("FlyText/Num");
        int childCount = tf.childCount;
        int curIndex = 0;

        for (int i = curIndex; i < childCount; i++)
        {
            tf.GetChild(i).gameObject.Show();
        }

        if (num == 0)
        {
            tf.GetChild(curIndex).GetComponent<Image>().sprite =
                AssetManager.Instance.GetSpriteAtlas("UIAtlas_Common_NumFavorability");
            tf.GetChild(curIndex).GetComponent<Image>().SetNativeSize();
            curIndex++;
            tf.GetChild(curIndex).GetComponent<Image>().sprite =
                AssetManager.Instance.GetSpriteAtlas("UIAtlas_Common_Num0");
            tf.GetChild(curIndex).GetComponent<Image>().SetNativeSize();
            curIndex++;
        }
        else if (num > 0)
        {
            tf.GetChild(curIndex).GetComponent<Image>().sprite =
                AssetManager.Instance.GetSpriteAtlas("UIAtlas_Common_NumFavorability");
            tf.GetChild(curIndex).GetComponent<Image>().SetNativeSize();
            curIndex++;
            List<int> numlist = new List<int>();
            int curNum = num;
            while (curNum > 0)
            {
                numlist.Add(curNum % 10);
                curNum = curNum / 10;
            }

            for (int i = numlist.Count - 1; i >= 0; i--)
            {
                tf.GetChild(curIndex).GetComponent<Image>().sprite =
                    AssetManager.Instance.GetSpriteAtlas("UIAtlas_Common_Num" + numlist[i].ToString());
                tf.GetChild(curIndex).GetComponent<Image>().SetNativeSize();
                curIndex++;
            }
        }
        else
        {
            tf.GetChild(curIndex).GetComponent<Image>().sprite =
                AssetManager.Instance.GetSpriteAtlas("UIAtlas_Common_NumMax");
            tf.GetChild(curIndex).GetComponent<Image>().SetNativeSize();
            curIndex++;
        }

        for (int i = curIndex; i < childCount; i++)
        {
            tf.GetChild(i).gameObject.Hide();
        }

        FlyText.ShowFlyText(tf);
    }

    private void OnStagingPostBtn()
    {
        SendMessage(new Message(MessageConst.CMD_MAIN_ON_STAGINGPOST_BTN));
    }

    private void OnFullViewBtn()
    {
        ShowFullView();
    }

    private void ShowFullView()
    {
        _animator.Play("MainPanel");
        _isfullview = true;
        PointerClickListener.Get(_backGround.GetChild(0).gameObject).onClick = go => { BackNormalView(); };
    }

    public void HideAllRedPoint()
    {
        _supporterBtn.transform.Find("RedPoint").gameObject.SetActive(false);
        _drawCardBtn.transform.Find("RedPoint").gameObject.SetActive(false);
        _appointmentBtn.transform.Find("RedPoint").gameObject.SetActive(false);
        _phoneBtn.transform.Find("RedPoint").gameObject.SetActive(false);
        _missionBtn.transform.Find("RedPoint").gameObject.SetActive(false);
        _mailBtn.transform.Find("RedPoint").gameObject.SetActive(false);
        _activityBar.Find("RedDot").gameObject.SetActive(false);
        _startButton.transform.Find("RedPoint").gameObject.SetActive(false);
        _achievementBtn.transform.Find("RedPoint").gameObject.SetActive(false);

        _starActivityBtn.transform.Find("RedPoint").gameObject.SetActive(false);
         _birthdayBtn.transform.Find("RedPoint").gameObject.SetActive(false);
        _cardBtn.transform.Find("RedPoint").gameObject.SetActive(false);
        _stagingPostBtn.transform.Find("RedPoint").gameObject.SetActive(false);
        _activityTemplateBtn.transform.Find("RedPoint").gameObject.SetActive(false);
        _activityCapsuleTemplateBtn.transform.Find("RedPoint").gameObject.SetActive(false);
        _activityMusicTemplateBtn.transform.Find("RedPoint").gameObject.SetActive(false);
    }

    public void SetRedPoint(string msgKey)
    {


       
        
        switch (msgKey)
        {
            case Constants.PHONE_MSG_KEY:
                _phoneBtn.transform.Find("RedPoint").gameObject.SetActive(true);
                break;
            case Constants.LOVE_MSG_KEY:
                _appointmentBtn.transform.Find("RedPoint").gameObject.SetActive(true);
                //Util.SetIsRedPoint(Constants.REDPOINT_LOVE_BTN_LOVEAPPOINT, true); //todo临时使用这个来存储  后期优化
                break;
            case Constants.DRAW_MSG_KEY:
                _drawCardBtn.transform.Find("RedPoint").gameObject.SetActive(true);
                break;
            case Constants.FRIEND_MSG_KEY:
            case Constants.SUPPORTER_MSG_KEY:
                _supporterBtn.transform.Find("RedPoint").gameObject.SetActive(true);
                break;
            case Constants.MISSION_MSG_KEY:
                _missionBtn.transform.Find("RedPoint").gameObject.SetActive(true);
                break;
            case Constants.MAIL_MSG_KEY:
                _mailBtn.transform.Find("RedPoint").gameObject.SetActive(true);
                break;
            case Constants.ACTIVITY_MSG_KEY:
            case Constants.ACTIVITY_MISSION:
                _activityBar.Find("RedDot").gameObject.SetActive(true);
                break;
            case Constants.ACTIVITY_MSG_FIRSTRECHARGE:
                _firstRechargeBtn.transform.Find("RedPoint").gameObject.SetActive(true);
                break;
            case Constants.MISSION_STAR_ROAD_KEY:
                _achievementBtn.transform.Find("RedPoint").gameObject.SetActive(true);
                break;
            case Constants.ENCOURAGE_ACT:
                _startButton.transform.Find("RedPoint").gameObject.SetActive(true);
                break;
            case Constants.CARD_MSG_KEY:
                _cardBtn.transform.Find("RedPoint").gameObject.SetActive(true);
                break;
            case Constants.MISSION_STARRY_COVENANT:
                _starActivityBtn.transform.Find("RedPoint").gameObject.SetActive(true);
                break;
            case Constants.MISSION_CHI_YU_BIRTHDAY:
                _birthdayBtn.transform.Find("RedPoint").gameObject.SetActive(true);
                break;
            case Constants.SHOP_FREEMALL_MSG_KEY:
                _stagingPostBtn.transform.Find("RedPoint").gameObject.SetActive(true);
                break;
           case Constants.ACTIVITY_DRAW_TEMPLATE:
               _activityTemplateBtn.transform.Find("RedPoint").gameObject.SetActive(true);
                break;
            case Constants.ACTIVITY_CAPSULE_TEMPLATE:
                _activityCapsuleTemplateBtn.transform.Find("RedPoint").gameObject.SetActive(true);
                break;            
            case Constants.ACTIVITY_MUSIC:
                _activityMusicTemplateBtn.transform.Find("RedPoint").gameObject.SetActive(true);
                break;
            default:
                return;
        }
    }

    private void OnStarActivityBtn()
    {

        var isOpen = GuideManager.IsOpen(ModulePB.Mission, FunctionIDPB.SevenDaysAwardActivity);
        if (!isOpen)
        {
            var msg = GuideManager.GetOpenConditionDesc(ModulePB.Mission, FunctionIDPB.SevenDaysAwardActivity);
            FlowText.ShowMessage(msg);
           return; 
        }
        
        SendMessage(new Message(MessageConst.CMD_MAIN_ON_STAR_ACTIVITY_BTN));
    }

    private void ActivityCapsuleTemplateBtn()
    {
        SendMessage(new Message(MessageConst.CMD_MAIN_ON_ACTIVITYCAPSULETEMPLATE_BTN));
    }

    private void ActivityMusicTemplateBtn()
    {
        SendMessage(new Message(MessageConst.CMD_MAIN_ON_ACTIVITYMUSICTEMPLATE_BTN)); 
    }

    private void ActivityTemplateBtn()
    {
       SendMessage(new Message(MessageConst.CMD_MAIN_ON_ACTIVITYTEMPLATE_BTN));
    }
    
    private void OnBirthdayBtn()
    {
        SendMessage(new Message(MessageConst.CMD_MAIN_ON_PLAYERBIRTHDAY));
    }
    
    private void BackNormalView()
    {
        _animator.Play("MainPanelBack");

        _isfullview = false;
        PointerClickListener.Get(_backGround.GetChild(0).gameObject).onClick = null;
    }


    private void OnChangeRoleBtn()
    {
        var isOpen = GuideManager.IsOpen(ModulePB.Favorability, FunctionIDPB.FavorabilityView);
        if (!isOpen)
        {
            var msg = GuideManager.GetOpenConditionDesc(ModulePB.Favorability, FunctionIDPB.FavorabilityView);
            FlowText.ShowMessage(msg);
            return;
        }

        SendMessage(new Message(MessageConst.CMD_MAIN_ON_CHANGE_ROLE_BTN));
    }

    private void OnAppointmentBtn()
    {
        SendMessage(new Message(MessageConst.CMD_APPOINTMENT_JUMPCHOOSEROLE));
    }

    private void OnMissionBtn()
    {
        var isOpen = GuideManager.IsOpen(ModulePB.Mission, FunctionIDPB.MissionEntry);
        if (!isOpen)
        {
            var msg = GuideManager.GetOpenConditionDesc(ModulePB.Mission, FunctionIDPB.MissionEntry);
            FlowText.ShowMessage(msg);
            return;
        }
        
        SendMessage(new Message(MessageConst.CMD_TASK_SHOW_DAILYTASK));
    }

    private void OnAchievementBtn()
    {
        var isOpen = GuideManager.IsOpen(ModulePB.Mission, FunctionIDPB.MissionStar);
        if (!isOpen)
        {
            var msg = GuideManager.GetOpenConditionDesc(ModulePB.Mission, FunctionIDPB.MissionStar);
            FlowText.ShowMessage(msg);
             return;  
        }
        
        SendMessage(new Message(MessageConst.CMD_GOTOACHIEVEMENT));
    }

    private void BuyGem(GameObject go)
    {
        AudioManager.Instance.PlayEffect(AudioManager.Instance.DefaultButtonEffect);
        if (! GuideManager.IsPass1_9())
        {
            FlowText.ShowMessage(I18NManager.Get("Guide_Battle6", "1-9"));
            return;
        }

        SendMessage(new Message(MessageConst.CMD_MAIN_SHOW_BUY_GEM));
    }

    private void BuyGold(GameObject go)
    {
        AudioManager.Instance.PlayEffect(AudioManager.Instance.DefaultButtonEffect);
        if (! GuideManager.IsPass1_9())
        {
            FlowText.ShowMessage(I18NManager.Get("Guide_Battle6", "1-9"));
            return;
        }

        SendMessage(new Message(MessageConst.CMD_MAIN_SHOW_BUY_GOLD));
    }

    private void PowerHint(GameObject go)
    {
        AudioManager.Instance.PlayEffect(AudioManager.Instance.DefaultButtonEffect);
        FlowText.ShowMessage(I18NManager.Get("GameMain_Hint4",
            GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.RESTORE_INFO_POWER_ONE_TIME)));
    }

    private void BuyPower(GameObject go)
    {
        AudioManager.Instance.PlayEffect(AudioManager.Instance.DefaultButtonEffect);
        if (! GuideManager.IsPass1_9())
        {
            FlowText.ShowMessage(I18NManager.Get("Guide_Battle6", "1-9"));
            return;
        }


        SendMessage(new Message(MessageConst.CMD_MAIN_SHOW_BUY_POWER));
    }


    private void BuyRecolletionPower(GameObject go)
    {
        AudioManager.Instance.PlayEffect(AudioManager.Instance.DefaultButtonEffect);
        SendMessage(new Message(MessageConst.CMD_RECOLLECTION_SENDBUYEVENT));
    }


    private void BuyRecolletionPowerHint(GameObject go)
    {
        AudioManager.Instance.PlayEffect(AudioManager.Instance.DefaultButtonEffect);      
        FlowText.ShowMessage(I18NManager.Get("GameMain_Hint3",
            GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.RESTORE_MEMORIES_POWER_ONE_TIME)));
    }

    private void BuySupporterPower(GameObject go)
    {
        AudioManager.Instance.PlayEffect(AudioManager.Instance.DefaultButtonEffect);
        SendMessage(new Message(MessageConst.CMD_SUPPORTERACTIVITY_BUYENCOURAGEPOWER));
    }

    private void BuySupporterPowerHint(GameObject go)
    {
        FlowText.ShowMessage(I18NManager.Get("GameMain_Hint2",
            GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.RESTORE_ENCOURAGE_ACT_POWER_ONE_TIME)));
        AudioManager.Instance.PlayEffect(AudioManager.Instance.DefaultButtonEffect);
    }


//    private void ShowActivityWindow(GameObject go)
//    {
//        SendMessage(new Message(MessageConst.CMD_MAIN_ON_ACTIVITY_BTN, int.Parse(go.name)));
//    }


    private void OnMailBtn()
    {
        SendMessage(new Message(MessageConst.CMD_MAIN_ON_MAIL_BTN));
    }

    private void OnFirstRechargeBtn()
    {                    
        SendMessage(new Message(MessageConst.CMD_MAIN_ON_FIRITSRECHARGE_BTN));
    }


    private void OnCardBtn()
    {
        SendMessage(new Message(MessageConst.CMD_MAIN_ON_CARD_BTN));
    }


    private void OnDrawCardBtn()
    {
       var isOpen = GuideManager.IsOpen(ModulePB.Draw, FunctionIDPB.DrawEntry);
       if (!isOpen)
       {
           var msg = GuideManager.GetOpenConditionDesc(ModulePB.Draw, FunctionIDPB.DrawEntry);
           FlowText.ShowMessage(msg);
          return; 
       }
        
        SendMessage(new Message(MessageConst.CMD_MAIN_ON_DRAWCARD_BTN));
    }

    private void OnPhoneBtn()
    {
        SendMessage(new Message(MessageConst.CMD_MAIN_ON_PHONE_BTN));
    }

    private void PopupUserInfo(GameObject go)
    {
        var prefab = GetPrefab("GameMain/Prefabs/SetPanel");
        var item = Instantiate(prefab, gameObject.transform.parent, false) as GameObject;
        item.transform.localScale = Vector3.one;
        item.AddComponent<SetPanelView>();
        item.GetComponent<SetPanelView>().SetData(go.GetComponent<UserInfoView>().GetUserInfo());
    }

    private void OnGiftBtn()
    {
        SendMessage(new Message(MessageConst.CMD_MAIN_ON_GIFT_BTN));
    }

    private void OnSupporterBtn()
    {
        SendMessage(new Message(MessageConst.CMD_MAIN_ON_SUPPORTER_BTN));
    }

    private void OnStartBtn()
    {      
        Debug.LogError("_isArrowGoto======>"+_isArrowGoto);
        SendMessage(new Message(MessageConst.CMD_MAIN_ON_START_BTN, Message.MessageReciverType.CONTROLLER,_isArrowGoto));
    }


    public void ShowTopBar(MainMenuDisplayState state = MainMenuDisplayState.ShowTopBar)
    {
        _topBar.gameObject.SetActive(true);

        bool isPowerBar = state == MainMenuDisplayState.ShowTopBar;

        _topBar.Find("PowerBar").gameObject.SetActive(isPowerBar);


        if (isPowerBar)
        {
            _topBar.Find("VisitPowerBar").gameObject.SetActive(!isPowerBar);
            _recolletionBar.gameObject.SetActive(!isPowerBar);
            _exchangeIntegralBar.gameObject.SetActive(!isPowerBar);
        }
        else
        {
            switch (state)
            {
                case MainMenuDisplayState.ShowVisitTopBar:
                    _topBar.Find("VisitPowerBar").gameObject.SetActive(false);
                    _recolletionBar.gameObject.SetActive(false);
                    _exchangeIntegralBar.gameObject.SetActive(false);
                    break;
                case MainMenuDisplayState.ShowRecollectionTopBar:
                    _topBar.Find("VisitPowerBar").gameObject.SetActive(false);
                    _recolletionBar.gameObject.SetActive(true);
                    _exchangeIntegralBar.gameObject.SetActive(false);
                    break;
                case MainMenuDisplayState.ShowExchangeIntegralBar:
                    _topBar.Find("VisitPowerBar").gameObject.SetActive(false);
                    _recolletionBar.gameObject.SetActive(false);
                    _exchangeIntegralBar.gameObject.SetActive(true);
                    break;
            }
        }

        _userInfoView.gameObject.SetActive(false);
        _buttons.gameObject.SetActive(false);
        _activityBar.gameObject.SetActive(false);
        _startPathBtnContainer.gameObject.SetActive(false);

        _backGround.gameObject.Hide();
        _characterContainer.gameObject.Hide();
        _live2dGraphic.Hide();
    }


    public void ShowAll(bool isShow = true)
    {
        _topBar.gameObject.SetActive(isShow);

        _userInfoView.gameObject.SetActive(isShow);
        _buttons.gameObject.SetActive(isShow);
        _activityBar.gameObject.SetActive(isShow);
        _startPathBtnContainer.gameObject.SetActive(isShow);

        _backGround.gameObject.SetActive(isShow);
        _characterContainer.gameObject.SetActive(isShow);

        if (isShow && _isShowLive2d)
        {
            _live2dGraphic.Show();
        }
        else
        {
            _live2dGraphic.Hide();
        }


        ChangeBackground();

        if (isShow)
            HandleFunctionOpen();
    }

    public void ShowTopBarAndUserInfo()
    {
        _topBar.gameObject.SetActive(true);
        _userInfoView.gameObject.SetActive(true);

        _buttons.gameObject.SetActive(false);
        _activityBar.gameObject.SetActive(false);
        _startPathBtnContainer.gameObject.SetActive(false);

        _backGround.gameObject.Hide();
        _characterContainer.gameObject.Hide();

        _live2dGraphic.Hide();
    }

    public void ShowUserInfo()
    {
        _userInfoView.gameObject.SetActive(true);

        _topBar.gameObject.SetActive(false);
        _buttons.gameObject.SetActive(false);
        _activityBar.gameObject.SetActive(false);
        _startPathBtnContainer.gameObject.SetActive(false);

        _backGround.gameObject.Hide();
        _characterContainer.gameObject.Hide();

        _live2dGraphic.Hide();
    }

    public void SetSupporterPowerInfo(bool isSupporter)
    {
        _powerTxt.gameObject.SetActive(!isSupporter);
        _powerIconImage.gameObject.SetActive(!isSupporter);
        _powerAddImg.gameObject.SetActive(!isSupporter);
        _powerBar.GetComponent<RawImage>().enabled = !isSupporter;
    }

    public void SetRoleBg(int roleId)
    {
        ChangeRole(roleId);
    }

    public void SetLevel(int level)
    {
        _uiv.SetLevel(level);
    }

    public void SetData(PlayerVo vo)
    {
        _uiv.InitData(vo);

        _goldTxt.text = vo.Gold + "";
        _gemTxt.text = vo.Gem + "";

        _visitPowerTxt.text = vo.EncourageEnergy + "/" + vo.MaxEncourageEnergy;

        _powerTxt.text = vo.Energy + "/" + vo.MaxEnergy;

        _recolletionBar.GetText("Text").text = vo.RecollectionEnergy + "/" +
                                               GlobalData.ConfigModel.GetConfigByKey(GameConfigKey
                                                   .RESTORE_MEMORIES_POWER_MAX_SIZE); //90;
    }


    public void ExchangeIntegralBarSetData(long exchangeIntegral)
    {
        _exchangeIntegralBar.GetText("Text").text = exchangeIntegral.ToString();
    }

    public void UpdateExchangeIntegral(long exchangeIntegral)
    {
        _exchangeIntegralBar.GetText("Text").text = exchangeIntegral.ToString();
    }


    public void SetMainLive2d(PlayerVo vo)
    {
        if (vo.Apparel[0] <= 0 || !L2DModel.HasResource(vo.Apparel[0].ToString()))
        {
            _isShowLive2d = false;
            _live2dGraphic.Hide();
            return;
        }

        ChangeRole(vo.Apparel[0]);
        if (GuideManager.CurStage()!= GuideStage.MainLine1_1Level_1_3Level_Stage)
        {
            Live2dTigger(EXPRESSIONTRIGERTYPE.LOGIN, -1, false);
        } 
       
    }

    private void ChangeRole(int id)
    {
        _isShowLive2d = true;
        _live2dGraphic.Show();
        var live2dName = id.ToString();      
        if (_extendCacheVo != null && _extendCacheVo.needDownload)
        {
            var curStep = GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide);
            if (curStep >= GuideConst.MainLineStep_OnClick_FavorabilityShowMainViewBtn)            
            {
                //老号，要读取默认的！
                _live2dGraphic.Hide();
                
                _live2dGraphic.LoadAnimationById(GlobalData.PlayerModel.PlayerVo.Apparel[0].ToString());
            }
            else
            {
                _live2dGraphic.LoadAnimationById(live2dName);
            }
        }
        else
        {
            //_cacheVo = CacheManager.CheckExtendCache();
            _live2dGraphic.LoadAnimationById(live2dName);
        }

//        Debug.LogError("load!");

        L2DModel model = _live2dGraphic.GetMainLive2DView.Model;
        model.StartMotion(L2DConst.MOTION_GROUP_IDLE, 0, L2DConst.PRIORITY_IDLE, true);
        model.SetExpression(model.ExpressionList[2]);
        model.StartEyeBlink();

        SetLive2dClickSize();

        SdkHelper.PushAgent.PushCreate30DaysNotice();
    }

    public void OnShow()
    {
        _uiv.OnShow();
        ChangeBackground();
        var curStep = GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide);

        //如果能保证新手引导要使用的资源提前达到包里的话，该判断条件可以去掉
        if ( curStep >= GuideConst.MainLineStep_OnClick_FavorabilityShowMainViewBtn&&_backDownLoading )                      
        {
            Debug.LogError("Check");
            DownloadBackEnd();
        }

        CheckNeedToDownLoadExtend();
        RefreshActivityImageAndActivityPage();
        InitOpenArrowCondition();
    }

    

    public void ChangeTriggerGift(bool show, int giftCount = 0, long maturityTime = -1)
    {
        _triggerGiftBtn.gameObject.SetActive(show);

        Text numText = _triggerGiftBtn.transform.GetText("Image/Text");
        numText.text = giftCount + "";

        if (show)
        {
            _triggerGiftText.text = DateUtil.GetTimeFormat3(maturityTime - ClientTimer.Instance.GetCurrentTimeStamp());

            ClientTimer.Instance.AddCountDown(_mainTirggerGiftCountDown, long.MaxValue, 1, count =>
            {
                if (maturityTime - ClientTimer.Instance.GetCurrentTimeStamp() <= 0)
                {
                    ChangeTriggerGift(false);
                    return;
                }

                _triggerGiftText.text =
                    DateUtil.GetTimeFormat3(maturityTime - ClientTimer.Instance.GetCurrentTimeStamp());
            }, null);
        }
        else
        {
            ClientTimer.Instance.RemoveCountDown(_mainTirggerGiftCountDown);
        }
    }

    private void OnDestroy()
    {
        ClientTimer.Instance.RemoveCountDown(_mainTirggerGiftCountDown);
    }
}