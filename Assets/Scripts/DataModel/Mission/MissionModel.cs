using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Framework.Utils;
using Com.Proto;
using DataModel;
using game.main;
using Google.Protobuf.Collections;
using UnityEngine;


public class MissionModel : Model
{
    //任务数据模型结构

    private MissionRuleRes _missionRuleRes;
    private UserMissionsRes _userMissionsRes;

    public List<UserMissionVo> UserMissionList;
    private Dictionary<int, MissionRulePB> _missionBaseDataDict;
    public Dictionary<PlayerPB, UserMissionActivityInfoPB> StarCourseSchedule;


    private List<UserMissionPB> _userMissionList;
    private List<MissionActivityRewardRulePB> MissionActivityRewardRulePbs;
    private List<MissionActivityRewardRulePB> WeekActivityRewardRulePbs;
    private List<MissionActivityRewardRulePB> StarRoadRewardRulePbs;
    private List<MissionActivityRewardRulePB> _starActivityRewardRulePbs; //星动之约活动任务奖励规则
    private List<MissionActivityRewardRulePB> _playerBirthdayRulePbs; //生日活动奖励规则
    private List<MissionActivityRewardRulePB> _newStarActivityRewardRulePbs; //新星动之约任务奖励规则 


    public UserMissionInfoPB UserMissionInfoPb;


    private Dictionary<PlayerPB, List<long>> _playerBirthdayTimeStamp; //四个角色生日任务起始时间，List[0]是开始时间，List[1]是结束时间



    public UserMissionActivityInfoPB DailyMissionActivityInfoPb;
    public UserMissionActivityInfoPB WeekMissionActivityInfoPb;
    public UserMissionActivityInfoPB AchievementMissionActivityInfoPb;

    public UserMissionActivityInfoPB StarActivityInfoPb; //星动之约用户pb
    public UserMissionActivityInfoPB PlayerBirthdayInfoPb; //玩家生日用户pb


    private Dictionary<int, List<UserMissionVo>> _starActivityMissions;
    private Dictionary<int, List<UserMissionVo>> _playerBirthdayMissions;

    private List<long> _starActivityTimePoint;
    private List<int> _redDotDays;



    private List<int> _birthdayDotDays;

    public bool IsHaveStarActivityMission = false;
    public bool IsHavePlayerBirthdayMission = false; //是否有生日任务

    public int Day; //星动之约开放天数
    public int StarActivityNum; //星动之约任务总数


    public int PlayerBirthdayMissionsDay; //生日任务开放天数
    public int PlayerBirthdayMissionsNum; //生日任务总数
    private PlayerPB _curPlayerBirthday;

    public MissionAttainmentModel MissionAttainmentModel;


   



    public void InitBaseData(MissionRuleRes missionRuleRes)
    {
       
        if (missionRuleRes != null)
        {
            _missionRuleRes = missionRuleRes;
            _missionBaseDataDict = new Dictionary<int, MissionRulePB>();
            foreach (var pb in _missionRuleRes.MissionRules)
            {
                _missionBaseDataDict[pb.MissionId] = pb;
            }

            MissionActivityRewardRulePbs = new List<MissionActivityRewardRulePB>();
            WeekActivityRewardRulePbs = new List<MissionActivityRewardRulePB>();
            StarRoadRewardRulePbs = new List<MissionActivityRewardRulePB>();
            _starActivityRewardRulePbs = new List<MissionActivityRewardRulePB>();
            _playerBirthdayRulePbs = new List<MissionActivityRewardRulePB>();
             _newStarActivityRewardRulePbs = new List<MissionActivityRewardRulePB>();
            foreach (var v in missionRuleRes.ActivityRewards)
            {
                if (v.MissionType == MissionTypePB.Daily)
                {
                    MissionActivityRewardRulePbs.Add(v);
                }
                else if (v.MissionType == MissionTypePB.WeekDaily)
                {
                    WeekActivityRewardRulePbs.Add(v);
                }
                else if (v.MissionType == MissionTypePB.StarCourse)
                {
                   // Debug.LogError(v);
                    StarRoadRewardRulePbs.Add(v);
                }
                else if (v.MissionType == MissionTypePB.StarryCovenant)
                {
                    _starActivityRewardRulePbs.Add(v);
                }
                else if (v.MissionType == MissionTypePB.NewStarryCovenant)
                {
                    _newStarActivityRewardRulePbs.Add(v);
                }
                else if (v.MissionType == MissionTypePB.ChiYuMission)
                {
                    _playerBirthdayRulePbs.Add(v);
                    _curPlayerBirthday = PlayerPB.ChiYu;
                }
            }

            StarCourseSchedule = new Dictionary<PlayerPB, UserMissionActivityInfoPB>();          
        }
    }


