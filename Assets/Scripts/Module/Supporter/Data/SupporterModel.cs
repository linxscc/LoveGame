using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using DataModel;
using Module.Supporter.Data;

namespace Assets.Scripts.Module.Supporter.Data
{
    public class SupporterModel : Model
    {
        public SupporterVo MainVo;
        public SupporterVo Active;
        public SupporterVo Financial;
        public SupporterVo Resource;
        public SupporterVo Transmission;
        public List<FansVo> FansList;
        
        public void InitData(MyDepartmentData res)
        {
            for (int i = 0; i < res.MyDepartments.Count; i++)
            {
                UserDepartmentPB pb = res.MyDepartments[i].UserDepartmentPb;
                switch (pb.DepartmentType)
                {
                    case DepartmentTypePB.Support:
                        MainVo = new SupporterVo(pb);
                        //MainVo.Exp = GlobalData.PlayerModel.PlayerVo.CurrentLevelExp;
                        break;
                    case DepartmentTypePB.Active:
                        Active = new SupporterVo(pb);
                        break;
                    case DepartmentTypePB.Financial:
                        Financial = new SupporterVo(pb);
                        break;
                    case DepartmentTypePB.Resource:
                        Resource = new SupporterVo(pb);
                        break;
                    case DepartmentTypePB.Transmission:
                        Transmission = new SupporterVo(pb);
                        break;
                }
            }

            FansList = res.Fanss;//new List<FansVo>();
        }

        public void Update(UserDepartmentPB pb,bool resetState=true)
        {
            SupporterVo vo = null;
            //UserDepartmentPB pb = res.MyDepartments;
            switch (pb.DepartmentType)
            {
                //没奖励的时候也要会动！
                
                case DepartmentTypePB.Support:
                    MainVo = new SupporterVo(pb);
                    break;
                case DepartmentTypePB.Active:
                    Active = new SupporterVo(pb);
                    SetAniState(resetState?1:0, 0, 0, 0);//resetState表示从挥手变成待机 pb.Awards.Count>0?1:resetState?0:2
                    break;
                case DepartmentTypePB.Financial:
                    Financial = new SupporterVo(pb);
                    SetAniState(0, resetState?1:0, 0, 0);
                    break;
                case DepartmentTypePB.Resource:
                    Resource = new SupporterVo(pb);
                    SetAniState(0, 0, resetState?1:0, 0);
                    break;
                case DepartmentTypePB.Transmission:
                    Transmission = new SupporterVo(pb);
                    SetAniState(0, 0, 0, resetState?1:0);
                    break;
            }


            DepartmentVo dpVo = new DepartmentVo();
            dpVo.UserDepartmentPb = pb;
            dpVo.RulePb = MyDepartmentData.GetDepartmentRule(dpVo.UserDepartmentPb.DepartmentType,dpVo.UserDepartmentPb.Level);
            for (int i = 0; i < GlobalData.DepartmentData.MyDepartments.Count; i++)
            {
                if (GlobalData.DepartmentData.MyDepartments[i].UserDepartmentPb.DepartmentType == dpVo.UserDepartmentPb.DepartmentType)
                {
                    GlobalData.DepartmentData.MyDepartments[i] = dpVo;
                    break;
                }
            }
        }

        
        
        private void SetAniState(int active,int finacial,int resource,int tranmission)
        {
            Active.AniState = active;
            Financial.AniState = finacial;
            Resource.AniState = resource;
            Transmission.AniState = tranmission;
        }
        
    }
}