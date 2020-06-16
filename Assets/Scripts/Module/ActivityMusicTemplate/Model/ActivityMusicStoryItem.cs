using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Core.Events;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using Assets.Scripts.Module.Framework.Utils;
using Common;
using game.main;
using GalaAccount.Scripts.Framework.Utils;
using UnityEngine;
using UnityEngine.UI;

public class ActivityMusicStoryItem : MonoBehaviour
{
   
    private GameObject _openObj;
    private GameObject _lockObj;
    private RawImage _bg;
    private Button _btnBg;
    private Text _contentText;
    private Text _lockText;
    private GameObject _redPoint;

    private string _bgPath;

    private ActivityStoryVo _data;

    private int _index;
    
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

    private void OnBtnBgClick()
    {
        if (!_data.IsCanEnterStory)
        {
           FlowText.ShowMessage(I18NManager.Get("ActivityMusicTemplate_StoryTitle"));
           return;
        }
        
        PopupManager.CloseAllWindow();


        EnterStory();
    }


    private void EnterStory()
    {    
        ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_STORY, false, false, _data.Rule);
    }
    
    public void SetData(ActivityStoryVo vo,int index)
    {
        _index = index;
        _data = vo;
        InitBgPath(index);
        _contentText.text = "剧情"+index;       
        SetOpen();
       
     
        
    }

    private void SetOpen()
    {
        if (_data.IsOpen)
        {
            _btnBg.interactable = true;
            _openObj.Show();
            _lockObj.Hide();
            _bg.texture = ResourceManager.Load<Texture>(_bgPath);
            _redPoint.SetActive(!_data.IsPass);
            
        }
        else
        {
            _btnBg.interactable = false;
            _openObj.Hide();
            _lockObj.Show();
            _bg.texture = null;
            _bg.color = new Color(0.9f, 0.9f, 0.9f, 1); 
            _lockText.text = I18NManager.Get("ActivityCapsuleTemplate_storyOpenTips", DateUtil.GetYMDD(_data.OpenTime));
        }
    }
    
    

    private void InitBgPath(int index)
    {
        _bgPath = "ActivityCapsuleTemplate/story_"+index;
    }
    
   
}
