using Assets.Scripts.Framework.GalaSports.Service;
using Com.Proto;
using DataModel;
using game.main;
using game.tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AirborneGameAwardItem : MonoBehaviour {
    private RawImage _propImage;
    private Text _propNameTxt;
    private Text _ownTxt;

    public void SetData(AirborneGameRunningItemVo vo)
    {
        Debug.Log(vo.ResourceId + " " + vo.Count + " " + vo.Resource);
        //transform.Find("ItemImg").GetComponent<Image>();
        //transform.Find("ItemNumTxt").GetComponent<Text>().text="X " + awardPB.Num;

        string path = "";
        string name = "";
        if (vo.Resource == ResourcePB.Item)
        {
            name = GlobalData.PropModel.GetPropBase(vo.ResourceId).Name;
        }
        else
        {
            name = ViewUtil.ResourceToString(vo.Resource);
        }

        if (vo.Resource == ResourcePB.Gold)
        {
            //  vo.OwnedNum = (int)GlobalData.PlayerModel.PlayerVo.Gold;
            path = "Prop/particular/" + PropConst.GoldIconId;
        }
        else if (vo.Resource == ResourcePB.Gem)
        {
            path = "Prop/particular/" + PropConst.GemIconId;
        }
        else if (vo.Resource == ResourcePB.Power)
        {
            path = "Prop/particular/" + PropConst.PowerIconId;
        }
        else if (vo.Resource == ResourcePB.Card)
        {
            path = "Head/" + vo.ResourceId;

            CardPB pb = GlobalData.CardModel.GetCardBase(vo.ResourceId);
            name = CardVo.SpliceCardName(pb.CardName, pb.Player);
        }
        else if (vo.Resource == ResourcePB.Memories)
        {
            path = "Prop/particular/123456789";
        }
        else
        {

            path = "Prop/" + vo.ResourceId;
        }
        _propImage = transform.Find("PropImage").GetComponent<RawImage>();
        _propNameTxt = transform.Find("PropNameTxt").GetComponent<Text>();
        _ownTxt = transform.Find("ObtainText").GetComponent<Text>();



        _propImage.texture = ResourceManager.Load<Texture>(path);
        _propNameTxt.text = name;
        _ownTxt.text = I18NManager.Get("GameMain_ActivityAwardItemObtainText", vo.Count);
        // _propImage.SetNativeSize();


        if (vo.Resource != ResourcePB.Card)
        {
            GameObject clickObj = transform.Find("ItemBg").gameObject;
            clickObj.transform.GetComponent<Image>().raycastTarget = true;
            PointerClickListener.Get(clickObj).onClick = go => { FlowText.ShowMessage(ClientData.GetItemDescById(vo.ResourceId, vo.Resource).ItemDesc); };
        }
    }
}
