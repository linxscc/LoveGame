using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;
using Framework.GalaSports.Service;
using game.main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : Controller
{

    public WeatherView WeatherView;
    private VisitVo _curVisitVo;
    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {
            case MessageConst.CMD_VISIT_WEATHER_JUMP_CLICK:
                Debug.Log("CMD_VISIT_WEATHER_JUMP_CLICK");
                if (_curVisitVo.CurWeather == VISIT_WEATHER.Fine)
                {
                    SendMessage(new Message(MessageConst.MODULE_VISIT_SHOW_LEVEL_PANEL, Message.MessageReciverType.DEFAULT, _curVisitVo.NpcId));
                }
                else
                {
                    PopupManager.ShowConfirmWindow(I18NManager.Get("Visit_Weather_Tips"), I18NManager.Get("Common_OK2")).WindowActionCallback = evt =>
                    {
                        if (evt == WindowEvent.Ok)
                        {
                            SendMessage(new Message(MessageConst.MODULE_VISIT_SHOW_LEVEL_PANEL, Message.MessageReciverType.DEFAULT, _curVisitVo.NpcId));
                        }
                    };
                }
                break;
            case MessageConst.CMD_VISIT_WEATHER_BLESSING_CLICK:
                Debug.Log("CMD_VISIT_WEATHER_BLESSING_CLICK");
                BlessResult result = (BlessResult)body[0];
                SendBlessingMsg(result);
                break;
            case MessageConst.CMD_VISIT_WEATHER_RESULT_CLICK:
                OnClickResultMask();
                break;

        }
    }

    public void SendBlessingMsg(BlessResult bless)
    {
        Debug.Log(_curVisitVo.NpcId + "  " + _curVisitVo.CurWeatherName);
        Debug.Log(bless);


        if (GlobalData.PlayerModel.PlayerVo.Gem < _curVisitVo.BlessCost)
        {
            FlowText.ShowMessage(I18NManager.Get("Shop_NotEnoughGem"));
            WeatherView.FailedBless();
            return;
        }


        if (_curVisitVo.CurWeather == VISIT_WEATHER.Fine)
        { return; }

        BlessReq req = new BlessReq
        {
            Player = _curVisitVo.NpcId,
            Level = (int)bless
        };
        var dataBytes = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<BlessRes>(CMD.VISITINGC_BLESS, dataBytes, OnVisitingBlessHandler, OnVisitingBlessHandlerError);
    }

    private void OnVisitingBlessHandlerError(HttpErrorVo obj)
    {
        Debug.LogError("OnVisitingBlessHandlerError");
        WeatherView.FailedBless();
    }

    private void OnVisitingBlessHandler(BlessRes res)
    {
        Debug.Log("OnVisitingBlessHandler");
        VISIT_WEATHER preWeatherId = _curVisitVo.CurWeather;
        Debug.LogError("OnVisitingBlessHandler   preWeatherId" + preWeatherId);
        if (res.UserMoney != null)
        {
            int blessBefore = GlobalData.PlayerModel.PlayerVo.Gem;
            GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
            int consumeBefore = blessBefore - GlobalData.PlayerModel.PlayerVo.Gem;
            //探班天气祈福 星钻消耗统计
            if (consumeBefore > 0)
            {
                SdkHelper.StatisticsAgent.OnPurchase("探班天气祈福", consumeBefore);
            }
        }
        if (res.UserWeather != null)
        {
            GetData<VisitModel>().UpdateMyWeather(res.UserWeather);
        }

        _curVisitVo.UpdateWeatherData(GetData<VisitModel>().GetWeatherRulesById(res.UserWeather.WeatherId));
        VISIT_WEATHER nowWeatherId = _curVisitVo.CurWeather;
        Debug.LogError("OnVisitingBlessHandler   nowWeatherId" + nowWeatherId);

        WeatherView.SetData(_curVisitVo,
            GetData<VisitModel>().WeatherRules,
            GetData<VisitModel>().WeatherBlessRules,
            true);
        //if (preWeatherId == nowWeatherId)//是否变为更好的天气
        //{
        //    FlowText.ShowMessage(I18NManager.Get("Visit_WeatherController_BlessFailed"));
        //    return;
        //}

    }

    private void OnClickResultMask()
    {
        //if (_curVisitVo.CurWeather == VISIT_WEATHER.Fine) //进入关卡界面
        //{
        //    SendMessage(new Message(MessageConst.MODULE_VISIT_SHOW_LEVEL_PANEL, Message.MessageReciverType.DEFAULT, _curVisitVo.NpcId));
        //    return;
        //}
        //返回水晶球界面

    }



    public override void Start()
    {

    }

    public override void Destroy()
    {
        base.Destroy();
    }


    public void SetData(PlayerPB npcId)
    {
        Debug.Log("WeatherController SetData NpcId is " + npcId);
        _curVisitVo = GetData<VisitModel>().GetVisitVo(npcId);

        WeatherView.SetData(_curVisitVo,
            GetData<VisitModel>().WeatherRules,
            GetData<VisitModel>().WeatherBlessRules
            );
    }
}
