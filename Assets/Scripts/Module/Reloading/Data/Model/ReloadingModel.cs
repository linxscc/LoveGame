using System;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using game.main;
using System.Collections.Generic;
using System.Diagnostics;
using Assets.Scripts.Module.Framework.Utils;
using UnityEngine;
using Debug = UnityEngine.Debug;

public enum ReloadingListState 
{
    Clothing,
    Backgroud,
}

/// <summary>
/// 换装Model
/// </summary>
public class ReloadingModel :Model {

 
    private List<ReloadingVO> _backgrounds =new  List<ReloadingVO>();
    private List<ReloadingVO> _cloths = new List<ReloadingVO>();
    
    
    /// <summary>
    /// 构造函数
    /// </summary>
    public ReloadingModel()
    {
        Init();
        
    }

    private void Init()
    {
        var rules = GlobalData.FavorabilityMainModel.DressUpUnlockRuleLists;
        var curRole = GlobalData.FavorabilityMainModel.CurrentRoleVo;

        var clothItemId = curRole.Apparel[0];
        var backgroundItemId = curRole.Apparel[1];
       
        foreach (var t in rules)
        {
            switch (t.ItemType)
            {
                case DressUpTypePB.TypeClothes:
                    if (t.Player==curRole.Player)
                    {                       
                        ReloadingVO vo1 = new ReloadingVO(t);
                        vo1.IsPitchOn = vo1.ItemId==clothItemId;
                        _cloths.Add(vo1);
                    }                 
                    break;
                case DressUpTypePB.TypeBackground:
                    ReloadingVO vo2 = new ReloadingVO(t);
                    vo2.IsPitchOn = vo2.ItemId==backgroundItemId;
                    _backgrounds.Add(vo2);
                    break;               
            } 
        }
        _cloths.Sort();
        _backgrounds.Sort();

        SetSpecialBackgroundRedFrame();
    }


    private void SetSpecialBackgroundRedFrame()
    {
        foreach (var t in _backgrounds)
        {
            if (t.IsPitchOn)
            {
               return; 
            }
        }

        _backgrounds[0].IsPitchOn = true;

    }


    public List<ReloadingVO> GetList(ReloadingListState state)
    {
        if (state == ReloadingListState.Clothing)
        {
            return _cloths;
        }
        else
        {
            return _backgrounds;
        }
    }


    public ReloadingVO GetData(int itemId,ReloadingListState state)
    {
        ReloadingVO vo = null;
        switch (state)
        {
            case ReloadingListState.Clothing:
                foreach (var t in _cloths)
                {
                    if (t.ItemId==itemId)
                    {
                         vo = t;
                         break;
                    }
                } 
                break;
            case ReloadingListState.Backgroud:
                foreach (var t in _backgrounds)
                {
                    if (t.ItemId==itemId)
                    {
                         vo = t;
                        break;
                    }
                } 
                break;           
        }

        return vo;
    }

    public void UpdateListItemRedFrameShow(int itemId ,ReloadingListState state)
    {
        switch (state)
        {
            case ReloadingListState.Clothing:
                foreach (var item in _cloths)
                {
                    item.IsPitchOn = item.ItemId == itemId;
                    if (item.ItemId==itemId && item.IsGet)
                    {
                        var key = GlobalData.PlayerModel.PlayerVo.UserId.ToString() + item.ItemId;
                         PlayerPrefs.SetInt(key,1);
                         item.IsShowRedDot = false;
                    }
                }
                break;
            case ReloadingListState.Backgroud:
                foreach (var item in _backgrounds)
                {
                    item.IsPitchOn = item.ItemId == itemId;
                    if (item.ItemId==itemId && item.IsGet)
                    {
                        var key = GlobalData.PlayerModel.PlayerVo.UserId.ToString() + item.ItemId;
                        PlayerPrefs.SetInt(key,1);
                        item.IsShowRedDot = false;
                    }
                }
                break;          
        }
    }


    public string GetBgImagePath(int backDrop)
    {
       
        int index=0;
        var dt = DateUtil.GetTodayDt();
        float minute = dt.Hour * 60 + dt.Minute; //转成分钟计算 1h=60m
        if (0<minute && minute <=6*60f)   //0点~6点
        {
            index = 2;
        }
        else if(6 * 60f < minute && minute <= 16 * 60f)//6点~16点
        {
            index = 0; 
        }
        else if (16 * 60f < minute && minute <= 19 * 60f) //16点~19点
        {
            index = 1;
        }
        else if (19 * 60f < minute && minute <= 24 * 60f)//19点~24点
        {
            index = 2; 
        }

       
        return Images(backDrop)[index];    
        
    }


    private List<string> Images(int itemId)
    {
        foreach (var t in _backgrounds)
        {
            if (t.ItemId == itemId)
            {
                return t.BgImages;
            }
        }
        
        
        return _backgrounds[0].BgImages;
    }

  
    
    






















}

















