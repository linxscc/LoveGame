using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Componets;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

public class FavorabilityGiftView : View
{

    private Transform _bottomBg;
    private GameObject _getDesc;   //获得途径描述
    private Transform _giftParent;
    private Button _giveGiftsBtn;
    private LongPressButton _giveGiftsLongPressBtn;
    private GameObject _itemDesc;

    
    private int _maxLevel;
    private int _addValue;
    private int _tempCurLevel;
    private int _tempCurExp;
    private int _tempCurNeedExp;
    private int _tempCurShowExp;
    private int _tempCurShowNeedExp;
    private int _giveNum;
    private int _itemNum;
    private GameObject _curGiveGiftsObj;
    private int _curIndex;
    private Vector3[] _contentV3 = new Vector3[4];
       
    private Transform _levelTra;
    private GameObject _onClick;
    /// <summary>
    /// 是否得到远端服务器Data
    /// </summary>
    public bool IsGetRemoteServerData = true;
    
    private bool _isShowHint = true;  
    private bool _isShowFullLevel = false;
    
    private void Awake()
    {
        
        _onClick = transform.Find("OnClick").gameObject;
        _bottomBg = transform.Find("GiftContent");
        _bottomBg.GetRectTransform().GetWorldCorners(_contentV3);
        
        _getDesc = _bottomBg.Find("HintText").gameObject;
        _giftParent = _bottomBg.Find("GiveGiftsList/Content");
        _giveGiftsBtn = _bottomBg.GetButton("GiveGiftsBtn");
        _giveGiftsLongPressBtn = _giveGiftsBtn.gameObject.GetComponent<LongPressButton>();
        _itemDesc = _bottomBg.Find("HintContent/Hint").gameObject;
        _maxLevel = GlobalData.FavorabilityMainModel.GetLatsFavorabilityLevelRulePB().Level;
        _giveGiftsLongPressBtn.OnDown = OnDown;
        _giveGiftsLongPressBtn.OnUp = OnUp;
        SteOnClikcHideHint();
    }

    private void Start()
    {
        InitTempData();
    }

    public void SetLevelTra(Transform tra)
    {
        _levelTra = tra;       
    }

    private void OnDown()
    {
        if(!IsGetRemoteServerData) {return;}

        if (_isShowHint)
        {
            HideHint();
            _isShowHint = false;
        }

        if (_tempCurLevel == _maxLevel)
        {
            if (_isShowFullLevel)
            {
                FlowText.ShowMessage(I18NManager.Get("FavorabilityMain_FullLevel"));
                _isShowFullLevel = false;
            }

            _levelTra.GetText("ProgressText").text = GlobalData.FavorabilityMainModel.GetLastExp().ToString();
            _levelTra.Find("ProgressBar").GetComponent<ProgressBar>().DeltaX = 0;
            _levelTra.Find("ProgressBar").GetComponent<ProgressBar>().ProgressY = 0;
            _levelTra.GetText("Image/LevelText").text = _maxLevel.ToString();
            return;
        }
        
        _giveNum++;
        _itemNum--;
        _tempCurExp += _addValue;
        _tempCurShowExp += _addValue;

        if (_tempCurExp >= _tempCurNeedExp)
        {
            _tempCurExp = _tempCurExp - _tempCurNeedExp;
            _tempCurLevel++;
            _tempCurNeedExp = GlobalData.FavorabilityMainModel.GetCurrentLevelExpNeed(_tempCurLevel);
            _tempCurShowNeedExp = GlobalData.FavorabilityMainModel.GetCurrentLevelRule(_tempCurLevel).Exp;
            _levelTra.GetText("Image/LevelText").text = _tempCurLevel.ToString();
            UpgradeAni();
        }
        _levelTra.GetText("ProgressText").text=_tempCurShowExp + "/" + _tempCurShowNeedExp;
        _levelTra.Find("ProgressBar").GetComponent<ProgressBar>().DeltaX = 0;
        _levelTra.Find("ProgressBar").GetComponent<ProgressBar>().ProgressY = (int) ((float) _tempCurExp / _tempCurNeedExp * 100);

        if (_itemNum <= 0)
        {
            DestroyImmediate(_giftParent.GetChild(_curIndex).gameObject);
            SendMessage(new Message(MessageConst.CMD_DISIPOSITION_REMO_GIVEGIFTS_ITEM, _curIndex)); //发送移除集合的元素
            SendMessage(new Message(MessageConst.CMD_FAVORABILITY_GIVEGIFTS_BTN, _giveNum)); //发送消耗数量的请求 
            _giveNum = 0;
            _isShowHint = true;
            _levelTra.GetText("Image/LevelText").text = _tempCurLevel.ToString(); 
            return;
        }
        _curGiveGiftsObj.transform.GetText("ItemImage/NumText").text = _itemNum.ToString();
    }
    
