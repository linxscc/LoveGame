using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
//using Assets.Scripts.Module.Components.List.EasyObjectPool;
using game.main;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Module;
using HedgehogTeam.EasyTouch;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicRhythmView :View  {

    List<GameObject> _clickAreas;
    int _clickAreasCount = 3;

    //Dictionary<int, List<GameObject>> _musicRhythmItemsDir;
    List<GameObject> _musicRhythmItems;

    private void Awake()
    {
        _musicRhythmItems = new List<GameObject>();
        //_musicRhythmItemsDir = new Dictionary<int, List<GameObject>>();
        _clickAreas = new List<GameObject>();
        _isEnters = new bool[4] { false, false, false, false };
        for (int i=1;i<= _clickAreasCount;i++)
        {
            GameObject obj = transform.Find("ClickAreas/" + i.ToString()).gameObject;
            _clickAreas.Add(obj);
            SetAreasShow(obj);
            UIEventListener.Get(obj).onEnter = OnAreasEnter;
            UIEventListener.Get(obj).onExit = OnAreasExit;

            // UIEventListener.Get(obj).onDown = (pointerEventData) =>
            // {
            //     OnAreasDown(obj);
            // };
            //
            // UIEventListener.Get(obj).onUp = (pointerEventData) =>
            // {
            //     OnAreasUp(obj);
            // };
        }

        _pool = new List<GameObject>();
        _poolParent = transform.Find("Pools");

        _testText = transform.GetText("TestText");
        _testText.gameObject.Hide();
        _testBtn = transform.GetButton("ThreeTouchButton");
        _testBtn.gameObject.Hide();
        _testBtn.onClick.AddListener(TriggerThreeTouch);
        
    }

    private void TriggerThreeTouch()
    {
        for (int i = 0; i < 3; i++)
        {
            ExecuteEvents.Execute(_clickAreas[i].gameObject, new PointerEventData(EventSystem.current),
                ExecuteEvents.pointerClickHandler);
        }
    }

    /// <summary>
    /// 更新分数和连击次数
    /// </summary>
    /// <param name="score"></param>
    /// <param name="combo"></param>
    public void UpdateViewScoreAndCombo(int score,int combo)
    {
        transform.GetText("Combo/Text").text = "Combo " + combo.ToString();
        transform.GetText("Score/Text").text = score.ToString();
    }

    public void OnUpdate(float delay)
    {
        for (int i = 0; i < _musicRhythmItems.Count; i++)
        {
            GameObject v = _musicRhythmItems[i];
            v.GetComponent<MusicRhythmItem>().OnUpdate(delay);
        }
    }

    public void AddMusicRhythmItem(Tick tick)
    {
        if (tick.Way > _clickAreas.Count)
        {
            Debug.Log("MUSIC->AddMusicRhythmItem tick.Way "+ tick.Way);
            return;
        }
            
        Transform parent = _clickAreas[tick.Way - 1].transform;

        // GameObject obj = ResourceManager.Instance.GetObjectFromPool(itemPath, true, 1);
        GameObject obj = GetObjectFromPool();

        obj.GetComponent<MusicRhythmItem>().SetData(tick);
        RectTransform nextItem = obj.GetComponent<RectTransform>();
        nextItem.transform.SetParent(parent.transform.Find("Mask/Path"), false);
        nextItem.localPosition=new Vector3(0, 2000, 0);
        nextItem.gameObject.SetActive(true);
        _musicRhythmItems.Add(obj);
    }

    public void OnShutdown()
    {
        //Debug.LogError("OnShutdown");
        for (int i = _musicRhythmItems.Count-1; i >=0; i--)
        {
            GameObject v = _musicRhythmItems[i];
            v.Hide();
            _musicRhythmItems.Remove(v);
            ReturnObjectToPool(v);      
        }
    }

    List<GameObject> _pool;
    GameObject GetObjectFromPool()
    {
        if(_pool.Count<=0)
        {
            string itemPath = "MusicRhythm/Items/MusicRhythmItem";
            GameObject obj = InstantiatePrefab(itemPath, _poolParent);
            return obj;
        }
        GameObject go = _pool[0];
        _pool.Remove(go);
        return go;
    }

    Transform _poolParent;

    void ReturnObjectToPool(GameObject go)
    {
        _pool.Add(go);
        go.transform.SetParent(_poolParent);
        go.Hide();
    }

    private void LateUpdate()
    {
        // _testText.text = "點擊效果：" + _hitCount;
    }

    public void RemoveMusicRhythmItem(Tick tick)
    {
        for (int i = 0; i < _musicRhythmItems.Count; i++)
        {
            GameObject v = _musicRhythmItems[i];
            if (v.GetComponent<MusicRhythmItem>().EqualTick(tick))
            {
                v.Hide();
                _musicRhythmItems.Remove(v);
                ReturnObjectToPool(v);
                return;
            }
        }
    }


    private void SetAreasShow(GameObject go,bool b=false)
    {
        go.transform.Find("Image").gameObject.SetActive(b);
    }

    private void OnAreasUp(GameObject go)
    {
        //Debug.Log("OnAreasUp " + go.name);
        SetAreasShow(go, false);
        int way = int.Parse(go.name);
        CheckAreasUp(way);
    }

    private void OnAreasDown(GameObject go)
    {
        //   Debug.Log("OnAreasDown " + go.name);
        SetAreasShow(go, true);

        int way = int.Parse(go.name);
        CheckAreasDown(way);
    }

    /// <summary>
    /// 点击按下，处理item逻辑
    /// </summary>
    /// <param name="way"></param>
    /// <returns></returns>
    private void CheckAreasDown(int way)
    {
        for(int i=0;i< _musicRhythmItems.Count;i++)
        {
            _musicRhythmItems[i].GetComponent<MusicRhythmItem>().OnWayDown(way);
        }
    }

    private void CheckAreasUp(int way)
    {
        for (int i = 0; i < _musicRhythmItems.Count; i++)
        {
            _musicRhythmItems[i].GetComponent<MusicRhythmItem>().OnWayUp(way);
        }
    }

    bool[] _isEnters;

    private void OnAreasExit(GameObject go)
    {
        //Debug.Log("OnExit "+go.name);
        SetAreasShow(go, false);
        int idx = int.Parse(go.name);
        _isEnters[idx] = true;
        ///ReturnObjectToPool(go.gameObject);
    }

    private void OnAreasEnter(GameObject go)
    {
        //Debug.Log("OnEnter " + go.name);
        int idx = int.Parse(go.name);
        _isEnters[idx] = false;
    }


    Transform tickResult;
    private Text _testText;
    private Button _testBtn;
    private RawImage _bg;

    public void PlayResult(MusicRhythmClickCallbackType musicRhythmClickCallbackType)
    {
        tickResult = transform.Find("TickResult");
        tickResult.gameObject.SetActive(false);
        Debug.Log(musicRhythmClickCallbackType);
        string res = "UIAtlas_MusicRhythm_" + musicRhythmClickCallbackType.ToString();
        // tickResult.GetImage("Image").sprite = UIAtlasController.Instance.LoadSprite(res);
        tickResult.GetImage("Image").sprite = AssetManager.Instance.GetSpriteAtlas(res); ;     
        tickResult.GetImage("Image").SetNativeSize();
        tickResult.gameObject.SetActive(true);
    }

    public void SetPercent(float per)
    {
        transform.Find("ProgressBar").GetComponent<MusicRhythmProgressBar>().SetPercent(per);
    }

    public void PlayShortOnce(Tick tick)
    {
        //  Debug.LogError(tick.Way);
        transform.Find("ClickAreas/" + tick.Way.ToString()).GetComponent<MusicRhythmClickAreasComment>().PlayShortOnce(tick);
    }
    public void PlayLongDown(Tick tick)
    {
        // Debug.LogError(tick.Way);
        transform.Find("ClickAreas/" + tick.Way.ToString()).GetComponent<MusicRhythmClickAreasComment>().PlayLongDown(tick);
    }
    public void PlayLongUp(Tick tick)
    {
        // Debug.LogError(tick.Way);
        transform.Find("ClickAreas/" + tick.Way.ToString()).GetComponent<MusicRhythmClickAreasComment>().PlayLongUp(tick);
    }


    public void SetData(int musicId, string musicName, string diffName)
    {
        transform.GetText("Name").text = musicName;
        transform.GetText("LevelBg/Level").text = diffName;

        _bg = transform.GetRawImage("Bg");
        _bg.texture = ResourceManager.Load<Texture>("TrainingRoom/background/" + musicId,
            ModuleConfig.MODULE_MUSICRHYTHM);
    }
   
    private void On_TouchStart(Gesture gesture)
    {
        GameObject currentGo = gesture.pickedUIElement;
        var parent = currentGo.transform.parent;
        var parentName = parent != null ? parent.name : "NONE";
    
        string evtName = "...";
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            evtName = EventSystem.current.currentSelectedGameObject.name;
        }
    
        Debug.LogWarning(
            "gesture.pickedUIElement=> " + currentGo.name + " parent:" + parentName + " evtName：" + evtName);
        
        
        for (int i = 0; i < _clickAreas.Count; i++)
        {
            if(currentGo == _clickAreas[i])
            {
                OnAreasDown(_clickAreas[i]);
                break;
            }
        }
        
    }
    private void On_TouchDown(Gesture gesture)
    {
        
    }
    private void On_TouchUp(Gesture gesture)
    {
        GameObject currentGo = gesture.pickedUIElement;
        for (int i = 0; i < _clickAreas.Count; i++)
        {
            if(currentGo == _clickAreas[i])
            {
                OnAreasUp(_clickAreas[i]);
                break;
            }
        }
    }
    private void OnEnable()
    {
        EasyTouch.On_TouchStart += On_TouchStart;       // 触摸开始
        EasyTouch.On_TouchDown += On_TouchDown;         // 触摸进行时
        EasyTouch.On_TouchUp += On_TouchUp;             // 触摸结束
    }
    private void OnDestroy()
    {
        EasyTouch.On_TouchStart -= On_TouchStart;       // 触摸开始
        EasyTouch.On_TouchDown -= On_TouchDown;         // 触摸进行时
        EasyTouch.On_TouchUp -= On_TouchUp; 
    }

    private void Update()
    {

        return;
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Time.timeScale += 1;
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Time.timeScale -= 1;
            if (Time.timeScale <= 1)
            {
                Time.timeScale = 0.5f;
            }
        }
        // return;
        // _testText.text = "Touch Count:" + Input.touchCount;
        bool uDown = Input.GetKeyDown(KeyCode.U);
        bool iDown = Input.GetKeyDown(KeyCode.I);
        bool oDown = Input.GetKeyDown(KeyCode.O);
        
        bool uUp = Input.GetKeyUp(KeyCode.U);
        bool iUp = Input.GetKeyUp(KeyCode.I);
        bool oUp = Input.GetKeyUp(KeyCode.O);

        if (uDown)
        {
            OnAreasDown(_clickAreas[0]);
        }
        if (iDown)
        {
            OnAreasDown(_clickAreas[1]);
        }
        if (oDown)
        {
            OnAreasDown(_clickAreas[2]);
        }

        if (uUp)
        {
            OnAreasUp(_clickAreas[0]);
        }
        if (iUp)
        {
            OnAreasUp(_clickAreas[1]);
        }
        if (oUp)
        {
            OnAreasUp(_clickAreas[2]);
        }
    }
}