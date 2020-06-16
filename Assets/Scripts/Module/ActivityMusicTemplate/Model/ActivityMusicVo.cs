using Com.Proto;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityMusicVo
{
    public int ActivityId;   //活动Id
    public int MusicId;   //活动Id
    public string MusicName;   //活动Id
    public MusicGameDiffTypePB Diff;   //难度
    public MusicGameScorePB GameScoreRule;//分数规则

    public MusicGameType MusicGameType = MusicGameType.Activity;
    
    public bool IsOpen;//是否开放

    public bool IsPass
    {
        get
        {
            if (_userActivityMusicInfoPB == null) 
            {
                return false;
            }

            if (_userActivityMusicInfoPB.MaxScore < GameScoreRule.CRank) 
            {
                return false;
            }
            return true;
        }
    }

    private UserActivityMusicInfoPB _userActivityMusicInfoPB;
    public ActivityMusicVo(UserActivityMusicInfoPB pb)
    {
        _userActivityMusicInfoPB = pb;
    }

}

public enum MusicGameType
{
    Activity,
    TrainingRoom,
    TrainingRoomPractice
}
