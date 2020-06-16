using System.Collections.Generic;
using System.Linq;
using Com.Proto;

namespace game.main
{

    /// <summary>
    /// 任务基础数据
    /// </summary>
    public class MissionVo
    {
        public int MissionId;
        public string MissionName;
        public MissionTypePB MissionType;
        public List<AwardPB> Award;
        public string MissionDesc;
        public string JumpTo;

        public void InitData(MissionRulePB pb)
        {
            MissionId = pb.MissionId;
            MissionName = pb.MissionName;
            MissionType = pb.MissionType;
            Award = pb.Award.ToList();
            MissionDesc = pb.MissionDesc;
            JumpTo = pb.JumpTo;
        }
        
    }
    

    
}