using Com.Proto;
using DataModel;
using game.main;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardResultVo  {
    public int CardId;
    public string Name;
    public bool IsNew;
    public CreditPB Credit;
    public ResourcePB Resource;

    public string Dialog = "";//抽到ssr卡播放语音

    public string CardPath
    {
        get { return "Card/Image/SmallCard/" + CardId; }
    }
    public string FunPath
    {
        get { return "FansTexture/Head/" + CardId; }
    }

    public string GetShowMatPath()
    {
        string str = "DrawCard/Chouka/Materials/";

        if(Resource == ResourcePB.Fans)
        {
            str += "13-lanxing";
        }
        else if(Resource==ResourcePB.Card|| Resource == ResourcePB.Puzzle|| Resource == ResourcePB.Signature)
        {
            if(Credit==CreditPB.R)
            {
                str += "13-chengxing";
            }
            else if(Credit==CreditPB.Sr)
            {
                str += "13-fenxing";
            }
            else if(Credit==CreditPB.Ssr)
            {
                str += "13-zixing";           
            }
            else
            {
                str += "13-chengxing";
            }
        }
        else
        {
            str += "13-chengxing";
        }


        return str;
    }

    public DrawCardResultVo(AwardPB pb)
    {
        CardId = pb.ResourceId;
        Resource = pb.Resource;
        IsNew = false;
        if (Resource==ResourcePB.Fans)
        {
            FansRulePB funsRulePb=MyDepartmentData.GetFansRule(CardId);
            Name = funsRulePb.FansName;
        }
        else
        {
            CardPB cardPb = GlobalData.CardModel.GetCardBase(CardId);
     
            Name = cardPb.CardName;
            Name = CardVo.SpliceCardName(Name, cardPb.Player);
            Credit = cardPb.Credit;
        }
    }
}
