using System;
using System.Collections.Generic;

namespace game.main
{
    [Serializable]
    public class SmsVo
    {
        public List<SmsDialogVo> dialogList;
        
        public string smsId;

        public SmsRole Role;
        
        public string bgImageId;
        
        public EventVo Event;
        
        public string GetRoleName()
        {
            switch (Role)
            {
                case SmsRole.Qinyuzhe:
                    return I18NManager.Get("Common_Role2");
                case SmsRole.Tangyichen:
                    return I18NManager.Get("Common_Role1");
                case SmsRole.Chiyu:
                    return I18NManager.Get("Common_Role4");
                case SmsRole.Yanji:
                    return I18NManager.Get("Common_Role3");
                case SmsRole.Stranger:
                    return I18NManager.Get("Common_Role_Stranger");
            }
            
            return I18NManager.Get("Story_RoleError");
        }
        
        public enum SmsRole
        {
            [Description("无")] None,
            [Description("秦予哲")] Qinyuzhe,
            [Description("唐弋辰")] Tangyichen,
            [Description("迟郁")] Chiyu,
            [Description("言季")] Yanji,
            [Description("陌生人")] Stranger,
        }
        
    }
}