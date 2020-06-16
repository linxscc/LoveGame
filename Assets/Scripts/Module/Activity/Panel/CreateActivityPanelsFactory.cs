using System;
using Assets.Scripts.Framework.GalaSports.Core;
using UnityEngine;

public class CreateActivityPanelsFactory  {

    
    public static  Panel CreateActivityPanels(ActivityType type)
    {
        Panel panel = null;
        switch (type)
        {
            case ActivityType.ActivitySevenDaySignin:
                panel = new ActivitySevenSigninPanel();
                break;
            case ActivityType.ActivityEveryDayPower:
                panel = new ActivityEveryPowerSigninPanel();
                break;
            case ActivityType.ActivityMonthSignin:
                panel = new ActivityMonthSigninPanel();
                break;
            case ActivityType.ActivityGrowthFund:
                panel = new ActivityGrowthCapitalPanel();
                break;
            case ActivityType.ActivityMonthCard:
                panel = new ActivityMonthCardPanel();
                break;
            case ActivityType.ActivityDailyGift:
                panel = new ActivityDailyGiftPanel();
                break;       
            case ActivityType.ActivityAccumulativeRecharge:
                panel=new CumulativeRechargePanel();
                break;            
            case ActivityType.ActivitySevenDaySigninTemplate:
                panel = new ActivitySevenSigninTemplatePanel();
                break;
            case ActivityType.ActivityTenDrawCard:
                panel = new ActivityDrawCardPanel();
                break;
               
        }

        return panel;
    }
    
//    public static Panel CreateActivityPanels(ActivityTypePB typePB)
//    {
//        Panel panel = null;
//        switch (typePB)
//        {
//            case ActivityTypePB.ActivityTenDaySig:
//                panel = new ActivityTenDaysSigninPanel();
//                break;
//            case ActivityTypePB.ActivitySevenDaySig:
//                
//                panel = new ActivitySevenSigninPanel();
//                break;
//            case ActivityTypePB.ActivityPowerGet:
//                panel = new ActivityEveryPowerSigninPanel();
//                break;
//            case ActivityTypePB.ActivityMonthSign:
//                panel = new ActivityMonthSigninPanel();
//                break;
//            case ActivityTypePB.ActivityGrowthFund:
//                panel = new ActivityGrowthCapitalPanel();
//                break;
//            case ActivityTypePB.ActivityMonthCard:
//                panel = new ActivityMonthCardPanel();
//                break;
//            case ActivityTypePB.ActivityDailyGift:
//                panel = new ActivityDailyGiftPanel();
//                break;
//           case ActivityTypePB.ActivityDeleteTest:
//                panel =new  ActivityDeleteTestPanel();
//               break;
//           case  ActivityTypePB.ActivityFaceBookAttention:
//               break;
////           case ActivityTypePB.ActivityMonthCardRecharge:
//////               panel = new ActivityMonthCardPanel();
////               break;
//           case ActivityTypePB.ActivityAccumulativeRecharge:
//               panel=new CumulativeRechargePanel();
//               break;
//            case ActivityTypePB.ActivityDrawTenContinuous:
//                panel = new ActivityDrawCardPanel();
//                break;
//            case ActivityTypePB.ActivitySevensigninTemplate:
//                panel = new ActivitySevenSigninTemplatePanel();
//                break;
//           
//        }
//        return panel;
//    }

   



    

}
