using System;
using System.Collections.Generic;
using Com.Proto;
using Google.Protobuf.Collections;
using UnityEngine;

namespace DataModel
{
    public class NpcModel
    {
        public List<NpcPB> NpcList;
        public List<UserNpcPB> UserNpcPbList;

        public void InitList(NpcRes res)
        {
            NpcList = new List<NpcPB>();
            NpcList.AddRange(res.Npcs);
        }

        public void InitUserNpc(UserNpcRes res)
        {
            UserNpcPbList=new List<UserNpcPB>();
            UserNpcPbList.AddRange(res.UserNpcs);
//            foreach (var v in res.UserNpcs)
//            {
//                UserNpcPbList.Add(v);
//            }
        }

        public int GetNpcDialyInteractCnt()
        {
            int n = 0;
            if (UserNpcPbList == null)
                return -1;
            for (int i = 0; i < UserNpcPbList.Count; i++) 
            {
                n += UserNpcPbList[i].DialyInteractCnt;
            }
            return n;
        }
        public void UpdateUserNpcPB(RepeatedField<UserNpcPB> userNpcs)
        {
            for (int i = 0; i < userNpcs.Count; i++) 
            {
                var temp= UserNpcPbList.Find((m) => { return m.Player == userNpcs[i].Player; });
                if (temp == null)
                {
                    UserNpcPbList.Add(userNpcs[i]);
                }
                else
                {
                    temp.DialyInteractCnt = userNpcs[i].DialyInteractCnt;
                }
            }
        }

        public NpcPB GetNpcById(int id)
        {
            return NpcList.Find((item) => { return item.NpcId == id; });
        }

        //public void Get(UserSetNpcBgStateRes res)
        //{
        //    if (UserNpcPbList==null)
        //    {
        //        UserNpcPbList=new List<UserPB>();
        //    }
        //    Debug.LogError(UserNpcPbList.Count);
        //    UserNpcPbList.Clear();
        //    //UserNpcPbList.AddRange(res.User);

        //}

        public UserNpcPB GetBgNpc()
        {
            if (UserNpcPbList != null)
            {
                foreach (var v in UserNpcPbList)
                {
//                    if (v.UseState == 1)
//                    {
//                        return v;
//                    }
                }
            }

            return null;
        }

        public string GetAnimationState(int state)
        {
            switch (state)
            {
                case    1:
                    return "01_dai_ji";
                case    2:
                    return "02_zou_lu";
                case    3:
                    return "03_hui_shou";
                default:                    
                    return "01_dai_ji";                
            }                        
        }


    }
}