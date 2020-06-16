using Com.Proto;
using DataModel;
using System.Collections.Generic;
using System.Linq;

public class SevenDaysLoginAwardVO
{

    public int ActivityId;
    public int DayId;
    public int IsSelect;  //0是固定奖励1是可选奖励

    public string IconPath; //Icon图片路径
    public bool IsShowGetBtn;    //是否显示领取Btn
    public bool IsShowGetMask;   //是否显示已领取Mask

    public bool IsCardAward = false;   //是否是卡
    public bool IsGiftbag = false;    //是否是礼包
    public bool IsPuzzle = false;
    
    
    public List<RewardVo> Rewards = new List<RewardVo>();

    public string GiftbagName;

   


    public SevenDaysLoginAwardVO(ActivityOptionalAwardRulePB pB)
    {
        
        ActivityId = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivitySevenDaySignin).ActivityId;
        DayId = pB.Id;
        IsSelect = pB.IsSelect;
     
        if (IsSelect==0)
        {
            AddAwards(pB.FixedAward.ToList());          
        }
        else if(IsSelect==1)
        {
            AddAwards(pB.OptionalAward.ToList());          
        }

        if (Rewards.Count==0)
        {
            IsGiftbag = true;
            IconPath = "Prop/GiftPack/tongyong6";
            GiftbagName = I18NManager.Get("Common_GiftBag");
        }
        else if(Rewards.Count>1)
        {
            IsGiftbag = true;
            var award= Rewards[0];
            if (award.Resource== ResourcePB.Item)
            {
                var prop = GlobalData.PropModel.GetPropBase(award.Id);
                if (prop!=null)
                {                 
                    if(prop.ExpType==8)
                    {
                        IconPath = "Prop/GiftPack/dali";  
                        GiftbagName = I18NManager.Get("Common_StarUpGiftBag");
                    }
                }
                else
                {
                    IconPath = "Prop/GiftPack/tongyong6";
                    GiftbagName = I18NManager.Get("Common_GiftBag");
                }
            }
            else if(award.Resource== ResourcePB.Power)
            {
                IconPath = "Prop/GiftPack/dati";  
                GiftbagName = I18NManager.Get("Common_VitalityGiftBag");
            }            
        }
        else if(Rewards.Count==1)
        {
            IsCardAward = Rewards[0].Resource == ResourcePB.Card;
            IsPuzzle = Rewards[0].Resource == ResourcePB.Puzzle;
            IconPath = Rewards[0].IconPath;
        }
        
        
        IsShowGetBtn = false;    
        IsShowGetMask = false;
        
    }
    
    
    private void AddAwards(List<AwardPB> pBs)
    {
        foreach (var t in pBs)
        {
            var vo = new RewardVo(t);        
            Rewards.Add(vo);
        }
    }






 



}


