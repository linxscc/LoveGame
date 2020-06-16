using System;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using Google.Protobuf;

public class VisitController : Controller
{
    public VisitView   VisitView;
    private VisitModel  _visitModel;
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            default:
                break;
        }
    }
    public override void Start()
    {
        _visitModel = GetData<VisitModel>();
        EventDispatcher.AddEventListener<PlayerPB>(EventConst.VisitSelectItemWeatherClick, OnVisitSelectItemWeatherClick);
        EventDispatcher.AddEventListener<PlayerPB>(EventConst.VisitSelectItemVisitClick, OnVisitSelectItemVisitClick);

        NetWorkManager.Instance.Send<VisitingRuleRes>(CMD.VISITINGC_RULE, null, OnVisitingRuleHandler, null, true,
            GlobalData.VersionData.VersionDic[CMD.VISITINGC_RULE]);
    }

    private void OnVisitingRuleHandler(VisitingRuleRes res)
    {
        _visitModel.InitRule(res);

        NetWorkManager.Instance.Send<MyVisitingRes>(CMD.VISITINGC_MYVISITINGS, null, OnMyVisitingHandler);
    }

    private void OnMyVisitingHandler(MyVisitingRes res)
    {
        _visitModel.InitMyData(res);
        //_visitModel.Init();
        VisitView.SetData(_visitModel.VisitVoList);
    }


    public void Refeash()
    {
        VisitView.SetData(_visitModel.VisitVoList);
    }

    private void OnVisitSelectItemWeatherClick(PlayerPB NpcId)
    {
        VISIT_WEATHER weather= _visitModel.GetWeatherByNpcId(NpcId);

        //if (weather == VISIT_WEATHER.Fine)
        //{
        //    FlowText.ShowMessage(I18NManager.Get("Visit_CurBestWeather"));
        //}
        //else
        //{
            SendMessage(new Message(MessageConst.MODULE_VISIT_SHOW_WEATHER_PANEL, Message.MessageReciverType.DEFAULT, NpcId));
       // }
    }

    private void OnVisitSelectItemVisitClick(PlayerPB NpcId)
    {
        VISIT_WEATHER weather = _visitModel.GetWeatherByNpcId(NpcId);
        SendMessage(new Message(MessageConst.MODULE_VISIT_SHOW_LEVEL_PANEL, Message.MessageReciverType.DEFAULT, NpcId));
        //if (weather == VISIT_WEATHER.Fine)
        //{
        //    SendMessage(new Message(MessageConst.MODULE_VISIT_SHOW_LEVEL_PANEL, Message.MessageReciverType.DEFAULT, NpcId));
        //}
        //else
        //{
        //    SendMessage(new Message(MessageConst.MODULE_VISIT_SHOW_WEATHER_PANEL, Message.MessageReciverType.DEFAULT, NpcId));
        //}
    }


    public override void Destroy()
    {
        EventDispatcher.RemoveEventListener<PlayerPB>(EventConst.VisitSelectItemWeatherClick, OnVisitSelectItemWeatherClick);
        EventDispatcher.RemoveEventListener<PlayerPB>(EventConst.VisitSelectItemVisitClick, OnVisitSelectItemVisitClick);

        base.Destroy();
    }

}
