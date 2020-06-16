using System;
using System.Collections.Generic;
using Com.Proto;
using Module.Supporter.Data;
using UnityEngine;

namespace DataModel
{
    public class MyDepartmentData
    {
        public List<DepartmentVo> MyDepartments;
        public List<FansVo> Fanss;
        public bool CanGetFriendsPower;
        public bool CanGetSupporterActAward=false;

        public void InitData(MyDepartmentRes res)
        {
            MyDepartments = new List<DepartmentVo>();
            foreach (var pb in res.MyDepartments)
            {
                var vo = new DepartmentVo
                {
                    UserDepartmentPb = pb, 
                    RulePb = GetDepartmentRule(pb.DepartmentType, pb.Level)
                };
                MyDepartments.Add(vo);
            }

            Fanss = new List<FansVo>();
            foreach (var pb in res.Fanss)
            {
                var rule = GetFansRule(pb.FansType);
                Fanss.Add(new FansVo(pb,rule));
            }
        }

        public void UpdateDepartmentMainValue()
        {
            int level = GlobalData.PlayerModel.PlayerVo.Level;
            foreach (var vo in MyDepartments)
            {
                if (vo.RulePb.DepartmentType == DepartmentTypePB.Support)
                {
                    vo.RulePb = GetDepartmentRule(vo.RulePb.DepartmentType, level);
                    vo.UserDepartmentPb = new UserDepartmentPB();
                    vo.UserDepartmentPb.Level = level;
                    vo.UserDepartmentPb.DepartmentType = DepartmentTypePB.Support;
                    vo.UserDepartmentPb.Exp = GlobalData.PlayerModel.PlayerVo.Exp;
                }
            }
        }

        public bool CanLevelUpDepartment()
        {
            if (GlobalData.PropModel.GetUserProp(PropConst.SupporterActive).Num > 0 &&
                GetTargetDepartment(DepartmentTypePB.Active).Level < 100 ||
                GlobalData.PropModel.GetUserProp(PropConst.SupporterFinancial).Num > 0 &&
                GetTargetDepartment(DepartmentTypePB.Financial).Level < 100 ||
                GlobalData.PropModel.GetUserProp(PropConst.SupporterResource).Num > 0 &&
                GetTargetDepartment(DepartmentTypePB.Resource).Level < 100||
                GlobalData.PropModel.GetUserProp(PropConst.SupporterTransmission).Num > 0 &&
                GetTargetDepartment(DepartmentTypePB.Transmission).Level < 100)
            {
                return true;
            }

            return false;
        }

        public UserDepartmentPB GetTargetDepartment(DepartmentTypePB typePb)
        {
            foreach (var vo in MyDepartments)
            {
                if (vo.RulePb.DepartmentType == typePb)
                {

                    return vo.UserDepartmentPb;
                }
            }

            return null;
        } 
        
        
        public void UpdateFans(int funId, int num)
        {
            FansVo funVo = GetFans(funId);
            if (funVo==null)
            {
                Debug.LogError(funId);
                var rule = GetFansRule(funId);
                Fanss.Add(new FansVo(funId, num, rule));

            }
            else
            {     
               // Debug.LogError(funId+" "+num);
                funVo.Num += num;
            }

        }

        public void UpdateFansWithNum(int funId, int num)
        {
            FansVo funVo = GetFans(funId);
            if (funVo==null)
            {
                Debug.LogError(funId);
                var rule = GetFansRule(funId);
                Fanss.Add(new FansVo(funId, num, rule));

            }
            else
            {                
                //应援会粉丝数量要更变，而不是累加！！                
                funVo.Num = num;
//                Debug.LogError(funVo.Name+" "+num);
            }  
        }
        

        public FansVo GetFans(int id)
        {
            if (Fanss==null)
            {
                Debug.LogError("need to ladata"+id);
            }
            
            foreach (var fansVo in Fanss)
            {
                if (fansVo.FansId == id)
                    return fansVo;
            }
            return null;
        }
        
        
        public static DepartmentRulePB GetDepartmentRule(DepartmentTypePB type, int level)
        {
            var rules = GlobalData.DepartmentRule.DepartmentRules;
            foreach (var rule in rules)
            {
                if (rule.DepartmentType.Equals(type) && rule.Level == level)
                {
                    return rule;
                }
            }

            return null;
        }
        
        
        
          
        public static FansRulePB GetFansRule(int type)
        {
            var rules = GlobalData.DepartmentRule.FansRules;
            foreach (var rule in rules)
            {
                if (rule.FansType.Equals(type))
                {
                    return rule;
                }
            }

            return null;
        }


        //GlobalData.DepartmentRule.FansRules[i]
    }
}