using Assets.Scripts.Framework.GalaSports.Core;
using Com.Proto;
using game.main;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MusicRhythmClickCallbackType
{
    None,
    Miss,
    Bad,
    Good,
    Perfect,
}

namespace DataModel
{
    public class MusicRhythmModel : Model
    {
        public MusicRhythmRunningInfo runningInfo;
        private MusicRhythmInfo _rhythmInInfo;

        private ActivityMusicVo _activityMusicVo;

        private List<ChallengeMusicDataPB> _challengeMusicDataPBs = new List<ChallengeMusicDataPB>();
        private List<MallInfoPB> _mallInfoPBs = new List<MallInfoPB>();
        private List<RefreshDataPB> _refreshDataPBs = new List<RefreshDataPB>();
        private List<MusicGameScorePB> _musicGameScorePBs = new List<MusicGameScorePB>();
        private List<MusicInfoPB> _musicInfoPBs = new List<MusicInfoPB>();
        private List<JudgeMusicDataPB> _judgeMusicDataPBs = new List<JudgeMusicDataPB>();
        private List<ComboPB> _comboPBs = new List<ComboPB>();


        public int MusicId
        {
            get
            {
                return _activityMusicVo.MusicId;
            }
        }

        public MusicGameType MusicGameType => _activityMusicVo.MusicGameType;

        public MusicGameDiffTypePB Diff   //难度
        {
            get
            {
                return _activityMusicVo.Diff;
            }
        }

        public void InitRule(Rules res)
        {
            //Debug.LogError("ChallengeMusicData " + res.ChallengeMusicData);
            //Debug.LogError("MallInfo " + res.MallInfo);
            //Debug.LogError("RefreshData " + res.RefreshData);
            //Debug.LogError("MusicGameScore " + res.MusicGameScore);
            //Debug.LogError("MusicInfo " + res.MusicInfo);
            //Debug.LogError("JudgeMusicData " + res.JudgeMusicData);
            //Debug.LogError("Combo " + res.Combo);
            _challengeMusicDataPBs.Clear();
            _challengeMusicDataPBs.AddRange(res.ChallengeMusicData);
            _mallInfoPBs.Clear();
            _mallInfoPBs.AddRange(res.MallInfo);
            _refreshDataPBs.Clear();
            _refreshDataPBs.AddRange(res.RefreshData);
            _musicGameScorePBs.Clear();
            _musicGameScorePBs.AddRange(res.MusicGameScore);
            _musicInfoPBs.Clear();
            _musicInfoPBs.AddRange(res.MusicInfo);
            _judgeMusicDataPBs.Clear();
            _judgeMusicDataPBs.AddRange(res.JudgeMusicData);
            _comboPBs.Clear();
            _comboPBs.AddRange(res.Combo);
        }
        /// <summary>
        /// 获取歌名
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetMusicNameById(int id)
        {
            return _activityMusicVo.MusicName;
        }
        /// <summary>
        /// 获取这首歌分数信息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="diff"></param>
        /// <returns></returns>
        public MusicGameScorePB GetMusicGameScoreRule(int id,MusicGameDiffTypePB diff)
        {
            foreach (var v in _musicGameScorePBs)
            {
                if (v.MusicId == id&&v.DiffType==diff)
                {
                    return v;
                }
            }
            return null;
        }



        public List<JudgeMusicDataPB> GetJudgeRule()
        {
            return _judgeMusicDataPBs;
        }


        public void InitData(ActivityMusicVo vo)
        {
            _activityMusicVo = vo;
        }

        public void InitRunningData(float musicLen)
        {
            runningInfo = new MusicRhythmRunningInfo();
            int  musicId = _activityMusicVo.MusicId;
            //LoadJson("10000");
            //LoadJson("10001");
            //LoadJson("10002");
            MusicGameDiffTypePB diff = _activityMusicVo.Diff;
            string jsonId = musicId.ToString() + ((int)diff).ToString();
            Debug.LogError(jsonId);

            _rhythmInInfo = LoadJson(jsonId);
            _rhythmInInfo.Ticks.Sort();
            runningInfo.activityId = _activityMusicVo.ActivityId;
            runningInfo.diff = _activityMusicVo.Diff;
            runningInfo.musicLen = musicLen;
            runningInfo.musicId = musicId;
            runningInfo.musicName = GetMusicNameById(musicId);
            runningInfo.ScoreRule = GetMusicGameScoreRule(musicId, diff);
            runningInfo.JudgeMusicDataPBs = GetJudgeRule();
        }

        private MusicRhythmInfo LoadJson(string jsonId)
        {
            string text = new AssetLoader().LoadTextSync(AssetLoader.GetMusicRhythmDataPath(jsonId));
            MusicRhythmInfo info = JsonConvert.DeserializeObject<MusicRhythmInfo>(text);
            TestMusicRhythmInfo(jsonId, info);
            return info;
        }


        private void TestMusicRhythmInfo(string jsonId,MusicRhythmInfo info)
        {
            int shortNum = 0;
            int longNum = 0;
            foreach(var v in info.Ticks)
            {
                if (    v.TickType==1)
                {

                    shortNum++;
                }
                else  if (v.TickType == 2)
                {
                    longNum++;
                }
            }
            Debug.LogError("jsonId = "+ jsonId+"  shortNum = " + shortNum+ "   longNum = "+ longNum);
        }

        //float _offset = 308;
        float _offset = 353;
        public Tick CheckSatisfyTick(float start,float end)
        {
            Tick tick = null;
            int index = runningInfo.curTickIndex;

            if (index >= _rhythmInInfo.Ticks.Count)
                return null;

            tick = _rhythmInInfo.Ticks[index];

            if (tick.TickTime- _offset > start*100 && tick.TickTime - _offset <= end*100)
            {
                runningInfo.curTickIndex++;
                return tick;
            }

            return null;
        }
    }
}