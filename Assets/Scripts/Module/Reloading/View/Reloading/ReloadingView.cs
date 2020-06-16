using System;
using System.Collections.Generic;
using System.Net.Mime;
//using System.Resources;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts.Framework.GalaSports.Service;
using DG.Tweening;
using game.main.Live2d;
using GalaAccount.Scripts.Framework.Utils;
using GalaAccountSystem;
using QFramework;


/// <summary>
/// 换装View
/// </summary>
public class ReloadingView : View
{    
    private RawImage _bgBG;          
    private Button _shareBtn;
    private Button _saveBtn; 
    private Transform _tabBar;


    private Transform _clothParent;
    private Transform _backgroundParent;
    
    private Live2dGraphic _live2DGraphic;
    
   

    private GameObject _aniOnClickControl;
    private Transform _clothHint;

    
    private ReloadingVO _lastOnClickCloth = null;
    private ReloadingVO _lastOnClickBackground = null;

  
    private void Awake()
    {
        _live2DGraphic = transform.Find("CharacterContainer/Live2dGraphic").GetComponent<Live2dGraphic>();
        if ((float) Screen.height / Screen.width > 1.8f)
        {
            _live2DGraphic.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        } 
        
        _bgBG = transform.Find("BG").GetComponent<RawImage>();     
        _shareBtn = transform.Find("Btn/ShareBtn").GetComponent<Button>();
        _saveBtn = transform.Find("Btn/SaveBtn").GetComponent<Button>();
        
        _shareBtn.onClick.AddListener(OnShareBtn);
        _saveBtn.onClick.AddListener(OnSaveBtn);
       
       
        
        _tabBar = transform.Find("TabBar");
        for (int i = 0; i < _tabBar.childCount; i++)
        {
            Toggle toggle = _tabBar.GetChild(i).GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(OnTabChange);
        }
        
        
        
        _aniOnClickControl = transform.Find("AniOnClickControl").gameObject;
        PointerClickListener.Get(_aniOnClickControl).onClick = go =>
        {           
            var slideContainers = transform.GetRectTransform("SlideContainers");
            _tabBar.GetRectTransform().DOAnchorPos(new Vector2(0,-329f), 0.5f);
            slideContainers.DOAnchorPosX(208, 0.5f);

            var btn = transform.GetRectTransform("Btn");
            btn.DOAnchorPosY(0, 0.5f);
           
            // _tabBar.GetChild(0).localPosition = new Vector2(0, _tabBar.GetChild(0).localPosition.y);
            //  _tabBar.GetChild(1).localPosition = new Vector2(0, _tabBar.GetChild(1).localPosition.y);
        };
        
        
        _clothHint = transform.Find("ClothHint");
      
        
        _clothHint.gameObject.Hide();
        


        _clothParent = transform.Find("SlideContainers/ClothingList/Content");
        _backgroundParent = transform.Find("SlideContainers/BackgroundList/Content");

        SetShowShareBtn();

    }

    private void SetShowShareBtn()
    { _shareBtn.gameObject.SetActive(AppConfig.Instance.SwitchControl.Share);}
    
    
    
    UserFavorabilityVo _vo = null;

    public void SetInfo(UserFavorabilityVo vo,string bgImagePath)
    {
        _vo = vo;
        PresonBGTexture(vo.Apparel[0].ToString());
        BGTexture(bgImagePath);
        var isShow = GlobalData.PlayerModel.PlayerVo.IsGetShareAward(ShareTypePB.ShareClothes);
        
        
        
        SetShareTipsShow(!isShow);
    }


    public void CreateClothsAndBackgrounds(List<ReloadingVO> cloths,List<ReloadingVO> backgrounds)
    {
                              
        var prefab = GetPrefab("Reloading/Item/ReloadingItem");

        foreach (var vo in cloths)
        {
            var clothItem = Instantiate(prefab, _clothParent, false);
            clothItem.transform.localScale = Vector3.one;
            clothItem.name = vo.ItemId.ToString();
            clothItem.GetComponent<ReloadingItem>().SetData(vo);
        }

        foreach (var vo in backgrounds)
        {
            var background = Instantiate(prefab, _backgroundParent, false);
            background.transform.localScale = Vector3.one;
            background.name = vo.ItemId.ToString();  
            background.GetComponent<ReloadingItem>().SetData(vo);
        }
               
    }


    public void RefreshRedFrameShow(List<ReloadingVO> list,ReloadingListState state )
    {
        switch (state)
        {
            case ReloadingListState.Clothing:
                for (int i = 0; i < _clothParent.childCount; i++)
                {
                    _clothParent.GetChild(i).GetComponent<ReloadingItem>().SetData(list[i]); 
                }                
                break;
            case ReloadingListState.Backgroud:
                for (int i = 0; i < _backgroundParent.childCount; i++)
                {
                    _backgroundParent.GetChild(i).GetComponent<ReloadingItem>().SetData(list[i]); 
                }
                break;          
        }
    }
 



    private void OnTabChange(bool isOn)
    {
        if (isOn == false) { return; }          
        string name = EventSystem.current.currentSelectedGameObject.name;       
        switch (name)
        {
            case "Clothing":
              //  _tabBar.GetChild(0).localPosition = new Vector2(0, _tabBar.GetChild(0).localPosition.y);
             //   _tabBar.GetChild(1).localPosition = new Vector2(25, _tabBar.GetChild(1).localPosition.y);

                _clothParent.parent.gameObject.Show();
                _backgroundParent.parent.gameObject.Hide();
             
                ResetBackground(); 
                       
                break;
            case "Background":
              //  _tabBar.GetChild(0).localPosition = new Vector2(25, _tabBar.GetChild(0).localPosition.y);
             //   _tabBar.GetChild(1).localPosition = new Vector2(0, _tabBar.GetChild(1).localPosition.y);

                _clothParent.parent.gameObject.Hide();
                _backgroundParent.parent.gameObject.Show();

                ResetCloth(); 
                
                break;         
        }

        SetAni();
    }

