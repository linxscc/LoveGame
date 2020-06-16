using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace game.main
{
    [Serializable]
    public class Live2dPlayableVo : IComparable<Live2dPlayableVo>
    {
        public PlayableType PlayableType = PlayableType.ByTime;

        /// <summary>
        /// 播放序列
        /// </summary>
        public int playIndex;
        
        /// <summary>
        /// 播放持续时间
        /// </summary>
        public float duration;

        /// <summary>
        /// 播放次数
        /// </summary>
        public int count;

        /// <summary>
        /// 播放延迟时间
        /// </summary>
        public float delay;
        
        /// <summary>
        /// 播放间隔时间
        /// </summary>
        public float interval;

        /// <summary>
        /// 播放完成后播放下一个序列的动画
        /// </summary>
        public bool playNextAnimation;

        /// <summary>
        /// 动作名称
        /// </summary>
        public string motionName;
        
        /// <summary>
        /// 表情名称
        /// </summary>
        public string expressionName;

        public Live2dPlayableVo Clone()
        {
            Live2dPlayableVo vo = new Live2dPlayableVo();
            vo.playIndex = playIndex;
            vo.count = count;
            vo.delay = delay;
            vo.duration = duration;
            vo.motionName = motionName;
            vo.expressionName = expressionName;
            vo.PlayableType = PlayableType;
            vo.playNextAnimation = playNextAnimation;
            vo.interval = interval;
            
            return null;
        }

        public int CompareTo(Live2dPlayableVo other)
        {
            return playIndex.CompareTo(other.playIndex);
        }
        
        public enum Live2dPlayType
        {
            Motion,
            EyeBlink,
            MontionLess
        }
    }
}