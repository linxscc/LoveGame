using System;
using System.Collections.Generic;
using Com.Proto;
using Com.Proto.Server;
using DataModel;
using Google.Protobuf.Collections;
using UnityEngine;

namespace game.main
{
    public class UserFavorabilityVo
    {
        public int UserId;    //用户Id
        public PlayerPB Player;  //角色Id
        public int Level;    //等级
        public int Exp;        //经验
   
        public RepeatedField<string> VoiceKeep;     //语音收藏
        public int ShowExp;

         
        /// <summary>
        /// 用户服饰
        /// Key:DressUpTypePB 0是服装 1是背景
        /// Value 是道具Id
        /// </summary>
        public MapField<int, int> Apparel;  //用户服饰
        public UserFavorabilityVo(UserFavorabilityPB pb)
        {
            UserId = pb.UserId;
            Player = pb.Player;
            Level = pb.Level;
            if (pb.Level>1)
            {
                Exp = GlobalData.FavorabilityMainModel.GetCurExp(pb.Exp, pb.Level);
            }
            else
            {
                Exp = pb.Exp;  
            }

            Apparel = pb.Apparel;
      
            ShowExp = pb.Exp;         
            VoiceKeep = pb.VoiceKeep;
        }

    }
    
    
}