    public void InitUserData(UserMissionsRes userMissionsRes)
    {
      
        if (userMissionsRes != null)
        {
            _userMissionsRes = userMissionsRes;
            _userMissionList = _userMissionsRes.UserMissions.ToList();

            UserMissionList = new List<UserMissionVo>();
            for (int i = 0; i < _userMissionList.Count; i++)
            {
                if (_userMissionList[i].MissionType == MissionTypePB.StarryCovenant ||
                    _userMissionList[i].MissionType == MissionTypePB.NewStarryCovenant
                    && IsHaveStarActivityMission == false)
                {
                    IsHaveStarActivityMission = true;
                }
                else if (_userMissionList[i].MissionType == MissionTypePB.ChiYuMission &&
                         IsHavePlayerBirthdayMission == false)
                {
                    IsHavePlayerBirthdayMission = true;
                }

                //fix 只添加有规则的数据！
                if (_missionBaseDataDict.ContainsKey(_userMissionList[i].MissionId))
                {
                    UserMissionList.Add(ParseUserMission(_userMissionList[i]));
                }

            }

            UpdateUserMissionInfo(userMissionsRes.UserMissionInfo);
        }



        if (IsHaveStarActivityMission)
        {
            InitStarActivityTimePoint();
            InitStarActivityMission();
            MissionAttainmentModel = new MissionAttainmentModel();
        }

        if (IsHavePlayerBirthdayMission)
        {
            InitPlayerBirthdays();
            InitPlayerBirthdayMission();
        }

    }


    /// <summary>
    /// 初始化生日任务时间戳
    /// </summary>
    private void InitPlayerBirthdays()
    {
        _playerBirthdayTimeStamp = new Dictionary<PlayerPB, List<long>>();

        foreach (var t in _missionRuleRes.PlayerBirthdayRulePB)
        {
            if (!_playerBirthdayTimeStamp.ContainsKey((PlayerPB) t.Player))
            {
                _playerBirthdayTimeStamp[(PlayerPB) t.Player] = new List<long>();
                var allDays = GetPlayerBirthdayAllDay();
                var starDateTime = new DateTime(DateUtil.GetDataTime(t.BirthdayDate).Year,
                    DateUtil.GetDataTime(t.BirthdayDate).Month, DateUtil.GetDataTime(t.BirthdayDate).Day, 6, 0, 0);
                for (int i = 0; i <= allDays; i++)
                {
                    long timeStamp = 0;

                    if (i == 0)
                    {
                        timeStamp = DateUtil.GetNotTimezoneTimeStamp(starDateTime);
                    }
                    else
                    {
                        timeStamp = DateUtil.GetNotTimezoneTimeStamp(starDateTime.AddDays(i));
                    }

                    //timeStamp = DateUtil.GetTimeStamp(i==0 ? starDateTime : starDateTime.AddDays(i));
                    _playerBirthdayTimeStamp[(PlayerPB) t.Player].Add(timeStamp);
                }
            }
        }

    }


    public MissionRuleRes GetMissionRuleRes()
    {
        return _missionRuleRes;
    }

  

    /// <summary>
    /// 初始化迟郁的生日任务数据
    /// </summary>
    private void InitPlayerBirthdayMission()
    {
        Debug.LogError("初始化迟郁的生日活动数据");
        _playerBirthdayMissions = new Dictionary<int, List<UserMissionVo>>();
        PlayerBirthdayMissionsNum = 0;
        foreach (var t in UserMissionList)
        {
            if (t.MissionType == MissionTypePB.ChiYuMission)
            {
                PlayerBirthdayMissionsNum++;
                var day = _missionBaseDataDict[t.MissionId].Extra.Days;
                var isKey = _playerBirthdayMissions.ContainsKey(day);
                if (!isKey)
                {
                    _playerBirthdayMissions[day] = new List<UserMissionVo>();
                }

                if (!_playerBirthdayMissions[day].Contains(t))
                {
                    _playerBirthdayMissions[day].Add(t);
                }
            }
        }

        Debug.LogError("生日任务总数===>" + PlayerBirthdayMissionsNum);
    }


