using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Module;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Module.Framework.Utils;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using game.main;
using Com.Proto;
using Common;

public class ActivityCapsuleTemplateStoryItem : MonoBehaviour {


    private GameObject _openObj;
    private GameObject _lockObj;
    private RawImage _bg;
    private Button _btnBg;
    private Text _contentText;
    private Text _lockText;
    private GameObject _redPoint;

    ActivityCapsuleStoryRule _data;
    private bool _isClearPre = true;

    private void Awake()
    {
        _bg = transform.Find("Bg").GetComponent<RawImage>();
        _btnBg = transform.Find("Bg").GetComponent<Button>();
        _openObj = transform.Find("OpenObj").gameObject;
        _lockObj = transform.Find("LockObj").gameObject;

        _contentText = _openObj.transform.GetText("Content");
        _lockText = _lockObj.transform.GetText("Des");
        _redPoint = _openObj.transform.Find("RedPoint").gameObject;

        _btnBg.onClick.AddListener(OnBtnBgClick);
    }

    // Use this for initialization
    void Start () {
		
	}
	

    public void SetData(string id, ActivityCapsuleTemplateModel model, bool isClearPre)
    {
        _isClearPre = isClearPre;

        ActivityCapsuleStoryRule rule = model.GetStoryRule(id);
        _data = rule;
        if (_data == null) return;
        long curTime = ClientTimer.Instance.GetCurrentTimeStamp();
        //Debug.Log("openTime:"+ _data.data.OpenTime + " curTime:"+curTime);
        _contentText.text = "剧情"+rule.index;
        if (curTime < _data.data.OpenTime)
        {
            _btnBg.interactable = false;
            _openObj.Hide();
            _lockObj.Show();
            _bg.texture = null;
            _bg.color = new Color(0.9f, 0.9f, 0.9f, 1);
            _lockText.text = I18NManager.Get("ActivityCapsuleTemplate_storyOpenTips", DateUtil.GetYMDD(_data.data.OpenTime));
        }
        else
        {
            _btnBg.interactable = true;
            _bg.texture = ResourceManager.Load<Texture>("ActivityCapsuleTemplate/story_" + _data.index);
            _openObj.Show();
            _lockObj.Hide();
            
            if (!isClearPre)
            {
                _redPoint.Hide();
            }
            else
            {
                _redPoint.SetActive(!model.IsReadStory(id));
            }
        }
    }

    private void OnBtnBgClick()
    {
        if (_data == null || _data.data == null) return;
        //_data = new ActivityCapsulePlotRulePB();
        if (!_isClearPre)
        {
            FlowText.ShowMessage(I18NManager.Get("ActivityCapsuleTemplate_preStoryTips"));
            return;
        }

        PopupManager.CloseAllWindow();
        Debug.Log("id:"+_data.data.PlotId);

        EventDispatcher.TriggerEvent<string, System.Action>(EventConst.ActivityCapsuleTemplateWatchStory, _data.data.PlotId,EnterStory);
        //ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STORY, false, false, _data.data);
    }

    private void EnterStory()
    {
        if (_data == null || _data.data == null) return;
        EventDispatcher.TriggerEvent(EventConst.ActivityCapsuleTemplateEnterStory);
        ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STORY, false, false, _data.data);
    }
}
