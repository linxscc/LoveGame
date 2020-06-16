using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using DataModel;
using game.main;
using game.tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakePhotosGameAwardItem : MonoBehaviour {
    private RawImage _propImage;
    private Text _propNameTxt;
    private Text _ownTxt;

    public void SetData(AwardPB pb)
    {
        Debug.Log(pb.ResourceId + " " + pb.Num + " " + pb.Resource);
        //transform.Find("ItemImg").GetComponent<Image>();
        //transform.Find("ItemNumTxt").GetComponent<Text>().text="X " + awardPB.Num;

        string path = "";
        string name = "";
        if (pb.Resource == ResourcePB.Item)
        {
            name = GlobalData.PropModel.GetPropBase(pb.ResourceId).Name;
        }
        else
        {
            name = ViewUtil.ResourceToString(pb.Resource);
        }

        if (pb.Resource == ResourcePB.Gold)
        {
            //  vo.OwnedNum = (int)GlobalData.PlayerModel.PlayerVo.Gold;
            path = "Prop/particular/" + PropConst.GoldIconId;
        }
        else if (pb.Resource == ResourcePB.Gem)
        {
            path = "Prop/particular/" + PropConst.GemIconId;
        }
        else if (pb.Resource == ResourcePB.Power)
        {
            path = "Prop/particular/" + PropConst.PowerIconId;
        }
        else if (pb.Resource == ResourcePB.Card)
        {
            path = "Head/" + pb.ResourceId;

            CardPB cpb = GlobalData.CardModel.GetCardBase(pb.ResourceId);
            name = CardVo.SpliceCardName(cpb.CardName, cpb.Player);
        }
        else if (pb.Resource == ResourcePB.Memories)
        {
            path = "Prop/particular/123456789";
        }
        else
        {

            path = "Prop/" + pb.ResourceId;
        }
        _propImage = transform.Find("PropImage").GetComponent<RawImage>();
        _propNameTxt = transform.Find("PropNameTxt").GetComponent<Text>();
        _ownTxt = transform.Find("ObtainText").GetComponent<Text>();


        _propImage.texture = ResourceManager.Load<Texture>(path);
        _propNameTxt.text = name;
        _ownTxt.text = I18NManager.Get("GameMain_ActivityAwardItemObtainText", pb.Num);
        if (pb.Resource != ResourcePB.Card)
        {
            GameObject clickObj = transform.Find("ItemBg").gameObject;
            clickObj.transform.GetComponent<Image>().raycastTarget = true;
            PointerClickListener.Get(clickObj).onClick =

            //UIEventListener.Get(clickObj).onClick =
            go => { FlowText.ShowMessage(ClientData.GetItemDescById(pb.ResourceId, pb.Resource).ItemDesc); };
        }
        // _propImage.SetNativeSize();
    }
}
