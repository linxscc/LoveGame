using System;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace game.main
{
    public class SpinePlay
    {
        public static float HairTrackTime;

        private SpinePlayableVo _playableVo;
        private SkeletonGraphic _skeletonGraphic;
        private TrackEntry _trackEntry;

        public float CurrentTime;

        public int CurrentCount;

        private Action _playNextAction;
        private int _trackIndex;
        private bool _isStop;

        public SpinePlay(int trackIndex, SpinePlayableVo playableVo, SkeletonGraphic skeletonGraphic,
            Action playNextAction)
        {
            _isStop = false;
            _trackIndex = trackIndex;
            _playNextAction = playNextAction;
            _skeletonGraphic = skeletonGraphic;

            _playableVo = playableVo;

            _trackEntry = _skeletonGraphic.AnimationState.SetAnimation(_trackIndex, playableVo.animationName, true);
            if (_trackEntry.Animation.Name.Contains("toufa_"))
            {
                _trackEntry.TrackTime = HairTrackTime;
            }

            _trackEntry.TimeScale = _playableVo.speed;
            _trackEntry.Delay = playableVo.delay;

            CurrentTime = 0;

            if (_playableVo.PlayableType == PlayableType.ByCount)
            {
                CurrentCount = 0;
                _trackEntry.Complete += OnCount;
            }
        }

        private void OnCount(TrackEntry trackentry)
        {
            CurrentCount++;

            if (_playableVo.interval > 0)
            {
                _trackEntry.TimeScale = 0;
                _trackEntry.TrackTime = 0;
            }

            if (CurrentCount >= _playableVo.count && _playableVo.count > 0)
            {
                _trackEntry.TrackTime = 0;
                _trackEntry.TimeScale = 0;
                PlayNext();
            }
        }

        public void Update(float time)
        {
            CurrentTime += time;

            if (_playableVo.PlayableType == PlayableType.ByCount)
            {
                if (CurrentTime >= _playableVo.interval)
                {
                    _trackEntry.TimeScale = _playableVo.speed;
                    CurrentTime = 0;
                }
            }
            else if (_playableVo.PlayableType == PlayableType.ByTime)
            {
                if (_playableVo.duration > 0 && CurrentTime >= _playableVo.duration)
                {
                    PlayNext();
                    _trackEntry.TrackTime = 0;
                    _trackEntry.TimeScale = 0;
                    _isStop = true;
                }
            }
        }

        private void PlayNext()
        {
            if (_isStop)
                return;

            if (_trackEntry.Animation.Name.Contains("toufa_"))
            {
                HairTrackTime = _trackEntry.TrackTime;
            }

            if (_playableVo.playNextAnimation)
                _playNextAction();
        }
    }
}