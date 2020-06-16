using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Framework.Utils;
using DataModel;
using UnityEngine;

public class ActivityPopupWindowData
{       
    public string ModuleName; 
    public int Sort;          
    public int Group;        
    public string Name;       
    public string ImgPath;    
    public bool IsShow=false;
    public bool IsCanJumpTo=false;
    public string PromptDesc=string.Empty;// 提示信息
    public string ActivityJumpId=string.Empty;  //活动里的跳转字段
    public string PopupType;
    
    public ActivityPopupWindowData(ActivityPopWindowVo vo)
    {
        Name = vo.Name;
        Sort = vo.Sort;
        Group = vo.Group;
        ImgPath = vo.Path;
        ModuleName = vo.ModuleName;
        ActivityJumpId = vo.ActivityJumpToId;
        PopupType = vo.PopupType;       
        Init(vo);       
        if(!IsCanJumpTo){PromptDesc = I18NManager.Get("ActivityCapsuleTemplate_noStartTips",DateUtil.GetYMDD(vo.StarTimeStamp));}             
    }

    private void Init(ActivityPopWindowVo vo)
    {
        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        switch (vo.PopupType)
        {
           case "CapsuleTemplate":            //扭蛋战斗模板                       
           case "SevenSigninTemplate":        //七日签到模板                        
           case "DrawTemplate":               //抽卡活动模板
           case "MusicTemplate":
           case "DrawCard":  
               IsShow = IsShowToRule(vo, curTimeStamp);
               IsCanJumpTo = curTimeStamp > vo.StarTimeStamp;  
               break;
           case "FirstRecharge":
               IsShow = IsShowFirstPrize();
               IsCanJumpTo = IsShow;
               break;
           case "SevenSignin":
               IsShow = IsShowSevenDaySig();
               IsCanJumpTo = IsShow;
               break;
           case "MonthCard":
               IsShow = IsShowMonthCard();
               IsCanJumpTo = IsShow;
               break;
           case "GrowthFund":
               IsShow = IsShowGrowthFund();
               IsCanJumpTo = IsShow;
               break;
           case "StarActivity":
               IsShow = IsShowStarActivity();
               IsCanJumpTo = IsShow;
               break;          
        } 
    }

 
    //是否显示根据规则时间
    private bool IsShowToRule(ActivityPopWindowVo vo, long curTimeStamp)
    {
        bool isShow = false;
        bool isAdvance = vo.AdvanceDay != 0;

        if (isAdvance)  //有提前
        {
            var advanceTimeStamp = vo.AdvanceDay* 86400000;
            var advanceStarTimeStamp =vo.StarTimeStamp-advanceTimeStamp;
            if (advanceStarTimeStamp<=curTimeStamp && curTimeStamp<vo.EndTimeStamp)            
                isShow = true;            
        }
        else
        {
            if (vo.StarTimeStamp <= curTimeStamp && curTimeStamp < vo.EndTimeStamp)            
                isShow = true;           
        }               
        return isShow;
    }

    //是否显示首充
    private bool IsShowFirstPrize()
    {
        return GlobalData.PlayerModel.PlayerVo.ExtInfo.FirstPrize == FirstPrizeStatusPB.FpNotInvolved;
    }

    //是否显示七日签到（用户）
    private bool IsShowSevenDaySig()
    {
        bool isShow = false;
        var curActivity = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivitySevenDaySignin); 
        var isNull = curActivity == null;
       if (!isNull&&!curActivity.IsActivityPastDue)
       {
           isShow = true;
       }
        return isShow;
    }

    //是否显示月卡
    private bool IsShowMonthCard()
    {
        bool isShow = false;
        if (AppConfig.Instance.SwitchControl.Recharge)
        {
            var isVip = GlobalData.PlayerModel.PlayerVo.IsOnVip;
            var curActivity =GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivityMonthCard);
            if (curActivity!=null &&!isVip)
            {
                isShow = true;
            }
        }
        
        return isShow;
    }

    //是否显示应援储备金
    private bool IsShowGrowthFund()
    {
        bool isShow = false;
        var isBuy = GlobalData.PlayerModel.PlayerVo.ExtInfo.GrowthFund;  //0是没购买，1是购买过的   
        var curActivity =GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivityGrowthFund);
        if (isBuy==0&&curActivity!=null)
        {
            isShow = true;
        }
        return isShow;
    }

    //是否显示星动之约
    private bool IsShowStarActivity()
    {       
        return  GlobalData.MissionModel.IsShowStarActivity();
    }
    
}
