using System;
using System.Collections.Generic;
using UnityEngine;

namespace game.main.Live2d
{
    public class Live2dPlaySequence
    {
        private List<Live2dPlayableVo> _playableList;

        private List<Live2dPlay> _sequence;
        
        private int _currentIndex;
        private int _lastPlayIndex;
        
        private L2DView _l2DView;

        private Action _endCallback;
        public Action EndCallback
        {
            get { return _endCallback; }
            set { _endCallback = value; }
        }

        public Live2dPlaySequence(L2DView l2DView, List<Live2dPlayableVo> playableList)
        {
            _l2DView = l2DView;
            _playableList = playableList;
        }

        public void Play(bool continuePlay = false)
        {
            _currentIndex = 0;
            _lastPlayIndex = -1;

            List<Live2dPlayableVo> currentPlayableList = new List<Live2dPlayableVo>();
            //找到同一个播放序列的Playable
            for (int i = 0; i < _playableList.Count; i++)
            {
                Live2dPlayableVo playableVo = _playableList[i];

                if (_lastPlayIndex == -1)
                    _lastPlayIndex = playableVo.playIndex;

                if (playableVo.playIndex != _lastPlayIndex)
                    break;

                currentPlayableList.Add(playableVo);
                _currentIndex = i;
            }

            PlaySequence(currentPlayableList, continuePlay);
        }
        
        private void PlaySequence(List<Live2dPlayableVo> list, bool continuePlay = false)
        {
            if(continuePlay == false)
                Reset();
            
            _sequence = new List<Live2dPlay>();
            
            for (int i = 0; i < list.Count; i++)
            {
                Live2dPlayableVo playableVo = list[i];
                Live2dPlay play = new Live2dPlay(playableVo, _l2DView, OnNextStep);
                _sequence.Add(play);
            }
        }

        private void Reset()
        {
            _l2DView.Model.StopAll();
        }

        private void OnNextStep()
        {
            _currentIndex++;
            
            if (_currentIndex >= _playableList.Count)
            {
                //End
                return;
            }

            int playIndex = _playableList[_currentIndex].playIndex;

            List<Live2dPlayableVo> currentPlayableList = new List<Live2dPlayableVo>();
            //找到同一个播放序列的Playable
            for (int i = 0; i < _playableList.Count; i++)
            {
                Live2dPlayableVo playableVo = _playableList[i];

                if (playableVo.playIndex == playIndex)
                {
                    currentPlayableList.Add(playableVo);
                    _currentIndex = i;
                }
            }

            if (currentPlayableList.Count == 0)
            {
                _endCallback?.Invoke();
            }
            else
            {
                PlaySequence(currentPlayableList);
            }
        }

        public void Update()
        {
            if (_sequence != null && _sequence.Count > 0)
            {
                for (int i = 0; i < _sequence.Count; i++)
                {
                    _sequence[i].Update(Time.deltaTime);
                }
            }
        }

        public void ChangeAnimation(List<Live2dPlayableVo> playableList)
        {
            _playableList = playableList;
            Play(true);
//            foreach (var vo in playableList)
//            {
//                foreach (var play in _sequence)
//                {
//                    if (vo.motionName == play.PlayableVo.motionName)
//                    {
//                        play.UpdatePlayData(vo);
//                    }
//                }
//            }
        }
    }
}