using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace game.main
{
    [Serializable]
    public class EventVo
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        public string EventId;

        /// <summary>
        /// 事件类型
        /// </summary>
        public EventType EventType;

        public List<EventType> SelectionTypes;
        
        public List<string> SelectionIds;
        
        public List<string> SelectionContents;

        public EventVo Clone()
        {
            EventVo vo = new EventVo();
            vo.EventType = EventType;
            vo.EventId = EventId;
            vo.SelectionIds = SelectionIds;
            vo.SelectionContents = SelectionContents;
            vo.SelectionTypes = SelectionTypes;
            
            return vo;
        }
    }
}