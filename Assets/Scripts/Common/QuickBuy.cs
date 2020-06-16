using System;
using System.Diagnostics;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.NetWork;
using Com.Proto;
using DataModel;
using game.main;
using Debug = UnityEngine.Debug;

namespace Common
{
    public class QuickBuy
    {
        #region
        //public static void BuyGold(string content = "是否花费10星钻购买1000金币？", Action<WindowEvent> callback = null,
        //    string title = "购买金币", string okBtnText = "确定", string cancelBtnText = "取消")
        //{
        //    PopupManager.ShowConfirmWindow(content, title, okBtnText, cancelBtnText).WindowActionCallback = evt =>
        //    {
        //        if (evt != WindowEvent.Ok)
        //        {
        //            callback?.Invoke(evt);
        //            return;
        //        }

        //        GemExchangeReq req = new GemExchangeReq
        //        {

        //            BuyType = BuyGemTypePB.BuyGold
        //        };
        //        byte[] data = NetWorkManager.GetByteData(req);
        //        NetWorkManager.Instance.Send<GemExchangeRes>(CMD.USERC_GEMEXCHANGE, data, res =>
        //        {
        //            GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
        //            FlowText.ShowMessage("购买成功");
        //            callback?.Invoke(evt);
        //        });
        //    };
        //}
        #endregion


        /// <summary>
        /// 购买金币或体力或星源体力
        /// </summary>
        /// <param name="buyItemId">要购买道具的Id</param>
        public static void BuyGlodOrPorwer(int buyItemId,int costItemId)
        {         
            PopupManager.ShowBuyWindow(buyItemId, costItemId).WindowActionCallback = evt =>
            {
                if (evt != WindowEvent.Ok) return;

                GemExchangeReq req = new GemExchangeReq();
                switch (buyItemId)
                {
                    case PropConst.GoldIconId:
                        req.BuyType = BuyGemTypePB.BuyGold;
                        break;
                    case PropConst.PowerIconId:
                        req.BuyType = BuyGemTypePB.BuyPower;
                        break;
                    case PropConst.EncouragePowerId:
                        req.BuyType = BuyGemTypePB.BuyEncouragePower;                        
                        break;
                }      

                byte[] data = NetWorkManager.GetByteData(req);
                NetWorkManager.Instance.Send<GemExchangeRes>(CMD.USERC_GEMEXCHANGE, data, res =>
                {                              
                   // 统计消耗钻石的数量(要在更新购买次数前获取购买消耗钻石量)
                    switch (buyItemId)
                    {
                        case PropConst.GoldIconId:
                            SdkHelper.StatisticsAgent.OnPurchase("货币栏金币购买",
                                GlobalData.PlayerModel.GetBuyGemRule(BuyGemTypePB.BuyGold, GlobalData.PlayerModel.PlayerVo.GoldNum).Gem);
                          
                            break;
                        case PropConst.PowerIconId:
                            SdkHelper.StatisticsAgent.OnPurchase("货币栏体力购买",
                                GlobalData.PlayerModel.GetBuyGemRule(BuyGemTypePB.BuyPower, GlobalData.PlayerModel.PlayerVo.PowerNum).Gem); 
                            break;
                        case PropConst.EncouragePowerId:
                            SdkHelper.StatisticsAgent.OnPurchase("货币栏探班行动力购买",
                                GlobalData.PlayerModel.GetBuyGemRule(BuyGemTypePB.BuyEncouragePower, GlobalData.PlayerModel.PlayerVo.EncourageNum).Gem);                            
                            break;
                    }
                    
                                        
                    GlobalData.PlayerModel.UpDataBuyNum(res.UserBuyGemInfo);
                    GlobalData.PlayerModel.UpdateUserMoney(res.UserMoney);
                    GlobalData.PlayerModel.UpdateUserPower(res.UserPower);
                   
                    //
                    EventDispatcher.TriggerEvent(EventConst.UpdateEnergy);
                    EventDispatcher.TriggerEvent(EventConst.BuyGoldSuccess);
                    EventDispatcher.TriggerEvent(EventConst.RefreshPoint);
                    FlowText.ShowMessage(I18NManager.Get("Common_BuySucceed"));// ("购买成功");
                    AudioManager.Instance.PlayEffect(buyItemId==PropConst.GoldIconId?"buyGold":"buypower"); 
                });
            };
        }



        
         

    }
}