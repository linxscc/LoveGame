using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;


    public class SupporterActivityModel : Model
    {
        public List<EncourageActRuleVo> EncourageActRuleVos;
        public Dictionary<int, EncourageActRuleVo> EncourageRuleDic;
        
        public List<EncourageActRefreshVo> EncourageActRefreshVos;
        public List<EncourageActDoneRuleVo> EncourageActDoneRuleVos;
        
        public List<UserEncourageActVo> UserEncourageActVos;
        public RepeatedField<EncourageActBuyRulePB> EncourageActBuyRulePbs;

        public UserEncourageActVo RefreshUserStartActItem;
        
        public int RefreshCount;
        public long NextTime;

        public int hasBuyTime;
        public int leftBuyTime;

        public void InitEncourageActRule(List<EncourageActRulePB> encourageActRulesReses)
        {
            if (EncourageActRuleVos==null)
            {
                EncourageActRuleVos=new List<EncourageActRuleVo>();
                EncourageRuleDic=new Dictionary<int, EncourageActRuleVo>();
            }

            foreach (var v in encourageActRulesReses)
            {
                EncourageActRuleVo vo=new EncourageActRuleVo(v);
                
                //记得做校验
                EncourageActRuleVos.Add(vo);
                if (EncourageRuleDic.ContainsKey(vo.Id))
                {
                    EncourageRuleDic[vo.Id] = vo;
                }
                else
                {
                    EncourageRuleDic.Add(vo.Id,vo);
                }
 
            }
            
            
            
        }

        public void InitEncourageRefresh(List<EncourageActRefreshRulePB> encourageActRefreshRulePbs)
        {
            if (EncourageActRefreshVos==null)
            {
                EncourageActRefreshVos=new List<EncourageActRefreshVo>();

            }

            foreach (var v in encourageActRefreshRulePbs)
            {
                EncourageActRefreshVo vo=new EncourageActRefreshVo(v);
                         
                //记得做校验
//                Debug.LogError(vo.Count+" "+vo.Gold);
                EncourageActRefreshVos.Add(vo);
            }
            
            
        }

        public void InitEncourageDone(List<EncourageActDoneRulePB> encourageActDoneRulePbs)
        {
            if (EncourageActDoneRuleVos==null)
            {
                EncourageActDoneRuleVos = new List<EncourageActDoneRuleVo>();
            }

            foreach (var v in encourageActDoneRulePbs)
            {
                EncourageActDoneRuleVo vo=new EncourageActDoneRuleVo(v);

                EncourageActDoneRuleVos.Add(vo);
            }
            
            
        }

        public void InitActBuyRule(RepeatedField<EncourageActBuyRulePB> vos)
        {
            EncourageActBuyRulePbs = vos;
           
        }

        public EncourageActBuyRulePB GetBuyRulePb(int buytimes)
        {
            foreach (var v in EncourageActBuyRulePbs)
            {
                if (v.Count==buytimes)
                {
                    return v;

                }
                
            }

            //应该不可以超过最大购买次数的！
            return EncourageActBuyRulePbs[EncourageActBuyRulePbs.Count-1];
        }
        

        public void GetMyEncourageActs(List<UserEncourageActPB> userEncourageActPbs)
        {
            if (UserEncourageActVos==null)
            {
                UserEncourageActVos = new List<UserEncourageActVo>();
            }
            UserEncourageActVos.Clear();
            foreach (var v in userEncourageActPbs)
            {
                UserEncourageActVo vo=new UserEncourageActVo(v);
                UserEncourageActVos.Add(vo);
            }
            
        }

        public void PushActFinishTime()
        {
            //var pushDic=new Dictionary<string,string>();
            
            foreach (var v in UserEncourageActVos)
            {
                if (v.StartState==1)
                {
                    long time = v.AcceptTime + EncourageRuleDic[v.ActId].NeedTime * 60 * 1000;
                    string gettime =
                        DateUtil.GetDataTime(time).ToString();
//                    Debug.LogError(gettime);
                    
                    //pushDic.Add(time.ToString(),I18NManager.Get("Push_HintGetEncAct"+UnityEngine.Random.Range(0,3)));
                    SdkHelper.PushAgent.PushSupporterAct(time);
                }
            }
            //SdkHelper.PushAgent.Push(pushDic);
           // SdkHelper.PushAgent.PushAll();
        }

        public void UpdateUserEncourageActs(List<UserEncourageActPB> userEncourageActPbs)
        {
            for (int i = 0; i < UserEncourageActVos.Count; i++)
            {
                foreach (var v in userEncourageActPbs)
                {
                    if (UserEncourageActVos[i].Id == v.Id)
                    {
                        UserEncourageActVos[i].ActId = v.ActId;
                        UserEncourageActVos[i].AcceptTime = v.AcceptTime;
                        UserEncourageActVos[i].AwardState = v.AwardState;
                        UserEncourageActVos[i].StartState = v.StartState;
                        UserEncourageActVos[i].ImmediateFinishState = v.ImmediateFinishState;
                    }
    

                }

            }
            
            
        }
        
        
        public void UpdateEncourageActs(UserEncourageActPB userEncourageActVo,int replaceId=0)
        {
            if (UserEncourageActVos==null)
            {
                Debug.LogError("BUG!USERdata is null");
                return;
            }
            
            
            foreach (var v in UserEncourageActVos)
            {
               // Debug.LogError(userEncourageActVo);
                if (v.ActId==userEncourageActVo.ActId)
                {
                    v.Id = userEncourageActVo.Id;
                    v.UserId = userEncourageActVo.UserId;
                    v.AcceptTime = userEncourageActVo.AcceptTime;
                    v.AwardState = userEncourageActVo.AwardState;
                    //Debug.LogError("userEncourageActVo.AwardState"+userEncourageActVo.AwardState);
                    v.ImmediateFinishState = userEncourageActVo.ImmediateFinishState;
                    if (v.ImmediateFinishState==1)
                    {
//                        Debug.LogError("ImmediateFinishState");
                    }
                    
                    v.StartState = userEncourageActVo.StartState;
                    v.UserId = userEncourageActVo.UserId;
                    RefreshUserStartActItem = v;
                    return;
                }            
            }

            if (replaceId!=userEncourageActVo.ActId)
            {
//                Debug.LogError("replace new task!");
                var removeItem=UserEncourageActVos.Find(x => x.ActId == replaceId);
//                UserEncourageActVos.Remove(removeItem);
//                UserEncourageActVo vo=new UserEncourageActVo(userEncourageActVo);
//                UserEncourageActVos.Add(vo);
                foreach (var v in UserEncourageActVos)
                {
                    if (v.ActId==replaceId)
                    {
                        v.ActId = userEncourageActVo.ActId;
                        v.Id = userEncourageActVo.Id;
                        v.UserId = userEncourageActVo.UserId;
                        v.AcceptTime = userEncourageActVo.AcceptTime;
                        v.AwardState = userEncourageActVo.AwardState;
                        //Debug.LogError("userEncourageActVo.AwardState"+userEncourageActVo.AwardState);
                        v.ImmediateFinishState = userEncourageActVo.ImmediateFinishState;
                        if (v.ImmediateFinishState==1)
                        {
//                        Debug.LogError("ImmediateFinishState");
                        }
                    
                        v.StartState = userEncourageActVo.StartState;
                        v.NeedToChangeAni = false;
                        v.CanReceiveAward = false;
                        RefreshUserStartActItem = v;
                        
                    }
                }
                
                
//                Debug.LogError(UserEncourageActVos.Count);
            }
            
            
        }

        public int GetGemByTime(long lefttime)
        {
            var hour = lefttime;
            foreach (var v in EncourageActDoneRuleVos)
            {
                //要求hour符合某区间
                if (hour<=(v.Time/60))
                {
                    return v.Gem;
                }
                
                
            }

            Debug.LogError("Could not find gem!"+hour);
            return 0;
        }

        public EncourageActRefreshVo GetRefreshCost(int refreshcount)
        {
            //小心，refreshCount是有限制的，大于0和小于16次
            if (EncourageActRefreshVos!=null)
            {
                foreach (var v in EncourageActRefreshVos)
                {
                    if (v.Count==refreshcount)
                    {
                        return v;
                    }

                    if (v.Count==16&&refreshcount>=16)
                    {
                        return v;
                    }
                    
                }
            }
       
            return null;

        }

        public UserEncourageActVo GetUserActVo(int id)
        {
            foreach (var vo in UserEncourageActVos)
            {
                if (vo.Id==id)
                {
                    return vo;
                }
            }

            Debug.LogError("no target uservo?");
            return null;
        }

        public bool GetRewardActNum()
        {
            int num = 0;
            foreach (var v in UserEncourageActVos)
            {
                if (v.StartState==1&&v.AwardState==0)
                {
                    num++;
                }
                
            }

            return num > 0;
        }
        
        public override void Destroy()
        {
            base.Destroy();
            ClientData.Clear();
        }
    }


