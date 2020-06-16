using System;

namespace game.main
{
    [Serializable]
    public class SmsDialogVo
    {
        public bool IsLeft;

        public string ContextText;
        
        public SmsDialogVo Clone()
        {
            SmsDialogVo vo = new SmsDialogVo();
            vo.IsLeft = IsLeft;
            vo.ContextText = ContextText;

            return vo;
        }

    }
}