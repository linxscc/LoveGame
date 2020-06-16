using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Com.Proto;
using DataModel;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;
public class ActivityAwardItem : MonoBehaviour
{
    private RawImage _propImage;
    private Text _propNameTxt;
    private Text _ownTxt;

    public void SetData(AwardPB awardPB)
    {

        Debug.Log(awardPB.ResourceId + " " + awardPB.Num + " " + awardPB.Resource);
        //transform.Find("ItemImg").GetComponent<Image>();
        //transform.Find("ItemNumTxt").GetComponent<Text>().text="X " + awardPB.Num;

        string path = "";
        string name = "";
        if (awardPB.Resource == ResourcePB.Item)
        {
            name = GlobalData.PropModel.GetPropBase(awardPB.ResourceId).Name;
        }
        else
        {
            name = ViewUtil.ResourceToString(awardPB.Resource);
        }

        if (awardPB.Resource == ResourcePB.Gold)
        {
            //  vo.OwnedNum = (int)GlobalData.PlayerModel.PlayerVo.Gold;
            path = "Prop/particular/" + PropConst.GoldIconId;
        }
        else if (awardPB.Resource == ResourcePB.Gem)
        {
            path = "Prop/particular/" + PropConst.GemIconId;
        }
        else if (awardPB.Resource == ResourcePB.Power)
        {
            path = "Prop/particular/" + PropConst.PowerIconId;
        }
        else if (awardPB.Resource == ResourcePB.Card)
        {
            path = "Head/" + awardPB.ResourceId;

            CardPB pb = GlobalData.CardModel.GetCardBase(awardPB.ResourceId);
            name = CardVo.SpliceCardName(pb.CardName, pb.Player);
        }
        else if (awardPB.Resource == ResourcePB.Memories)
        {
            path = "Prop/particular/123456789";
        }
        else
        {
            //var prop = GlobalData.PropModel.GetUserProp(award.ResourceId);
            //vo.OwnedNum = prop != null ? prop.Num : 0;
            //vo.PropId = award.ResourceId;
            path = "Prop/" + awardPB.ResourceId;
        }
        _propImage = transform.Find("PropImage").GetComponent<RawImage>();
        _propNameTxt = transform.Find("PropNameTxt").GetComponent<Text>();
        _ownTxt = transform.Find("ObtainText").GetComponent<Text>();


        //transform.Find("LeftHeadIcon/Image").GetComponent<RawImage>().texture = ResourceManager.Load<Texture>(headPath, ModuleConfig.MODULE_PHONE);
        //_propImage.sprite = ResourceManager.Load<Sprite>(path);

        _propImage.texture =ResourceManager.Load<Texture>(path);


        _propNameTxt.text = name;
      //  _ownTxt.text = "数量：" + awardPB.Num;
        _ownTxt.text = I18NManager.Get("GameMain_ActivityAwardItemObtainText", awardPB.Num);
        // _propImage.SetNativeSize();
    }
}
