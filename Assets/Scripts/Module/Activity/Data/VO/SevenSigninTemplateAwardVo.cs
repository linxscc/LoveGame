using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Com.Proto;
using DataModel;
using UnityEngine;

public class SevenSigninTemplateAwardVo
{

    public int ActivityId;
    public int DayId;
    public int IsSelect; //0是固定奖励1是可选奖励

    public string IconPath; //Icon图片路径
    public bool IsShowGetBtn; //是否显示领取Btn
    public bool IsShowGetMask; //是否显示已领取Mask

    public bool IsCardAward = false; //是否是卡
    public bool IsGiftbag = false; //是否是礼包
    public bool IsPuzzle = false;


    public List<RewardVo> Rewards = new List<RewardVo>();

    public string GiftbagName;

    public string LastName;
    public string LastIconPath;
    public Vector2 LastIconV2;
    
    public SevenSigninTemplateAwardVo(ActivityOptionalAwardRulePB pB)
    {
        ActivityId = GlobalData.ActivityModel.GetActivityVo(ActivityType.ActivitySevenDaySigninTemplate).ActivityId;
        DayId = pB.Id;
        IsSelect = pB.IsSelect;

        if (IsSelect == 0)
        {
            AddAwards(pB.FixedAward.ToList());
        }
        else if (IsSelect == 1)
        {
            AddAwards(pB.OptionalAward.ToList());
        }

        if (Rewards.Count == 0)
        {
            IsGiftbag = true;
            IconPath = "Prop/GiftPack/tongyong6";
            GiftbagName = I18NManager.Get("Common_GiftBag");
        }
        else if (Rewards.Count > 1)
        {
            IsGiftbag = true;
            var award = Rewards[0];
            if (award.Resource == ResourcePB.Item)
            {
                var prop = GlobalData.PropModel.GetPropBase(award.Id);
                if (prop != null)
                {
                    if (prop.ExpType == 8)
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
            else if (award.Resource == ResourcePB.Power)
            {
                IconPath = "Prop/GiftPack/dati";
                GiftbagName = I18NManager.Get("Common_VitalityGiftBag");
            }
            else if (award.Resource == ResourcePB.Memories)
            {
                IconPath = "Prop/GiftPack/tongyong2";
                GiftbagName = I18NManager.Get("Common_MemoriesGiftBag");
            }
            else if (award.Resource == ResourcePB.Gem)
            {
                IconPath = "Prop/GiftPack/tongyong2";
                GiftbagName = I18NManager.Get("Common_GemGiftBag");
            }
           
        }
        else if (Rewards.Count == 1)
        {
            IsCardAward = Rewards[0].Resource == ResourcePB.Card;
            IsPuzzle = Rewards[0].Resource == ResourcePB.Puzzle;
            IconPath = Rewards[0].IconPath;
        }

        IsShowGetBtn = false;
        IsShowGetMask = false;
        SetLastIconData();
    }


    private void AddAwards(List<AwardPB> pBs)
    {
        foreach (var t in pBs)
        {
            var vo = new RewardVo(t);
            Rewards.Add(vo);
        }
    }


    private void SetLastIconData()
    {
        if (DayId==7)
        {
            if (IsGiftbag)
            {
                var award = Rewards[0];
                if (award.Resource == ResourcePB.Memories)
                {
                    LastName =I18NManager.Get("Common_MemoriesGiftBag");
                    LastIconPath = "Activity/LastIconhuiyigift";
                    LastIconV2 =new Vector2(1041,673);
                }
            }
            else
            {
                if (IsCardAward)
                {
                    var id = Rewards[0].Id;
                    LastName = GlobalData.CardModel.GetCardBase(id).CardName; 
                    LastIconPath = "Activity/LastIconhuiyigift";
                    LastIconV2 =new Vector2(1041,673);
                }
            }
        }
        
    }
    
    private string SetName(int ResourceId)
    {
        var _id = ResourceId / 1000;
        string preson = string.Empty;
        switch (_id)
        {
            case 1:
                preson = I18NManager.Get("Common_Role1");
                break;
            case 2:
                preson = I18NManager.Get("Common_Role2");
                break;
            case 3:
                preson = I18NManager.Get("Common_Role3");
                break;
            case 4:
                preson = I18NManager.Get("Common_Role4");
                break;           
        }

        return preson;
    }


}
