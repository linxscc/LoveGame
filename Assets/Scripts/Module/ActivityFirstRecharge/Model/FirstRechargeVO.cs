using Com.Proto;
using DataModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstRechargeVO
{

    public RewardVo RewardVo;
    public string BigCradImage;   
    public string SmallCradImage;

    public string ShowCardImage;
    public string Signature;
    public string CardName;
    public int Group;
    private int NpcId;
    public FirstRechargeVO(AwardPB pB)
    {
        RewardVo = new RewardVo(pB);
        if (pB.Resource == ResourcePB.Card)
        {
            CardName = SetName(pB.ResourceId) + "•" + GlobalData.CardModel.GetCardBase(pB.ResourceId).CardName;
             BigCradImage   = "Card/Image/"+pB.ResourceId;
            SmallCradImage = "Head/"+pB.ResourceId;
            ShowCardImage = "Activity/FirstRechargeBG_"+NpcId;
            Signature = "UIAtlas_Activity_"+pB.ResourceId;
        }
        

    }

    private string SetName(int ResourceId)
    {
        string preson = "";
        var _id = ResourceId / 1000;
        switch (_id)
        {
            case 1:
                preson = I18NManager.Get("Common_Role1");
                NpcId = 1;
                break;
            case 2:
                preson = I18NManager.Get("Common_Role2");
                NpcId = 2;
                break;
            case 3:
                preson = I18NManager.Get("Common_Role3");
                NpcId = 3;
                break;
            case 4:
                preson = I18NManager.Get("Common_Role4");
                NpcId = 4;
                break;
        }

        return preson;
    }




}
