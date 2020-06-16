using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace game.main
{
    [Serializable]
    public class SpinePlayableVo : IComparable<SpinePlayableVo>
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
        /// 播放速度
        /// </summary>
        public float speed;
        
        /// <summary>
        /// 播放间隔时间
        /// </summary>
        public float interval;

        /// <summary>
        /// 播放完成后播放下一个序列的动画
        /// </summary>
        public bool playNextAnimation;
        
        /// <summary>
        /// 动画名称
        /// </summary>
        public string animationName;

        

        public SpinePlayableVo Clone()
        {
            SpinePlayableVo vo = new SpinePlayableVo();
            vo.playIndex = playIndex;
            vo.count = count;
            vo.delay = delay;
            vo.duration = duration;
            vo.animationName = animationName;
            vo.PlayableType = PlayableType;
            vo.playNextAnimation = playNextAnimation;
            vo.speed = speed;
            vo.interval = interval;
            
            return vo;
        }


        public int CompareTo(SpinePlayableVo other)
        {
            return playIndex.CompareTo(other.playIndex);
        }
    }

    
    public enum PlayableType
    {
        /// <summary>
        /// 根据持续时间播放
        /// </summary>
        ByTime,
        /// <summary>
        /// 根据次数播放
        /// </summary>
        ByCount
    }
}