using Com.Proto;
using DataModel;
using Google.Protobuf.Collections;

namespace Assets.Scripts.Module.Supporter.Data
{
    public class SupporterVo
    {
        public int Level;

        /// <summary>
        /// 当前等级的经验
        /// </summary>
        public int Exp;

        public DepartmentTypePB type;

        public UserPropVo Prop;

        public int Power;
        
        private DepartmentRulePB _rule;
        

        public int AniState=0; //0表示不需要改变待机。1.表示走路。2.表示挥手

        public RepeatedField<AwardPB> AwardPbs;
        
        public int CostNum=0;


        /// <summary>
        /// 升级需要经验
        /// </summary>
        public int ExpNeed;

        public SupporterVo(UserDepartmentPB pb)
        {
            Level = pb.Level;
            AwardPbs = pb.Awards;
            type = pb.DepartmentType;

            int propId = -1;
            switch (pb.DepartmentType)
            {
                case DepartmentTypePB.Support:
                    break;
                case DepartmentTypePB.Active:
                    propId = PropConst.SupporterActive;
                    break;
                case DepartmentTypePB.Financial:
                    propId = PropConst.SupporterFinancial;
                    break;
                case DepartmentTypePB.Resource:
                    propId = PropConst.SupporterResource;
                    break;
                case DepartmentTypePB.Transmission:
                    propId = PropConst.SupporterTransmission;
                    break;
            }

            Prop = GlobalData.PropModel.GetUserProp(propId);
            if(Prop == null)
            {
                Prop = new UserPropVo(propId);
            }

            _rule = GetRule(pb.Level);

            //Debug.LogError(pb.DepartmentType+" _rule.Power"+_rule.Power);
            if (pb.DepartmentType==DepartmentTypePB.Support&&_rule.Power>GlobalData.PlayerModel.BaseSupportPower)
            {
                GlobalData.PlayerModel.BaseSupportPower = _rule.Power;
                //Debug.LogError(GlobalData.PlayerModel.BaseSupportPower);
            }
            else
            {
                Power = _rule.Power+(GlobalData.PlayerModel.BaseSupportPower/4);//这个值要添加一个基础值。
                //Debug.LogError(Power);
            }


            DepartmentRulePB preLevelRule = GetRule(pb.Level - 1);

            DepartmentRulePB curLevelRule = GetRule(pb.Level);
            //Debug.LogError("preLevelRule"+preLevelRule?.Exp+" _rule.Exp"+ curLevelRule?.Exp+" ");
            if (preLevelRule==null)
            {
                ExpNeed = curLevelRule.Exp;
            }
            else
            {
                ExpNeed =  curLevelRule.Exp-preLevelRule.Exp;
            }


            DepartmentRulePB prevRule = GetRule(pb.Level - 1);
            if (prevRule == null)
            {
                Exp = pb.Exp;
            }
            else
            {
                Exp = pb.Exp - prevRule.Exp;
            }
        }

        public DepartmentRulePB GetRule(int level)
        {
            if (level < 0)
                return null;
            
            int levelStart = (int) type * 101;
            return GlobalData.DepartmentRule.DepartmentRules[levelStart + level];
        }
        
        public string PropsTexturePath
        {
            get { return "Prop/" + 1100; }//Prop.ItemId
        }
    }
}