    private void SetAni()
    {
        _tabBar.GetRectTransform().DOAnchorPos(new Vector2(-340,-329f), 0.5f); 
       var slideContainers = transform.GetRectTransform("SlideContainers");
        var btn = transform.GetRectTransform("Btn");
        slideContainers.DOAnchorPosX(-208, 0.5f);
       btn.DOAnchorPosY(-430, 0.5f);
     
       
   }
    
    
    
    
    private void BGTexture( string image)
    {
        _bgBG.texture = ResourceManager.Load<Texture>(AssetLoader.GetStoryBgImage(image), ModuleName);
    }
    
    private void PresonBGTexture(string image)
    {
        _live2DGraphic.LoadAnimationById(image);        
    }
    void SetShareTipsShow(bool isShow)
    {

        if (!AppConfig.Instance.SwitchControl.Share)
        {
             Debug.LogError("No Open Share");
             _shareBtn.transform.Find("ShareTips").gameObject.SetActive(false);
             return;
        }
        
        _shareBtn.transform.Find("ShareTips").gameObject.SetActive(isShow);
        var rule = GlobalData.PlayerModel.ShareRules.Find((pb => pb.ShareType == ShareTypePB.ShareClothes));
       
        var num = rule.Awards[0].Num;
        _shareBtn.transform.Find("ShareTips/Text").GetText().text =  I18NManager.Get("Share_Cloth",num);
        var type = rule.Awards[0].Resource;

        switch (type)
        {
            case ResourcePB.Gem:
                _shareBtn.transform.Find("ShareTips/Gem").gameObject.SetActive(true);
                break;
            case ResourcePB.Gold:
                _shareBtn.transform.Find("ShareTips/Gold").gameObject.SetActive(true);
                break;
        }


    }
    private void OnShareBtn()
    {
        if (_vo == null)
            return;

       
        
        int backId = _vo.Apparel[1];
        int clothId = _vo.Apparel[0];
        if(_lastOnClickBackground != null)
        {
            if (!_lastOnClickBackground.IsGet)
            {
                FlowText.ShowMessage(I18NManager.Get("Reloading_BackgroundNounlock"));
                return;
            }
            backId = _lastOnClickBackground.ItemId;
        }
        if(_lastOnClickCloth != null)
        {
            if(!_lastOnClickCloth.IsGet)
            {
                FlowText.ShowMessage(I18NManager.Get("Reloading_ClothNounlock"));
                return;
            }
            clothId = _lastOnClickCloth.ItemId;
        }
    
        SdkHelper.ShareAgent.ShareCloth(backId, clothId);
        // FlowText.ShowMessage(I18NManager.Get("Common_Underdevelopment"));     
        // SendMessage(new Message(MessageConst.MODULE_FACORABLILITY_SHOW_SAVEANDSHARE_VIEW));
        SetShareTipsShow(false);
    }
 
    private void OnSaveBtn()
    {    
        SendMessage(new Message(MessageConst.CMD_FACORABLILITY_VIEW_ONSAVE_BTN));
    }


    private void ResetCloth()
    {
        if (_lastOnClickCloth!=null && _lastOnClickCloth.IsGet==false) //把衣服切回原来的
        {
            PresonBGTexture(_curRole.Apparel[0].ToString());
            SendMessage(new Message(MessageConst.CMD_RELOADING_RESET_CLOTH_RED_FRAME,_curRole.Apparel[0]));
            _clothHint.gameObject.Hide();
            _lastOnClickCloth = null;
        }  
        
    }

    private void ResetBackground()
    {
        if (_lastOnClickBackground!=null &&_lastOnClickBackground.IsGet==false)//把背景切回原来的
        {
            BGTexture(_model.GetBgImagePath(_curRole.Apparel[1]));
            SendMessage(new Message(MessageConst.CMD_RELOADING_RESET_BACKGROUND_RED_FARME,_curRole.Apparel[1])); 
            _clothHint.gameObject.Hide();
            _lastOnClickBackground = null;
        }       
    }

    private ReloadingModel _model;
    private UserFavorabilityVo _curRole;
    
    public void ShowHint(ReloadingVO vo,UserFavorabilityVo cur,ReloadingModel model,string bgImage=null)
    {
        _curRole = cur;
        _model = model;
        var clothHintTxt = _clothHint.GetText("Text");
        if (vo.ItemType == DressUpTypePB.TypeClothes)
        {
            _lastOnClickCloth = vo;           
            if (vo.IsGet==false)
            {
                _clothHint.gameObject.Show();
                clothHintTxt.text = I18NManager.Get("Reloading_Hint1", vo.UnlockDesc);
            }
            else
            {
                _clothHint.gameObject.Hide();             
                clothHintTxt.text = ""; 
            }
               
            PresonBGTexture(vo.ItemId.ToString());
        }

        if (vo.ItemType == DressUpTypePB.TypeBackground)
        {
            _lastOnClickBackground = vo;
            if (vo.IsGet==false)
            {
                _clothHint.gameObject.Show();
                clothHintTxt.text= I18NManager.Get("Reloading_Hint2",vo.UnlockDesc);
            }
            else
            {                     
                _clothHint.gameObject.Hide();
                clothHintTxt.text = "";
            }
            
            BGTexture(bgImage);
        }

    }
     
}
