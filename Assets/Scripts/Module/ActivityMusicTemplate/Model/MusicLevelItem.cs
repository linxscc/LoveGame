using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Framework.GalaSports.Core;
using Assets.Scripts.Framework.GalaSports.Service;
using Assets.Scripts.Module;
using game.main;
using UnityEngine;
using UnityEngine.UI;

public class MusicLevelItem : MonoBehaviour
{
    private Button _enterBtn;
    private Button _lockBtn;
    ActivityMusicVo _activityMusicVo;
    private void Awake()
    {
        _enterBtn = transform.GetButton("EnterBtn");
        _lockBtn =transform.GetButton("LockBtn");
        
        _enterBtn.onClick.AddListener(OnClickEnterBtn);
        _lockBtn.onClick.AddListener(OnClickLockBtn);
    }


    
    private void OnClickLockBtn()
    {
        FlowText.ShowMessage(I18NManager.Get("ActivityMusicTemplate_MusicTaskNoPassTitle"));
    }

    private void OnClickEnterBtn()
    {
        Debug.LogError("进入关卡");
        PopupManager.CloseAllWindow();
        ModuleManager.Instance.EnterModule(ModuleConfig.MODULE_MUSICRHYTHM, false, true, _activityMusicVo);
    }
 
    public void SetData(ActivityMusicVo vo, int id)
    {
        _activityMusicVo = vo;
        string level = "ActivityMusicTemplate/MusicLevel_" + id;
        string levelLock = "ActivityMusicTemplate/MusicLevelLock_" + id;
        _enterBtn.transform.GetRawImage("Icon").texture = ResourceManager.Load<Texture>(level,ModuleConfig.MODULE_ACTIVITYMUSICTEMPLATE,true);
        _lockBtn.transform.GetRawImage("Icon").texture =ResourceManager.Load<Texture>(levelLock,ModuleConfig.MODULE_ACTIVITYMUSICTEMPLATE,true);
        
        if (vo.IsOpen)
        {
            _enterBtn.gameObject.SetActive(true);
            _lockBtn.gameObject.SetActive(false);
        }
        else
        {
            _enterBtn.gameObject.SetActive(false);
            _lockBtn.gameObject.SetActive(true); 
        }
    }


    public void JumpTo()
    {
      var isOpen =  _activityMusicVo.IsOpen;
      if (isOpen)
      {         
         OnClickEnterBtn();
      }
      else
      {
          OnClickLockBtn();
      }
    }
}
