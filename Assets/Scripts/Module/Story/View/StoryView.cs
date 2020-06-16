using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Effect;
using Common;
using game.main;
using game.main.Live2d;
using game.tools;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;
using EventType = game.main.EventType;

public partial class StoryView : StoryViewBase
{
    private RawImage _bgImage;
    private RawImage _role1;
    private RawImage _role2;

    private DialogFrame _dialogFrame;

    private List<DialogVo> _dialogList;

    private DialogVo _currentDialogVo;

    private string _lastBgId;

    private StoryLoader _storyLoader;
    private string _lastRoleId1;
    private string _lastRoleId2;

    private string _lastBgMusicId;

    private bool _isFade;
    private RectTransform _mask;
    private bool _isDelayDialog;
    

    
    
    private bool _isUseClick;

    private Image _screenEffect;
    
    private void Awake()
    {
        _bgImage = transform.Find("Bg").GetComponent<RawImage>();

        _role1 = transform.Find("Role1").GetComponent<RawImage>();
        _role2 = transform.Find("Role2").GetComponent<RawImage>();

        _dialogFrame = transform.Find("DialogFrame").GetComponent<DialogFrame>();

        _mask = transform.Find("Mask").GetComponent<RectTransform>();

        _screenEffect = transform.GetImage("ScreenEffect");

        _recordBtn = transform.Find("RecordBtn").GetComponent<Button>();
        _recordBtn.onClick.AddListener(ShowRecordView);
        
        _skipBtn = transform.Find("SkipBtn").GetComponent<Button>();
        _skipBtn.onClick.AddListener(OnSkip);
        
        _playBtn = transform.Find("PlayBtn").GetComponent<Button>();
        _playBtn.transform.GetText("Text").text = I18NManager.Get("Story_Autoplay");
        _playBtn.onClick.AddListener(()=>
        {
            OnAutoPlay(!_isAutoPlay);
        });

        PointerClickListener.Get(gameObject).onClick = NextStep;

        RectTransform rect = GetComponent<RectTransform>();
        float containerH = rect.GetSize().y;
        _offsetY = (int) ((containerH - Main.StageHeight) / 2);

        Reset();

        _isAutoPlay = false;
        _dialogFrame.OnStepEnd = DoAutoPlay;

        _spineCache = new List<SkeletonGraphic>();
        _live2DCache = new List<Live2dGraphic>();
        
        _screenEffect.gameObject.Hide();
    }

