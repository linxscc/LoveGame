using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;
using DataModel;
using game.main;
using game.tools;
using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Download;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SetPanelView : View {

    private GameObject blankOnClickArea;        
    private PlayerVo data; 
    private RawImage _headIcon;
    
    private Text _name;
    private Text _id;
    private Text _level;
    private Text _exp;
    private ProgressBar _progressBar;


    private Button _changeBtn;   //更换头像
    private Button _modifyBtn;  //修改名字
    private Button _copyIdBtn;  //复制Id   
    
    private Transform _bottomBg;
    private Button _setVolumeBtn;   //设置音量
    private Button _userCenterBtn;  //用户中心
    private Button _cdKeyBtn;       //兑换码
    private Button _customServiceBtn;  //客服中心
    private Button _switchAccountBtn; //退出登录
    private Button _downloadBtn;     //下载

    private HeadModel _headModel;
    private RectTransform _starRect;
    
    private RectTransform _mask;
    private void Awake()
    {      
        
        blankOnClickArea = transform.Find("BlankOnClickArea").gameObject;               
        _headIcon = transform.GetRawImage("Bg/Head/HeadIconMask/HeadIcon");

        _name = transform.GetText("Bg/Name/Bg/NameText");
        _id = transform.GetText("Bg/ID/Bg/IDText");
        _level = transform.GetText("Bg/LevelInfo/Level");
        _exp = transform.GetText("Bg/LevelInfo/Exp");
        _progressBar = transform.GetProgressBar("Bg/LevelInfo/ProgressBar");
        _mask = _progressBar.transform.GetRectTransform("Mask");
        _starRect = _progressBar.transform.GetRectTransform("Star");
        _changeBtn = transform.GetButton("Bg/Head/ChangeHeadBtn");
        _modifyBtn = transform.GetButton("Bg/Name/Bg/ModificationBtn"); 
        _copyIdBtn = transform.GetButton("Bg/ID/Bg/CopyBtn");
        
        _changeBtn.onClick.AddListener(ChangeHead);
        _modifyBtn.onClick.AddListener(ShowModificationNameWindow);             
        _copyIdBtn.onClick.AddListener(CopyUserId);
               
        _bottomBg = transform.Find("Bg/BottomBg");
        
        _setVolumeBtn = _bottomBg.GetButton("SetVolumeBtn");
        _userCenterBtn = _bottomBg.GetButton("UserCenterBtn");
        _cdKeyBtn =_bottomBg.GetButton("CDkeyBtn");
        _customServiceBtn = _bottomBg.GetButton("CustomServiceBtn");    
        _switchAccountBtn = _bottomBg.GetButton("SwitchAccountBtn");      
        _downloadBtn = _bottomBg.GetButton("DownloadBtn");
        
        _setVolumeBtn.onClick.AddListener(ShowSetVolumeWindow);
        _userCenterBtn.onClick.AddListener(UserCenterBtn);
        _cdKeyBtn.onClick.AddListener(ShowCDKeyWindow);
        _customServiceBtn.onClick.AddListener(SdkHelper.CustomServiceAgent.Show);       
        _switchAccountBtn.onClick.AddListener(ShowConfirmWindow);
        _downloadBtn.onClick.AddListener(Download);
            
        var redDot = _userCenterBtn.gameObject.transform.Find("RedDot").gameObject;
        redDot.SetActive(GlobalData.PlayerModel.PlayerVo.IsGuset);
        
        _userCenterBtn.gameObject.SetActive(AppConfig.Instance.UseGalaLogin);
        _cdKeyBtn.gameObject.SetActive(AppConfig.Instance.SwitchControl.Code);
        _customServiceBtn.gameObject.SetActive(AppConfig.Instance.SwitchControl.CustomerServices);
                        
        PointerClickListener.Get(blankOnClickArea.gameObject).onClick = go => { Destroy(gameObject);};
           
    }

    private void ChangeHead()
    {
             
        var win = PopupManager.ShowWindow<SetHeadWindow>("GameMain/Prefabs/SetHeadWindow");   
        win.SetData(_headModel);
        win.WindowActionCallback = evt =>
        {
            if (evt == WindowEvent.Ok)
            {
                SetHeadImg();
               
            }     
            SetHeadRedDot();
        };
    }
    
    private void SetHeadImg()
    {
        var userOther = GlobalData.PlayerModel.PlayerVo.UserOther;
        transform.GetRawImage("Bg/Head/HeadIconMask/HeadIcon").texture = 
            ResourceManager.Load<Texture>(GlobalData.DiaryElementModel.GetHeadPath(userOther.Avatar, ElementTypePB.Avatar));
        transform.GetRawImage("Bg/Head/HeadIconMask/HeadFrame").texture = 
            ResourceManager.Load<Texture>(GlobalData.DiaryElementModel.GetHeadPath(userOther.AvatarBox,ElementTypePB.AvatarBox));
    }
    
    private void ShowModificationNameWindow()
    {       
        var win = PopupManager.ShowWindow<ModificationNameWindow>("GameMain/Prefabs/ModificationNameWindow");
        win.SetData(data);      
    }
    
    private void ShowSetVolumeWindow()
    {
        PopupManager.ShowWindow<SetVolumeWindow>("GameMain/Prefabs/SetVolumeWindow");
    }
    
    private void UserCenterBtn()
    {       
        GalaAccountManager.Instance.ShowUserCenter(Channel.LoginType());
    }
    
    private void ShowCDKeyWindow()
    {
            PopupManager.ShowWindow<CDKeyWindow>("GameMain/Prefabs/CDKeyWindow");     
    }
    
    private void ShowConfirmWindow()
    {
        PopupManager.ShowConfirmWindow(I18NManager.Get("GameMain_SetPanelHint3"), I18NManager.Get("Common_OK2")).WindowActionCallback = evt =>
        {
            if (evt == WindowEvent.Ok)
            {
                Debug.LogWarning("GalaAccountManager.Instance.IsLogin = "+GalaAccountManager.Instance.IsLogin);
                EventDispatcher.TriggerEvent<bool>(EventConst.ForceToLogin,true);
                GalaAccountManager.Instance.GameLoginHasChange(false,AppConfig.Instance.serverId,GlobalData.PlayerModel.PlayerVo.UserId.ToString()); 
                SdkHelper.AccountAgent.Logout();
                
                               
                 
//                if (AppConfig.Instance.channelInfo == Channel.ChannelInfo_4399)
//                {
//                    ClientTimer.Instance.DelayCall(() =>
//                    {
//                        SdkHelper.AccountAgent.Logout();
//                    }, 1.0f);
//                }
//                else
//                {
//                    SdkHelper.AccountAgent.Logout();
//                }
            }
        };
    }
    
    private void Download()
    {
        bool isDownloadAll = AppConfig.Instance.version+"DownloadAll"==PlayerPrefs.GetString("IsDownloadAll");      
        Debug.LogError(isDownloadAll);           
        var  window=PopupManager.ShowWindow<DownloadAllWindow>("GameMain/Prefabs/DownloadAllWindow");
        if (isDownloadAll)
        {
            window.SetData(true);  
        }
        else
        {             
            if (CacheManager.GetDownloadAllSize()!=0)
            {
                var size =Math.Round(CacheManager.GetDownloadAllSize()*1f/1048576f,2) ;
                window.SetData(false,I18NManager.Get("GameMain_DownloadWindow3",size));     
            }
            else
            {
                window.SetData(true); 
                PlayerPrefs.SetString("IsDownloadAll","FinishDownloadAll");
            }              
        }   
        
        window.WindowActionCallback = evt =>
        {
            if (evt== WindowEvent.Yes)
            {                   
                CacheManager.DownloadAllAudio((s =>
                {    
                    PlayerPrefs.SetString("IsDownloadAll",AppConfig.Instance.version +"DownloadAll");   
                    FlowText.ShowMessage(I18NManager.Get("GameMain_SetPanelDownloadFinishHint1"));
                }));
            }
        };
        
    }

    private void CopyUserId()
    {
        string result = string.Format(_id.text);
        TextEditor editor = new TextEditor();
        editor.content = new GUIContent(result);
        editor.OnFocus();
        editor.Copy();
        FlowText.ShowMessage(I18NManager.Get("GameMain_CopyIdSucceed"));
    }

   
    private void Start()
    {
        EventDispatcher.AddEventListener(EventConst.UpDataSetPanelName, UpDataSetPanelName);   
    }
    
    private void OnDestroy()
    {
        EventDispatcher.RemoveEvent(EventConst.UpDataSetPanelName);
    }
    
    private void UpDataSetPanelName()
    {
        SetData(GlobalData.PlayerModel.PlayerVo);
    }

     
    public void SetData(PlayerVo vo)
    {
        data = vo;
       
        _name.text = vo.UserName;
        _id.text = vo.UserId.ToString();
        _level.text = I18NManager.Get("Common_Level", vo.Level);
    
        _exp.text = I18NManager.Get("Common_LevelProportion", vo.CurrentLevelExp, vo.NeedExp);
        _progressBar.Progress = (int) ((float) vo.CurrentLevelExp / vo.NeedExp * 100);

        SetStarPos();
        
        _headModel =new HeadModel();
        SetHeadImg();
        SetHeadRedDot();
    }


    private void SetStarPos()
    {
        _starRect.anchoredPosition = new Vector2(_mask.GetWidth()-10,0);
    }
    
    private void SetHeadRedDot()
    {
        var redDot = _changeBtn.transform.Find("Red").gameObject;
        var isShow = _headModel.IsShowHeadFrameRedDot();
        redDot.SetActive(isShow);
        
    }
}
