﻿using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using game.main;
using UnityEngine;
using DataModel;
using Google.Protobuf.Collections;


public class BuyGemModel : Model
    {

        public RepeatedField<MallRefreshGoldRulePB> MallRefreshGoldRulePbs;
        public UserBuyMallInfoPB UserBuyMallInfoPb;

        public Dictionary<int, GameMallVo> GameMallDic;
        public Dictionary<int, RmbMallVo> RmbMallDic;

        public List<UserBuyGameMallVo> UserBuyGameMallList;
        public List<UserBuyRmbMallVo> UserBuyRmbMallList;

        public string[] MallPageDesc =  {"充值问题，客服电话，刷新机制等描述","充值问题，客服电话，刷新机制等描述","充值问题，客服电话，刷新机制等描述","" };
        public string BuyGemDesc = I18NManager.Get("Shop_BuyGemDesc");

        public bool Refresh = false;
        
        public enum PageIndex
        {
            GiftPage=1,
            VipPage=2,
            GemPage=3,
            GoldPage=4,
        }
        
        
        public void InitRule(MallRuleRes res)
        {
            MallRefreshGoldRulePbs = res.MallRefreshGoldRules;

            if (GameMallDic==null)
            {
                GameMallDic=new Dictionary<int, GameMallVo>();
            }
            
            foreach (var pb in res.GameMallRules)
            {
//                Debug.LogError(pb);
                var vo=new GameMallVo(pb);
                if (GameMallDic.ContainsKey(vo.MallId))
                {
                    GameMallDic[vo.MallId] = vo;
                }
                else
                {
                    GameMallDic.Add(vo.MallId,vo);  
                }
                
               
            }


            if (RmbMallDic==null)
            {
                RmbMallDic=new Dictionary<int, RmbMallVo>();
            }

            foreach (var pb in res.RmbMallRules)
            {
//                Debug.LogError(pb);
                var vo=new RmbMallVo(pb);
                if (RmbMallDic.ContainsKey(vo.MallId))
                {
//                    Debug.LogError(vo.MallId);
                    RmbMallDic[vo.MallId] = vo;
                }
                else
                {
                    RmbMallDic.Add(vo.MallId,vo); 
                }

            }
            


        }

        public void InitUserMallInfo(MallInfoRes res)
        {
            UserBuyMallInfoPb = res.UserBuyMallInfo;

            if (UserBuyGameMallList==null)
            {
                UserBuyGameMallList=new List<UserBuyGameMallVo>();
            }
            UserBuyGameMallList.Clear();
            foreach (var v in res.UserBuyGameMall)
            {
//                Debug.LogError(v);
                var buygamemallItem=new UserBuyGameMallVo(v);
                UserBuyGameMallList.Add(buygamemallItem);
            }

//            Debug.LogError(UserBuyGameMallList.Count);
            
            if (UserBuyRmbMallList==null)
            {
                UserBuyRmbMallList=new List<UserBuyRmbMallVo>();
            }
            UserBuyRmbMallList.Clear();
            foreach (var v in res.UserBuyRmbMall)
            {
                var buyrmbmallItem=new UserBuyRmbMallVo(v);
                UserBuyRmbMallList.Add(buyrmbmallItem);
            }

        }

        public RmbMallVo GetMonthCardMall
        {
            get
            {
                return RmbMallDic[6001];
            }
        }
        
        public List<UserBuyRmbMallVo> GetBuyGemRmbMallList
        {
            get
            {
                
                
                List<UserBuyRmbMallVo> buyRmbMallVos=new List<UserBuyRmbMallVo>();
                foreach (var vo in UserBuyRmbMallList)
                {
                    if (RmbMallDic.ContainsKey(vo.MallId))
                    {
                        bool hasthisMall = GlobalData.PayModel.GetProduct(vo.MallId) != null;
                        var rmbmallvo = RmbMallDic[vo.MallId];
                        if (hasthisMall&&rmbmallvo.MallSortPb==MallSortPB.MallOrdinary&&GlobalData.PayModel.GetProduct(vo.MallId)?.ProductType==CommodityTypePB.Recharge&&rmbmallvo.EndTime>ClientTimer.Instance.GetCurrentTimeStamp())                     
                        {
                            buyRmbMallVos.Add(vo);  
                        }                                     
                    }
                    
//                    if (RmbMallDic.ContainsKey(vo.MallId)&&(RmbMallDic[vo.MallId].MallSortPb==MallSortPB.MallOrdinary)&&rmbmallvo.EndTime>ClientTimer.Instance.GetCurrentTimeStamp())
//                    {
//                        buyRmbMallVos.Add(vo);                     
//                    }
                }
                if (buyRmbMallVos.Count>0)
                {
                    buyRmbMallVos.Sort((vo1, vo2) =>
                    {
                        if (RmbMallDic[vo1.MallId].Slot < RmbMallDic[vo2.MallId].Slot) return -1;
                        if (RmbMallDic[vo1.MallId].Slot > RmbMallDic[vo2.MallId].Slot)
                        {

                            return 1;
                        }

                        return 0;
                    });
                
                }
                

                return buyRmbMallVos;
            }
        }

        public bool HasDoublePrice(int realproce)
        {
            foreach (var v in GlobalData.PlayerModel.PlayerVo.FirstRecharges)
            {
                if (realproce == v.Amount)
                {
                    return true;
                }
            }

            return false;
        }
        
        
        /// <summary>
        /// 获取礼包或者VIP商品
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<UserBuyRmbMallVo> GetTargetRmbMallList(int page)
        {
            var targetList = new List<UserBuyRmbMallVo>();
            
            foreach (var vo in UserBuyRmbMallList)
            {
                if (RmbMallDic.ContainsKey(vo.MallId))
                {
                    var rmbmallvo = RmbMallDic[vo.MallId];
                    if (rmbmallvo.Slot/100==page&&rmbmallvo.BuyMax>0)                     
                    {
                        targetList.Add(vo);  
                    }                                     
                }
                                
            }

//            Debug.LogError("targetList"+targetList.Count);
            return targetList;
        }

        /// <summary>
        /// 获取星钻金币商品
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<UserBuyGameMallVo> GetTargetGameMallList(int page)
        {
            var targetList = new List<UserBuyGameMallVo>();
            
            foreach (var vo in UserBuyGameMallList)
            {
                //Debug.LogError(page+" "+vo.MallId+" "+vo.BuyNum+" "+vo.MallTypePb);
                if (GameMallDic.ContainsKey(vo.MallId)&&GameMallDic[vo.MallId].Slot/100==page)
                {
//                    Debug.LogError(""+vo.MallId);
                    targetList.Add(vo);                     
                }
                                
            }

//            Debug.LogError("targetList"+targetList.Count);
            return targetList;
        }

        public void UpdateUserBuyGameMallVo(UserBuyGameMallPB userBuyGameMallPbs)
        {
            foreach (var v in UserBuyGameMallList)
            {
                if (v.MallId==userBuyGameMallPbs.MallId)
                {
                    v.UserId = userBuyGameMallPbs.UserId;
                    v.BuyNum = userBuyGameMallPbs.BuyNum;
                    v.MallTypePb = userBuyGameMallPbs.MallType;
                    return;
                } 
            }
            Debug.LogError("could not find vo");
            UserBuyGameMallList.Add(new UserBuyGameMallVo(userBuyGameMallPbs));
                        
        }
        
        public void UpdateUserBuyGameMallVo(RepeatedField<UserBuyGameMallPB> userBuyGameMallPbs)
        {
            var tempbuymallList = new List<UserBuyGameMallVo>();
            for (int i = 0; i < UserBuyGameMallList.Count; i++)
            {
                if (GameMallDic[UserBuyGameMallList[i].MallId].MallType==MallTypePB.MallGold)
                {
                    tempbuymallList.Add(UserBuyGameMallList[i]);
                }               
            }

            foreach (var v in tempbuymallList)
            {
                UserBuyGameMallList.Remove(v);
            }
            

            foreach (var v in userBuyGameMallPbs)
            {
                var buygamemallItem=new UserBuyGameMallVo(v);
                UserBuyGameMallList.Add(buygamemallItem);
            }
            
        }

        public int GetMallRefreshGoldCost(int count)
        {
            foreach (var v in MallRefreshGoldRulePbs)
            {
                if (v.Count==count)
                {
                    return v.Gem;
                }
            }
            Debug.LogError("refresh time over");
            Refresh = true;
            return MallRefreshGoldRulePbs[MallRefreshGoldRulePbs.Count - 1].Gem;
        }

        public override void Destroy()
        {
            base.Destroy();
        }
    }
    
    



