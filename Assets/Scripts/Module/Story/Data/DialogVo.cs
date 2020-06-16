using System;
using System.Collections.Generic;

namespace game.main
{
    [Serializable]
    public class DialogVo
    {
        public int DialogId;
        
        public string BgImageId;

        /// <summary>
        /// 配置${mute}可以停止播放背景音乐，不配置会播放之前的音乐
        /// </summary>
        public string BgMusicId;

        /// <summary>
        /// 背景宽度
        /// </summary>
        public float Width;
        /// <summary>
        /// 背景高度
        /// </summary>
        public float Height;
        
        public string DubbingId;

        public List<EntityVo> EntityList;

        /// <summary>
        /// 过场动画
        /// </summary>
        public CutScenesType CutScenesType;

        public ScreenEffectType ScreenEffectType;

        public EventVo Event;
        
        public DialogVo()
        {
            EntityList = new List<EntityVo>();
        }

        public DialogVo Clone()
        {
            DialogVo vo = new DialogVo();

            vo.BgImageId = BgImageId;
            vo.DialogId = DialogId;
            vo.BgMusicId = BgMusicId;
            vo.DubbingId = DubbingId;
            vo.CutScenesType = CutScenesType;
            vo.ScreenEffectType = ScreenEffectType;
            if(Event != null)
                vo.Event = Event.Clone();
            
            vo.EntityList = new List<EntityVo>();

            for (int i = 0; i < EntityList.Count; i++)
            {
                vo.EntityList.Add(EntityList[i].Clone());
            }

            return vo;
        }
    }

    public enum ScreenEffectType
    {
        [Description("无")]
        None,
        [Description("30%透明白色蒙版")]
        Percent30White
    }
    
    public enum TelphoneBgEffectType
    {
        [Description("无")]
        None,
        [Description("高斯模糊")]
        GaussianBlur
    }

    public enum CutScenesType
    {
        [Description("无")]
        None,
        [Description("从右到左（白）")]
        Right2LeftWhite,
        [Description("从右到左（黑）")]
        Right2LeftBlack,
        [Description("渐黑渐出")]
        FadeOutBlack,
        [Description("向左擦除渐黑渐出")]
        CutLeftFadeOutBlack,
        [Description("睡眼朦胧")]
        Awake,
        [Description("变模糊")]
        ToBlur,
        
//        [Description("淡入淡出（白）")]
//        FadeWhite,
//        [Description("淡入淡出（黑）")]
//        FadeBlack
    }

    public enum EventType
    {
        [Description("无")]
        None,
        [Description("电话")]
        Telephone,
        [Description("短信")]
        Sms,
        [Description("剧情")]
        Story,
        [Description("三个跳转选项")]
        Selection = 666
    }

    public class DescriptionAttribute : Attribute
    {
        private string m_strDescription;
        public DescriptionAttribute(string strPrinterName)
        {
            m_strDescription = strPrinterName;
        }

        public string Description
        {
            get { return m_strDescription; }
        }
    }
}