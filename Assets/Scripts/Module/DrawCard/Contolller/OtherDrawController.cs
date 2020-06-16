using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using Common;
using DataModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherDrawController : Controller
{
    public DrawView2 View;
    public override void Init()
    {

    }
    List<DrawCardResultVo> _awardPbs;
    Action _finished;

    List<AwardPB> Awards;
    public void SetData()
    {
        List<DrawCardResultVo> _awardPbs = new List<DrawCardResultVo>();

        foreach (var v in Awards)
        {
            DrawCardResultVo drawCardResultVo = new DrawCardResultVo(v);
            _awardPbs.Add(drawCardResultVo);

            switch (v.Resource)
            {
                case ResourcePB.Card:
                    if (GlobalData.CardModel.GetUserCardById(v.ResourceId) == null)
                    {
                        drawCardResultVo.IsNew = true;
                    }
                    //如果是ssr卡,需要播放语音
                    if (drawCardResultVo.Credit == CreditPB.Ssr)
                    {
                        drawCardResultVo.Dialog = GetData<DrawData>().GetRandomDialogById(drawCardResultVo.CardId);
                    }
                    //GlobalData.CardModel.UpdateUserCardsByIdAndNum(v.ResourceId, v.Num);
                    break;
                case ResourcePB.Puzzle:
                    break;
                case ResourcePB.Fans:
                    //GlobalData.DepartmentData.UpdateFans(v.ResourceId, v.Num);
                    break;
            }
        }

        bool isShowLapiao = GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide) >= GuideConst.MainLineStep_OnClick_LoveStroy_1;
        bool _show = AppConfig.Instance.SwitchControl.Share;

        Debug.Log("OtherDrawController SetData " + isShowLapiao + " _show  " + _show);
        View.isOtherShow = true;
        View.SetShowCard(_awardPbs, _show&& isShowLapiao);

    }

    public void SetData(object[] paramsObjects)
    {
        Awards = (List<AwardPB>)paramsObjects[1];
   
        SetData();
       
    }
}
