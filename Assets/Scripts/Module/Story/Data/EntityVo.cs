using System;
using System.Collections.Generic;
using UnityEngine;

namespace game.main
{
    [Serializable]
    public class EntityVo
    {
        public enum EntityType
        {
            Role,
            DialogFrame,
            Selection,
            Spine,
            Live2D
        }
        
        public enum DialogFrameStyle
        {
            [Description("旁白")]
            None,
            [Description("女主带头像")]
            Heroine,
            [Description("秦予哲")]
            Qinyuzhe,
            [Description("唐弋辰")]
            Tangyichen,
            [Description("迟郁")]
            Chiyu,
            [Description("言季")]
            Yanji,
            [Description("NPC")]
            Npc,
            [Description("女主无头像")]
            Heroine2,
        }

        public int index;
        
        public EntityType type;

        /// <summary>
        /// 对话框样式ID，或者人物图片ID
        /// </summary>
        public string id;

        public float color = 1.0f;

        public string dialog;
        public string roleName;
        public string headId;

        public float x;
        public float y;
        public float width;
        public float height;

        public List<string> SelectionIds;
        public List<string> SelectionContents;

        public List<SpinePlayableVo> playableList;
        
        public List<Live2dPlayableVo> l2dPlayableList;

        public List<float> L2dScaleDataList;

        /// <summary>
        /// 语音对嘴型
        /// </summary>
        public bool lipSpync;

        /// <summary>
        /// 对话框震动
        /// </summary>
        public bool shakeDialog;

        public virtual EntityVo Clone()
        {
            EntityVo vo = new EntityVo();
            vo.index = index;
            vo.dialog = dialog;
            vo.height = height;
            vo.id = id;
            vo.headId = headId;
            vo.roleName = roleName;
            vo.type = type;
            vo.width = width;
            vo.x = x;
            vo.y = y;
            vo.color = color;
            vo.lipSpync = lipSpync;
            vo.shakeDialog = shakeDialog;

            vo.SelectionIds = SelectionIds;
            vo.SelectionContents = SelectionContents;

            if (playableList != null)
            {
                List<SpinePlayableVo> list = new List<SpinePlayableVo>();
                for (int i = 0; i < playableList.Count; i++)
                {
                    list.Add(playableList[i].Clone());
                }
                vo.playableList = list;
            }

            if (l2dPlayableList != null)
            {
                List<Live2dPlayableVo> list = new List<Live2dPlayableVo>();
                for (int i = 0; i < l2dPlayableList.Count; i++)
                {
                    list.Add(l2dPlayableList[i].Clone());
                }
                vo.l2dPlayableList = l2dPlayableList;
            }

            vo.L2dScaleDataList = L2dScaleDataList;
            
            return vo;
        }
    }
}