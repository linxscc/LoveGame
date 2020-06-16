using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using game.main;
using UnityEngine;
using DataModel;
using Google.Protobuf.Collections;


public class ShopModel : Model
    {

        public RepeatedField<MallRefreshGoldRulePB> MallRefreshGoldRulePbs;
        public UserBuyMallInfoPB UserBuyMallInfoPb;

        public Dictionary<int, GameMallVo> GameMallDic;
        public Dictionary<int, RmbMallVo> RmbMallDic;

        public List<UserBuyGameMallVo> UserBuyGameMallList;
        public List<UserBuyRmbMallVo> UserBuyRmbMallList;

        public string[] MallPageDesc =  {I18NManager.Get("Shop_GiftDesc"),I18NManager.Get("Shop_VIPDesc"),I18NManager.Get("Shop_GemDesc"),I18NManager.Get("Shop_GoldDesc"),I18NManager.Get("Shop_BuyGemDesc") ,I18NManager.Get("Shop_BuyGemDesc")};
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
            
            GameMallDic.Clear();
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

            RmbMallDic.Clear();
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
//            Debug.LogError(DateUtil.GetDataTime(UserBuyMallInfoPb.RefreshTime));

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

        public UserBuyGameMallVo GetFreeGift
        {
            get
            {
                foreach (var vo in UserBuyGameMallList)
                {
                    if (GameMallDic.ContainsKey(vo.MallId)&&GameMallDic[vo.MallId].MallLabelPb==MallLabelPB.LabelDailyGift)
                    {

                        return vo;
                    }
                    
                }

                return null;
            }
        }

        public List<UserBuyRmbMallVo> GetSpecialGift
        {
            get
            {
                List<UserBuyRmbMallVo> buyRmbMallVos=new List<UserBuyRmbMallVo>();
                foreach (var vo in UserBuyRmbMallList)
                {
                    if (RmbMallDic.ContainsKey(vo.MallId)&&RmbMallDic[vo.MallId].Special)
                    {
                        buyRmbMallVos.Add(vo);
                    }
                    
                }
                return buyRmbMallVos;
            }
        }
        
        public RmbMallVo GetMonthCardMall
        {
            get
            {
                //这个之后要配表！
                var num = GlobalData.PayModel.GetMonthCardProduct();
                if (GlobalData.PayModel.GetMonthCardProduct()!=null)
                {
                    if (RmbMallDic.ContainsKey(num.CommodityId))
                    {
                        return RmbMallDic[num.CommodityId];  
                    }
  
                }

                    return null;

                
                
                if (RmbMallDic.ContainsKey(5000))
                {
                    return RmbMallDic[5000]; 
                }
                else
                {
                    foreach (var v in RmbMallDic)
                    {
                        if (v.Value.MallSortPb==MallSortPB.MallOrdinary)
                        {
                            return v.Value;
                        }
                        
                    }

                    return null;

                }

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

        public bool HasFreeGemMall()
        {

            foreach (var vo in UserBuyGameMallList)
            {
                if (GameMallDic.ContainsKey(vo.MallId))
                {
                    //bool hasthisMall = GlobalData.PayModel.GetProduct(vo.MallId) != null;
                    var gamemallvo = GameMallDic[vo.MallId];
                    if (gamemallvo.RealPrice==0&&vo.BuyNum<gamemallvo.BuyMax&&gamemallvo.MallLabelPb==MallLabelPB.LabelResources)
                    {
//                        Debug.LogError("HasFreeGemMalltrue");
                        return true;

                    }     
                    
                }
            }
 //           Debug.LogError("HasFreeGemMallfalse");
            return false;
        }

        /// <summary>
        /// 获取礼包或者VIP商品
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<UserBuyRmbMallVo> GetTargetRmbMallList(MallLabelPB label)
        {
            var targetList = new List<UserBuyRmbMallVo>();
            foreach (var vo in UserBuyRmbMallList)
            {
                if (RmbMallDic.ContainsKey(vo.MallId))
                {
                    bool hasthisMall = GlobalData.PayModel.GetProduct(vo.MallId) != null;
                    var rmbmallvo = RmbMallDic[vo.MallId];
                    bool buymax = vo.BuyNum == 1 && rmbmallvo.BuyRefreshDay == 9999;
//                    bool timeenable = rmbmallvo.MallLabelPb==label;
//                    if (timeenable)
//                    {
//                        Debug.LogError(rmbmallvo.MallId+" "+rmbmallvo.MallName); 
//                    }
//                    
//                    
//                    if (hasthisMall)
//                    {
//                        if (!timeenable)
//                        {
//                            //Debug.LogError(rmbmallvo.MallName+" MallLabelPb: "+rmbmallvo.MallLabelPb+" setLabel "+label);
//                        }
//                        else
//                        {
//                            Debug.LogError(rmbmallvo.MallName);
//                        }
//
//                    }
                    
                    //Debug.LogError(vo.MallId+" hasthisMall:"+hasthisMall+" time "+timeenable+"　buymax"+buymax);
                    if (rmbmallvo.MallLabelPb==label&&rmbmallvo.EndTime>ClientTimer.Instance.GetCurrentTimeStamp()&&hasthisMall&&!buymax)                     
                    {
                        targetList.Add(vo);  
                    }                                     
                }
                                
            }

            
//            Debug.LogError("targetList"+targetList.Count);
            if (targetList.Count>0)
            {
                targetList.Sort((vo1, vo2) =>
                {
                    if (RmbMallDic[vo1.MallId].Slot < RmbMallDic[vo2.MallId].Slot) return -1;
                    if (RmbMallDic[vo1.MallId].Slot > RmbMallDic[vo2.MallId].Slot)
                    {

                        return 1;
                    }

                    return 0;
                });
                
            }
            
            return targetList;



        }

        /// <summary>
        /// 获取星钻金币商品
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<UserBuyGameMallVo> GetTargetGameMallList(MallLabelPB label)
        {
            var targetList = new List<UserBuyGameMallVo>();
            
            foreach (var vo in UserBuyGameMallList)
            {
                //Debug.LogError(page+" "+vo.MallId+" "+vo.BuyNum+" "+vo.MallTypePb);
                if (GameMallDic.ContainsKey(vo.MallId)&&GameMallDic[vo.MallId].MallLabelPb==label)
                {
//                    Debug.LogError(""+vo.MallId);
                    targetList.Add(vo);                     
                }
                                
            }

            if (targetList.Count>0)
            {
                targetList.Sort((vo1, vo2) =>
                {
                    if (GameMallDic[vo1.MallId].Slot < GameMallDic[vo2.MallId].Slot) return -1;
                    if (GameMallDic[vo1.MallId].Slot > GameMallDic[vo2.MallId].Slot)
                    {

                        return 1;
                    }

                    return 0;
                });
                
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
                if (GameMallDic.ContainsKey(UserBuyGameMallList[i].MallId)&&GameMallDic[UserBuyGameMallList[i].MallId].MallType==MallTypePB.MallGold)
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

        public void UpdateUserRmbMallVo(RepeatedField<UserBuyRmbMallPB> userBuyRmbMallPbs)
        {
            for (int i = 0; i < userBuyRmbMallPbs.Count; i++)
            {
                foreach (var v in UserBuyRmbMallList)
                {
                    if (userBuyRmbMallPbs[i].MallId==v.MallId)
                    {
                        v.BuyNum = userBuyRmbMallPbs[i].BuyNum;
                        v.RefreshTime = userBuyRmbMallPbs[i].RefreshTime;
                        v.UserId = userBuyRmbMallPbs[i].UserId;
                    }
                }
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

    }
    
    



