using System;
using Assets.Scripts.Framework.GalaSports.Core;

public class CumulativeRechargeModule : ModuleBase
{
    CumulativeRechargePanel _cumulativerechargepanel;

    public override void Init()
    {
        _cumulativerechargepanel = new CumulativeRechargePanel();
        _cumulativerechargepanel.Init(this);
        _cumulativerechargepanel.Show(0);
    }

    public override void OnShow(float delay)
    {
        base.OnShow(delay);
        _cumulativerechargepanel.Show(0);
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        object[] body = message.Params;
        switch (name)
        {

        }
    }
}