    /// <summary>
    /// 初始化星动之约任务数据
    /// </summary>
    private void InitStarActivityMission()
    {
        Debug.LogError("初始化星动之约活动数据");
        _starActivityMissions = new Dictionary<int, List<UserMissionVo>>();
        StarActivityNum = 0;
      
        var isNew = IsNewStarActivity();
        Debug.LogError("isNew===>"+isNew);
        
        MissionTypePB type = isNew ? MissionTypePB.NewStarryCovenant : MissionTypePB.StarryCovenant;

        foreach (var t in UserMissionList)
        {
            if (t.MissionType == type)
            {
                StarActivityNum++;
                var day = _missionBaseDataDict[t.MissionId].Extra.Days;
                var isKey = _starActivityMissions.ContainsKey(day);
                if (isKey == false)
                {
                    _starActivityMissions[day] = new List<UserMissionVo>();
                }

                if (!_starActivityMissions[day].Contains(t))
                {
                    _starActivityMissions[day].Add(t);
                }
            }
        }

        Debug.LogError("星动之约任务总数===>" + StarActivityNum);
    }


    /// <summary>
    /// 是否是新星动之约
    /// 是拿创建号的时间来做
    /// </summary>
    /// <returns></returns>
    public bool IsNewStarActivity()
    {
        var timePoint = GlobalData.ConfigModel.GetGameTimeConfigByKey(GameConfigKey.NEW_STARRY_COVENANT_DECISION_TIME);
        var createTime = GlobalData.PlayerModel.PlayerVo.CreateTime;    
        return createTime >= timePoint;
    }

    /// <summary>
    /// 初始化星动之约活动时间点
    /// </summary>
    private void InitStarActivityTimePoint()
    {               
        var userStarDt = DateUtil.GetDataTime(UserMissionInfoPb.SevenDcCreateTime);
        var userZeroDt = new DateTime(userStarDt.Year, userStarDt.Month, userStarDt.Day, 0, 0, 0);
        var userSixDt = new DateTime(userStarDt.Year, userStarDt.Month, userStarDt.Day, 6, 0, 0);

        var zeroPointTimeStamp = DateUtil.GetNotTimezoneTimeStamp(userZeroDt);
        var sixPointTimeStamp = DateUtil.GetNotTimezoneTimeStamp(userSixDt);

        var lastDayDt = userStarDt.AddDays(-1);
        var lastDaySixDt = new DateTime(lastDayDt.Year, lastDayDt.Month, lastDayDt.Day, 6, 0, 0);

        var isLastDay = UserMissionInfoPb.SevenDcCreateTime >= zeroPointTimeStamp && UserMissionInfoPb.SevenDcCreateTime <= sixPointTimeStamp;
    
        var allDays = GetStarActivityAllDay();
            
        var starTimeStamp = isLastDay ? lastDaySixDt :userSixDt;
     
        if (_starActivityTimePoint==null)
        {
           _starActivityTimePoint =new List<long>();

           for (int i = 0; i <=allDays; i++)
           {
               _starActivityTimePoint.Add(i == 0? DateUtil.GetNotTimezoneTimeStamp(starTimeStamp)
                   : DateUtil.GetNotTimezoneTimeStamp(starTimeStamp.AddDays(i)));                   
           }                
        }

    }


    /// <summary>
    /// 获取星动之约时间点结合
    /// </summary>
    /// <returns></returns>
    public List<long> GetStarActivityRefreshTimePintList()
    {
        return _starActivityTimePoint;
    }


    /// <summary>
    /// 获取星动之约结束时间戳
    /// </summary>
    /// <returns></returns>
    public long GetStarActivityOverTimeStamp()
    {
        return _starActivityTimePoint[_starActivityTimePoint.Count - 1];
    }

