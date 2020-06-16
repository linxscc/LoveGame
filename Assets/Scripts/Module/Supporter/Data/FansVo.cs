using Com.Proto;

namespace Module.Supporter.Data
{
    public class FansVo
    {
        public string Name;

        public int Num;

        public string Description;

        public int FansId;
        
        public int Power;

        
        
        public FansVo(UserFansPB pb, FansRulePB rule)
        {
            Name = rule.FansName;
            Description = rule.FansDesc;
            FansId = pb.FansType;
            Num = pb.Num;
            Power = rule.Power;
        }
        public FansVo(int fansId,int num, FansRulePB rule)
        {
            Name = rule.FansName;
            Description = rule.FansDesc;
            FansId = fansId;
            Num = num;
            Power = rule.Power;
        }

        public string FansTexturePath
        {
            get { return "FansTexture/" + FansId; }     //更改路径
        }
        
        public string FansHeadPath
        {
            get { return "FansTexture/Head/" + FansId; }
        }

        public string FansBigTexturePath
        {
            get { return "FansTexture/BigCards/" + FansId; }
        }

        public string FansTextColor
        {
            get
            {
                switch (FansId)
                {
                    case 1000:
                        return "#ff877c";
                    case 1001:
                        return "#81aeff";
                    case 1002:
                        return "#ff97ae";
                    case 1003:
                        return "#f9bd65";
                    case 1004:
                        return "#fed877";
                    case 1005:
                        return "#b8dfa2";
                    case 1006:
                        return "#acd1fe";
                    case 1007:
                        return "#d4bddc";
                    case 1008:
                        return "#808080";
                    case 1009:
                        return "#ffadca";
                       default:
                           return "#ff877c";
                }
            }
        }
    }
}