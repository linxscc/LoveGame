using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;

namespace DataModel
{
    public class FriendModel : Model
    {
        public List<FriendInfo> FriendMainInfoList;

        //感觉还是要包一层类才行！
        public RepeatedField<FriendBaseInfo> FriendCommentList;
        public RepeatedField<FriendBaseInfo> FriendApplyList;

        public FriendBaseInfo SearchFriendInfo;
        public int DailyPower = 0;
        public long RefreshTime = 0;

        public void Init(UserFriendsRes res)
        {
            DailyPower = res.DailyPower;
            if (FriendMainInfoList == null)
            {
                FriendMainInfoList = new List<FriendInfo>();
            }

            foreach (var v in res.UserFriends)
            {
                var friendInfo = new FriendInfo(v);
                FriendMainInfoList.Add(friendInfo);
            }
        }

        public void InitCommentList(CommentsRes res)
        {
            FriendCommentList = res.FriendBases;
            RefreshTime = res.RefreshTime;
        }

        public void UpdateCommentList(RepeatedField<FriendBaseInfo> baseInfos)
        {
            FriendCommentList.Clear();
            foreach (var v in baseInfos)
            {
                FriendCommentList.Add(v); 
            }
            
        }
        
        public void InitApplyList(ApplysRes res)
        {
//            Debug.LogError("" + res.FriendBases);
            FriendApplyList = res.FriendBases;
        }

        //申请后的操作,删掉一个item
        public void UpdateCommentList(int friendId)
        {
            FriendBaseInfo target = null;
            foreach (var v in FriendCommentList)
            {
                if (v.UserId == friendId)
                {
                    Debug.LogError(v);
                    target = v;
                }
            }

            if (target != null)
            {
                FriendCommentList.Remove(target);
            }
        }

        public void UpdateApplyList(int friendId)
        {
            FriendBaseInfo target = null;
            foreach (var v in FriendApplyList)
            {
                if (v.UserId == friendId)
                {
                    target = v;
                }
            }

            FriendApplyList.Remove(target);

            GlobalData.DepartmentData.CanGetFriendsPower = FriendApplyList.Count > 0;
        }

        public void AddFriend(FriendDetailPB pb)
        {
            var friendInfo = new FriendInfo(pb);
            if (!FriendMainInfoList.Contains(friendInfo))
            {
                FriendMainInfoList.Add(friendInfo);
            }
        }

        public void UpdateFriendInfo(UserFriendPB pb)
        {
            foreach (var v in FriendMainInfoList)
            {
                if (v.UserId == pb.UserId)
                {
                    v.IsGetPower = pb.PowerGetState;
                    v.IsGivePower = pb.PowerSendState;
                    v.UpdateFriendPro();
                }
            }
        }

        public void UpdateFriendInfo(RepeatedField<UserFriendPB> pbs)
        {
            foreach (var v in pbs)
            {
                foreach (var a in FriendMainInfoList)
                {
                    if (a.UserId == v.UserId)
                    {
                        a.IsGetPower = v.PowerGetState;
                        a.IsGivePower = v.PowerSendState;
                        a.UpdateFriendPro();
                    }
                }
            }
        }

        public void DeleteFriend(UserFriendPB pb)
        {
            FriendInfo friendInfo = null;
            Debug.LogError(pb.UserId);
            foreach (var v in FriendMainInfoList)
            {
                Debug.LogError("v" + v.UserId);
                if (v.UserId == pb.UserId)
                {
                    friendInfo = v;
                }
            }


            if (FriendMainInfoList.Contains(friendInfo))
            {
                Debug.LogError("Remove" + friendInfo?.UserName);
                FriendMainInfoList.Remove(friendInfo);
            }
        }

        public void RefreshFriendList(RepeatedField<FriendDetailPB> pbs)
        {
            FriendMainInfoList.Clear();
            foreach (var v in pbs)
            {
                var friendInfo = new FriendInfo(v);
                FriendMainInfoList.Add(friendInfo);
            }
        }

        public RepeatedField<int> GetPowerSendFriend()
        {
            RepeatedField<int> friendpowerlist = new RepeatedField<int>();
            int count = 0;

            int max = GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.FRIEND_DAILY_MAX_POWER_ID);
            foreach (var v in FriendMainInfoList)
            {
                
                if (v.IsBeGivenPower==1&&v.IsGetPower == 0)
                {
                    if (count < max)
                    {
                        friendpowerlist.Add(v.UserId);
                        count++;
                    }
                }
            }

            return friendpowerlist;
        }

        public RepeatedField<int> SendAllPowerFriend()
        {
            RepeatedField<int> friendpowerlist = new RepeatedField<int>();
            int count = 0;
            foreach (var v in FriendMainInfoList)
            {
                
                if (v.IsGivePower==0)
                {
                    friendpowerlist.Add(v.UserId);
                    count++;
                }
            }

            return friendpowerlist;
        }
        
    }
}