    private void OnUp()
    {
        if (_giveNum > 0)
        {
            SendMessage(new Message(MessageConst.CMD_FAVORABILITY_GIVEGIFTS_BTN, _giveNum));
            _giveNum = 0;
        }
        
        _isShowHint = true;
        _isShowFullLevel = true;
    }

   
    public void RefreshGiveGiftsItemRedFrameShow(List<FavorabilityGiveGiftsItemVO> list)
    {
        for (int i = 0; i < _giftParent.childCount; i++)
        {
            _giftParent.GetChild(i).GetComponent<FavorabilityGiveGiftsItem>().SetData(list[i]);
        }
    }

    /// <summary>
    /// 生成送礼Item
    /// </summary>
    /// <param name="list"></param>
    public void CreateGiveGiftsItems(List<FavorabilityGiveGiftsItemVO> list)
    {
        var giveGiftsPrefab = GetPrefab("FavorabilityMain/Prefabs/GiveGiftsItem/GiveGiftsItems");
        foreach (var t in list)
        {
            var item = Instantiate(giveGiftsPrefab, _giftParent, false); 
            item.name = t.ItemId.ToString();
            item.GetComponent<FavorabilityGiveGiftsItem>().SetData(t);
        }
    }


    public void GetCurPitchOnGiveGiftsItem(int index, FavorabilityGiveGiftsItemVO vo)
    {
        _curIndex = index;
        _curGiveGiftsObj = _giftParent.GetChild(index).gameObject;  
        _itemNum = int.Parse(_curGiveGiftsObj.transform.GetText("ItemImage/NumText").text);
        SetCurGiveGiftsItemCalculateOffset(CalculateOffset()); 
        ShowHint(CalculateOffset(), vo.ItemDesc);
        AddExp(vo);
    }

    /// <summary>
    /// 是否显示获得礼物途径描述
    /// </summary>
    /// <param name="count"></param>
    public void IsShowHintText(int count)
    {
        bool isZero = count == 0;
        
        if (isZero)        
           _getDesc.Show();        
        else       
            _getDesc.Hide();

        _giveGiftsBtn.interactable = false;
        _giveGiftsLongPressBtn.enabled = false;
        _giveGiftsBtn.transform.GetText("Text").text = I18NManager.Get("Common_Presented");
    }

    /// <summary>
    /// 是否有选中道具
    /// </summary>
    /// <param name="isOn"></param>
    public void IsPitchOnItem(bool isOn)
    {
        _giveGiftsBtn.interactable = isOn;
        _giveGiftsLongPressBtn.enabled = isOn;
        _giveGiftsBtn.transform.GetText("Text").text = I18NManager.Get(isOn ? "Common_Presented" : "FavorabilityMain_ChoiceGift");
    }
    
    
    /// <summary>
    /// 初始化临时数据
    /// </summary>
    private void InitTempData()
    {
        _addValue = 0;
        _tempCurLevel = 0;
        _tempCurExp = 0;
        _tempCurNeedExp = 0;
        _tempCurShowExp = 0;
        _tempCurShowNeedExp = 0;
        _giveNum = 0;
        _tempCurLevel = GlobalData.FavorabilityMainModel.CurrentRoleVo.Level;
        _tempCurExp = GlobalData.FavorabilityMainModel.CurrentRoleVo.Exp;
        _tempCurNeedExp = GlobalData.FavorabilityMainModel.GetCurrentLevelExpNeed(_tempCurLevel);
        _tempCurShowExp = GlobalData.FavorabilityMainModel.CurrentRoleVo.ShowExp;
        _tempCurShowNeedExp = GlobalData.FavorabilityMainModel.GetCurrentLevelRule(_tempCurLevel).Exp;
    }

