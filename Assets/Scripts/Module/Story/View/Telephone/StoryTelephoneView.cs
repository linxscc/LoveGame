using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module.Effect;
using Common;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;
using EventType = game.main.EventType;

namespace game.main
{
    public class StoryTelephoneView : StoryViewBase
    {
        private RawImage _bgImage;
        private Text _roleNameText;
        private TelephoneVo _telephoneVo;
        private DialogFrameTelephone _dialogFrame;

        private Image _headImage;
        private BackgroundBlurEffect _bgBlurEffect;

        private void Awake()
        {
            AudioManager.Instance.TweenVolumTo(0.33f, 0);
            
            _bgImage = transform.GetRawImage("BgImage");
            _roleNameText = transform.GetText("RoleNameText");
            _dialogFrame = transform.Find("DialogFrame").GetComponent<DialogFrameTelephone>();

            _headImage = transform.GetImage("Head/Mask/HeadImage");

            _dialogFrame.OnStepEnd = DoAutoPlay;

            _skipBtn = transform.Find("SkipBtn").GetComponent<Button>();
            _skipBtn.onClick.AddListener(OnSkip);

            _recordBtn = transform.Find("RecordBtn").GetComponent<Button>();
            _recordBtn.onClick.AddListener(ShowRecordView);

            _playBtn = transform.Find("PlayBtn").GetComponent<Button>();
            _playBtn.transform.GetText("Text").text = I18NManager.Get("Story_Autoplay");
            _playBtn.onClick.AddListener(()=>
            {
                OnAutoPlay(!_isAutoPlay);
            });

            PointerClickListener.Get(gameObject).onClick = NextStep;

            IsWait = true;

            _bgBlurEffect = EffectManager.CreateImageBlurEffect();
        }

        private void DoAutoPlay()
        {
            if (_isAutoPlay)
                NextStep(null);
        }

        protected override void OnAutoPlay(bool isAutoPlay)
        {
            base.OnAutoPlay(isAutoPlay);
            if (_dialogFrame.IsPlaying == false && _isAutoPlay)
            {
                NextStep(null);
            }
        }

        public void Reset()
        {
            HideSelection();
            IsWait = true;
            _dialogFrame.gameObject.SetActive(false);

            _currentIndex = 0;

            OnAutoPlay(_continueAutoPlay);
        }
        
        public void SetData(TelephoneVo vo, bool showAnimation)
        {
            OpenAnimation(showAnimation);
            
            _telephoneVo = vo;

            _bgBlurEffect.StartRecord(
                ResourceManager.Load<Texture>(AssetLoader.GetStoryBgImage(_telephoneVo.bgImageId), ModuleName), tex =>
                {
                    _bgImage.texture = tex;
                });
            _roleNameText.text = vo.GetRoleName();

            _headImage.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Story_Role" + (int) vo.Role);

            _currentIndex = 0;
            IsWait = false;
            _dialogFrame.gameObject.Show();

            if (_continueAutoPlay)
            {
                OnAutoPlay(_continueAutoPlay);
            }
            else
            {
                NextStep(null);
            }
        }
        
        private void NextStep(GameObject go)
        {
            if (IsWait || _hasSelection)
                return;

            if (_telephoneVo.dialogList.Count > _currentIndex)
            {
                if (_dialogFrame.IsPlaying)
                {
                    _dialogFrame.Typing();
                }
                else
                {
                    TelephoneDialogVo data = _telephoneVo.dialogList[_currentIndex++];
                    _dialogFrame.SetData(data, _telephoneVo.GetRoleName());
                    _dialogFrame.Show(0.5f);
                    
                    if (!string.IsNullOrEmpty(data.AudioId))
                    {
                        new AssetLoader().LoadAudio(AssetLoader.GetDubbingById(data.AudioId),
                            (clip, loader) =>
                            {
                                if (clip == null)
                                {
                                    Debug.LogWarning("TelephoneView AssetLoader Error->"+loader.FilePath);
                                    return;
                                }
                                ClientTimer.Instance.DelayCall(() =>
                                {
                                    _dialogFrame.SetAudioTime(clip.length);
                                    AudioManager.Instance.PlayDubbing(clip);
                                }, 0.5f);
                            });
                    }
                    else
                    {
                        AudioManager.Instance.StopDubbing(); 
                    }

                    string roleName = data.IsHeroine ? GlobalData.PlayerModel.PlayerVo.UserName : _telephoneVo.GetRoleName();
                    SendMessage(new Message(MessageConst.CMD_STORY_RECODE_DIALOG, Message.MessageReciverType.DEFAULT,
                        data,roleName ));
                }
            }
            else
            {
                if (_dialogFrame.IsPlaying)
                {
                    _dialogFrame.Typing();
                    return;
                }

                if (_telephoneVo.Event != null)
                {
                    if (_telephoneVo.Event.EventType == EventType.Selection)
                    {
                        CreateSelection(_telephoneVo.Event);
                    }
                    else
                    {
                        OnEvent(_telephoneVo.Event);
                    }
                }
                else
                {
                    EndStory();
                }

                IsWait = true;
            }
        }

        private void OnDestroy()
        {
            EffectManager.DestroyBackgroundEffect();
            AudioManager.Instance.TweenVolumTo(1f, 2.5f);
        }


        
    }
}