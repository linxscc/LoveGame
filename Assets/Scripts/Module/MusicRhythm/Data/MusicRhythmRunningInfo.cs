using System.Collections;
using System.Collections.Generic;
using Com.Proto;
using UnityEngine;

public enum GameScoreLevel
{
    D,
    C,
    B,
    A,
    S,
    SS
}

public class MusicRhythmRunningInfo  {

    public string musicName;
    public int musicId;
    public float musicLen;//音乐时长
    public float curRuningTime;//当前运行时间
    public float totalTime;//总时长
    public int curTickIndex;//当前索引
    public int activityId;//活动Id
    public MusicGameDiffTypePB diff;//难易等级


    public MusicGameScorePB ScoreRule;//分数规则
    public List<JudgeMusicDataPB> JudgeMusicDataPBs;
    public string diffName => I18NManager.Get("MusicRhythm_Level"+ ((int)diff)); //难易等级名

    public GameScoreLevel gameScoreLevel
    {
        get
        {
            GameScoreLevel l = GameScoreLevel.D;

            if(Socre< ScoreRule.CRank)
            {
                 l = GameScoreLevel.D;
            }else if(Socre < ScoreRule.BRank)
            {
                l = GameScoreLevel.C;
            }
            else if (Socre < ScoreRule.ARank)
            {
                l = GameScoreLevel.B;
            }
            else if (Socre < ScoreRule.SRank)
            {
                l = GameScoreLevel.A;
            }
            else if (Socre < ScoreRule.SSRank)
            {
                l = GameScoreLevel.S;
            }
            else if (Socre >= ScoreRule.SSRank)
            {
                l = GameScoreLevel.SS;
            }
            return l;
        }
    }

    public int Perfect { get; private set; }
    public int Good { get; private set; }
    public int Bad { get; private set; }
    public int Miss { get; private set; }
    public int Combo { get; private set; }

    /// <summary>
    /// 最大连击
    /// </summary>
    public int MaxCombo { get; private set; }

    public int Socre { get{
            int score = Perfect * GetJudgeRule(MusicRhythmClickCallbackType.Perfect).Score
               + Good * GetJudgeRule(MusicRhythmClickCallbackType.Good).Score
               + Bad * GetJudgeRule(MusicRhythmClickCallbackType.Bad).Score
               + Miss * GetJudgeRule(MusicRhythmClickCallbackType.Miss).Score;
            return score;
        } }


    private JudgeMusicDataPB GetJudgeRule(MusicRhythmClickCallbackType clickType)
    {
        string key = "";
        switch(clickType)
        {
            case MusicRhythmClickCallbackType.Perfect:
                key = "perfect";
                break;
            case MusicRhythmClickCallbackType.Good:
                key = "good";
                break;
            case MusicRhythmClickCallbackType.Bad:
                key = "bad";
                break;
            case MusicRhythmClickCallbackType.Miss:
                key = "miss";
                break;
            default:
                break;
        }
        return JudgeMusicDataPBs.Find((m) => { return m.Operation == key; });
    }


    public MusicRhythmRunningInfo()
    {
        curRuningTime = 0;
        curTickIndex = 0;
        Perfect = 0;
        Good = 0;
        Bad = 0;
        Miss = 0;
        Combo = 0;
        MaxCombo = 0;
    }

    public void AddOperate(MusicRhythmClickCallbackType musicRhythmClickCallbackType)
    {
        switch(musicRhythmClickCallbackType)
        {
            case MusicRhythmClickCallbackType.Perfect:
                Combo++;
                Perfect++;
                break;
            case MusicRhythmClickCallbackType.Good:
                Combo++;
                Good++;
                break;
            case MusicRhythmClickCallbackType.Bad:
                Combo++;
                Bad++;
                break;
            case MusicRhythmClickCallbackType.Miss:
                Combo = 0;
                Miss++;
                break;
            default:
                break;
        }
        MaxCombo = MaxCombo < Combo ? Combo : MaxCombo;
    }
}
