using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Assets.Scripts.Framework.GalaSports.Core;
//服务器
using Assets.Scripts.Framework.GalaSports.Service;
//协议条
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using Common;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;

public class DrawController : Controller
{

    public DrawView DrawView;
    public ShowDrawCardView ShowCardView;
    //public DrawData _drawData;
    public int CardCount = 0;
    public string CardType = "";
    public DrawTypePB DrawType;
    public long GemRefreshTime;
    //star装赋值data和顶信息   每次赋值之后都可以初始化一次
    public override void Start()
    {
        base.Start();
    }

    public override void OnMessage(Message message)
    {
        string name = message.Name;
        switch (name)
        {

            case MessageConst.MODULE_DRAWCARD_SHOW_SHARE_CLICK:
        
                break;
            //服务器去扣费这里不用管
            //抽1次十次   需要发到主view中进行扣费  弹窗和付费窗口
            //生成动画的展示操作（view中的中间动画）                    
            case MessageConst.CMD_DRAWCARD_GET_CARD:
                //更新服务器的信息，然后显示卡牌
                DrawView.Hide();
                ShowCardView.Show();
                break;
            case MessageConst.CMD_DRAWCARD_GET_CARDLIST:
                //再这里发具体的消息
                //每次赋值后要更新list               
                //这里后来要发送到服务器的 接收换到另一个地方   这边不用管在 服务端有这个信息const就行了
                //进行获取和扣费
                SendMessage(new Message(MessageConst.CMD_DRAWCARD_GET_CARDLIST + CardType + CardCount));

                DrawView.Hide();
                ShowCardView.Show();
                //每张卡之间的动画                
                //在此处接到了list信息并显出来
                //showView.InitData(tempList);
                break;
            case MessageConst.CMD_DRAWCARD_DRAWCARD:
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
                req.DrawPoolType = DrawPoolTypePB.DrawPoolCommon;

                byte[] buffer = NetWorkManager.GetByteData(req);
                NetWorkManager.Instance.Send<DrawRes>(CMD.DRAWC_DRAW, buffer, res =>
                {

                    Debug.Log("CMD.DRAWC_DRAW---->" + res.ToString());
                    UserItemPB pb = res.UserItem;
                    if (res.UserDraw.DrawType==DrawTypePB.ByGem)
                    {
                        if (GlobalData.LevelModel.FindLevel("1-7").IsPass && GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide)<GuideConst.MainLineStep_OnClick_GlodDrawCard)                     
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
                    else if(res.UserDraw.DrawType == DrawTypePB.ByGold)
                    {
                        if(GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide)==GuideConst.MainLineStep_OnClick_GemDrawCard)
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
                       // GetData<DrawData>().GetRandomDialogById(3307);
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
                                    drawCardResultVo.Dialog = GetData<DrawData>().GetRandomDialogById(drawCardResultVo.CardId);
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
                    SetData(VList);
                }, str =>
                {
                    //todo 错误返回到抽卡主界面
                    Debug.Log("DrawCard error :" + str);
                    SendMessage(new Message(MessageConst.MODULE_VIEW_BACK_DRAWCARD));

                });
                break;

        }
    }

    public void SetData(List<DrawCardResultVo> resVo)
    {
        bool isShowLapiao = GuideManager.GetRemoteGuideStep(GuideTypePB.MainGuide) >= GuideConst.MainLineStep_OnClick_LoveStroy_1;


        bool _show = AppConfig.Instance.SwitchControl.Share;

        DrawView.SetShowCard(resVo, isShowLapiao && _show);
    }

    public override void Destroy()
    {
        base.Destroy();
        // EventDispatcher.RemoveEvent(EventConst.PhoneCallItemClick);
    }
    private long GetLeftDrawTime(DrawTypePB drawTypePB)
    {
        long curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
        switch (drawTypePB)
        {
            case DrawTypePB.ByGem:
                return GemRefreshTime - curTimeStamp;
            default:
                return long.MaxValue;
        }
    }

}
