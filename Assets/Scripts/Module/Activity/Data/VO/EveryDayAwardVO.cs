
using Com.Proto;
using DataModel;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EveryDayAwardVO
{
    public int Num;
    public ResourcePB ResourcePB;
    public string Name;
    public string ImagePath;
    public int ResourceId;
    public EveryDayAwardVO(AwardPB pB)
    {
        Num = pB.Num;
        ResourcePB = pB.Resource;
        ImagePath = pB.ResourceId.ToString();
        ResourceId = pB.ResourceId;
        InitData(pB);
       // Debug.LogError("Name：" + Name + "；ResourcePB：" + ResourcePB + "；Num：" + Num);
    }

    private void InitData(AwardPB pB)
    {
        
        switch (ResourcePB)
        {
            case ResourcePB.Card:              
                Name = GlobalData.CardModel.GetCardBase(pB.ResourceId).CardName;
                ImagePath = "Card/Image/"+ ResourceId;
                break;            
            case ResourcePB.Item:
                Name = GlobalData.PropModel.GetPropBase(pB.ResourceId).Name;
                ImagePath = "Prop/" + ResourceId;
                break;
            case ResourcePB.Power:
                Name = I18NManager.Get("Common_Power");  //体力
                ImagePath = "Prop/particular/" + PropConst.PowerIconId;
                break;
            case ResourcePB.Gem:
                Name = I18NManager.Get("Common_Gem");// "钻石";
                ImagePath = "Prop/particular/" + PropConst.GemIconId;
                break;
            case ResourcePB.Gold:
                Name = I18NManager.Get("Common_Gold"); //"金币";
                ImagePath = "Prop/particular/" + PropConst.GoldIconId;
                break;
        }
    }

}