    private void Start()
    {
        _mask.anchoredPosition =
            new Vector2(_mask.anchoredPosition.x, ModuleManager.OffY / 2);
        
        _mask.sizeDelta = new Vector2(_mask.sizeDelta.x,
            GetComponent<RectTransform>().rect.height + ModuleManager.OffY);
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

    public void ShowSkip(bool enable)
    {
        _skipBtn.gameObject.SetActive(enable);
    }
    
    public void Reset()
    {
        HideSelection();
        IsWait = true;
        _role1.gameObject.SetActive(false);
        _role2.gameObject.SetActive(false);
        _dialogFrame.gameObject.SetActive(false);
        _mask.gameObject.Hide();

        _currentIndex = 0;

        transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(10000, 0);
        
        OnAutoPlay(_continueAutoPlay);
    }

    public void InitData(List<DialogVo> dialogList)
    {
        _dialogList = dialogList;

        _storyLoader = new StoryLoader(_dialogList, OnAssetLoaded);
        _storyLoader.PreLoadAsset(_currentIndex, OnAssetLoaded);
    }

    public void InitBranch(List<DialogVo> dialogList)
    {
        _currentIndex = 0;
        _dialogList = dialogList;
        IsWait = true;

        _storyLoader = new StoryLoader(_dialogList, OnBranchLoaded);
        _storyLoader.PreLoadAsset(_currentIndex, OnBranchLoaded);
    }

    private void OnBranchLoaded(int index)
    {
        _hasSelection = false;
        _currentIndex = index;
        _isDelayDialog = false;
        IsWait = false;

        transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        ShowPage(_currentIndex);
    }

    private void OnAssetLoaded(int index)
    {
        if (index == _currentIndex)
        {
            if (index == 0)
            {
                SendMessage(new Message(MessageConst.CMD_STORY_READY));
                transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                IsWait = false;
                _isDelayDialog = true;
            }

            ShowPage(_currentIndex);
        }
    }

    public void ShowPage(int index = 0)
    {
        _currentDialogVo = _dialogList[index];
        
        ResetMask();

        _screenEffect.gameObject.SetActive(_currentDialogVo.ScreenEffectType == ScreenEffectType.Percent30White);
        
        switch (_currentDialogVo.CutScenesType)
        {
            case CutScenesType.None:
                _isFade = false;
                ShowPageStep2();
                break;
            case CutScenesType.Right2LeftWhite:
                Right2LeftAnimation("UIAtlas_Story_white");
                break;
            case CutScenesType.Right2LeftBlack:
                Right2LeftAnimation("UIAtlas_Story_black");
                break;
            case CutScenesType.FadeOutBlack:
                FadeOutBlack();
                break;
            case CutScenesType.CutLeftFadeOutBlack:
                CutLeftFadeOutBlack();
                break;
            case CutScenesType.Awake:
                AwakeEffect();
                break;
            case CutScenesType.ToBlur:
                ToBlurEffect();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    

    private void ShowPageStep2()
    {
        if (_lastBgId != null && _lastBgId != _currentDialogVo.BgImageId)
        {
            AssetManager.Instance.UnloadBundle(AssetLoader.GetStoryBgImage(_lastBgId));
            _storyLoader.BgImageCache.Remove(_lastBgId);
        }

        if (!string.IsNullOrEmpty(_currentDialogVo.BgMusicId) && _lastBgMusicId != _currentDialogVo.BgMusicId)
        {
            _lastBgMusicId = _currentDialogVo.BgMusicId;

            if (_lastBgMusicId.IndexOf("${mute}", StringComparison.Ordinal) != -1)
            {
                AudioManager.Instance.StopBackgroundMusic();
            }
            else
            {
                AudioManager.Instance.TweenVolumTo(AudioManager.Instance.BgMusicVolum, 2.5f);
                new AssetLoader().LoadAudio(AssetLoader.GetBackgrounMusicById(_lastBgMusicId),
                    (clip, loader) => { AudioManager.Instance.PlayBackgroundMusic(clip); });
            }
        }

        if (!string.IsNullOrEmpty(_currentDialogVo.DubbingId))
        {
            AudioManager.Instance.TweenVolumTo(0.33f, 0);
            new AssetLoader().LoadAudio(AssetLoader.GetDubbingById(_currentDialogVo.DubbingId),
                (clip, loader) =>
                {
                    if (clip == null)
                    {
                        Debug.LogWarning("AssetLoader Error->"+loader.FilePath);
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
            AudioManager.Instance.TweenVolumTo(1f, 2.5f);
        }

        _lastBgMusicId = _currentDialogVo.BgMusicId;

        if (_lastBgId != _currentDialogVo.BgImageId)
        {
            _lastBgId = _currentDialogVo.BgImageId;
            _bgImage.texture = _storyLoader.BgImageCache[_currentDialogVo.BgImageId];

            _bgImage.GetComponent<RectTransform>().sizeDelta =
                new Vector2(_currentDialogVo.Width, _currentDialogVo.Height);
            BackgroundSizeFitter fitter = _bgImage.GetComponent<BackgroundSizeFitter>();
            if (_currentDialogVo.Width > 1080)
            {
                fitter.FitType = FitType.Heigth;
            }
            else
            {
                fitter.FitType = FitType.Background;
            }

            fitter.Reset();
            fitter.DoFit();
        }

        _bgImage.color = Color.white;

        bool hasDialog = false;

        int roleIndex = 0;
        int spineIndex = 0;
        int live2dIndex = 0;

        _role1.gameObject.SetActive(false);
        _role2.gameObject.SetActive(false);

        int childIndex = 0;
        if (_currentDialogVo.EntityList.Count > 0)
        {
            for (int i = 0; i < _currentDialogVo.EntityList.Count; i++)
            {
                EntityVo entity = _currentDialogVo.EntityList[i];
                RectTransform rt = null;
                if (entity.type == EntityVo.EntityType.Role)
                {
                    roleIndex++;
                    childIndex++;
                    bool resetSprite = true;

                    if (roleIndex == 1)
                    {
                        if (_lastRoleId1 == entity.id)
                            resetSprite = false;

                        rt = _role1.GetComponent<RectTransform>();
                        _lastRoleId1 = entity.id;
                    }
                    else
                    {
                        if (_lastRoleId2 == entity.id)
                            resetSprite = false;

                        rt = _role2.GetComponent<RectTransform>();
                        _lastRoleId2 = entity.id;
                    }

                    RawImage img = rt.GetComponent<RawImage>();
                    if (resetSprite)
                    {
                        img.texture = _storyLoader.RoleImageCache[entity.id];
                    }

                    img.color = new Color(entity.color, entity.color, entity.color);

                    rt.gameObject.SetActive(true);
                    rt.anchoredPosition = new Vector3(entity.x, entity.y - _offsetY);
                    rt.sizeDelta = new Vector2(entity.width, entity.height);
                    
                    rt.SetSiblingIndex(childIndex);
                }
                else if (entity.type == EntityVo.EntityType.DialogFrame)
                {
                    hasDialog = true;

                    rt = _dialogFrame.GetComponent<RectTransform>();
                    rt.localPosition = new Vector3(0, -240 - _offsetY, 0);
                    rt.sizeDelta = new Vector2(entity.width, entity.height);

                    float delayTime = _isDelayDialog ? 1.3f : 0.38f;
                    _dialogFrame.SetData(entity);
                    _dialogFrame.Show(delayTime);
                    
                    SendMessage(new Message(MessageConst.CMD_STORY_RECODE_DIALOG, Message.MessageReciverType.DEFAULT, entity, entity.roleName));

                    if (!string.IsNullOrEmpty(entity.headId))
                    {
                        _dialogFrame.SetHeadTexture(_storyLoader.HeadImageCache[entity.headId]);
                    }
                }
                else if (entity.type == EntityVo.EntityType.Spine)
                {
                    spineIndex++;
                    CreateSpine(entity, spineIndex, ++childIndex);
                }
                else if(entity.type == EntityVo.EntityType.Live2D)
                {
                    live2dIndex++;
                    CreateLive2D(entity, live2dIndex, ++childIndex);
                }
            }
        }

        //3个跳转选项
        if (_currentIndex >= _dialogList.Count - 1 && 
            _currentDialogVo.Event != null &&
            _currentDialogVo.Event.EventType == EventType.Selection)
        {
            CreateSelection(_currentDialogVo.Event);
        }
        

        for (int i = _spineCache.Count - 1; i >= 0; i--)
        {
            if (i >= spineIndex)
            {
                SkeletonGraphic skg = _spineCache[i];
                DestroyImmediate(skg);
                _spineCache.RemoveAt(i);
            }
        }
        
        for (int i = _live2DCache.Count - 1; i >= 0; i--)
        {
            if (i >= live2dIndex)
            {
                Live2dGraphic l2g = _live2DCache[i];
                l2g.Hide();
            }
        }

        if (_isDelayDialog)
        {
            _dialogFrame.gameObject.SetActive(false);
            _isDelayDialog = false;
        }
        
        if (hasDialog == false)
        {
            ClientTimer.Instance.DelayCall(DoAutoPlay, 1.2f);
            _dialogFrame.gameObject.SetActive(false);
        }
    }

    private void NextStep(GameObject go)
    {
        //等待中，加载中，有分支剧情选项时
        if (IsWait || _storyLoader.IsLoading || _hasSelection)
            return;

        //区分用户点击和自动播放调用
        if (go != null)
            _isUseClick = true;

        if (_dialogList.Count - 1 > _currentIndex)
        {
            if (_dialogFrame.IsPlaying)
            {
                _dialogFrame.Typing();
                //正在打字的时候忽略用户点击对自动播放的影响
                _isUseClick = false;
            }
            else
            {
                _storyLoader.PreLoadAsset(++_currentIndex, ShowPage);
            } 
        }
        else
        {
            if (_dialogFrame.IsPlaying)
            {
                _dialogFrame.Typing();
                return;
            }

            DialogVo vo = _dialogList[_dialogList.Count-1];
            if (vo.Event != null)
            {
                OnEvent(vo.Event);                
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
        _storyLoader.Dispose();

        _spineCache.Clear();

        foreach (var live2DGraphic in _live2DCache)
        {
            DestroyImmediate(live2DGraphic);
        }
        _live2DCache.Clear();

        EffectManager.DestroyBackgroundEffect();

        Resources.UnloadUnusedAssets();
    }

}