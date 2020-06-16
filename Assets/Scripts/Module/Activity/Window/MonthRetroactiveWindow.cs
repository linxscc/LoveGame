using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using game.tools;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class MonthRetroactiveWindow : Window
{

    private Text _cost;
    private Text _num;

  

    private void Awake()
    {
        _cost = transform.GetText("Hint");
        _num = transform.GetText("Accumulative/Text");

        var cancel = transform.Find("Btn/CancelButton");
        PointerClickListener.Get(cancel.gameObject).onClick = go => { Close(); };
    }


    public void SetData(int cost,int num)
    {
        _cost.text = I18NManager.Get("Activity_Cost",cost);
        _num.text = I18NManager.Get("Activity_MonthAccumulativeSiginResidueNum", num);

        var ok = transform.Find("Btn/OkButton");
        PointerClickListener.Get(ok.gameObject).onClick = go =>
        {
            WindowEvent = WindowEvent.Yes;
            Close();
        };
    }

}
