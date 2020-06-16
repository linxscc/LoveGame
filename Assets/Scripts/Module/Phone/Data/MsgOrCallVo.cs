//using System.Collections.Generic;
//using Com.Proto;
//using DataModel;

//namespace Assets.Scripts.Module.Phone.Data
//{
//    /// <summary>
//    /// 一条消息
//    /// </summary>
//    public class MsgOrCallVo
//    {
//        public string Sender;
//        public int SenderHead;
//        public string Content;
//        public long SendTime;
//        public int Type;
//        public string MusicId;
//        public float MusicLen;
//        public bool IsPlayed;//如果是语音判断是否播播放过
//        public int SceneId;
//        public int TalkId;
 
//        public string HeadPath
//        {
//            get
//            {
//                return MySmsOrCallVo.GetHeadPath(Type);
//            }
//        }

//        public List<int> selectIds;
//        public List<string> SelectContents; //选项文案（按钮文案）
        
//        public void SetData(MsgOrCallRulePB msgRule)
//        {
//            MusicLen= msgRule.MusicLen;
//            SceneId = msgRule.SceneId;
//            TalkId = msgRule.TalkId;
//            Content = msgRule.Content;
//            selectIds=new List<int>();
//            SelectContents = new List<string>();
//            MusicId = msgRule.MusicId;
//            selectIds.AddRange(msgRule.SelectIds);
//            SelectContents.AddRange(msgRule.SelectContents);
//            Sender = msgRule.Type == 0
//                ? GlobalData.PlayerModel.PlayerVo.UserName
//                : GlobalData.NpcModel.GetNpcById(msgRule.Type).NpcName;
//            Type = msgRule.Type;
//            IsPlayed = false;
//        }
//    }
//}