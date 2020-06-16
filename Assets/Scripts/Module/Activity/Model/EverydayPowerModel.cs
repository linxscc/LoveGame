using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class EverydayPowerModel : Model
{

    private List<EveryDayPowerVO> _everyDayPower;       //每日体力集合    

    public EverydayPowerModel()
    {
        Init();
    }

    public void Init()
    {       
        InitEveryDayPowerList();
    }

    private void InitEveryDayPowerList()
    {
        var listRule = GlobalData.ActivityModel.BaseActivityRule.ActivityPowerGetRules;
        _everyDayPower = new List<EveryDayPowerVO>();   
        foreach (var t in listRule)
        {
            var item = new EveryDayPowerVO(t);
            _everyDayPower.Add(item);
        }
      
    }


    public List<EveryDayPowerVO> GetEveryDayPowerList()
    {
        return _everyDayPower;
    }

    public void UpdataEveryDayPowerList(int id)
    {
        for (int i = 0; i < _everyDayPower.Count; i++)
        {
            if (id== _everyDayPower[i].Id)
            {
                _everyDayPower[i].State = EveryDaySignState.AlreadyGet;
                break;
            }
        }
    }





}
