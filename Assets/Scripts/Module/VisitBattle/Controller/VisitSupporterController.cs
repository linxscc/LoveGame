using Assets.Scripts.Framework.GalaSports.Core;
using Module.VisitBattle.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using game.main;
using DataModel;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Common;

public class VisitSupporterController : Controller
{

    public VisitSupporterView view;
    private VisitBattleModel _model;

    public override void Start()
    {
        _model = GetData<VisitBattleModel>();
        EventDispatcher.AddEventListener(EventConst.UpdateSupporterNum, UpdatePowerData);
        view.SetFansData(_model.LevelVo, GlobalData.DepartmentData.Fanss);
        view.SetGoodsData(_model.LevelVo);
        UpdatePowerData();
    }
    void UpdatePowerData()
    {
        view.UpdatePowerData();
    }


    public override void Destroy()
    {
        base.Destroy();
        EventDispatcher.RemoveEvent(EventConst.UpdateSupporterNum);
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

        }
    }
}
