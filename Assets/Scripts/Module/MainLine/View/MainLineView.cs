using System;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Download;
using Assets.Scripts.Module.Framework.Utils;
using Assets.Scripts.Module.MainLine.View;
using DataModel;
using DG.Tweening;
using game.main;
using game.tools;
using UnityEngine;
using UnityEngine.UI;

public class MainLineView : View
{
    private int _currentChapterId;
    private Transform _prevChapterBtn;
    private Transform _nextChapterBtn;
    private RawImage _calendar;
    private RawImage _topBg;
    private Dropdown _dropdown;
    private LevelModel _levelModel;
    private ChapterVo _currentChapter;
    private Text _starText;
    private CanvasGroup _canvasGroup;
    private Image _dropdownImg;
    private Transform _download;
    private Button _downloadBtn;
    private Image _dropdownBg;
    private Image _expression;

    public static Color[] Colors;
    private RectTransform _calendar2;
    private Image _newNextChapter;

    private void Awake()
    {
        Colors = new Color[]
        {
            ColorUtil.HexToColor("A77254"), ColorUtil.HexToColor("3C5890"), ColorUtil.HexToColor("C9615D"),
            ColorUtil.HexToColor("8F7EAC")
        };
        
        _calendar = transform.GetRawImage("Calendar");
        _calendar2 = transform.GetRectTransform("Calendar2");
        _topBg = transform.GetRawImage("TopBg");
        
        _prevChapterBtn = transform.Find("Calendar2/PrevChapterBtn");
        _nextChapterBtn = transform.Find("Calendar2/NextChapterBtn");
        _newNextChapter =  _nextChapterBtn.GetImage("NewIcon");

        _dropdown = transform.Find("Calendar2/ChapterInfo/Dropdown").GetComponent<Dropdown>();
        _dropdown.onValueChanged.AddListener(OnSelectChapter);

        _dropdownBg = transform.GetImage("Calendar2/ChapterInfo/Dropdown/Template");
        _expression = transform.GetImage("Calendar2/Expression");

        _starText = transform.GetText("Calendar2/ChapterInfo/StarNum/Text");

        PointerClickListener.Get(_prevChapterBtn.gameObject).onClick = OnPrevChapter;
        PointerClickListener.Get(_nextChapterBtn.gameObject).onClick = OnNextChapter;

        _canvasGroup = transform.GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;

        _dropdownImg = transform.GetImage("Calendar2/ChapterInfo/DropdownImage");

        _download = transform.Find("Calendar2/ChapterInfo/Download");
        _download.gameObject.Hide();
        _downloadBtn = _download.GetButton("DownloadBtn");
        _downloadBtn.onClick.AddListener(DownloadChapterAudio);

        if ((float) Screen.height / Screen.width < 1.4f)
        {
            Vector2 vector = new Vector2(-64, _calendar2.anchoredPosition.y);

            _calendar.transform.GetRectTransform().anchoredPosition = vector;
            _calendar2.anchoredPosition = vector;
        }
    }

    private void OnNextChapter(GameObject go)
    {
        if (_levelModel.ChapterList[_dropdown.value].NextChapterVo == null)
        {
            PopupManager.ShowAlertWindow(I18NManager.Get("Guide_MainLineTip"));
            _dropdown.value = _currentChapterId - 1;
            return;
        }

        if (!_levelModel.ChapterList[_dropdown.value + 1].IsSupporterLevelSatisfy)
        {
            MainlineTipWindow win = PopupManager.ShowWindow<MainlineTipWindow>("MainLine/Prefabs/MainlineTipWindow");
            win.SetData(_levelModel.NewNormalLevel.DepartmentLevel);

            _dropdown.value = _currentChapterId - 1;
            return;
        }

        _dropdown.value = _dropdown.value + 1;
    }

    private void OnPrevChapter(GameObject go)
    {
        _dropdown.value = _dropdown.value - 1;
    }

