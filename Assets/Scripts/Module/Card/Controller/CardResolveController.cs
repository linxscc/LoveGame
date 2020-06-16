using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using Google.Protobuf.Collections;

public class CardResolveController : Controller {
	
	public CardResolveView View;

    public CardResolveController()
    {
        EventDispatcher.AddEventListener<ResolveCardVo>(EventConst.CardResolveSelectedChange, OnSelectChang);
    }

    private void OnSelectChang(ResolveCardVo vo)
    {
        View.ChangeSelect();
    }

    /// <summary>
    /// 处理View消息
    /// </summary>
    /// <param name="message"></param>
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_CARD_RESOLVE:
                ResolveCards((RepeatedField<UserCardSimplePB>)message.Body);
                break;
            case MessageConst.CMD_CARD_REFRESH_USER_CARDS:
                View.SetData(GlobalData.CardModel.ResolveCardList);
                break;
        }
    }

    private void ResolveCards(RepeatedField<UserCardSimplePB> list)
    {
        ResolveReq req = new ResolveReq();
        req.UserCards.Add(list);
        byte[] buffer = NetWorkManager.GetByteData(req);
        
        NetWorkManager.Instance.Send<ResolveRes>(CMD.CARDC_RESOLVE,buffer, OnResolve);
    }

    private void OnResolve(ResolveRes res)
    {
        AudioManager.Instance.PlayEffect("cardResolve"); 
        FlowText.ShowMessage(I18NManager.Get("Card_ResolveSuccess"));
        GlobalData.PropModel.UpdateProps(res.UserItems.ToArray());

        GlobalData.CardModel.UpdateUserCards(res.UserCards.ToArray());
        
        View.SetData(GlobalData.CardModel.ResolveCardList,GlobalData.CardModel.CurPlayerPb);

        View.InitProps();
    }

    public override void Destroy()
    {
        base.Destroy();
        EventDispatcher.RemoveEventListener<ResolveCardVo>(EventConst.CardResolveSelectedChange, OnSelectChang);
    }
}