    private void SetCurGiveGiftsItemCalculateOffset(Vector3 vector3)
    {
        var itemRect = _curGiveGiftsObj.GetComponent<RectTransform>();
        Vector3[] item = new Vector3[4];
        itemRect.GetWorldCorners(item);
        var itemLeftPeak = item[0].x;
        var itemRightPeak = item[3].x;
        var screenWidth = Screen.width;
        
        if (itemLeftPeak < _contentV3[0].x)
        {
            var offset = -vector3.x * vector3.z;
            var anchoredPosition = _giftParent.GetRectTransform().anchoredPosition;
            anchoredPosition =
                new Vector2((anchoredPosition.x) + offset, (anchoredPosition.y));
            _giftParent.GetRectTransform().anchoredPosition = anchoredPosition;
        }

        if (itemRightPeak > _contentV3[3].x)
        {
            var offset = (vector3.y - screenWidth) * vector3.z;
            var anchoredPosition = _giftParent.GetRectTransform().anchoredPosition;
            anchoredPosition =
                new Vector2((anchoredPosition.x) - offset, (anchoredPosition.y));
            _giftParent.GetRectTransform().anchoredPosition = anchoredPosition;
        }
        
    }

    private Vector3 CalculateOffset()
    {
        var itemRect = _curGiveGiftsObj.GetComponent<RectTransform>();
        var itemWidth = itemRect.GetWidth();
        var curLeftPeck = RectTransformUtility.WorldToScreenPoint(Camera.main, _curGiveGiftsObj.transform.position).x;
        var localPosition = _curGiveGiftsObj.transform.localPosition;
        var tempV3 = new Vector3(localPosition.x + itemWidth,localPosition.y, localPosition.z);  
        var curRightPeck = RectTransformUtility.WorldToScreenPoint(Camera.main, _curGiveGiftsObj.transform.parent.TransformPoint(tempV3)).x;
        var scale = itemWidth / ((float) (curRightPeck - curLeftPeck));
        Vector3 vector3 = new Vector3(curLeftPeck, curRightPeck, scale);
        return vector3;
    }

    private void ShowHint(Vector3 vector3, string hintStr)
    {
        var rect = _itemDesc.GetComponent<RectTransform>(); 
        var leftPeak = vector3.x;
        var rightPeak = vector3.y;
        var scale = vector3.z;
        var width = rightPeak - leftPeak;
        var centerPos = leftPeak + width / 2;
        var screenWidth = Screen.width;
        var max = width * 2;
        
        if (centerPos < max || centerPos < 0)
        {
            rect.pivot = new Vector2(0, 0.5f);
            rect.anchoredPosition = new Vector2(0, 0);
            rect.anchorMin = new Vector2(0, 0.5f);
            rect.anchorMax = new Vector2(0, 0.5f);
        }
        else if (screenWidth - max < centerPos && centerPos < screenWidth || centerPos > screenWidth)
        {
            rect.pivot = new Vector2(1, 0.5f);
            rect.anchoredPosition = new Vector2(0, 0);
            rect.anchorMin = new Vector2(1, 0.5f);
            rect.anchorMax = new Vector2(1, 0.5f);
        }
        else if (max <= centerPos || centerPos < screenWidth - max)
        {
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = new Vector2(centerPos * scale, 0);
            rect.anchorMin = new Vector2(0, 0.5f);
            rect.anchorMax = new Vector2(0, 0.5f);
        }
        _itemDesc.Show();
        _itemDesc.transform.Find("Text").GetComponent<Text>().text = hintStr;
    }

