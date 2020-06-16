using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Com.Proto;
using Common;
using DataModel;
using UnityEngine;

public class ActivityExchangeShopModel : Model
{
    private List<ActivityMallRulePB> _exchangeShopRules;
    private List<ActivityExchangeShopVo> _exchangeShopUserInfo;

    /// <summary>
    /// 兑换道具Id
    /// </summary>
    public int ExchangeItemId;


    /// <summary>
    /// 兑换道具Img路径
    /// </summary>
    public string ExchangeItemImgPath;
    
    /// <summary>
    /// 活动兑换商店用户数据
    /// </summary>
    public List<ActivityExchangeShopVo> GetUserData => _exchangeShopUserInfo;


    private ActivityVo _curActivity;
    
    public ActivityExchangeShopModel(ActivityVo curActivity)
    {
        _curActivity = curActivity;
        InitRule(curActivity.ActivityId);       
        InitUserInfo(curActivity);
        InitExchangeItemInfo(curActivity.ActivityExtra.ItemId);
    }

    /// <summary>
    /// 初始化兑换商店规则
    /// </summary>
    /// <param name="activityId"></param>
    private void InitRule(int activityId)
    {
        _exchangeShopRules =new List<ActivityMallRulePB>();
        var baseRule = GlobalData.ActivityModel.BaseTemplateActivityRule.ActivityMallRules;
        foreach (var t in baseRule)
        {
            if (t.ActivityId==activityId)
            {               
              _exchangeShopRules.Add(t);  
            }   
        }
    }

    private void InitExchangeItemInfo(int exchangeItemId)
    {
        ExchangeItemId = exchangeItemId;
        ExchangeItemImgPath="Prop/"+exchangeItemId;
    }
    
    /// <summary>
    /// 初始化兑换商店用户信息
    /// </summary>
    /// <param name="curActivity"></param>
    private void InitUserInfo(ActivityVo curActivity)
    {
        _exchangeShopUserInfo =new List<ActivityExchangeShopVo>();
        var baseUserInfo = GlobalData.ActivityModel.GetActivityTemplateListRes(curActivity.ActivityType).UserBuyActivityMalls;
        foreach (var t in _exchangeShopRules)
        {
            var vo = new ActivityExchangeShopVo(t);
            _exchangeShopUserInfo.Add(vo);
        }

        var isNull = baseUserInfo == null;
        if (!isNull)
        {
            foreach (var t in baseUserInfo)
            {
                if (t.ActivityId == curActivity.ActivityId)
                {
                    UpdateUserData(curActivity.ActivityId, t);
                } 
            } 
        }
    }

    /// <summary>
    /// 更新用户数据
    /// </summary>
    /// <param name="activityId"></param>
    /// <param name="pb"></param>
    public void UpdateUserData(int activityId, UserBuyActivityMallPB pb)
    {
        foreach (var t in _exchangeShopUserInfo)
        {
            if (t.ActivityId==activityId && t.MallId ==pb.MallId)
            {
                t.BuyNum = pb.BuyNum;
                t.RemainBuyNum = t.BuyMax - t.BuyNum;
                Debug.LogError("t-------------->"+t.MallId);
                break;
            }
        }

        GlobalData.ActivityModel.UpdateActivityExchangeShop(_curActivity.ActivityType, pb);
    }

    
}
