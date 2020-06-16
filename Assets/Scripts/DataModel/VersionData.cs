using Assets.Scripts.Module.NetWork;
using System.Collections.Generic;

namespace DataModel
{
    public class VersionData
    {
        private Dictionary<string, string> _versionDic;

        public Dictionary<string, string> VersionDic => _versionDic;

        public string ForceUpdateAddr = "";
        
        public VersionData()
        {
            _versionDic = new Dictionary<string, string>();
            _versionDic[CMD.GAME_CONFIG] = "0";
            _versionDic[CMD.DEPARTMENTC_DEPARTMENT_RULE] = "0";
            _versionDic[CMD.CARDC_CARDS] = "0";
            _versionDic[CMD.CARDC_CARDRULE] = "0";
            _versionDic[CMD.ITEMC_ITEMS] = "0";
            _versionDic[CMD.CAREERC_LEVELS] = "0";
            _versionDic[CMD.NPC_RULES] = "0";
            _versionDic[CMD.MISSION_RULES] = "0";
            _versionDic[CMD.APPOINTMENT_RULES] = "0";
            //_versionDic[CMD.PHONEC_RULES] = "0";
            _versionDic[CMD.USERC_USERRULE] = "0";
            _versionDic[CMD.CARDMEMORIESC_CARDMEMORIESRULE] = "0";
            _versionDic[CMD.DRAWC_DRAW_PROBS] = "0";
            _versionDic[CMD.DIARYC_ELEMENTS_RULES] = "0";           
            _versionDic[CMD.FAVORABILITY_RULE] = "0";
            _versionDic[CMD.SUPPORTERACTIVITY_ENCOURAGEACTRULES] = "0";
            _versionDic[CMD.VISITINGC_RULE] = "0";
            _versionDic[CMD.ACTIVITY_RULES] = "0";
            _versionDic[CMD.RECHARGEC_RECHARGERULE] = "0";
            _versionDic[CMD.MALL_RULE] = "0";
            _versionDic[CMD.LITTLEGAMEC_GAMEJUMPINFOS] = "0";
            _versionDic[CMD.MUSICGAMEC_RULES] = "0";
            _versionDic[CMD.TAKEPHOTOC_RULES] = "0";
            _versionDic[CMD.ACTIVITY_ACTIVITYRULELIST] = "0";

        }

    }
}