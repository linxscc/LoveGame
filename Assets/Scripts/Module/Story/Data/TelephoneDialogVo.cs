using System;
using UnityEngine.Serialization;

namespace game.main
{
    [Serializable]
    public class TelephoneDialogVo
    {
        public EntityVo.DialogFrameStyle DialogFrameStyle;

        public bool IsHeroine;

        public string Content;

        public string HeadId;

        public string AudioId;

        public TelephoneDialogVo Clone()
        {
            TelephoneDialogVo vo = new TelephoneDialogVo();

            vo.DialogFrameStyle = DialogFrameStyle;
            vo.IsHeroine = IsHeroine;
            vo.Content = Content;
            vo.HeadId = HeadId;
            vo.AudioId = AudioId;
            
            return vo;
        }
    }
}