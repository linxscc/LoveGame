using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Download;
using Common;
using game.main;
using System;
using System.Collections;
using System.Collections.Generic;
using game.tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Newtonsoft.Json;
using Framework.Utils;

public class VisitLevelView : View
{
    VisitVo _curVisitVo;
    List<VisitLevelVo> _levelList;
    LoopVerticalScrollRect _loopRect;
    RectTransform MapBg;
    Button _weather;
    Button _loaddown;
    private Image _loveStage;
    private string _stageStr;

    Text _weatherTip;
    private void Awake()
    {
        //MapBg = transform.Find("MapBg").GetComponent<RectTransform>();
        _stageStr = "恋人";
        MapBg = transform.Find("Scroll View/Content/MapBg").GetComponent<RectTransform>();
        _weather = transform.GetButton("Weather");
        _weatherTip = transform.GetText("WeatherTip/Text");
        _loveStage = transform.GetImage("LoveStage");
        _weather.onClick.AddListener(() =>
        {
            if (_curVisitVo != null)
            {

                //if (_curVisitVo.CurWeather == VISIT_WEATHER.Fine)
                //{
                //    //FlowText.ShowMessage("当前已为最佳探班天气！");
                //    FlowText.ShowMessage(I18NManager.Get("Visit_CurBestWeather"));
                //    return;
                //}

                SendMessage(new Message(MessageConst.MODULE_VISIT_SHOW_WEATHER_PANEL, Message.MessageReciverType.DEFAULT, _curVisitVo.NpcId));
            }
        });
        _loaddown = transform.GetButton("Loaddown");
        _loaddown.gameObject.Hide();
        _loaddown.onClick.AddListener(() =>
        {
            if (_curVisitVo != null)
            {
                int npcId = (int)_curVisitVo.NpcId;
                int size = CacheManager.GetNeedVisitStoryItemLoaddownSize(npcId);
                string NpcName = Util.GetPlayerName(_curVisitVo.NpcId);
                string content = I18NManager.Get("Download_ConfirmDownloadLoveStory2", size.ToString());
                PopupManager.ShowConfirmWindow(content).WindowActionCallback = evt =>
                {
                    if (evt == WindowEvent.Ok)
                    {
                        CacheManager.DownloadVisitStoryCache(npcId, (s) =>
                        {
                            _loaddown.gameObject.Hide();
                        });
                    }
                };

            }

        });

        PointerClickListener.Get(_loveStage.gameObject).onClick = go =>
        {
            //Debug.LogError("tips？！");
            PopupManager.ShowAlertWindow($"此探班章节剧情的甜蜜度\n<color='#AF7697'>{_stageStr}</color>");
            
        };

        //UIEventListener.Get(gameObject).onDrag =  OnDrag;
        //UIEventListener.Get(gameObject).onBeginDrag =  OnBeginDrag;
        //UIEventListener.Get(gameObject).onEndDrag = OnEndDrag;

    }

    public void UpdateResetLevelTime()
    {

    }

    private void Start()
    {
        RectTransform rect = transform.GetRectTransform("Scroll View");
        rect.offsetMax = new Vector2(0, ModuleManager.OffY);
    }

    float beginMovePosY = 0;
    float beginDragPosY = 0;
    float scaleX = 1;
    float scaleY = 1;
    private void OnBeginDrag(PointerEventData data)
    {
        //PosRangeY.x = (Screen.height - MapBg.GetHeight() * scaleY) * 0.5f;
        //PosRangeY.y = (MapBg.GetHeight() * scaleY - Screen.height) * 0.5f;
        beginMovePosY = MapBg.localPosition.y;
        beginDragPosY = data.position.y;
        Debug.Log(data.position);
        //scaleX = MapBg.GetComponent<RectTransform>().localScale.x;
        //scaleY = MapBg.GetComponent<RectTransform>().localScale.y;
        Debug.Log("scaleX  " + scaleX + " scaleY " + scaleY);
    }
    Vector2 PosRangeY;//x is min ,y is max;
    //Vector2 ScreenSize;
    private void OnDrag(PointerEventData data)
    {
        float newPosy = beginMovePosY + data.position.y - beginDragPosY;
        if (newPosy < PosRangeY.x || newPosy > PosRangeY.y)
        {
            return;
        }

        MapBg.localPosition = new Vector3(
            MapBg.localPosition.x,
            newPosy,
            MapBg.localPosition.z);
        //  Debug.Log(data.position);
    }

    private void OnEndDrag(PointerEventData data)
    {
        //  Debug.Log(data.position);
    }

    public List<Vector3> GetLevelItemPosition()
    {
        Transform tf = MapBg.transform;
        List<Vector3> pos = new List<Vector3>();
        for (int i = 0; i < tf.childCount; i++)
        {
            pos.Add(tf.GetChild(i).localPosition);
        }
        return pos;
    }