    private void OnSelectChapter(int index)
    {
        if (_levelModel.ChapterList[index].IsNormalOpen == false)
        {
            FlowText.ShowMessage(I18NManager.Get("Common_ChapterNotUnlock"));
            _dropdown.value = _currentChapterId - 1;
            return;
        }

        if (!_levelModel.ChapterList[index].IsSupporterLevelSatisfy)
        {
            // FlowText.ShowMessage("应援会 Lv." + _levelModel.NewNormalLevel?.DepartmentLevel + "解锁");
            FlowText.ShowMessage(I18NManager.Get("MainLine_Hint3", _levelModel.NewNormalLevel?.DepartmentLevel));
            _dropdown.value = _currentChapterId - 1;
            return;
        }

        _currentChapterId = index + 1;
        _currentChapter = _levelModel.ChapterList[_currentChapterId - 1];

        IsShowDownloadBtn(_currentChapterId);

        int imageIndex = (_currentChapterId - 1) % 4 + 1;
        _calendar.texture = ResourceManager.Load<Texture>("MainLine/calendar" + imageIndex, ModuleName);
        _dropdownImg.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_MainLine_" + imageIndex);
        _dropdownBg.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_MainLine_dropdownBg" + imageIndex);

        _expression.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_MainLine_Expression" + (index % 12 + 1));

        _calendar.transform.RemoveChildren();

        Color color = Colors[index % 4];
        _prevChapterBtn.GetComponentInChildren<Text>().color = color;
        _nextChapterBtn.GetComponentInChildren<Text>().color = color;
        
            
        if(_currentChapter.NextChapterVo != null && _currentChapter.IsNormalPass && _currentChapter.NextChapterVo.LevelList[0].IsPass == false)
        {
            _newNextChapter.gameObject.SetActive(true);
        }
        else
        {
            _newNextChapter.gameObject.SetActive(false);
        }
//        foreach (var levelData in _levelModel.LocalDataList[index])
//        {
//            if (!string.IsNullOrEmpty(levelData.itemId))
//            {
//                Image tagImage = Instantiate(tagItem).transform.GetImage("Image");
//                tagImage.transform.parent.SetParent(_calendar.transform, false);
//                tagImage.sprite = AssetManager.Instance.GetSpriteAtlas(levelData.itemId);
//                tagImage.SetNativeSize();
//
//                tagImage.transform.parent.GetRectTransform().anchoredPosition = levelData.position;
//            }
//        }


        var battleItemPref = GetPrefab("MainLine/Prefabs/BattleItem");
        var storyItemPref = GetPrefab("MainLine/Prefabs/StoryItem");

        _prevChapterBtn.gameObject.SetActive(_currentChapterId != 1);

        if (_currentChapter.NextChapterVo == null && _currentChapter.IsNormalPass)
        {
            _nextChapterBtn.gameObject.SetActive(true);
        }
        else if (_currentChapter.NextChapterVo != null && _currentChapter.NextChapterVo.IsNormalOpen)
        {
            _nextChapterBtn.gameObject.SetActive(true);
        }
        else
        {
            _nextChapterBtn.gameObject.SetActive(false);
        }

        //策划说不要这个地方了
        // _starText.text = _currentChapter.CurrentStar + "/" + _currentChapter.MaxStar;

        Transform item = null;
        foreach (var levelVo in _currentChapter.LevelList)
        {
            if (levelVo.IsPass == false && levelVo != _levelModel.NewNormalLevel)
                continue;

            if (levelVo.LevelType == LevelTypePB.Story)
            {
                //剧情
                item = Instantiate(storyItemPref).transform;
            }
            else if (levelVo.LevelType == LevelTypePB.Value)
            {
                //战斗
                item = Instantiate(battleItemPref).transform;
            }

            bool doAnimation = _levelModel.LastLevel != null && 
                               _levelModel.NewNormalLevel != null &&
                               _levelModel.NewNormalLevel.LevelId != _levelModel.LastLevel.LevelId &&
                               levelVo == _levelModel.NewNormalLevel;

            item.gameObject.AddComponent<CalendarItem>().SetData(levelVo, doAnimation);
            item.GetComponent<RectTransform>().anchoredPosition = levelVo.Positon;
            item.SetParent(_calendar.transform, false);
            PointerClickListener.Get(item.gameObject).onClick = OnCalendarItemClick;
        }
        
        _levelModel.LastLevel = _levelModel.NewNormalLevel;

        var tagItem = GetPrefab("MainLine/Prefabs/TagItem");

        if (_levelModel.NewNormalLevel != null && _levelModel.NewNormalLevel.ChapterGroup == _currentChapterId)
        {
            Image tagImage = Instantiate(tagItem).transform.GetImage("Image");
            tagImage.transform.parent.SetParent(_calendar.transform, false);
            
            tagImage.color = new Color(1,1,1,0);
            tagImage.DOFade(1, 0.05f).SetDelay(0.6f);
            tagImage.transform.parent.GetRectTransform().anchoredPosition = _levelModel.NewNormalLevel.Positon;
        }

        if (_canvasGroup.alpha < 1)
        {
            _canvasGroup.alpha = 0.5f;
            _canvasGroup.DOFade(1, 0.2f);
        }
    }

