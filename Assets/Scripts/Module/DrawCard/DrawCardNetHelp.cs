using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardNetHelp  {
    public static void SendDraw(DrawTypePB DrawType, DrawPoolTypePB poolType, DrawData drawData, Action<List<DrawCardResultVo>> success, Action failure)
    {
        DrawReq req = new DrawReq();
        req.DrawType = DrawType;
        //if (DrawType == DrawTypePB.ByGem)
        //{
        //    //req.UseItem = GetLeftDrawTime(DrawType) > 0 ? true : false;
        //    req.UseItem = true;
        //}
        //else if (DrawType == DrawTypePB.ByGem10)
        //{
        //    req.UseItem = true;
        //}
        //else
        //{
        //    req.UseItem = false;
        //}
        req.DrawPoolType = poolType;

        byte[] buffer = NetWorkManager.GetByteData(req);
        NetWorkManager.Instance.Send<DrawRes>(CMD.DRAWC_DRAW, buffer, res =>
        {
            Debug.Log("CMD.DRAWC_DRAW---->" + res.ToString());
            UserItemPB pb = res.UserItem;

            if (res.UserDraw.DrawPoolType == DrawPoolTypePB.DrawPoolCommon)
            {
                if (res.UserDraw.DrawType == DrawTypePB.ByGem)
                {
                    if (GlobalData.LevelModel.FindLevel("1-7").IsPass && GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide) < GuideConst.MainLineStep_OnClick_GemDrawCard)
                    //if (GuideManager.IsMainGuidePass(GuideConst.MainStep_MainStory1_7_End))
                    {
                        //防止网络异常先模拟数据
                        UserGuidePB userGuide = new UserGuidePB()
                        {
                            GuideId = GuideConst.MainLineStep_OnClick_GemDrawCard,
                            GuideType = GuideTypePB.MainGuide
                        };
                        GuideManager.UpdateRemoteGuide(userGuide);
                    }
                    GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_OnClick_GemDrawCard);

                }
                else if (res.UserDraw.DrawType == DrawTypePB.ByGold)
                {
                    if (GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide) == GuideConst.MainLineStep_OnClick_GemDrawCard)
                    //                      //  if (GuideManager.IsMainGuidePass(GuideConst.MainStep_DrawCard_GetCard))
                    {
                        //防止网络异常先模拟数据
                        UserGuidePB userGuide = new UserGuidePB()
                        {
                            GuideId = GuideConst.MainLineStep_OnClick_GlodDrawCard,
                            GuideType = GuideTypePB.MainGuide
                        };
                        GuideManager.UpdateRemoteGuide(userGuide);
                    }
                    GuideManager.SetStatisticsRemoteGuideStep(GuideConst.MainLineStep_OnClick_GlodDrawCard);
                }
            }
            switch (res.UserDraw.DrawType)
            {
                //统计
                case DrawTypePB.ByGold:
                    SdkHelper.StatisticsAgent.OnEvent("抽卡金币消耗", GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.DRAW_GOLD));
                    break;
                case DrawTypePB.ByGold10:
                    SdkHelper.StatisticsAgent.OnEvent("抽卡金币消耗", GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.DRAW_GOLD_10));
                    break;
            }
            if (res.UserItem != null)
            {
                GlobalData.PropModel.UpdateProps(new[] { res.UserItem });
            }
            if (res.UserMoney != null)
            {
                GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
            }
            GlobalData.RandomEventModel.AddDrawCardTimes(res.UserDraw.DrawType);
            List<DrawCardResultVo> VList = new List<DrawCardResultVo>();
            foreach (var v in res.Awards)
            {
                DrawCardResultVo drawCardResultVo = new DrawCardResultVo(v);
                VList.Add(drawCardResultVo);
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
                            GlobalData.RandomEventModel.SsrGet = true;
                            drawCardResultVo.Dialog = drawData.GetRandomDialogById(drawCardResultVo.CardId);
                        }
                        GlobalData.CardModel.UpdateUserCardsByIdAndNum(v.ResourceId, v.Num);
                        break;
                    case ResourcePB.Puzzle:
                        break;
                    case ResourcePB.Fans:
                        GlobalData.DepartmentData.UpdateFans(v.ResourceId, v.Num);
                        break;
                }
            }
            success?.Invoke(VList);
        }, str =>{
            //todo 错误返回到抽卡主界面
            Debug.Log("DrawCard error :" + str);
            failure?.Invoke();
        });
    }
    
}
