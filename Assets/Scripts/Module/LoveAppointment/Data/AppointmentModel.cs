using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using game.main;
using UnityEngine;
using DataModel;
using Utils;

public class AppointmentModel : Model
    {
        //约会数据模型结构
        public List<AppointmentRuleVo> _appointmentRuleVos;
        public List<UserAppointmentVo> _userAppointmentVo;
        public List<AppointmentRuleVo> _targetAppointmentVo;
        //public Dictionary<int, UserAppointmentVo> _userAppointmentDic;
        public AppointmentRuleVo _curAppointmentRuleVo;//全局的约会数据


        public void InitMyAppointmentData(MyAppointmentRes res)
        {
            if (_userAppointmentVo == null)
            {
                _userAppointmentVo=new List<UserAppointmentVo>();
                foreach (var t in res.UserAppointments)
                {                  
                    _userAppointmentVo.Add(new UserAppointmentVo(t)); 
                }
            }
            else
            {
                _userAppointmentVo.Clear();
                foreach (var t in res.UserAppointments)
                {
                    _userAppointmentVo.Add(new UserAppointmentVo(t)); 
                }
            }
        }

        public int GetAppointmentUnlockNum(PlayerPB playerPb)
        {
            int curUnlockNum = 0;
            int npcId = (int) playerPb;
            foreach (var user in _userAppointmentVo)
            {
                if (user.AppointmentId/1000== npcId)
                {
                    curUnlockNum += user.ActiveGateInfos.Count;
                }
            } 
            
            return  curUnlockNum;
        }
        
        
        public void SetCurAppointmentRule(AppointmentRuleVo vo)
        {
            _curAppointmentRuleVo = vo;
        }
        
        public void UpdateUserAppointment(UserAppointmentPB pb)
        {
            //Debug.LogError(pb);
            var userappointmentVo=new UserAppointmentVo(pb);
            if (_userAppointmentVo.Count==0||_userAppointmentVo.Find(x=>x.AppointmentId==userappointmentVo.AppointmentId)==null)
            {
                _userAppointmentVo.Add(userappointmentVo);
                return;
            }
            foreach (var v in _userAppointmentVo)
            {
                if (v.AppointmentId==userappointmentVo.AppointmentId)
                {
                    v.ActiveGateInfos = userappointmentVo.ActiveGateInfos;
                    v.ActiveState = userappointmentVo.ActiveState;
//                    v.ClearState = userappointmentVo.ClearState;
                    v.CreateTime = userappointmentVo.CreateTime;
                    v.FinishGateInfos = userappointmentVo.FinishGateInfos;
                    v.LastModifyTime = userappointmentVo.LastModifyTime;
//                    v.NailUpState = userappointmentVo.NailUpState;
                    v.UserId = userappointmentVo.UserId;
                }
            }
            
        }

        public void UpdatePassGateAwardToProp(UserAppointmentVo userAppointmentVo)
        {
            var appointmentvo = GetAppointmentRule(userAppointmentVo.AppointmentId);
            foreach (var v in appointmentvo.GateInfos)
            {
                if (v.Gate==userAppointmentVo.FinishGateInfos[userAppointmentVo.FinishGateInfos.Count-1])
                {
                    //Debug.LogError("find pass gate reward");
                    Debug.LogError("get reward!"+v.Awards);
                    RewardUtil.AddReward(v.Awards);
                }
            }
        }

        public AppointmentRuleVo GetAppointmentRule(int appointmentId)
        {
            return _appointmentRuleVos.Find(x => x.Id == appointmentId);
        }
        
        
        public UserAppointmentVo GetUserAppointment(int id)
        {
            if (_userAppointmentVo == null) return null;
            foreach (var v in _userAppointmentVo)
            {
                if (v.AppointmentId==id)
                {
                    return v;
                }
            }

            return null;
        }
        
        public GateState IsGateActive(int jouruanid,int gateid)
        {
            //TODO 优化成Map

            foreach (var v in _userAppointmentVo)
            {
                //Debug.LogError(v.ActiveGateInfos.Count);

                if (v.AppointmentId != jouruanid) continue;
                if (v.FinishGateInfos != null)
                {
                    //Debug.LogError(v.FinishGateInfos.Count);
                    foreach (var b in v.FinishGateInfos)
                    {
                        if (b==gateid)
                        {
                            return GateState.Finish;
                        }
                    }
                }
                foreach (var a in v.ActiveGateInfos)
                {
                    if (a == gateid)
                    {
                        return GateState.Active;
                    }
                }
            }
            
            return GateState.NotAcive;
        }
        
        
        public List<AppointmentRuleVo> GetTargetData(int typeId)
        {
            if (_appointmentRuleVos != null)
            {
                int targetApoId = typeId - 10000;
                _targetAppointmentVo=new List<AppointmentRuleVo>();
                //Debug.LogError(_appointmentRuleVos.Count);
                foreach (var v in _appointmentRuleVos)
                {
                    if (v.Id / 1000 == targetApoId)
                    {
                        _targetAppointmentVo.Add(v);
                    }
                }

                return _targetAppointmentVo;
            }

            Debug.LogError("No target AppointmentVo");
            return null;
        }

        public UserAppointmentVo GetCardAppointmentVo(int cardId)
        {
            if (_appointmentRuleVos!=null)
            {
                foreach (var v in _appointmentRuleVos)
                {
                    if (v.ActiveCards.Count>0&&v.ActiveCards[0]==cardId)
                    {

                        return GetUserAppointment(v.Id);
                    }
                }                
            }

            return null;
        } 
        
        public AppointmentRuleVo GetCardAppointmentRuleVo(int cardId)
        {
            if (_appointmentRuleVos!=null)
            {
                foreach (var v in _appointmentRuleVos)
                {
                    if (v.ActiveCards.Count>0&&v.ActiveCards[0]==cardId)
                    {

                        return v;
                    }
                }                
            }

            return null;
        } 
        
        
        public List<AppointmentRuleVo> GetAppointmentRules(AppointmentRuleRes res)
        {
            if (_appointmentRuleVos == null)
            {
                _appointmentRuleVos=new List<AppointmentRuleVo>();        
                foreach (var v in res.AppointmentRules)
                {
                    _appointmentRuleVos.Add(new AppointmentRuleVo(v));
                }

                return _appointmentRuleVos;
            }

            return _appointmentRuleVos;
        }


        
        
        public bool NeedSetRedPoint(UserAppointmentVo userappointment,int id)
        {
            var cardvo = GlobalData.CardModel.GetUserCardById(id);
            if (cardvo==null)
            {
                //Debug.LogError("no cardvo"+id);
                return false;
            }
            //Debug.LogError(userappointment.ActiveState);
            if (userappointment.ActiveState!=1)
            {
                return true;
            }

            if (userappointment.ActiveState==1&&userappointment.FinishGateInfos.Count==0)
            {
                return true;
            }

            //此处为新增功能，可能有BUG！
            if (userappointment.ActiveGateInfos.Count!=userappointment.FinishGateInfos.Count)
            {
                return true;
            }
            

            if (userappointment.FinishGateInfos.Count>0)
            {
                //判断解锁到第几章，然后判断
                var appointmentvo = _appointmentRuleVos.Find(x => x.Id == userappointment.AppointmentId); 
                switch (userappointment.FinishGateInfos.Count)
                {
                    case 1:
                        if (cardvo?.Star>=appointmentvo.GateInfos[1].Star)
                        {
                            return true;
//                            bool isEnough = false;
//                            foreach (var v in appointmentvo.GateInfos[1].Cosumes)
//                            {
//                                isEnough = v.Value <= GlobalData.PropModel.GetUserProp(v.Key).Num;
//                            }
//                            
//                            return isEnough;
                        }
                        break;
                    case 2:
                        if (cardvo?.Star>=appointmentvo.GateInfos[2].Star)
                        {
                            return true;
//                            bool isEnough = false;
//                            foreach (var v in appointmentvo.GateInfos[2].Cosumes)
//                            {
//                                isEnough = v.Value <= GlobalData.PropModel.GetUserProp(v.Key).Num;
//                            }
//                            
//                            return isEnough;
                        }
                        break;
                    case 3:
                        //var appointmentvo3 = _appointmentRuleVos.Find(x => x.Id == userappointment.AppointmentId); 
                        if (cardvo?.Evolution>=(EvolutionPB)appointmentvo.GateInfos[3].Evo)
                        {
                            return true;
//                            bool isEnough = false;
//                            foreach (var v in appointmentvo.GateInfos[3].Cosumes)
//                            {
//                                isEnough = v.Value <= GlobalData.PropModel.GetUserProp(v.Key).Num;
//                            }
//                            
//                            return isEnough;
                        }
                        break;
                    case 4:
                        //暂时还没有拍立得钉起来。
                        //新需求，拍立得需要显示红点？

                        foreach (var v in appointmentvo.GateInfos)
                        {
                            foreach (var e in v.Awards)
                            {
                                if (!GlobalData.DiaryElementModel.IsUserElement(e.ResourceId))
                                {
                                    Debug.LogError("redpoint!");
                                    return true;
                                }
                            }

                        }
                        
                        break;
                }

            }
            return false;
        }
    }

