using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Com.Proto;
using Common;
using game.main;
using Google.Protobuf.Collections;

/// <summary>
///     练习室Model
/// </summary>
public class TrainingRoomModel : Model
{
    public enum MusicGameActivity
    {
     /// <summary>
     /// 练习模式
     /// </summary>
        PRACTICING,
    /// <summary>
    /// 商业演出
    /// </summary>
        COMMERCIAL,
        /**
     * livehouse演出
     */
        LIVEHOUSE,
        /**
     * 演唱会演出
     */
        CONCERT,
        /**
     * 粉丝见面会演出
     */
        FANMEETING
    }

    private RepeatedField<ChallengeMusicDataPB> _challengeMusicData; //音游活动数据
    private RepeatedField<ComboPB> _combo; //combo信息
    private int _curSongIndex; //今天歌曲Index

    private long _integral; //当前积分
    private RepeatedField<MallInfoPB> _mallInfo; //商店信息

    private List<UserMusicGameVO> _musicActivityDates; //选择音乐活动数据
    private int _musicDiffProgress; //歌曲难度进度

    private List<RefreshDataPB> _musicGameRefreshRules; //音乐更换要求规则
    private RepeatedField<MusicInfoPB> _musicInfo; //音乐信息
    private RepeatedField<JudgeMusicDataPB> _musicOperation; //音游操作信息
    private RepeatedField<MusicGameScorePB> _rating; //评级信息
    private RepeatedField<RefreshDataPB> _refreshData; //刷新信息
    private int _refreshMusicNum; //更换歌曲演奏要求次数

    private List<TrainingRoomCardVo> _trainingRoomCards;

    public List<TrainingRoomCardVo> ChooseCards;

    public UserMusicGameVO CurMusicGame;

    /// <summary>
    ///     获取音游规则
    /// </summary>
    /// <param name="rules"></param>
    public void InitRules(Rules rules)
    {
        _challengeMusicData = rules.ChallengeMusicData;
        _mallInfo = rules.MallInfo;
        _refreshData = rules.RefreshData;
        _rating = rules.MusicGameScore;
        _musicInfo = rules.MusicInfo;
        _musicOperation = rules.JudgeMusicData;
        _combo = rules.Combo;

        InitMusicActivityRefreshRules(_refreshData);
    }

    public MusicInfoPB GetMusicInfoPbById(int musicId)
    {
        foreach (MusicInfoPB pb in _musicInfo)
        {
            if (pb.MusicId == musicId)
                return pb;
        }

        return null;
    }

    public MusicGameScorePB GetMusicGameScorePB(int musicId, MusicGameDiffTypePB diff)
    {
        foreach (MusicGameScorePB musicGameScorePb in _rating)
        {
            if (musicGameScorePb.MusicId == musicId && diff == musicGameScorePb.DiffType)
            {
                return musicGameScorePb;
            }
        }

        return null;
    }
    /// <summary>
    ///     获取商店信息规则
    /// </summary>
    /// <returns></returns>
    public RepeatedField<MallInfoPB> GetExchangeShopRules()
    {
        return _mallInfo;
    }

    /// <summary>
    ///     获取刷新规则
    /// </summary>
    /// <returns></returns>
    public RepeatedField<RefreshDataPB> GetRefreshRules()
    {
        return _refreshData;
    }


    /// <summary>
    ///     获取评级规则
    /// </summary>
    /// <returns></returns>
    public RepeatedField<MusicGameScorePB> GetRatingRules()
    {
        return _rating;
    }


    /// <summary>
    ///     获取音乐信息
    /// </summary>
    /// <returns></returns>
    public RepeatedField<MusicInfoPB> GetMusicInfo()
    {
        return _musicInfo;
    }


    /// <summary>
    ///     进入音游第一个界面Init
    /// </summary>
    /// <param name="res"></param>
    public void InitOpenMusicGame(OpenMusicGame res)
    {
        _integral = res.Integral;
        _curSongIndex = res.Progress;
    }

    /// <summary>
    ///     获取今日歌曲信息
    /// </summary>
    /// <returns></returns>
    public MusicInfoPB GetTodayMusicInfo()
    {
        return _musicInfo[_curSongIndex];
    }


    /// <summary>
    ///     获取今天歌曲索引
    /// </summary>
    /// <returns></returns>
    public int GetCurSongIndex()
    {
        return _curSongIndex;
    }


    /// <summary>
    ///     获取明天歌曲信息
    /// </summary>
    /// <returns></returns>
    public MusicInfoPB GetTomorrowMusicInfo()
    {
        var nextSongIndex = _curSongIndex + 1;
        if (nextSongIndex > _musicInfo.Count - 1) nextSongIndex = 0;
        return _musicInfo[nextSongIndex];
    }

    /// <summary>
    ///     获取当前积分
    /// </summary>
    /// <returns></returns>
    public long GetCurIntegral()
    {
        return _integral;
    }

    /// <summary>
    ///     更新当前积分
    /// </summary>
    /// <param name="integral"></param>
    public void UpdateCurIntegral(long integral)
    {
        _integral = integral;
        EventDispatcher.TriggerEvent(EventConst.UpdateExchangeIntegral);
    }


    /// <summary>
    ///     进入演奏选择界面Init
    /// </summary>
    /// <param name="res"></param>
    public void InitUserMusicGameContent(Concert res)
    {
        _refreshMusicNum = res.RefreshMusicCount;
        _musicDiffProgress = res.MusicDiffProgress;
        InitMusicActivityDates(res.UserMusicGame);
    }