    private void OnCalendarItemClick(GameObject go)
    {
        LevelVo data = go.GetComponent<CalendarItem>().LevelVo;
        if (data.LevelType == LevelTypePB.Value)
        {
            SendMessage(new Message(MessageConst.MODULE_MAINLINE_SHOW_BATTLE_VIEW, Message.MessageReciverType.DEFAULT,
                data));
        }
        else
        {
            if (IsNeedDownload(_currentChapterId)) //是否需要下载
            {
                if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                {
                    CacheManager.DownloadChapterCache(_currentChapterId, str =>
                    {
                        Debug.LogError(str);
                        _download.gameObject.Hide();
                        SendMessage(new Message(MessageConst.MODULE_MAINLINE_SHOW_STORY_VIEW,
                            Message.MessageReciverType.DEFAULT, data));
                    });
                }
                else
                {
                    var isHasKey = PlayerPrefs.HasKey("ChapterId" + _currentChapterId);
                    if (isHasKey)
                    {
                        SendMessage(new Message(MessageConst.MODULE_MAINLINE_SHOW_STORY_VIEW,
                            Message.MessageReciverType.DEFAULT, data));
                    }
                    else
                    {
                        string content = I18NManager.Get("Download_MainStory1",
                            (Math.Round(GetAudioSize(_currentChapterId) * 1f / 1048576f, 2)));
                        CacheManager.ConfirmNeedToDownloadChapterAudio(content, I18NManager.Get("Common_OK1"),
                            I18NManager.Get("Download_JumpDownload"), _currentChapterId,
                            (s =>
                            {
                                SendMessage(new Message(MessageConst.MODULE_MAINLINE_SHOW_STORY_VIEW,
                                    Message.MessageReciverType.DEFAULT, data));
                            }), (() =>
                            {
                                string key = "ChapterId" + _currentChapterId;
                                var value = key;
                                PlayerPrefs.SetString(key, value);
                                SendMessage(new Message(MessageConst.MODULE_MAINLINE_SHOW_STORY_VIEW,
                                    Message.MessageReciverType.DEFAULT, data));
                            }));
                    }
                }
            }
            else
            {
                SendMessage(new Message(MessageConst.MODULE_MAINLINE_SHOW_STORY_VIEW,
                    Message.MessageReciverType.DEFAULT, data));
            }
        }
    }

    public void EnterChapter(LevelModel levelModel)
    {
        _levelModel = levelModel;
        _currentChapterId = levelModel.ActiveLevel.ChapterGroup;


        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        for (int i = 0; i < levelModel.ChapterList.Count; i++)
        {
            Dropdown.OptionData opt =
                new Dropdown.OptionData(levelModel.ChapterList[i].ChapterName + "·" +
                                        levelModel.ChapterList[i].ChapterDesc);

            options.Add(opt);
        }

        _dropdown.options = options;

        if (_dropdown.value == _currentChapterId - 1)
        {
            OnSelectChapter(_currentChapterId - 1);
        }
        else
        {
            _dropdown.value = _currentChapterId - 1;
        }
    }


    private void ShowIconImg(int id)
    {
        Transform content = transform.Find("Content");
        for (int i = 0; i < content.childCount; i++)
        {
            content.GetChild(i).gameObject.SetActive(id.ToString() == content.GetChild(i).gameObject.name);
        }
    }


    //是否显示下载按钮
    private void IsShowDownloadBtn(int currentChapterId)
    {
        if (IsNeedDownload(currentChapterId))
        {
            _download.gameObject.Show();
        }
        else
        {
            _download.gameObject.Hide();
        }
    }


    private bool IsNeedDownload(int currentChapterId)
    {
        var isNeed = CacheManager.IsNeedDownLoad(currentChapterId);
        return isNeed;
    }


    //下载按钮
    private void DownloadChapterAudio()
    {
        var size = Math.Round(GetAudioSize(_currentChapterId) * 1f / 1048576f, 2);
        string content = I18NManager.Get("Download_MainStory", _currentChapterId, size);
        CacheManager.ConfirmNeedToDownloadChapterAudio(content, I18NManager.Get("Common_OK1"),
            I18NManager.Get("Common_Cancel1"), _currentChapterId, (
                s => { _download.gameObject.Hide(); }));
    }


    //获取大小
    private long GetAudioSize(int currentChapterId)
    {
        var cacheVo = CacheManager.CheckMainStoryCache(currentChapterId);
        return cacheVo.sizeList[0];
    }

    public void ShowNextChapter()
    {
        OnNextChapter(null);
    }
}