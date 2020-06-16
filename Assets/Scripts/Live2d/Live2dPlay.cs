using System;

namespace game.main.Live2d
{
    public class Live2dPlay
    {
        public Live2dPlayableVo PlayableVo => _playableVo;
        
        private Action _onNextStep;
        private L2DView _l2DView;
        private Live2dPlayableVo _playableVo;

        private float _currentTime = 0;

        private int _currentCount = 0;

        private float _delayTime = 0;

        private Live2dPlayableVo.Live2dPlayType _playType;

        private bool _isDelayState = false;
        private float _intervalTime;

        public Live2dPlay(Live2dPlayableVo playableVo, L2DView l2DView, Action onNextStep)
        {
            _onNextStep = onNextStep;
            _l2DView = l2DView;
            _playableVo = playableVo;

            if (playableVo.motionName.Contains(L2DConst.MOTION_EYEBLINK))
            {
                _playType = Live2dPlayableVo.Live2dPlayType.EyeBlink;
            }
            else if(playableVo.motionName.Contains(L2DConst.MOTION_MOTIONLESS))
            {
                _playType = Live2dPlayableVo.Live2dPlayType.MontionLess;
            }
            else
            {
                _playType = Live2dPlayableVo.Live2dPlayType.Motion;
            }
            
            if(playableVo.delay <= 0)
            {
                Play();
            }
            else
            {
                _isDelayState = true;
            }
        }

        private void Play()
        {
            switch (_playType)
            {
                case Live2dPlayableVo.Live2dPlayType.Motion:
                    PlayMotion();
                    break;
                case Live2dPlayableVo.Live2dPlayType.EyeBlink:
                    PlayEyeBlink();
                    break;
                case Live2dPlayableVo.Live2dPlayType.MontionLess:
                    _l2DView.Model.StopAll();
                    break;
            }
        }
        
        private void Stop()
        {
            switch (_playType)
            {
                case Live2dPlayableVo.Live2dPlayType.Motion:
                case Live2dPlayableVo.Live2dPlayType.MontionLess:
                    _l2DView.Model.StopAll();
                    break;
                case Live2dPlayableVo.Live2dPlayType.EyeBlink:
                    _l2DView.Model.StopEyeBlink();
                    break;
            }
        }

        private void PlayEyeBlink()
        {
            _l2DView.Model.StartEyeBlink((int) (_playableVo.interval*1000));
            _l2DView.Model.SetExpression(_playableVo.expressionName);
        }
        
        private void PlayMotion()
        {
            int no = _l2DView.Model.MotionList.IndexOf(_playableVo.motionName);
            _l2DView.Model.StartMotion(L2DConst.MOTION_GROUP_IDLE, no, L2DConst.PRIORITY_IDLE, false);
            _l2DView.Model.SetExpression(_playableVo.expressionName);
        }

        public void Update(float time)
        {
            if (_isDelayState)
            {
                _delayTime += time;
                if (_delayTime >= _playableVo.delay)
                {
                    _isDelayState = false;
                    Play();
                }
            }
            else
            {
                if (_playableVo.PlayableType == PlayableType.ByTime)
                {
                    //按照持续时间来播放
                    _currentTime += time;
                    if (_currentTime >= _playableVo.duration && _playableVo.duration > 0)
                    {
                        DoNext();
                    }   
                    else if (_l2DView.Model.getMainMotionManager().isFinished())
                    {
                        if (_playableVo.interval <= 0)
                        {
                            Play();
                        }
                        else
                        {
                            _intervalTime += time;
                            if (_intervalTime >= _playableVo.interval)
                            {
                                _intervalTime = 0;
                                Play();
                            }
                        }
                    }
                }
                else if (_playableVo.PlayableType == PlayableType.ByCount)
                {
                    if (_l2DView.Model.getMainMotionManager().isFinished())
                    {
                        _currentCount++;
                        if (_currentCount >= _playableVo.count && _playableVo.count > 0)
                        {
                            DoNext();
                        }
                        else
                        {
                            if (_playableVo.interval > 0)
                            {
                                _intervalTime += time;
                                if (_intervalTime >= _playableVo.interval)
                                {
                                    _intervalTime = 0;
                                    Play();
                                }
                            }
                            else
                            {
                                _intervalTime = 0;
                                Play();
                            }
                        }
                    }
                }
            }
        }
        
        private void DoNext()
        {
            if (_playableVo.playNextAnimation)
            {
                _onNextStep();
            }
        }

        public void UpdatePlayData(Live2dPlayableVo vo)
        {
            if(_playableVo.expressionName != vo.expressionName)
                _l2DView.Model.SetExpression(vo.expressionName);
                
            if(_playableVo.motionName != vo.motionName)
            {
                int no = _l2DView.Model.MotionList.IndexOf(_playableVo.motionName);
                _l2DView.Model.StartMotion(L2DConst.MOTION_GROUP_IDLE, no, L2DConst.PRIORITY_IDLE, false);
            }
            
            _playableVo = vo;
        }
    }
}