using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Framework.GalaSports.Core;
using System;
using Common;

public class DrawController2 : Controller
{
    public DrawView2 DrawView;
    public override void Start()
    {
        base.Start();
    }

    public void SetData(List<DrawCardResultVo> list)
    {
        bool isShowLapiao = GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide) >= GuideConst.MainLineStep_OnClick_LoveStroy_1;
        bool _show = AppConfig.Instance.SwitchControl.Share;
        Debug.Log("DrawController2 SetData "+ isShowLapiao + " _show  "+ _show);
        DrawView.SetShowCard(list, isShowLapiao && _show);
    }
}
