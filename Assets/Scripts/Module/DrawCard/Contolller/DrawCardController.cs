using UnityEngine;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;
using System;
using System.Collections.Generic;
using game.main;
using Componets;
using Common;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Module.Framework.Utils;

public class DrawCardController : Controller
{
    public DrawCardView View;

    private long _gemRefreshTime;
    private long _goldRefreshTime;
    private int[] CostNum;
    private int[] _drawTimes;
   // private Dictionary<string, string> NotificationTimeDic;

    public override void Init()
    {         
        //NotificationTimeDic=new Dictionary<string, string>();
    }

    public override void Start()
    {
        CostNum = new int[4];
        _drawTimes = new int[2];
       // CostNum[(int)DrawTypePB.ByGem] = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.DRAW_GEM);
        CostNum[(int)DrawTypePB.ByGem] = 1;
        CostNum[(int)DrawTypePB.ByGem10] = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.DRAW_GEM_ITEM_10);
        CostNum[(int)DrawTypePB.ByGold] = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.DRAW_GOLD);
        CostNum[(int)DrawTypePB.ByGold10] = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.DRAW_GOLD_10);       
        Main.ChangeMenu(MainMenuDisplayState.ShowTopBar);
        LoadingOverlay.Instance.Show();
        NetWorkManager.Instance.Send<DrawProbRes>(CMD.DRAWC_DRAW_PROBS, null, OnGetCardList, null, true, GlobalData.VersionData.VersionDic[CMD.DRAWC_DRAW_PROBS]);

        NetWorkManager.Instance.Send<DrawInfoRes>(CMD.DRAWC_DRAWINFO, null, OnGetDrawInfo);

        //抽奖剩余计放在这
    }

    private void OnGetDrawInfo(DrawInfoRes obj)
    {
        if(View==null)
        { return; }
        UserDrawInfoPB infoPb = obj.UserDrawInfo;
        int total = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.DRAW_TOTAL_NUM);
       // View.SetLeftDrawTime(infoPb.GemDrawNum,0, infoPb.GoldDrawNum, total);
        _drawTimes[0] = infoPb.GemDrawNum;
        _drawTimes[1] = infoPb.GoldDrawNum;

        foreach (var v in obj.UserDraw)
        {
            //  Debug.Log(v.DrawType + "   " + v.RefreshTime);
            if (v.DrawPoolType == DrawPoolTypePB.DrawPoolCommon)
            {
                if (v.DrawType == DrawTypePB.ByGem)
                {
                    _gemRefreshTime = v.RefreshTime;
                    //               Debug.LogError(DateUtil.GetDataTime(_gemRefreshTime));
                    SdkHelper.PushAgent.PushFreeGemDraw(_gemRefreshTime);

                }
                else if (v.DrawType == DrawTypePB.ByGold)
                {
                    _goldRefreshTime = v.RefreshTime;
                    //              Debug.LogError(DateUtil.GetDataTime(_goldRefreshTime));
                    SdkHelper.PushAgent.PushFreeGoldDraw(_goldRefreshTime);

                }
            }
        }
       
        SdkHelper.PushAgent.PushNew();
        ClientTimer.Instance.AddCountDown("UpdateDarwCardTime", Int64.MaxValue, 1f, UpdateDarwCardTime, null);
        UpdateDarwCardTime(0);

    }

    private void OnGetCardList(DrawProbRes res)
    {
        LoadingOverlay.Instance.Hide();
        DrawData drawData = GetData<DrawData>();
        drawData.InitData(res);

        int id = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.DRAW_GEM_ITEM_ID);
     
        UserPropVo propVo = GlobalData.PropModel.GetUserProp(id);

        View.SetData(drawData.GetOwnNum(DrawPoolTypePB.DrawPoolCommon ,DrawEventPB.GemBase),
            drawData.GetTotalNum(DrawPoolTypePB.DrawPoolCommon, DrawEventPB.GemBase),
            drawData.GetOwnNum(DrawPoolTypePB.DrawPoolCommon, DrawEventPB.GoldBase),
            drawData.GetTotalNum(DrawPoolTypePB.DrawPoolCommon, DrawEventPB.GoldBase),
            drawData.GetOwnNum(DrawPoolTypePB.DrawPoolActivity, DrawEventPB.GemBase),
            drawData.GetTotalNum(DrawPoolTypePB.DrawPoolActivity, DrawEventPB.GemBase),
            CostNum,
            propVo.Num
            );
    }

    public void UpdateCardNum()
    {
        NetWorkManager.Instance.Send<DrawProbRes>(CMD.DRAWC_DRAW_PROBS, null, OnGetCardList);
        NetWorkManager.Instance.Send<DrawInfoRes>(CMD.DRAWC_DRAWINFO, null, OnGetDrawInfo);
        _isSendMessage = false;
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        switch (name)
        {
            case MessageConst.CMD_DRAWCARD_GEM_DRAW_TEN:
                if (!CheckIsDrawCard(DrawTypePB.ByGem10))
                {
                    FlowText.ShowMessage(I18NManager.Get("DrawCard_Hint1"));
                    return;
                }

                if (!CheckIsEnough(DrawTypePB.ByGem10))
                {

                    //FlowText.ShowMessage(I18NManager.Get("DrawCard_Hint2"));
                    View.BuyStarcard(null);
                    //SendMessage(new Message(MessageConst.CMD_DRAWCARD_BUYSTARCARD));
                    return;
                }
                DrawCard(false, true);
                break;
            case MessageConst.CMD_DRAWCARD_GEM_DRAW_ONCE:
                if (!CheckIsDrawCard(DrawTypePB.ByGem))
                {
                    FlowText.ShowMessage(I18NManager.Get("DrawCard_Hint1"));
                 
                    return;
                }
                if (!CheckIsEnough(DrawTypePB.ByGem)&& GetLeftDrawTime(DrawPoolTypePB.DrawPoolCommon, DrawTypePB.ByGem)>=1)
                {
                    //FlowText.ShowMessage(I18NManager.Get("DrawCard_Hint2"));
                    //SendMessage(new Message(MessageConst.CMD_DRAWCARD_BUYSTARCARD));
                    View.BuyStarcard(null);
                    return;
                }
                DrawCard(true, true);
                break;
            case MessageConst.CMD_DRAWCARD_GOLD_DRAW_ONCE:
                if (!CheckIsDrawCard(DrawTypePB.ByGold))
                {
                    FlowText.ShowMessage(I18NManager.Get("DrawCard_Hint1"));
                    return;
                }
                if (!CheckIsEnough(DrawTypePB.ByGold)&& GetLeftDrawTime(DrawPoolTypePB.DrawPoolCommon, DrawTypePB.ByGold) >= 1)
                {
                    FlowText.ShowMessage(I18NManager.Get("DrawCard_Hint3"));
                    return;
                }
                DrawCard(true, false);
                break;
            case MessageConst.CMD_DRAWCARD_GOLD_DRAW_TEN:
                if (!CheckIsDrawCard(DrawTypePB.ByGold10))
                {
                    FlowText.ShowMessage(I18NManager.Get("DrawCard_Hint1"));
                    return;
                }
                if (!CheckIsEnough(DrawTypePB.ByGold10))
                {
                    FlowText.ShowMessage(I18NManager.Get("DrawCard_Hint3"));
                    return;
                }
                DrawCard(false, false);
                break;
            case MessageConst.CMD_DRAWCARD_GOTO_GOLD_DRAW:
                Debug.LogError("CMD_DRAWCARD_GOTO_GOLD_DRAW");
                //View.ScrollingDisplay(false);
                View.SetTabBtnPage(DrawCardView.GOLD_UI);
                break;
            case MessageConst.CMD_DRAWCARD_ACTIVITY_DRAW_ONCE:
                DrawCardActivity(true);
                break;
            case MessageConst.CMD_DRAWCARD_ACTIVITY_DRAW_TEN:
                DrawCardActivity(false);
                break;
        }
    }
 
   
    


    private bool CheckIsDrawCard(DrawTypePB drawTypePB)
    {
        int total = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.DRAW_TOTAL_NUM);
        int costNum = total;
        switch (drawTypePB)
        {
            case DrawTypePB.ByGem:
                costNum = _drawTimes[0];
                return costNum + 1 <= total ? true : false;
            case DrawTypePB.ByGem10:
                costNum = _drawTimes[0];
                return costNum + 10 <= total ? true : false;
            case DrawTypePB.ByGold:
                costNum = _drawTimes[1];
                return costNum + 1 <= total ? true : false;
            case DrawTypePB.ByGold10:
                costNum = _drawTimes[1];
                return costNum + 10 <= total ? true : false;
            default:
                return false;
        }
    }

    private bool CheckIsEnough(DrawTypePB drawTypePB)
    {

        int costNum = CostNum[(int)drawTypePB];
        switch (drawTypePB)
        {
            case DrawTypePB.ByGem:
                // return GlobalData.PlayerModel.PlayerVo.Gem >= costNum ? true : false;
                int id1 = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.DRAW_GEM_ITEM_ID);
                return GlobalData.PropModel.GetUserProp(id1).Num >= costNum ? true : false;
            case DrawTypePB.ByGem10:
                //return GlobalData.PlayerModel.PlayerVo.Gem >= costNum ? true : false;
                int id10 = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.DRAW_GEM_ITEM_ID);
                return GlobalData.PropModel.GetUserProp(id10).Num >= costNum ? true : false;
            case DrawTypePB.ByGold:
                return GlobalData.PlayerModel.PlayerVo.Gold >= costNum ? true : false;
            case DrawTypePB.ByGold10:
                return GlobalData.PlayerModel.PlayerVo.Gold >= costNum ? true : false;
            default:
                return false;
        }
    }
    private void DrawCard(bool isOnce, bool isGem)
    {
       // View.Hide();
        DrawTypePB drawType;
        if (isGem)
        {
            drawType = isOnce ? DrawTypePB.ByGem : DrawTypePB.ByGem10;
        }
        else
        {
            drawType = isOnce ? DrawTypePB.ByGold : DrawTypePB.ByGold10;
        }
        //新版动画不需要进入下一个界面
        //SendMessage(new Message(MessageConst.MODULE_CARD_SHOW_DRAW_VIEW, Message.MessageReciverType.DEFAULT, drawType,_gemRefreshTime));
        if(_isSendMessage)
        {
            return;
        }

        _isSendMessage = true;
        DrawCardNetHelp.SendDraw(drawType, DrawPoolTypePB.DrawPoolCommon,  GetData<DrawData>(), SuccessCallback, FailureCallback);

    }

    private void DrawCardActivity(bool isOnce)
    {
        DrawTypePB drawType = isOnce ? DrawTypePB.ByGem : DrawTypePB.ByGem10;
        if (_isSendMessage)
        {
            return;
        }

        _isSendMessage = true;
        DrawCardNetHelp.SendDraw(drawType, DrawPoolTypePB.DrawPoolActivity, GetData<DrawData>(), SuccessCallback, FailureCallback);

    }


    bool _isSendMessage = false;

    private void FailureCallback()
    {
        Debug.Log("FailureCallback");
        _isSendMessage = false;
    }

    private void SuccessCallback(List<DrawCardResultVo> lsit)
    {
        Debug.Log("SuccessCallback");
        SendMessage(new Message(MessageConst.MODULE_CARD_SHOW_DRAW_VIEW, Message.MessageReciverType.DEFAULT, lsit)); 

        
    }

    private void UpdateDarwCardTime(int obj)
    {
        string gemStr="";
        long lefTime = GetLeftDrawTime(DrawPoolTypePB.DrawPoolCommon, DrawTypePB.ByGem);
       
        if (lefTime < 1000)
        {
            gemStr = "0";
        }
        else
        {
            //DateTime gemDt = new DateTime(lefTime * 10000);
            //Debug.LogError(gemDt.Hour);        
            //gemStr = string.Format("{00:F}", gemDt);
            long s = (lefTime/1000) % 60;
            long m = (lefTime / (60 * 1000)) % 60;
            long h = lefTime / (60 * 60*1000);
            gemStr = string.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);
        }

        string goldStr = "";
        lefTime = GetLeftDrawTime(DrawPoolTypePB.DrawPoolCommon, DrawTypePB.ByGold);
        if (lefTime < 1000) 
        {
            goldStr = "0";
        }
        else
        {
            //DateTime goldDt = new DateTime(lefTime * 10000);
            //Debug.LogError(goldDt.Day);
            //goldStr = string.Format("{0:F}", goldDt);
            long s = (lefTime / 1000) % 60;
            long m = (lefTime / (60 * 1000)) % 60;
            long h = lefTime / (60 * 60 * 1000);
            goldStr = string.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);
        }

        string activityStr = "";
        lefTime = GetLeftDrawTime(DrawPoolTypePB.DrawPoolActivity, DrawTypePB.ByGem);

        if (lefTime < 1000)
        {
            gemStr = "0";
        }
        else
        {
            long s = (lefTime / 1000) % 60;
            long m = (lefTime / (60 * 1000)) % 60;
            long h = lefTime / (60 * 60 * 1000);
            activityStr = string.Format("{0:D2}:{1:D2}:{2:D2}", h, m, s);
        }

        if (View==null)
        {
            return;
        }

        View.SetRemainTime(gemStr, goldStr, CostNum);
    }

    private long GetLeftDrawTime(DrawPoolTypePB poolType, DrawTypePB drawTypePB)
    {
        if (GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide) < GuideConst.MainLineStep_OnClick_GemDrawCard)
        {
            //GuideManager.SetRemoteGuideStep(GuideTypePB.MainGuide, GuideConst.MainStep_PhoneSms_End);
            return 0;
        }


        long curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        if (poolType == DrawPoolTypePB.DrawPoolCommon)
        {
            switch (drawTypePB)
            {
                case DrawTypePB.ByGem:

                    return _gemRefreshTime - curTimeStamp;
                case DrawTypePB.ByGold:
                    return _goldRefreshTime - curTimeStamp;
                default:
                    return long.MaxValue;
            }
        }
        else
        {
            return long.MaxValue;
        }
    }

    public override void Destroy()
    {
        ClientTimer.Instance.RemoveCountDown("UpdateDarwCardTime");
        View = null;
        base.Destroy();
    }
}