    private void  SteOnClikcHideHint()
    {
        PointerClickListener.Get(_onClick).onClick = go => { HideHint(); };
        PointerClickListener.Get(_bottomBg.gameObject).onClick = go => { HideHint(); };
    }

    private void AddExp(FavorabilityGiveGiftsItemVO vo)
    {
        var pb = GlobalData.FavorabilityMainModel.CurrentRoleVo.Player;
        _addValue = 0;
        switch (vo.GradeType)
        {
            case FavorabilityGiveGiftsItemVO.ItemGradeType.ExclusiveBasicsItem:
                if (IsCorrespondence(vo.PlayerPBs, pb)){_addValue = vo.Exp + vo.Power;}               
                else{ _addValue = vo.Power;}
                break;
            case FavorabilityGiveGiftsItemVO.ItemGradeType.GMBasicsItem:
                _addValue = vo.Power;
                break;
            case FavorabilityGiveGiftsItemVO.ItemGradeType.ExclusiveAdvancedItem:
                if (IsCorrespondence(vo.PlayerPBs, pb)){_addValue = vo.Exp + vo.Power;}            
                else{ _addValue = vo.Power;}
                break;
            case FavorabilityGiveGiftsItemVO.ItemGradeType.GMAdvancedItem:
                _addValue = vo.Power;
                break;           
        }
    }
    
    
    /// <summary>
    ///送礼是否是对应的角色
    /// </summary>
    private bool IsCorrespondence(List<PlayerPB> playerPBs, PlayerPB curPB)
    {
        bool temp = false;
        foreach (var t in playerPBs)
        {
            if (t == curPB)
            {
                temp = true;
                break;
            }
        }

        return temp;
    }
    
    public void HideHint()
    {
        _itemDesc.Hide(); 
        _itemDesc.transform.Find("Text").GetComponent<Text>().text = "";
    }

    private void UpgradeAni()
    {
        FlowText.ShowMessage(I18NManager.Get("FavorabilityMain_Hint3")); // ("好感度升级");
        Tweener numScale = _levelTra.Find("Image/LevelText").DOScale(1.3f, 0.2f);
        Tweener scaleBack = _levelTra.Find("Image/LevelText").DOScale(1f, 0.2f);
        _levelTra.Find("Image/LoveHeartShine").GetComponent<ParticleSystem>().Play();
        DOTween.Sequence().Append(numScale).Append(scaleBack);   
    }
    
    
    /// <summary>
    /// 更新伪数据
    /// </summary>
    public void UpDataDummyData(int itemNum, int curExp, int curLevel, int myCurExp)
    {
        _itemNum = itemNum;
        _tempCurLevel = curLevel;
        _tempCurNeedExp = GlobalData.FavorabilityMainModel.GetCurrentLevelExpNeed(curLevel);

        _tempCurShowNeedExp = GlobalData.FavorabilityMainModel.GetCurrentLevelRule(curLevel).Exp;

        _levelTra.GetText("Image/LevelText").text = _tempCurLevel.ToString();

        if (curLevel == _maxLevel)
        {
            _tempCurExp = GlobalData.FavorabilityMainModel.GetLastExp();

            _levelTra.GetText("ProgressText").text = GlobalData.FavorabilityMainModel.GetLastExp().ToString();
            _levelTra.Find("ProgressBar").GetComponent<ProgressBar>().DeltaX = 0;
            _levelTra.Find("ProgressBar").GetComponent<ProgressBar>().ProgressY = 0;
        }
        else
        {
            _tempCurExp = curExp;
            _tempCurShowExp = myCurExp;

            _levelTra.GetText("ProgressText").text = _tempCurShowExp + "/" + _tempCurShowNeedExp;
            _levelTra.Find("ProgressBar").GetComponent<ProgressBar>().DeltaX = 0;
            _levelTra.Find("ProgressBar").GetComponent<ProgressBar>().ProgressY = (int) ((float) _tempCurExp / _tempCurNeedExp * 100);
        }

    }
}
