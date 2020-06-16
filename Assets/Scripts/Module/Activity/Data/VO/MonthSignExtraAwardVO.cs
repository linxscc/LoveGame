using Com.Proto;
using DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonthSignExtraAwardVO
{
    public int Num;
    public string IconPath;
    public string Name;
    private string preson;
    public int Id;
    
    public MonthSignExtraAwardVO(AwardPB pB)
    {

      
        Id = pB.ResourceId;
        Num = pB.Num;

        if (pB.Resource== ResourcePB.Card)
        {
            IconPath = "Card/Image/" +pB.ResourceId;

            SetName(pB.ResourceId);

            Name = preson+ " • " + GlobalData.CardModel.GetCardBase(pB.ResourceId).CardName; 
        }
        else if(pB.Resource== ResourcePB.Item)
        {
            Name = GlobalData.PropModel.GetPropBase(pB.ResourceId).Name + "x" + Num;
        }
     
      
    }
    
    private string SetName(int ResourceId)
    {
        var _id = ResourceId / 1000;
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
