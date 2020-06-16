using System;
using System.Collections.Generic;
using System.Linq;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace game.main
{
    public class SpinePlaySequence : IDisposable
    {
        private List<SpinePlayableVo> _playableList;
        private SkeletonGraphic _skeletonGraphic;

        private List<SpinePlay> _sequence;
        private int _currentIndex;
        private int _lastPlayIndex;

        public SpinePlaySequence(SkeletonGraphic skeletonGraphic, List<SpinePlayableVo> playableList)
        {
            _playableList = playableList;
            _skeletonGraphic = skeletonGraphic;
            
            _skeletonGraphic.UpdateComplete += OnUpdate;
        }

        public void Play()
        {
            _skeletonGraphic.Skeleton.SetToSetupPose();
            _skeletonGraphic.AnimationState.ClearTracks();
            
            _currentIndex = 0;
            _lastPlayIndex = -1;

            List<SpinePlayableVo> currentPlayableList = new List<SpinePlayableVo>();
            //找到同一个播放序列的Playable
            for (int i = 0; i < _playableList.Count; i++)
            {
                SpinePlayableVo playable = _playableList[i];
               

                if (_lastPlayIndex == -1)
                    _lastPlayIndex = playable.playIndex;

                if (playable.playIndex != _lastPlayIndex)
                    break;

                currentPlayableList.Add(playable);
                _currentIndex = i;
            }

            PlaySequence(currentPlayableList);
        }

        private void OnUpdate(ISkeletonAnimation animated)
        {
            for (int i = 0; i < _sequence.Count; i++)
            {
                _sequence[i].Update(Time.deltaTime);
            }
        }

        private void PlaySequence(List<SpinePlayableVo> list)
        {
            _sequence = new List<SpinePlay>();
            for (int i = 0; i < list.Count; i++)
            {
                SpinePlayableVo playable = list[i];
                SpinePlay play = new SpinePlay(i, playable, _skeletonGraphic, OnNextStep);
                _sequence.Add(play);
            }
        }

        private void OnNextStep()
        {
            _skeletonGraphic.AnimationState.ClearTracks();
            _skeletonGraphic.Skeleton.SetToSetupPose();

            List<SpinePlayableVo> currentPlayableList = new List<SpinePlayableVo>();

            _currentIndex++;
            
            if (_currentIndex >= _playableList.Count)
            {
                //End
                return;
            }

            int playIndex = _playableList[_currentIndex].playIndex;

            //找到同一个播放序列的Playable
            for (int i = _currentIndex; i < _playableList.Count; i++)
            {
                SpinePlayableVo playable = _playableList[i];

                if (playable.playIndex != playIndex)
                    break;

                currentPlayableList.Add(playable);
                _currentIndex = i;
            }

            PlaySequence(currentPlayableList);
        }

        public void Dispose()
        {
            _skeletonGraphic.UpdateComplete -= OnUpdate;
        }
    }
}