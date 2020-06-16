using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;
using Utils;

public class ActivityCapsuleTemplateDrawController : Controller
{

    public ActivityCapsuleTemplateDrawView View;

    public override void Init()
    {
        EventDispatcher.AddEventListener<int>(EventConst.ActivityCapsuleTemplateDrawAnimOver, OnDrawAnimOver);
        EventDispatcher.AddEventListener(EventConst.ActivityCapsuleTemplateRefreshUserInfo, Refresh);
        EventDispatcher.AddEventListener<System.Action<int>>(EventConst.ActivityCapsuleTemplateDraw, SendDrawReq);
    }

    public override void Start()
    {
        View.SetData(GetData<ActivityCapsuleTemplateModel>());
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_ACTIVITY_LOTTERY_TEMPLATE1_ON_LOTTERY_BTN:

                break;
        }
    }

    public void Refresh()
    {
        View.SetData(GetData<ActivityCapsuleTemplateModel>());
    }

    private void OnDrawAnimOver(int id)
    {
        ActivityCapsuleItemPB item = GetData<ActivityCapsuleTemplateModel>().GetCapsuleItem(id);
        RewardVo reward = new RewardVo(item.AwardPB, true);
        if (reward.Resource == ResourcePB.Card)
        {
            SendMessage(new Message(MessageConst.MODULE_ACTIVITY_CAPSULE_TEMPLATE_Hide_BACKBTN, Message.MessageReciverType.UnvarnishedTransmission, false));
            Action finish = () =>
            {
                SendMessage(new Message(MessageConst.MODULE_ACTIVITY_CAPSULE_TEMPLATE_Hide_BACKBTN, Message.MessageReciverType.UnvarnishedTransmission, true));
            };
            ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_DRAWCARD, false, false, "DrawCard_CardShow", new List<AwardPB>() { item.AwardPB }, finish);
        }
        else
        {
            var window = PopupManager.ShowWindow<CommonAwardWindow>("GameMain/Prefabs/AwardWindow/CommonAwardWindow");
            window.SetData(new List<AwardPB>() { item.AwardPB }, false, ModuleConfig.MODULE_ACTIVITYCAPSULETEMPLATE);
        }
        SendUserInfoReq();
    }

    //用户信息
    private void SendUserInfoReq()
    {
        GetUserActivityCapsuleReq req = new GetUserActivityCapsuleReq
        {
            ActivityId = GetData<ActivityCapsuleTemplateModel>().CurActivityId
        };
        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<GetUserActivityCapsuleRes>(CMD.ACTIVITYC_CAPSULE_INFO, data, GetUserInfoRep);
    }

    private void GetUserInfoRep(GetUserActivityCapsuleRes res)
    {
        if (res.UserActivityCapsuleInfoPB != null)
        {
            GetData<ActivityCapsuleTemplateModel>().SetReadStoryIds(res.UserActivityCapsuleInfoPB.PlotIds.ToList<string>());
            GetData<ActivityCapsuleTemplateModel>().SetGainCapsuleItemIds(res.UserActivityCapsuleInfoPB.DrawAwardProgress.ToList<int>());
        }
        else
        {
            GetData<ActivityCapsuleTemplateModel>().SetReadStoryIds(null);
            GetData<ActivityCapsuleTemplateModel>().SetGainCapsuleItemIds(null);
        }
        Refresh();
    }


    //扭蛋
    private void SendDrawReq(System.Action<int> finishCallback = null)
    {
        ActivityCapsuleTemplateModel model = GetData<ActivityCapsuleTemplateModel>();
        if (model.costItem != null)
        {
            int num = PropUtils.GetUserPropNum(model.costItem.ResourceId);
            if(num < model.costItem.Num)
            {
                PopupManager.ShowAlertWindow(I18NManager.Get("ActivityCapsuleTemplate_drawNoEnoughItem"));
                return;
            }
        }

        DrawAwardReq req = new DrawAwardReq
        {
            ActivityId = model.CurActivityId
        };
        byte[] data = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<DrawAwardRes>(CMD.ACTIVITYC_CAPSULE_DRAW_AWARD, data, (res) =>
        {
            GlobalData.PropModel.UpdateProps(new[] { res.UserItem });
            View.UpdateUserProp(GetData<ActivityCapsuleTemplateModel>());
            if (finishCallback != null)
                finishCallback(res.AwardId);
        });
    }

    public override void Destroy()
    {
        EventDispatcher.RemoveEvent(EventConst.ActivityCapsuleTemplateDrawAnimOver);
        EventDispatcher.RemoveEvent(EventConst.ActivityCapsuleTemplateRefreshUserInfo);
        EventDispatcher.RemoveEvent(EventConst.ActivityCapsuleTemplateDraw);
    }
}