    public void SetData(VisitVo curVisitVo, VisitChapterVo visitChapterVoList, List<MapPos> mapPos)
    {
        //资源加载 
        int npcId = (int)curVisitVo.NpcId;
        bool isShowLoaddown = CacheManager.IsVisitStoryItemLoaddown(npcId);
        _loaddown.gameObject.SetActive(isShowLoaddown);

        _curVisitVo = curVisitVo;
        string pathPre = "Visit/Maps/" + visitChapterVoList.LevelList[0].ChapterBackdrop;
        Debug.LogError(pathPre);
        MapBg.transform.GetComponent<RawImage>().texture = ResourceManager.Load<Texture>(pathPre, ModuleConfig.MODULE_VISIT);

        for (int i = MapBg.childCount - 1; i >= 0; i--)
        {
            Destroy(MapBg.GetChild(i).gameObject);
        }

        SetLeftTime();
        _levelList = visitChapterVoList.LevelList;

        if (_levelList.Count > mapPos.Count)
        {
            Debug.LogError("Level's count is biger than map's");
        }

        Debug.Log("MaxVisitTime  " + curVisitVo.MaxVisitTime + "   CurVisitTime   " + curVisitVo.CurVisitTime + "LevelCount  " + _levelList.Count);
        for (int i = 0; i < _levelList.Count; i++)
        {
            var prefab = GetLevelItem(_levelList[i]);
            var _go = Instantiate(prefab) as GameObject;
            _go.gameObject.Hide();
            _go.transform.SetParent(MapBg, false);
            _go.transform.localScale = Vector3.one;
            _go.transform.localPosition = new Vector3((i % 2) * (-400) + 200, MapBg.GetHeight() * 0.5f - 400 - i * 150, 0);
            _go.transform.localPosition = mapPos[i].pos;
            if (!String.IsNullOrEmpty(_levelList[i].Sweetness))
            {
                SetSweetnessStage(_levelList[i].Sweetness);
            }
            _go.GetComponent<VisitLevelItem>().SetData(_levelList[i]);

            _go.gameObject.Show();
        }

    }

    public void UpdateFirstPassAward()
    {
        for (int i = MapBg.childCount - 1; i >= 0; i--)
        {
            MapBg.GetChild(i).GetComponent<VisitLevelItem>().UpdateItem();
        }
    }

    readonly string levelCardPath = "Visit/Prefabs/Item/VisitLevelCardItem";
    readonly string levelPropsPath = "Visit/Prefabs/Item/VisitLevelPropsItem";
    readonly string levelStoryPath = "Visit/Prefabs/Item/VisitLevelStoryItem";
    GameObject levelCard = null;
    GameObject propsCard = null;
    GameObject storyCard = null;
    private string GetLevelItemPath(VisitLevelVo vo)
    {
        if (vo.LevelType == LevelTypePB.Story)
        { return levelStoryPath; }

        if (vo.LevelAwardsType == ResourcePB.Puzzle)
            return levelCardPath;
        return levelPropsPath;
    }

    private GameObject GetLevelItem(VisitLevelVo vo)
    {
        string path = GetLevelItemPath(vo);
        GameObject res = null;
        if (path == levelCardPath)
        {
            if (levelCard == null)
            {
                levelCard = GetPrefab(path);
            }
            res = levelCard;
        }
        else if (path == levelStoryPath)
        {
            if (storyCard == null)
            {
                storyCard = GetPrefab(path);
            }
            res = storyCard;
        }
        else if (path == levelPropsPath)
        {
            if (propsCard == null)
            {
                propsCard = GetPrefab(path);
            }
            res = propsCard;
        }
        return res;
    }

    public void SetLeftTime()
    {
        //transform.Find("LeftTimes/Text").GetComponent<Text>().text = "关卡剩余次数：" + n;
        string text = I18NManager.Get("Visit_BattleIntroductionPopupTips",
        _curVisitVo.CurWeatherName, _curVisitVo.MaxVisitTime - _curVisitVo.CurVisitTime, _curVisitVo.MaxVisitTime);
        transform.Find("Weather/Button/Text").GetComponent<Text>().text = text;
        Image weather = transform.Find("Weather/Button").GetComponent<Image>();
        weather.sprite =
            AssetManager.Instance.GetSpriteAtlas("UIAtlas_Visit_levelWeather" + _curVisitVo.CurWeatherPB.WeatherId);
        weather.SetNativeSize();
        string _weatherTiptext;
        if (_curVisitVo.CurWeather==VISIT_WEATHER.Fine)
        {
            _weatherTiptext = I18NManager.Get("Visit_Level_WeatherTips2");
        }
        else
        {
            _weatherTiptext = I18NManager.Get("Visit_Level_WeatherTips1");
        }

        _weatherTip.text = _weatherTiptext;
    }

    private void SetSweetnessStage(string stage)
    {
        Debug.Log(stage);
        switch (stage)
        {
            case "0":
                _stageStr = "朋友";
                _loveStage.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Common_friendStage");
                break;
            case "1":
                _stageStr = "恋人未满";
                _loveStage.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Common_nearloverStage");
                break;
            case "2":
                _stageStr = "恋人";
                _loveStage.sprite = AssetManager.Instance.GetSpriteAtlas("UIAtlas_Common_loverStage");
                break;
            default:
                Debug.LogError("SweetNess:"+stage);
                break;
        }
		
		
		
    }
#if UNITY_EDITOR

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveToJson();
        }
    }

    private void SaveToJson()
    {
        Debug.Log("LevelPos  SaveToJson");
        var poss = GetLevelItemPosition();
        List<MapPos> mapPos = new List<MapPos>();
        for (int i = 0; i < poss.Count; i++)
        {
            MapPos pos = new MapPos();
            pos.index = i;
            pos.pos = poss[i];
            mapPos.Add(pos);
        }
        string json = JsonConvert.SerializeObject(mapPos);
        json = FileUtil.ConvertJsonString(json);
        //string path = AssetLoader.GetDiaryTemplateDataPath("Sb11d");
        FileUtil.SaveFileText("DiaryTemplate/Data", "Temp.json", json);

    }
#endif
}