    /// <summary>
    /// 获取星动之约活动最大天数
    /// </summary>
    /// <returns></returns>
    public int GetStarActivityAllDay()
    {
        return GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.SEVEN_DAYS_CARNIVAL_OPEN_DAYS);
    }


    /// <summary>
    /// 获取星动之约开放的天数
    /// </summary>
    /// <returns></returns>
    public int GetOpenDay()
    {
        int day = 0;
        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();

        for (int i = 0; i < _starActivityTimePoint.Count; i++)
        {
            if (curTimeStamp < _starActivityTimePoint[i])
            {
                day = i;
                break;
            }
        }

        return day;
    }

    /// <summary>
    /// 是否可以预览星动之约的任务
    /// </summary>
    /// <returns></returns>
    public bool IsPreviewStarActivity()
    {
        return GetOpenDay() + 1 <= _starActivityMissions.Count;
    }


    /// <summary>
    /// 获取星动之约对应天数的任务
    /// </summary>
    /// <param name="day">天</param>
    /// <returns></returns>
    public List<UserMissionVo> GetStarActivityMission(int day)
    {
        Debug.LogError("获取任务天数day===>" + day);

        if (IsPreviewStarActivity())
        {
            if (day == GetOpenDay() + 1)
            {
                foreach (var t in _starActivityMissions[day])
                {
                    t.IsPreview = true;
                }
            }
        }

        _starActivityMissions[day].Sort();
        return _starActivityMissions[day];
    }

    /// <summary>
    /// 获取生日任务时间点结合
    /// </summary>
    /// <returns></returns>
    public List<long> GetPlayerBirthdayRefreshTimePointList()
    {
        return _playerBirthdayTimeStamp[_curPlayerBirthday];
    }

    /// <summary>
    /// 获取生日任务结束时间戳
    /// </summary>
    /// <returns></returns>
    public long GetPlayerBirthdayOverTimeStamp()
    {
        var list = _playerBirthdayTimeStamp[_curPlayerBirthday];
        return list[list.Count - 1];
    }

    /// <summary>
    /// 获取生日活动最大天数
    /// </summary>
    /// <returns></returns>
    public int GetPlayerBirthdayAllDay()
    {
        return GlobalData.ConfigModel.GetConfigByKey(GameConfigKey.PLAYER_BIRTHDAY_TIME);
    }

    /// <summary>
    /// 获取生日任务对于天数的任务
    /// </summary>
    /// <param name="day"></param>
    /// <returns></returns>
    public List<UserMissionVo> GetPlayerBirthdayMission(int day)
    {
        _playerBirthdayMissions[day].Sort();
        return _playerBirthdayMissions[day];
    }

    /// <summary>
    /// 获取生日开放的天数
    /// </summary>
    /// <returns></returns>
    public int GetPlayerBirthdayOpenDay()
    {
        int day = 0;
        var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();

        var list = _playerBirthdayTimeStamp[_curPlayerBirthday];

        for (int i = 0; i < list.Count; i++)
        {
            if (curTimeStamp < list[i])
            {
                day = i;

                break;
            }
        }

        return day;
    }

    public UserMissionVo GetUserMissionDataByMissionId(int missionId)
    {
        UserMissionVo vo = null;
        foreach (var data in UserMissionList)
        {
            if (missionId == data.MissionId)
            {
                vo = data;
                break;
            }
        }

        return vo;
    }


    /// <summary>
    /// 获取生日任务红点天数
    /// </summary>
    /// <returns></returns>
    public List<int> GetPlayerBirthdayRedDotDays()
    {
        _redDotDays = new List<int>();
        var count = _playerBirthdayMissions.Count;
        int day = GetPlayerBirthdayOpenDay();
        if (day >= count)
        {
            day = count;
        }

        for (int i = 1; i <= day; i++)
        {
            var list = _playerBirthdayMissions[i];
            foreach (var t in list)
            {
                if (t.Status == MissionStatusPB.StatusUnclaimed)
                {
                    _redDotDays.Add(i);
                    break;
                }
            }
        }

        return _redDotDays;

    }

    /// <summary>
    /// 获取星动之约红点天数
    /// </summary>
    /// <returns></returns>
    public List<int> GetRedDotDays()
    {
        _redDotDays = new List<int>();

        var count = _starActivityMissions.Count;
        int day = GetOpenDay();
        if (day >= count)
        {
            day = count;
        }

        for (int i = 1; i <= day; i++)
        {
            var list = _starActivityMissions[i];
            foreach (var t in list)
            {
                if (t.Status == MissionStatusPB.StatusUnclaimed)
                {
                    _redDotDays.Add(i);
                    break;
                }
            }
        }

        return _redDotDays;
    }






    public void UpdateUserMissionInfo(UserMissionInfoPB pb)
    {

        if (pb.ActivityInfos.Count == 0)
        {
            Debug.LogError("pb.ActivityInfos.Count=0");
            return;
        }



        UserMissionInfoPb = pb;

        foreach (var v in pb.ActivityInfos)
        {
            switch (v.MissionType)
            {
                case MissionTypePB.Daily:
                    DailyMissionActivityInfoPb = v;
                    break;
                case MissionTypePB.WeekDaily:
                    WeekMissionActivityInfoPb = v;
                    break;
                case MissionTypePB.StarCourse:
                       // Debug.LogError(v);
                    if (StarCourseSchedule.ContainsKey(v.Player))
                    {
                        StarCourseSchedule[v.Player] = v;
                    }
                    else
                    {
                        StarCourseSchedule.Add(v.Player, v);
                    }

                    break;
                case MissionTypePB.StarryCovenant:
                case MissionTypePB.NewStarryCovenant:
                    StarActivityInfoPb = v;

                    break;
                case MissionTypePB.ChiYuMission:
                    PlayerBirthdayInfoPb = v;
                    break;

            }
        }

    }


    private UserMissionVo ParseUserMission(UserMissionPB userMissionPb)
    {
        UserMissionVo vo = new UserMissionVo();
        vo.InitData(userMissionPb);

        return vo;
    }


    /// <summary>
    /// 整体刷新星动之约任务数据
    /// </summary>
    /// <param name="pbs"></param>
    public void UpdateStarActivityMission(RepeatedField<UserMissionPB> pbs)
    {
        if (pbs.Count > 0)
        {
            foreach (var v in pbs)
            {
                foreach (var a in UserMissionList)
                {
                    if (v.MissionId == a.MissionId)
                    {
                        a.InitData(v);
                    }
                }
            }
        }

        InitStarActivityMission();
    }


    /// <summary>
    /// 整体刷新迟郁的生日数据
    /// </summary>
    /// <param name="pbs"></param>
    public void UpdatePlayerBirthdayMission(RepeatedField<UserMissionPB> pbs)
    {
        if (pbs.Count > 0)
        {
            foreach (var v in pbs)
            {
                foreach (var a in UserMissionList)
                {
                    if (v.MissionId == a.MissionId)
                    {
                        a.InitData(v);
                    }
                }
            }
        }

        InitPlayerBirthdayMission();
    }

    public void UpdateDailyMission(RepeatedField<UserMissionPB> pbs)
    {
        //UserMissionList.Clear();
//            Debug.LogError("Update"+pbs.Count);
//            for (int i = 0; i < pbs.Count; i++)
//            {
//                Debug.LogError("Update");
//                UserMissionList.Add(ParseUserMission(pbs[i]));
//            }
        if (pbs.Count > 0)
        {
            foreach (var v in pbs)
            {
                foreach (var a in UserMissionList)
                {
                    if (v.MissionId == a.MissionId)
                    {
                        a.InitData(v);
                    }
                }
            }


        }


    }


    public List<UserMissionVo> GetMissionListByType(MissionTypePB type)
    {
        List<UserMissionVo> TargetTask = new List<UserMissionVo>();
        foreach (var task in UserMissionList)
        {
            if (type == task.MissionType)
            {
                TargetTask.Add(task);
            }
        }

        return TargetTask;
    }

    /// <summary>
    /// 判断是否有日常活跃奖励红点
    /// </summary>
    /// <returns></returns>
    public bool HasDailyActivityAward(List<UserMissionVo> userMissionVos)
    {
        bool hasUnclaimed = false;

        //首先要判断是否有任务完成的奖励未领取
        foreach (var vo in userMissionVos)
        {
            if (vo.Status == MissionStatusPB.StatusUnclaimed)
            {
                hasUnclaimed = true;
                break;
            }
        }

        bool hasActawrad = false;

        for (int i = 0; i < 5; i++)
        {
            int weight = GetReceiveWeight(i);
            if (DailyMissionActivityInfoPb.List.Contains(weight))
            {
                hasActawrad = false;
            }
            else
            {
                //活跃度不到
                if (weight > DailyMissionActivityInfoPb.Progress)
                {
                    hasActawrad = false;
                }
                else
                {
                    hasActawrad = true;
                    break;
                }
            }

        }

        bool result = hasUnclaimed || hasActawrad; //满足其中一个就要返回红点

        return result;
    }

    /// <summary>
    /// 判断是否有周任务奖励红点
    /// </summary>
    /// <param name="userMissionVos"></param>
    /// <returns></returns>
    public bool HasWeekActivityAward(List<UserMissionVo> userMissionVos)
    {
        bool hasUnclaimed = false;

        //首先要判断是否有任务完成的奖励未领取
        foreach (var vo in userMissionVos)
        {
            if (vo.Status == MissionStatusPB.StatusUnclaimed)
            {
                hasUnclaimed = true;
                break;
            }
        }

        bool hasActawrad = false;

        if (WeekMissionActivityInfoPb != null)
        {
            for (int i = 0; i < 5; i++)
            {
                int weight = GetReceiveWeight(i);
                if (WeekMissionActivityInfoPb.List.Contains(weight))
                {
                    hasActawrad = false;
                }
                else
                {
                    //活跃度不到
                    if (weight > WeekMissionActivityInfoPb.Progress)
                    {
                        hasActawrad = false;
                    }
                    else
                    {
                        hasActawrad = true;
                        break;
                    }
                }

            }
        }

        bool result = hasUnclaimed || hasActawrad; //满足其中一个就要返回红点

        return result;
    }



    public int GetHasFinishTaskCount()
    {
        int i = 0;
        foreach (var v in UserMissionList)
        {
            if (v.Status == MissionStatusPB.StatusBeRewardedWith)
            {
                i++;
            }
        }

        return i;
    }

    public List<UserMissionVo> GetMissionByPlayerPB(PlayerPB pb)
    {
        List<UserMissionVo> targetvo = new List<UserMissionVo>();
        foreach (var v in UserMissionList)
        {
            if (_missionBaseDataDict.ContainsKey(v.MissionId))
            {
                var _missionitem = _missionBaseDataDict[v.MissionId];
                if (_missionitem.Player == pb && _missionitem.MissionType == MissionTypePB.StarCourse)
                {
                    targetvo.Add(v);
                }
            }
        }

        return targetvo;

    }



    public bool HasReceiveChievement(PlayerPB pb)
    {
        foreach (var v in UserMissionList)
        {
            if (_missionBaseDataDict.ContainsKey(v.MissionId))
            {
                var _missionitem = _missionBaseDataDict[v.MissionId];
                if (_missionitem.Player == pb && _missionitem.MissionType == MissionTypePB.StarCourse)
                {
                    if (v.MissionPro == 0)
                    {
                        return true;
                    }

                }
            }
        }

        //判断是否有星路旅程阶段奖励
        if (StarCourseSchedule.ContainsKey(pb))
        {
            var usermissioninfo = StarCourseSchedule[pb];
            int weight = 0;
            GetStarRoadRewardPBByCount(usermissioninfo.Progress, pb, ref weight, usermissioninfo.List);
//                Debug.LogError(usermissioninfo?.Progress+" "+weight);
            if (usermissioninfo?.Progress >= weight)
            {
                return true;
            }

        }


        return false;

    }


    public MissionRulePB GetMissionById(int missionId)
    {
        if (!_missionBaseDataDict.ContainsKey(missionId))
        {
            Debug.LogError("missionDict didn't have missionId= " + missionId);
            return null;
        }

        return _missionBaseDataDict[missionId];
    }



    public List<MissionRulePB> GetMissionRulePBListByType(MissionTypePB type)
    {
        List<MissionRulePB> resRule = new List<MissionRulePB>();
        foreach (var v in _missionBaseDataDict)
        {
            if (type == v.Value.MissionType)
            {
                resRule.Add(v.Value);
            }
        }

        return resRule;
    }



    public void UpdateUserMission(UserMissionPB vo)
    {
        if (UserMissionList != null)
        {
            foreach (var v in UserMissionList)
            {
                if (v.MissionId == vo.MissionId)
                {
//                        Debug.LogError(vo);
                    v.Progress = vo.Progress;
                    v.Finish = vo.Finish;
                    v.Status = vo.Status;
                    v.MissionType = vo.MissionType;

                    //需要更新权重！
                    v.UpdateMissionPro(vo.Status);
                }
            }
        }
    }

    /// <summary>
    /// 更新星动之约任务数据
    /// </summary>
    /// <param name="vo"></param>
    /// <param name="day"></param>
    public void UpdateUserStarActivityMission(UserMissionPB vo, int day)
    {
        var list = GetStarActivityMission(day);
        if (list != null)
        {
            foreach (var v in list)
            {
                if (v.MissionId == vo.MissionId)
                {
                    v.Progress = vo.Progress;
                    v.Finish = vo.Finish;
                    v.Status = vo.Status;
                    v.MissionType = vo.MissionType;
                    v.UpdateMissionPro(vo.Status);
                }
            }
        }
    }



    public void UpdateUserPlayerBirthdayMission(UserMissionPB vo, int day)
    {
        var list = GetPlayerBirthdayMission(day);
        if (list != null)
        {
            foreach (var v in list)
            {
                if (v.MissionId == vo.MissionId)
                {
                    v.Progress = vo.Progress;
                    v.Finish = vo.Finish;
                    v.Status = vo.Status;
                    v.MissionType = vo.MissionType;
                    v.UpdateMissionPro(vo.Status);
                }
            }
        }
    }

    public void ReplaceUserMission(UserMissionPB vo, int replaceId)
    {
        if (UserMissionList != null)
        {
            foreach (var v in UserMissionList)
            {
                if (v.MissionId == replaceId)
                {
//                        Debug.LogError(vo);
                    v.MissionId = vo.MissionId;
                    v.Progress = vo.Progress;
                    v.Finish = vo.Finish;
                    v.Status = vo.Status;
                    v.MissionType = vo.MissionType;

                    //需要更新权重！
                    v.UpdateMissionPro(vo.Status);
                }
            }
        }
    }




    public int GetReceiveWeight(int index)
    {
        switch (index)
        {
            case 0:
                return 20;
            case 1:
                return 40;
            case 2:
                return 60;
            case 3:
                return 80;
            case 4:
                return 100;
            default:
                Debug.LogError(index);
                return 40;
        }

    }


    public int GetStarReceiveWeight(int index)
    {
        var isNew = IsNewStarActivity();
        return isNew ? _newStarActivityRewardRulePbs[index].Weight : _starActivityRewardRulePbs[index].Weight;
    }



    public int GetPlayerBirthdayWeight(int index)
    {
        return _playerBirthdayRulePbs[index].Weight;
    }

    public RepeatedField<AwardPB> GetMissionawardByWeight(int count)
    {
        foreach (var v in MissionActivityRewardRulePbs)
        {
            if (v.Weight == count)
            {
                return v.Awards;
            }
        }

        Debug.LogError("No Such PB");
        return null;
    }

    public string GetPlayerName(PlayerPB playeridx)
    {
        string cardName = "";
        switch (playeridx)
        {
            case PlayerPB.TangYiChen:
                cardName = I18NManager.Get("Common_Role1");
                break;
            case PlayerPB.QinYuZhe:
                cardName = I18NManager.Get("Common_Role2");
                break;
            case PlayerPB.YanJi:
                cardName = I18NManager.Get("Common_Role3");
                break;
            case PlayerPB.ChiYu:
                cardName = I18NManager.Get("Common_Role4");
                break;
            default:
                cardName = I18NManager.Get("MainLine_BattleSweepWindowText");
                break;
        }

        return cardName;
    }

    public RepeatedField<AwardPB> GetWeekMissionRewardPBByCount(int weight)
    {
        foreach (var v in WeekActivityRewardRulePbs)
        {
            if (v.Weight == weight)
            {
                return v.Awards;
            }

        }

        Debug.LogError("No Such PB");
        return null;

    }

    public RepeatedField<AwardPB> GetStarRoadRewardPBByCount(int count, PlayerPB player, ref int weight,
        RepeatedField<int> list)
    {
        foreach (var v in StarRoadRewardRulePbs)
        {
            if (v.Player == player)
            {
                if (list == null && v.Weight >= count)
                {
                    weight = v.Weight;
                    return v.Awards;
                }
                else if (list != null && !list.Contains(v.Weight))
                {
                    weight = v.Weight;
                    return v.Awards;
                }
            }

        }

        Debug.LogError("No Such PB");
        return null;
    }

    /// <summary>
    /// 获取星动之约活跃奖励
    /// </summary>
    /// <param name="weight"></param>
    /// <returns></returns>
    public RepeatedField<AwardPB> GetStarActivityRewardPBByCount(int weight)
    {

        var isNew = IsNewStarActivity();
        var list = isNew ? _newStarActivityRewardRulePbs : _starActivityRewardRulePbs;

        foreach (var v in list)
        {
            if (v.Weight == weight)
            {
                return v.Awards;
            }

        }

        Debug.LogError("No Such PB");
        return null;
    }

    /// <summary>
    /// 获取生日活跃奖励
    /// </summary>
    /// <param name="weight"></param>
    /// <returns></returns>
    public RepeatedField<AwardPB> GetPlayerBirthdayRewardPBByCount(int weight)
    {

        foreach (var v in _playerBirthdayRulePbs)
        {
            if (v.Weight == weight)
            {
                return v.Awards;
            }

        }

        Debug.LogError("No Such PB");
        return null;
    }

    /// <summary>
    /// 是否显示生日按钮
    /// </summary>
    /// <returns></returns>
    public bool IsShowPlayerBirthday()
    {
        if (IsHavePlayerBirthdayMission)
        {
            var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
            var overTimeStamp = GetPlayerBirthdayOverTimeStamp();
            return curTimeStamp < overTimeStamp && overTimeStamp - curTimeStamp > 1000;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// 是否显示星动之约主界面按钮
    /// </summary>
    /// <returns></returns>
    public bool IsShowStarActivity()
    {

        if (IsHaveStarActivityMission)
        {
            var curTimeStamp = ClientTimer.Instance.GetCurrentTimeStamp();
            var overTimeStamp = _starActivityTimePoint[_starActivityTimePoint.Count - 1];
            return curTimeStamp < overTimeStamp && overTimeStamp - curTimeStamp > 1000;
        }
        else
        {
            return false;
        }
    }


   



    /// <summary>
    /// 星动之约任务进度
    /// </summary>
    /// <returns></returns>
    public float StarActivityProgress()
    {
        float progress = 0;

        var userInfo = StarActivityInfoPb;
        
        if (userInfo!=null)
        {
            Debug.LogError("userInfo.Progress"+userInfo.Progress);
            float fixedDis = 162.5f;
            var list = IsNewStarActivity() ? _newStarActivityRewardRulePbs : _starActivityRewardRulePbs;
            int index=-1;
            for (int i = 0; i < list.Count; i++)
            {
                if (userInfo.Progress<=list[i].Weight)
                {
                    index = i;
                    break;
                }
            }

            if (index==-1)
            {
                var maxActive = StarActivityNum;
               if (userInfo.Progress>=maxActive)
               {
                  progress = fixedDis * 6;  
               }
               else
               {
                   var lastIndex = list.Count - 1;
                   var offset = maxActive - list[lastIndex].Weight;				
                   var num = userInfo.Progress - list[lastIndex].Weight;
                   progress = (list.Count * fixedDis) + fixedDis / offset * num; 
               }
            }
            else
            {
                if (index==0)
                {
                    progress = fixedDis / list[index].Weight*userInfo.Progress;
                }
                else
                {
                    var offset = list[index].Weight - list[index - 1].Weight;
                    var num = userInfo.Progress - list[index - 1].Weight;
                    progress = (index * fixedDis) + fixedDis / offset * num;
                }
            }
            
        }

        return progress;
    }


    /// <summary>
    /// 获取星路历程（奖励预览）数据
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public List<AwardPreviewVo> GetAwardPreviews(PlayerPB player)
    {
        List<AwardPreviewVo> list =new List<AwardPreviewVo>();

        foreach (var t in StarRoadRewardRulePbs)
        {
            if (t.Player==player)
            {
                var vo = new AwardPreviewVo(t);
                list.Add(vo);
            }
        }
        return list;
    }



#region 星路历程代码
//        public void UpdateStarCourse(MapField<int,int> starcourseSchedule)
//        {
//            if (StarCourseSchedule==null)
//            {
//                StarCourseSchedule=new Dictionary<int, int>();                    
//            }
//            foreach (var v in starcourseSchedule)
//            {
//                if (StarCourseSchedule.ContainsKey(v.Key))
//                {
//                    StarCourseSchedule[v.Key] = v.Value;
//                }
//                else
//                {
//                    StarCourseSchedule.Add(v.Key, v.Value);
//                }
//                                                             
//            }
//                    
//        }


        

        #endregion
        
        
    }
    
    