    /// <summary>
    ///     初始化音游活动集合
    /// </summary>
    /// <param name="list"></param>
    private void InitMusicActivityDates(RepeatedField<UserMusicGamePB> list)
    {
//        if (_musicActivityDates ==null)
//        {
        _musicActivityDates = new List<UserMusicGameVO>();

        foreach (var t in list)
        {
            var vo = new UserMusicGameVO(t);
            _musicActivityDates.Add(vo);
        }

        // }
    }


    /// <summary>
    ///     初始化音游活动刷新规则
    /// </summary>
    private void InitMusicActivityRefreshRules(RepeatedField<RefreshDataPB> list)
    {
        if (_musicGameRefreshRules == null) _musicGameRefreshRules = new List<RefreshDataPB>();

        foreach (var t in list) //0是音游活动更换规则
            if (t.RefreshType == 0)
                _musicGameRefreshRules.Add(t);
    }


    public RefreshDataPB GetCurMusicGameRefreshRules(int refreshNum)
    {
        RefreshDataPB pb = null;
        var curNum = refreshNum + 1;

        foreach (var t in _musicGameRefreshRules)
            if (t.RefreshTimes == curNum)
            {
                pb = t;
                break;
            }

        //当刷新次数大于7次时，我们封顶拿最后刷新消耗钻石的数量
        return pb ?? (pb = _musicGameRefreshRules[_musicGameRefreshRules.Count - 1]);
    }


    public void UpdateMusicActivityDates()
    {
        var curActivityId = CurMusicGame.ActivityId;
        foreach (var t in _musicActivityDates)
            if (t.ActivityId == curActivityId)
            {
                foreach (var t1 in ChooseCards) t.UserCards.Add(t1);
                break;
            }
    }


    /// <summary>
    ///     更新音乐活动集合数据
    /// </summary>
    /// <param name="vo"></param>
    public void UpdateMusicActivityDates(UserMusicGameVO vo)
    {
        for (var i = 0; i < _musicActivityDates.Count; i++)
            if (_musicActivityDates[i].ActivityId == vo.ActivityId)
            {
                _musicActivityDates[i] = vo;
                break;
            }
    }

    /// <summary>
    ///     获取音游活动数据集合
    /// </summary>
    /// <returns></returns>
    public List<UserMusicGameVO> GetMusicActivityDates()
    {
        return _musicActivityDates;
    }

    public UserMusicGameVO GetMusicActivityDate(int activity)
    {
        return _musicActivityDates[activity - 1];
    }


    /// <summary>
    ///     获取音游活动数据
    /// </summary>
    /// <param name="activityId"></param>
    /// <param name="musicChapterId"></param>
    /// <returns></returns>
    public ChallengeMusicDataPB GetMusicData(int activityId, int musicChapterId)
    {
        ChallengeMusicDataPB pb = null;
        foreach (var t in _challengeMusicData)
            if (t.ActivityType == activityId && t.MusicChapterId == musicChapterId)
            {
                pb = t;
                break;
            }

        return pb;
    }

    /// <summary>
    ///     获取歌曲更换要求次数
    /// </summary>
    /// <returns></returns>
    public int GetRefreshMusicNum()
    {
        return _refreshMusicNum;
    }

    /// <summary>
    ///     更新歌曲更换要求次数
    /// </summary>
    /// <param name="num"></param>
    public void UpdateRefreshMusicNum(int num)
    {
        _refreshMusicNum = num;
    }

    /// <summary>
    ///     获取歌曲难度进度
    /// </summary>
    /// <returns></returns>
    public int GetMusicDiffProgress()
    {
        return _musicDiffProgress;
    }

    /// <summary>
    ///     更新歌曲难度进度
    /// </summary>
    /// <param name="musicDiffProgress"></param>
    public void UpdateMusicDiffProgress(int musicDiffProgress)
    {
        _musicDiffProgress = musicDiffProgress;
    }


    /// <summary>
    ///     获取能力描述
    /// </summary>
    /// <param name="abilityId"></param>
    /// <returns></returns>
    public string GetAbility(int abilityId)
    {
        var pb = (AbilityPB) abilityId;
        var temp = string.Empty;
        switch (pb)
        {
            case AbilityPB.Singing:
                temp = "唱功";
                break;
            case AbilityPB.Dancing:
                temp = "舞力";
                break;
            case AbilityPB.Composing:
                temp = "原创";
                break;
            case AbilityPB.Popularity:
                temp = "人气";
                break;
            case AbilityPB.Charm:
                temp = "魅力";
                break;
            case AbilityPB.Perseverance:
                temp = "毅力";
                break;
        }

        return temp;
    }


    public void InitTrainingRoomCardList(List<UserCardVo> list)
    {
        _trainingRoomCards = new List<TrainingRoomCardVo>();
        for (var i = 0; i < list.Count; i++)
        {
            var vo = new TrainingRoomCardVo(list[i]);
            _trainingRoomCards.Add(vo);
        }
    }

    public List<TrainingRoomCardVo> GetTrainingRoomCards()
    {
        return _trainingRoomCards;
    }

    public void InitChooseCards()
    {
        ChooseCards = new List<TrainingRoomCardVo>();
    }

    public void AddCard(TrainingRoomCardVo vo)
    {
        ChooseCards.Add(vo);
    }

    public void CancelCard(TrainingRoomCardVo vo)
    {
        for (var i = 0; i < ChooseCards.Count; i++)
            if (vo.UserCardVo.CardId == ChooseCards[i].UserCardVo.CardId)
            {
                ChooseCards.RemoveAt(i);
                break;
            }
    }
}