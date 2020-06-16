using System;
using System.Collections.Generic;

namespace game.main
{
    [Serializable]
    public class TelephoneVo
    {
        public enum TelphoneRole
        {
            [Description("无")] None,
            [Description("秦予哲")] Qinyuzhe,
            [Description("唐弋辰")] Tangyichen,
            [Description("迟郁")] Chiyu,
            [Description("言季")] Yanji,
            [Description("陌生人")] Stranger,
            [Description("凌晓影")] LingXiaoYing,
        }
        
        public TelphoneRole Role;
        
        public string telphoneId;

        public string bgImageId;

        public List<TelephoneDialogVo> dialogList;
        
        public EventVo Event;
                
        public string GetRoleName()
        {
            switch (Role)
            {
                case TelphoneRole.Qinyuzhe:
                    return I18NManager.Get("Common_Role2");
                case TelphoneRole.Tangyichen:
                    return I18NManager.Get("Common_Role1");
                case TelphoneRole.Chiyu:
                    return I18NManager.Get("Common_Role4");
                case TelphoneRole.Yanji:
                    return I18NManager.Get("Common_Role3");
                case TelphoneRole.Stranger:
                    return I18NManager.Get("Common_Role_Stranger");
                case TelphoneRole.None:
                    break;
                case TelphoneRole.LingXiaoYing:
                    return I18NManager.Get("Common_Role55");
            }
            
            return I18NManager.Get("Story_RoleError");
        }
        
//        public string GetRoleImagePath()
//        {
//            string path = null;
//            switch (Role)
//            {
//                case TelphoneRole.Qinyuzhe:
//                    path = "Head/2008";
//                    break;
//                case TelphoneRole.Tangyichen:
//                    path = "Head/1005";
//                    break;
//                case TelphoneRole.Chiyu:
//                    path = "Head/EvolutionHead/4112";
//                    break;
//                case TelphoneRole.Yanji:
//                    path = "Head/EvolutionHead/3107";
//                    break;
//                case TelphoneRole.Stranger:
//                    path = "Head/PlayerHead/Stranger";
//                    break;
//            }
//
//            return path;
//        }
    }
}